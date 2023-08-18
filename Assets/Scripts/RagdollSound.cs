using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollSound : GameBehaviour
{
    private AudioSource audioSource;
    public bool hasBeenLaunched;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if(hasBeenLaunched)
        {
            audioSource.clip = _SM.GetScreamSound();
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
