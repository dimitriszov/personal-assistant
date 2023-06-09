using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public DialogMessage[] messages;
    public Actor[] actors;

    public void OpenDialogue()
    {
        FindObjectOfType<DialogManager>().OpenDialogue(messages, actors);
    }

    public void OpenDialogue(string message)
    {
        DialogMessage[] dialogArray = new DialogMessage[]
                                        {
                                            new DialogMessage(0, message)
                                        };
        FindObjectOfType<DialogManager>().OpenDialogue(dialogArray, actors);
    }
}

[Serializable]
public class DialogMessage
{
    public int acrorId;
    [TextArea(3, 10)]
    public string message;

    public DialogMessage(int acrorId, string message)
    {
        this.acrorId = acrorId;
        this.message = message;
    }
}

[Serializable]
public class Actor {
    public string name;
    public Sprite sprite;
}
