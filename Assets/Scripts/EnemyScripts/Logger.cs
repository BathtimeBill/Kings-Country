using System.Collections;
using UnityEngine;

public class Logger : Enemy
{
    [Header("Woodcutter Type")]
    public WoodcutterType woodcutterType;
    [Header("Tick")]
    public float seconds = 0.5f;
    [Header("Stats")]
    public float loggerStoppingDistance;
    public bool hasStopped = false;

    [Header("AI")]
    public EnemyState state;
    public Transform closestUnit;
    public float distanceFromClosestUnit;
    
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

        if (_GuardiansExist)
        {
            closestUnit = GetClosestUnit();
            distanceFromClosestUnit = Vector3.Distance(closestUnit.transform.position, transform.position);
        }
        if (_TreesExist)
        {
            closestTree = GetClosestTree();
            distanceFromClosestTree = Vector3.Distance(closestTree.transform.position, transform.position);
        }
        
        healthBar.ChangeUnitState(state.ToString());
        
        switch (state)
        {
            case EnemyState.Work:
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
                break;

            case EnemyState.Attack:
                if(_GuardiansExist)
                {
                    if (distanceFromClosestUnit >= attackRange)
                    {
                        state = EnemyState.Work;
                    }
                    FindUnit();
                    SmoothFocusOnEnemy();
                }
                break;
            
            case EnemyState.Cheer:
                agent.SetDestination(transform.position);
                break;
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
        yield return new WaitForSeconds(seconds);
        StartCoroutine(Tick());
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

        if (_GM.trees.Count == 0)
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

    

    public Transform GetClosestTree()
    {
        
        float closestDistance = Mathf.Infinity;
        Transform trans = null;

        foreach (GameObject go in _GM.trees)
        {
            float currentDistance;
            currentDistance = Vector3.Distance(transform.position, go.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                trans = go.transform;
            }
        }
        return trans;
    }

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
        Log("Finding Home Tree");
        agent.SetDestination(homeTree.transform.position);
    }
    public override void Win()
    {
        StopCoroutine(Tick());
        state = EnemyState.Cheer;
    }
}
