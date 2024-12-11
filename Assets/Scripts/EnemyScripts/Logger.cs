using System.Collections;
using UnityEngine;

public class Logger : Enemy
{
    #region AI
    public override void SetState()
    {
        SetClosestUnit();
        attackRange = unitData.attackRange;
        if (_GuardiansExist && distanceFromClosestUnit < attackRange)
        {
            ChangeState(EnemyState.Attack);
            targetObject = closestUnit.transform;
        }
        else if (_TreesExist)
        {
            targetObject = ObjectX.GetClosest(gameObject, _GM.trees).transform;
        }
        else
        {
            targetObject = _HOME.transform;
            attackRange *= 2;
        }
        
        //agent.SetDestination(targetObject.position);
        //distanceToTarget = Vector3.Distance(targetObject.transform.position, transform.position);
        HandleState();
    }
    #endregion

    #region Triggers

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
    
    #endregion
    
    #region Damage
    public override void TakeDamage(int damage, string _damagedBy)
    {
        base.TakeDamage(damage, _damagedBy);
    }
    
    public override void Die(Enemy _thisUnit, string _killedBy, DeathID _deathID)
    {
        base.Die(_thisUnit, _killedBy, _deathID);
    }
    #endregion
}
