using System.Collections;
using UnityEngine;

public class Warrior : Enemy
{
    [Header("Horgr")]
    private float distanceFromClosestHorgr;
    private bool horgrSwitch;
    public float distanceToTree;
    public bool hasArrivedAtHorgr;
    public float distanceToHorgr;
    private GameObject horgrTargetPoint;
    
    #region Startup
    public override void Start()
    {
        if (_HorgrExists)
        {
            Vector3 randomHorgrPos = SpawnX.GetSpawnPositionInRadius(_HORGR.transform.position,
                _HORGR.GetComponent<SphereCollider>().radius);
            horgrTargetPoint = new GameObject("HorgrTargetPoint");
            horgrTargetPoint.transform.position = randomHorgrPos;
        }
        base.Start();
        StartCoroutine(Tick());
    }
    #endregion

    #region AI
    
    public IEnumerator Tick()
    {
        distanceToTree = _HOME ? Vector3.Distance(_HOME.transform.position, transform.position) : 20000;
        distanceToHorgr = _HorgrExists ? Vector3.Distance(horgrTargetPoint.transform.position, transform.position) : 20000;
        SetClosestUnit();
        
        if (hasArrivedAtHorgr)
        {
            if (distanceFromClosestUnit < attackRange)
            {
                targetObject = closestUnit;
                ChangeState(EnemyState.Attack);
            }
            else
            {
                targetObject = transform;
                ChangeState(EnemyState.DefendSite);
            }
        }
        else if (distanceFromClosestUnit < attackRange)
        {
            targetObject = closestUnit;
            ChangeState(EnemyState.Attack);
        }
        else if (_HorgrExists && distanceToHorgr > distanceFromClosestUnit)
        {
            targetObject = closestUnit;
            ChangeState(EnemyState.Work);
        }
        else if (_HorgrExists && distanceToHorgr <= distanceFromClosestUnit && !spawnedFromSite)
        {
            targetObject = horgrTargetPoint.transform;
            ChangeState(EnemyState.ClaimSite);
        }
        else
        {
            targetObject = _HOME.transform;
            ChangeState(EnemyState.Work);
        }
        
        HandleState();
        
        print("State: " + state + " | Target: " + targetObject.name);
        
        agent.SetDestination(targetObject.position);
        distanceToTarget = Vector3.Distance(targetObject.transform.position, transform.position);
        bool attacking = agent.velocity == Vector3.zero || CanAttack; 
        animator.SetBool("Attacking", attacking);
        
        if (_GM.gameState == GameState.Lose)
        {
            ChangeState(EnemyState.Victory);
            StopAllCoroutines();
            HandleVictoryState();
        }
        
        yield return new WaitForSeconds(tickRate);
        StartCoroutine(Tick());
    }
    
    public override void HandleWorkState()
    {
        if (!targetObject)
            return;

        if (TargetWithinRange)
            ChangeState(EnemyState.Attack);
    }

    public override void HandleAttackState()
    {
        animator.SetBool("Holding", false);
        if (!TargetWithinRange)
            ChangeState(EnemyState.Work);
    }

    public override void HandleClaimState()
    {
        if(hasArrivedAtHorgr)
            HandleDefendState();
    }
    
    public override void HandleDefendState()
    {
        if (_HORGR.HasUnits())
        {
            targetObject = closestUnit;
            ChangeState(EnemyState.Attack);
        }
        else
        {
            animator.SetBool("Holding", true);
            StandStill(); 
            ChangeState(EnemyState.DefendSite);
        }
    }
    
    private IEnumerator WaitForHorgr()
    {
        ChangeState(EnemyState.DefendSite);
        yield return new WaitForSeconds(0.5f);
        hasArrivedAtHorgr = true;
        animator.SetBool("Holding", true);
    }
    #endregion

    #region Triggers
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (!_HorgrExists)
            return;
        
        if(other.CompareTag("Horgr") && !_HORGR.ContainsEnemy(this) && !spawnedFromSite)
        {
            _HORGR.AddEnemy(this);
            StartCoroutine(WaitForHorgr());
        }
    }
    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.CompareTag("Horgr") && !spawnedFromSite)
        {
            _HORGR.RemoveEnemy(this);
            SetState();
            hasArrivedAtHorgr = false;
        }
    }
    #endregion
    
    #region Damage
    public override void TakeDamage(int damage, string _damagedBy)
    {
        base.TakeDamage(damage, _damagedBy);
    }
    public override void Die(Enemy _thisUnit, string _killedBy, DeathID _deathID)
    {
        Destroy(horgrTargetPoint);
        base.Die(_thisUnit, _killedBy, _deathID);
    }
    #endregion
    
    public void FindUnit()
    {
        if (_NoGuardians)
        {
            state = EnemyState.Work;
        }
        agent.SetDestination(closestUnit.transform.position);
    }

    public void FindHomeTree()
    {
        agent.SetDestination(_HOME.transform.position);
    }
}
