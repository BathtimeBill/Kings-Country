using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dog : Enemy
{
    Animator animator;
    public GameObject targetTree;
    public GameObject explosion;
    public GameObject bloodParticle1;
    List<GameObject> trees;

    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        trees = _GM.trees;
        int target = Random.Range(0, trees.Count);
        targetTree = trees[target];
        agent.SetDestination(targetTree.transform.position);
    }
    private void Update()
    {
        trees = _GM.trees;

        if (Vector3.Distance(gameObject.transform.position, targetTree.transform.position) < 10)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag == "Spit")
        {
            agent.speed = speed / 2;
        }
        if (other.tag == "Hand")
        {

        }
    }
    public override void TakeDamage(int damage, string _damagedBy)
    {
        base.TakeDamage(damage, _damagedBy);
    }
    public override void Die(Enemy _thisUnit, string _killedBy, DeathID _deathID)
    {
        Instantiate(explosion, transform.position, transform.rotation);
        base.Die(_thisUnit, _killedBy, _deathID);
    }
}
