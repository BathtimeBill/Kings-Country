using UnityEngine;

public class Dog : Enemy
{
    #region AI
    public override void DetermineState()
    {
        attackRange = enemyData.attackRange;
        if (_TreesExist)
        {
            targetObject = ObjectX.GetClosest(gameObject, _GAME.trees).transform;
        }
        else if (_GuardiansExist)
        {
            targetObject = closestUnit.transform;
        }
        else
        {
            targetObject = _HOME.transform;
            attackRange *= 2;
        }
        HandleWorkState();
    }
    
    public override void HandleWorkState()
    {
        ChangeState(EnemyState.Work);
    }
    
    public override void HandleAttackState()
    {
        canAttack = distanceToTarget <= attackRange;
        if (canAttack)
        {
            TakeDamage(10000, "Dog");
        }
    }
    #endregion
}