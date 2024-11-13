using UnityEngine;

public class Dog : Enemy
{
    public float detonationDistance = 10f;
    public GameObject explosion;
    private GameObject targetTree;
    
    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        GetRandomTree();
    }

    private void GetRandomTree()
    {
        targetTree = _GM.GetRandomTree;
        agent.SetDestination(targetTree.transform.position);
    }
    private void Update()
    {
        if (_NoTrees)
            return;
        
        if(!targetTree)
            GetRandomTree();
        
        if (Vector3.Distance(gameObject.transform.position, targetTree.transform.position) < detonationDistance)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
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
