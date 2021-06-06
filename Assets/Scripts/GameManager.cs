using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public int miceNum = 50;
    
    [SerializeField]
    Text m_MiceUI;

    [SerializeField]
    Text m_winUI;
    // Start is called before the first frame update
    void Start()
    {
        UpdateWinUI(0);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMiceUI();
        if (miceNum == 0){
            UpdateWinUI(1);
        }

        if(Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.Joystick1Button12)){
            SceneManager.LoadScene("LoadGameScene");
        }
    }

    void UpdateMiceUI(){
        m_MiceUI.text = (50 - miceNum) + "/50";
    }

    void UpdateWinUI(int m){
        if (m == 1){
            m_winUI.gameObject.SetActive(true);
        }
        else{
            m_winUI.gameObject.SetActive(false);
        }
    }

}
