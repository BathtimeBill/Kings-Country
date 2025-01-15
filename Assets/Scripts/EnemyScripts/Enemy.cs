using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : GameBehaviour
{
    public EnemyID enemyID;
    [HideInInspector] public EnemyData enemyData;
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
    public float distanceToClosestUnit;
    public Transform targetObject;
    public float distanceToTarget;
    public bool canAttack;
    [Header("Weapons")]
    public Collider weaponCollider;
    [Header("Audio")]
    public GameObject SFXPool;
    private int soundPoolCurrent;
    private AudioSource[] soundPool;
    [Header("Debug")] 
    public DebugUnit debugUnit;

    private bool invincible = true;
    private bool initializeHack = false;
    
    #region Getters & Setters
    [HideInInspector] private float attackRangeValue;
    public float attackRange
    {
        get { return attackRangeValue; }
        set {attackRangeValue = value; UpdateDebug(); }
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
        debugUnit.AdjustRange(attackRange, stoppingDistance);
    }
    #endregion
    
    #region Initialization
    public virtual void Awake()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        soundPool = SFXPool.GetComponents<AudioSource>();
        enemyData = _DATA.GetEnemy(enemyID);
    }

    public virtual void Start() { Setup();}

    public void Setup()
    {
        maxHealth = enemyData.health;
        health = maxHealth;
        speed = enemyData.speed;
        agent.speed = speed;
        damage = enemyData.damage;
        attackRange = enemyData.attackRange;
        stoppingDistance = enemyData.stoppingDistance;
        agent.stoppingDistance = stoppingDistance;
        _SM.PlaySound(enemyData.spawnSound);
        if(NotNull(healthBar))
            healthBar.AdjustHealthBar(health, maxHealth);
        StartCoroutine(WaitForInvincible());
        GameEvents.ReportOnHumanSpawned(enemyID);
        ChangeState(EnemyState.Work);
        StartCoroutine(Tick());
        ExecuteAfterSeconds(1f, () => initializeHack = true);

    }
    private IEnumerator WaitForInvincible()
    {
        invincible = true;
        yield return new WaitForSeconds(enemyData.invincibleTime);
        invincible = false;
    }
    #endregion
    
    #region AI
    public IEnumerator Tick()
    {
        UpdateDistances();
        DetermineState();
        HandleMovement();
        if(initializeHack)
            HandleAttackState();
        
        //print("State: " + state + " | Target: " + targetObject.name);
        yield return new WaitForSeconds(tickRate);
        if (_GAME.gameState == GameState.Lose)
            HandleVictoryState();
        else
            StartCoroutine(Tick());
    }
    public virtual void UpdateDistances()
    {
        closestUnit = GetClosestUnit();
        distanceToClosestUnit = !closestUnit ? 20000 : Vector3.Distance(closestUnit.transform.position, transform.position);
    }
    public virtual void DetermineState() { }

    public void HandleMovement()
    {
        agent.SetDestination(targetObject.position);
        distanceToTarget = Vector3.Distance(targetObject.transform.position, transform.position);
        enemyAnimation.PlayWalkAnimation(agent.velocity.magnitude);
    }
    
    public void ChangeState(EnemyState _state)
    {
        state = _state;
        if (NotNull(healthBar))
            healthBar.ChangeUnitState(state.ToString());
    }

    public virtual void HandleWorkState()
    {
        ChangeState(EnemyState.Work);
    }
    public virtual void HandleIdleState() { }

    public virtual void HandleDefendState()
    {
        StandStill(); 
        ChangeState(EnemyState.DefendSite);
    }

    public virtual void HandleAttackState()
    {
        bool attacking = agent.velocity == Vector3.zero || canAttack; 
        if(attacking) 
            ChangeState(EnemyState.Attack);
        else
            ChangeState(EnemyState.Work);
        enemyAnimation.PlayAttackAnimation(attacking);
    }
    public void HandleVictoryState()
    {
        ChangeState(EnemyState.Victory);
        StandStill();
        StopAllCoroutines();
        enemyAnimation.PlayVictoryAnimation();
    }
    public void StandStill() => agent.SetDestination(transform.position);
    public bool TargetWithinAttackRange => distanceToTarget < attackRange;
    
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
        distanceToClosestUnit = !closestUnit ? 20000 : Vector3.Distance(closestUnit.transform.position, transform.position);
    }
    private void FixedUpdate()
    {
        if(state == EnemyState.Attack || state == EnemyState.Work && targetObject != transform)
        {
            SmoothFocusOnTarget(targetObject);
        }
        if(Input.GetKeyDown(KeyCode.N))
            Die(this, GuardianID.Orcus.ToString(), DeathID.Launch);
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
                    if (_DATA.GetEnemyType(enemyID) != EnemyType.Warrior)
                        Die(this, _DATA.GetTool(ToolID.Fyre).id.ToString(), DeathID.Explosion);
                    else
                    {
                        TakeDamage(uwc.Damage, _DATA.GetTool(ToolID.Fyre).id.ToString());
                        enemyAnimation.PlayImpactAnimation();
                    }
                    break;
                case ToolID.Stormer:
                    if (_DATA.GetEnemyType(enemyID) != EnemyType.Warrior)
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
            switch(uwc.guardianID)
            {
                case GuardianID.Leshy:
                    if (_DATA.GetEnemyType(enemyID) == EnemyType.Woodcutter)
                        Die(this, _DATA.GetUnit(GuardianID.Leshy).id.ToString(), DeathID.Launch);

                    if (_DATA.GetEnemyType(enemyID) == EnemyType.Hunter)
                    {
                        if (enemyID != EnemyID.Bjornjeger)
                            Die(this, _DATA.GetUnit(GuardianID.Leshy).id.ToString(), DeathID.Launch);
                        else
                            TakeDamage(_DATA.GetUnit(GuardianID.Leshy).damage, GuardianID.Leshy.ToString());
                    }

                    if (_DATA.GetEnemyType(enemyID) == EnemyType.Warrior)
                        TakeDamage(_DATA.GetUnit(GuardianID.Leshy).damage, GuardianID.Leshy.ToString());
                    break;
                case GuardianID.Satyr:
                case GuardianID.Orcus:
                case GuardianID.Huldra:
                case GuardianID.Skessa:
                case GuardianID.Goblin:
                case GuardianID.Mistcalf:
                case GuardianID.Tower:
                case GuardianID.FidhainTower:
                    TakeDamage(uwc.Damage, other.GetComponent<UnitWeaponCollider>().UnitID);
                    break;
                case GuardianID.Fidhain:
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
            switch (uwc.guardianID)
            {
                case GuardianID.Fidhain:
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
        
        if (NotNull(enemyData.hitParticles))
        {
            GameObject hitParticle = Instantiate(enemyData.hitParticles, transform.position + new Vector3(0, 1, 0), transform.rotation);
            Destroy(hitParticle, 5);
        }
        //GameObject bloodParticle = Instantiate(_DATA.GetUnit(unitID).bloodParticles, transform.position, Quaternion.LookRotation(forward));
        health -= _damage;
        if(NotNull(healthBar)) 
            healthBar.AdjustHealthBar(health, maxHealth);
        if (health <= 0)
        {
            Die(this, _damagedBy, DeathID.Regular);
        }
        else
        {
            PlaySound(enemyData.hitSounds);
        }
    }

    public virtual void Die(Enemy _enemy, string _killedBy, DeathID _deathID) 
    {
        RemoveFromSites();
        StopAllCoroutines();
        agent.enabled = false;
        animator.enabled = false;
        ComponentX.Disable<Outline>(gameObject);
        
        if (NotNull(enemyData.dieParticles))
        {
            GameObject dieParticle = Instantiate(enemyData.dieParticles, transform.position + new Vector3(0, 1, 0),
                transform.rotation);
            Destroy(dieParticle, 5);
        }
        
        Ragdoll ragdoll = GetComponent<Ragdoll>();
        switch (_deathID)
        {
            case DeathID.Regular:
                ragdoll.Die(ArrayX.GetRandomItemFromArray(_enemy.enemyData.dieSounds));
                ragdoll.Launch(0, 0);
                break;
            case DeathID.Explosion:
                ragdoll.Die(ArrayX.GetRandomItemFromArray(_enemy.enemyData.dieSounds), true);
                ragdoll.Launch(10000, -16000);
                break;
            case DeathID.Launch:
                ragdoll.Die(ArrayX.GetRandomItemFromArray(_enemy.enemyData.dieSounds));
                ragdoll.Launch(6000, -10000);
                break;
        }
        
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

        foreach (Guardian unit in _GM.guardianList)
        {
            float currentDistance;
            currentDistance = Vector3.Distance(transform.position, unit.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                trans = unit.transform;
            }
        }

        if (_GM.guardianList == null)
            return null;
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
    public void Footstep(string _foot) => PlaySound(enemyData.footstepSounds);
    public virtual void Attack(int _attack) { }
    
    #endregion
    
    public virtual void Win(){}
    private void OnGameOver()
    {
        HandleVictoryState();
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
