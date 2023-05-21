using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.Net.Http;
using System.IO;
using System;

public class GetWeatherInfo : MonoBehaviour
{
    public Text cityText, tempText, conditionText, humidityText, pressureText;
    public Material[] skyboxes;
    public ParticleSystem snow, rain;
    [SerializeField] public RawImage image;
    public string units;
    #region Weather API Key
    const string OpenWeatherAPIKey = "52402191a5aeba7fe2cfd6e4633968a6";
    #endregion 

    public enum EPhase
    {
        NotStarted,
        GetPublicIP,
        GetGeographicData,
        GetWeatherData,

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


    public class OpenWeather_Coordinates
    {
        [JsonProperty("lon")] public double Longitude { get; set; }
        [JsonProperty("lat")] public double Latitude { get; set; }
    }

    // Condition Info: https://openweathermap.org/weather-conditions
    public class OpenWeather_Condition
    {
        [JsonProperty("id")] public int ConditionID { get; set; }
        [JsonProperty("main")] public string Group { get; set; }
        [JsonProperty("description")] public string Description { get; set; }
        [JsonProperty("icon")] public string Icon { get; set; }
    }

    public class OpenWeather_KeyInfo
    {
        [JsonProperty("temp")] public double Temperature { get; set; }
        [JsonProperty("feels_like")] public double Temperature_FeelsLike { get; set; }
        [JsonProperty("temp_min")] public double Temperature_Minimum { get; set; }
        [JsonProperty("temp_max")] public double Temperature_Maximum { get; set; }
        [JsonProperty("pressure")] public int Pressure { get; set; }
        [JsonProperty("sea_level")] public int PressureAtSeaLevel { get; set; }
        [JsonProperty("grnd_level")] public int PressureAtGroundLevel { get; set; }
        [JsonProperty("humidity")] public int Humidity { get; set; }
    }

    public class OpenWeather_Wind
    {
        [JsonProperty("speed")] public double Speed { get; set; }
        [JsonProperty("deg")] public int Direction { get; set; }
        [JsonProperty("gust")] public double Gust { get; set; }
    }

    public class OpenWeather_Clouds
    {
        [JsonProperty("all")] public int Cloudiness { get; set; }
    }

    public class OpenWeather_Rain
    {
        [JsonProperty("1h")] public int VolumeInLastHour { get; set; }
        [JsonProperty("3h")] public int VolumeInLast3Hours { get; set; }
    }

    public class OpenWeather_Snow
    {
        [JsonProperty("1h")] public int VolumeInLastHour { get; set; }
        [JsonProperty("3h")] public int VolumeInLast3Hours { get; set; }
    }

    public class OpenWeather_Internal
    {
        [JsonProperty("type")] public int Internal_Type { get; set; }
        [JsonProperty("id")] public int Internal_ID { get; set; }
        [JsonProperty("message")] public double Internal_Message { get; set; }
        [JsonProperty("country")] public string CountryCode { get; set; }
        [JsonProperty("sunrise")] public int SunriseTime { get; set; }
        [JsonProperty("sunset")] public int SunsetTime { get; set; }
    }

    class OpenWeatherResponse
    {
        [JsonProperty("coord")] public OpenWeather_Coordinates Location { get; set; }
        [JsonProperty("weather")] public List<OpenWeather_Condition> WeatherConditions { get; set; }
        [JsonProperty("base")] public string Internal_Base { get; set; }
        [JsonProperty("main")] public OpenWeather_KeyInfo KeyInfo { get; set; }
        [JsonProperty("visibility")] public int Visibility { get; set; }
        [JsonProperty("wind")] public OpenWeather_Wind Wind { get; set; }
        [JsonProperty("clouds")] public OpenWeather_Clouds Clouds { get; set; }
        [JsonProperty("rain")] public OpenWeather_Rain Rain { get; set; }
        [JsonProperty("snow")] public OpenWeather_Snow Snow { get; set; }
        [JsonProperty("dt")] public int TimeOfCalculation { get; set; }
        [JsonProperty("sys")] public OpenWeather_Internal Internal_Sys { get; set; }
        [JsonProperty("timezone")] public int Timezone { get; set; }
        [JsonProperty("id")] public int CityID { get; set; }
        [JsonProperty("name")] public string CityName { get; set; }
        [JsonProperty("cod")] public int Internal_COD { get; set; }
    }

