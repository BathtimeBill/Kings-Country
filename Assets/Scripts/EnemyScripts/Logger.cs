using System.Collections;
using UnityEngine;

public class Logger : Enemy
{

    #region Startup
    public override void Start()
    {
        base.Start();
    }
    #endregion

    #region AI
    public override void HandleWorkState()
    {
        SetClosestUnit();
        attackRange = unitData.attackRange;
        if (_GuardiansExist && distanceFromClosestUnit < attackRange)
        {
            targetObject = closestUnit;
            print("Targeting Unit");
        }
        else if (_TreesExist)
        {
            targetObject = ObjectX.GetClosest(gameObject, _GM.trees).transform;
            print("Targeting Tree");
        }
        else
        {
            targetObject = _HOME.transform;
            attackRange *= 2;
            print("Targeting Home Tree");
        }

        base.HandleWorkState();
    }

    public override void HandleRelaxState()
    {
    }

    public override void HandleAttackState()
    {
       base.HandleAttackState();
    }

    public override void HandleClaimState()
    {
    }

    public override void HandleVictoryState()
    {
        base.HandleVictoryState();
    }
    
    #endregion

    #region Damage

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
    
    #endregion
    
    public override void TakeDamage(int damage, string _damagedBy)
    {
        base.TakeDamage(damage, _damagedBy);
    }
    
    public override void Die(Enemy _thisUnit, string _killedBy, DeathID _deathID)
    {
        base.Die(_thisUnit, _killedBy, _deathID);
    }
    public override void DropMaegen()
    {
        int rnd;
        if (_TUTORIAL.isTutorial && _TUTORIAL.tutorialStage == 8)
        {
            rnd = 1;
        }
        else
        {
            rnd = Random.Range(1, maxRandomDropChance);
        }
        if (rnd == 1)
        {
            Instantiate(_SETTINGS.general.maegenPickup, transform.position, transform.rotation);
        }
    }
    //public override void Launch()
    //{
    //    base.Launch();
    //}

    public override void Win()
    {
        base.Win();
    }
}
