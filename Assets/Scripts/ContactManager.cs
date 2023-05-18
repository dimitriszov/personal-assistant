using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ContactManager : MonoBehaviour
{

    [SerializeField] public List<Contact> contacts;
    [SerializeField] public InputField contactInputField;
    // Start is called before the first frame update

   


    private void Start()
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
