using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text pauseText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseGame()
    {
        if (pauseText.text == "Pause")
        {
            GameManager.pause = true;
            Time.timeScale = 0;
            pauseText.color = Color.black;
            pauseText.text = "Resume";
        }
        else
        {
            GameManager.pause = false;
            Time.timeScale = 1;
            //pauseText.color = new Color(222, 222, 222, 255); // hexadecimal #DEDEDE
            pauseText.text = "Pause";
        }
        
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
