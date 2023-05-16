using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace MyMap.Manager
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] public RawImage image;
        private static string ApiKey = "Ah5XwZwPAQ5zlWx98JuOw3X9ntgd85RAEbypRrWfVfTXUQ8Gn7xd9E3ttD9FOaii";
        private static string mapImageUrl = "https://dev.virtualearth.net/REST/v1/Imagery/Map/Road/Routes/driving";
        public string startLocation = "Seattle, WA";
        public string endLocation = "Bellevue, WA";
        public string zoomLevel = "15";
        public string mapSize = "1000,1000";

        //Singleton pattern
        public static MapManager Instance;

        private void Awake()
        {
            Instance = this;
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
            StartCoroutine(GetMap());
        }

        // Getters
        public string getApiKey() { return ApiKey; }
        public string getMapImageURL() { return mapImageUrl; }
        public string getStatrtLocation() { return startLocation; }
        public string getZoomLevel() { return zoomLevel; }
        public string getMapSize() { return mapSize; }

        // Setters
        public void setStartLocation(string location) { this.startLocation = location; }
        public void setEndLocation(string location) { this.endLocation = location; }
        public void setZoomLevel(string zoomLevel) { this.zoomLevel = zoomLevel; }
        public void setMapSize(string mapSize) { this.mapSize = mapSize; }

        private IEnumerator GetMap()
        {
            string requestUrl = $"{mapImageUrl}?wp.0={startLocation}&wp.1={endLocation}&mapSize={mapSize}&key={ApiKey}";
            Debug.Log(requestUrl);
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(requestUrl);
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                image.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            }
        }
    }
}