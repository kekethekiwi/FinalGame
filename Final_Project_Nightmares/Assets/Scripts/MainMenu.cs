using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnStart()
    {
        StartCoroutine(LoadNextScene());
        // loading bar lerp
    }

    private IEnumerator LoadNextScene()
    {
        int currentSceneNumber = SceneManager.GetActiveScene().buildIndex;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(currentSceneNumber + 1);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }
}
