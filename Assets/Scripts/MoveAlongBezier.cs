using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongBezier : MonoBehaviour
{
    [SerializeField] BezierCurve bezierCurve;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float turnSpeed = 10f;
    [SerializeField] float accelerationPerSecound = 5f;
    [Range(10, 100)]
    [SerializeField] int pointsOnPath = 50;

    Vector3[] pathPoints;

    int currentIndex;

    void Start()
    {
        int pointsPerLine = Mathf.CeilToInt(((float)pointsOnPath / bezierCurve.GetControlPointsCount()));
        pathPoints = bezierCurve.GetAllPointsOnCurve(pointsPerLine);

        transform.position = pathPoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (currentIndex == pathPoints.Length - 1)
            currentIndex = 0;

        if (transform.position != pathPoints[currentIndex])
        {
            Vector3 direction = pathPoints[currentIndex] - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, pathPoints[currentIndex], moveSpeed * Time.deltaTime);
        }
        else
        {
            currentIndex++;
        }

        if(Input.GetMouseButtonDown(0))
        {
            
        }
    }
    IEnumerator SpeedDuration()
    {
        yield return new WaitForSeconds(accelerationPerSecound);
    }
}
