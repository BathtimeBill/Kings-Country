using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SaveData : Singleton<SaveData>
{
    public SaveManager saveManager;
    [Header("LevelScores")]
    public int levelsUnlocked;
    public int level1HighScore;
    public int level1Score;
    public int level2HighScore;
    public int level2Score;
    public bool lvl1Complete;
    public bool lvl2Complete;
    public int level3HighScore;
    public int level3Score;
    public bool lvl3Complete;
    public int level4HighScore;
    public int level4Score;
    public bool lvl4Complete;
    public int level5HighScore;
    public int level5Score;
    public bool lvl5Complete;
    [Header("Overworld")]
    public int overworldMaegen;
    public int overworldMaegenTotal;
    public bool hasComeFromWin;
    [Header("Perks")]
    public bool satyrPerk;
    public bool orcusPerk;
    public bool leshyPerk;
    public bool willowPerk;
    public bool skessaPerk;
    public bool goblinPerk;
    public bool fidhainPerk;
    public bool oakPerk;
    public bool huldraPerk;
    public bool golemPerk;
    public bool explosiveTreePerk;
    public bool homeTreePerk;
    public bool runePerk;
    public bool fyrePerk;
    public bool bearPerk;




    private void Start()
    {
        saveManager = FindObjectOfType<SaveManager>();
        
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.F5))
    //    {
    //        OnGameWin();
    //    }
    //    if (Input.GetKeyDown(KeyCode.F6))
    //    {
    //        Load();
    //    }
    //}
    private PlayerProfile SaveLevel()
    {
        PlayerProfile profile = new PlayerProfile
        {
            levelsUnlocked = levelsUnlocked,
            LVL1HighScore = level1HighScore,
            LVL2HighScore = level2HighScore,
            LVL3HighScore = level3HighScore,
            LVL4HighScore = level4HighScore,
            LVL5HighScore = level5HighScore,
            hasCompletedLVL1 = lvl1Complete,
            hasCompletedLVL2 = lvl2Complete,
            hasCompletedLVL3 = lvl3Complete,
            hasCompletedLVL4 = lvl4Complete,
            hasCompletedLVL5 = lvl5Complete,
            overworldMaegen = _OM.overWorldMaegen,
            overworldMaegenTotal = _OM.overWorldMaegenTotal,
            hasComeFromWin = hasComeFromWin,
            firstPlay = _TUTM.firstPlay,
            firstWave = _TUTM.firstWave,
            firstMine = _TUTM.firstMine,
            firstLord = _TUTM.firstLord,
            firstLevel2 = _TUTM.firstLevel2,
            satyrPerk = _PERK.satyrPerk,
            orcusPerk = _PERK.orcusPerk,
            leshyPerk = _PERK.leshyPerk,
            willowPerk = _PERK.willowPerk,
            skessaPerk = _PERK.skessaPerk,
            goblinPerk = _PERK.goblinPerk,
            fidhainPerk = _PERK.fidhainPerk,
            oakPerk = _PERK.oakPerk,
            huldraPerk = _PERK.huldraPerk,
            golemPerk = _PERK.golemPerk,
            explosiveTreePerk = _PERK.explosiveTreePerk,
            homeTreePerk = _PERK.homeTreePerk,
            runePerk = _PERK.runePerk,
            fyrePerk = _PERK.fyrePerk,
            bearPerk = _PERK.bearPerk,
        };
        return profile;
    }
    public void Save()
    {
        if(_GM.level == LevelNumber.One)
        {
            if (lvl1Complete == false)
            {
                levelsUnlocked += 1;

                lvl1Complete = true;
            }
        }
        if (_GM.level == LevelNumber.Two)
        {
            if (lvl2Complete == false)
            {
                levelsUnlocked += 1;

                lvl2Complete = true;
            }
        }
        if (_GM.level == LevelNumber.Three)
        {
            if (lvl3Complete == false)
            {
                levelsUnlocked += 1;

                lvl3Complete = true;
            }
        }
        if (_GM.level == LevelNumber.Four)
        {
            if (lvl4Complete == false)
            {
                levelsUnlocked += 1;

                lvl4Complete = true;
            }
        }
        if (_GM.level == LevelNumber.Five)
        {
            if (lvl5Complete == false)
            {
                levelsUnlocked += 1;

                lvl5Complete = true;
            }
        }

        saveManager.SaveGameData(SaveLevel());
    }
    public void OverworldSave()
    {
        overworldMaegen = _OM.overWorldMaegen;
        overworldMaegenTotal = _OM.overWorldMaegenTotal;
        hasComeFromWin = _OM.hasComeFromWin;
        satyrPerk = _PERK.satyrPerk;
        orcusPerk = _PERK.orcusPerk;
        leshyPerk = _PERK.leshyPerk;
        willowPerk = _PERK.willowPerk;
        skessaPerk = _PERK.skessaPerk;
        goblinPerk = _PERK.goblinPerk;
        fidhainPerk = _PERK.fidhainPerk;
        oakPerk = _PERK.oakPerk;
        huldraPerk = _PERK.huldraPerk;
        golemPerk = _PERK.golemPerk;
        explosiveTreePerk = _PERK.explosiveTreePerk;
        homeTreePerk = _PERK.homeTreePerk;
        runePerk = _PERK.runePerk;
        fyrePerk = _PERK.fyrePerk;
        bearPerk = _PERK.bearPerk;
        saveManager.SaveGameData(SaveLevel());
    }
    public void Load()
    {
        PlayerProfile loadedData = saveManager.LoadGameData();
        if(loadedData != null)
        {
            //Debug.Log("Loaded Score: " + loadedData.score);
            //Debug.Log("Levels Unlocked: " + loadedData.levelsUnlocked);
            levelsUnlocked = loadedData.levelsUnlocked;
            lvl1Complete = loadedData.hasCompletedLVL1;
            lvl2Complete = loadedData.hasCompletedLVL2;
            lvl3Complete = loadedData.hasCompletedLVL3;
            lvl4Complete = loadedData.hasCompletedLVL4;
            lvl5Complete = loadedData.hasCompletedLVL5;
            level1HighScore = loadedData.LVL1HighScore;
            level2HighScore = loadedData.LVL2HighScore;
            level3HighScore = loadedData.LVL3HighScore;
            level4HighScore = loadedData.LVL4HighScore;
            level5HighScore = loadedData.LVL5HighScore;
            overworldMaegen = loadedData.overworldMaegen;
            overworldMaegenTotal = loadedData.overworldMaegenTotal;
            hasComeFromWin = loadedData.hasComeFromWin;
            _TUTM.firstPlay = loadedData.firstPlay;
            _TUTM.firstWave = loadedData.firstWave;
            _TUTM.firstMine = loadedData.firstMine;
            _TUTM.firstLord = loadedData.firstLord;
            _TUTM.firstLevel2 = loadedData.firstLevel2;
            _TUTM.firstPlay = loadedData.firstPlay;
            _TUTM.firstWave = loadedData.firstWave;
            _TUTM.firstMine = loadedData.firstMine;
            _TUTM.firstLord = loadedData.firstLord;
            _TUTM.firstLevel2 = loadedData.firstLevel2;
            _PERK.satyrPerk = loadedData.satyrPerk;
            _PERK.orcusPerk = loadedData.orcusPerk;
            _PERK.leshyPerk = loadedData.leshyPerk;
            _PERK.willowPerk = loadedData.willowPerk;
            _PERK.skessaPerk = loadedData.skessaPerk;
            _PERK.goblinPerk = loadedData.goblinPerk;
            _PERK.fidhainPerk = loadedData.fidhainPerk;
            _PERK.oakPerk = loadedData.oakPerk;
            _PERK.huldraPerk = loadedData.huldraPerk;
            _PERK.golemPerk = loadedData.golemPerk;
            _PERK.explosiveTreePerk = loadedData.explosiveTreePerk;
            _PERK.homeTreePerk = loadedData.homeTreePerk;
            _PERK.runePerk = loadedData.runePerk;
            _PERK.fyrePerk = loadedData.fyrePerk;
            _PERK.bearPerk = loadedData.bearPerk;
        }
        else
        {
            Debug.Log("ERROR!");
        }
    }


    private void OnGameWin()
    {
        StartCoroutine(WaitToSaveScore());
        //score = _SCORE.score;
        //print(_SCORE.score);
        //Save();
    }
    IEnumerator WaitToSaveScore()
    {
        yield return new WaitForEndOfFrame();
        if(_GM.level == LevelNumber.One)
        {
            level1Score = _SCORE.score;
            if (level1Score > level1HighScore)
            {
                level1HighScore = level1Score;
            }
            else
            {
                level1HighScore = _SAVE.level1HighScore;
            }
        }
        if (_GM.level == LevelNumber.Two)
        {
            level2Score = _SCORE.score;
            if (level2Score > level2HighScore)
            {
                level2HighScore = level2Score;

            }
            else
            {
                level2HighScore = _SAVE.level2HighScore;
            }
        }
        if (_GM.level == LevelNumber.Three)
        {
            level3Score = _SCORE.score;
            if (level3Score > level3HighScore)
            {
                level3HighScore = level3Score;

            }
            else
            {
                level3HighScore = _SAVE.level3HighScore;
            }
        }
        if (_GM.level == LevelNumber.Four)
        {
            level4Score = _SCORE.score;
            if (level4Score > level4HighScore)
            {
                level4HighScore = level4Score;

            }
            else
            {
                level4HighScore = _SAVE.level4HighScore;
            }
        }
        if (_GM.level == LevelNumber.Five)
        {
            level5Score = _SCORE.score;
            if (level5Score > level5HighScore)
            {
                level5HighScore = level5Score;

            }
            else
            {
                level5HighScore = _SAVE.level5HighScore;
            }
        }


        if (_GM.level == LevelNumber.One)
        {
            _SCORE.highScoreText.text = level1HighScore.ToString();
        }
        if (_GM.level == LevelNumber.Two)
        {
            _SCORE.highScoreText.text = level2HighScore.ToString();
        }
        if (_GM.level == LevelNumber.Three)
        {
            _SCORE.highScoreText.text = level3HighScore.ToString();
        }
        if (_GM.level == LevelNumber.Four)
        {
            _SCORE.highScoreText.text = level4HighScore.ToString();
        }
        if (_GM.level == LevelNumber.Five)
        {
            _SCORE.highScoreText.text = level5HighScore.ToString();
        }

        print(_SCORE.score);
        yield return new WaitForEndOfFrame();
        Save();
    }
    private void OnEnable()
    {
        GameEvents.OnGameWin += OnGameWin;
    }
    private void OnDisable()
    {
        GameEvents.OnGameWin -= OnGameWin;
    }
}
