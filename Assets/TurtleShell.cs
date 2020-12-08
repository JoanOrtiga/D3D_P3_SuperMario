using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleShell : MonoBehaviour
{
    public GameObject turtle;

    private void OnDisable()
    {
        Instantiate(turtle, transform.position, transform.rotation);
    }
}
