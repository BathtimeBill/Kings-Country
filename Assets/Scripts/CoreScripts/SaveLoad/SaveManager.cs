using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


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
    public string levelName;
    public int highScore = 0;
    public int playCount = 0;
    public bool completed;
    public bool unlocked;
}
[Serializable]
public class PerkSaveObject
{
    public PerkID perkID;
    public bool unlocked;
}

[Serializable]
public class UnitStats
{
    public string unitID;
    public int summonCount;
    public int mostDaysSurvived;     //total days survived
    public int totalDaysSurvived;
    public List<KillStat> killedBy;
    public List<KillStat> iHaveKilled;
}

[Serializable]
public class KillStat
{
    public string killedID;
    public int amount;
}

[Serializable]
public class PlayerSettings
{
    public string playerHandle = "Your Name!";
    public bool miniMapRotation = false;
    public bool miniMapShowing = true;
    public bool miniMapIcons = true;
    public bool unitOutlines = true;
    public bool unitHealthBars = false;
    public PanelColourID panelColour;
    [Header("Audio")]
    public float musicVolume = 1.0f;
    public float sfxVolume = 1.0f;
    public float movementSpeed = 1.0f;
    public float zoomSpeed = 1.0f;
    public float rotationSpeed = 1.0f;
}

[Serializable]
public class PlayerStats
{
    public int currentMaegen;
    public int totalMaegen;
    public int daysPlayed;
    public int daysWon;

    //public int treesPlanted;
    //public int treesLost;
    //public int willowsPlanted;
    //public int willowsLost;
    //public int ficusPlanted;
    //public int ficusLost;
}

[Serializable]
public class TreeStatsObject
{
    public int treesPlanted;
    public int treesLost;
    public int willowsPlanted;
    public int willowsLost;
    public int ficusPlanted;
    public int ficusLost;
}

[Serializable]
public class ToolStatsObject
{
    public int fyreUsed;
    public int stormerUsed;
}

[Serializable]
public class WildlifeStatsObject
{
    public int rabbitsSpawned;
    public int rabbitsKilled;
    public int deerSpawned;
    public int deerKilled;
    public int boarsSpawned;
    public int boarsKilled;
    public int bearsSpawned;
    public int bearsKilled;
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
public class GlossaryObject
{
    public bool tutorialComplete;
    public List<GlossaryID> glossaryIDs;
}

[Serializable]
public class ExperienceObject
{
    public int currentLevel;
    public int currentEXP;
}

[Serializable]
public class SaveDataObject : BGG.GameDataBase
{
    // Player info
    public PlayerSettings playerSettings = new PlayerSettings();
    //Player Stats
    public PlayerStats playerStats = new PlayerStats();
    // Times
    public PlayTimeObject playTime = new PlayTimeObject();
    //Tree Stats
    public TreeStatsObject treeStats = new TreeStatsObject();
    //Unit Stats
    public List<UnitStats> unitStats = new List<UnitStats>();
    //Tool Stats
    public ToolStatsObject toolStats = new ToolStatsObject();
    //Wildlife Stats
    public WildlifeStatsObject wildlifeStats = new WildlifeStatsObject();
    // Level data
    public List<LevelSaveObject> levels = new List<LevelSaveObject>();
    public int levelsPlayed = 0;
    //Perks
    public Dictionary<PerkID, PerkSaveObject> perks = new Dictionary<PerkID, PerkSaveObject>();
    //Achievements TODO
    public List<string> achievements = new List<string>();
    //Tutorial
    public GlossaryObject glossary = new GlossaryObject();
    //Experience
    public ExperienceObject experience = new ExperienceObject();

    // Data getters
    
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
    public GameData gameData;
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

        // Load Game data
        save = LoadDataObject<SaveDataObject>();
        if (save == null)
            InitialiseData();

        timeofLastSave = DateTime.Now;
    }

