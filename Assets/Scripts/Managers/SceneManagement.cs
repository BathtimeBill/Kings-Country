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

    public GameObject loadingScreen;
    public GameObject levelSelectScreen;

    public GameObject areYouSurePanel;

    AsyncOperation loadingOperation;


    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    public void CloseLevelSelectScreen()
    {
        levelSelectScreen.SetActive(false);
    }
    public void OpenLevelSelectScreen()
    {
        levelSelectScreen.SetActive(true);
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


}
