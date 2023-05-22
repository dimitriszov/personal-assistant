using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UI.Tables;
using TMPro;

namespace UI.Dates
{
    public class DatePicker_DayButton : DatePicker_Button
    {
        public DateTime Date;
        public DatePickerDayButtonType Type;
        [SerializeField] public Text dayText;


        [HideInInspector]
        public DatePicker DatePicker;

        public void Clicked()
        {
            if (!Button.interactable) return;
            

            DatePicker.DayButtonClicked(Date);
          //  string selectedDay = gameObject.name;
           // CheckNoteForDay(selectedDay);
        }
        
        public void MouseOver()
        {
            if (!Button.interactable) return;

            DatePicker.DayButtonMouseOver(Date);
        }

       // public void OnButtonClick()
      //  {
      //      string selectedDay = dayText.text;
      //      CheckNoteForDay(selectedDay);
      //  }

       
    }
}
