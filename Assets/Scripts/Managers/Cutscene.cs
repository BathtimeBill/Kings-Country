using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using Unity.VisualScripting;

public class Cutscene : GameBehaviour
{
    public AudioSource audioSource;

    public string levelName;
    public string titleName;

    public AudioClip hoverSound;
    public AudioClip clickSound;

    public VideoPlayer player;

    public GameObject loadingScreen;
    public GameObject skipButton;

    AsyncOperation loadingOperation;

    private void Update()
    {
        if(Input.anyKeyDown)
        {
            skipButton.SetActive(true);
        }
    }

    public void LoadSceneFirst(string _sceneName)
    {
        loadingScreen.SetActive(true);
        player.Stop();
        loadingOperation = SceneManager.LoadSceneAsync(_sceneName);
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(titleName);
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
