using EasyUI.Popup;
using System;
using System.Net.Mail;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Lean.Gui;
using UnityEditor;
using SimpleFileBrowser;

public class EmailManagement : MonoBehaviour
{
    public Button emailButton;
    public Button addFile;
    public ScrollView files;
    [SerializeField] public LeanPulse notification;

    public class Email
    {
        public string username = "aayemailer@gmail.com";
        public string password = "fhdlbwbbcceyqugc";
        public string recipient;
        public string subject;
        public string message;
        public List<string> attachments = new List<string>();
    }

    // The current email instance
    Email email = new Email();
    private List<AttachmentClass> attachments = new List<AttachmentClass>();

    //Singleton pattern
    public static EmailManagement Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        emailButton = root.Q<Button>("sendButton");
        addFile = root.Q<Button>("filesButton");
        files = root.Q<ScrollView>("filesList");

        addFile.clicked += () => {
            var groupBoxTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/attachement.uxml");
            var attachement = groupBoxTemplate.CloneTree();

            // Get the label and button elements from the GroupBox
            var label = attachement.Q<Label>("atText");
            var button = attachement.Q<Button>("atButton");

            // Modify the label text
            label.text = "New Label Text";

            // Handle the button press to remove the GroupBox
            button.clicked += () =>
            {
                files.Remove(attachement);
            };

            // Add the modified GroupBox to the groupBoxContainer
            files.Add(attachement);
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendEmail()
    {
        // string fromEmail = PlayerPrefs.GetString("UserField");
        // string password = PlayerPrefs.GetString("passwordField");

        // Check if the recipient email is valid and not empty
        if (String.IsNullOrEmpty(email.recipient))
        {
            // Show an error message popup if the email is not valid
            Popup.Show("Error", "No recipient", "OK", PopupColor.Red);
            return;
        }

        try
        {
            // Create a MailMessage object
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(email.username);
            mail.To.Add(email.recipient);
            mail.Subject = email.subject;
            mail.Body = email.message;

            // Add attachments to the email if there are any
            if (email.attachments.Count >= 1)
            {
                for (int i = 0; i < email.attachments.Count; i++)
                {
                    mail.Attachments.Add(new System.Net.Mail.Attachment(email.attachments[i]));
                }
            }

            // Create a SmtpClient object and set the necessary properties
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(email.username, email.password);
            smtp.EnableSsl = true;

            // Send the email
            smtp.Send(mail);
            notification.Pulse();
            for (int i = 0; i < attachments.Count; i++)
            {
                if (attachments[i] != null)
                    Destroy(attachments[i].gameObject);
            }
            attachments.RemoveAll(s => s == null);

            // Show a success message popup if the email is sent successfully
            // Popup.Show("Success", "Your email was sent succesfully", "OK", PopupColor.Green, () => SceneManager.LoadScene("EmailSceneV1"));
        }
        catch (Exception e)
        {
            // Show an error message popup if there is an exception
            Popup.Show("Error", e.Message, "OK", PopupColor.Red);
        }
    }

    public void SaveToEmail(string input)
    {
        email.recipient = input;
    }

    // SaveSubject saves the user input as the email subject
    public void SaveSubject(string input)
    {
        email.subject = input;
    }

    // SaveBody saves the user input as the email message body
    public void SaveBody(string input)
    {
        email.message = input;
    }

    // addAttachment adds the user input as an attachment to the email
    public void addAttachment(string input)
    {
        if (String.IsNullOrEmpty(input.Trim()))
        {
            return;
        }
        email.attachments.Add(input);
        createAttachmentonList(input);
    }

    public void removeAttachment(string input)
    {
        email.attachments.RemoveAll(x => ((string)x) == input);
    }

    public void addAttachments(string[] input)
    {
        foreach (string item in input)
        {
            this.addAttachment(item);
        }
    }

    public void createAttachmentonList(string input)
    {
        var groupBoxTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/attachement.uxml");
        var attachement = groupBoxTemplate.CloneTree();

        // Get the label and button elements from the GroupBox
        var label = attachement.Q<Label>("atText");
        var button = attachement.Q<Button>("atButton");

        // Modify the label text
        label.text = "New Label Text";

        // Handle the button press to remove the GroupBox
        button.clicked += () =>
        {
            files.Remove(attachement);
        };

        // Add the modified GroupBox to the groupBoxContainer
        files.Add(attachement);
    }
}
