using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killer : MonoBehaviour
{
    private int damage = 10;
    public GameManager manager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.LoseLife(10);
        }
    }
}
