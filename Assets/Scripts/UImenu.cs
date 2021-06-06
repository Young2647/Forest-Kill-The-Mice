using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UImenu : MonoBehaviour
{
    LocalizationManager m_localizationManager;
    LocKey m_Lockey;

    UIManager m_UIManager;
    // Start is called before the first frame update
    void Start()
    {
        m_localizationManager = FindObjectOfType<LocalizationManager>();
        m_Lockey = FindObjectOfType<LocKey>();
        m_UIManager = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnStartClicked()
    {
        UIManager.m_Instance.PlayConfirmSound();

        Debug.Log("Start");
        SceneManager.LoadScene("GameScene");
    }

    public void OnOptionsClicked()
    {
        UIManager.m_Instance.PlayConfirmSound();

        Debug.Log("Options");
    }

    public void OnQuitClicked()
    {
        UIManager.m_Instance.PlayConfirmSound();

        Debug.Log("Quit");
        Application.Quit();
    }

    public void OnLanguageOptionsClicked()
    {
        UIManager.m_Instance.PlayConfirmSound();
        FindObjectOfType<LanguageLogic>().SetLanguage();
        if (m_localizationManager.m_currentLanguage == Language.English)
            m_localizationManager.m_currentLanguage = Language.Chinese;
        else
            m_localizationManager.m_currentLanguage = Language.English;
        m_localizationManager.Reset();
        m_Lockey.Update();
        Debug.Log("Language Options");
    }
}
