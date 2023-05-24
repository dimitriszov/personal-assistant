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
    public string savedTextKey = "SavedText";
    public string previousDateText = ""; 

    private Dictionary<DateTime, List<string>> calendarData = new Dictionary<DateTime, List<string>>();



    void Start()
    {

        previousDateText = text.text;
        string savedText = PlayerPrefs.GetString("SavedText");
        input.text = savedText;
    
    }

    void Update()
    {
        if (text.text != previousDateText)
        {
            DateTime selectedDate;
            if (DateTime.TryParse(text.text, out selectedDate))
            {
                List<string> jobs = GetJobsForDate(selectedDate);
                UpdateInputField(jobs);
            }

            previousDateText = text.text;
        }
    }
    private List<string> GetJobsForDate(DateTime selectedDate)
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
        PlayerPrefs.SetString("SavedText", textToSave);
        PlayerPrefs.Save();
        Debug.Log("Save");

        DateTime currentDate = DateTime.Now;
        // Create the formatted note string with date and text
        string note = string.Format("[{0}] {1}", currentDate.ToString(), textToSave);

        text.text = textToSave;
        AddJob(currentDate, textToSave);
       // input.text = ""; // Clear the input field

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

