using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
    }
    public void OnControllerColliderHit(ControllerColliderHit hit)
    {

        //print(hit.gameObject.name);
        this.GetComponent<Rigidbody>().AddForceAtPosition(-hit.normal, hit.point);

    }

}
