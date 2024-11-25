using UnityEngine;

public class Goblin : Unit
{
    [Header("Goblin Specific")] 
    public Transform firingPoint;
    public GameObject arrowObject;
    private Arrow arrow;

    public override void Start()
    {
        base.Start();
        arrow = arrowObject.GetComponent<Arrow>();
    }

    public override void Attack(int _attack)
    {
        base.Attack(_attack);
        if (_EM.allEnemiesDead)
            return;
        
        if(distanceToClosestEnemy < unitData.attackRange)
        {
            arrowObject.transform.position = firingPoint.position;
            firingPoint.transform.LookAt(ClosestEnemy);
            arrowObject.SetActive(true);
            arrow.Setup(ClosestEnemy);
            DisableAfterTime(arrowObject, 1);
        }
    }
}
