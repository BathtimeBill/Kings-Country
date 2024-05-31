using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;


[Serializable]
public enum GamePreferences
{
    MusicVolume,
    GraphicsQuality,
}

public enum PrefGraphicsQuality
{
    Low,
    Medium,
    High,
}

//
// Game Save Objects -- SAVED
//
[Serializable]
public class LevelSaveObject
{
    public LevelID levelID;
    public int highScore = 0;
    public int playCount = 0;
    public bool completed;
}
[Serializable]
public class PerkSaveObject
{
    public PerkID perkID;
    public bool unlocked;
}

[Serializable]
public class PlayerSaveObject
{
    public string playerHandle = "Your Name!";
}

[Serializable]
public class PlayTimeObject
{
    public int hoursPlayed = 0;
    public int minutesPlayed = 0;
    public int secondsPlayed = 0;
    public float totalSeconds = 0;
}

[Serializable]
public class SaveDataObject : BGG.GameDataBase
{
    // Player info
    public PlayerSaveObject playerInfo = new PlayerSaveObject();
    public int maegen;
    // Level data
    public Dictionary<LevelID, LevelSaveObject> levels = new Dictionary<LevelID, LevelSaveObject>();
    public int levelsPlayed = 0;
    //Perks
    public Dictionary<PerkID, PerkSaveObject> perks = new Dictionary<PerkID, PerkSaveObject>();
    //Achievements TODO
    public List<string> achievements = new List<string>();
    // Times
    public PlayTimeObject playTime = new PlayTimeObject();

    // Data getters
    public LevelSaveObject GetLevelSaveData(LevelID _levelID)
    {
        if (levels.ContainsKey(_levelID))
            return levels[_levelID];
        return null;
    }
    public PerkSaveObject GetPerkSaveData(PerkID _perkID)
    {
        if (perks.ContainsKey(_perkID))
            return perks[_perkID];
        return null;
    }
}

//
// Data Behaviour
//
public class SaveManager : BGG.GameData
{
    public Settings settings;
    //[HideInInspector]
    public SaveDataObject save = new SaveDataObject();

    public DateTime timeofLastSave;

    static SaveManager _instance;
    public static SaveManager instance { get { return _instance; } }

