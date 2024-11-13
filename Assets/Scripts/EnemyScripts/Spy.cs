using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Spy : Enemy
{
    [Header("Components")]
    public GameObject spawnParticle;

    [Header("Death Objects")]
    public GameObject deadFire;

    [Header("Audio")]
    public GameObject SFXPool;
    public int soundPoolCurrent;
    public AudioSource[] soundPool;
    public AudioSource audioSource;

    public override void Start()
    {
        base.Start();
        health = maxHealth;
        soundPool = SFXPool.GetComponents<AudioSource>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.SetDestination(_HOME.transform.position);
        GameEvents.ReportOnSpySpawned();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("hasStopped", agent.velocity == Vector3.zero);
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        /*
        if (other.tag == "Spit")
        {
            TakeDamage(_GM.spitDamage);
        }
        if (other.tag == "SpitExplosion")
        {
            TakeDamage(_GM.spitExplosionDamage);
        }*/
        if (other.CompareTag("River"))
        {
            transform.position = SpawnX.GetSpawnPositionOnLevel();
            Instantiate(spawnParticle, transform.position, transform.rotation);
        }
    }
    public override void TakeDamage(int damage, string _damagedBy)
    {
        audioSource.clip = _SM.GetGruntSounds();
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
        base.TakeDamage(damage, _damagedBy);
    }
    //public override void Die(Enemy _thisUnit, string _killedBy, DeathID _deathID)
    //{
    //    GameObject go;
    //    go = Instantiate(deathObject, transform.position, transform.rotation);
    //    Destroy(go, 15);
    //    base.Die(_thisUnit, _killedBy, _deathID);
    //}
}
