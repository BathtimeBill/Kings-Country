using UnityEngine;

public class LightningSound : GameBehaviour
{
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = _SM.GetLightningSound();
        audioSource.Play();
        Destroy(gameObject, 8);
    }
}
