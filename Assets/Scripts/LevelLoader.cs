using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EasyUI.Popup;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    public GameObject loadingPanel;
    [SerializeField] 
    public  RectTransform fxHolder;
    [SerializeField] 
    public Image circle;
    [SerializeField] 
    public Text progressText;
    public void loadLevel(string name)
    {
        if (SceneManager.GetActiveScene().name == name)
            return;
        loadingPanel.SetActive(true);
        StartCoroutine(LoadAsychronously(name));
        loadingPanel.SetActive(false);
    }

    IEnumerator LoadAsychronously(string name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress/ .9f);
            progressText.text = Mathf.Floor(progress * 100).ToString();
            fxHolder.rotation = Quaternion.Euler(new Vector3(0f, 0f, -progress*360));
            Debug.Log(progress);
            yield return null;
        }
    }

    public void closeApp()
    {
        Debug.Log("Closing App");
        // Popup.Show("Exit App", "Are you sure you want to exit?", "Yes", PopupColor.Red, () => Application.Quit());
        Application.Quit();
    }
}
