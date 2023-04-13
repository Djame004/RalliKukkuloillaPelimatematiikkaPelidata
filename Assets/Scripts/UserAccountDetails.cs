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

    private void Awake()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        UserInfo.OnUserAuthStateChanged += UserInfo_OnUserAuthStateChanged;
    }

    private void UserInfo_OnUserAuthStateChanged(bool isSignedIn)
    {
        if (isSignedIn)
            ReadWriteUserDetails("Pasi", 1234.5f, false, false);
    }

    private async void ReadWriteUserDetails(string username, float playtime, bool updateUsername, bool updateTotalPlaytime)
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
                        await dbReference.Child("users").Child(auth.CurrentUser.UserId).Child("TotalPlayTime").SetPriorityAsync(playtime);
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
                userDetails = new UserDetails(username, playtime, updateUsername, updateTotalPlaytime);
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
            Debug.Log(userDetails.UserName + " | playtime : " + userDetails.TotalPlayTime);
        }

        //DataSnapshot dataSnapshot = task.Result;
        //Debug.Log("dataa" + dataSnapshot.GetRawJsonValue());

        //UserDetails userDetails = new UserDetails(username, playtime);
        //string json = JsonUtility.ToJson(userDetails);
        //string userId = auth.CurrentUser.UserId;

        //dbReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
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
