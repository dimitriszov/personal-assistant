using UnityEngine;

public class DropdownHandler : MonoBehaviour
{
    public LevelLoader levelLoader;
    public DialogTrigger dialogTrigger;

    public void Start()
    {
        dialogTrigger.OpenDialogue();
    }

    public void HandleInputData(int val)
    {
        switch (val)
        {
            case 0:
                levelLoader.loadLevel("Mail");
                break;
            case 1:
                Application.OpenURL("https://mail.google.com/");
                break;
            case 2:
                levelLoader.loadLevel("SearchScene");
                break;
            case 3:
                levelLoader.loadLevel("NavigationScene");
                break;
            case 4:
                levelLoader.loadLevel("WeatherScene");
                break;
            case 5:
                levelLoader.loadLevel("Diary");
                break;
            case 6:
                levelLoader.loadLevel("Clock");
                break;
            case 7:
                levelLoader.loadLevel("Contacts");
                break;
            /*case 8:
                levelLoader.loadLevel("SearchScene");
                break;
            case 9:
                levelLoader.loadLevel("SearchScene");
                break;
            case 10:
                levelLoader.loadLevel("SearchScene");
                break;
            case 11:
                levelLoader.loadLevel("SearchScene");
                break;
            case 12:
                levelLoader.loadLevel("SearchScene");
                break;
            case 13:
                levelLoader.loadLevel("SearchScene");
                break;
            case 14:
                levelLoader.loadLevel("SearchScene");
                break;
                */
        }
    }
}
