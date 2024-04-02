using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dog : GameBehaviour
{
    Animator animator;
    NavMeshAgent agent;
    public float health;
    public float maxHealth;
    public float speed;
    public GameObject targetTree;
    public GameObject explosion;
    public GameObject bloodParticle1;

    List<GameObject> trees;

    private void Update()
    {
        trees = _GM.trees;

        if(Vector3.Distance(gameObject.transform.position, targetTree.transform.position) < 10)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            _EM.enemies.Remove(gameObject);
            Destroy(gameObject);
        }
    }
    void Start()
    {
        _EM.enemies.Add(gameObject);
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        health = maxHealth;
        agent.speed = speed;
        trees = _GM.trees;
        int target = Random.Range(0, trees.Count);
        targetTree = trees[target];
        agent.SetDestination(targetTree.transform.position);
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
            TakeDamage(_GM.skessaDamage);
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
        if (other.tag == "Explosion" || other.tag == "Explosion2")
        {

        }
        if (other.tag == "Spit")
        {
            agent.speed = speed / 2;
        }
        if (other.tag == "Hand")
        {

        }
    }
    public void TakeDamage(float damage)
    {
        GameObject bloodParticle;
        bloodParticle = Instantiate(bloodParticle1, transform.position + new Vector3(0, 5, 0), transform.rotation);
        health -= damage;
        Die();
    }
    private void Die()
    {
        if (health <= 0)
        {
            _EM.enemies.Remove(gameObject);
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
