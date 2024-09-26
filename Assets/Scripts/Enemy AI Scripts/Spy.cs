using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Spy : Enemy
{
    [Header("Components")]
    public GameObject homeTree;
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
        homeTree = GameObject.FindGameObjectWithTag("Home Tree");
        agent.SetDestination(homeTree.transform.position);
        GameEvents.ReportOnSpySpawned();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.velocity != Vector3.zero)
        {
            animator.SetBool("hasStopped", false);
        }
        if (agent.velocity == Vector3.zero)
        {
            animator.SetBool("hasStopped", true);
        }
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
        if (other.tag == "River")
        {
            _SPYM.Respawn();
            Instantiate(spawnParticle, transform.position, transform.rotation);
            Destroy(gameObject);
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
