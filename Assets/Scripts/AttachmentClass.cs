using MyEmail.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttachmentClass : MonoBehaviour
{
    [SerializeField] public Button button = null;
    [SerializeField] public Text attachmentText;
    [SerializeField] public EmailManager emailManager = null;

    private void Awake()
    {
        button.onClick.AddListener(() => emailManager.removeAttachment(attachmentText.text));
        button.onClick.AddListener(() => Destroy(this.gameObject));
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    public void changeText(string text)
    {
        attachmentText.text = text;
    }
}
