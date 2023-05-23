using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchResult : MonoBehaviour
{
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeText(string newText)
    {
        if (newText != null)
        {
            text.text = newText;
        }
    }

    public void openLink()
    {
        Application.OpenURL(text.text);
    }
}
