using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaDieState : State<GoombaMachine>
{
    public static GoombaDieState Instance { get; private set; }
    static GoombaDieState()
    {
        Instance = new GoombaDieState();
    }


    public override void Enter(GoombaMachine entity)
    {
        entity.timer = 0.0f;

        entity.pNavMeshAgent.isStopped = true;

        foreach (Collider item in entity.GetComponentsInChildren<Collider>())
        {
            item.enabled = false;
        }

        entity.animator.SetTrigger("Die");
    }

    public override void Execute(GoombaMachine entity)
    {
        float graphValue = entity.fadeOut.Evaluate(entity.timer);
        entity.timer += Time.deltaTime;

        FadeOut(entity, graphValue);
    }

    public override void Exit(GoombaMachine entity)
    {

    }

    private void FadeOut(GoombaMachine entity, float graphValue)
    {
        for (int i = 0; i < entity.goombaRenderer.Length; i++)
        {
            entity.goombaRenderer[i].material.color = new Color(entity.goombaRenderer[i].material.color.r, entity.goombaRenderer[i].material.color.g, entity.goombaRenderer[i].material.color.b, graphValue);
        }

        if (entity.goombaRenderer[0].material.color.a <= 0)
        {
            GameObject.Instantiate(entity.drop, entity.transform.position, entity.transform.rotation);

            entity.gameObject.SetActive(false);
        }
    }
}
