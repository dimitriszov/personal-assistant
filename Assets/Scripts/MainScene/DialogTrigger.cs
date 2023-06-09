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
}

[Serializable]
public class DialogMessage
{
    public int acrorId;
    [TextArea(3, 10)]
    public string message;
}

[Serializable]
public class Actor {
    public string name;
    public Sprite sprite;
}
