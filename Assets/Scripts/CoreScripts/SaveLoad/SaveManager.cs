using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour 
{
    private string savePath;
    private void Start()
    {
        savePath = Application.persistentDataPath + "/saves";
        if(!File.Exists(savePath))
        {
            Debug.Log("Save file not found. Creating a new one with default data.");
            PlayerProfile defaultData = new PlayerProfile
            {
            levelsUnlocked = 1,
            LVL1Score = 0,
            LVL1HighScore = 0,
            hasCompletedLVL1 = false,
            LVL2Score = 0,
            LVL2HighScore = 0,
            hasCompletedLVL2 = false
            };
            SaveGameData(defaultData);
        }
        else
        {
            Debug.Log("Save file found.");
        }
    }

    public void SaveGameData(PlayerProfile profile)
    {
        string jsonData = JsonUtility.ToJson(profile);
        File.WriteAllText(savePath, jsonData);  
    }

    public PlayerProfile LoadGameData()
    {
        if(File.Exists(savePath))
        {
            string jsonData = File.ReadAllText(savePath);
            PlayerProfile profile = JsonUtility.FromJson<PlayerProfile>(jsonData);
            return profile;
        }
        else
        {
            Debug.LogWarning("No saved data found.");
            return null;
        }
    }
}
