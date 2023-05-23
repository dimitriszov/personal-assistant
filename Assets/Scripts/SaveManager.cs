using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class SaveManager : MonoBehaviour
{
    [SerializeField] public Text text;
    [SerializeField] public InputField input;
    [SerializeField] public GameObject Panel;

    public string savedTextKey = "SavedText";
    void Start()
    {
        string savedText = PlayerPrefs.GetString("SavedText");
        input.text = savedText;
    }



    public void saveSchedule()
    {
        string textToSave = input.text;
        PlayerPrefs.SetString("SavedText", textToSave);
        PlayerPrefs.Save();
        Debug.Log("Save");

        input.text = ""; // Clear the input field

    }
    public void LoadSchedule()
    {
        string savedText = PlayerPrefs.GetString(savedTextKey);
        text.text = savedText;
        Debug.Log("Load");
    }


    public void deleteSchedule()
    {

        PlayerPrefs.DeleteKey("SavedText");
        PlayerPrefs.Save();

        Debug.Log("Delete");
    }

    public void OnButtonClick()
    {
        string selectedDay = gameObject.name; // Assuming the day is set as the button's name
        CheckNoteForDay(selectedDay);
    }

    public void CheckNoteForDay(string day)
    {
        if (PlayerPrefs.HasKey(day))
        {
            Debug.Log("Note exists for " + day);
        }
        else
        {
            Debug.Log("No note found for " + day);
        }
    }
}

