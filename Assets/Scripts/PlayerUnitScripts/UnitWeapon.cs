using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWeapon : GameBehaviour
{
    public GameObject firingPoint;
    public GameObject arrow;
    public GameObject arrow2;
    public Collider weaponCollider;
    public GameObject footstepParticle;
    public GameObject stompParticle;
    public GameObject leftFoot;
    public GameObject rightFoot;


    private void Update()
    {
        if(gameObject.GetComponentInParent<Unit>().unitType == UnitType.GoblinUnit)
        {
            firingPoint.transform.LookAt(gameObject.GetComponentInParent<Unit>().closestEnemy);
        }    
    }
    public void Arrow()
    {
        GameObject go = Instantiate(arrow, firingPoint.transform.position, transform.rotation);
        go.GetComponent<Rigidbody>().AddForce(transform.forward * 1000f);
        go.transform.LookAt(gameObject.GetComponentInParent<Unit>().closestEnemy);
        Destroy(go, 2);
    }
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
    public void Flap()
    {
        gameObject.GetComponentInParent<Unit>().PlayFlapSound();
    }
}
