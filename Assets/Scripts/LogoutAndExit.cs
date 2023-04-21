using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

public class LogoutAndExit : MonoBehaviour
{
    public Button logoutButton;

    void Start()
    {
        logoutButton.onClick.AddListener(LogoutAndExitApp);
    }

    void LogoutAndExitApp()
    {
        // Sign out the user
        FirebaseAuth.DefaultInstance.SignOut();
        Debug.Log("Signed Out");

        // Stop the game in the Editor
        UnityEditor.EditorApplication.isPlaying = false;
        Debug.Log("Stop play");

        // Exit the application
        Application.Quit();
        Debug.Log("App Quit");
    }
}
