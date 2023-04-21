using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class LapCounter : MonoBehaviour
{
    public GameObject car;
    [Range(0.1f, 5f)]
    public float colliderCheck = 4.1f;
    public float distFinish;
    public int lapCount = 0;
    private int currentLapCount = 0;
    bool lapCounter = false;
    public UserAccountDetails userAccountDetails;
    public GameObject[] particleEffects;
    public float CurrentLapTime = 0;
    public float BestLaptime = 9999;
  

    private void Start()
    {
        userAccountDetails = FindObjectOfType<UserAccountDetails>();
        userAccountDetails.UpdateLapCountAndTime();
    }

    void Update()
    {
        distFinish = Vector3.Distance(transform.position, car.transform.position);
        //Aseta lapCounter: true kun auto on colliderin sisällä
        if (distFinish < colliderCheck)
        {
            lapCounter = true;
        }
        //kun auto poistuu colliderista se lisää lapCounteriin yhden kierroksen
        if (distFinish > colliderCheck && lapCounter)
        {
            currentLapCount++;
            lapCounter = false;
            
            if (currentLapCount > lapCount)
            {
                lapCount = currentLapCount;
                userAccountDetails.UpdateLapCountAndTime();
                foreach (GameObject particleEffect in particleEffects)
                {
                    StartCoroutine(ParticleSpawner(particleEffect));
                }
            }
            BestLaptime = userAccountDetails.dbBestTime;
            if (CurrentLapTime < BestLaptime)
            {
                BestLaptime = CurrentLapTime;
                userAccountDetails.UpdateLapCountAndTime();
            }
            CurrentLapTime = 0;
        }

        CurrentLapTime += Time.deltaTime;
        

        //Debug.Log("Lap " + currentLapCount + " completed. Lap time: " + CurrentLapTime);


       
    }

    public int Getlapcount()
    {
        return lapCount;
    }
    public float GetlapTime()
    {
        return BestLaptime;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, colliderCheck);
    }
    IEnumerator ParticleSpawner(GameObject particleEffect)
    {
        particleEffect.SetActive(true);
        yield return new WaitForSeconds(5);
        particleEffect.SetActive(false);

    }
}
