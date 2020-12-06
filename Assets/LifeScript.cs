using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeScript : MonoBehaviour
{
    public GameManager Manager;
    private float maxHP=8;
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            
            if(Manager.GetLife() < maxHP)
            {
                Manager.AddLife(1);
                Destroy(gameObject);
            }
            
        }
    }
}
