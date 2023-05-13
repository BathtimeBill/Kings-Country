using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodParticle : GameBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;

    //plays a random impact sound when the particle is instantiated.
    void Start()
    {
        audioSource.clip = _SM.GetImpactSounds();
        audioSource.Play();
    }
}
