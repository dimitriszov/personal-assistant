using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public string PathToFolder; // Relative path to the default folder inside Unity Assets

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Diamond.SetActive(false);
        LoadAudioClips();
        Play();
    }

    // Update is called once per frame
    void Update()
    {
        Songname.text = AudioClips[CurrentTrack].name;
        ProgressBar.value = Mathf.Clamp01(audioSource.time / 100);
        Diamond.SetActive(isPlaying);
        PlayButton.SetActive(!isPlaying);
    }

    void LoadAudioClips()
    {
        // Clear the existing AudioClips list
        AudioClips.Clear();

        // Get the absolute path of the default folder inside Unity Assets
        string folderPath = Path.Combine(Application.dataPath, PathToFolder);

        // Get all MP3 files in the specified folder
        DirectoryInfo dir = new DirectoryInfo(folderPath);
        FileInfo[] fileInfo = dir.GetFiles("*.mp3", SearchOption.TopDirectoryOnly);

        // Load each MP3 file as an AudioClip and add it to the list
        foreach (FileInfo file in fileInfo)
        {
            StartCoroutine(LoadAudioClip(file.FullName));
        }
    }

    IEnumerator LoadAudioClip(string filePath)
    {
        string url = string.Format("file://{0}", filePath);
        using (var www = new WWW(url))
        {
            yield return www;
            AudioClip clip = www.GetAudioClip();
            clip.name = Path.GetFileNameWithoutExtension(filePath);
            AudioClips.Add(clip);
        }
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