    private void InitialiseData()
    {
        //Debug.Log("RESETTING GAME DATA");
        save = new SaveDataObject();

        // Initialize Game Data
        save.levels = new List<LevelSaveObject>();
        for (int l = 0; l < gameData.levelData.Count; l++)
        {
            LevelData ld = gameData.levelData[l];
            LevelSaveObject lso = new LevelSaveObject
            {
                levelID = ld.id,
                levelName = EnumX.EnumNameFormatted(ld.id.ToString()),
                highScore = 0,
                playCount = 0,
                completed = false,
                unlocked = l == 0 ? true : false
            };
            save.levels.Add(lso);
        }

        save.perks = new Dictionary<PerkID, PerkSaveObject>();
        for (int p = 0; p < gameData.perkData.Count; p++)
        {
            PerkData pd = gameData.perkData[p];
            PerkSaveObject pso = new PerkSaveObject
            {
                perkID = pd.id,
                unlocked = false,
            };
            save.perks.Add(pso.perkID, pso);
        }

        save.unitStats = new List<UnitStats>();

        save.playerSettings = new PlayerSettings();
        save.playerSettings.musicVolume = 0.8f;
        save.playerSettings.sfxVolume = 0.8f;
        save.playerSettings.unitOutlines = true;
        save.playerSettings.unitHealthBars = true;
        save.playerSettings.miniMapShowing = true;
        save.playerSettings.miniMapIcons = true;
        save.playerSettings.miniMapRotation = true;
        save.playerSettings.panelColour = PanelColourID.Black;

        save.playerStats = new PlayerStats();

        save.treeStats = new TreeStatsObject();

        save.toolStats = new ToolStatsObject();

        save.wildlifeStats = new WildlifeStatsObject();

        save.playTime = new PlayTimeObject();

        save.glossary = new GlossaryObject();
        save.glossary.glossaryIDs = new List<GlossaryID>();

        save.experience = new ExperienceObject();
        save.experience.currentLevel = 1;
        save.experience.currentEXP = 0;

        timeofLastSave = DateTime.Now;
    }

    public override void SaveData()
    {
        SaveTimePlayed();
        SaveDataObject(save);
    }

    public override void DeleteData()
    {
        DeleteDataObject();
        InitialiseData();
    }
    #endregion

    #region Player Stats
    public PlayerStats GetPlayerStats => save.playerStats;
    //Maegen
    public void SetMaegen(int _maegen)
    {
        save.playerStats.currentMaegen = _maegen;
    }
    public void IncrementMaegen(int _maegen)
    {
        save.playerStats.currentMaegen += _maegen;
        save.playerStats.totalMaegen += _maegen;
    }
    public void DecrementMaegen(int _amount)
    {
        save.playerStats.currentMaegen -= _amount;
        //SaveData();
    }

    public int GetCurrentMaegen => save.playerStats.currentMaegen;
    public int GetTotalMaegen => save.playerStats.totalMaegen;

    //Days
    public int GetDaysPlayed => save.playerStats.daysPlayed;
    public int GetDaysWon => save.playerStats.daysWon;

    
    //Audio
    public void SetMusicVolume(float _volume)
    {
        save.playerSettings.musicVolume = _volume;
        //SaveData();
    }
    public float GetMusicVolume => save.playerSettings.musicVolume;
    public void SetSFXVolume(float _volume)
    {
        save.playerSettings.sfxVolume = _volume;
        //SaveData();
    }
    public float GetSFXVolume => save.playerSettings.sfxVolume;

    //Units
    public void SetUnitOutline(bool _outline)
    {
        save.playerSettings.unitOutlines = _outline;
        //SaveData();
    }
    public bool GetUnitOutline => save.playerSettings.unitOutlines;
    public void SetUnitHealthBars(bool _show)
    {
        save.playerSettings.unitHealthBars = _show;
        //SaveData();
    }
    public bool GetUnitHealthBars => save.playerSettings.unitHealthBars;

