using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCannotPlace : GameBehaviour
{
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = _SM.GetTreePlaceSound();
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
