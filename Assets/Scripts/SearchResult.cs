using UnityEngine;
using TMPro;

public class SearchResult : MonoBehaviour
{
    public TMP_Text TitleText, LinkText, DescriptionText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTitleText(string newText)
    {
        if (newText != null)
        {
            TitleText.text = newText;
        }
    }

    public void setLinkText(string newText) 
    {
        if (newText != null)
        {
            LinkText.text = newText;
        }
    }

    public void setDescriptionText(string newText) 
    {  
        if (newText != null)
        { 
            DescriptionText.text = newText; 
        }
    }

    public void openLink()
    {
        Application.OpenURL(LinkText.text);
    }
}
