using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class AlarmClock : MonoBehaviour
{
    public Text timeText; // reference to the TextMeshProUGUI object where the time is displayed
    public AudioSource alarmSound; // reference to the AudioSource component that plays the alarm sound
    public GameObject alarmActiveIndicator; // reference to the game object that indicates whether the alarm is active or not

    private DateTime alarmTime ; // the time when the alarm should go off
    private bool alarmActive = false; // whether the alarm is currently active or not

    private void Start()
    {
        // Set the current time as the default time
        UpdateTime(DateTime.Now);
    }

    private void Update()
    {
        // Update the time every frame
        UpdateTime(DateTime.Now);

        // Check if the current time matches the alarm time and the alarm is active
        if (DateTime.Now.Hour == alarmTime.Hour && DateTime.Now.Minute == alarmTime.Minute && alarmActive)
        {
            // Play the alarm sound and show the alarm active indicator
            alarmSound.Play();
            alarmActiveIndicator.SetActive(true);
        }
        else
        {
            // Stop the alarm sound and hide the alarm active indicator
            alarmSound.Stop();
            alarmActiveIndicator.SetActive(false);
        }
    }

    private void UpdateTime(DateTime dateTime)
    {
        // Update the timeText object with the current time
        timeText.text = dateTime.ToString("hh:mm:ss tt");
    }

    public void SettingAlarmTime(DateTime newAlarmTime)
    {
        // Set the new alarm time and activate the alarm
        alarmTime = newAlarmTime;
        alarmActive = true;
    }

    public void DisableAlarm()
    {
        // Deactivate the alarm
        alarmActive = false;
    }
}
