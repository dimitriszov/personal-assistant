using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using Newtonsoft.Json;

public class JobsListManager : MonoBehaviour
{
    [SerializeField] public Text dateText;
    public TMP_Text jobsText;
    private Dictionary<string, List<string>> calendarData = new Dictionary<string, List<string>>();
    private string previousDateText = "Hello";

    private void Awake()
    {
        LoadCalendarData();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dateText.text != previousDateText)
        {
            List<string> jobs = GetJobsForDate(dateText.text);
            UpdateInputField(jobs);

            previousDateText = dateText.text;
        }
    }

    private void UpdateInputField(List<string> jobs)
    {
        jobsText.text = string.Join("\n", jobs);
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

    public void LoadCalendarData()
    {
        if (File.Exists("calendar.json"))
        {
            string json = File.ReadAllText("calendar.json");
            calendarData = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
        }
        else
        {
            Debug.LogWarning("Dictionary file not found.");
        }
    }
}
