using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class LapCounter : MonoBehaviour
{
    public GameObject car;
    [Range(0.1f,5f)]
    public float colliderCheck = 4.1f;
    public float distFinish;
    public int lapCount = 0;
    private int currentLapCount = 0;
    bool lapCounter = false;
    public UserAccountDetails userAccountDetails;

    private void Start()
    {
        userAccountDetails = FindObjectOfType<UserAccountDetails>();
    }

    void Update()
    {   
        distFinish = Vector3.Distance(transform.position, car.transform.position);

        if (distFinish < colliderCheck)
        {
            lapCounter = true;
        }

        if (distFinish > colliderCheck && lapCounter)
        {
            currentLapCount++;
            userAccountDetails.UpdateLapCount(currentLapCount);
            lapCounter = false;
        }

        if (currentLapCount > lapCount)
        {
            lapCount = currentLapCount;
        }
        Debug.Log("Lap " + currentLapCount + " completed.");
    }

    public int Getlapcount()
    {
        return lapCount;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, colliderCheck);
    }
}
