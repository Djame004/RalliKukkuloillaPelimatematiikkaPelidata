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
    public float dbBestTime;
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

    private async void ReadWriteUserDetails(string username, float LapCount, float lapTime)
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
                userDetails = new UserDetails(username, 0, 9999999);
                string json = JsonUtility.ToJson(userDetails);
                string userId = auth.CurrentUser.UserId;

                await dbReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
                Debug.Log("New user added: " + username);
                dbBestTime = 9999999;
            }

            else
            {
                userDetails = new UserDetails(username, LapCount, lapTime);
                string json = JsonUtility.ToJson(userDetails);
                string userId = auth.CurrentUser.UserId;

                await dbReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
                
                dbBestTime = float.Parse(dataSnapshot.Child("BestLapTime").GetRawJsonValue().ToString().Substring(0, 4).Replace('.', ','));

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
                    string tempscore = child.Child("BestLapTime").GetRawJsonValue().ToString().Substring(0, 4);
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
