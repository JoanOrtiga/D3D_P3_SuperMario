using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioController : MonoBehaviour , IRestartGameElement
{
    public enum TPunchType
    {
        leftPunch,
        rightPunch,
        kick
    }

    [Header("Input")]
    public KeyCode leftKey = KeyCode.A;
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode punchKey = KeyCode.Mouse0;
    public KeyCode jumpKey = KeyCode.Space;

    public float movementSpeed = 7.0f;
    public float jumpSpeed = 10.0f;
    public float jumpSpeedOnKillEnemy = 6.0f;
    public float verticalSpeedToKill = 0.0f;

    public float lerpRotation = 0.1f;

    Animator animator;

    public CameraController cameraController;
    private CharacterController characterController;

    float verticalSpeed;

    [Header("Punch")]
    public GameObject leftPunchCollider;
    public GameObject rightPunchCollider;
    public GameObject kickCollider;

    float currentComboTime = 0.0f;
    public float comboTime = 1.2f;
    private int currentPunch = 0;
    private bool comboTimeStarted = false;


    //RESTART
    Vector3 startPosition;
    Quaternion startRotation;

    public int bridgeForce;
    public Rigidbody bridge;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        SetRestartPoint();
    }

    private void Update()
    {
        Vector3 right = cameraController.transform.right;
        right.y = 0.0f;
        right.Normalize();
        Vector3 forward = cameraController.transform.forward;
        forward.y = 0.0f;
        forward.Normalize();

        Vector3 movement = Vector3.zero;

        float speed = 0.0f;
        if (Input.GetKey(leftKey))
        {
            speed = 0.2f;
            movement = -right;
        }
        if (Input.GetKey(rightKey))
        {
            speed = 0.2f;
            movement = right;
        }         
        if (Input.GetKey(upKey))
        {
            speed = 0.2f;
            movement = movement + forward;
        }
        if (Input.GetKey(downKey))
        {
            speed = 0.2f;
            movement = movement - forward;
        }

        movement.Normalize();

        if (Input.GetKey(runKey) && speed == 0.2f)
            speed = 1.0f;

        if (Input.GetKeyDown(jumpKey))
        {
            verticalSpeed = jumpSpeed;
        }

        if (Input.GetKeyDown(punchKey) && animator.GetBool("Punch") == false)
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

        CollisionFlags collisionFlags = characterController.Move(movement);
        if((collisionFlags & CollisionFlags.Below) != 0)
        {
            verticalSpeed = 0.0f;
        }

        if (speed == 0)
            desiredRotation = transform.rotation;

        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, lerpRotation * Time.deltaTime);

        animator.SetFloat("Speed", speed, 0.2f, Time.deltaTime);

        UpdateComboTime();
    }

    public void Step(int side)
    {
       // Debug.Log(side);
    }

    public void UpdatePunch(TPunchType punchType, bool enabled)
    {
        GameObject punchCollider = null;

        if(punchType == TPunchType.leftPunch)
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
        if(currentComboTime > 0.0f && comboTimeStarted)
        {
            currentComboTime -= Time.deltaTime;
            if(currentComboTime <= 0.0f)
            {
                comboTimeStarted = false;
            }
        }
    }

    int CurrentComboPunch()
    {
        if(currentComboTime <= 0.0f)
        {
            currentPunch = 1;

            return 0;
        }
        else
        {
            int currentCombo = currentPunch;
            ++currentPunch;

            if(currentPunch >= 3)
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
        verticalSpeed = jumpSpeedOnKillEnemy;
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
    }

    public void SetCheckPoint(Vector3 position, Quaternion rotation)
    {
        startPosition = position;
        startRotation = rotation;
    }

    public void OnControllerColliderHit(ControllerColliderHit hit) 
    {
        bridge.AddForceAtPosition(-hit.normal * bridgeForce, hit.point);
    }

}
