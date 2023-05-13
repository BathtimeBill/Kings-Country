using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWeapon : GameBehaviour
{

    public Collider weaponCollider;
    public GameObject footstepParticle;
    public GameObject stompParticle;
    public GameObject leftFoot;
    public GameObject rightFoot;
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
        gameObject.GetComponentInParent<Unit>().PlayFootstepSound();
    }
    public void LeftLeshyFootstep()
    {
        gameObject.GetComponentInParent<Unit>().PlayLeshyFootstepSound();
        Instantiate(footstepParticle, leftFoot.transform.position, Quaternion.Euler(90, 0, 0));
    }
    public void RightLeshyFootstep()
    {
        gameObject.GetComponentInParent<Unit>().PlayLeshyFootstepSound();
        Instantiate(footstepParticle, rightFoot.transform.position, Quaternion.Euler(90, 0, 0));
    }
    public void Stomp()
    {
        Instantiate(stompParticle, rightFoot.transform.position, Quaternion.Euler(90, 0, 0));
        gameObject.GetComponentInParent<Unit>().PlayLeshyStompSound();
    }
}
