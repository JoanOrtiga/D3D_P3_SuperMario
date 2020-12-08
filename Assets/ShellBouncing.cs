using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellBouncing : MonoBehaviour
{
    public int numBounces = 3;
    private Vector3 reflection;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact;

        print(collision.GetContact(0).normal);

        for (int i = 0; i < collision.contactCount; i++)
        {
            collision.GetContact(i);
        }

        if(numBounces > 0)
        {
            contact = collision.GetContact(0);
            float dot = Vector3.Dot(contact.normal, (-transform.forward));
            dot *= 2;
            reflection = contact.normal * dot;
            reflection = reflection + transform.forward;
            rb.velocity = rb.transform.TransformDirection(reflection.normalized * 15.0f);
            numBounces -= 1;
        }
    }
}
