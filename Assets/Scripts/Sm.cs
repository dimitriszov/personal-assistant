using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Sm : MonoBehaviour
{
    public AudioClip[] AudioClips;
    AudioSource audioSource;
    public int CurrentTrack = 0;
    bool isPlaying = false;
    public Text Songname;
    public Slider ProgressBar;
    public GameObject Diamond;
    public GameObject PlayButton;
    




    // Start is called before the first frame update
    void Start()
    {
        
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = AudioClips[CurrentTrack];
        Diamond.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Songname.text = AudioClips[CurrentTrack].name;
        ProgressBar.value = Mathf.Clamp01(audioSource.time / 100);
        if (isPlaying==true )
        {
            Diamond.SetActive(true);
            PlayButton.SetActive(false);

        }
        else
        {
            Diamond.SetActive(false);
            PlayButton.SetActive(true);
        }

    }
    void Play()
    {

        if (isPlaying == false)
        {
            audioSource.Play();
            isPlaying = true;
        }
    }

    void Pause()
    {
        audioSource.Pause();
        isPlaying = false;
    }

    void Mute()
    {
        audioSource.mute = true;
    }

    void UnMute()
    {
        audioSource.mute = false;
    }

    void Stop()
    {
        audioSource.Stop();
        isPlaying = false;

    }

    void StopRaw()
    {
        audioSource.Stop();
    }

    void Next()
    {
        StopRaw();
        if (CurrentTrack == AudioClips.Length - 1)
            CurrentTrack = 0;
        else
            CurrentTrack++;
        audioSource.clip = AudioClips[CurrentTrack];
        if (isPlaying)
        {
            isPlaying = false;
            Play();
        }
    }

    void Previous()
    {
        StopRaw();
        if (CurrentTrack == 0)
            CurrentTrack = AudioClips.Length - 1;
        else
            CurrentTrack--;
        audioSource.clip = AudioClips[CurrentTrack];
        if (isPlaying)
        {

            isPlaying = false;
            Play();
        }
    }





  








}

