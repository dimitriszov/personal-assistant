using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPlayAudio : MonoBehaviour
{
    public void PlaySound(string name)
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            audioManager.Play(name);
        }
        else
        {
            Debug.LogWarning("In order for the sound to play you either add an AudioManager from the prefabs (Prefabs -> MainScene) or you come here from the MainScene!!!");
        }
    }
}
