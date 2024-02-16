using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Unity.Burst.CompilerServices;

public class Spy : GameBehaviour
{
    [Header("Stats")]
    public float health;
    public float maxHealth;

    [Header("Components")]
    public NavMeshAgent navAgent;
    public Animator animator;
    public GameObject homeTree;
    public GameObject spawnParticle;

    [Header("Death Objects")]
    public GameObject deathObject;
    public GameObject bloodParticle1;
    public GameObject deadFire;

    [Header("Audio")]
    public GameObject SFXPool;
    public int soundPoolCurrent;
    public AudioSource[] soundPool;
    public AudioSource audioSource;

    void Start()
    {
        _EM.enemies.Add(gameObject);
        health = maxHealth;
        soundPool = SFXPool.GetComponents<AudioSource>();
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        homeTree = GameObject.FindGameObjectWithTag("Home Tree");
        navAgent.SetDestination(homeTree.transform.position);
        GameEvents.ReportOnSpySpawned();
    }

    // Update is called once per frame
    void Update()
    {

        if (navAgent.velocity != Vector3.zero)
        {
            animator.SetBool("hasStopped", false);
        }
        if (navAgent.velocity == Vector3.zero)
        {
            animator.SetBool("hasStopped", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerWeapon")
        {
            TakeDamage(_GM.satyrDamage);
        }
        if (other.tag == "PlayerWeapon2")
        {
            TakeDamage(_GM.orcusDamage);
        }
        if (other.tag == "PlayerWeapon3")
        {

            TakeDamage(_GM.leshyDamage);
        }
        if (other.tag == "PlayerWeapon4")
        {
            TakeDamage(_GM.skessaDamage);
        }
        if (other.tag == "PlayerWeapon5")
        {
            TakeDamage(_GM.goblinDamage);
        }
        if (other.tag == "PlayerWeapon6")
        {
            TakeDamage(_GM.golemDamage);
        }
        if (other.tag == "Spit")
        {
            TakeDamage(_GM.spitDamage);
        }
        if (other.tag == "SpitExplosion")
        {
            TakeDamage(_GM.spitExplosionDamage);
        }
        if (other.tag == "River")
        {
            _EM.enemies.Remove(gameObject);
            _SPYM.Respawn();
            Instantiate(spawnParticle, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        if (other.tag == "Explosion" || other.tag == "Explosion2")
        {
            _EM.enemies.Remove(gameObject);
            GameObject go;
            go = Instantiate(deadFire, transform.position, transform.rotation);
            go.GetComponentInChildren<Rigidbody>().AddForce(transform.up * 2000);
            go.GetComponentInChildren<Rigidbody>().AddForce(transform.forward * -16000);
            Destroy(go, 15);
            Destroy(gameObject);
        }
    }
    public void TakeDamage(float damage)
    {
        audioSource.clip = _SM.GetGruntSounds();
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
        GameObject bloodParticle;
        bloodParticle = Instantiate(bloodParticle1, transform.position, transform.rotation);
        health -= damage;
        Die();
    }
    private void Die()
    {
        if (health <= 0)
        {
            _EM.enemies.Remove(gameObject);
            GameObject go;
            go = Instantiate(deathObject, transform.position, transform.rotation);
            Destroy(go, 15);
            Destroy(gameObject);
        }
    }
}
