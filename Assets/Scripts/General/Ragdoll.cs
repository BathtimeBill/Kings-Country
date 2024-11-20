using UnityEngine;

public class Ragdoll : GameBehaviour
{
    public GameObject fireParticles;
    public AudioSource audioSource;
    void Start()
    {
        if(fireParticles)
            fireParticles.SetActive(false);
    }

    public void Die(AudioClip _deathSound, bool _onFire = false)
    {
        if(_onFire) 
            fireParticles.SetActive(true);
        _SM.PlaySound(audioSource, _deathSound);
    }

    public void Launch(float _up, float _forward)
    {
        GetComponentInChildren<Rigidbody>().AddForce(transform.up * _up);
        GetComponentInChildren<Rigidbody>().AddForce(transform.forward * _forward);
    }
}
