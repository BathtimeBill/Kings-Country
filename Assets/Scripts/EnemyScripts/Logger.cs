using UnityEngine;

public class Logger : Enemy
{
    #region AI
    public override void DetermineState()
    {
        if (_GuardiansExist && distanceToClosestUnit < attackRange)
        {
            targetObject = closestUnit.transform;
            HandleAttackState();
        }
        else
        {
            targetObject = _TreesExist ? ObjectX.GetClosest(gameObject, _GAME.trees).transform : _HOME.transform;
            attackRange = _TreesExist ? enemyData.attackRange : enemyData.attackRange * 2;
            HandleWorkState();
        }
    }
    
    public override void HandleAttackState()
    {
        canAttack = distanceToTarget <= stoppingDistance && targetObject != transform;
        base.HandleAttackState();
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
