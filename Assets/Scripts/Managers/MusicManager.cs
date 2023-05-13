using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    public AudioSource musicPlayer;
    public List<AudioClip> combatMusic;
    public List<AudioClip> peaceMusic;
    public AudioClip currentClip;

    void Start()
    {
        musicPlayer.clip = GetRandomCombatMusic();
        musicPlayer.loop = false;
        musicPlayer.Play();
        StartCoroutine(WaitForCombatTrackToEnd());
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
        if (_EM.enemies.Length > 0)
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
        if (_EM.enemies.Length == 0)
            StartCoroutine(WaitForPeaceTrackToEnd());
        else
            StartCoroutine(WaitForCombatTrackToEnd());
    }
}
