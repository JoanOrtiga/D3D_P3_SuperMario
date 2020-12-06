using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneChaseState : State<GoombaMachine>
{
    public static DroneChaseState Instance { get; private set; }

    static DroneChaseState()
    {
        Instance = new DroneChaseState();
    }
    public override void Enter(GoombaMachine entity)
    {
        entity.timer = 0.0f;

        CalculateChasePosition(entity);

        entity.animator.SetBool("SeesPlayer", true);
    }

    public override void Execute(GoombaMachine entity)
    {
        CalculateChasePosition(entity);

        if (!entity.SeesPlayer())
        {
            entity.pStateMachine.ChangeState(GoombaPatrolState.Instance);
        }

     /*   if (entity.IsInAttackDistance())
        {
            entity.pStateMachine.ChangeState(GoombaAttackState.Instance);
        }*/
    }

    public override void Exit(GoombaMachine entity)
    {
        entity.animator.SetBool("SeesPlayer", false);
    }

    private void CalculateChasePosition(GoombaMachine entity)
    {
        Vector3 direction = entity.player.transform.position - entity.transform.position;
        float distanceToPlayer = direction.magnitude;
        float movementDistance = distanceToPlayer - entity.minDistanceToAttack;
        direction /= distanceToPlayer;
        Vector3 chasePosition = entity.transform.position + direction * movementDistance;

        entity.pNavMeshAgent.SetDestination(chasePosition);
        entity.pNavMeshAgent.isStopped = false;
    }
}
