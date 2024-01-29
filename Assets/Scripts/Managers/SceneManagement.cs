using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneManagement : GameBehaviour
{
    public AudioSource audioSource;

    public string levelName;
    public string titleName;

    public AudioClip hoverSound;
    public AudioClip clickSound;
    public AudioClip beginLevelSound;
    public AudioClip changeLevelSound;

    public GameObject loadingScreen;
    public GameObject levelSelectScreen;
    public GameObject overworldScreen;

    public GameObject areYouSurePanel;

    AsyncOperation loadingOperation;

    [Header("Level Selection")]
    public int selectedLevel = 1;
    public TMP_Text levelTitle;
    public TMP_Text levelDescription;
    public TMP_Text waveNumberText;
    public TMP_Text enemySpawnText;
    public TMP_Text challengeRatingText;
    public TMP_Text highScoreText;
    public Image levelImage;
    public Sprite level1Sprite;
    public Sprite level2Sprite;
    public GameObject[] selectionBarLocations;
    public GameObject selectionBar;


    private void Start()
    {
        Time.timeScale = 1.0f;

    }

    public void CloseLevelSelectScreen()
    {
        levelSelectScreen.SetActive(false);
    }
    public void CloseOverworldScreen()
    {
        overworldScreen.SetActive(false);
        _SAVE.OverworldSave();   
    }
    public void OpenOverworldScreen()
    {
        _SAVE.Load();
        StartCoroutine(_OM.WaitForLoadGame());
        overworldScreen.SetActive(true);

    }
    public void OpenLevelSelectScreen()
    {
        levelSelectScreen.SetActive(true);
        StartCoroutine(WaitToUpdateLevelSelectUI());
    }
    public void LoadScene(string _sceneName)
    {
        loadingScreen.SetActive(true);
        loadingOperation = SceneManager.LoadSceneAsync(_sceneName);
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(titleName);
    }
    public void AreYouSureOpen()
    {
        areYouSurePanel.SetActive(true);
    }
    public void AreYouSureClose()
    {
        areYouSurePanel.SetActive(false);
    }

    //Quit Button
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnHoverSound()
    {
        audioSource.clip = hoverSound;
        audioSource.Play();
    }
    public void OnClickSound()
    {
        audioSource.clip = clickSound;
        audioSource.Play();
    }
    public void OnChangeLevelSound()
    {
        audioSource.clip = changeLevelSound;
        audioSource.Play();
    }
    public void OnBeginLevelSound()
    {
        audioSource.clip = beginLevelSound;
        audioSource.Play();
    }
    public void LevelOneButton()
    {
        selectionBar.SetActive(true);
        selectedLevel = 1;
        StartCoroutine(WaitToUpdateLevelSelectUI());
        selectionBar.transform.position = selectionBarLocations[0].transform.position;
    }
    public void LevelTwoButton()
    {
        selectionBar.SetActive(true);
        selectedLevel = 2;
        StartCoroutine(WaitToUpdateLevelSelectUI());
        selectionBar.transform.position = selectionBarLocations[1].transform.position;
    }
    IEnumerator WaitToUpdateLevelSelectUI()
    {
       
        yield return new WaitForEndOfFrame();
        if(selectedLevel== 1)
        {
            levelImage.sprite = level1Sprite;
            levelTitle.text = "Level 1 - Wormturn Road";
            levelDescription.text = "An isolated crossroads on boundry of the King's relm.\r\n<br>Survive for 10 waves to secure victory.";
            waveNumberText.text = "10";
            enemySpawnText.text = "3";
            challengeRatingText.text = "4/10";
            highScoreText.text = "High Score: " + _SAVE.level1HighScore.ToString();
        }
        if (selectedLevel == 2)
        {
            levelImage.sprite = level2Sprite;
            levelTitle.text = "Level 2 - Mistbourne Plains";
            levelDescription.text = "A natural land bridge on the Dunholm river. Open grasslands in the King's relm.\r\n<br>Survive for 10 waves to secure victory.";
            waveNumberText.text = "10";
            enemySpawnText.text = "15";
            challengeRatingText.text = "5/10";
            highScoreText.text = "High Score: " + _SAVE.level2HighScore.ToString();
        }
    }
    public void LoadSceneFromSelection()
    {
        LoadScene("Level"+selectedLevel.ToString());
    }
}
