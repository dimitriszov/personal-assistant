using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;


public class AlarmController : MonoBehaviour
{
 
    [SerializeField] public Text timeText;
    [SerializeField] public TMP_InputField hoursInput, minutesInput, secondsInput;
    [SerializeField] public TMP_Dropdown dropdown;
    [SerializeField] public GameObject alarmPanel;
    public bool isAlarmSet = false;
    public DateTime alarmTime = DateTime.Today;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int hours = DateTime.Now.Hour;
        int minutes = DateTime.Now.Minute;
        int seconds = DateTime.Now.Second;


        bool isAM = hours < 12;

        timeText.text = $"{hours % 12:D2}: {minutes:D2}: {seconds:D2} {(isAM ? "AM": "PM")}";

        if(isAlarmSet && DateTime.Now > alarmTime)
        {
            //Debug.Log("ALARM");
            alarmPanel.SetActive(true);
        }

    }


    public void SetAlarm()
    {

        alarmTime = DateTime.Today;

        int hours;
        if (dropdown.value == 0)
        {
            hours = int.Parse(hoursInput.text);
        }
        else;
        {
            hours = int.Parse(hoursInput.text) + 12;
        }
        TimeSpan ts = TimeSpan.Parse($"{hours}:{minutesInput.text}: {secondsInput.text}");
        alarmTime += ts;

        if (DateTime.Now >= alarmTime)
        {
            alarmTime = alarmTime.AddDays(1);

        }
       // Debug.Log(alarmTime);

        isAlarmSet = true;

    }
}
