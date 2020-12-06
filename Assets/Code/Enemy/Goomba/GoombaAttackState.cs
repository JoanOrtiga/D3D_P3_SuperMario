using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaAttackState : State<GoombaMachine>
{
    public static GoombaAttackState Instance { get; private set; }

    static GoombaAttackState()
    {
        Instance = new GoombaAttackState();
    }


    public override void Enter(GoombaMachine entity)
    {
        entity.timer = 0.0f;
    // entity.animator.SetInteger(entity.animationState, 0);
    }

    public override void Execute(GoombaMachine entity)
    {
    /*    //DamagePlayer

        var lookPos = entity.player.transform.position - entity.transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        entity.transform.rotation = Quaternion.Slerp(entity.transform.rotation, rotation, entity.rotationAttackLerp);

        entity.timer -= Time.deltaTime;

        if(entity.transform.rotation.eulerAngles.y - rotation.eulerAngles.y >= -5f && entity.transform.rotation.eulerAngles.y - rotation.eulerAngles.y <= 5f)
        {
            entity.animator.SetInteger(entity.animationState, 0);

        }
        else if (entity.transform.rotation.eulerAngles.y - rotation.eulerAngles.y >= 0f)
        {
            entity.animator.SetInteger(entity.animationState, 2);
        }
        else
        {
            entity.animator.SetInteger(entity.animationState, 3);
        }

        if (entity.SeesPlayer())
        {
            if (!entity.IsInAttackDistance())
            {
                entity.pStateMachine.ChangeState(DroneChaseState.Instance);
            }

            if(entity.timer <= 0)
            {
                entity.player.GetComponent<FPS_CharacterController>().LoseHeal(entity.attackDamage);
                entity.timer = entity.attackCooldown;
            }
        }
        else
        {
            entity.pStateMachine.ChangeState(GoombaPatrolState.Instance);
        }*/
    }

    public override void Exit(GoombaMachine entity)
    {

    }
}
