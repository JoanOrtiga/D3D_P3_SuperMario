using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoombaMachine : MonoBehaviour
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
    
    public Renderer[] droneRenderer { get; private set; }


    [Header("REFERENCES")]
    public MarioController player;
    public Transform eyes;

    //Animation
    public Animator animator { get; private set; }

        
    [Header("HEALTH")]
    public int maxHP = 1;
    public int currentHP { get; private set; }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        droneRenderer = GetComponentsInChildren<Renderer>();
        animator = GetComponent<Animator>();

        player = FindObjectOfType<MarioController>();
    }

    private void Start()
    {
        currentHP = maxHP;

        material = new Material(droneRenderer[0].material);

        stateMachine = new StateMachine<GoombaMachine>(this);
        stateMachine.ChangeState(GoombaPatrolState.Instance);

    }

    private void Update()
    {
        print(stateMachine.CurrentState());
        stateMachine.UpdateMachine();
    }

    public bool SeesPlayer()
    {
        Vector3 direction = (player.transform.position + Vector3.up * 1.5f) - eyes.position;
        float distanceToPlayer = direction.magnitude;
        direction /= distanceToPlayer;

        bool isOnCone = Vector3.Dot(transform.forward, direction) >= Mathf.Cos(coneAngle * Mathf.Deg2Rad * 0.5f);

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

 /*  public override void RestartObject()
    {
        base.RestartObject();


        for (int i = 0; i < droneRenderer.Length; i++)
        {
            droneRenderer[i].material.color = new Color(droneRenderer[i].material.color.r, droneRenderer[i].material.color.g, droneRenderer[i].material.color.b, 1);
        }

        currentHP = maxHP;
        HealthUpdate();

        foreach (Collider item in GetComponentsInChildren<Collider>())
        {
            item.enabled = true;
        }

        stateMachine.ChangeState(DroneIdleState.Instance);

    }*/
  
    public void RecieveDamage(int damage)
    {
  /*      currentHP -= damage;

        if (currentHP <= 0)
        {
            stateMachine.ChangeState(DroneDieState.Instance);
        }
        else
        {
            stateMachine.ChangeState(DroneHitState.Instance);
        }*/
    }
}
