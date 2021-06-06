using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public enum Language
{
    English,
    Chinese
}

public class LocalizationManager : MonoBehaviour
{
    static LocalizationManager m_instance;
    public static LocalizationManager Get() => m_instance;

    [SerializeField]
    public Language m_currentLanguage = Language.English;

    Dictionary<Language, TextAsset> m_localizationFiles = new Dictionary<Language, TextAsset>();
    Dictionary<string, string> m_localizationData = new Dictionary<string, string>();

    void Start(){
        DontDestroyOnLoad(gameObject);
    }
    void Awake()
    {
        SetupLanguage();
        SetupSingleton();
        SetupLocalizationFiles();
        SetupLocalizationData();
    }

    void SetupLanguage(){
        GameObject m_gameObject = GameObject.Find("LanguageObject");
        m_currentLanguage = m_gameObject.GetComponent<LanguageLogic>().m_language;
    }
    void SetupSingleton()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void SetupLocalizationFiles()
    {
        foreach(Language language in Language.GetValues(typeof(Language)))
        {
            string textAssetPath = "Localization/" + language;
            TextAsset textAsset = (TextAsset)Resources.Load(textAssetPath);

            if(textAsset)
            {
                m_localizationFiles[language] = textAsset;
                Debug.Log("Added Text File for Language: " + language);
            }
            else
            {
                Debug.LogError("Could NOT add Text File for Language: " + language);
            }
        }
    }

    void SetupLocalizationData()
    {
        TextAsset textAsset;

        if(m_localizationFiles.ContainsKey(m_currentLanguage))
        {
            textAsset = m_localizationFiles[m_currentLanguage];
        }
        else
        {
            Debug.LogError("LocalizationFile DOES NOT exist for Language: " + m_currentLanguage);
            return;
        }

        // Load XML Document
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(textAsset.text);

        // Get All Elements called "Entry"
        XmlNodeList entryList = xmlDocument.GetElementsByTagName("Entry");

        string key;
        string value;
        foreach(XmlNode entry in entryList)
        {
            key = entry.FirstChild.InnerText;
            value = entry.LastChild.InnerText;

            // Key doesn't exist, add it to our dictionary
            if(!m_localizationData.ContainsKey(key))
            {
                m_localizationData[key] = value;
                Debug.Log("Added Key: " + key + " with Value: " + value);
            }
            else
            {
                Debug.LogError("Key already exists for: " + key);
            }
        }
    }

    public string GetLocalizedString(string key)
    {
        string localizedString = "";
        if(!m_localizationData.TryGetValue(key, out localizedString))
        {
            localizedString = "CANNOT FIND: " + key;
        }

        return localizedString;
    }

    public void Reset(){
        m_localizationData.Clear();
        SetupSingleton();
        SetupLocalizationFiles();
        SetupLocalizationData();
    }
}
