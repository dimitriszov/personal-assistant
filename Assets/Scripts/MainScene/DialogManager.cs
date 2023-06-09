using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Image actorImage;
    public TMP_Text actorName;
    public TMP_Text messageText;
    public RectTransform backroundBox;
    DialogMessage[] currentMessages;
    Actor[] currentActors;
    int activeMessage = 0;
    public static bool isActive = false;

    public void OpenDialogue(DialogMessage[] messages, Actor[] actors)
    {
        isActive = true;
        currentMessages = messages;
        currentActors = actors;
        activeMessage = 0;

        DisplayMessage();
        backroundBox.LeanScale(Vector3.one, 0.5f);
    }

    void DisplayMessage()
    {
        DialogMessage messageToDisplay = currentMessages[activeMessage];
        StopAllCoroutines();
        StartCoroutine(TypeSentence(messageToDisplay.message));

        Actor actorToDisplay = currentActors[messageToDisplay.acrorId];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;
        AnimateTextColor();
    }

    public void NextMessage()
    {
        activeMessage++;
        if (activeMessage < currentMessages.Length) 
        {
            DisplayMessage();
        } else
        {
            backroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();
            isActive = false;
        }
    }

    void AnimateTextColor()
    {
        LeanTween.textAlpha(messageText.rectTransform, 0, 0);
        LeanTween.textAlpha(messageText.rectTransform, 1, 0.5f);
    }

    IEnumerator TypeSentence(string message)
    {
        messageText.text = "";
        foreach(char letter in message.ToCharArray())
        {
            messageText.text += letter;
            yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        backroundBox.transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)  && isActive == true) 
        { 
            NextMessage();
        }
    }
}
