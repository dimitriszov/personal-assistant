using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    Resolution[] resolutions;
    public Dropdown resDropdown;
    public AudioMixer mixer;
    public GameObject button;
    public Text titleText;
    public string title;
    //public static SettingsMenu instance;

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
        
        /*if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);*/
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
    // Start is called before the first frame update
    public void SetVolume(float volume)
    {
        mixer.SetFloat("Volume", volume);
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
