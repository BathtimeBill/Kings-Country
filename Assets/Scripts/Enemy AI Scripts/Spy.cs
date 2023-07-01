using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Spy : GameBehaviour
{
    [Header("Stats")]
    public float health;
    public float maxHealth;

    [Header("Components")]
    public NavMeshAgent navAgent;
    public Animator animator;
    public GameObject homeTree;

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
        health = maxHealth;
        soundPool = SFXPool.GetComponents<AudioSource>();
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        homeTree = GameObject.FindGameObjectWithTag("Home Tree");
        navAgent.SetDestination(homeTree.transform.position);
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
        if(other.tag == "River")
        {
            Destroy(gameObject);
        }

        if (other.tag == "Explosion")
        {
            Debug.Log("Explosion");
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
            GameObject go;
            go = Instantiate(deathObject, transform.position, transform.rotation);
            Destroy(go, 15);
            Destroy(gameObject);
        }
    }
}
