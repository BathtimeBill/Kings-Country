using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : GameBehaviour
{
    public virtual void Die() {}

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<UnitWeaponCollider>() != null)
        {
            TakeDamage(other.GetComponent<UnitWeaponCollider>().Damage);
        }

        if (other.tag == "PlayerWeapon")
        {
            TakeDamage(_DATA.GetUnit(CreatureID.Satyr).damage);
        }
        if (other.tag == "PlayerWeapon2")
        {
            TakeDamage(_DATA.GetUnit(CreatureID.Orcus).damage);
        }
        //if (other.tag == "PlayerWeapon3")
        //{
        //    if (type != HunterType.Bjornjeger)
        //    {
        //        Launch();
        //    }
        //    else
        //        TakeDamage(_DATA.GetUnit(CreatureID.Leshy).damage);
        //}
        if (other.tag == "PlayerWeapon4")
        {
            TakeDamage(_DATA.GetUnit(CreatureID.Skessa).damage);
        }
        if (other.tag == "PlayerWeapon5")
        {
            TakeDamage(_DATA.GetUnit(CreatureID.Goblin).damage);
        }
        if (other.tag == "PlayerWeapon6")
        {
            TakeDamage(_DATA.GetUnit(CreatureID.Mistcalf).damage);
        }
        if (other.tag == "Spit")
        {
            TakeDamage((int)_GM.spitDamage);
        }
        if (other.tag == "SpitExplosion")
        {
            TakeDamage((int)_GM.spitExplosionDamage);
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

    public virtual void TakeDamage(int _damage)
    {

    }
}
