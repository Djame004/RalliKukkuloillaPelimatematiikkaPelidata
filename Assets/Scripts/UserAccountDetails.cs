using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;
using System.Threading.Tasks;
using System.Linq;

public class UserAccountDetails : MonoBehaviour
{
    private DatabaseReference dbReference;
    private Firebase.Auth.FirebaseAuth auth;
    public LapCounter lapTimer;
    public List<LeaderBoardEntry> leaderBoardList = new List<LeaderBoardEntry>();

    GameObject userAccountDetails;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        

        if (userAccountDetails == null)
        {
            userAccountDetails = gameObject;
        }
        else Destroy(gameObject);

        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        UserInfo.OnUserAuthStateChanged += UserInfo_OnUserAuthStateChanged;

    }

    private void UserInfo_OnUserAuthStateChanged(bool isSignedIn)
    {
        Debug.Log(auth.CurrentUser.Email);
        if (isSignedIn)
            ReadWriteUserDetails(auth.CurrentUser.Email, 0, lapTimer.GetlapTime());
    }

    public async void ReadWriteUserDetails(string username, float LapCount, float lapTime)
    {
        UserDetails userDetails = null;

        Task<DataSnapshot> task = FirebaseDatabase.DefaultInstance
            .GetReference("users/" + auth.CurrentUser.UserId + "/")
            .GetValueAsync();



        await task;

        try
        {
            DataSnapshot dataSnapshot = task.Result;
            if (!dataSnapshot.Exists)
            {
                Debug.Log("User Has NO Data");
                userDetails = new UserDetails(username, 0, 9999999);
                string json = JsonUtility.ToJson(userDetails);
                string userId = auth.CurrentUser.UserId;

                await dbReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
                Debug.Log("New user added: " + username);
                lapTimer.dbBestTime = 9999999;
            }

            else
            {
                Debug.Log("User Has Data!");
                Debug.Log("Got time: " + float.Parse(dataSnapshot.Child("BestLapTime").GetRawJsonValue().ToString().Substring(0, 4).Replace('.', ',')));
                if(lapTimer.hasCompletedLap)
                {
                    userDetails = new UserDetails(username, LapCount, lapTime);
                }
               else
                {
                    userDetails = new UserDetails(username, LapCount, float.Parse(dataSnapshot.Child("BestLapTime").GetRawJsonValue().ToString().Substring(0, 4).Replace('.', ',')));
                }

                Debug.Log("Laptime: " + lapTime + " will be set to best lap time from data: " + float.Parse(dataSnapshot.Child("BestLapTime").GetRawJsonValue().ToString().Substring(0, 4).Replace('.', ',')));


                
                string json = JsonUtility.ToJson(userDetails);
                string userId = auth.CurrentUser.UserId;

                await dbReference.Child("users").Child(userId).SetRawJsonValueAsync(json);

                lapTimer.dbBestTime = float.Parse(dataSnapshot.Child("BestLapTime").GetRawJsonValue().ToString().Substring(0, 4).Replace('.', ','));

            }
        }
        catch (AggregateException ae)
        {
            foreach (var e in ae.InnerExceptions)
            {
                Debug.LogError(e.Message);
            }
        }

        if (userDetails != null)
        {
            Debug.Log("User details set to: " + lapTimer.dbBestTime);
            userDetails.BestLapTime = lapTimer.dbBestTime;
            Debug.Log(userDetails.UserName + " | BestLapTime : " + userDetails.BestLapTime);
        }
        GetLeaderBoards();
    }

    public void UpdateLapCountAndTime()
    {
        lapTimer = FindObjectOfType<LapCounter>();
        Debug.Log(lapTimer);
        ReadWriteUserDetails(auth.CurrentUser.Email, lapTimer.Getlapcount(), lapTimer.GetlapTime());
    }

    public class UserDetails
    {
        public string UserName;
        public float LapCount;
        public float BestLapTime;

        public UserDetails(string username, float lapCount, float bestLapTime)
        {
            UserName = username;
            LapCount = lapCount;
            BestLapTime = bestLapTime;
        }
    }

    private async void GetLeaderBoards()
    {

        Task<DataSnapshot> task = FirebaseDatabase.DefaultInstance
            .GetReference("users/")
            .GetValueAsync();

        await task;

        try
        {
            DataSnapshot dataSnapshot = task.Result;
            if (dataSnapshot.Exists)
            {
                //Luodaan uusi lista mihin tungetaan databasesta (users alta kaikki childit).
                List<LeaderBoardEntry> dataBaseList = new List<LeaderBoardEntry>();

                foreach (DataSnapshot child in dataSnapshot.Children)
                {
                    string name = child.Child("UserName").GetRawJsonValue().ToString();
                    string rawValue = child.Child("BestLapTime").GetRawJsonValue().ToString();
                    string tempscore = rawValue.Length >= 4 ? rawValue.Substring(0, 4) : rawValue;

                    float score = float.Parse(tempscore.Replace('.', ','));

                    LeaderBoardEntry entry = new LeaderBoardEntry(name, score);

                    dataBaseList.Add(entry);

                }
                //tyhjennetään lista vanhasta datasta.
                leaderBoardList.Clear();
                //asetetaan objektit listaan (10kpl) Time muutujan mukaan suurimmasta pienimpään.
                leaderBoardList = dataBaseList.OrderBy(o => o.Time).Take(10).ToList();

            }
        }
        catch (AggregateException ae)
        {
            foreach (var e in ae.InnerExceptions)
            {
                Debug.LogError(e.Message);
            }
        }
    }
    public class LeaderBoardEntry
    {
        public string Name;
        public float Time;
        public LeaderBoardEntry(string name, float time)
        {
            Name = name;
            Time = time;
        }
    }
}
