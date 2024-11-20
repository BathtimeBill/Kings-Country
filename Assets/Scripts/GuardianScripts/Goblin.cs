using UnityEngine;

public class Goblin : Unit
{
    [Header("Goblin Specific")] 
    public Transform firingPoint;
    public GameObject arrow;

    public override void Attack(int _attack)
    {
        base.Attack(_attack);
        if (_EM.allEnemiesDead)
            return;
        
        if(distanceToClosestEnemy < unitData.attackRange)
        {
            arrow.transform.position = firingPoint.position;
            firingPoint.transform.LookAt(ClosestEnemy);
            arrow.SetActive(true);
            arrow.GetComponent<EnemyProjectile>().target = ClosestEnemy.gameObject;
            DisableAfterTime(arrow, 1);
        }
    }
}
