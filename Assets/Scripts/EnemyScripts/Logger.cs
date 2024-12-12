using UnityEngine;

public class Logger : Enemy
{
    #region AI
    public override void DetermineState()
    {
        if (_GuardiansExist && distanceToClosestUnit < attackRange)
        {
            ChangeState(EnemyState.Attack);
            targetObject = closestUnit.transform;
        }
        else
        {
            ChangeState(EnemyState.Work);
            targetObject = _TreesExist ? ObjectX.GetClosest(gameObject, _GM.trees).transform : _HOME.transform;
            attackRange = _TreesExist ? unitData.attackRange : unitData.attackRange * 2;
        }
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