    //Mini Map
    public void SetMiniMapShow(bool _show)
    {
        save.playerSettings.miniMapShowing = _show;
        //SaveData() ;
    }
    public bool GetMiniMapShow => save.playerSettings.miniMapShowing;
    public void SetMiniMapIcons(bool _show)
    {
        save.playerSettings.miniMapIcons = _show;
        //SaveData();
    }
    public bool GetMiniMapIcons => save.playerSettings.miniMapIcons;
    public void SetMiniMapRotation(bool _rotation)
    {
        save.playerSettings.miniMapRotation = _rotation;
        //SaveData();
    }
    public bool GetMiniMapRotation => save.playerSettings.miniMapRotation;

    //Aesthetics
    public void SetPanelColour(PanelColourID _panelColour)
    {
        save.playerSettings.panelColour = _panelColour;
        //SaveData();
    }
    public PanelColourID GetPanelColour()
    {
        return save.playerSettings.panelColour;
    }
    #endregion

    #region Tutorial/Glossary
    public void SetTutorialComplete()
    {
        save.glossary.tutorialComplete = true;
        SaveData();
    }
    public bool GetTutorialStatus => save.glossary.tutorialComplete;
    public void SetGlossaryItemUnlocked(GlossaryID _id)
    {
        if (save.glossary.glossaryIDs.Contains(_id))
            return;

        save.glossary.glossaryIDs.Add(_id);
        //SaveData();
    }
    public bool GetGlossaryItemUnlocked(GlossaryID _id) => save.glossary.glossaryIDs.Contains(_id); 
    public void SetGlossaryItemsUnlocked(List<GlossaryID> _items)
    {
        save.glossary.glossaryIDs = _items;
        //SaveData();
    }
    public List<GlossaryID> GetGlossaryItemsUnlocked => save.glossary.glossaryIDs;

    #endregion

    #region Level Specific
    public LevelSaveObject GetLevelSaveData(LevelID _levelID)
    {
        return save.levels.Find(x => x.levelID == _levelID);
    }
    
    // Scores
    public int GetLevelHighScore(LevelID _levelID)
    {
        LevelSaveObject track = GetLevelSaveData(_levelID);
        if (track == null) return 0;
        return track.highScore;
    }

    // Play Count
    public int GetLevelPlayCount(LevelID _levelID)
    {
        LevelSaveObject track = GetLevelSaveData(_levelID);
        if (track == null) return 0;
        return track.playCount;
    }

    public int GetLevelCompleteCount()
    {
        int count = 0;
        foreach (LevelID levelID in Enum.GetValues(typeof(LevelID)))
        {
            LevelSaveObject lso = GetLevelSaveData(levelID);
            if(lso == null)
            {
                lso = new LevelSaveObject();
                lso.levelID = levelID;
            }
            count += lso.completed ? 1 : 0;
        }
        return count;
    }

    public bool GetLevelCompleted(LevelID _id) => GetLevelSaveData(_id).completed;

    public void SetLevelScore(LevelID _levelID, int _score, bool _completed)
    {
        save.levelsPlayed++;
        LevelSaveObject level = GetLevelSaveData(_levelID);
        if (level != null)
        {
            if (_score > level.highScore)
                level.highScore = _score;
            if (_completed)
            {
                level.playCount++;
                level.completed = _completed;
                level.unlocked = true;
                LevelSaveObject next = GetLevelSaveData(EnumX.Next(_levelID));
                next.unlocked = true;
            }
            Log("GameData:: COMPLETED level [" + _levelID + "] highScore [" + level.highScore + "] playCount [" + level.playCount + "]");
        }
    }
    #endregion

    #region Perks
    public void SetPerkStatus(PerkID _perkID, bool _unlocked)
    {
        PerkSaveObject perk = save.GetPerkSaveData(_perkID);
        if (perk != null)
        {
            perk.unlocked = _unlocked;
            //SaveData();
        }
    }
    public bool GetPerkStatus(PerkID _perkID)
    {
        PerkSaveObject perk = save.GetPerkSaveData(_perkID);
        if(perk == null) return false;
        return perk.unlocked;
    }

