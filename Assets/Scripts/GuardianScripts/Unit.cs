using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Unit : GameBehaviour
{
    [Header("Unit Type")]
    public CreatureID unitID;
    
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

    public Transform ClosestEnemy => closestEnemy;
    [HideInInspector] public UnitData unitData;
    
    //Misc
    private int startingDay;
    private GameObject hitParticle => _DATA.GetUnit(unitID).hitParticles;
    private GameObject dieParticle => _DATA.GetUnit(unitID).dieParticles;
    
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
    private float stopRangeValue;
    public float stopRange
    {
        get { return stopRangeValue; }
        set {stopRangeValue = value; UpdateDebug(); }
    }
    private void UpdateDebug()
    {
        if (!debugUnit)
            return;
        debugUnit.AdjustRange(detectRange, attackRange, stopRange);
    }
    #endregion
    
    public virtual void Start()
    {
        unitData = _DATA.GetUnit(unitID);
        if(!isFirstPerson)
        {
            soundPool = SFXPool.GetComponents<AudioSource>();
            pointer = _Pointer;
            Setup();
            GameEvents.ReportOnGuardianSpawned(unitID);
            SpawnInMove();
            startingDay = _currentDay;
        }
        _UM.unitList.Add(this);
    }

    public virtual void Setup()
    {
        //Health
        maxHealth = unitData.health;
        if (_DATA.HasPerk(PerkID.BarkSkin))
            maxHealth = MathX.GetPercentageIncrease(maxHealth, _DATA.GetPerk(PerkID.BarkSkin).increaseValue);
        SetHealth(maxHealth);

        //Speed
        unitSpeed = unitData.speed;
        if (_DATA.HasPerk(PerkID.FlyFoot))
            unitSpeed = MathX.GetPercentageIncrease(unitSpeed, _DATA.GetPerk(PerkID.FlyFoot).increaseValue);
        navAgent.speed = unitSpeed;

        //Detection
        detectRange = unitData.detectionRadius;
        stopRange = unitData.stoppingDistance;
        attackRange = unitData.attackRange;

        //Other
        healthBar.ChangeCombatModeIcon(_ICONS.attackIcon);
        healthBar.ChangeGroupNumber("");
        
        UpdateDebug();
    }

    IEnumerator WaitForIsMovingCheck()
    {
        yield return new WaitForSeconds(0.1f);
        if(GetComponentInChildren<UnitAnimation>().currentSpeed == 0)
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

        if (!_EM.allEnemiesDead)
            UpdateClosestEnemy();

        HandleState();
    }

    #region AI
    private void UpdateClosestEnemy()
    {
        closestEnemy = ObjectX.GetClosest(gameObject, _EM.enemies).transform;
        distanceToClosestEnemy = Vector3.Distance(closestEnemy.transform.position, transform.position);
        bool isLord = closestEnemy.gameObject.CompareTag("Lord");
        bool isSpecialUnit = unitID != CreatureID.Fidhain && unitID != CreatureID.Goblin;
        navAgent.stoppingDistance = isLord && isSpecialUnit ? stopRange * 2 : stopRange;
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
        }
    }

    private void HandleIdleState()
    {
        if (_EM.allEnemiesDead)
            return;

        if (unitID == CreatureID.Goblin || unitID == CreatureID.Fidhain)
        {
            navAgent.stoppingDistance = stopRange;
        }

        if (distanceToClosestEnemy < detectRange)
        {
            state = UnitState.Attack;
        }
        else if (combatMode == CombatMode.Defend)
        {
            navAgent.SetDestination(defendPosition);
        }
    }

    private void HandleAttackState()
    {
        if (_EM.allEnemiesDead)
        {
            state = UnitState.Idle;
            return;
        }

        if (distanceToClosestEnemy < detectRange || hitByArrow)
        {
            navAgent.SetDestination(closestEnemy.transform.position);
            SmoothFocusOnEnemy();
        }
        else if (distanceToClosestEnemy >= detectRange && !hitByArrow)
        {
            state = UnitState.Moving;
        }

        navAgent.stoppingDistance = unitID == CreatureID.Goblin ? 50 : 20;
    }

    private void HandleMovingState()
    {
        if (!isMovingCheck)
        {
            isMovingCheck = true;
            StartCoroutine(WaitForIsMovingCheck());
        }

        float distanceThreshold = unitID == CreatureID.Leshy ? 11 : 5;
        if (Vector3.Distance(pointer.position, transform.position) <= distanceThreshold)
        {
            state = UnitState.Idle;
        }

        if (unitID == CreatureID.Goblin || unitID == CreatureID.Fidhain)
        {
            navAgent.stoppingDistance = 4;
        }

        if (combatMode == CombatMode.AttackMove && distanceToClosestEnemy < detectRange)
        {
            state = UnitState.Attack;
        }
    }

    private void HandleTrackState()
    {
        if (trackTarget != null)
        {
            navAgent.SetDestination(trackTarget.transform.position);
            float attackDistance = unitID == CreatureID.Goblin ? 30 : 10;
            if (Vector3.Distance(transform.position, trackTarget.transform.position) <= attackDistance)
            {
                state = UnitState.Attack;
            }
        }
        else
        {
            state = UnitState.Idle;
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
            if(unitID != CreatureID.Mistcalf)
                IncreaseHealth(100);
        }
        if (other.CompareTag("Maegen"))
        {
            _GM.maegen += 1;
        }
        if (other.CompareTag("Horgr"))
        {
            if(_horgrExists)
                _HORGR.AddUnit(this);
            GameEvents.ReportOnUnitArrivedAtHorgr();
        }
        if (other.CompareTag("Hut"))
        {
            if (_hutExists)
                _HUT.AddUnit(this);
            GameEvents.ReportOnUnitArrivedAtHut();
        }
        if(other.CompareTag("Tower"))
        {
            isTooCloseToTower = true;
        }

        if (other.CompareTag("Lord"))
        {
            state = UnitState.Attack;
        }

        if (other.CompareTag("Rune"))
        {
            if (unitID != CreatureID.Mistcalf)
            {
                healingParticle.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Horgr") && _horgrExists)
            _HORGR.RemoveUnit(this);
        
        if(other.CompareTag("Hut") && _hutExists)
            _HUT.RemoveUnit(this);
        
        if (other.CompareTag("Tower"))
            isTooCloseToTower = false;
        
        if (unitID != CreatureID.Mistcalf)
            healingParticle.SetActive(false);

    }

    private void OnTriggerStay(Collider other)
    {
        
        if(other.CompareTag("Rune"))
        {
            if (unitID != CreatureID.Mistcalf)
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
        else if (other.GetComponent<UnitWeaponCollider>().humanID == HumanID.LogCutter)
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

        string attacker = _DATA.GetUnit(uwc.humanID).ToString();
        float damage = _DATA.GetUnit(uwc.humanID).damage;

        switch (uwc.humanID)
        {
            case HumanID.Logger:
                TakeDamage(attacker, damage); 
                break;
            case HumanID.Lumberjack:
                TakeDamage(attacker, damage);
                break;
            case HumanID.Dreng:
                if (unitID == CreatureID.Leshy) { attacker = "Unknown"; damage = 50; }
                TakeDamage(attacker, damage);
                break;
            case HumanID.Berserkr:
                if (unitID == CreatureID.Leshy) { attacker = "Unknown"; damage = 65; }
                TakeDamage(attacker, damage);
                break;
            case HumanID.Wathe:
                HitByArrow();
                if (unitID == CreatureID.Skessa) { damage *=3; }
                TakeDamage(attacker, damage);
                break;
            case HumanID.Poacher:
                HitByArrow();
                if (unitID == CreatureID.Skessa) { damage *= 3; }
                TakeDamage(attacker, damage);
                break;
            case HumanID.Lord:
                HitByArrow();
                TakeDamage(attacker, damage);
                break;
            case HumanID.Dog:
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
            GameObject go = Instantiate(unitData.ragdollModel, transform.position, transform.rotation);
            isColliding = true;
            Destroy(go, 15);
        }
        _UI.CheckPopulousUI();
        int daysSurvived = _currentDay - startingDay;
        GameEvents.ReportOnGuardianKilled(unitID.ToString(), _attacker, daysSurvived);
        _UM.RemoveGuardian(this, transform.position, transform.rotation);
    }
    
    private void RemoveFromSites()
    {
        if (_hutExists)
            _HUT.RemoveUnit(this);
        
        if (_horgrExists)
            _HORGR.RemoveUnit(this);
    }
    
    public void SetDestination(Transform _destination) => navAgent.SetDestination(_destination.position);
    
    private void OnContinueButton()
    {
        if(unitID != CreatureID.Mistcalf)
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
        PlaySound(unitData.footstepSounds);

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
            if (unitID == CreatureID.Goblin)
            {
                navAgent.speed = unitData.speed;
            }
        }
        combatMode = CombatMode.Move;
    }
    
    private void DefendSelected()
    {
        if (combatMode != CombatMode.Defend)
        {
            if (unitID != CreatureID.Goblin)
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
        if (unitID != CreatureID.Fidhain || unitID != CreatureID.Fidhain)
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
            GameObject go = Instantiate(unitData.ragdollModel, transform.position, transform.rotation);
            Destroy(go, 15);
            _UI.CheckPopulousUI();
            int daysSurvived = _currentDay - startingDay;
            GameEvents.ReportOnGuardianKilled(unitID.ToString(), "Unknown", daysSurvived);
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
                if (_PC.mouseOverEnemyBool)
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
