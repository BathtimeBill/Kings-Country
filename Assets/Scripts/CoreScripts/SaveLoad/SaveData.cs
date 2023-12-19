using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SaveData : Singleton<SaveData>
{
    public int levelsUnlocked;
    public int level1HighScore;
    public int level1Score;
    public int level2HighScore;
    public int level2Score;
    public bool lvl1Complete;
    public bool lvl2Complete;
    public SaveManager saveManager;


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
            hasCompletedLVL1 = lvl1Complete,
            hasCompletedLVL2 = lvl2Complete,
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
            level1HighScore = loadedData.LVL1HighScore;
            level2HighScore = loadedData.LVL2HighScore;
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


        if (_GM.level == LevelNumber.One)
        {
            _SCORE.highScoreText.text = level1HighScore.ToString();
        }
        if (_GM.level == LevelNumber.Two)
        {
            _SCORE.highScoreText.text = level2HighScore.ToString();
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
