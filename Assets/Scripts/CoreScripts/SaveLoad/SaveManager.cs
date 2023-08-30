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
    }

    public void SaveGameData(int levelsUnlocked, int score, int highScore, bool lvl1)
    {
        PlayerProfile profile = new PlayerProfile
        {
            LVL1HighScore = highScore,
            LVL1Score = score,
            levelsUnlocked = levelsUnlocked,
            hasCompletedLVL1 = lvl1,
        };

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
