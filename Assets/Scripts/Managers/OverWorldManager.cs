using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OverWorldManager : Singleton<OverWorldManager>
{
    public TMP_Text maegenText;
    public int overWorldMaegen;
    public int overWorldMaegenTotal;
    public TMP_Text highScoreText;
    public TMP_Text levelTitleText;
    public TMP_Text waveNumberText;
    public TMP_Text enemySpawnNumberText;
    public TMP_Text challengeText;
    public Image levelScreenshot;
    public GameObject levelDescriptionPanel;
    public bool hasComeFromWin;



    protected override void Awake()
    {
        Setup();
    }
    //private void Start()
    //{

    //    CheckScene();
    //}

    public IEnumerator WaitForLoadGame()
    {
        yield return new WaitForEndOfFrame();
        //_SAVE.Load();
        yield return new WaitForEndOfFrame();
        //overWorldMaegen = _SAVE.overworldMaegen;
       // overWorldMaegenTotal = _SAVE.overworldMaegenTotal;
        maegenText.text = overWorldMaegenTotal.ToString();
    }

    public void CheckScene()
    {
        StartCoroutine(WaitForLoadGame());
        Setup();
    }


    private void Setup()
    {
        TMP_Text[] textObjects = GameObject.FindObjectsOfType<TMP_Text>(true);

        foreach ( TMP_Text textObject in textObjects )
        {
            if(textObject.tag == "HighScoreText")
            {
                highScoreText = textObject;
            }
            if (textObject.tag == "LevelTitleText")
            {
                levelTitleText = textObject;
            }
            if (textObject.tag == "WaveNumberText")
            {
                waveNumberText = textObject;
            }
            if (textObject.tag == "EnemySpawnNumberText")
            {
                enemySpawnNumberText = textObject;
            }
            if (textObject.tag == "ChallengeText")
            {
                challengeText = textObject;
            }
            if (textObject.tag == "MaegenText")
            {
                maegenText = textObject;
            }
            Image[] images = GameObject.FindObjectsOfType<Image>(true);
            foreach ( Image image in images )
            {
                if(image.tag == "LevelScreenshot")
                {
                    levelScreenshot = image;
                }
            }
            GameObject[] contents = GameObject.FindObjectsOfType<GameObject>(true);
            foreach (GameObject go in contents)
            {
                if(go.tag == "Contents")
                {
                    levelDescriptionPanel = go;
                }
            }
        }


        //highScoreText = GameObject.FindGameObjectWithTag("HighScoreText").GetComponent<TMP_Text>();
        //levelTitleText = GameObject.FindGameObjectWithTag("LevelTitleText").GetComponent<TMP_Text>();
        //waveNumberText = GameObject.FindGameObjectWithTag("WaveNumberText").GetComponent<TMP_Text>();
        //enemySpawnNumberText = GameObject.FindGameObjectWithTag("EnemySpawnNumberText").GetComponent<TMP_Text>();
        //challengeText = GameObject.FindGameObjectWithTag("ChallengeText").GetComponent<TMP_Text>();
        //levelScreenshot = GameObject.FindGameObjectWithTag("LevelScreenshot").GetComponent<Image>();
        //levelDescriptionPanel = GameObject.FindGameObjectWithTag("Contents");
}
    private void OnLevelWin(LevelID _levelID, int _score, int _maegen)
    {
        overWorldMaegen = _GM.maegen;
        overWorldMaegenTotal += overWorldMaegen;
        hasComeFromWin = true;
        //_SAVE.OverworldSave();
    }
    private void OnEnable()
    {
        GameEvents.OnLevelWin += OnLevelWin;
    }

    private void OnDisable()
    {
        GameEvents.OnLevelWin -= OnLevelWin;
    }
}
