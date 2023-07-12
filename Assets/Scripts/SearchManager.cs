using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class SearchManager : MonoBehaviour
{
    public TMP_InputField input;
    [SerializeField] public Transform container;
    [SerializeField] public GameObject prefab;
    private string APIkey = "AIzaSyCpvJ4yyPfI2Qz2s9crxSZyh1LRuqhrj1I";
    private string searchEngineId = "4715c1f418ff84742";

    // Models for deserialization
    [Serializable]
    public class SearchResponse
    {
        public List<Result> Items { get; set; }
    }

    [Serializable]
    public class Result
    {
        public string Link { get; set; }
        public string Snippet { get; set; }
        public string Title { get; set; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void search()
    {
        StartCoroutine(GetResults(input.text));
    }

    IEnumerator GetResults(string query)
    {
        emptyContainer();
        // Build the API request URL
        string apiUrl = $"https://www.googleapis.com/customsearch/v1?key={APIkey}&cx={searchEngineId}&q={query}";
        //Debug.Log(apiUrl);
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            request.timeout = 20;
            yield return request.SendWebRequest();

            // did the request succeed?
            if (request.result == UnityWebRequest.Result.Success)
            {
                // Process the JSON response
                string responseContent = request.downloadHandler.text;
                // Deserialize the JSON response
                SearchResponse searchResponse = JsonConvert.DeserializeObject<SearchResponse>(responseContent);

                // Extract the results
                List<Result> results = searchResponse.Items;

                // Limit the results to the first 20 sites
                List<Result> first20Results = results.GetRange(0, Math.Min(20, results.Count));

                // Print the URLs of the first 20 sites
                foreach (Result result in first20Results)
                {
                    //Debug.Log("Title: " + result.Title);
                    //Debug.Log("Description: " + result.Snippet);
                    //Debug.Log("URL: " + result.Link);
                    //Debug.Log("-----------------------------");
                    addResult(result.Title, result.Snippet, result.Link);
                }

                // For Debug, we'll just print the response content
                // Debug.Log(responseContent);
            }
            else
            {
                Debug.LogError($"Request failed with error: {request.error}");
            }
        }
        yield return null;
    }

    public void addResult(string title, string descr, string link)
    {
        GameObject result = Instantiate(prefab, container);
        if (result.TryGetComponent<SearchResult>(out SearchResult item))
        {
            item.setTitleText(title);
            item.setDescriptionText(descr);
            item.setLinkText(link);
        }
    }

    public void emptyContainer()
    {
        foreach (Transform child in container)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
