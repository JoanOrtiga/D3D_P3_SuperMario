using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoombaMachine : MonoBehaviour , IRestartGameElement
{
    private StateMachine<GoombaMachine> stateMachine;
    public StateMachine<GoombaMachine> pStateMachine
    {
        get { return stateMachine; }
    }

    private NavMeshAgent navMeshAgent;
    public NavMeshAgent pNavMeshAgent
    {
        get { return navMeshAgent; }
    }

    public float timer { get; set; }

    [Header("ATTACK")]
    public float minDistanceToAttack = 3.0f;
    public float maxDistanceToAttack = 7.0f;
    public float rotationAttackLerp = 0.05f;
    public int attackDamage = 1;
    public float attackCooldown = 1f;

    [Header("PATROL")]
    public List<Transform> waypoints;
    [HideInInspector] public int currentWaypointID = 0;

    [Header("CHASE")]
    public float maxDistanceToRaycast = 15f;
    public float coneAngle = 60f;
    public LayerMask sightLayerMask;

    [Header("DIE")]
    [Tooltip("y must be between 1 and 0")]
    public AnimationCurve fadeOut;

    [HideInInspector] public Material material;
    public Renderer[] goombaRenderer { get; private set; }


    [Header("REFERENCES")]
    public MarioController player;
    public Transform eyes;
    public GameManager gameManager { get; private set; }

    //Animation
    public Animator animator { get; private set; }


    [Header("HEALTH")]
    public int maxHP = 1;
    public int currentHP { get; private set; }


    private Vector3 startPosition;
    private Quaternion startRotation;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        goombaRenderer = GetComponentsInChildren<Renderer>(true);

        animator = GetComponent<Animator>();

        player = FindObjectOfType<MarioController>();

        gameManager = FindObjectOfType<GameManager>();

    }

    private void Start()
    {
        SetRestartPoint();

        currentHP = maxHP;

        material = new Material(goombaRenderer[0].material);

        stateMachine = new StateMachine<GoombaMachine>(this);
        stateMachine.ChangeState(GoombaPatrolState.Instance);
    }

    private void Update()
    {
        stateMachine.UpdateMachine();
    }

    public bool SeesPlayer()
    {
        if (player.dead)
            return false;

        Vector3 direction = (player.transform.position + Vector3.up * 1.5f) - eyes.position;

        bool isOnCone = Vector3.Angle(transform.forward, direction.normalized) < coneAngle;

        if (isOnCone && !Physics.Linecast(eyes.position, player.transform.position + Vector3.up * 1.5f, sightLayerMask.value))
        {
            return true;
        }

        return false;
    }

    public bool IsInAttackDistance()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        return distanceToPlayer < maxDistanceToAttack;
    }

    public void SetRestartPoint()
    {
        GameObject.FindObjectOfType<GameManager>().AddRestartGameElement(this);
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void Restart()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        currentHP = maxHP;

        for (int i = 0; i < goombaRenderer.Length; i++)
        {
            goombaRenderer[i].material.color = new Color(goombaRenderer[i].material.color.r, goombaRenderer[i].material.color.g, goombaRenderer[i].material.color.b, 1);
        }

        foreach (Collider item in GetComponentsInChildren<Collider>())
        {
            item.enabled = true;
        }

        gameObject.SetActive(true);

        currentWaypointID = 0;

        stateMachine.ChangeState(GoombaPatrolState.Instance);
    }

    public void RecieveDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            stateMachine.ChangeState(GoombaDieState.Instance);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RecieveDamage(1);
        }
    }
}