    #region Core
    void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("GameSaveManager already instanced!");
            return;
        }
        _instance = this;
        if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);

        // Initialize preferences (not synched)
        //BV.Prefs.InitializeFloat(GamePreferences.MusicVolume, 1f);
        //BV.Prefs.InitializeInt(GamePreferences.GraphicsQuality, (int)PrefGraphicsQuality.High);

        // Load Game data
        save = LoadDataObject<SaveDataObject>();
        if (save == null)
        {
            Debug.Log("NEW GAME DATA");
            save = new SaveDataObject();

            // Initialize Game Data
            save.levels = new Dictionary<LevelID, LevelSaveObject>();
            for (int l = 0; l < _DATA.levelData.Count; l++)
            {
                LevelData ld = _DATA.levelData[l];
                LevelID levelID = ld.id;
                save.levels[levelID] = new LevelSaveObject
                {
                    levelID = ld.id,
                    highScore = 0,
                    playCount = 0,
                    completed = false
                };
            }

            save.perks = new Dictionary<PerkID, PerkSaveObject>();
            for(int p = 0; p < _DATA.perkData.Count; p++)
            {
                PerkData pd = _DATA.perkData[p];
                save.perks[pd.id] = new PerkSaveObject
                {
                    perkID = pd.id,
                    unlocked = false,
                };
            }

            save.playerInfo = new PlayerSaveObject();
            save.playTime = new PlayTimeObject();
        }

        timeofLastSave = DateTime.Now;
    }

    public override void SaveData()
    {
        // if (GameSettings.Instance.saveUnfinishedLevelsScore)
        //    SetCurrentTrackScore(ScoreManager.Instance.currentScore, false);
        SaveDataObject(save);
    }

    public override void DeleteData()
    {
        DeleteDataObject();
    }
    #endregion

    #region Player Stats
    //Maegen
    public void SetMaegen(int _maegen)
    {
        save.maegen = _maegen;
    }
    public void IncrementMaegen(int _maegen)
    {
        save.maegen += _maegen;
    }

    public int GetMaegen()
    {
        return save.maegen;
    }
    #endregion

    #region Level Specific
    public void SetLevelScore(LevelID _levelID, int score, bool completed)
    {
        save.levelsPlayed++;
        LevelSaveObject level = save.GetLevelSaveData(_levelID);
        if (level != null)
        {
            if (score > level.highScore)
                level.highScore = score;
            if (completed)
            {
                level.playCount++;
                level.completed = completed;
            }
            Debug.Log("GameData:: COMPLETED level [" + _levelID + "] highScore [" + level.highScore + "] playCount [" + level.playCount + "]");
        }
    }

    //
    // Scores
    public int GetLevelHighScore(LevelID _levelID)
    {
        LevelSaveObject track = save.GetLevelSaveData(_levelID);
        if (track == null) return 0;
        return track.highScore;
    }

    //
    // Play Count
    public int GetLevelPlayCount(LevelID _levelID)
    {
        LevelSaveObject track = save.GetLevelSaveData(_levelID);
        if (track == null) return 0;
        return track.playCount;
    }

    public int GetLevelCompleteCount()
    {
        int count = 0;
        foreach (LevelID levelID in Enum.GetValues(typeof(LevelID)))
        {
            LevelSaveObject lso = save.GetLevelSaveData(levelID);
            count += lso.completed ? 1 : 0;
        }
        return count;
    }
    #endregion

    #region Perks
    public void SetPerkStatus(PerkID _perkID, bool _unlocked)
    {
        PerkSaveObject perk = save.GetPerkSaveData(_perkID);
        if (perk != null)
        {
            perk.unlocked = _unlocked;
            SaveData();
        }
    }
    public bool GetPerkStatus(PerkID _perkID)
    {
        PerkSaveObject perk = save.GetPerkSaveData(_perkID);
        if(perk == null) return false;
        return perk.unlocked;
    }

    #endregion

    #region Overall game progress
    public void GetElapsedTime()
    {
        DateTime now = DateTime.Now;
        int seconds = (now - timeofLastSave).Seconds;
        save.playTime.totalSeconds += seconds;
        save.playTime.hoursPlayed = GetHours();
        save.playTime.minutesPlayed = GetMinutes();
        save.playTime.secondsPlayed = GetSeconds();
        //Debug.Log(GetHours() + " Hours, " + GetMinutes() + " Minutes, "+ GetSeconds() + " Seconds");
        timeofLastSave = DateTime.Now;
    }
    public void SaveTimePlayed()
    {
        GetElapsedTime();
    }
    int GetSeconds()
    {
        return Mathf.FloorToInt(save.playTime.totalSeconds % 60);
    }
    int GetMinutes()
    {
        return Mathf.FloorToInt(save.playTime.totalSeconds / 60);
    }
    int GetHours()
    {
        return Mathf.FloorToInt(save.playTime.totalSeconds / 3600);
    }

    #endregion

    #region Achievements
    //TODO Once implemented
    /*
    public void AchievementChallenge(AchievementID _achievement)
    {
        if (_achievement != null && _achievement.achievementID != "")
        {
            string ID = _achievement.achievementID;
            if (!save.achievements.Contains(ID))
            {
                //Debug.Log("Achievement completed [" + ID + "]");
                save.achievements.Add(ID);
            }
        }
        SaveData();
    }
    public bool IsAchievementCompleted(AchievementID _achievement)
    {
        if (_achievement != null)
        {
            string ID = _achievement.achievementID;
            return save.achievements.Contains(ID);
        }
        return false;
    }*/
    #endregion


    //
    // Events
    //
    void OnLevelWin(LevelID _levelID, int _score, int _maegen)
    {
        IncrementMaegen(_maegen);
        SetLevelScore(_levelID, _score, true);
        SaveTimePlayed();
        SaveData();
    }
    void OnEnable()
    {
        GameEvents.OnLevelWin += OnLevelWin;
    }


    void OnDisable()
    {
        GameEvents.OnLevelWin -= OnLevelWin;
    }

    #region Editor
#if UNITY_EDITOR

    [MenuItem("BGG/Delete Game Data")]
    static void ResetPlayerPrefs()
    {
        if (EditorUtility.DisplayDialog("Delete Game Data", "Are you sure you want to delete Data?", "Yes", "No"))
        {
            instance.DeleteData();
        }
    }

    [CustomEditor(typeof(SaveManager))]
    public class SaveManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SaveManager gameSaveManager = (SaveManager)target;
            GUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Delete Data"))
            {
                if (EditorUtility.DisplayDialog("Delete Game Data", "Are you sure you want to delete Data?", "Yes", "No"))
                {
                    gameSaveManager.DeleteData();
                    EditorUtility.SetDirty(gameSaveManager);
                }
            }
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Save Data"))
            {
                gameSaveManager.SaveData();
                EditorUtility.SetDirty(gameSaveManager);
            }
            GUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;
            DrawDefaultInspector();
        }
    }
#endif
    #endregion
}


/*
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
                hasCompletedLVL2 = false,
                LVL3Score = 0,
                LVL3HighScore = 0,
                hasCompletedLVL3 = false,
                LVL4Score = 0,
                LVL4HighScore = 0,
                hasCompletedLVL4 = false,
                LVL5Score = 0,
                LVL5HighScore = 0,
                hasCompletedLVL5 = false,
                overworldMaegen = 0,
                overworldMaegenTotal = 0,
                hasComeFromWin = false,
                firstPlay = false,
                firstWave = false,
                firstMine = false,
                firstLord = false,
                firstLevel2 = false,
                satyrPerk = false,
                orcusPerk = false,
                leshyPerk = false,
                willowPerk = false,
                skessaPerk = false,
                goblinPerk = false,
                fidhainPerk = false,
                oakPerk = false,
                huldraPerk = false,
                golemPerk = false,
                explosiveTreePerk = false,
                homeTreePerk = false,
                runePerk = false,
                fyrePerk = false,
                bearPerk = false,

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
*/