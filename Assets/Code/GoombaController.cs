using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaController : MonoBehaviour, IRestartGameElement
{
    public int life = 1;
    public int startLife = 1;

    //RESTART
    Vector3 startPosition;
    Quaternion startRotation;

    void Start()
    {
        SetRestartPoint();
    }

    public void SetRestartPoint()
    {
        GameObject.FindObjectOfType<GameManager>().AddRestartGameElement(this);
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void Restart()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        life = startLife;
        gameObject.SetActive(true);
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            MarioController marioController = hit.gameObject.GetComponent<MarioController>();

            if(marioController.CanKillWithFeet())
            {
                Die();
                marioController.JumpOverEnemy();
            }
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
}
