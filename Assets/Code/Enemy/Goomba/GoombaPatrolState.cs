using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoombaPatrolState : State<GoombaMachine>
{
    public static GoombaPatrolState Instance { get; private set; }

    static GoombaPatrolState()
    {
        Instance = new GoombaPatrolState();
    }

    public override void Enter(GoombaMachine entity)
    {
        entity.timer = 0.0f;

        entity.pNavMeshAgent.isStopped = false;

        MoveToNextPatrolPosition(entity);

        entity.animator.SetTrigger("Patrol");
    }

    public override void Execute(GoombaMachine entity)
    {
        if (!entity.pNavMeshAgent.hasPath && entity.pNavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            UpdateWaypointID(entity);
            MoveToNextPatrolPosition(entity);
        }

        if (entity.SeesPlayer())
        {
          /*  if (entity.IsInAttackDistance())
                entity.pStateMachine.ChangeState(GoombaAttackState.Instance);
            else*/
                entity.pStateMachine.ChangeState(DroneChaseState.Instance);
        }
    }

    public override void Exit(GoombaMachine entity)
    {

    }

    private void MoveToNextPatrolPosition(GoombaMachine entity)
    {
        entity.pNavMeshAgent.SetDestination(entity.waypoints[entity.currentWaypointID].position);
    }

    private void UpdateWaypointID(GoombaMachine entity)
    {
        entity.currentWaypointID++;

        if (entity.currentWaypointID >= entity.waypoints.Count)
        {
            entity.currentWaypointID = 0;
        }
    }
}