    #endregion

    #region Unit Stats
    public UnitStats GetUnitStats(string _unitID) => save.unitStats.Find(x => x.unitID == _unitID);

    public void AddUnitStats(UnitStats _unitStats) => save.unitStats.Add(_unitStats);
    public int GetKillCount(string _unitID, string _killedID)
    {
        UnitStats stat = GetUnitStats(_unitID);
        if (stat == null) return 0;
        int killAmount = stat.iHaveKilled.Find(x=>x.killedID == _killedID).amount;
        return killAmount;
    }
    public int GetCreatureKillCount(string _unitID)
    {
        UnitStats stat = GetUnitStats(_unitID);

        if (stat == null) return 0;
        if(stat.iHaveKilled == null || stat.iHaveKilled.Count == 0) return 0;

        int killAmount = 0;
        for(int i=0; i< stat.iHaveKilled.Count; i++)
        {
            killAmount += stat.iHaveKilled[i].amount;
        }
        return killAmount;
    }
    public int GetCreatureDeathCount(string _unitID)
    {
        UnitStats stat = GetUnitStats(_unitID);

        if (stat == null) return 0;
        if (stat.killedBy == null || stat.killedBy.Count == 0) return 0;

        int deathAmount = 0;
        for (int i = 0; i < stat.killedBy.Count; i++)
        {
            deathAmount += stat.killedBy[i].amount;
        }
        return deathAmount;
    }

    public int GetToolKillCount(ToolID _toolID)
    {
        int killCount = 0;
        for (int i = 0; i < save.unitStats.Count; i++)
        {
            if (save.unitStats[i].killedBy.Find(x => x.killedID == _toolID.ToString()) != null)
            {
                int count = save.unitStats[i].killedBy.Find(x => x.killedID == _toolID.ToString()).amount;
                killCount += count;
            }
        }
        return killCount;
    }

    public void OnCreatureKilled(string _creature, string _killer, int _daysSurvived)
    {
        FillDeathStat(_creature, _killer, _daysSurvived);
        FillKillStat(_killer, _creature, _daysSurvived);
    }

    public void OnHumanKilled(Enemy _enemy, string _creature)
    {
        FillDeathStat(_enemy.unitID.ToString(), _creature, 0);
        FillKillStat(_creature, _enemy.unitID.ToString(), 0);
    }

    private void FillDeathStat(string _unit, string _killer, int _daysSurvived)
    {
        //print(_creature + " was killed by " + _killer);
        UnitStats stat = GetUnitStats(_unit);
        if (stat == null)
        {
            stat = new UnitStats();
            stat.unitID = _unit;
            stat.killedBy = new List<KillStat>();
            AddUnitStats(stat);
        }
        List<KillStat> killStats = stat.killedBy;
        KillStat ks = killStats.Find(x => x.killedID == _killer);
        if (ks == null)
        {
            ks = new KillStat();
            ks.killedID = _killer;
            ks.amount = 0;
            killStats.Add(ks);
        }
        ks.amount += 1;

        if (_daysSurvived > stat.mostDaysSurvived)
            stat.mostDaysSurvived = _daysSurvived;
        stat.totalDaysSurvived += 1;
        //print(stat.unitID + " has been killed by " + ks.killedID + " " + ks.amount + " times");
        //SaveData();
    }

    public void FillKillStat(string _unit, string _killer, int _daysSurvived)
    {
        UnitStats stat = GetUnitStats(_unit);
        if (stat == null)
        {
            stat = new UnitStats();
            stat.unitID = _unit;
            stat.iHaveKilled = new List<KillStat>();
            AddUnitStats(stat);
        }
        List<KillStat> killStats = stat.iHaveKilled == null ? new List<KillStat>() : stat.iHaveKilled;
        KillStat ks = killStats.Find(x => x.killedID == _killer.ToString());
        if (ks == null)
        {
            ks = new KillStat();
            ks.killedID = _killer.ToString();
            ks.amount = 0;
            killStats.Add(ks);
        }
        ks.amount += 1;
        //print(stat.unitID + " has killed " + ks.killedID + " " + ks.amount + " times");
        //SaveData();
    }

