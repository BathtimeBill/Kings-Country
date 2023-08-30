using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SaveData : Singleton<SaveData>
{
    public int levelsUnlocked;
    public int score;
    public int highScore;
    public bool lvl1Complete;

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

    public void Save()
    {
        if (lvl1Complete == false)
        {
            levelsUnlocked += 1;

            lvl1Complete = true;
        }
        saveManager.SaveGameData(levelsUnlocked, score, highScore, lvl1Complete);
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
            highScore = loadedData.LVL1HighScore;
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
        score = _SCORE.score;
        if (score > highScore)
        {
            highScore = score;

        }
        else
        {
            highScore = _SAVE.highScore;
        }
        
        _SCORE.highScoreText.text = highScore.ToString();
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
