using UnityEngine;

public class Dog : Enemy
{
    #region AI
    public override void DetermineState()
    {
        if (_TreesExist)
        {
            targetObject = ObjectX.GetClosest(gameObject, _GM.trees).transform;
        }
        else if (_GuardiansExist)
        {
            targetObject = closestUnit.transform;
        }
        else
        {
            targetObject = _HOME.transform;
        }
        HandleWorkState();
    }
    
    public override void HandleWorkState()
    {
        ChangeState(EnemyState.Work);
    }
    
    public override void CheckAttackState()
    {
        canAttack = distanceToTarget <= stoppingDistance;
        if (canAttack)
        {
            TakeDamage(10000, "Dog");
        }
    }
    #endregion
}