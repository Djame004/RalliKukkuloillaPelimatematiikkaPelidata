using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public Transform[] controlPoints;
    public int curveResolution = 10;
    public bool loop = false;
    public Color curveColor = Color.white;

    private void OnDrawGizmos()
    {
        if (controlPoints.Length < 2)
            return;

        // Set the color of the Gizmos
        Gizmos.color = curveColor;

        // Draw the curve
        for (int i = 0; i < curveResolution; i++)
        {
            float t1 = (float)i / curveResolution;
            float t2 = (float)(i + 1) / curveResolution;
            Vector3 p1 = CalculateBezierPoint(t1, controlPoints);
            Vector3 p2 = CalculateBezierPoint(t2, controlPoints);
            Gizmos.DrawLine(p1, p2);
        }

        // If loop is enabled, draw a line from the last point to the first point
        if (loop)
        {
            Vector3 p1 = CalculateBezierPoint(0f, controlPoints);
            Vector3 p2 = CalculateBezierPoint(1f, controlPoints);
            Gizmos.DrawLine(p2, p1);
        }
    }

    private Vector3 CalculateBezierPoint(float t, Transform[] controlPoints)
    {
        int n = controlPoints.Length - 1;
        Vector3 point = Vector3.zero;
        for (int i = 0; i <= n; i++)
        {
            float b = BinomialCoefficient(n, i) * Mathf.Pow(t, i) * Mathf.Pow(1 - t, n - i);
            point += b * controlPoints[i].position;
        }
        return point;
    }

    private int BinomialCoefficient(int n, int k)
    {
        if (k == 0 || k == n)
        {
            return 1;
        }
        int result = 1;
        for (int i = 1; i <= k; i++)
        {
            result *= n - k + i;
            result /= i;
        }
        return result;
    }
}
