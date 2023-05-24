using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ContactManager : MonoBehaviour
{

    [SerializeField] public List<Contact> contacts;
    [SerializeField] public InputField contactInputField;
    [SerializeField] public InputField contactInputFieldNumber;
    [SerializeField] public Text searchResultsText;

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
        if (contactInputField == null || contactInputFieldNumber == null)
        {
            Debug.LogError("InputField references are not assigned.");
            return;
        }
      
        string name = contactInputField.text;
        string phoneNumber = contactInputFieldNumber.text;
        Contact newContact = new Contact(name, phoneNumber);
        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(phoneNumber))
        {
            contacts.Add(newContact);
            SaveContacts();
            // Refresh the contact search list
            DisplaySearchResults("");
        }
        /* string contactInfo = contactInputField.text;
         string contactInfoN = contactInputFieldNumber.text;

       Contact newContact = ParseContactInfo(contactInfo);

        if (newContact != null)
        {
            contacts.Add(newContact);
            SaveContacts();
            // Refresh the contact search list
            DisplaySearchResults("");
        }
      */
        contactInputField.text = string.Empty; // Clear the input field
        contactInputFieldNumber.text = string.Empty; // Clear the input field
    }

    public Contact ParseContactInfo(string contactInfo)
    {
           string[] contactData = contactInfo.Split(';'); // Assuming contact information is separated by semicolons
        if (contactData.Length >= 2) // Assuming contact information should contain at least name, and phone number
        {
            string name = contactData[0].Trim();
            string phoneNumber = contactData[1].Trim();

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
        Debug.Log("Save");
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
            // Refresh the contact search list
            DisplaySearchResults("");
        }
        contactInputField.text = string.Empty;
        Debug.Log("Delete");
    }

    public Contact FindContact(string contactInfo)
    {
        foreach (Contact contact in contacts)
        {
            // if (contact.Name.ToLower() == contactInfo.ToLower()  ||ontact.PhoneNumber == contactInfo)

            if (contact.Name.ToLower() == contactInfo.ToLower() || contact.PhoneNumber == contactInfo)
            {
                return contact;
            }
        }
        return null; // Contact not found
    }


    public void DisplaySearchResults(string name)
    {

        if (searchResultsText == null)
        {
            Debug.LogError("Text reference is not assigned.");
            return;
        }
        List<Contact> searchResults = SearchContactsByName(name);

        if (string.IsNullOrEmpty(name))
        {
            // If the name is empty or null, display all contacts
            searchResults = contacts;
        }
        else
        {
            // Perform the search by name
            searchResults = SearchContactsByName(name);
        }

        // Clear the search results UI
        searchResultsText.text = string.Empty;
        // Clear the search results UI
        searchResultsText.text = string.Empty;

        // Display the search results
        foreach (Contact result in searchResults)
        {
            searchResultsText.text += result.Name + ": " + result.PhoneNumber + "\n";
        }
    }

}

