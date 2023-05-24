using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



public class SaveManager : MonoBehaviour
{
    [SerializeField] public Text dateText;
    [SerializeField] public InputField input;
    [SerializeField] public GameObject Panel;
    public string savedTextKey = "SavedText";
    private string previousDateText = "Hello";

    private Dictionary<string, List<string>> calendarData = new Dictionary<string, List<string>>();



    void Start()
    {
        previousDateText = dateText.text;

        //string savedText = PlayerPrefs.GetString("SavedText");
        //input.text = savedText;
    }

    void Update()
    {
        if (dateText.text != previousDateText)
        {
            List<string> jobs = GetJobsForDate(dateText.text);
            UpdateInputField(jobs);

            previousDateText = dateText.text;
        }
    }
    private List<string> GetJobsForDate(string selectedDate)
    {
        if (calendarData.ContainsKey(selectedDate))
        {
            return calendarData[selectedDate];
        }
        else
        {
            return new List<string>();
        }
    }

    private void UpdateInputField(List<string> jobs)
    {
        input.text = string.Join("\n", jobs);
    }

    void Awake()
    {
        LoadCalendarData();
    }


    public void saveSchedule()
    {
        string textToSave = input.text;
        //PlayerPrefs.SetString("SavedText", textToSave);
        //PlayerPrefs.Save();
        Debug.Log("Save");

        string currentDate = dateText.text;
        AddJob(currentDate, textToSave);
        // input.text = ""; // Clear the input field

    }
    public void LoadSchedule()
    {
        string savedText = PlayerPrefs.GetString(savedTextKey);
        dateText.text = savedText;
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
    public void AddJob(string selectedDate, string job)
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
            calendarData = JsonUtility.FromJson<Dictionary<string, List<string>>>(jsonData);
        }
        else
        {
            calendarData = new Dictionary<string, List<string>>();
        }
    }


}

