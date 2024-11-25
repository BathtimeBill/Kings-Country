using UnityEngine.AI;
using UnityEngine;

public class Spy : Enemy
{
    [Header("Components")]
    public GameObject spawnParticle;
    
    public override void Start()
    {
        base.Start();
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.SetDestination(_HOME.transform.position);
        GameEvents.ReportOnSpySpawned();
    }
    
    void Update()
    {
        animator.SetBool("hasStopped", agent.velocity == Vector3.zero);
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag("River"))
        {
            transform.position = SpawnX.GetSpawnPositionOnLevel();
            Instantiate(spawnParticle, transform.position, transform.rotation);
        }
    }
    public override void TakeDamage(int damage, string _damagedBy)
    {
        base.TakeDamage(damage, _damagedBy);
    }
}