    const string URL_GetPublicIP = "https://api.ipify.org/";
    const string URL_GetGeographicData = "http://www.geoplugin.net/json.gp?ip=";
    const string URL_GetWeatherData = "https://api.openweathermap.org/data/2.5/weather";

    public EPhase Phase { get; private set; } = EPhase.NotStarted;

    string PublicIP;
    geoPluginResponse GeographicData;
    OpenWeatherResponse WeatherData;
    bool ShownWeatherInfo = false;

    private void Awake()
    {
        rain.Stop();
        snow.Stop();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (string.IsNullOrEmpty(OpenWeatherAPIKey))
        {
            Debug.LogError("No API key set for https://openweathermap.org/");
            return;
        }
        StartCoroutine(GetWeather_Phase1_PublicIP());
    }

    // Update is called once per frame
    void Update()
    {
        string iconCode = "";
        if (Phase == EPhase.Succeeded && !ShownWeatherInfo)
        {
            var sb = new System.Text.StringBuilder();
            ShownWeatherInfo = true;

            Debug.Log($"Weather info for {WeatherData.CityName}");
            cityText.text = WeatherData.CityName;
            Debug.Log($"Temperature: {WeatherData.KeyInfo.Temperature}");
            tempText.text = WeatherData.KeyInfo.Temperature.ToString();
            Debug.Log($"Humidity: {WeatherData.KeyInfo.Humidity}");
            humidityText.text = WeatherData.KeyInfo.Humidity.ToString();
            Debug.Log($"Pressure: {WeatherData.KeyInfo.Pressure}");
            pressureText.text = WeatherData.KeyInfo.Pressure.ToString();
            string pathFile = "Assets/Images/Weather/01d.png";
            foreach (var condition in WeatherData.WeatherConditions)
            {
                Debug.Log($"{condition.Group}: {condition.Description}");
                sb.Append(condition.Group + ": " + condition.Description + "\n");
                iconCode = condition.Icon;
                pathFile = $"Assets/Images/Weather/Icons/{condition.Icon}.png";
            }
            conditionText.text = sb.ToString();
            var rawData = File.ReadAllBytes(pathFile);
            if(iconCode.Equals("03d"))
            {
                changeSkyBox(1);
            }
            switch (iconCode)
            {
                case "01d":
                    changeSkyBox(0);
                    break;
                case "02d":
                case "03d":
                case "04d":
                    changeSkyBox(2);
                    break;
                case "09d":
                case "10d":
                case "11d":
                    changeSkyBox(1);
                    rain.Play();
                    break;
                case "13d":
                    changeSkyBox(1);
                    snow.Play();
                    break;
                case "50d":
                    changeSkyBox(1);
                    break;
            }
            Texture2D tex = new Texture2D(2, 2); // Create an empty Texture; size doesn't matter (she said)
            tex.LoadImage(rawData);
            image.texture = tex;
        }
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
            request.timeout = 1;
            yield return request.SendWebRequest();

            // did the request succeed?
            if (request.result == UnityWebRequest.Result.Success)
            {
                GeographicData = JsonConvert.DeserializeObject<geoPluginResponse>(request.downloadHandler.text);
                StartCoroutine(GetWeather_Phase3_WeatherInformation());
            }
            else
            {
                Debug.LogError($"Failed to get geographic data: {request.downloadHandler.text}");
                Phase = EPhase.Failed;
            }
        }

        yield return null;
    }

    IEnumerator GetWeather_Phase3_WeatherInformation()
    {
        Phase = EPhase.GetWeatherData;

        string weatherURL = URL_GetWeatherData;
        weatherURL += $"?lat={GeographicData.Latitude}";
        weatherURL += $"&lon={GeographicData.Longitude}";
        weatherURL += $"&appid={OpenWeatherAPIKey}";
        weatherURL += $"&units={units}";

        // attempt to retrieve the geographic data
        using (UnityWebRequest request = UnityWebRequest.Get(weatherURL))
        {
            request.timeout = 10;
            yield return request.SendWebRequest();

            // did the request succeed?
            if (request.result == UnityWebRequest.Result.Success)
            {
                WeatherData = JsonConvert.DeserializeObject<OpenWeatherResponse>(request.downloadHandler.text);
                Phase = EPhase.Succeeded;
            }
            else
            {
                Debug.LogError($"Failed to get geographic data: {request.downloadHandler.text}");
                Phase = EPhase.Failed;
            }
        }

        yield return null;
    }

    public void changeSkyBox(int index)
    {
        RenderSettings.skybox = skyboxes[index];
    }
}
