using UnityEngine;
using TMPro;

public class EnableOnFirebaseException : MonoBehaviour
{
    public TextMeshProUGUI errorMessageText;
    public float wait = 3f;

    void Start()
    {
        // Attach the FirebaseExceptionHandler method to the Application.logMessageReceived event
        Application.logMessageReceived += FirebaseExceptionHandler;
    }

    void OnDestroy()
    {
        // Detach the FirebaseExceptionHandler method from the Application.logMessageReceived event
        Application.logMessageReceived -= FirebaseExceptionHandler;
    }

    void FirebaseExceptionHandler(string logString, string stackTrace, LogType type)
    {
        // Check if the logString contains the FirebaseException message for an email format exception
        if (logString.Contains("FirebaseException: An email address must be provided.")
            || logString.Contains("FirebaseException: The email address is badly formatted."))
        {
            // Set the error message text to display the email format error message
            errorMessageText.text = "Please enter a valid email address";
            // Enable the errorMessageText
            errorMessageText.gameObject.SetActive(true);
            // Disable the errorMessageText after 5 seconds
            Invoke("DisableErrorMessageText", wait);
        }
        // Check if the logString contains the FirebaseException message for a no user record exception
        else if (logString.Contains("FirebaseException: There is no user record corresponding to this identifier."))
        {
            // Set the error message text to display the no user record error message
            errorMessageText.text = "There is no user with this email address";
            // Enable the errorMessageText
            errorMessageText.gameObject.SetActive(true);
            // Disable the errorMessageText after 5 seconds
            Invoke("DisableErrorMessageText", wait);
        }
        // Check if the logString contains the FirebaseException message for an invalid password exception
        else if (logString.Contains("FirebaseException: The given password is invalid.")
                 || logString.Contains("FirebaseException: A password must be provided."))
        {
            // Set the error message text to display the invalid password error message
            errorMessageText.text = "The given password is invalid";
            // Enable the errorMessageText
            errorMessageText.gameObject.SetActive(true);
            // Disable the errorMessageText after 5 seconds
            Invoke("DisableErrorMessageText", wait);
        }
        else if (logString.Contains("FirebaseException: The password is invalid or the user does not have a password.")
                 || logString.Contains("FirebaseException: The password is invalid or the user does not have a password."))
        {
            // Set the error message text to display the invalid password error message
            errorMessageText.text = "The given password is invalid";
            // Enable the errorMessageText
            errorMessageText.gameObject.SetActive(true);
            // Disable the errorMessageText after 5 seconds
            Invoke("DisableErrorMessageText", wait);
        }
    }
    
    void DisableErrorMessageText()
    {
        // Disable the errorMessageText
        errorMessageText.gameObject.SetActive(false);
    }
}
