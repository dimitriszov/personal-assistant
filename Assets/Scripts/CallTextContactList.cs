using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
public class ContactData
{
    public List<Contact> contacts;
}

[System.Serializable]
public class Contact
{
    public string Name;
    public string PhoneNumber;
}

[System.Serializable]
public class CallTextContactList : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform parentTransform;

    public List<Contact> contactList = new List<Contact>();

    public void StartReadingContacts()
    {
        // Specify the file path where the JSON file is saved
        string filePath = Application.dataPath + "/contacts.json";

        if (File.Exists(filePath))
        {
            // Read the JSON file as a string
            string jsonText = File.ReadAllText(filePath);

            // Deserialize the JSON string into ContactData
            ContactData contactData = JsonUtility.FromJson<ContactData>(jsonText);

            // Access the contacts list
            List<Contact> contacts = contactData.contacts;

            // Clear the existing list before adding new contacts
            contactList.Clear();

            // Iterate through each contact and add it to the contactList
            foreach (Contact contact in contacts)
            {
                // Access the contact properties
                string name = contact.Name;
                string phoneNumber = contact.PhoneNumber;

                // Create a new Contact object
                Contact newContact = new Contact
                {
                    Name = name,
                    PhoneNumber = phoneNumber
                };

                // Add the new contact to the contactList
                contactList.Add(newContact);
            }

            // Create buttons from contacts
            CreateButtonsFromContacts();
        }
        else
        {
            Debug.LogError("JSON file not found at: " + filePath);
        }
    }

    private void CreateButtonsFromContacts()
    {
        // Clear the existing buttons
        foreach (Transform child in parentTransform)
        {
            Destroy(child.gameObject);
        }

        // Create buttons for each contact
        foreach (Contact contact in contactList)
        {
            // Instantiate a new button prefab
            GameObject buttonObject = Instantiate(buttonPrefab, parentTransform);

            // Get the Text component from the button prefab
            TextMeshProUGUI buttonText = buttonObject.GetComponentInChildren<TextMeshProUGUI>();

            // Set the contact information in the button's text field
            buttonText.text = "Name: " + contact.Name + "\nPhone Number: " + contact.PhoneNumber;
        }
    }
}
