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
    public Collider weaponCollider2;
    public GameObject footstepParticle;
    public GameObject stompParticle;
    public GameObject leftFoot;
    public GameObject rightFoot;
    public GameObject hand;
    public GameObject spit;
    public ParticleSystem spitParticle;
    public Collider spitCollider;

    private void Update()
    {
        if(gameObject.GetComponentInParent<Unit>().unitType == UnitType.GoblinUnit)
        {
            firingPoint.transform.LookAt(gameObject.GetComponentInParent<Unit>().closestEnemy);
        }    
    }
    public void Arrow()
    {
        if(_EM.enemies.Count != 0 && GetComponentInParent<Unit>().distanceToClosestEnemy < 80)
        {
            GameObject go = Instantiate(arrow, firingPoint.transform.position, transform.rotation);
            go.GetComponent<EnemyProjectile>().target = GetComponentInParent<Unit>().closestEnemy.gameObject;
            Destroy(go, 1);
        }
    }
    public void EnableCollider()
    {
        weaponCollider.enabled = true;
    }
    public void DisableCollider()
    {
        weaponCollider.enabled = false;
    }
    public void EnableHandCollider()
    {
        weaponCollider2.enabled = true;
    }
    public void DisableHandCollider()
    {
        weaponCollider2.enabled = false;
    }
    public void Footstep()
    {
        gameObject.GetComponentInParent<Unit>().PlayFootstepSound();
    }
    public void GolemFootstep()
    {
        gameObject.GetComponentInParent<Unit>().PlayGolemFootstepSound();
    }
    public void LeftLeshyFootstep()
    {
        gameObject.GetComponentInParent<Unit>().PlayLeshyFootstepSound();
        Instantiate(footstepParticle, leftFoot.transform.position, Quaternion.Euler(0, 0, 0));
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
    public void HandStomp()
    {
        Instantiate(stompParticle, hand.transform.position, Quaternion.Euler(90, 0, 0));
        gameObject.GetComponentInParent<Unit>().PlayLeshyStompSound();
    }
    public void Flap()
    {
        gameObject.GetComponentInParent<Unit>().PlayFlapSound();
    }
    public void GolemStompLeft()
    {
        Instantiate(stompParticle, rightFoot.transform.position, Quaternion.Euler(-90, 0, 0));
        gameObject.GetComponentInParent<Unit>().PlayLeshyStompSound();
    }
    public void GolemStompRight()
    {
        Instantiate(stompParticle, leftFoot.transform.position, Quaternion.Euler(-90, 0, 0));
        gameObject.GetComponentInParent<Unit>().PlayLeshyStompSound();
    }
    public void EnableSpit()
    {
        spit.SetActive(true);
        spitParticle.Play();
        spitCollider.enabled = true;
    }
    public void DisableSpit()
    {

        spitParticle.Stop();
        spitCollider.enabled = false;
    }
}
