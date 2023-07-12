using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class TranslateManager : MonoBehaviour
{
    public TMP_Dropdown initialLanguageDropdown;
    public TMP_Dropdown targetLanguageDropdown;
    public TMP_InputField inputField;
    public TMP_Text output;

    private Dictionary<string, string> languageCodes;

    private void Start()
    {
        // Initialize the language codes dictionary
        languageCodes = new Dictionary<string, string>()
        {
            {"Afrikaans", "af"},
            {"Albanian", "sq"},
            {"Amharic", "am"},
            {"Arabic", "ar"},
            {"Armenian", "hy"},
            {"Azerbaijani", "az"},
            {"Basque", "eu"},
            {"Belarusian", "be"},
            {"Bengali", "bn"},
            {"Bosnian", "bs"},
            {"Bulgarian", "bg"},
            {"Catalan", "ca"},
            {"Cebuano", "ceb"},
            {"Chinese (Simplified)", "zh-CN"},
            {"Chinese (Traditional)", "zh-TW"},
            {"Corsican", "co"},
            {"Croatian", "hr"},
            {"Czech", "cs"},
            {"Danish", "da"},
            {"Dutch", "nl"},
            {"English", "en"},
            {"Esperanto", "eo"},
            {"Estonian", "et"},
            {"Finnish", "fi"},
            {"French", "fr"},
            {"Frisian", "fy"},
            {"Galician", "gl"},
            {"Georgian", "ka"},
            {"German", "de"},
            {"Greek", "el"},
            {"Gujarati", "gu"},
            {"Haitian Creole", "ht"},
            {"Hausa", "ha"},
            {"Hawaiian", "haw"},
            {"Hebrew", "he"},
            {"Hindi", "hi"},
            {"Hmong", "hmn"},
            {"Hungarian", "hu"},
            {"Icelandic", "is"},
            {"Igbo", "ig"},
            {"Indonesian", "id"},
            {"Irish", "ga"},
            {"Italian", "it"},
            {"Japanese", "ja"},
            {"Javanese", "jw"},
            {"Kannada", "kn"},
            {"Kazakh", "kk"},
            {"Khmer", "km"},
            {"Korean", "ko"},
            {"Kurdish", "ku"},
            {"Kyrgyz", "ky"},
            {"Lao", "lo"},
            {"Latin", "la"},
            {"Latvian", "lv"},
            {"Lithuanian", "lt"},
            {"Luxembourgish", "lb"},
            {"Macedonian", "mk"},
            {"Malagasy", "mg"},
            {"Malay", "ms"},
            {"Malayalam", "ml"},
            {"Maltese", "mt"},
            {"Maori", "mi"},
            {"Marathi", "mr"},
            {"Mongolian", "mn"},
            {"Myanmar (Burmese)", "my"},
            {"Nepali", "ne"},
            {"Norwegian", "no"},
            {"Nyanja (Chichewa)", "ny"},
            {"Pashto", "ps"},
            {"Persian", "fa"},
            {"Polish", "pl"},
            {"Portuguese (Portugal, Brazil)", "pt"},
            {"Punjabi", "pa"},
            {"Romanian", "ro"},
            {"Russian", "ru"},
            {"Samoan", "sm"},
            {"Scots Gaelic", "gd"},
            {"Serbian", "sr"},
            {"Sesotho", "st"},
            {"Shona", "sn"},
            {"Sindhi", "sd"},
            {"Sinhala (Sinhalese)", "si"},
            {"Slovak", "sk"},
            {"Slovenian", "sl"},
            {"Somali", "so"},
            {"Spanish", "es"},
            {"Sundanese", "su"},
            {"Swahili", "sw"},
            {"Swedish", "sv"},
            {"Tagalog (Filipino)", "tl"},
            {"Tajik", "tg"},
            {"Tamil", "ta"},
            {"Telugu", "te"},
            {"Thai", "th"},
            {"Turkish", "tr"},
            {"Ukrainian", "uk"},
            {"Urdu", "ur"},
            {"Uzbek", "uz"},
            {"Vietnamese", "vi"},
            {"Welsh", "cy"},
            {"Xhosa", "xh"},
            {"Yiddish", "yi"},
            {"Yoruba", "yo"},
            {"Zulu", "zu"}
        };

        // Add listeners to the dropdowns' OnValueChanged events
        initialLanguageDropdown.onValueChanged.AddListener(OnInitialLanguageValueChanged);
        targetLanguageDropdown.onValueChanged.AddListener(OnTargetLanguageValueChanged);

        // Populate dropdowns with language options
        PopulateDropdowns();
    }

    private void PopulateDropdowns()
    {
        // Clear existing options
        initialLanguageDropdown.ClearOptions();
        targetLanguageDropdown.ClearOptions();

        // Create a new list of dropdown options
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        // Iterate through the language codes dictionary and add options to the list
        foreach (KeyValuePair<string, string> language in languageCodes)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(language.Key);
            options.Add(option);
        }

        // Add the options list to the dropdowns
        targetLanguageDropdown.AddOptions(options);
        options.Insert(0, new TMP_Dropdown.OptionData("Detect Language"));
        initialLanguageDropdown.AddOptions(options);
    }

    private void OnInitialLanguageValueChanged(int index)
    {
        string selectedLanguage = initialLanguageDropdown.options[index].text;

        // Disable the selected language in the target language dropdown
        DisableLanguageOption(targetLanguageDropdown, selectedLanguage);
    }

    private void OnTargetLanguageValueChanged(int index)
    {
        string selectedLanguage = targetLanguageDropdown.options[index].text;

        // Disable the selected language in the initial language dropdown
        DisableLanguageOption(initialLanguageDropdown, selectedLanguage);
    }

    private void DisableLanguageOption(TMP_Dropdown dropdown, string language)
    {
        int selectedOptionIndex = dropdown.value;
        TMP_Dropdown.OptionData selectedOption = dropdown.options[selectedOptionIndex];

        // Check if the selected language matches the option text
        if (selectedOption.text == language)
        {
            // Swap the selected options
            SwapSelectedOptions();
        }

        // Refresh the dropdown to reflect the updated options
        dropdown.RefreshShownValue();
    }

    public void SwapSelectedOptions()
    {
        // Get the indices and values of the current selected options
        int initialIndex = initialLanguageDropdown.value;
        if (initialIndex == 0)
            return;
        int targetIndex = targetLanguageDropdown.value;

        // Swap the selected options between the dropdowns
        initialLanguageDropdown.value = targetIndex + 1;
        targetLanguageDropdown.value = initialIndex - 1;
    }

    public async Task<string> DetectLanguage(string text)
    {
        using (var client = new HttpClient())
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://google-translate1.p.rapidapi.com/language/translate/v2/detect"),
                Headers =
                {
                    { "X-RapidAPI-Key", "2b28b4bfa1msh866db562f6172f2p1d2042jsnc115e1b5cef4" },
                    { "X-RapidAPI-Host", "google-translate1.p.rapidapi.com" },
                },
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "q", text },
                }),
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return body;
            }
        }
    }

    public async Task<string> TranslateText(string text, string targetLanguage, string sourceLanguage)
    {
        using (var client = new HttpClient())
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://google-translate1.p.rapidapi.com/language/translate/v2"),
                Headers =
                {
                    { "X-RapidAPI-Key", "2b28b4bfa1msh866db562f6172f2p1d2042jsnc115e1b5cef4" },
                    { "X-RapidAPI-Host", "google-translate1.p.rapidapi.com" },
                },
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "q", text },
                    { "target", targetLanguage },
                    { "source", sourceLanguage },
                }),
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Debug.Log(body);

                // Parse the JSON string
                TranslationData translationData = JsonUtility.FromJson<TranslationData>(body);

                // Access the translatedText field
                string translatedText = translationData.data.translations[0].translatedText;

                // Output the result
                Debug.Log(translatedText);
                return translatedText;
            }
        }
    }


    public async void translateTextAsync()
    {
        string text = inputField.text;
        if (string.IsNullOrWhiteSpace(text))
            return;
        string sourceLanguage = "";
        string targetLanguage = languageCodes[targetLanguageDropdown.options[targetLanguageDropdown.value].text];

        int initialIndex = initialLanguageDropdown.value;
        if (initialIndex == 0)
        {
            string detectedLanguageJson = await DetectLanguage(inputField.text);

            // Parse the JSON response
            JObject jsonResponse = JObject.Parse(detectedLanguageJson);

            // Access the language value
            sourceLanguage = jsonResponse["data"]["detections"][0][0]["language"].ToString();
        } 
        else
        {
            sourceLanguage = languageCodes[initialLanguageDropdown.name];
        }
        output.text = await TranslateText(text, targetLanguage, sourceLanguage);
    }
}

[System.Serializable]
public class TranslationData
{
    public TranslationContainer data;
}

[System.Serializable]
public class TranslationContainer
{
    public Translation[] translations;
}

[System.Serializable]
public class Translation
{
    public string translatedText;
}