using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager saveManager;
    private static GameData currentData;
    private static string mainFilePath;

    private void Awake()
    {
        if (saveManager != null) Destroy(this.gameObject);
        saveManager = this;
        mainFilePath = Path.Combine(Application.persistentDataPath, "save.json");
        
    }

    public static void SetSaveSlot(int slot)
    {
        currentData.saveSlot = Mathf.Max(slot, 1);
    }
    public static int GetSaveSlot()
    {
        return currentData.saveSlot;
    }

    public static void SaveGame()
    {
        SaveData(currentData, mainFilePath);
    }

    private static void SaveData(GameData aData, string aPath)
    {
        //try
        //{
        //    string saveData = JsonUtility.ToJson(aData);
        //    File.WriteAllText(saveData, aPath);
        //}
        //catch (System.Exception e)
        //{
        //    Debug.LogError(e);
        //}

        try
        {
            using (StreamWriter writer = new StreamWriter(aPath))
            {
                string dataToWrite = JsonUtility.ToJson(aData);
                writer.Write(dataToWrite);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }

    }   

    public static void LoadGame()
    {
        GameData aData = LoadData(mainFilePath);
        if (aData != null)
        {
            currentData = aData;
        }
        else
        {
            currentData = new GameData();
        }
    }

    private static GameData LoadData(string aPath)
    {
        //try
        //{
        //    string saveData = File.ReadAllText(aPath);
        //    return JsonUtility.FromJson<GameData>(saveData);
            
        //}
        //catch (System.Exception e)
        //{
        //    Debug.LogError(e);
        //}
        //return null;

        try
        {
            if (System.IO.File.Exists(aPath))
            {
                using (StreamReader reader = new StreamReader(aPath))
                {
                    string dataToLoad = reader.ReadToEnd();
                    return JsonUtility.FromJson<GameData>(dataToLoad);

                }
            }
            else
            {
                Debug.Log("Error: No save file exists at:" + aPath);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        return null;
    }
}

[System.Serializable]
public class GameData
{
    public int saveSlot = 0;
}
