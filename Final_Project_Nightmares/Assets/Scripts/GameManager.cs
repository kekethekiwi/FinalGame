using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TMP_Text displaySaveSlot;
    public static GameManager gameManager;
    private static int saveSlot;
    public static float musicVol;
    public static float sfxVol;
    public ShakeCamera shakeCamera;
    [SerializeField] private GameObject menu;

    private bool toggle = false;
    public static bool pause = false;

    private void Awake()
    {
        if (gameManager != null) Destroy(this.gameObject);
        gameManager = this;

        if (menu != null) menu.SetActive(false);


        SaveManager.LoadGame();
        saveSlot = SaveManager.GetSaveSlot();
        sfxVol = SaveManager.GetMusicVol();
        musicVol = SaveManager.GetSFXVol();
        if (displaySaveSlot != null) displaySaveSlot.text = "" + saveSlot;


    }

    // Start is called before the first frame update
    void Start()
    {
        SaveManager.SetSaveSlot(1);
    }

    // Update is called once per frame
    void Update()
    {
        ManageMenu();
    }

    private void ManageMenu()
    {
        if (Input.GetKeyDown(KeyCode.M) && !pause)
        {
            if (menu != null)
            {
                toggle = !toggle;
                menu.SetActive(toggle);
            }

        }
    }

    private void OnApplicationQuit()
    {
        SaveManager.SaveGame();
    }

    public static void ShakeTheCamera(float amt, float duration)
    {
        if (gameManager != null && !pause) gameManager.shakeCamera.ShakeTheCamera(amt, duration);
    }

    public void GameReset()
    {
        PlayerController.SetIsAlive(true);
        SceneManager.LoadScene(0);
       
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
