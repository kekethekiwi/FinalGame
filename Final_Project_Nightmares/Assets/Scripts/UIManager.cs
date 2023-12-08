using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text pauseText;
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
            pauseText.text = "Pause";
        }
        
    }

    // IGNORE THIS
    //pauseText.color = new Color(222, 222, 222, 255); // hexadecimal #DEDEDE
}
