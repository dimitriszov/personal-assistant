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
    public int Position;
    public Contact(string name, string phoneNumber, int position)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        Position = position;
    }
}

[System.Serializable]
public class CallTextContactList : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform parentTransform;

    public List<Contact> contactList = new List<Contact>();

    public void StartReadingContacts()
    {

        string filePath = Application.dataPath + "/contacts.json";

        if (File.Exists(filePath))
        {

            string jsonText = File.ReadAllText(filePath);

            ContactData contactData = JsonUtility.FromJson<ContactData>(jsonText);

            List<Contact> contacts = contactData.contacts;

            contactList.Clear();

            for (int i = 0; i < contacts.Count; i++)
            {

                string name = contacts[i].Name;
                string phoneNumber = contacts[i].PhoneNumber;


                Contact newContact = new Contact(name, phoneNumber, i);


                contactList.Add(newContact);
            }


            CreateButtonsFromContacts();
        }
        else
        {
            Debug.LogError("JSON file not found at: " + filePath);
        }
    }


    private void CreateButtonsFromContacts()
    {

        foreach (Transform child in parentTransform)
        {
            Destroy(child.gameObject);
        }


        foreach (Contact contact in contactList)
        {

            GameObject buttonObject = Instantiate(buttonPrefab, parentTransform);


            TextMeshProUGUI buttonText = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI numberText = buttonObject.transform.Find("Number").GetComponent<TextMeshProUGUI>();


            buttonText.text = "Name: " + contact.Name + "\nPhone Number: " + contact.PhoneNumber;
            numberText.text = (contact.Position + 1).ToString();
        }
    }
}
