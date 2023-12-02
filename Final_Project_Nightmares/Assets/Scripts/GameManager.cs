using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text displaySaveSlot;
    public static GameManager gameManager;
    private static int saveSlot;

    private void Awake()
    {
        if (gameManager != null) Destroy(this.gameObject);
        gameManager = this;
        saveSlot = SaveManager.GetSaveSlot();
        SaveManager.LoadGame();
    }

    // Start is called before the first frame update
    void Start()
    {
        SaveManager.SetSaveSlot(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        SaveManager.SaveGame();
    }
}
