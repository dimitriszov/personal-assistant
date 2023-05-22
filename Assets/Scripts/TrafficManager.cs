using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class TrafficManager : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI textTraffic;
    public enum EPhase
    {
        NotStarted,
        GetPublicIP,
        GetGeographicData,
        GetTrafficData,

        Failed,
        Succeeded
    }
    class geoPluginResponse
    {
        [JsonProperty("geoplugin_request")] public string Request { get; set; }
        [JsonProperty("geoplugin_status")] public int Status { get; set; }
        [JsonProperty("geoplugin_delay")] public string Delay { get; set; }
        [JsonProperty("geoplugin_credit")] public string Credit { get; set; }
        [JsonProperty("geoplugin_city")] public string City { get; set; }
        [JsonProperty("geoplugin_region")] public string Region { get; set; }
        [JsonProperty("geoplugin_regionCode")] public string RegionCode { get; set; }
        [JsonProperty("geoplugin_regionName")] public string RegionName { get; set; }
        [JsonProperty("geoplugin_areaCode")] public string AreaCode { get; set; }
        [JsonProperty("geoplugin_dmaCode")] public string DMACode { get; set; }
        [JsonProperty("geoplugin_countryCode")] public string CountryCode { get; set; }
        [JsonProperty("geoplugin_countryName")] public string CountryName { get; set; }
        [JsonProperty("geoplugin_inEU")] public int InEU { get; set; }
        [JsonProperty("geoplugin_euVATrate")] public bool EUVATRate { get; set; }
        [JsonProperty("geoplugin_continentCode")] public string ContinentCode { get; set; }
        [JsonProperty("geoplugin_continentName")] public string ContinentName { get; set; }
        [JsonProperty("geoplugin_latitude")] public string Latitude { get; set; }
        [JsonProperty("geoplugin_longitude")] public string Longitude { get; set; }
        [JsonProperty("geoplugin_locationAccuracyRadius")] public string LocationAccuracyRadius { get; set; }
        [JsonProperty("geoplugin_timezone")] public string TimeZone { get; set; }
        [JsonProperty("geoplugin_currencyCode")] public string CurrencyCode { get; set; }
        [JsonProperty("geoplugin_currencySymbol")] public string CurrencySymbol { get; set; }
        [JsonProperty("geoplugin_currencySymbol_UTF8")] public string CurrencySymbolUTF8 { get; set; }
        [JsonProperty("geoplugin_currencyConverter")] public double CurrencyConverter { get; set; }
    }
    class Point
    {
        [JsonProperty("type")] public string TypeDescr { get; set; }
        [JsonProperty("coordinates")] public List<double> Coordinates { get; set; }
    }

    class Resource
    {
        [JsonProperty("__type")] public string TypeDescr { get; set; }
        [JsonProperty("point")] public Point Points { get; set; }
        [JsonProperty("description")] public string Description { get; set; }
        [JsonProperty("detour")] public string Detour { get; set; }
        [JsonProperty("end")] public string End { get; set; }
        [JsonProperty("incidentId")] public long Id { get; set; }
        [JsonProperty("lane")] public string Lane { get; set; }
        [JsonProperty("lastModified")] public string LastModified { get; set; }
        [JsonProperty("roadClosed")] public bool RoadClosed { get; set; }
        [JsonProperty("severity")] public int Severity { get; set; }
        [JsonProperty("start")] public string Start { get; set; }
        [JsonProperty("type")] public int Type { get; set; }
        [JsonProperty("verified")] public bool Verified { get; set; }
    }

    class ResourceSet
    {
        [JsonProperty("estimatedTotal")] public int Total { get; set; }
        [JsonProperty("resources")] public List<Resource> Resources { get; set; }
    }

    class TrafficResult
    {
        [JsonProperty("authenticationResultCode")] public string ResultCode { get; set; }
        [JsonProperty("brandLogoUri")] public string BrandLogoURI { get; set; }
        [JsonProperty("copyright")] public string Copyright { get; set; }
        [JsonProperty("resourceSets")] public List<ResourceSet> ResourceSets { get; set; }
        [JsonProperty("statusCode")] public int Status { get; set; }
        [JsonProperty("statusDescription")] public string StatusDescription { get; set; }
        [JsonProperty("traceId")] public string Id { get; set; }
    }

    const string URL_GetPublicIP = "https://api.ipify.org/";
    public EPhase Phase { get; private set; } = EPhase.NotStarted;

    string PublicIP; 
    const string URL_GetGeographicData = "http://www.geoplugin.net/json.gp?ip=";
    const string URL_GetTrafficData = "http://dev.virtualearth.net/REST/v1/Traffic/Incidents/";
    private static string ApiKey = "Ah5XwZwPAQ5zlWx98JuOw3X9ntgd85RAEbypRrWfVfTXUQ8Gn7xd9E3ttD9FOaii";
    geoPluginResponse GeographicData;
    TrafficResult trafficResult;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetWeather_Phase1_PublicIP());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GetWeather_Phase1_PublicIP()
    {
        Phase = EPhase.GetPublicIP;

        // attempt to retrieve our public IP address
        using (UnityWebRequest request = UnityWebRequest.Get(URL_GetPublicIP))
        {
            request.timeout = 20;
            yield return request.SendWebRequest();

            // did the request succeed?
            if (request.result == UnityWebRequest.Result.Success)
            {
                PublicIP = request.downloadHandler.text.Trim();
                StartCoroutine(GetWeather_Phase2_GeographicInformation());
            }
            else
            {
                Debug.LogError($"Failed to get public IP: {request.downloadHandler.text}");
                Phase = EPhase.Failed;
            }
        }

        yield return null;
    }

    IEnumerator GetWeather_Phase2_GeographicInformation()
    {
        Phase = EPhase.GetGeographicData;

        // attempt to retrieve the geographic data
        using (UnityWebRequest request = UnityWebRequest.Get(URL_GetGeographicData + PublicIP))
        {
            Debug.Log(URL_GetGeographicData + PublicIP);
            request.timeout = 1;
            yield return request.SendWebRequest();

            // did the request succeed?
            if (request.result == UnityWebRequest.Result.Success)
            {
                GeographicData = JsonConvert.DeserializeObject<geoPluginResponse>(request.downloadHandler.text);
                StartCoroutine(GetTraffic_Phase3());
            }
            else
            {
                Debug.LogError($"Failed to get geographic data: {request.downloadHandler.text}");
                Phase = EPhase.Failed;
            }
        }

        yield return null;
    }

    IEnumerator GetTraffic_Phase3()
    {
        Phase = EPhase.GetTrafficData;
        // attempt to retrieve the geographic data
        string mapArea = GetBoundingBox(GeographicData.Latitude, GeographicData.Longitude);
        // Debug.Log(URL_GetTrafficData + mapArea + "?key=" + ApiKey);
        using (UnityWebRequest request = UnityWebRequest.Get(URL_GetTrafficData + mapArea + "?key=" + ApiKey))
        {
            request.timeout = 1;
            yield return request.SendWebRequest();
            // did the request succeed?
            if (request.result == UnityWebRequest.Result.Success)
            {
                trafficResult = JsonConvert.DeserializeObject<TrafficResult>(request.downloadHandler.text);
                //Debug.Log(request.downloadHandler.text);
                printTraffic();
            }
            else
            {
                Debug.LogError($"Failed to get geographic data: {request.downloadHandler.text}");
                Phase = EPhase.Failed;
            }
        }
    }

    public string GetBoundingBox(string lat, string lon, double radius = 10)
    {
        double latitude = double.Parse(lat, System.Globalization.CultureInfo.InvariantCulture);
        double longitude = double.Parse(lon, System.Globalization.CultureInfo.InvariantCulture);
        // radius is in kilometers
        double r_earth = 6371; // Earth's radius in kilometers
        double delta_lat = 2 * Math.Asin(Math.Sin(radius / (2 * r_earth)) / Math.Cos(Math.PI / 180 * latitude));
        double delta_lon = 2 * Math.Asin(Math.Sin(radius / (2 * r_earth)) / Math.Cos(Math.PI / 180 * longitude));
        double min_latitude = latitude - (180 / Math.PI) * delta_lat;
        double max_latitude = latitude + (180 / Math.PI) * delta_lat;
        double min_longitude = longitude - (180 / Math.PI) * delta_lon;
        double max_longitude = longitude + (180 / Math.PI) * delta_lon;
        return min_latitude.ToString("F") + "," + min_longitude.ToString("F") + "," + max_latitude.ToString("F") + "," + max_longitude.ToString("F");
    }

    public void printTraffic()
    {
        string result = "<align=\"center\"><b>Traffic Info</b>\n<align=\"left\">";
        foreach(Resource res in trafficResult.ResourceSets[0].Resources)
        {
            result += res.Description + "\n";
        }
        textTraffic.text = result;
    }
}
