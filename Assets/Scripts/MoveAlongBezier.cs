using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongBezier : MonoBehaviour
{
    [SerializeField] BezierCurve bezierCurve;
    [SerializeField] float turnSpeed;
    [Range(10, 10000)]
    [SerializeField] int pointsOnPath = 50;

    Vector3[] pathPoints;

    int currentIndex;

    MoveAlongManager moveManager; // reference to the MoveAlongManager script

    void Start()
    {
        int pointsPerLine = Mathf.CeilToInt(((float)pointsOnPath / bezierCurve.GetControlPointsCount()));
        pathPoints = bezierCurve.GetAllPointsOnCurve(pointsPerLine);

        transform.position = pathPoints[0];

        moveManager = FindObjectOfType<MoveAlongManager>(); // find the MoveAlongManager script in the scene
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
            transform.position = Vector3.MoveTowards(transform.position, pathPoints[currentIndex], moveManager.moveSpeed * Time.deltaTime); // use the moveSpeed variable from the MoveAlongManager script
        }
        else
        {
            currentIndex++;
        }

        
    }
}
