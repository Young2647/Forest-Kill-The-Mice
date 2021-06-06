using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageLogic : MonoBehaviour
{
    public Language m_language;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        m_language = Language.English;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLanguage(){
        if (m_language == Language.English)
            m_language = Language.Chinese;
        else
            m_language = Language.English;
    }
}
