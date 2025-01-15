using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Ragdoll : GameBehaviour
{
    public GameObject ragdollSource;
    public GameObject fireParticles;
    public AudioSource audioSource;

    private Rigidbody[] rigidbodies;
    private CharacterJoint[] characterJoints;
    private List<Collider> colliders;

    private void Start()
    {
        rigidbodies = ragdollSource.GetComponentsInChildren<Rigidbody>();
        characterJoints = ragdollSource.GetComponentsInChildren<CharacterJoint>();
        colliders = ragdollSource.GetComponentsInChildren<Collider>().ToList();
        colliders.RemoveAll(x => x.gameObject.CompareTag("EnemyWeapon"));
        ToggleRagdoll(false);
    }
    
    public void Die(AudioClip _deathSound, bool _onFire = false)
    {
        ToggleRagdoll(true);
        if(_onFire) 
            fireParticles.SetActive(true);
        _SM.PlaySound(audioSource, _deathSound);
    }

    public void Launch(float _up, float _forward)
    {
        GetComponentInChildren<Rigidbody>().AddForce(transform.up * _up);
        GetComponentInChildren<Rigidbody>().AddForce(transform.forward * _forward);
    }

    public void ToggleRagdoll(bool _isRagdoll)
    {
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.detectCollisions = _isRagdoll;
            rigidbody.useGravity = _isRagdoll;
        }

        foreach (CharacterJoint cj in characterJoints)
        {
            cj.enableCollision = _isRagdoll;
        }

        foreach (Collider col in colliders)
        {
            col.enabled = _isRagdoll;
        }
    }
}
