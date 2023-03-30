using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierControlPoint : MonoBehaviour
{
    [SerializeField] Transform point1;
    [SerializeField] Transform point2;

    void OnDrawGizmosSelected()
    {
        if (point1 == null || point2 == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, point1.position);
        Gizmos.DrawLine(transform.position, point2.position);
    }

    public Vector3 GetMainPointPosition()
    {
        return transform.position;
    }

    public Vector3 GetControlPoint1Position()
    {
        return point1.position;
    }

    public Vector3 GetControlPoint2Position()
    {
        return point2.position;
    }
}
