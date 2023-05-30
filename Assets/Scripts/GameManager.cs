using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int maxMessages = 25;

    public GameObject chatPanel, textObject;
    public TMP_InputField chatBox;
    bool FLAG = false;
    [SerializeField]
    List<Message> messageList = new List<Message>(); 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string h = System.DateTime.UtcNow.ToString();
        if (chatBox.isFocused && chatBox.text== "" && FLAG == true )
        {
            
            SendMessageToChat("Sent by User");
            SendMessageToChat(h);
            FLAG = false;
            

        }
        if (chatBox.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessageToChat(chatBox.text);
                chatBox.text = "";
                chatBox.ActivateInputField();
                FLAG = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                chatBox.ActivateInputField();
                
            }
        }
        
      

    }









    public void SendMessageToChat(string text)
    {
      
        if (messageList.Count >= maxMessages) 
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }
        Message newMessage = new Message();
        newMessage.text = text;


        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject= newText.GetComponent<Text>();

        newMessage.textObject.text = newMessage.text;

        messageList.Add(newMessage);
    }
}




[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
}