using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongBezier : MonoBehaviour
{
    [SerializeField] BezierCurve bezierCurve;
    [SerializeField] float moveSpeed;
    public float boostSpeed;
    [SerializeField] float turnSpeed;
    [Range(10, 10000)]
    [SerializeField] int pointsOnPath = 50;

    Vector3[] pathPoints;

    int currentIndex;

    void Start()
    {
        int pointsPerLine = Mathf.CeilToInt(((float)pointsOnPath / bezierCurve.GetControlPointsCount()));
        pathPoints = bezierCurve.GetAllPointsOnCurve(pointsPerLine);

        transform.position = pathPoints[0];
        InvokeRepeating("increaseSpeed", 0f, 0.5f);


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
        if (moveSpeed >= 20f)
        {
            CancelInvoke("increaseSpeed");
        }
        if (Input.GetMouseButtonDown(0))
        {
            moveSpeed += boostSpeed;
        }
        if (Input.GetMouseButtonDown(1))
        {
            moveSpeed -= boostSpeed;
        }
        
    }

    void increaseSpeed()
    {
        moveSpeed = moveSpeed + 1f;
    }
}
