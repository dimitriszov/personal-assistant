using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ContactManager : MonoBehaviour
{

    [SerializeField] public List<Contact> contacts;
    [SerializeField] public InputField contactInputField;

   
    public void Start()
    {
        contacts = new List<Contact>();
    }

    public List<Contact> SearchContactsByName(string name)
    {
        List<Contact> searchResults = new List<Contact>();
        foreach (Contact contact in contacts)
        {
            if (contact.Name.ToLower().Contains(name.ToLower()))
            {
                searchResults.Add(contact);
            }
        }
        return searchResults;
    }

    public void AddContact()
    {
        string contactInfo = contactInputField.text;
        Contact newContact = ParseContactInfo(contactInfo);
        if (newContact != null)
        {
            contacts.Add(newContact);
            SaveContacts();
        }
        contactInputField.text = string.Empty; // Clear the input field
    }

    public Contact ParseContactInfo(string contactInfo)
    {
        string[] contactData = contactInfo.Split(';'); // Assuming contact information is separated by semicolons
        if (contactData.Length >= 2) // Assuming contact information should contain at least name, and phone number
        {
            string name = contactData[0].Trim();
            string phoneNumber = contactData[2].Trim();

            // Create and return a new Contact object
            return new Contact(name, phoneNumber);
        }
        else
        {
            Debug.LogError("Invalid contact information: " + contactInfo);
            return null;
        }
    }

    public void SaveContacts()
    {
        // Convert the contacts list to JSON format
        string contactsData = JsonUtility.ToJson(contacts);

        // Save the contacts data to PlayerPrefs or your chosen storage mechanism
        PlayerPrefs.SetString("ContactsData", contactsData);
        PlayerPrefs.Save();
    }

    public void DeleteContact()
    {
        string contactInfo = contactInputField.text;
        Contact contactToDelete = FindContact(contactInfo);
        if (contactToDelete != null)
        {
            contacts.Remove(contactToDelete);
            // Save or update the contacts in the storage mechanism
            SaveContacts();
        }
        contactInputField.text = string.Empty;
    }

    public Contact FindContact(string contactInfo)
    {
        foreach (Contact contact in contacts)
        {
            if (contact.Name.ToLower() == contactInfo.ToLower() || contact.PhoneNumber == contactInfo)
            {
                return contact;
            }
        }
        return null; // Contact not found
    }

}
[System.Serializable]
public class Contact
    {
        public string Name;
        public string PhoneNumber;

        public Contact(string name, string phoneNumber)
        {
            Name = name;
            PhoneNumber = phoneNumber;
        }
    }
