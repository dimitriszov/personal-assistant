using UnityEngine;

public class OpenGmailButton : MonoBehaviour
{
    public string gmailUrl = "https://mail.google.com/";

    public void OpenGmail()
    {
        Application.OpenURL(gmailUrl);
    }
}