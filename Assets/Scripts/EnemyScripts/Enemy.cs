using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : GameBehaviour
{
    public HumanID unitID;
    [HideInInspector] public EnemyData unitData;
    public EnemyState state;
    public HealthBar healthBar;
    public EnemyAnimation enemyAnimation;
    public bool spawnedFromSite;
    [Header("Stats")]
    private int health;
    private int maxHealth;
    private int damage;
    private float speed;
    public int maxRandomDropChance;
    [Header("General AI")]
    [HideInInspector] public NavMeshAgent agent;
    public Animator animator;
    public float tickRate = 0.5f;
    public Transform closestUnit;
    public float distanceFromClosestUnit;
    public Transform targetObject;
    public float distanceToTarget;
    [Header("Weapons")]
    public Collider weaponCollider;
    [Header("Audio")]
    public GameObject SFXPool;
    private int soundPoolCurrent;
    private AudioSource[] soundPool;
    [Header("Debug")] 
    public DebugUnit debugUnit;
    private bool invincible = true;
    
    #region Getters & Setters
    [HideInInspector] private float attackRangeValue;
    public float attackRange
    {
        get { return attackRangeValue; }
        set {attackRangeValue = value; UpdateDebug(); }
    }
    private float detectRangeValue;
    public float detectRange
    {
        get { return detectRangeValue; }
        set {detectRangeValue = value; UpdateDebug(); }
    }
    private float stoppingDistanceValue;
    public float stoppingDistance
    {
        get { return stoppingDistanceValue; }
        set {stoppingDistanceValue = value; UpdateDebug(); }
    }
    private void UpdateDebug()
    {
        if (!debugUnit)
            return;
        debugUnit.AdjustRange(detectRange, attackRange, stoppingDistance);
    }
    #endregion
    
    #region Initialization
    public virtual void Awake()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        soundPool = SFXPool.GetComponents<AudioSource>();
        unitData = _DATA.GetUnit(unitID);
    }

    public virtual void Start() { Setup();}

    public void Setup()
    {
        maxHealth = unitData.health;
        health = maxHealth;
        speed = unitData.speed;
        agent.speed = speed;
        damage = unitData.damage;
        attackRange = unitData.attackRange;
        detectRange = unitData.detectRange;
        stoppingDistance = unitData.stoppingDistance;
        agent.stoppingDistance = stoppingDistance;
        _SM.PlaySound(unitData.spawnSound);
        if(healthBar != null)
            healthBar.AdjustHealthBar(health, maxHealth);
        StartCoroutine(WaitForInvincible());
        GameEvents.ReportOnHumanSpawned(unitID);
        ChangeState(EnemyState.Work);
        StartCoroutine(Tick());
    }
    
    private IEnumerator WaitForInvincible()
    {
        invincible = true;
        yield return new WaitForSeconds(5);
        invincible = false;
    }

    
    #endregion
    
    #region AI
    public IEnumerator Tick()
    {
        if (_GM.gameState == GameState.Lose)
            StopAllCoroutines();
        
        SetState();
        
        yield return new WaitForSeconds(tickRate);
        StartCoroutine(Tick());
    }
    public virtual void SetState() { }
    
    public void ChangeState(EnemyState _state)
    {
        state = _state;
        healthBar.ChangeUnitState(state.ToString());
        if(state == EnemyState.Attack)
            DoAttack();
    }

    public void HandleState()
    {
        agent.SetDestination(targetObject.position);
        distanceToTarget = Vector3.Distance(targetObject.transform.position, transform.position);
        print("State: " + state + " | Target: " + targetObject.name);
        enemyAnimation.PlayWalkAnimation(agent.velocity.magnitude);
        
        switch (state)
        {
            case EnemyState.Work:
                HandleWorkState();
                break;
            case EnemyState.Idle:
                HandleIdleState();
                break;
            case EnemyState.Attack:
                HandleAttackState();
                break;
            case EnemyState.ClaimSite:
                HandleClaimState();
                break;
            case EnemyState.DefendSite:
                HandleDefendState();
                break;
            case EnemyState.Victory:
                HandleVictoryState();
                break;
        }
    }
    
    public virtual void HandleWorkState()
    {
        if (!targetObject)
            return;

        if (TargetWithinRange)
        {
            ChangeState(EnemyState.Attack);
        }
    }

    public virtual void HandleIdleState()
    {
        StandStill();
    }
    public virtual void HandleAttackState() 
    { 
        if (!TargetWithinRange)
            ChangeState(EnemyState.Work);
        else
            DoAttack();
    }
    public virtual void HandleClaimState() { }

    public virtual void HandleDefendState()
    {
        StandStill(); 
    }
    public virtual void HandleVictoryState()
    {
        StandStill();
        StopAllCoroutines();
        enemyAnimation.StopAllCoroutines();
        enemyAnimation.PlayVictoryAnimation();
    }
    //This ensures that we move into the attack animation
    public void DoAttack()
    {
        StandStill();
        SmoothFocusOnTarget(targetObject);
        enemyAnimation.PlayAttackAnimation();
    }
    public void StandStill() => agent.SetDestination(transform.position);
    public bool TargetWithinRange => distanceToTarget < attackRange;
    public void SmoothFocusOnTarget(Transform _target)
    {
        if (!_target)
            return;
        var targetRotation = Quaternion.LookRotation(_target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
    }
    public void SetClosestUnit()
    {
        closestUnit = GetClosestUnit();
        distanceFromClosestUnit = !closestUnit ? 20000 : Vector3.Distance(closestUnit.transform.position, transform.position);
    }
    private void FixedUpdate()
    {
        if(state == EnemyState.Attack || state == EnemyState.Work || state == EnemyState.ClaimSite)
        {
            SmoothFocusOnTarget(targetObject);
        }
    }
    #endregion
    
    #region Triggers
    public virtual void OnTriggerEnter(Collider other)
    {
        if (invincible)
            return;
        
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
                        enemyAnimation.PlayImpactAnimation();
                    }
                    break;
                case ToolID.Stormer:
                    if (_DATA.GetUnitType(unitID) != EnemyType.Warrior)
                        Die(this, _DATA.GetTool(ToolID.Stormer).id.ToString(), DeathID.Explosion);
                    else
                    {
                        TakeDamage(uwc.Damage, _DATA.GetTool(ToolID.Stormer).id.ToString());
                        enemyAnimation.PlayImpactAnimation();
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
                case CreatureID.FidhainTower:
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
        if (invincible)
            return;
        
        UnitWeaponCollider uwc = other.GetComponent<UnitWeaponCollider>();
        if (!uwc)
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
    
    #endregion

    #region Damage/Death
    public virtual void TakeDamage(int _damage, string _damagedBy)
    {
        if (invincible)
            return;
        
        Vector3 forward = new Vector3(0, 180, 0);
        GameObject bloodParticle = Instantiate(_DATA.GetUnit(unitID).bloodParticles, transform.position + new Vector3(0, 5, 0), transform.rotation);
        //GameObject bloodParticle = Instantiate(_DATA.GetUnit(unitID).bloodParticles, transform.position, Quaternion.LookRotation(forward));
        health -= _damage;
        if(healthBar != null) 
            healthBar.AdjustHealthBar(health, maxHealth);
        if (health <= 0)
        {
            Die(this, _damagedBy, DeathID.Regular);
        }
        else
        {
            PlaySound(unitData.hitSounds);
        }
    }

    public virtual void Die(Enemy _enemy, string _killedBy, DeathID _deathID) 
    {
        RemoveFromSites();
        _EM.RemoveEnemy(_enemy, _killedBy, _deathID, transform.position, transform.rotation);
        if(_deathID == DeathID.Launch)
            DropMaegen();
    }
    
    #endregion
    private void RemoveFromSites()
    {
        if (_HutExists)
            _HUT.RemoveEnemy(this);
        
        if (_HorgrExists)
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
    
    #region Sound
    public void PlaySound(AudioClip[] _clips)
    {
        if (_clips.Length == 0)
            return;
        
        AudioClip clip = ArrayX.GetRandomItemFromArray(_clips);
        PlaySound(clip);
    }
    public void PlaySound(AudioClip _clip)
    {
        soundPoolCurrent = ArrayX.IncrementCounter(soundPoolCurrent, soundPool);
        AudioX.PlaySound(_clip, soundPool[soundPoolCurrent]);
    }
    #endregion

    #region Animation Events
    public void Footstep(string _foot) => PlaySound(unitData.footstepSounds);
    public virtual void Attack(int _attack) { }
    
    #endregion
    
    public virtual void Win(){}
    private void OnGameOver()
    {
        Win();
        StopCoroutine(Tick());
        ChangeState(EnemyState.Victory);
    }

    private void OnEnable()
    {
        GameEvents.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= OnGameOver;
    } 
}
