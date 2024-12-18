using UnityEngine;

public class Goblin : Guardian
{
    [Header("Goblin Specific")] 
    public Transform firingPoint;
    public GameObject arrowObject;
    private Arrow arrow;
    public float rangeMultiplier = 20f;

    public override void Start()
    {
        base.Start();
        arrow = arrowObject.GetComponent<Arrow>();
    }

    public override void Attack(int _attack)
    {
        base.Attack(_attack);
        if (_NoEnemies)
            return;
        
        if(distanceToClosestEnemy < guardianData.attackRange)
        {
            arrowObject.transform.position = firingPoint.position;
            firingPoint.transform.LookAt(ClosestEnemy);
            arrowObject.SetActive(true);
            arrow.Setup(ClosestEnemy);
            DisableAfterTime(arrowObject, 1);
        }
    }

    public override void HandleAttackState()
    {
        stoppingDistance *= rangeMultiplier;
        base.HandleAttackState();
    }
    public override void HandleMovingState()
    {
        stoppingDistance = guardianData.stoppingDistance;
        base.HandleMovingState();
    }
}
