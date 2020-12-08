using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeScript : MonoBehaviour
{
    public GameManager manager;
    private float maxHP=8;
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            
            if(manager.GetLife() < maxHP)
            {
                manager.AddLife(1);
                Destroy(gameObject);
            }
            else
            {
                manager.AddCoins(20);
                Destroy(gameObject);

            }

        }
    }
}
