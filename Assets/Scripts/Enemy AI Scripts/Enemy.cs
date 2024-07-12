using UnityEngine;
using UnityEngine.AI;

public class Enemy : GameBehaviour
{
    public HumanID unitID;
    [Header("Stats")]
    public int health;
    public int maxHealth;
    public int damage;
    public float speed;
    public int maxRandomDropChance;
    [Header("General AI")]
    [HideInInspector] public NavMeshAgent agent;
    public Animator animator;

    public virtual void Awake()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    public virtual void Start()
    {
        Setup();
        GameEvents.ReportOnHumanSpawned(unitID);
    }

    private void Setup()
    {
        agent.speed = _DATA.GetUnit(unitID).speed;
        maxHealth = _DATA.GetUnit(unitID).health;
        health = maxHealth;
        speed = _DATA.GetUnit(unitID).speed;
        damage = _DATA.GetUnit(unitID).damage;
    }

    public virtual void Die(Enemy _unitID, string _killedBy, DeathID _deathID) 
    {
        if(_deathID == DeathID.Regular)
            RagdollDeath();
        if (_deathID == DeathID.Launch)
            LaunchDeath();
        if (_deathID == DeathID.Explosion)
            ExplosionDeath();

        GameEvents.ReportOnEnemyUnitKilled(_unitID, _killedBy);
        Destroy(gameObject);
    }

    public virtual void TakeDamage(int _damage, string _damagedBy)
    {
        Vector3 forward = new Vector3(0, 180, 0);
        GameObject bloodParticle = Instantiate(_DATA.GetUnit(unitID).bloodParticles, transform.position + new Vector3(0, 5, 0), transform.rotation);
        //GameObject bloodParticle = Instantiate(_DATA.GetUnit(unitID).bloodParticles, transform.position, Quaternion.LookRotation(forward));
        health -= _damage;
        if (health <= 0)
            Die(this, _damagedBy, DeathID.Regular);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        UnitWeaponCollider uwc = other.GetComponent<UnitWeaponCollider>();
        if (uwc == null)
            return;

        if (uwc.unitType == UnitType.Human)
            return;

        TakeDamage(uwc.Damage, other.GetComponent<UnitWeaponCollider>().UnitID);

        if (other.tag == "PlayerWeapon") // Satyr
        {
            TakeDamage(_DATA.GetUnit(CreatureID.Satyr).damage, CreatureID.Satyr.ToString());
        }
        if (other.tag == "PlayerWeapon2") //Orcus
        {
            TakeDamage(_DATA.GetUnit(CreatureID.Orcus).damage, CreatureID.Satyr.ToString());
        }
        if (other.tag == "PlayerWeapon3") //Leshy
        {
            if (_DATA.GetUnitType(unitID) == EnemyType.Woodcutter)
                Die(this, _DATA.GetUnit(CreatureID.Leshy).id.ToString(), DeathID.Launch);

            if (_DATA.GetUnitType(unitID) == EnemyType.Hunter)
            {
                if (unitID != HumanID.Bjornjeger)
                    Die(this, _DATA.GetUnit(CreatureID.Leshy).id.ToString(), DeathID.Launch);
                else
                    TakeDamage(_DATA.GetUnit(CreatureID.Leshy).damage, CreatureID.Leshy.ToString());
            }

            if (_DATA.GetUnitType(unitID) == EnemyType.Warrior)
                TakeDamage(_DATA.GetUnit(CreatureID.Leshy).damage, CreatureID.Leshy.ToString());
        }

        if (other.tag == "PlayerWeapon4") //Skessa
        {
            TakeDamage(_DATA.GetUnit(CreatureID.Skessa).damage, CreatureID.Skessa.ToString());
        }
        if (other.tag == "PlayerWeapon5") //Goblin
        {
            TakeDamage(_DATA.GetUnit(CreatureID.Goblin).damage, CreatureID.Goblin.ToString());
        }
        if (other.tag == "PlayerWeapon6") //Mistcals
        {
            TakeDamage(_DATA.GetUnit(CreatureID.Mistcalf).damage, CreatureID.Mistcalf.ToString());
        }
        if (other.tag == "Spit") //Fidhein?
        {
            TakeDamage((int)_GM.spitDamage, "Spit");
            agent.speed = speed / 2;
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
        if (other.tag == "Explosion")
        {
            if (_DATA.GetUnitType(unitID) != EnemyType.Warrior)
                Die(this, "Fyre", DeathID.Explosion);
            else
            {
                TakeDamage(50, "Fyre");
                if (animator != null)
                    animator.SetTrigger("Impact");
            }
            
            //hasArrivedAtBeacon = false;
            //state = EnemyState.Attack;
        }
        if (other.tag == "Explosion2")
        {
            if (_DATA.GetUnitType(unitID) != EnemyType.Warrior)
                Die(this, "Fyre", DeathID.Explosion);
            else
            {
                TakeDamage(100, "Fyre");
                if (animator != null)
                    animator.SetTrigger("Impact");
            }
            //hasArrivedAtBeacon = false;
            //state = EnemyState.Attack;
        }
        //if (other.tag == "Spit")
        //{
        //    navAgent.speed = speed / 2;
        //}
        if (other.tag == "Explosion" || other.tag == "Explosion2")
        {
            if (_HUTM.enemies.Contains(gameObject))
                _HUTM.enemies.Remove(gameObject);
            ExplosionDeath();
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.tag == "Spit")
        {
            agent.speed = speed;
        }
    }

    private void RagdollDeath()
    {
        bool isColliding = false;

        if (_HUTM.enemies.Contains(gameObject))
            _HUTM.enemies.Remove(gameObject);
        if (!isColliding)
        {
            isColliding = true;
            GameObject go;
            go = Instantiate(_DATA.GetUnit(unitID).ragdollModel, transform.position, transform.rotation);
            Destroy(go, 15);
        }
    }

    private void ExplosionDeath()
    {
        GameObject go;
        go = Instantiate(_DATA.GetUnit(unitID).ragdollFireModel, transform.position, transform.rotation);
        go.GetComponentInChildren<Rigidbody>().AddForce(transform.up * 2000);
        go.GetComponentInChildren<Rigidbody>().AddForce(transform.forward * -16000);
        Destroy(go, 15);
    }

    public virtual void LaunchDeath()
    {
        print("Launching");
        bool isColliding = false;
        if (_HUTM.enemies.Contains(gameObject))
            _HUTM.enemies.Remove(gameObject);

        DropMaegen();
        float thrust = 20000f;
        if (!isColliding)
        {
            isColliding = true;
            GameObject go;
            go = Instantiate(_DATA.GetUnit(unitID).ragdollModel, transform.position, transform.rotation);
            go.GetComponentInChildren<Rigidbody>().AddForce(transform.up * thrust);
            go.GetComponentInChildren<Rigidbody>().AddForce(transform.forward * -thrust);
            if(GetComponent<RagdollSound>() != null)
                go.GetComponent<RagdollSound>().hasBeenLaunched = true;
            Destroy(go, 25);
        }
    }

    public virtual void DropMaegen()
    {
        int rnd = Random.Range(1, maxRandomDropChance);
        if (rnd == 1)
        {
            Instantiate(_SETTINGS.general.maegenPickup, transform.position, transform.rotation);
        }
    }
}
