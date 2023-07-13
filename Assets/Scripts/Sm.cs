using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sm : MonoBehaviour
{
    public List<AudioClip> AudioClips;
    AudioSource audioSource;
    public int CurrentTrack = 0;
    bool isPlaying = false;
    public Text Songname;
    public Slider ProgressBar;
    public GameObject Diamond;
    public GameObject PlayButton;
    public string PathToFolder; // Relative path to the default folder inside "Resources"

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Diamond.SetActive(false);
        LoadAudioClips();
        
    }

    // Update is called once per frame
    void Update()
    {
        Songname.text = AudioClips[CurrentTrack].name;
        ProgressBar.value = Mathf.Clamp01(audioSource.time / AudioClips[CurrentTrack].length);
        Diamond.SetActive(isPlaying);
        PlayButton.SetActive(!isPlaying);
    }

    void LoadAudioClips()
    {
        // Clear the existing AudioClips list
        AudioClips.Clear();

        // Load all AudioClips from the specified folder in "Resources"
        AudioClip[] clips = Resources.LoadAll<AudioClip>(PathToFolder);

        // Add the loaded AudioClips to the list
        AudioClips.AddRange(clips);
    }

    void Play()
    {
        if (!isPlaying)
        {
            audioSource.clip = AudioClips[CurrentTrack];
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
        CurrentTrack = (CurrentTrack + 1) % AudioClips.Count;
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
        CurrentTrack = (CurrentTrack + AudioClips.Count - 1) % AudioClips.Count;
        audioSource.clip = AudioClips[CurrentTrack];
        if (isPlaying)
        {
            isPlaying = false;
            Play();
        }
    }
}
