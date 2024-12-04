using System.Collections;
using UnityEngine;

public class Logger : Enemy
{
    [Header("Stats")]
    public float loggerStoppingDistance;
    public bool hasStopped = false;

    [Header("Trees")]
    public Transform closestTree;
    public float distanceFromClosestTree;
    public GameObject homeTree;
    
    #region Startup
    public override void Start()
    {
        base.Start();
        state = EnemyState.Work;
        homeTree = GameObject.FindGameObjectWithTag("Home Tree");
        StartCoroutine(Tick());
    }
    #endregion

    #region AI
    IEnumerator Tick()
    {
        if (_GM.gameState == GameState.Lose)
            StopAllCoroutines();

        SetClosestUnit();
        
        if (_TreesExist)
        {
            closestTree = ObjectX.GetClosest(gameObject, _GM.trees).transform;
            distanceFromClosestTree = Vector3.Distance(closestTree.transform.position, transform.position);
        }
        
        if (agent.velocity != Vector3.zero)
        {
            animator.SetBool("hasStopped", false);
            hasStopped = false;
        }
        if (agent.velocity == Vector3.zero)
        {
            animator.SetBool("hasStopped", true);
            hasStopped = true;
        }
        HandleState();
        yield return new WaitForSeconds(tickRate);
        StartCoroutine(Tick());
    }
    
    public override void HandleWorkState()
    {
        if (distanceFromClosestUnit < attackRange)
            state = EnemyState.Attack;

        if (_NoTrees)
        {
            FindHomeTree();
        }
        else
        {
            if (distanceFromClosestTree < attackRange)
            {
                SmoothFocusOnTree();
            }
            FindTree();
        }
    }

    public override void HandleRelaxState()
    {
    }

    public override void HandleAttackState()
    {
        if(_GuardiansExist)
        {
            if (distanceFromClosestUnit >= attackRange)
            {
                state = EnemyState.Work;
            }
            FindUnit();
            SmoothFocusOnEnemy();
        }
    }

    public override void HandleClaimState()
    {
    }

    public override void HandleVictoryState()
    {
    }
    #endregion

    #region Damage

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    private int RandomCheerAnim() => Random.Range(1, 3);


    #endregion

    private void SmoothFocusOnEnemy()
    {
        var targetRotation = Quaternion.LookRotation(closestUnit.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
    }
    private void SmoothFocusOnTree()
    {

        if (_NoTrees)
        {
            var targetRotation = Quaternion.LookRotation(homeTree.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
        }
        else
        {
            var targetRotation = Quaternion.LookRotation(closestTree.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
        }
    }

    public override void TakeDamage(int damage, string _damagedBy)
    {
        base.TakeDamage(damage, _damagedBy);
    }
    
    public override void Die(Enemy _thisUnit, string _killedBy, DeathID _deathID)
    {
        base.Die(_thisUnit, _killedBy, _deathID);
    }
    public override void DropMaegen()
    {
        int rnd;
        if (_TUTORIAL.isTutorial && _TUTORIAL.tutorialStage == 8)
        {
            rnd = 1;
        }
        else
        {
            rnd = Random.Range(1, maxRandomDropChance);
        }
        if (rnd == 1)
        {
            Instantiate(_SETTINGS.general.maegenPickup, transform.position, transform.rotation);
        }
    }
    //public override void Launch()
    //{
    //    base.Launch();
    //}
    

    public void FindTree()
    {
        agent.SetDestination(closestTree.transform.position);
    }
    public void FindUnit()
    {
        if(_NoGuardians && _TreesExist)
        {
            state = EnemyState.Work;
        }
        agent.SetDestination(closestUnit.transform.position);
    }
    public void FindHomeTree()
    {
        agent.SetDestination(homeTree.transform.position);
    }
    public override void Win()
    {
        StopCoroutine(Tick());
        state = EnemyState.Victory;
        base.Win();
    }
}
