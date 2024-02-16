using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorWeapon : GameBehaviour
{
    public Collider weaponCollider;
    public AudioSource weaponSource;
    public void EnableCollider()
    {
        weaponCollider.enabled = true;
    }
    public void DisableCollider()
    {
        weaponCollider.enabled = false;
    }

    public void Footstep()
    {
        gameObject.GetComponentInParent<Logger>().PlayFootstepSound();
    }
    public void WarriorFootstep()
    {
        gameObject.GetComponentInParent<Warrior>().PlayFootstepSound();
    }
    public void KnightFootstep()
    {
        gameObject.GetComponentInParent<Warrior>().PlayKnightFootstepSound();
    }
    public void Whoosh()
    {
        weaponSource.clip = _SM.GetWhooshSounds();
        weaponSource.pitch = Random.Range(0.8f, 1.2f);
        weaponSource.Play();
    }
}
