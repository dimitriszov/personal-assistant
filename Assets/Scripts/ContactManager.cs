using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ContactManager : MonoBehaviour
{

    public List<Contact> contacts;
    [SerializeField] public InputField contactInputField;
    // Start is called before the first frame update

    [System.Serializable]
    public class Contact
    {
        public string Name;
        public string Email;
        public string PhoneNumber;

        public Contact(string name, string email, string phoneNumber)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
        }


    private void Start()
    {
        contacts = new List<Contact>();
    }

    // public void ContactList()

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

    public void  AddContact()
    {
        string contactInfo = contactInputField.text;
        Contact newContact = ParseContactInfo(contactInfo);
        if (newContact != null)
        {
            contacts.Add(newContact);
            // Save or update the contacts in the storage mechanism
            SaveContacts();
        }
        contactInputField.text = string.Empty; // Clear the input field
    }
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
        contactInputField.text = string.Empty; // Clear the input field
    }
    }

   // public void UpdateContact()
    private  void SaveContacts()
    {
        string contactsData = JsonUtility.ToJson(contacts);

       
        PlayerPrefs.SetString("ContactsData", contactsData);
        PlayerPrefs.Save();
    }

    // Function to parse the contact information and create a new Contact object
    private Contact ParseContactInfo(string contactInfo)
    {
        string[] contactData = contactInfo.Split(';'); // Assuming contact information is separated by semicolons
        if (contactData.Length >= 3) // Assuming contact information should contain at least name, email, and phone number
        {
            string name = contactData[0].Trim();
            string email = contactData[1].Trim();
            string phoneNumber = contactData[2].Trim();

            // Create and return a new Contact object
            return new Contact(name, email, phoneNumber);
        }
        else
        {
            Debug.LogError("Invalid contact information: " + contactInfo);
            return null;
        }
    }

    // Function to find a contact by number or name
    private Contact FindContact(string contactInfo)
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
