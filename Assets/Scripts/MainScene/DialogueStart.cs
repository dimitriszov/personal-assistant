using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStart : MonoBehaviour
{
    public DialogTrigger dialogTrigger;
    // Start is called before the first frame update
    void Start()
    {
        dialogTrigger.OpenDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
