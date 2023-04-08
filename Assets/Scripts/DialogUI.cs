using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


namespace EasyUI.Dialogs
{
    public class Dialog
    {
        public string Title = "Title";
        public string Message = "Message goes here.";
    }

    public class DialogUI : MonoBehaviour
    {
        [SerializeField] GameObject canvas;
        [SerializeField] Text titleUIText;
        [SerializeField] Text messageUIText;
        [SerializeField] Button closeUIButton;

        Dialog dialog = new Dialog();


        //Singleton pattern
        public static DialogUI Instance;


        private void Awake()
        {
            Instance = this;

            // Add close event listener
            closeUIButton.onClick.RemoveAllListeners();
            closeUIButton.onClick.AddListener(Hide);
        }

        public DialogUI SetTitle(string title)
        {
            dialog.Title = title;
            return Instance;
        }

        public DialogUI SetMessage(string message)
        {
            dialog.Message = message;
            return Instance;
        }

        public void Show()
        {
            titleUIText.text = dialog.Title;
            messageUIText.text = dialog.Message;

            canvas.SetActive(true);
        }

        public void Hide()
        {
            canvas.SetActive(false);

            //Reset Dialog
            dialog = new Dialog();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

            // Update is called once per frame
         void Update()
         {

         }
    }
}
