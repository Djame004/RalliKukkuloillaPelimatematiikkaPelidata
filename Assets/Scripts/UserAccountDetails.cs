using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;
using System.Threading.Tasks;

public class UserAccountDetails : MonoBehaviour
{
    private DatabaseReference dbReference;
    private Firebase.Auth.FirebaseAuth auth;
    public LapCounter lapCounter;

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
            ReadWriteUserDetails(auth.CurrentUser.Email, lapCounter.Getlapcount(), false, false);
    }

    private async void ReadWriteUserDetails(string username, int lapCount, bool updateUsername, bool updateTotalPlaytime)
    {
        UserDetails userDetails = null;

        Task<DataSnapshot> task = FirebaseDatabase.DefaultInstance
            .GetReference("users/" + auth.CurrentUser.UserId + "/")
            .GetValueAsync();

        await task;

        try
        {
            DataSnapshot dataSnapshot = task.Result;

            if (dataSnapshot.Exists)
            {
                if (updateTotalPlaytime || updateUsername)
                {
                    if (updateUsername)
                    {
                        await dbReference.Child("users").Child(auth.CurrentUser.UserId).Child("UserName").SetPriorityAsync(username);
                    }
                    if (updateTotalPlaytime)
                    {
                        await dbReference.Child("users").Child(auth.CurrentUser.UserId).Child("BestLapCount").SetPriorityAsync(lapCount);
                    }
                }
                else
                {
                    userDetails = JsonUtility.FromJson<UserDetails>(dataSnapshot.GetRawJsonValue());
                    Debug.Log("Existing user: " + userDetails.UserName);
                }


            }

            else
            {
                userDetails = new UserDetails(username, FindObjectOfType<LapCounter>().Getlapcount(), updateUsername, updateTotalPlaytime);
                string json = JsonUtility.ToJson(userDetails);
                string userId = auth.CurrentUser.UserId;

                await dbReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
                Debug.Log("New user added: " + username);
            }
        }

        catch(AggregateException ae)
        {
            foreach(var e in ae.InnerExceptions)
            {
                Debug.LogError(e.Message);
            }
        }

        if(userDetails != null)
        {
            Debug.Log(userDetails.UserName + " | LapCounter : " + userDetails.TotalPlayTime);
        }

        //DataSnapshot dataSnapshot = task.Result;
        //Debug.Log("dataa" + dataSnapshot.GetRawJsonValue());

        //UserDetails userDetails = new UserDetails(username, playtime);
        //string json = JsonUtility.ToJson(userDetails);
        //string userId = auth.CurrentUser.UserId;

        //dbReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }

    public void UpdateLapCount(int lapCount)
    {
        ReadWriteUserDetails(auth.CurrentUser.Email, FindObjectOfType<LapCounter>().Getlapcount(), false, false);
    }

    public class UserDetails
    {
        public string UserName;
        public float TotalPlayTime;
        public bool UpdateTotalPlaytime;
        public bool UpdateUsername;

        public UserDetails(string username, float totalPlayTime, bool updateUsername, bool updateTotalPlaytime)
        {
            UserName = username;
            TotalPlayTime = totalPlayTime;
            UpdateTotalPlaytime = updateTotalPlaytime;
            UpdateUsername = updateUsername;
        }
            
        
    }
}
