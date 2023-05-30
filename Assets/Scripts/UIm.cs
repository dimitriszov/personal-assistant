using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIm : MonoBehaviour
{
    public GameObject PlayButton;
    public GameObject PauseButton;
    public GameObject NextButton;
    public GameObject PreviousButton;
    public GameObject MuteButton;
    public GameObject UnmuteButton;
    
   

    public GameObject Sm;
    // Start is called before the first frame update
    void Start()
    {
        NextButton.SetActive(true); 
        PreviousButton.SetActive(true);
        MuteButton.SetActive(true);
        UnmuteButton.SetActive(false);
       // Diamond.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        Sm.SendMessage("Play");
       // Diamond.SetActive(true);
    }

    public void Pause()
    {
        Sm.SendMessage("Pause");
       // Diamond.SetActive(false);
    }

    public void Next()
    {
        Sm.SendMessage("Next");
    }

    public void Previous()
    {
        Sm.SendMessage("Previous");
    }

    public void Stop()
    {
        Sm.SendMessage("Stop");
       // Diamond.SetActive(false);
    }

    public void Mute()
    {
        MuteButton.SetActive(false);
        UnmuteButton.SetActive(true);
        Sm.SendMessage("Mute");
    }

    public void Unmute()
    {
        UnmuteButton.SetActive(false);
        MuteButton.SetActive(true);
        Sm.SendMessage("UnMute");
    }
}
