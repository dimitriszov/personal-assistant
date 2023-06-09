using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    Resolution[] resolutions;
    public Dropdown resDropdown;
    public AudioMixer mixer;
    public Slider musicSlider;
    public Slider sfxSlider;
    public GameObject button;
    public TMP_Text titleText;
    public string title;

    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";

    private void Awake()
    {
        if (title != null)
        {
            titleText.text = title;
        }
        else 
        {
            titleText.text = "NO TITLE PROVIDED";
        }
        if (SceneManager.GetActiveScene().buildIndex == 0)
            button.SetActive(false);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        resDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].width + " x " + resolutions[i].height);

            if (resolutions[i].width == GetCurrentResolution().width && resolutions[i].height == GetCurrentResolution().height)
            {
                currentResIndex = i;
            }
        }
        resDropdown.AddOptions(options);
        resDropdown.value = currentResIndex;
        resDropdown.RefreshShownValue();

        // Set volume sliders
        musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);
        sfxSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicSlider.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, sfxSlider.value);
    }

    private static Resolution GetCurrentResolution()
    {
        return Screen.currentResolution;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(volume) * 20);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
