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
    public float CurrentLapTime = 9999999;
    public float BestLaptime = 9999999;


  

    public void Awake()
    {
        userAccountDetails = FindObjectOfType<UserAccountDetails>();
        userAccountDetails.UpdateLapCountAndTime();
        Debug.LogWarning("Aika: dbBestTime: " + userAccountDetails.dbBestTime);
        BestLaptime = userAccountDetails.dbBestTime;
        Debug.LogWarning("Aika: BestLapTime: " + BestLaptime);
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
                Debug.LogWarning("Aika: Perse Lap time" + BestLaptime + " ja " + CurrentLapTime);
                BestLaptime = CurrentLapTime;
                Debug.LogWarning("Aika: Perseen jälkee Lap time" + BestLaptime + " ja " + CurrentLapTime);

                userAccountDetails.UpdateLapCountAndTime();
                Debug.LogWarning("Aika: Account data Personal best time: " + userAccountDetails.dbBestTime);

                Debug.LogWarning("Aika: Personal Best: " + BestLaptime);
            }
            CurrentLapTime = 0;
        }

        // Update lap time continuously
        CurrentLapTime += Time.deltaTime;

        //Debug.Log("Lap " + currentLapCount + " completed. Lap time: " + CurrentLapTime);
    }

    public int Getlapcount()
    {
        return lapCount;
    }
    public float GetlapTime()
    {

        //BestLaptime = userAccountDetails.dbBestTime;
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
