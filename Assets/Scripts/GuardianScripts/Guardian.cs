using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Guardian : GameBehaviour
{
    [Header("Guardian ID")]
    public GuardianID guardianID;
    
    [Header("UI")]
    public HealthBar healthBar;
    public SelectionRing selectionRing;
    
    [Header("Components")]
    public NavMeshAgent navAgent;
    public Animator animator;
    [Header("AI")]
    public UnitState state;
    private float spawnInMoveDistance = 12f;
    private GameObject trackTarget;
    private Transform pointer;
    
    [Header("Weapon Objects")]
    public Collider[] attackColliders;
    [Header("Particles")]
    public ParticleSystem attackParticles;
    public ParticleSystem footstepLeftParticles;
    public ParticleSystem footstepRightParticles;
    public GameObject healingParticle;
    [Header("Bools")]
    public bool isSelected;
    public bool inCombat;
    public bool isMovingCheck;
    private bool isTooCloseToTower;
    private bool hitByArrow;
    public bool isFirstPerson;
    [Header("Audio")]
    public GameObject SFXPool;
    private int soundPoolCurrent;
    private AudioSource[] soundPool;
    
    [Header("Debug")]
    public DebugUnit debugUnit;

    [Header("Body")] 
    public Transform leftHand;
    public Transform rightHand;
    public Transform leftFoot;
    public Transform rightFoot;
    
    //Stats
    private float health;
    private float maxHealth;
    private float unitSpeed;
    private float focusSpeed = 5f;
    
    //Combat Mode
    private CombatMode combatMode;
    private Vector3 defendPosition;
    
    //AI
    private Transform closestEnemy;
    [HideInInspector] public float distanceToClosestEnemy;
    private GuardianAnimation guardianAnimation;

    public Transform ClosestEnemy => closestEnemy;
    [HideInInspector] public GuardianData guardianData;
    
    //Misc
    private int startingDay;
    private GameObject hitParticle => _DATA.GetUnit(guardianID).hitParticles;
    private GameObject dieParticle => _DATA.GetUnit(guardianID).dieParticles;
    
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
        set {stoppingDistanceValue = value; UpdateDebug(); SetStoppingDistance(); }
    }
    private void UpdateDebug()
    {
        if (!debugUnit)
            return;
        debugUnit.AdjustRange(detectRange, attackRange, stoppingDistance);
    }
    public void SetStoppingDistance() => navAgent.stoppingDistance = stoppingDistance;
    #endregion

    public void Awake()
    {
        guardianAnimation = GetComponentInChildren<GuardianAnimation>();
    }

    public virtual void Start()
    {
        guardianData = _DATA.GetUnit(guardianID);
        if(!isFirstPerson)
        {
            soundPool = SFXPool.GetComponents<AudioSource>();
            pointer = _Pointer;
            Setup();
            GameEvents.ReportOnGuardianSpawned(guardianID);
            SpawnInMove();
            startingDay = _CurrentDay;
        }
        _UM.unitList.Add(this);
    }

    public virtual void Setup()
    {
        //Health
        maxHealth = guardianData.health;
        if (_DATA.HasPerk(PerkID.BarkSkin))
            maxHealth = MathX.GetPercentageIncrease(maxHealth, _DATA.GetPerk(PerkID.BarkSkin).increaseValue);
        SetHealth(maxHealth);

        //Speed
        unitSpeed = guardianData.speed;
        if (_DATA.HasPerk(PerkID.FlyFoot))
            unitSpeed = MathX.GetPercentageIncrease(unitSpeed, _DATA.GetPerk(PerkID.FlyFoot).increaseValue);
        navAgent.speed = unitSpeed;

        //Detection
        attackRange = guardianData.attackRange;
        detectRange = guardianData.detectRange;
        stoppingDistance = guardianData.stoppingDistance;
        navAgent.stoppingDistance = stoppingDistance;

        //Other
        healthBar.ChangeCombatModeIcon(_ICONS.attackIcon);
        healthBar.ChangeGroupNumber("");
        
        UpdateDebug();
    }

    IEnumerator WaitForIsMovingCheck()
    {
        yield return new WaitForSeconds(0.1f);
        if(guardianAnimation.currentSpeed == 0)
        {
            state = UnitState.Idle;
        }    
    }
    
    private void SmoothFocusOnEnemy()
    {
        var targetRotation = Quaternion.LookRotation(closestEnemy.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, focusSpeed * Time.deltaTime);
    }

    void Update()
    {
        if (isFirstPerson || !navAgent)
            return;

        if (_EnemiesExist)
            UpdateClosestEnemy();

        HandleState();
    }

    #region AI
    private void UpdateClosestEnemy()
    {
        closestEnemy = _NoEnemies ? transform : ObjectX.GetClosest(gameObject, _EM.enemies).transform;
        distanceToClosestEnemy = Vector3.Distance(closestEnemy.transform.position, transform.position);
    }

    private void HandleState()
    {
        healthBar.ChangeUnitState(state.ToString());
        switch (state)
        {
            case UnitState.Idle:
                HandleIdleState();
                break;
            case UnitState.Attack:
                HandleAttackState();
                break;
            case UnitState.Moving:
                HandleMovingState();
                break;
            case UnitState.Track:
                HandleTrackState();
                break;
            case UnitState.Focus:
                HandleFocusState();
                break;
        }
    }

    private void HandleIdleState()
    {
        if (_NoEnemies)
            return;
        
        if (distanceToClosestEnemy < detectRange)
        {
            state = UnitState.Focus;
        }
        else if (combatMode == CombatMode.Defend)
        {
            navAgent.SetDestination(defendPosition);
        }
    }
    
    //Within the detect range
    public virtual void HandleFocusState()
    {
        if (_NoEnemies)
        {
            state = UnitState.Idle;
            return;
        }

        if (distanceToClosestEnemy < attackRange)
            state = UnitState.Attack;
        else if (distanceToClosestEnemy < detectRange || hitByArrow)
        {
            navAgent.SetDestination(closestEnemy.transform.position);
            SmoothFocusOnEnemy();
        }
        else if (distanceToClosestEnemy >= detectRange && !hitByArrow)
        {
            state = UnitState.Moving;
        }
    }

    //Within the attack range
    public virtual void HandleAttackState()
    {
        if (_NoEnemies)
        {
            state = UnitState.Idle;
            return;
        }

        if (distanceToClosestEnemy >= attackRange && !hitByArrow)
        {
            guardianAnimation.CheckAttack();
            state = UnitState.Focus;
        }
        
        navAgent.SetDestination(closestEnemy.transform.position);
        guardianAnimation.PlayAttack();
        SmoothFocusOnEnemy();
    }

    public virtual void HandleMovingState()
    {
        if (!isMovingCheck)
        {
            isMovingCheck = true;
            StartCoroutine(WaitForIsMovingCheck());
        }
        
        if (Vector3.Distance(pointer.position, transform.position) <= stoppingDistance)
        {
            state = UnitState.Idle;
        }

        if (combatMode == CombatMode.AttackMove && distanceToClosestEnemy < detectRange)
        {
            state = UnitState.Focus;
        }
    }

    //Targeting specific location
    public virtual void HandleTrackState()
    {
        if(!trackTarget)
            state = UnitState.Idle;
        else
        {
            navAgent.SetDestination(trackTarget.transform.position);
            if (Vector3.Distance(transform.position, trackTarget.transform.position) <= attackRange)
                state = UnitState.Attack;
        }
    }

    
    #endregion

    #region Sound
    //Look to move into SFXPool script
    public void PlaySound(AudioClip[] _clips)
    {
        if (_clips.Length == 0)
            return;
        
        AudioClip clip = ArrayX.GetRandomItemFromArray(_clips);
        PlaySound(clip);
    }
    public void PlaySound(AudioClip _clip)
    {
        if (!_clip)
            return;
        
        soundPoolCurrent = ArrayX.IncrementCounter(soundPoolCurrent, soundPool);
        AudioX.PlaySound(_clip, soundPool[soundPoolCurrent]);
    }
    #endregion
    
    #region Health
    private void IncreaseHealth(float _health)
    {
        health += _health;
        if(health > maxHealth) health = maxHealth;
        healthBar.AdjustHealthBar(health, maxHealth);
    }

    private void DecreaseHealth(float _health)
    {
        health -= _health;
        healthBar.AdjustHealthBar(health, maxHealth);
    }

    private void SetHealth(float _health)
    {
        health = _health;
        healthBar.AdjustHealthBar(health, maxHealth);
    }
    #endregion

    #region UI
    private void ToggleSelectionCircle(bool _on) => selectionRing.Select(_on);
    #endregion

    IEnumerator HitByArrowDelay()
    {
        yield return new WaitForSeconds(1);
        hitByArrow = false;
        detectRange = detectRange / 2;
        state = UnitState.Moving;
        navAgent.SetDestination(transform.position);
    }
    void SpawnInMove() => navAgent.SetDestination(SpawnX.GetSpawnPositionInRadius(this.transform.position, spawnInMoveDistance));
    
    private void OnTriggerEnter(Collider other)
    {
        WeaponLogic(other);

        if(other.CompareTag("Heal"))
        {
            if(guardianID != GuardianID.Mistcalf)
                IncreaseHealth(100);
        }
        if (other.CompareTag("Maegen"))
        {
            _GM.maegen += 1;
        }
        if (other.CompareTag("Horgr"))
        {
            if(_HorgrExists)
                _HORGR.AddUnit(this);
        }
        if (other.CompareTag("Hut"))
        {
            if (_HutExists)
                _HUT.AddUnit(this);
        }
        if(other.CompareTag("Tower"))
        {
            isTooCloseToTower = true;
        }
        if (other.CompareTag("Rune"))
        {
            if (guardianID != GuardianID.Mistcalf)
            {
                healingParticle.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Horgr") && _HorgrExists)
            _HORGR.RemoveUnit(this);
        
        if(other.CompareTag("Hut") && _HutExists)
            _HUT.RemoveUnit(this);
        
        if (other.CompareTag("Tower"))
            isTooCloseToTower = false;
        
        if (guardianID != GuardianID.Mistcalf)
            healingParticle.SetActive(false);

    }

    private void OnTriggerStay(Collider other)
    {
        
        if(other.CompareTag("Rune"))
        {
            if (guardianID != GuardianID.Mistcalf)
            {
                float healRate = _GM.runeHealRate * Time.deltaTime;
                if (_DATA.HasPerk(PerkID.Rune))
                {
                    healRate *= 2;
                }
                IncreaseHealth(healRate);
            }
        }
        if (!other.GetComponent<UnitWeaponCollider>())
            return;
        
        if (other.GetComponent<UnitWeaponCollider>().enemyID == EnemyID.LogCutter)
        {
            DecreaseHealth(0.5f * Time.deltaTime);
            if (health < 0)
            {
                Die("Unknown");
            }
        }
    }

    private void WeaponLogic(Collider _other)
    {
        UnitWeaponCollider uwc = _other.GetComponent<UnitWeaponCollider>();

        if (!uwc)
            return;

        string attacker = _DATA.GetEnemy(uwc.enemyID).ToString();
        float damage = _DATA.GetEnemy(uwc.enemyID).damage;

        switch (uwc.enemyID)
        {
            case EnemyID.Logger:
                TakeDamage(attacker, damage); 
                break;
            case EnemyID.Lumberjack:
                TakeDamage(attacker, damage);
                break;
            case EnemyID.Dreng:
                if (guardianID == GuardianID.Leshy) { attacker = "Unknown"; damage = 50; }
                TakeDamage(attacker, damage);
                break;
            case EnemyID.Berserkr:
                if (guardianID == GuardianID.Leshy) { attacker = "Unknown"; damage = 65; }
                TakeDamage(attacker, damage);
                break;
            case EnemyID.Wathe:
                HitByArrow();
                if (guardianID == GuardianID.Skessa) { damage *=3; }
                TakeDamage(attacker, damage);
                break;
            case EnemyID.Poacher:
                HitByArrow();
                if (guardianID == GuardianID.Skessa) { damage *= 3; }
                TakeDamage(attacker, damage);
                break;
            case EnemyID.Lord:
                state = UnitState.Attack;
                HitByArrow();
                TakeDamage(attacker, damage);
                break;
            case EnemyID.Dog:
                HitByArrow();
                TakeDamage(attacker, damage);
                break;
        }

        _other.enabled = false;
    }

    private void HitByArrow()
    {
        hitByArrow = true;
        detectRange = detectRange * 2;
        state = UnitState.Attack;
        StartCoroutine(HitByArrowDelay());
    }

    public void TakeDamage(string attacker, float damage)
    {
        state = UnitState.Attack;
        GameObject go = Instantiate(hitParticle, transform.position + new Vector3(0, 5, 0), transform.rotation);
        //go.transform.rotation = Quaternion.Inverse(transform.rotation);
        DecreaseHealth(damage);
        if (health <= 0)
            Die(attacker);
    }

    private void Die(string _attacker)
    {
        RemoveFromSites();
        _UM.Deselect(this);
        _UM.unitList.Remove(this);
        bool isColliding = false;
        if(!isColliding)
        {
            GameObject go = Instantiate(guardianData.ragdollModel, transform.position, transform.rotation);
            isColliding = true;
            Destroy(go, 15);
        }
        _UI.CheckPopulousUI();
        int daysSurvived = _CurrentDay - startingDay;
        GameEvents.ReportOnGuardianKilled(guardianID.ToString(), _attacker, daysSurvived);
        _UM.RemoveGuardian(this, transform.position, transform.rotation);
    }
    
    private void RemoveFromSites()
    {
        if (_HutExists)
            _HUT.RemoveUnit(this);
        
        if (_HorgrExists)
            _HORGR.RemoveUnit(this);
    }
    
    public void SetDestination(Transform _destination) => navAgent.SetDestination(_destination.position);
    
    private void OnContinueButton()
    {
        if(guardianID != GuardianID.Mistcalf)
        {
            SetHealth(maxHealth);
        }
    }
    
    public virtual void Attack(int _attack)
    {
    }
    public virtual void StopAttack(int _attack){}
    
    #region Animation Events

    public void Footstep(string _foot)
    {
        PlaySound(guardianData.footstepSounds);

        if (_foot == "Left")
            ParticlesX.PlayParticles(footstepLeftParticles, leftFoot.position);
        else
            ParticlesX.PlayParticles(footstepRightParticles, rightFoot.position);
    }

    public void PlayParticle(Transform _transform)
    {
        //TODO pool Particles
        Instantiate(hitParticle, _transform.position, Quaternion.Euler(0, 0, 0));
    }
    
    public void PlayParticle(){}
    
    #endregion
    
    #region Combat Buttons
    private void OnCombatSelected(CombatID _combatID)
    {
        if (!isSelected)
            return;
        
        switch (_combatID)
        {
            case CombatID.Attack: 
                AttackSelected();
                break;
            case CombatID.Defend:
                DefendSelected();
                break;
            case CombatID.Formation:
                break;
            case CombatID.Stop:
                StopSelected();
                break;
        }
    }
    
    private void AttackSelected()
    {
        healthBar.ChangeCombatModeIcon(_ICONS.attackIcon);
        if(combatMode != CombatMode.Move || combatMode != CombatMode.AttackMove)
        {
            detectRange = detectRange * 2;
            if (guardianID == GuardianID.Goblin)
            {
                navAgent.speed = guardianData.speed;
            }
        }
        combatMode = CombatMode.Move;
    }
    
    private void DefendSelected()
    {
        if (combatMode != CombatMode.Defend)
        {
            if (guardianID != GuardianID.Goblin)
            {
                detectRange = detectRange / 2;
            }
            healthBar.ChangeCombatModeIcon(_ICONS.defendIcon);
        }
        defendPosition = transform.position;
        combatMode = CombatMode.Defend;
    }

    private void StopSelected()
    {
        healthBar.ChangeCombatModeIcon(_ICONS.stopIcon);
        SetDestination(transform);
    }
    
    #endregion
    
    #region Input
    private void OnTowerButton()
    {
        if (guardianID != GuardianID.Fidhain || guardianID != GuardianID.Fidhain)
            return;

        if (!isSelected)
            return;

        if (isTooCloseToTower)
        {
            _UI.SetError(ErrorID.TooCloseToTower);
        }
        else
        {
            Tower();
            _UM.Deselect(this);
            _UM.unitList.Remove(this);
            Destroy(gameObject);
        }
    }

    protected virtual void Tower(){}

    private void OnSuicideButton()
    {
        if(isSelected)
        {
            if (_HUT != null) _HUT.RemoveUnit(this);
            if (_HORGR != null) _HORGR.RemoveUnit(this);
            _UM.Deselect(this);
            _UM.unitList.Remove(this);
            GameObject go = Instantiate(guardianData.ragdollModel, transform.position, transform.rotation);
            Destroy(go, 15);
            _UI.CheckPopulousUI();
            int daysSurvived = _CurrentDay - startingDay;
            GameEvents.ReportOnGuardianKilled(guardianID.ToString(), "Unknown", daysSurvived);
            _UM.RemoveSelectedUnit(this);
            GameEvents.ReportOnObjectSelected(null);
            Destroy(gameObject);
        }
    }
    
    private void OnDeselectButtonPressed()
    {
        if (isSelected)
        {
            //if (Input.GetMouseButtonDown(1))
            {
                StopAllCoroutines();
                GameEvents.ReportOnUnitMove();
                if (_PC.mouseOverEnemy)
                {
                    trackTarget = _PC.mouseOverEnemy;
                    state = UnitState.Track;
                    _PC.mouseOverEnemy.GetComponent<OutlineFlash>().BeginFlash();
                    _SM.PlaySound(_SM.targetEnemySound);
                }
                else
                {
                    state = UnitState.Moving;
                }
            }
        }
        else
        {
            ToggleSelectionCircle(false);
        }
    }
    
    #endregion
    
    private void OnEnable()
    {
        GameEvents.OnCombatSelected += OnCombatSelected;
        GameEvents.OnContinueButton += OnContinueButton;
        InputManager.OnTowerButton += OnTowerButton; 
        InputManager.OnSuicideButton += OnSuicideButton;
        InputManager.OnDeselectButtonPressed += OnDeselectButtonPressed;
    }

    private void OnDisable()
    {
        GameEvents.OnCombatSelected -= OnCombatSelected;
        GameEvents.OnContinueButton -= OnContinueButton;
        InputManager.OnTowerButton -= OnTowerButton; 
        InputManager.OnSuicideButton -= OnSuicideButton;
        InputManager.OnDeselectButtonPressed -= OnDeselectButtonPressed;
    }
}
