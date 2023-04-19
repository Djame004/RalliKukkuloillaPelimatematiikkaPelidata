using UnityEngine;
using UnityEngine.UI;
using System; //<-- AGGREGATE EXCEPTION
using System.Threading.Tasks; //<-- TASK

public class UISignInUp : MonoBehaviour
{
    public RectTransform SignInView;
    public RectTransform SignUpView;

    public Button ButtonShowSignUpView;
    public Button ButtonBackToSignInView;

    /*** Sign up ***/
    public InputField InputFieldSignUpUsername;
    public InputField InputFieldSignUpPassword;
    public Button ButtonNewAccount;

    /*** Sign in ***/
    public InputField InputFieldSignInUsername;
    public InputField InputFieldSignInPassword;
    public Button ButtonSignIn;

    //Reference to FirebaseAuth instance
    private Firebase.Auth.FirebaseAuth auth;

    public bool SignInActive { get; private set; } = true;
    public void Awake()
    {
        //Set reference to FirebaseAuth instance
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        //Toggle sign up / sign in views with buttons
        ButtonShowSignUpView.onClick.AddListener(ToggleSignUpInView);
        ButtonBackToSignInView.onClick.AddListener(ToggleSignUpInView);

        //Handle new account button click
        ButtonNewAccount.onClick.AddListener(() => CreateUserEmailPassword(InputFieldSignUpUsername.text, InputFieldSignUpPassword.text));

        //Handle sign in button click
        ButtonSignIn.onClick.AddListener(() => SignInUserEmailPassword(InputFieldSignInUsername.text, InputFieldSignInPassword.text));

        //Handle enter key press for sign in
        InputFieldSignInUsername.onEndEdit.AddListener((value) =>
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SignInUserEmailPassword(InputFieldSignInUsername.text, InputFieldSignInPassword.text);
            }
        });

        InputFieldSignInPassword.onEndEdit.AddListener((value) =>
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SignInUserEmailPassword(InputFieldSignInUsername.text, InputFieldSignInPassword.text);
            }
        });
    }


    private void ToggleSignUpInView()
    {
        SignInActive = !SignInActive;
        SignInView.gameObject.SetActive(SignInActive);
        SignUpView.gameObject.SetActive(!SignInActive);
    }

    //CreateUserEmailPassword with ASYNC/AWAIT
    private async void CreateUserEmailPassword(string email, string password)
    {
        Task<Firebase.Auth.FirebaseUser> task = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        await task;
        //Waiting for task results...

        try
        {
            //which may be fine and with continue on this thread:
            Firebase.Auth.FirebaseUser user = task.Result;
            Debug.Log("USER CREATED: " + user.UserId);
        }
        catch (AggregateException ae)
        {
            //or there can be exception(s)
            Debug.LogError("CREATING NEW USER FAILED");
            foreach (var ex in ae.InnerExceptions)
                Debug.LogErrorFormat("{0}: {1}", ex.GetType().Name, ex.Message);
        }
    }

    private async void SignInUserEmailPassword(string email, string password)
    {
        Task<Firebase.Auth.FirebaseUser> task = auth.SignInWithEmailAndPasswordAsync(email, password);
            await task;

        Debug.Log("JEE!!");

        try
        {
            Firebase.Auth.FirebaseUser user = task.Result;
            Debug.Log("USER SIGNED IN: " + user.UserId);
        }
        catch(AggregateException ae)
        {
            foreach (var ex in ae.InnerExceptions)
                Debug.LogErrorFormat("{0}: {1}", ex.GetType().Name, ex.Message);
        }
    }
}