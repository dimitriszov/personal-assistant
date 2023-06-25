using TMPro;
using UnityEngine;

public class ClosePanelButton : MonoBehaviour
{
    private GameObject contactsPanel;
    private GameObject CallPanel;
    private TextMeshProUGUI buttonTextField;

    private void Start()
    {
        contactsPanel = GameObject.Find("IntroPanel");
        CallPanel  = GameObject.Find("CallPanel");
        buttonTextField = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void CloseContactsPanel()
    {
        GameObject otherTextObject = GameObject.Find("IDNUMBER");
        if (otherTextObject != null && buttonTextField != null)
        {
            TextMeshProUGUI otherTextField = otherTextObject.GetComponent<TextMeshProUGUI>();
            if (otherTextField != null)
            {
                otherTextField.text = buttonTextField.text;
            }
        }

        if (contactsPanel != null)
        {
            contactsPanel.SetActive(false);
        }
        if (CallPanel != null)
        {
            CallPanel.SetActive(true);
        }
    }
}
