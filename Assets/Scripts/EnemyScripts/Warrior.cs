using System.Collections;
using UnityEngine;

public class Warrior : Enemy
{
    private float distanceToTree;
    private bool hasArrivedAtHorgr;
    private float distanceToHorgr;
    private GameObject horgrTargetPoint;
    
    #region Startup
    public override void Start()
    {
        InitializeHorgrTargetPoint();
        base.Start();
    }

    private void InitializeHorgrTargetPoint()
    {
        if (_HorgrExists)
        {
            Vector3 randomHorgrPos = SpawnX.GetSpawnPositionInRadius(_HORGR.transform.position,
                _HORGR.GetComponent<SphereCollider>().radius);
            horgrTargetPoint = new GameObject("HorgrTargetPoint");
            horgrTargetPoint.transform.position = randomHorgrPos;
        }
    }
    #endregion

    #region AI
    public override void UpdateDistances()
    {
        distanceToTree = _HOME ? Vector3.Distance(_HOME.transform.position, transform.position) : 20000;
        distanceToHorgr = _HorgrExists ? Vector3.Distance(horgrTargetPoint.transform.position, transform.position) : 20000;
        base.UpdateDistances();
    }

    public override void DetermineState()
    {
        if (hasArrivedAtHorgr)
        {
            if (distanceToClosestUnit < attackRange)
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
        else if (distanceToClosestUnit < attackRange)
        {
            targetObject = closestUnit;
            ChangeState(EnemyState.Attack);
        }
        else if (_HorgrExists && distanceToHorgr > distanceToClosestUnit)
        {
            targetObject = closestUnit;
            ChangeState(EnemyState.Work);
        }
        else if (_HorgrExists && distanceToHorgr <= distanceToClosestUnit && !spawnedFromSite)
        {
            targetObject = horgrTargetPoint.transform;
            ChangeState(EnemyState.Work);
        }
        else
        {
            targetObject = _HOME.transform;
            ChangeState(EnemyState.Work);
        }
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
        enemyAnimation.PlayHoldAnimation(false);
        if (!TargetWithinRange)
            ChangeState(EnemyState.Work);
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
            enemyAnimation.PlayHoldAnimation(true);
            StandStill(); 
            ChangeState(EnemyState.DefendSite);
        }
    }
    
    private IEnumerator WaitForHorgr()
    {
        yield return new WaitForSeconds(1f);
        ChangeState(EnemyState.DefendSite);
        hasArrivedAtHorgr = true;
    }
    #endregion

    #region Triggers
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
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
            hasArrivedAtHorgr = false;
            ChangeState(EnemyState.Work);
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
}
