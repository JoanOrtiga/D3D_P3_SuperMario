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

    private void Start()
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<GoombaMachine>().RecieveDamage(1);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.LoseLife(1);
            collision.gameObject.GetComponent<MarioController>().HitAnimation(gameManager.GetLife(), transform.forward);
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

    private void Bounce(ContactPoint contact)
    {
        if (numBounces > 0)
        {
            /*     float dot = Vector3.Dot(contact.normal, (-transform.forward));
                 dot *= 2;
                 reflection = contact.normal * dot;
                 reflection = reflection + transform.forward;
                 rb.velocity = rb.transform.TransformDirection(reflection.normalized * speed);
                 movement = rb.velocity;
                 */

            /*   Vector3 incomingVec = hit.point - gunObj.position;

               Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);*/


            rb.velocity = Vector3.Reflect(rb.velocity.normalized, contact.normal) * speed;
            //     movement = rb.velocity;

            numBounces -= 1;
        }
    }
}
