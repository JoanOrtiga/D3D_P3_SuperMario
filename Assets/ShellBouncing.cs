using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellBouncing : MonoBehaviour
{
    public int numBounces = 3;
    private Vector3 reflection;

    private Rigidbody rb;
    private GameManager gameManager;

    private Vector3 movement;

    public float speed = 15.0f;

    private void OnEnable()
    {

        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();

    }

    private void FixedUpdate()
    {
        Vector3 vel = rb.velocity.normalized;

        rb.velocity = new Vector3(speed * vel.x, rb.velocity.y, speed * vel.z);

        if (numBounces <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (this.enabled)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<GoombaMachine>().RecieveDamage(1);
            }
            else if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<MarioController>().LoseHP(1, transform.forward);
            }

            for (int i = 0; i < collision.contactCount; i++)
            {
                ContactPoint contact = collision.GetContact(i);

                if (contact.normal.x != 0 || contact.normal.z != 0)
                {
                    Bounce(contact);
                }
            }
        }

    }

    private void Bounce(ContactPoint contact)
    {
        if (numBounces > 0)
        {
            Vector3 vel = new Vector3(rb.velocity.normalized.x, 0, rb.velocity.normalized.z);

            rb.velocity = (Vector3.Reflect(vel, contact.normal) * speed) + new Vector3(0, rb.velocity.y, 0);

            numBounces -= 1;
        }
    }
}
