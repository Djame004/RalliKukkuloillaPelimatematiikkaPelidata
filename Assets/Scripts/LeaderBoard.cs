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
    public GameObject leaderBoard;
    public bool isLeaderBoardToggled = false;
    // Start is called before the first frame update
    void Start()
    {
        userAccountDetails = FindObjectOfType<UserAccountDetails>();

    }

    public void ToggleLeaderBoard()
    {
        int count = 0;

        //Piilotetaan defaulttina kaikki entries
        foreach (GameObject entries in leaderBoardEntries)
        {
            entries.SetActive(false);
        }
        //t‰ytet‰‰n array
        for (int i = 0; i < userAccountDetails.leaderBoardList.Count; i++)
        {
            leaderBoardEntries[i].SetActive(true);
            //t‰ytet‰‰n lista arraylla.
            userName[i].text = userAccountDetails.leaderBoardList[i].Name;
            bestTime[i].text = userAccountDetails.leaderBoardList[i].Time.ToString();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isLeaderBoardToggled)
        {
            ToggleLeaderBoard();
            isLeaderBoardToggled = true;
            leaderBoard.SetActive(true);
        }
        else
        {
            //leaderBoard.SetActive(false);
            isLeaderBoardToggled = false;
        }
    }
}
