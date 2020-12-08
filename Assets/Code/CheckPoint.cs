using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Transform startPosition;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<MarioController>().SetCheckPoint(startPosition.position, startPosition.rotation);

            gameObject.SetActive(false);
        }
    }
}
