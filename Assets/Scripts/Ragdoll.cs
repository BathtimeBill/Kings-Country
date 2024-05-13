using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    public AudioClip[] deathVocals;
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = GetDeathSound();
        audioSource.Play();
    }
    public AudioClip GetDeathSound()
    {
        return deathVocals[Random.Range(0, deathVocals.Length)];
    }
}
