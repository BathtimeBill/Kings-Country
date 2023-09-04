using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [Header("Score")]
    public int score;
    public int highScore;
    [Header("Final Score Panel")]
    public TMP_Text maegenText;
    public TMP_Text treesText;
    public TMP_Text wildlifeText;
    public TMP_Text finalScoreText;
    public TMP_Text highScoreText;
    public GameObject finalScorePanel;
    

    public int CalculateScore()
    {
        int maegen = _GM.maegen;
        int wildlife = _GM.wildlife * 2;
        int trees = 1;

        if(_GM.trees.Count > 0 && _GM.trees.Count < 10)
        {
            trees = 1;
        }
        if (_GM.trees.Count > 9 && _GM.trees.Count < 20)
        {
            trees = 1;
        }
        if (_GM.trees.Count > 19 && _GM.trees.Count < 30)
        {
            trees = 2;
        }
        if (_GM.trees.Count > 29 && _GM.trees.Count < 40)
        {
            trees = 3;
        }
        if(_GM.trees.Count == 40)
        {
            trees = 4;
        }



        int finalScore = maegen + wildlife * trees;
        print("Final Score: " + finalScore);

        
        return finalScore;

    }
    private void UpdateUI()
    {
        int trees = 1;
        if (_GM.trees.Count > 0 && _GM.trees.Count < 10)
        {
            trees = 1;
        }
        if (_GM.trees.Count > 9 && _GM.trees.Count < 20)
        {
            trees = 1;
        }
        if (_GM.trees.Count > 19 && _GM.trees.Count < 30)
        {
            trees = 2;
        }
        if (_GM.trees.Count > 29 && _GM.trees.Count < 40)
        {
            trees = 3;
        }
        if (_GM.trees.Count == 40)
        {
            trees = 4;
        }
        maegenText.text = "+ " + _GM.maegen.ToString();
        treesText.text = _GM.trees.Count.ToString() + " (x " + trees.ToString() + " to total score)";
        wildlifeText.text = "+ " + _GM.wildlife.ToString() + " x2 = " + _GM.wildlife * 2;
        finalScoreText.text = CalculateScore().ToString();

    }
    public void OpenFinalScorePanel()
    {
        finalScorePanel.SetActive(true);
        _UI.winPanel.SetActive(false);
    }

    void OnGameWin()
    {
        score = CalculateScore();
        UpdateUI();
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
