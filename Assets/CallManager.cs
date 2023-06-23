using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CallManager : MonoBehaviour
{
    // Helper class to wrap the list of contacts
    [System.Serializable]
    public class ContactList
    {
        public List<Contact> contactsList;
        public ContactList(List<Contact> contacts)
        {
            this.contactsList = contacts;
        }

        public List<Contact> getContacts()
        {
            return this.contactsList;
        }
    }

    public class Contact
    {
        public string Name;
        public string PhoneNumber;
    }

    private List<Contact> contacts;

    void Awake()
    {
        ReadContactsFromJson();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ReadContactsFromJson()
    {
        // Specify the file path where the JSON file is located
        string filePath = Application.dataPath + "/contacts.json";

        // Read the JSON file as a string
        string json = File.ReadAllText(filePath);

        // Convert the JSON string to a ContactList object
        ContactList contactList = JsonUtility.FromJson<ContactList>(json);

        // Assign the contacts from the ContactList to the list in the ContactManager
        contacts = contactList.getContacts();
    }
}

