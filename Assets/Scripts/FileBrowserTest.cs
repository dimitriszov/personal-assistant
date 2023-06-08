using UnityEngine;
using System.Collections;
using System.IO;
using SimpleFileBrowser;
using UnityEngine.UIElements;
using Lean.Gui;
using System.Collections.Generic;
using UnityEditor;
using EasyUI.Popup;
using System.Net.Mail;
using System.Net;
using System;

public class FileBrowserTest : MonoBehaviour
{
    public VisualElement mainPanel;
    public VisualElement loginPanel;
    public Button emailButton;
    public Button addFile;
    public Button loginButton;
    public ScrollView files;
    public TextField emailField;
    public TextField passwordField;
    public TextField toEmailField;
    public TextField subjectText;
    public TextField text;
    //[SerializeField] public LeanPulse notification;

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
    public static FileBrowserTest Instance;

    private void Awake()
    {
        Instance = this;
    }
    // Warning: paths returned by FileBrowser dialogs do not contain a trailing '\' character
    // Warning: FileBrowser can only show 1 dialog at a time

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        mainPanel = root.Q<VisualElement>("MainPanel");
        loginPanel = root.Q<VisualElement>("LoginPanel");

        emailButton = root.Q<Button>("sendButton");
        addFile = root.Q<Button>("filesButton");
        loginButton = root.Q<Button>("login");
        files = root.Q<ScrollView>("filesList");
        toEmailField = root.Q<TextField>("EmailInput");
        subjectText = root.Q<TextField>("SubjectInput");
        text = root.Q<TextField>("TextInput");
        passwordField = root.Q<TextField>("Pas");

        passwordField.RegisterValueChangedCallback((evt) =>
        {
            passwordField.isPasswordField = true;
            // Use the text as needed, e.g., store it in a variable, process it, etc.
            Debug.Log("Text entered: " + evt.newValue);
        });

        toEmailField.RegisterValueChangedCallback((evt) =>
        {
            SaveToEmail(evt.newValue);
            // Use the text as needed, e.g., store it in a variable, process it, etc.
            Debug.Log("Text entered: " + evt.newValue);
        });

        subjectText.RegisterValueChangedCallback((evt) => {
            SaveSubject(evt.newValue);
        });

        text.RegisterValueChangedCallback((evt) => { 
            SaveBody(evt.newValue);
        });

        addFile.clicked += showDialog;

        emailButton.clicked += SendEmail;

        loginButton.clicked += login;

        // Set filters (optional)
        // It is sufficient to set the filters just once (instead of each time before showing the file browser dialog), 
        // if all the dialogs will be using the same filters
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));

        // Set default filter that is selected when the dialog is shown (optional)
        // Returns true if the default filter is set successfully
        // In this case, set Images filter as the default filter
        //FileBrowser.SetDefaultFilter(".jpg");

        // Set excluded file extensions (optional) (by default, .lnk and .tmp extensions are excluded)
        // Note that when you use this function, .lnk and .tmp extensions will no longer be
        // excluded unless you explicitly add them as parameters to the function
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".exe");

        // Add a new quick link to the browser (optional) (returns true if quick link is added successfully)
        // It is sufficient to add a quick link just once
        // Name: Users
        // Path: C:\Users
        // Icon: default (folder icon)
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);
    }

    public void showDialog()
    {
        // Show a select folder dialog
        // onSuccess event: print the selected folder's path
        // onCancel event: print "Canceled"
        // Load file/folder: folder, Allow multiple selection: false
        // Initial path: default (Documents), Initial filename: empty
        // Title: "Select Folder", Submit button text: "Select"
        FileBrowser.ShowLoadDialog( ( paths ) => { addAttachments(paths); },
        						   () => {  },
        						   FileBrowser.PickMode.Files, true, null, null, "Select Files", "Select" );
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
            //notification.Pulse();
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
        return;
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
        label.text = input.Replace("\\", "/");

        // Handle the button press to remove the GroupBox
        button.clicked += () =>
        {
            files.Remove(attachement);
            removeAttachment(input);
        };

        // Get the groupBoxContainer VisualElement from the ScrollView
        VisualElement groupBoxContainer = files.contentContainer.Q<VisualElement>("unity-content-container");
        // Add the modified GroupBox to the groupBoxContainer
        groupBoxContainer.Add(attachement);
    }

    public void login()
    {
        loginPanel.style.display = DisplayStyle.None;
        mainPanel.style.display = DisplayStyle.Flex;
    }
}