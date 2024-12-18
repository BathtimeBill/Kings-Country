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
            Vector3 randomHorgrPos = SpawnX.GetSpawnPositionInRadius(_HORGR.transform.position, _HORGR.GetComponent<SphereCollider>().radius);
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
        //First determine the target object...
        if (_HorgrExists && !spawnedFromSite)
        {
            if (hasArrivedAtHorgr)
            {
                targetObject = (distanceToClosestUnit < attackRange) ? closestUnit : transform;
                HandleDefendState();
            }
            else
            {
                targetObject = (distanceToHorgr > distanceToClosestUnit) ? closestUnit : horgrTargetPoint.transform;
                HandleWorkState();
            }
        }
        else
        {
            targetObject = (distanceToClosestUnit < attackRange) ? closestUnit : _HOME.transform;
            HandleWorkState();
        }
    }
    
    public override void HandleWorkState()
    {
        base.HandleWorkState();
        enemyAnimation.PlayHoldAnimation(false);
    }
    
    public override void HandleDefendState()
    {
        if (_HORGR.HasUnits())
        {
            targetObject = closestUnit;
            HandleAttackState();
        }
        else
        {
            enemyAnimation.PlayHoldAnimation(true);
            base.HandleDefendState();
        }
    }

    public override void HandleAttackState()
    {
        if (_HorgrExists && targetObject == horgrTargetPoint.transform)
        {
            canAttack = false;
            ChangeState(EnemyState.Work);
        }
        else
        {
            canAttack = distanceToTarget <= stoppingDistance && targetObject != transform;
            base.HandleAttackState();
        }
    }
    
    private IEnumerator WaitForHorgr()
    {
        yield return new WaitForSeconds(1f);
        base.HandleDefendState();
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