    private void OnCreatureSpawned(CreatureID _id)
    {
        UnitStats stat = GetUnitStats(_id.ToString());
        if (stat == null)
        {
            stat = new UnitStats();
            stat.unitID = _id.ToString();
            stat.killedBy = new List<KillStat>();
            AddUnitStats(stat);
        }
        stat.summonCount += 1;
    }

    private void OnHumanSpawned(HumanID _id)
    {
        UnitStats stat = GetUnitStats(_id.ToString());
        if (stat == null)
        {
            stat = new UnitStats();
            stat.unitID = _id.ToString();
            stat.killedBy = new List<KillStat>();
            AddUnitStats(stat);
        }
        stat.summonCount += 1;
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
    public string GetElapsedTimeFormatted()
    {
        return $"{GetHours()}:{GetMinutes()}:{GetSeconds()}";
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

    #region Experience
    public ExperienceObject GetExperience() => save.experience;

    public void SetExperience(ExperienceObject _experience)
    {
        save.experience = _experience;
        SaveData();
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

    #region Trees
    public void OnTreePlaced(ToolID _id)
    {
        CheckIfNull(save.treeStats);
        if (_id == ToolID.Tree)
            save.treeStats.treesPlanted += 1;
        if (_id == ToolID.Willow)
            save.treeStats.willowsPlanted += 1;
        if (_id == ToolID.Ficus)
            save.treeStats.ficusPlanted += 1;
    }
    private void OnTreeDestroyed(ToolID _id)
    {
        CheckIfNull(save.treeStats);
        if (_id == ToolID.Tree)
            save.treeStats.treesLost += 1;
        if (_id == ToolID.Willow)
            save.treeStats.willowsLost += 1;
        if (_id == ToolID.Ficus)
            save.treeStats.ficusLost += 1;
    }
    public int GetTreePlantedStats(ToolID _id)
    {
        CheckIfNull(save.toolStats);
        if (_id == ToolID.Tree)
            return save.treeStats.treesPlanted;
        else if (_id == ToolID.Willow)
            return save.treeStats.willowsPlanted;
        else if (_id == ToolID.Ficus)
            return save.treeStats.ficusPlanted;
        else
            return 0;
    }
    public int GetTreeLostStats(ToolID _id)
    {
        CheckIfNull(save.toolStats);
        if (_id == ToolID.Tree)
            return save.treeStats.treesLost;
        else if (_id == ToolID.Willow)
            return save.treeStats.willowsLost;
        else if (_id == ToolID.Ficus)
            return save.treeStats.ficusLost;
        else
            return 0;
    }
    private void CheckIfNull(TreeStatsObject tso)
    {
        if (tso == null) tso = new TreeStatsObject();
    }
    #endregion

    #region Wildlife
    private void OnWildlifeKilled()
    {
        CheckIfNull(save.wildlifeStats);
        save.wildlifeStats.rabbitsKilled += 1;
    }

    public int GetWildlifeSpawnCount(WildlifeID _id)
    {
        CheckIfNull(save.wildlifeStats);
        if (_id == WildlifeID.Rabbit)
            return save.wildlifeStats.rabbitsSpawned;
        else if (_id == WildlifeID.Deer)
            return save.wildlifeStats.deerSpawned;
        else if (_id == WildlifeID.Boar)
            return save.wildlifeStats.boarsSpawned;
        else if (_id == WildlifeID.Bear)
            return save.wildlifeStats.bearsSpawned;
        else
            return 0;
    }
    public int GetWildlifeKilledCount(WildlifeID _id)
    {
        CheckIfNull(save.wildlifeStats);
        if (_id == WildlifeID.Rabbit)
            return save.wildlifeStats.rabbitsKilled;
        else if (_id == WildlifeID.Deer)
            return save.wildlifeStats.deerKilled;
        else if (_id == WildlifeID.Boar)
            return save.wildlifeStats.boarsKilled;
        else if (_id == WildlifeID.Bear)
            return save.wildlifeStats.bearsKilled;
        else
            return 0;
    }
    private void CheckIfNull(WildlifeStatsObject wso)
    {
        if (wso == null) wso = new WildlifeStatsObject();
    }
    #endregion

    #region Tools
    private void OnFyrePlaced()
    {
        CheckIfNull(save.toolStats);
        save.toolStats.fyreUsed += 1;
    }

    public int GetFyreUsed => save.toolStats.fyreUsed;
    private void OnStormerPlaced()
    {
        CheckIfNull(save.toolStats);
        save.toolStats.stormerUsed += 1;
    }
    public int GetStormerUsed => save.toolStats.stormerUsed;


    private void CheckIfNull(ToolStatsObject tso)
    {
        if (tso == null) tso = new ToolStatsObject();
    }
    #endregion


    #region Events
    private void OnLevelWin(LevelID _levelID, int _score, int _maegen)
    {
        IncrementMaegen(_maegen);
        SetLevelScore(_levelID, _score, true);
        SaveData();
    }

    private void OnDayBegin()
    {
        save.playerStats.daysPlayed += 1;
    }

    private void OnDayOver()
    {
        save.playerStats.daysWon += 1;
    }


    private void OnEnable()
    {
        GameEvents.OnLevelWin += OnLevelWin;
        GameEvents.OnDayBegin += OnDayBegin;
        GameEvents.OnDayOver += OnDayOver;
        GameEvents.OnUnitOutlines += SetUnitOutline;
        GameEvents.OnUnitHealthBars += SetUnitHealthBars;
        GameEvents.OnMiniMapShow += SetMiniMapShow;
        GameEvents.OnMiniMapIcons += SetMiniMapIcons;
        GameEvents.OnMiniMapRotation += SetMiniMapRotation;

        GameEvents.OnCreatureKilled += OnCreatureKilled;
        GameEvents.OnHumanKilled += OnHumanKilled;
        GameEvents.OnHumanSpawned += OnHumanSpawned;
        GameEvents.OnCreatureSpawned += OnCreatureSpawned;

        GameEvents.OnTreePlaced += OnTreePlaced;
        GameEvents.OnTreeDestroyed += OnTreeDestroyed;
        GameEvents.OnFyrePlaced += OnFyrePlaced;
        GameEvents.OnStormerPlaced += OnStormerPlaced;
        GameEvents.OnWildlifeKilled += OnWildlifeKilled;
    }

    

    private void OnDisable()
    {
        GameEvents.OnLevelWin -= OnLevelWin;
        GameEvents.OnDayBegin -= OnDayBegin;
        GameEvents.OnDayOver -= OnDayOver;
        GameEvents.OnUnitOutlines -= SetUnitOutline;
        GameEvents.OnUnitHealthBars -= SetUnitHealthBars;
        GameEvents.OnMiniMapShow -= SetMiniMapShow;
        GameEvents.OnMiniMapIcons -= SetMiniMapIcons;
        GameEvents.OnMiniMapRotation -= SetMiniMapRotation;

        GameEvents.OnCreatureKilled -= OnCreatureKilled;
        GameEvents.OnHumanKilled -= OnHumanKilled;
        GameEvents.OnHumanSpawned -= OnHumanSpawned;
        GameEvents.OnCreatureSpawned -= OnCreatureSpawned;

        GameEvents.OnTreePlaced -= OnTreePlaced;
        GameEvents.OnTreeDestroyed -= OnTreeDestroyed;
        GameEvents.OnFyrePlaced -= OnFyrePlaced;
        GameEvents.OnStormerPlaced -= OnStormerPlaced;
        GameEvents.OnWildlifeKilled -= OnWildlifeKilled;
    }

    #endregion

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