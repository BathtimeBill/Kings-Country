using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : GameBehaviour
{
    public HumanID unitID;

    public virtual void Start()
    {
        _EM.enemies.Add(gameObject);
        GameEvents.ReportOnUnitSpawned(unitID.ToString());
    }

    public virtual void Die(string _unitID, string _killedBy) 
    {
        GameEvents.ReportOnUnitKilled(_unitID, _killedBy);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<UnitWeaponCollider>() != null)
        {
            TakeDamage(other.GetComponent<UnitWeaponCollider>().Damage, other.GetComponent<UnitWeaponCollider>().UnitID);
        }

        if (other.tag == "PlayerWeapon")
        {
            TakeDamage(_DATA.GetUnit(CreatureID.Satyr).damage, CreatureID.Satyr.ToString());
        }
        if (other.tag == "PlayerWeapon2")
        {
            TakeDamage(_DATA.GetUnit(CreatureID.Orcus).damage, CreatureID.Satyr.ToString());
        }

        if (other.tag == "PlayerWeapon4")
        {
            TakeDamage(_DATA.GetUnit(CreatureID.Skessa).damage, CreatureID.Skessa.ToString());
        }
        if (other.tag == "PlayerWeapon5")
        {
            TakeDamage(_DATA.GetUnit(CreatureID.Goblin).damage, CreatureID.Goblin.ToString());
        }
        if (other.tag == "PlayerWeapon6")
        {
            TakeDamage(_DATA.GetUnit(CreatureID.Mistcalf).damage, CreatureID.Mistcalf.ToString());
        }
        if (other.tag == "Spit")
        {
            TakeDamage((int)_GM.spitDamage, "Spit");
        }
        if (other.tag == "SpitExplosion")
        {
            TakeDamage((int)_GM.spitExplosionDamage, "SpitExplosion");
        }
        //if (other.tag == "Beacon")
        //{
        //    animator.SetTrigger("Cheer" + RandomCheerAnim());
        //    hasArrivedAtBeacon = true;
        //}
        //if (other.tag == "Horgr")
        //{
        //    if (!_HM.enemies.Contains(gameObject) && spawnedFromBuilding == false)
        //    {
        //        _HM.enemies.Add(gameObject);
        //        StartCoroutine(WaitForHorgr());
        //    }
        //}
        //if (other.tag == "Explosion")
        //{
        //    TakeDamage(50);
        //    animator.SetTrigger("Impact");
        //    hasArrivedAtBeacon = false;
        //    state = EnemyState.Attack;
        //}
        //if (other.tag == "Explosion2")
        //{
        //    TakeDamage(100);
        //    animator.SetTrigger("Impact");
        //    hasArrivedAtBeacon = false;
        //    state = EnemyState.Attack;
        //}
        //if (other.tag == "Spit")
        //{
        //    navAgent.speed = speed / 2;
        //}
    }

    public virtual void TakeDamage(int _damage, string _damagedBy)
    {

    }
}
