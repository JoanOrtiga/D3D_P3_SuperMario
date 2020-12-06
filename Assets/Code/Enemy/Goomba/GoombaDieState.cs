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

        foreach (Collider item in entity.GetComponentsInChildren<Collider>())
        {
            item.enabled = false;
        }
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
      /*  for (int i = 0; i < entity.droneRenderer.Length; i++)
        {
            entity.droneRenderer[i].material.color = new Color(entity.droneRenderer[i].material.color.r, entity.droneRenderer[i].material.color.g, entity.droneRenderer[i].material.color.b, graphValue);
        }

        if (entity.droneRenderer[0].material.color.a <= 0)
        {
            GameObject drop = CalculateDropChance(entity);

            if(drop!=null)
                GameObject.Instantiate(drop, entity.transform.position + Vector3.down * 2.3f, new Quaternion(), GameManager.instance.destroyObjects);

            entity.gameObject.SetActive(false);
        }*/
    }
}
