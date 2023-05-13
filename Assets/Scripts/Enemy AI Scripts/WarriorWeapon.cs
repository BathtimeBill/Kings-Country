using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorWeapon : MonoBehaviour
{
    public Collider weaponCollider;
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

}
