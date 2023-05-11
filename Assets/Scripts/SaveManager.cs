using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class SaveManager : MonoBehaviour
{
    [SerializeField] public Text text;
    [SerializeField] public InputField input;
    void Start()
    {
        string savedText = PlayerPrefs.GetString("SavedText");
        input.text = savedText;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void saveSchedule()
    {
       string textToSave = input.text;
       PlayerPrefs.SetString("SavedText", textToSave);
       PlayerPrefs.Save();
       Debug.Log("Save");

    }

  
    public void deleteSchedule()
    {
        PlayerPrefs.DeleteKey("SavedText");
        PlayerPrefs.Save();

        Debug.Log("Delete");
    }
}

