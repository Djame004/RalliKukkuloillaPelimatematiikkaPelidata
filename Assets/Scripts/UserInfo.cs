using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfo : MonoBehaviour
{
    public RectTransform InfoPanel;
    public Text userEmailText;
    public Button ButtonSignOut;

    private Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;
    // Start is called before the first frame update
    void Awake()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;

        ButtonSignOut.onClick.AddListener(()=> auth.SignOut());
    }

    // Update is called once per frame
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
                InfoPanel.gameObject.SetActive(false);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.Email);
                InfoPanel.gameObject.SetActive(true);
                userEmailText.text = user.Email;
            }
        }
    }
}
