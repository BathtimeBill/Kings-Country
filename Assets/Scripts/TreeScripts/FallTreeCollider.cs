using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTreeCollider : GameBehaviour
{
    private AudioSource audioSource;
    public GameObject landParticle;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = _SM.GetTreeLandSound();
    }

    //Determines weather the trunk of the falling tree has collided with the ground, at which point a partile effect is instantiated and a sound is played.
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground")
        {
            Instantiate(landParticle, transform.position, Quaternion.Euler(0, 0, 0));
            audioSource.Play();
        }
    }
}
