using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MarioController : MonoBehaviour, IRestartGameElement
{
    public enum TPunchType
    {
        leftPunch,
        rightPunch,
        kick
    }

    public enum TJumpCombo
    {
        noJump,
        jump,
        doubleJump,
        tripleJump
    }

    [Header("Input")]
    public KeyCode leftKey = KeyCode.A;
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode rightKey = KeyCode.D;

    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode runGamePad = KeyCode.JoystickButton8;
    public KeyCode punchKey = KeyCode.Mouse0;
    public KeyCode punchGamePad = KeyCode.JoystickButton5;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode jumpGamePad = KeyCode.JoystickButton0;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode crouchGamePad = KeyCode.JoystickButton1;

    public float movementSpeed = 7.0f;
    public float jumpSpeedOnKillEnemy = 6.0f;
    public float verticalSpeedToKill = 0.0f;

    public float lerpRotation = 0.1f;


    private CollisionFlags collisionFlags;

    Animator animator;

    public CameraController cameraController;
    private CharacterController characterController;

    float verticalSpeed;

    [Header("MOVEMENT")]
    public float walkingSpeed = 0.2f;
    public float runningSpeed = 1f;
    public float crouchingSpeed = 0.2f;
    private float speed = 0.0f;

    [Header("Punch")]
    public GameObject leftPunchCollider;
    public GameObject rightPunchCollider;
    public GameObject kickCollider;

    float currentComboTime = 0.0f;
    public float comboTime = 1.2f;
    private int currentPunch = 0;
    private bool comboTimeStarted = false;


    public float m_UpElevatorDot;
    private GameObject m_CurrentElevator;

    [Header("Jump")]
    public float jumpSpeed = 10.0f;
    private bool onGround;
    public float longJumpSpeed = 4f;
    private bool onWall;
    private float onWallTimer = 0.0f;
    private float onWallMaxTime = 0.1f;
    private bool wallJumpEnabled = true;
    private float fallingTime = 0.3f;
    private float fallingTimer = 0.0f;

    private TJumpCombo currentJump;
    private float currentJumpComboTime = 0.0f;
    public float jumpComboTime = 1f;

    private bool onEnemy = false;

    [Header("IDLE")]
    public bool isIdle = true;
    private float idleTimer;
    public float timeToIdle = 1.5f;
    private Vector3 lastPosition;

    [Header("Sounds")]
    public AudioClip singleJumpSound;
    public AudioClip doubleJumpSound;
    public AudioClip tripleJumpSound;
    public AudioClip step1;
    public AudioClip step2;
    public AudioClip punch1Sound;
    public AudioClip punch2Sound;
    public AudioClip punch3Sound;
    private AudioSource sound;

    public bool dead { get; private set; }

    //RESTART
    Vector3 startPosition;
    Quaternion startRotation;

    [Header("BRIDGE")]
    public int bridgeForce;
    public Rigidbody bridge;

    [Header("Particulas")]
    public GameObject runParticles;
    public GameObject walkParticles;
    public GameObject groundParticles;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        sound = gameObject.GetComponent<AudioSource>();
        SetRestartPoint();
        speed = 0.0f;
    }

    private void Update()
    {
        if (dead)
            return;

        /*  foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
          {
              if (Input.GetKeyDown(kcode))
                  Debug.Log("KeyCode down: " + kcode);
          }*/

        Vector3 right = cameraController.transform.right;
        right.y = 0.0f;
        right.Normalize();
        Vector3 forward = cameraController.transform.forward;
        forward.y = 0.0f;
        forward.Normalize();
        speed = 0.0f;
        Vector3 movement = Vector3.zero;

        bool haveMoved = false;


        if (Input.GetKey(leftKey) || Input.GetAxisRaw("MovementJoysticX") < -0.01)
        {
            speed = walkingSpeed;
            movement = -right;
            haveMoved = true;
        }
        if (Input.GetKey(rightKey) || Input.GetAxisRaw("MovementJoysticX") > 0.01)
        {
            speed = walkingSpeed;
            movement = right;
            haveMoved = true;
        }
        if (Input.GetKey(upKey) || Input.GetAxisRaw("MovementJoysticY") > 0.01)
        {
            speed = walkingSpeed;
            movement = movement + forward;
            haveMoved = true;
        }
        if (Input.GetKey(downKey) || Input.GetAxisRaw("MovementJoysticY") < -0.01)
        {
            speed = walkingSpeed;
            movement = movement - forward;
            haveMoved = true;
        }

        movement.Normalize();

        if ((Input.GetKey(runKey) || Input.GetKey(runGamePad)) && speed == walkingSpeed)
            speed = runningSpeed;

        if (((Input.GetKey(jumpKey) || Input.GetKey(jumpGamePad)) && onGround && !animator.GetBool("Crouch")) || onEnemy)
        {
            onEnemy = false;

            verticalSpeed = jumpSpeed;
            UpdateJumpComboState();
        }

        ///
        if ((Input.GetKey(crouchKey) || Input.GetKey(crouchGamePad)) && onGround)
        {
            animator.SetBool("Crouch", true);
        }
        else if ((Input.GetKeyUp(crouchKey) || Input.GetKeyUp(crouchGamePad)))
        {
            animator.SetBool("Crouch", false);
        }

        if ((Input.GetKey(crouchKey) || Input.GetKey(crouchGamePad)) && onGround && (Input.GetKey(jumpKey) || Input.GetKey(jumpGamePad)))
        {
            animator.SetBool("Crouch", true);
            animator.SetTrigger("Jump");
            verticalSpeed = longJumpSpeed;
        }

        if ((Input.GetKeyDown(punchKey) || Input.GetKeyDown(punchGamePad)) && animator.GetBool("Punch") == false)
        {
            animator.SetTrigger("Punch");
            animator.SetInteger("ComboPunch", CurrentComboPunch());
            currentComboTime = comboTime;
        }


        Quaternion desiredRotation = Quaternion.identity;
        if (movement != Vector3.zero)
        {
            desiredRotation = Quaternion.LookRotation(movement);
        }


        movement = movement * Time.deltaTime * movementSpeed * speed;

        verticalSpeed += Physics.gravity.y * Time.deltaTime;
        movement.y = verticalSpeed * Time.deltaTime;


        collisionFlags = characterController.Move(movement);



        if (!haveMoved)
        {
            idleTimer -= Time.deltaTime;

            if (idleTimer <= 0)
            {
                isIdle = true;
            }
        }
        else
        {
            idleTimer = timeToIdle;
            isIdle = false;
        }

        lastPosition = transform.position;

        if ((collisionFlags & CollisionFlags.Below) != 0)
        {
            verticalSpeed = -Physics.gravity.y * Time.deltaTime;
        }

        if (onWall)
        {
            animator.SetBool("onWall", true);

            onWallTimer -= Time.deltaTime;

            if ((Input.GetKey(jumpKey) || Input.GetKey(jumpGamePad)) && wallJumpEnabled == true)
            {
                verticalSpeed = jumpSpeed;
                UpdateJumpComboState();

                wallJumpEnabled = false;
            }
        }

        if (onWall && onWallTimer > 0)
        {
            if (!(Input.GetKey(jumpKey) || Input.GetKey(jumpGamePad)))
                verticalSpeed = 0;
        }
        else if (!onWall)
        {
            onWallTimer = onWallMaxTime;
            animator.SetBool("onWall", false);
        }



        if (speed == 0)
            desiredRotation = transform.rotation;

        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, lerpRotation * Time.deltaTime);

        animator.SetFloat("Speed", speed, 0.2f, Time.deltaTime);

        UpdateComboTime();

        if (onGround)
        {
            UpdateJumpComboTime();
            wallJumpEnabled = true;
            fallingTimer = fallingTime;
            animator.ResetTrigger("Falling");
        }
        else
        {
            fallingTimer -= Time.deltaTime;

            if(fallingTimer<= 0.0f)
            {
                animator.SetTrigger("Falling");
                runParticles.SetActive(false);
                walkParticles.SetActive(false);
                groundParticles.SetActive(false);

            }
        }
        if (speed == runningSpeed && fallingTimer>0)
        {
            runParticles.SetActive(true);
            walkParticles.SetActive(false);
            groundParticles.SetActive(false);
        }
        if(speed== walkingSpeed && fallingTimer > 0)
        {
            runParticles.SetActive(false);
            walkParticles.SetActive(true);
            groundParticles.SetActive(false);
        }
        if(speed==0 && fallingTimer > 0)
        {
            runParticles.SetActive(false);
            walkParticles.SetActive(false);
            groundParticles.SetActive(false);
        }




        UpdateElevator();

        GravityUpdate();
    }

    private void GravityUpdate()
    {
        onGround = (collisionFlags & CollisionFlags.CollidedBelow) != 0;

        onWall = (collisionFlags & CollisionFlags.CollidedSides) != 0;

        animator.SetBool("Grounded", onGround);

        if (onGround || ((collisionFlags & CollisionFlags.CollidedAbove) != 0 && verticalSpeed > 0.0f))
        {
            verticalSpeed = -Physics.gravity.y * Time.deltaTime;
        }
    }

    public void LaLaLand()
    {
        runParticles.SetActive(false);
        walkParticles.SetActive(false);
        groundParticles.SetActive(true);
    }

    public void UpdateJumpComboTime()
    {
        currentJumpComboTime -= Time.deltaTime;

        if (currentJumpComboTime < 0)
        {
            currentJump = TJumpCombo.noJump;
        }
    }

    public void UpdateJumpComboState()
    {

        switch (currentJump)
        {
            case TJumpCombo.noJump:
                currentJump = TJumpCombo.jump;
                currentJumpComboTime = jumpComboTime;
                break;
            case TJumpCombo.jump:
                currentJump = TJumpCombo.doubleJump;
                currentJumpComboTime = jumpComboTime;
                break;
            case TJumpCombo.doubleJump:
                currentJump = TJumpCombo.tripleJump;
                currentJumpComboTime = jumpComboTime;
                break;
            case TJumpCombo.tripleJump:
                currentJump = TJumpCombo.jump;
                currentJumpComboTime = jumpComboTime;
                break;
            default:
                break;
        }

        animator.SetTrigger("Jump");
        runParticles.SetActive(false);
        walkParticles.SetActive(false);
        groundParticles.SetActive(false);
        animator.SetInteger("JumpState", (int)currentJump);
    }

    public void UpdatePunch(TPunchType punchType, bool enabled)
    {
        GameObject punchCollider = null;

        if (punchType == TPunchType.leftPunch)
        {
            punchCollider = leftPunchCollider;
        }
        else if (punchType == TPunchType.rightPunch)
        {
            punchCollider = rightPunchCollider;
        }
        else if (punchType == TPunchType.kick)
        {
            punchCollider = kickCollider;
        }

        punchCollider.SetActive(enabled);
    }

    public void StartComboTime()
    {
        comboTimeStarted = true;
    }

    void UpdateComboTime()
    {
        if (currentComboTime > 0.0f && comboTimeStarted)
        {
            currentComboTime -= Time.deltaTime;
            if (currentComboTime <= 0.0f)
            {
                comboTimeStarted = false;
            }
        }
    }

    int CurrentComboPunch()
    {
        if (currentComboTime <= 0.0f)
        {
            currentPunch = 1;

            return 0;
        }
        else
        {
            int currentCombo = currentPunch;
            ++currentPunch;

            if (currentPunch >= 3)
            {
                currentPunch = 0;
            }

            return currentCombo;
        }
    }


    public bool CanKillWithFeet()
    {
        return verticalSpeed < verticalSpeedToKill;
    }

    public void JumpOverEnemy()
    {
        onEnemy = true;
    }

    public void SetRestartPoint()
    {
        GameObject.FindObjectOfType<GameManager>().AddRestartGameElement(this);

        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void Restart()
    {
        characterController.enabled = false;
        transform.position = startPosition;
        transform.rotation = startRotation;
        characterController.enabled = true;

        dead = false;

        animator.SetTrigger("Restart");
    }

    public void SetCheckPoint(Vector3 position, Quaternion rotation)
    {
        startPosition = position;
        startRotation = rotation;
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        bridge.AddForceAtPosition(-hit.normal * bridgeForce, hit.point);

        if (hit.collider.CompareTag("Enemy") && hit.normal.y > 0.1f)
        {
            hit.collider.GetComponent<GoombaMachine>().RecieveDamage(1);
            JumpOverEnemy();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("elevator"))
        {
            SetCurrentElevator(other.gameObject);
        }
    }

    void SetCurrentElevator(GameObject Elevator)
    {
        if (isOnElevator(Elevator.transform))
        {
            m_CurrentElevator = Elevator;
            transform.SetParent(Elevator.transform);
        }
    }

    bool isOnElevator(Transform ElevatorTransform)
    {
        return Vector3.Dot(ElevatorTransform.transform.forward, Vector3.up) > m_UpElevatorDot;
    }
    void UpdateElevator()
    {
        if (m_CurrentElevator != null)
        {
            if (!isOnElevator(m_CurrentElevator.transform))
            {
                DetachElevator();
            }
        }
    }

    void DetachElevator()
    {
        m_CurrentElevator = null;
        transform.SetParent(null);
        transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f);
    }

    private void Step(int side)
    {
        if (speed > 0.0f)
        {
            switch (side)
            {
                case -1:
                    sound.clip = step1;
                    sound.Play();
                    break;
                case 1:
                    sound.clip = step2;
                    sound.Play();
                    break;

                default:
                    break;
            }
        }
    }
    private void Punch(int punchType)
    {
        switch (punchType)
        {
            case 1:
                sound.clip = punch1Sound;
                sound.Play();
                break;
            case 2:
                sound.clip = punch2Sound;
                sound.Play();
                break;
            case 3:
                sound.clip = punch3Sound;
                sound.Play();
                break;
            default:
                break;
        }
    }
    private void Jump(int jumpType)
    {
        switch (jumpType)
        {
            case 1:
                sound.clip = singleJumpSound;
                sound.Play();
                break;

            case 2:
                sound.clip = doubleJumpSound;
                sound.Play();
                break;

            case 3:
                sound.clip = tripleJumpSound;
                sound.Play();
                break;

            default:
                break;
        }
    }

    public void HitAnimation(float currentHealth, Vector3 forward)
    {
        if (currentHealth <= 0)
        {
            animator.SetTrigger("Die");
            characterController.enabled = false;
            dead = true;
            return;
        }

        if (Vector3.Angle(forward, transform.forward) <= 90)
        {
            animator.SetTrigger("Hitted_Front");
        }
        else
        {
            animator.SetTrigger("Hitted_Back");
        }
    }
}
