using System.Net;
using System.Net.Mail;
using UnityEngine;
using EasyUI.Popup;
using System;
using System.Collections.Generic;
using Lean.Gui;

namespace MyEmail.Manager
{
    public class EmailManager : MonoBehaviour
    {
        [SerializeField] public Transform container;
        [SerializeField] public GameObject prefab;
        [SerializeField] public LeanPulse notification;

        // Email class defines the structure of an email
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
        public static EmailManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        // SendEmail sends an email using the SMTP protocol
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

        // SaveUser saves the user input as the email username in PlayerPrefs
        public void SaveUser(string input)
        {
            PlayerPrefs.SetString("UserField", input);
        }

        // SavePassword saves the user input as the email password in PlayerPrefs
        public void SavePassword(string input)
        {
            PlayerPrefs.SetString("passwordField", input);
        }

        // SaveToEmail saves the user input as the email recipient
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
            if(String.IsNullOrEmpty(input.Trim()))
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
            GameObject attachment = Instantiate(prefab, container);
            if(attachment.TryGetComponent<AttachmentClass>(out AttachmentClass item))
            {
                item.emailManager = this;
                item.changeText(input);
                attachments.Add(item);
            }
        }
    }
}