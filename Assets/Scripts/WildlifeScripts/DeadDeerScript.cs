using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadDeerScript : GameBehaviour
{
    public AudioSource audiosource;
    private AudioClip audioclip;

    //Plays a random deer dying sound effect when instantiated.
    void Start()
    {
        audioclip = _SM.GetDeerDistressSound();
        audiosource.clip = audioclip;
        audiosource.Play();
    }
}
