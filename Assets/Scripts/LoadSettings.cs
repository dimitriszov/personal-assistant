using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSettings : MonoBehaviour
{
    [SerializeField]
    public GameObject settingsCanvas;
    // Start is called before the first frame update
    public void showSettings()
    {
        Instantiate(settingsCanvas);
    }
}
