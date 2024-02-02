using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUnlockManager : GameBehaviour
{
    public Button[] levelButtons;

    public GameObject overworldPanel;
    public GameObject messagePanel;
    public TMP_Text messageText;
    public int level1HighScore;
    public int level2HighScore;
    public int level3HighScore;
    public int level4HighScore;
    public int level5HighScore;



    private void Start()
    {
        StartCoroutine(WaitForLoadGame());
        _OM.CheckScene();
    }
    public void CloseMessagePanel()
    {
        messagePanel.SetActive(false);
    }

    IEnumerator WaitForLoadGame()
    {
        yield return new WaitForSeconds(0.1f);
        _SAVE.Load();

        if(_SAVE.hasComeFromWin == true)
        {
            messagePanel.SetActive(true);
            overworldPanel.SetActive(true);
            messageText.text = "You recieved " + _SAVE.overworldMaegen.ToString() + " maegen!";
        }

        for (int i = 0; i < _SAVE.levelsUnlocked; i++)
        {
            levelButtons[i].interactable = true;
        }
        yield return new WaitForEndOfFrame();
        level1HighScore = _SAVE.level1Score;
        level2HighScore = _SAVE.level2Score;
        level3HighScore = _SAVE.level3Score;
        level4HighScore = _SAVE.level4Score;
        level5HighScore = _SAVE.level5Score;
        _OM.hasComeFromWin = false;
    }
}
