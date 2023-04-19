using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void LateUpdate()
    {
        Vector3 desiredDirection = (target.position + offset) - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(desiredDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, smoothSpeed);
    }
}
