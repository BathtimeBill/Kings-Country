using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitExplosion : GameBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = _SM.GetSpitExplosionSounds();
        audioSource.Play();
    }


}
