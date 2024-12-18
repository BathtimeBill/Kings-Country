using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class MusicManager : Singleton<MusicManager>
{
    public AudioSource musicPlayer;
    public List<AudioClip> combatMusic;
    public List<AudioClip> peaceMusic;
    public AudioClip winMusic;
    public AudioClip currentClip;

    void Start()
    {
        musicPlayer.clip = GetRandomPeaceMusic();
        musicPlayer.loop = false;
        musicPlayer.Play();
        StartCoroutine(WaitForPeaceTrackToEnd());
    }

    public AudioClip GetRandomCombatMusic()
    {
        int rnd = Random.Range(0, combatMusic.Count);
        currentClip = combatMusic[rnd];
        return combatMusic[rnd];
    }
    public AudioClip GetRandomPeaceMusic()
    {
        int rnd = Random.Range(0, peaceMusic.Count);
        currentClip = peaceMusic[rnd];
        return peaceMusic[rnd];
    }

    IEnumerator WaitForCombatTrackToEnd()
    {
        while (musicPlayer.isPlaying)
        {
            yield return new WaitForSeconds(Random.Range(5, 20));

        }
        combatMusic.Remove(currentClip);
        musicPlayer.clip = GetRandomCombatMusic();
        musicPlayer.Play();
        if (_EnemiesExist)
            StartCoroutine(WaitForCombatTrackToEnd());
        else
            StartCoroutine(WaitForPeaceTrackToEnd());
    }
    IEnumerator WaitForPeaceTrackToEnd()
    {
        while (musicPlayer.isPlaying)
        {
            yield return new WaitForSeconds(Random.Range(5, 20));

        }
        peaceMusic.Remove(currentClip);
        musicPlayer.clip = GetRandomPeaceMusic();
        musicPlayer.Play();
        StartCoroutine(WaitForPeaceTrackToEnd());
    }


    public void OnDayOver(int _day)
    {
        musicPlayer.Stop();
        StopCoroutine(WaitForCombatTrackToEnd());
    }
    public void OnContinueButton()
    {
        StartCoroutine(WaitForPeaceTrackToEnd());
    }
    public void OnDayBegin(int _day)
    {
        musicPlayer.Stop();
        StopCoroutine(WaitForPeaceTrackToEnd());
        StartCoroutine(WaitForCombatTrackToEnd());
    }
    public void OnGameOver()
    {
        musicPlayer.Stop();
    }
    public void OnLevelWin(LevelID _levelID, int _score, int _maegen)
    {
        musicPlayer.Stop();
        musicPlayer.clip = winMusic;
        musicPlayer.Play();
    }
    private void OnEnable()
    {
        GameEvents.OnGameOver += OnGameOver;
        GameEvents.OnLevelWin += OnLevelWin;
        GameEvents.OnDayOver += OnDayOver;
        GameEvents.OnContinueButton += OnContinueButton;
        GameEvents.OnDayBegin += OnDayBegin;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= OnGameOver;
        GameEvents.OnLevelWin -= OnLevelWin;
        GameEvents.OnDayOver -= OnDayOver;
        GameEvents.OnContinueButton -= OnContinueButton;
        GameEvents.OnDayBegin -= OnDayBegin;
    }
}
