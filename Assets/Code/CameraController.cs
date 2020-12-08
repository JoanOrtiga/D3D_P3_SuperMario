using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour , IRestartGameElement
{
    [Header("INVERTS")]
    public bool invertedX;
    public bool invertedY;
    
    [Header("REFERENCES")]
    public Transform lookAtPlayer;
    private MarioController marioController;

    float yaw = 0.0f;
    float pitch = 0.0f;

    public float distance = 8.0f;

    [Header("SENSIBILITY")]
    public float horizontalSpeed = 0.0f;
    public float verticalSpeed = 0.0f;

    [Header("MINMAX DISTANCE")]
    public float minDistance = 3.0f;
    public float maxDistance = 5.0f;

    [Header("RANGE")]
    public float minPitch;
    public float maxPitch;

    [Header("COLLISION")]
    public LayerMask collisionLayerMask;
    public float cameraCollisionOffset = 0.1f;

    public Transform cameraIdlePosition;

    //RESET
    Vector3 startPosition;
    Quaternion startRotation;

    private void Start()
    {


        marioController = lookAtPlayer.GetComponentInParent<MarioController>();

        SetRestartPoint();
    }

    private void Update()
    {

        Vector3 lookAt = lookAtPlayer.position;
        Vector3 directionToPlayer = lookAt - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        distanceToPlayer = Mathf.Clamp(distanceToPlayer, minDistance, maxDistance);
        directionToPlayer.y = 0.0f;
        directionToPlayer.Normalize();
        yaw = Mathf.Atan2(directionToPlayer.z, directionToPlayer.x);

        float horizontalInput = Input.GetAxis("Mouse X") + Input.GetAxis("CameraJoystickX");
        float verticalInput = Input.GetAxis("Mouse Y") + Input.GetAxis("CameraJoystickY");

        yaw += horizontalInput * (horizontalSpeed * Mathf.Rad2Deg) * Time.deltaTime * (invertedX ? 1 : -1);
        pitch += verticalInput * (verticalSpeed * Mathf.Rad2Deg) * Time.deltaTime * (invertedY ? 1 : -1);

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Vector3 direction = new Vector3(Mathf.Cos(yaw) * Mathf.Cos(pitch), Mathf.Sin(pitch), Mathf.Sin(yaw) * Mathf.Cos(pitch));
        Vector3 desiredPosition = lookAtPlayer.position - direction * distanceToPlayer;

        RaycastHit raycastHit;

        Ray ray = new Ray(lookAtPlayer.position, -direction);
        //if(Physics.Linecast(transform.position, lookAtPlayer.position, out raycastHit, collisionLayerMask.value))
        if (Physics.Raycast(ray, out raycastHit, distanceToPlayer, collisionLayerMask.value))
        {
            desiredPosition = raycastHit.point + direction * cameraCollisionOffset;
        }

        if (marioController.isIdle)
        {
            transform.position = Vector3.Lerp(transform.position, cameraIdlePosition.position, 0.05f);
            desiredPosition = transform.position;
            yaw = 0;
            pitch = 0; 
        }
        else
        {
            transform.position = desiredPosition;
        }

        transform.LookAt(lookAt);
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
        yaw = 0;
        pitch = 0;
    }
}
