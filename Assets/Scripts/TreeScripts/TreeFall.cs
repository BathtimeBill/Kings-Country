using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFall : GameBehaviour
{
    public AudioSource treeFallSound;

    void Start()
    {
        transform.rotation = Quaternion.Euler(Random.Range(2, 5), Random.Range(0, 360), 0);
        TreeFallSound();
        Destroy(gameObject, 10);
    }

    public void TreeFallSound()
    {
        treeFallSound.clip = _SM.GetTreeFallSound();
        treeFallSound.pitch = Random.Range(0.9f, 1.1f);
        treeFallSound.Play();
    }
}
