using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private TMP_Text percentText;
    [SerializeField] private Slider slider;
    void Start()
    {
        progressBar.SetActive(false);
        buttons.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void OnStart()
    {
        StartCoroutine(LoadNextScene());
        buttons.SetActive(false);
        progressBar.SetActive(true);
    }

    private IEnumerator LoadNextScene()
    {
        int currentSceneNumber = SceneManager.GetActiveScene().buildIndex;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(currentSceneNumber + 1);

        while (!asyncOperation.isDone)
        {
            UpdateUI(asyncOperation.progress);
            yield return null;
        }
    }

    private void UpdateUI(float progress)
    {
        slider.value = progress;
        percentText.text = (int)(progress * 100f) + "%";
    }
}
