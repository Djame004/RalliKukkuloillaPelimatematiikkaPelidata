using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    UserAccountDetails userAccountDetails;
    public GameObject[] leaderBoardEntries;
    public Text[] userName;
    public Text[] bestTime;
   
    void Start()
    {
        userAccountDetails = FindObjectOfType<UserAccountDetails>();
    }

    public void ToggleLeaderBoard()
    {
        
        for (int i = 0; i < userAccountDetails.leaderBoardList.Count; i++)
        {
           
            userName[i].text = userAccountDetails.leaderBoardList[i].Name;
            if(userAccountDetails.leaderBoardList[i].Time == 9999)
            {
                bestTime[i].text = "Waiting for DB";
            }
            else
            {
                bestTime[i].text = userAccountDetails.leaderBoardList[i].Time.ToString();
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        ToggleLeaderBoard();
    }
}
