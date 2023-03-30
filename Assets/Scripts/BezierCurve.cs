using System;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public Transform StartPoint;
    public Transform[] AnchorPoints;
    public Transform[] EndPoints;
    public int numPoints = 50;

    public Vector3[] _points;

    public void OnDrawGizmos()
    {
        int numPairs = Mathf.Min(AnchorPoints.Length, EndPoints.Length);
        _points = new Vector3[numPairs * numPoints];
        int currentPoint = 0;
        Vector3 lastPoint = StartPoint.position;

        for (int i = 0; i < numPairs; i++)
        {
            for (int j = 0; j < numPoints; j++)
            {
                float t = (float)j / (numPoints - 1);
                _points[currentPoint] = CalculateBezierPoint(t, lastPoint, AnchorPoints[i].position, EndPoints[i].position);
                currentPoint++;
            }

            lastPoint = _points[currentPoint - 1];
        }

        Gizmos.color = Color.white;
        for (int i = 0; i < _points.Length - 1; i++)
        {
            Gizmos.DrawLine(_points[i], _points[i + 1]);
        }
    }

    public Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }
    public Vector3 CalculateBezierPointDerivative(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = -2 * u * p0;
        p += 2 * (u - t) * p1;
        p += 2 * t * p2;

        return p.normalized;
    }


}
