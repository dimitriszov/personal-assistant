using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    public GameObject loadingPanel;
    [SerializeField]
    public GameObject settingsPanel;
    [SerializeField] 
    public  RectTransform fxHolder;
    [SerializeField] 
    public UnityEngine.UI.Image circle;
    [SerializeField] 
    public Text progressText;
    public void loadLevel(string name)
    {
        StartCoroutine(LoadAsychronously(name));
    }

    IEnumerator LoadAsychronously(string name)
    {
        settingsPanel.SetActive(false);
        loadingPanel.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress/ .9f);
            progressText.text = Mathf.Floor(progress * 100).ToString();
            fxHolder.rotation = Quaternion.Euler(new Vector3(0f, 0f, -progress*360));
            Debug.Log(progress);
            yield return null;
        }
        loadingPanel.SetActive(false);
    }

    public void closeApp()
    {
        Debug.Log("Closing App");
        Application.Quit();
    }
}
