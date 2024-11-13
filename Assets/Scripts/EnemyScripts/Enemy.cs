using UnityEngine;
using UnityEngine.AI;

public class Enemy : GameBehaviour
{
    public HumanID unitID;
    private EnemyData unitData;
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
        unitData = _DATA.GetUnit(unitID);
        agent.speed = unitData.speed;
        maxHealth = unitData.health;
        health = maxHealth;
        speed = unitData.speed;
        damage = unitData.damage;
        _SM.PlaySound(unitData.spawnSounds);
    }

    public virtual void Die(Enemy _unitID, string _killedBy, DeathID _deathID) 
    {
        if(_deathID == DeathID.Regular)
            RagdollDeath();
        if (_deathID == DeathID.Launch)
            LaunchDeath();
        if (_deathID == DeathID.Explosion)
            ExplosionDeath();

        GameEvents.ReportOnHumanKilled(_unitID, _killedBy);
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

        if(uwc.unitType == UnitType.Tool)
        {
            switch (uwc.toolID)
            {
                case ToolID.Fyre:
                    if (_DATA.GetUnitType(unitID) != EnemyType.Warrior)
                        Die(this, _DATA.GetTool(ToolID.Fyre).id.ToString(), DeathID.Explosion);
                    else
                    {
                        TakeDamage(uwc.Damage, _DATA.GetTool(ToolID.Fyre).id.ToString());
                        if (animator != null)
                            animator.SetTrigger("Impact");
                    }
                    break;
                case ToolID.Stormer:
                    if (_DATA.GetUnitType(unitID) != EnemyType.Warrior)
                        Die(this, _DATA.GetTool(ToolID.Stormer).id.ToString(), DeathID.Explosion);
                    else
                    {
                        TakeDamage(uwc.Damage, _DATA.GetTool(ToolID.Stormer).id.ToString());
                        if (animator != null)
                            animator.SetTrigger("Impact");
                    }
                    break;
            }
        }

        if (uwc.unitType == UnitType.Guardian)
        {
            switch(uwc.creatureID)
            {
                case CreatureID.Leshy:
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
                    break;
                case CreatureID.Satyr:
                case CreatureID.Orcus:
                case CreatureID.Huldra:
                case CreatureID.Skessa:
                case CreatureID.Goblin:
                case CreatureID.Mistcalf:
                case CreatureID.Tower:
                case CreatureID.SpitTower:
                    TakeDamage(uwc.Damage, other.GetComponent<UnitWeaponCollider>().UnitID);
                    break;
                case CreatureID.Fidhain:
                    agent.speed = speed / 2;
                    TakeDamage(uwc.Damage, other.GetComponent<UnitWeaponCollider>().UnitID);
                    break;
                default:
                    TakeDamage(uwc.Damage, other.GetComponent<UnitWeaponCollider>().UnitID);
                    break;
            }
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        UnitWeaponCollider uwc = other.GetComponent<UnitWeaponCollider>();
        if (uwc == null)
            return;

        if (uwc.unitType == UnitType.Human)
            return;

        if (uwc.unitType == UnitType.Guardian)
        {
            switch (uwc.creatureID)
            {
                case CreatureID.Fidhain:
                    agent.speed = speed;
                    break;
            }
        }
    }

    private void RagdollDeath()
    {
        bool isColliding = false;

        RemoveFromSites();
        
        if (!isColliding)
        {
            isColliding = true;
            GameObject go = Instantiate(unitData.ragdollModel, transform.position, transform.rotation);
            go.GetComponent<Ragdoll>().Die(ArrayX.GetRandomItemFromArray(unitData.dieSounds));
            Destroy(go, 15);
        }
    }

    private void ExplosionDeath()
    {
        RemoveFromSites();
        GameObject go = Instantiate(unitData.ragdollModel, transform.position, transform.rotation);
        Ragdoll ragdoll = go.GetComponent<Ragdoll>();
        ragdoll.Die(ArrayX.GetRandomItemFromArray(unitData.dieSounds), true);
        ragdoll.Launch(2000, -16000);
        Destroy(go, 15);
    }

    public virtual void LaunchDeath()
    {
        bool isColliding = false;
        RemoveFromSites();
        DropMaegen();
        float thrust = 20000f;
        if (!isColliding)
        {
            print("Launching");
            isColliding = true;
            GameObject go = Instantiate(unitData.ragdollModel, transform.position, transform.rotation);
            Ragdoll ragdoll = go.GetComponent<Ragdoll>();
            ragdoll.Die(ArrayX.GetRandomItemFromArray(unitData.dieSounds));
            ragdoll.Launch(20000, -20000);
            Destroy(go, 25);
        }
    }

    private void RemoveFromSites()
    {
        if (_hutExists)
            _HUT.RemoveEnemy(this);
        
        if (_horgrExists)
            _HORGR.RemoveEnemy(this);
    }

    public virtual void DropMaegen()
    {
        int rnd = Random.Range(1, maxRandomDropChance);
        if (rnd == 1)
        {
            Instantiate(_SETTINGS.general.maegenPickup, transform.position, transform.rotation);
        }
    }

    public Transform GetClosestUnit()
    {
        float closestDistance = Mathf.Infinity;
        Transform trans = null;

        foreach (Unit unit in _UM.unitList)
        {
            float currentDistance;
            currentDistance = Vector3.Distance(transform.position, unit.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                trans = unit.transform;
            }
        }

        if (_UM.unitList == null)
        {
            return null;
        }
        else
            return trans;
    }
}
