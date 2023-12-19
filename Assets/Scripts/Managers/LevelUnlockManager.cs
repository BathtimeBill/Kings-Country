using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUnlockManager : GameBehaviour
{
    public Button[] levelButtons;
    public TMP_Text highScoreText;
    public int level1HighScore;
    public int level2HighScore;

    private void Start()
    {
        StartCoroutine(WaitForLoadGame());

    }

    IEnumerator WaitForLoadGame()
    {
        yield return new WaitForSeconds(0.1f);
        _SAVE.Load();

        for (int i = 0; i < _SAVE.levelsUnlocked; i++)
        {
            levelButtons[i].interactable = true;
        }
        yield return new WaitForEndOfFrame();
        level1HighScore = _SAVE.level1Score;
        level2HighScore = _SAVE.level2Score;
        highScoreText.text = level1HighScore.ToString();
    }
}
