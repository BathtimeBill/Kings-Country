using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverworldLevelButton : GameBehaviour
{
    public LevelButton levelButton;
    public float alphaThreshold = 0.1f;
    public GameObject lockedImage;
    public Image buttonImage;
    public Sprite unlockedImage;
    public Sprite highlightImage;
    public Sprite levelCompleteHighlightImage;
    public Sprite levelImage;
    public string message;
    public string title;
    public string waveNumber;
    public string enemySpawnNumber;
    public string challengeText;


    private void Start()
    {
        gameObject.GetComponent<Image>().alphaHitTestMinimumThreshold = alphaThreshold;
        StartCoroutine(WaitToCheckForButtonStatus());
    }
    IEnumerator WaitToCheckForButtonStatus()
    {
        yield return new WaitForSeconds(0.11f);
        CheckButtonStatus();
    }
    public void MouseOverButton()
    {
        if(gameObject.GetComponent<Button>().interactable)
        {
            _Tool.SetAndShowTooltip(message, title);
            _Tool.SetLevelImage(levelImage);
            _SCENE.OnHoverSound();
        }
    }
    public void MouseOffButton()
    {
        _Tool.HideTooltip();
    }

    private void CheckButtonStatus()
    {
        switch (levelButton)
        {
            case LevelButton.One:
                //if (_SAVE.levelsUnlocked >= 1)
                //{
                //    lockedImage.SetActive(false);

                //}
                //if(_SAVE.lvl1Complete)
                //{
                //    buttonImage.sprite = unlockedImage;
                //}
                break;
        }
        switch (levelButton)
        {
            case LevelButton.Two:
                //if(_SAVE.levelsUnlocked >= 2)
                //{
                //    lockedImage.SetActive(false);
                //    //buttonImage.sprite = unlockedImage;
                //}
                //if (_SAVE.lvl2Complete)
                //{
                //    buttonImage.sprite = unlockedImage;
                //}

                break;
        }
        switch (levelButton)
        {
            case LevelButton.Three:
                //if (_SAVE.levelsUnlocked >= 3)
                //{
                //    lockedImage.SetActive(false);
                //    //buttonImage.sprite = unlockedImage;
                //}
                //if (_SAVE.lvl3Complete)
                //{
                //    buttonImage.sprite = unlockedImage;
                //}

                break;
        }
        switch (levelButton)
        {
            case LevelButton.Four:
                //if (_SAVE.levelsUnlocked >= 4)
                //{
                //    lockedImage.SetActive(false);
                //    //buttonImage.sprite = unlockedImage;
                //}
                //if (_SAVE.lvl4Complete)
                //{
                //    buttonImage.sprite = unlockedImage;
                //}

                break;
        }
        switch (levelButton)
        {
            case LevelButton.Five:

                //if (_SAVE.levelsUnlocked >= 5)
                //{
                //    lockedImage.SetActive(false);
                //    //buttonImage.sprite = unlockedImage;
                //}
                //if (_SAVE.lvl5Complete)
                //{
                //    buttonImage.sprite = unlockedImage;
                //}
                break;
        }
    }

    public void OnButtonPressed()
    {
        _OM.levelTitleText.text = title;
        _OM.levelScreenshot.sprite = levelImage;
        _OM.waveNumberText.text = waveNumber;
        _OM.enemySpawnNumberText.text = enemySpawnNumber;
        _OM.challengeText.text = challengeText;
        _OM.levelDescriptionPanel.SetActive(true);

        switch (levelButton)
        {
            case LevelButton.One:
                //_OM.highScoreText.text = _SAVE.level1HighScore.ToString();
                _SCENE.selectedLevel = 1;
                break;
            case LevelButton.Two:
                //_OM.highScoreText.text = _SAVE.level2HighScore.ToString();
                _SCENE.selectedLevel = 2;
                break;
            case LevelButton.Three:
                //_OM.highScoreText.text = _SAVE.level3HighScore.ToString();
                _SCENE.selectedLevel = 3;
                break;
            case LevelButton.Four:
                //_OM.highScoreText.text = _SAVE.level4HighScore.ToString();
                _SCENE.selectedLevel = 4;
                break;
            case LevelButton.Five:
                //_OM.highScoreText.text = _SAVE.level5HighScore.ToString();
                _SCENE.selectedLevel = 5;
                break;
        }
    }
}
