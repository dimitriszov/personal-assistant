using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



public class SaveManager : MonoBehaviour
{
    [SerializeField] public Text text;
    [SerializeField] public InputField input;
    [SerializeField] public GameObject Panel;
    private Dictionary<DateTime, List<string>> calendarData = null;


    public string savedTextKey = "SavedText";
    void Start()
    {
        string savedText = PlayerPrefs.GetString("SavedText");
        input.text = savedText;
    }

    void Awake()
    {
        LoadCalendarData();
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
    public void AddJob(DateTime selectedDate, string job)
    {
        if (calendarData.ContainsKey(selectedDate))
        {
            calendarData[selectedDate].Add(job);
        }
        else
        {
            calendarData[selectedDate] = new List<string> { job };
        }
    }
    public void SaveCalendarData()
    {
        string jsonData = JsonUtility.ToJson(calendarData);
        PlayerPrefs.SetString("CalendarData", jsonData);
        PlayerPrefs.Save();
    }
    public void LoadCalendarData()
    {
        if (PlayerPrefs.HasKey("CalendarData"))
        {
            string jsonData = PlayerPrefs.GetString("CalendarData");
            calendarData = JsonUtility.FromJson<Dictionary<DateTime, List<string>>>(jsonData);
        }
        else
        {
            calendarData = new Dictionary<DateTime, List<string>>();
        }
    }

}

