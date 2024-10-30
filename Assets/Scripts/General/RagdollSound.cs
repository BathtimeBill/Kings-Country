using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollSound : GameBehaviour
{
    public AudioSource audioSource;
    public bool hasBeenLaunched;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(WaitForScreamSound());
    }
    IEnumerator WaitForScreamSound()
    {
        yield return new WaitForEndOfFrame();
        if (hasBeenLaunched)
        {
            audioSource.clip = _SM.GetScreamSound();
            audioSource.Play();
        }
    }
}
