using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocKey : MonoBehaviour
{
    [SerializeField]
    string m_locKey;

    Text m_text;

    // Start is called before the first frame update
    void Start()
    {
        string translatedString = LocalizationManager.Get().GetLocalizedString(m_locKey);

        m_text = GetComponent<Text>();
        if(m_text)
        {
            m_text.text = translatedString;
        }
    }

    public void Update(){
        string translatedString = LocalizationManager.Get().GetLocalizedString(m_locKey);

        m_text = GetComponent<Text>();
        if(m_text)
        {
            m_text.text = translatedString;
        }
    }
}
