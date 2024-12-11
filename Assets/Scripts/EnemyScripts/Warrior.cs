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
    }
    #endregion

    #region AI
    
    public override void SetState()
    {
        distanceToTree = _HOME ? Vector3.Distance(_HOME.transform.position, transform.position) : 20000;
        distanceToHorgr = _HorgrExists ? Vector3.Distance(horgrTargetPoint.transform.position, transform.position) : 20000;
        SetClosestUnit();
        
        if (hasArrivedAtHorgr)
        {
            targetObject = transform;
            ChangeState(EnemyState.DefendSite);
        }
        else if (distanceFromClosestUnit < attackRange && _GuardiansExist)
        {
            targetObject = closestUnit;
            ChangeState(EnemyState.Attack);
        }
        else if (distanceToHorgr > distanceFromClosestUnit)
        {
            targetObject = closestUnit;
            ChangeState(EnemyState.Work);
        }
        else if (distanceToHorgr <= distanceFromClosestUnit && !spawnedFromSite)
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
    }
    
    public override void HandleWorkState()
    {
        /*if (_NoWildlife && _NoGuardians && !_HorgrExists)
        {
            ChangeState(EnemyState.Idle);
            return;
        }*/
        base.HandleWorkState();
    }

    public override void HandleClaimState()
    {
        if(hasArrivedAtHorgr)
            ChangeState(EnemyState.DefendSite);
        base.HandleClaimState();
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
            ChangeState(EnemyState.DefendSite);
        }
    }
    
    private IEnumerator WaitForHorgr()
    {
        ChangeState(EnemyState.DefendSite);
        yield return new WaitForSeconds(0.5f);
        hasArrivedAtHorgr = true;
    }
    #endregion

    #region Triggers
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (!_HorgrExists)
            return;
        
        if(other.CompareTag("Horgr") && _HORGR.ContainsEnemy(this) && !spawnedFromSite)
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
