using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    [SerializeField] BezierControlPoint[] points;
    [Range(1, 10000)]
    [SerializeField] int renderingCount = 10;
    [SerializeField] bool drawGizmos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3[] GetAllPointsOnCurve(int count)
    {
        List<Vector3> allPoints = new List<Vector3>();

        for (int i = 0; i < points.Length; i++)
        {
            //Get index for the points
            int curIndex = 0;
            int nextIndex = 0;
            if (i == points.Length - 1) //Last to first
            {
                curIndex = i;
                nextIndex = 0;
            }
            else //To next one
            {
                curIndex = i;
                nextIndex = i + 1;
            }

            //Get all points on one line (4)
            Vector3[] pointsOnOneLine = new Vector3[4];
            pointsOnOneLine[0] = points[curIndex].GetMainPointPosition();
            pointsOnOneLine[1] = points[curIndex].GetControlPoint1Position();
            pointsOnOneLine[2] = points[nextIndex].GetControlPoint2Position();
            pointsOnOneLine[3] = points[nextIndex].GetMainPointPosition();

            //Get points along the bezier curve
            for (int t = 0; t < count; t++)
            {
                float scaledT = (float)(t + 1) / count; //Scale t from range (1-10) to (0-1)

                Vector3 line1T = Vector3.Lerp(pointsOnOneLine[0], pointsOnOneLine[1], scaledT);
                Vector3 line2T = Vector3.Lerp(pointsOnOneLine[1], pointsOnOneLine[2], scaledT);
                Vector3 line3T = Vector3.Lerp(pointsOnOneLine[2], pointsOnOneLine[3], scaledT);

                Vector3 tLine1 = Vector3.Lerp(line1T, line2T, scaledT);
                Vector3 tLine2 = Vector3.Lerp(line2T, line3T, scaledT);

                Vector3 bezierPoint = Vector3.Lerp(tLine1, tLine2, scaledT);

                allPoints.Add(bezierPoint);
            }
        }

        return allPoints.ToArray();
    }

    public int GetControlPointsCount()
    {
        return points.Length;
    }

    void OnDrawGizmos()
    {
        if (!drawGizmos)
            return;

        if (points.Length < 3)
            return;

        Gizmos.color = Color.green;

        //Loop all points
        for (int i = 0; i < points.Length; i++)
        {
            int curIndex = 0;
            int nextIndex = 0;

            if (i == points.Length - 1) //Last to first
            {
                curIndex = i;
                nextIndex = 0;
            }
            else //To next one
            {
                curIndex = i;
                nextIndex = i + 1;
            }

            //Get all points on one line (4)
            Vector3[] pointsOnOneLine = new Vector3[4];
            pointsOnOneLine[0] = points[curIndex].GetMainPointPosition();
            pointsOnOneLine[1] = points[curIndex].GetControlPoint1Position();
            pointsOnOneLine[2] = points[nextIndex].GetControlPoint2Position();
            pointsOnOneLine[3] = points[nextIndex].GetMainPointPosition();

            //Get points along the bezier curve
            Vector3[] bezierPoinsOnLine = new Vector3[renderingCount];
            for (int t = 0; t < renderingCount; t++)
            {
                float scaledT = (float)(t + 1) / renderingCount; //Scale t from range (1-10) to (0-1)

                Vector3 line1T = Vector3.Lerp(pointsOnOneLine[0], pointsOnOneLine[1], scaledT);
                Vector3 line2T = Vector3.Lerp(pointsOnOneLine[1], pointsOnOneLine[2], scaledT);
                Vector3 line3T = Vector3.Lerp(pointsOnOneLine[2], pointsOnOneLine[3], scaledT);

                Vector3 tLine1 = Vector3.Lerp(line1T, line2T, scaledT);
                Vector3 tLine2 = Vector3.Lerp(line2T, line3T, scaledT);

                Vector3 bezierPoint = Vector3.Lerp(tLine1, tLine2, scaledT);
                bezierPoinsOnLine[t] = bezierPoint;
            }

            //Draw bezier curve
            for (int x = -1; x < renderingCount - 1; x++)
            {
                if (x < 0)
                {
                    Gizmos.DrawLine(pointsOnOneLine[0], bezierPoinsOnLine[0]);
                }
                else
                {
                    Gizmos.DrawLine(bezierPoinsOnLine[x], bezierPoinsOnLine[x + 1]);
                }
            }                
        }
    }
}
