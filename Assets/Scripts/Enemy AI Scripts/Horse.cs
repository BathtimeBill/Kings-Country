using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : GameBehaviour
{
    public AudioSource gallopAudioSource;
    public AudioSource armourAudioSource;
    public AudioSource swingAudioSource;


    public void Armour()
    {
        armourAudioSource.clip = _SM.GetKnightFootstepSound();
        armourAudioSource.Play();
    }
    public void Whoosh()
    {
        swingAudioSource.clip = _SM.GetWhooshSounds();

        swingAudioSource.Play();
    }

    public void EnableGallop()
    {
        gallopAudioSource.Play();
    }
    public void DisableGallop()
    {
        gallopAudioSource.Stop();
    }
}
