using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBoar : GameBehaviour
{
    public AudioSource audiosource;
    private AudioClip audioclip;

    //Plays a random boar dying sound effect when instantiated.
    void Start()
    {
        audioclip = _SM.GetBoarDistressSound();
        audiosource.clip = audioclip;
        audiosource.Play();
    }
}
