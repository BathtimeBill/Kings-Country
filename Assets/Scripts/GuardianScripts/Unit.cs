using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Unit : GameBehaviour
{
    [Header("Unit Type")]
    public CreatureID unitID;
    
    [Header("UI")]
    public HealthBar healthBar;
    [FormerlySerializedAs("selectionRingCircle")] public SelectionRing selectionRing;
    
    [Header("Components")]
    public NavMeshAgent navAgent;
    public Animator animator;
    [Header("AI")]
    public UnitState state;
    private float spawnInMoveDistance = 12f;
    private GameObject trackTarget;
    private Transform pointer;
    
    [FormerlySerializedAs("weaponCollider")] [Header("Weapon Objects")]
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
    //public float isMovingCheckTime;
    private Vector3 attackDestination;
    [Header("Audio")]
    public GameObject SFXPool;
    public int soundPoolCurrent;
    public AudioSource[] soundPool;
    private Transform[] rangedAttackLocations;

    [Header("Body")] 
    public Transform leftHand;
    public Transform rightHand;
    public Transform leftFoot;
    public Transform rightFoot;
    
    //Stats
    private float health;
    private float maxHealth;
    private float unitSpeed;
    private float stoppingDistance;
    private float focusSpeed = 5f;
    
    //Combat Mode
    private CombatMode combatMode;
    private Vector3 defendPosition;
    
    //AI
    private float detectionRadius;
    private Transform closestEnemy;
    [HideInInspector] public float distanceToClosestEnemy;

    public Transform ClosestEnemy => closestEnemy;
    [HideInInspector] public UnitData unitData;
    
    //Misc
    private int startingDay;
    private GameObject hitParticle => _DATA.GetUnit(unitID).hitParticles;
    private GameObject dieParticle => _DATA.GetUnit(unitID).dieParticles;
    
    public virtual void Start()
    {
        unitData = _DATA.GetUnit(unitID);
        if(!isFirstPerson)
        {
            soundPool = SFXPool.GetComponents<AudioSource>();
            pointer = _Pointer;
            Setup();
            GameEvents.ReportOnCreatureSpawned(unitID);
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

        //Detecion
        detectionRadius = unitData.detectionRadius;
        stoppingDistance = unitData.stoppingDistance;

        //Other
        healthBar.ChangeCombatModeIcon(_ICONS.attackIcon);
        healthBar.ChangeGroupNumber("");
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
        {
            closestEnemy = ObjectX.GetClosest(gameObject, _EM.enemies).transform;
            distanceToClosestEnemy = Vector3.Distance(closestEnemy.transform.position, transform.position);
            bool isLord = closestEnemy.gameObject.CompareTag("Lord"); 
            bool isSpecialUnit = unitID != CreatureID.Fidhain && unitID != CreatureID.Goblin; 
            navAgent.stoppingDistance = isLord && isSpecialUnit ? stoppingDistance * 2 : stoppingDistance;
        }

        switch (state)
        {
            case UnitState.Idle:
                if (_EM.allEnemiesDead)
                    return;
                
                if (unitID == CreatureID.Goblin || unitID == CreatureID.Fidhain)
                {
                    navAgent.stoppingDistance = stoppingDistance;
                }

                if (distanceToClosestEnemy < detectionRadius)
                {
                    state = UnitState.Attack;
                }
                else if (combatMode == CombatMode.Defend)
                {
                    navAgent.SetDestination(defendPosition);
                }
                break;
            case UnitState.Attack:
                if (_EM.allEnemiesDead)
                {
                    state = UnitState.Idle;
                }
                else
                {
                    if (distanceToClosestEnemy < detectionRadius || hitByArrow)
                    {
                        navAgent.SetDestination(closestEnemy.transform.position);
                        SmoothFocusOnEnemy();
                    }

                    if (distanceToClosestEnemy >= detectionRadius && !hitByArrow)
                    {
                        state = UnitState.Moving;
                    }
                }

                if (unitID == CreatureID.Goblin)
                {
                    navAgent.stoppingDistance = 50;
                }
                else if (unitID == CreatureID.Fidhain)
                {
                    navAgent.stoppingDistance = 20;
                }
                break;
            case UnitState.Moving:
                if (isMovingCheck == false)
                {
                    isMovingCheck = true;
                    StartCoroutine(WaitForIsMovingCheck());
                }
                if (unitID == CreatureID.Leshy)
                {
                    if (Vector3.Distance(pointer.position, transform.position) <= 11)
                    {
                        state = UnitState.Idle;
                    }
                }
                else
                {
                    if (Vector3.Distance(pointer.position, transform.position) <= 5)
                    {
                        state = UnitState.Idle;
                    }
                }
                if (unitID == CreatureID.Goblin || unitID == CreatureID.Fidhain)
                {
                    navAgent.stoppingDistance = 4;
                }
                if (combatMode == CombatMode.AttackMove)
                {
                    if (distanceToClosestEnemy < detectionRadius)
                    {
                        state = UnitState.Attack;
                    }
                }
                break;
            case UnitState.Track:
                //navAgent.angularSpeed = 500;
                if (trackTarget != null)
                {
                    navAgent.SetDestination(trackTarget.transform.position);
                    if (unitID == CreatureID.Goblin)
                    {
                        if (Vector3.Distance(transform.position, trackTarget.transform.position) <= 30)
                        {
                            state = UnitState.Attack;
                        }
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, trackTarget.transform.position) <= 10)
                        {
                            state = UnitState.Attack;
                        }
                    }

                }
                else
                {
                    state = UnitState.Idle;
                }
                break;
        }

    }
   
    #region Sound
    public void PlayFootstepSound()
    {
        PlaySound(_SM.GetForestFootstepSound());
    }
    public void PlayFlapSound()
    {
        PlaySound(_SM.GetFlapSound());
    }
    public void PlayGolemFootstepSound()
    {
        PlaySound(_SM.GetGolemFootsteps());
    }
    public void PlayGolemVocalSound()
    {
        PlaySound(_SM.GetGolemVocal());
    }
    public void PlayLeshyFootstepSound()
    {
        PlaySound(_SM.GetLeshyFootstepSound());
    }
    public void PlayLeshyStompSound()
    {
        PlaySound(unitData.attackSounds);
    }

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
        soundPool[soundPoolCurrent].clip = _clip;
        soundPool[soundPoolCurrent].pitch = Random.Range(0.8f, 1.2f);
        soundPool[soundPoolCurrent].Play();
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
        detectionRadius = detectionRadius / 2;
        state = UnitState.Moving;
        navAgent.SetDestination(transform.position);
    }
    void SpawnInMove() => navAgent.SetDestination(SpawnX.GetSpawnPositionInRadius(this.transform.position, spawnInMoveDistance));
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Axe1")
        {
            TakeDamage(_DATA.GetUnit(HumanID.Logger).id.ToString(), _DATA.GetUnit(HumanID.Logger).damage);
            other.enabled = false;
        }
        if (other.tag == "Axe2")
        {
            TakeDamage(_DATA.GetUnit(HumanID.Lumberjack).id.ToString(), _DATA.GetUnit(HumanID.Lumberjack).damage);
            other.enabled = false;
        }
        if (other.tag == "Sword2")
        {
            if(unitID != CreatureID.Leshy)
            {
                TakeDamage(_DATA.GetUnit(HumanID.Dreng).id.ToString(), _DATA.GetUnit(HumanID.Dreng).damage);
            }
            else
            {
                TakeDamage("Unknown", 50);
            }
 
            other.enabled = false;
        }
        if (other.tag == "Sword3")
        {
            if (unitID != CreatureID.Leshy)
            {
                TakeDamage(_DATA.GetUnit(HumanID.Berserkr).id.ToString(), _DATA.GetUnit(HumanID.Berserkr).damage);
            }
            else
            {
                TakeDamage("Unknown", 65);
            }
            other.enabled = false;
        }
        if (other.tag == "Arrow")
        {
            hitByArrow = true;
            detectionRadius = detectionRadius * 2;
            state = UnitState.Attack;
            StartCoroutine(HitByArrowDelay());
            if (unitID == CreatureID.Skessa)
            {
                TakeDamage(_DATA.GetUnit(HumanID.Wathe).id.ToString(), _DATA.GetUnit(HumanID.Wathe).damage * 3);
            }
            else
            {
                TakeDamage(_DATA.GetUnit(HumanID.Wathe).id.ToString(), _DATA.GetUnit(HumanID.Wathe).damage);
            }

            Destroy(other.gameObject);
        }
        if (other.tag == "Arrow2")
        {
            hitByArrow = true;
            detectionRadius = detectionRadius * 2;
            state = UnitState.Attack;
            StartCoroutine(HitByArrowDelay());
            if (unitID == CreatureID.Skessa)
            {
                TakeDamage(_DATA.GetUnit(HumanID.Poacher).id.ToString(), _DATA.GetUnit(HumanID.Poacher).damage * 3);
            }
            else
            {
                TakeDamage(_DATA.GetUnit(HumanID.Poacher).id.ToString(), _DATA.GetUnit(HumanID.Poacher).damage);
            }

            Destroy(other.gameObject);
        }
        
        
        
        if(other.CompareTag("Heal"))
        {
            if(unitID != CreatureID.Mistcalf)
            {
                IncreaseHealth(100);
            }
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

        if (other.tag == "LordWeapon")
        {
            TakeDamage(_DATA.GetUnit(HumanID.Lord).id.ToString(), _DATA.GetUnit(HumanID.Lord).damage);
        }
        if (other.tag == "Lord")
        {
            state = UnitState.Attack;
        }
        if (other.tag == "Explosion3")
        {
            TakeDamage("Dog", 100);
        }
        if (other.tag == "Rune")
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
        {
            healingParticle.SetActive(false);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Axe3")
        {
            DecreaseHealth(0.5f * Time.deltaTime);
            if(health < 0)
            {
                Die("Unknown");
            }
        }
        if(other.tag == "Rune")
        {
            if(unitID != CreatureID.Mistcalf)
            {
                if (_DATA.HasPerk(PerkID.Rune))
                {
                    IncreaseHealth(_GM.runeHealRate * 2 * Time.deltaTime);
                }
                else
                {
                    IncreaseHealth(_GM.runeHealRate * Time.deltaTime);
                }
            }
        }
    }

    public void TakeDamage(string attacker, float damage)
    {
        state = UnitState.Attack;
        GameObject go = Instantiate(hitParticle, transform.position + new Vector3(0, 5, 0), transform.rotation);
        //go.transform.rotation = Quaternion.Inverse(transform.rotation);
        DecreaseHealth(damage);
        Die(attacker);
    }

    private void Die(string _attacker)
    {
        bool isColliding = false;
        if (health <= 0)
        {
            RemoveFromSites();
            _UM.Deselect(this);
            _UM.unitList.Remove(this);

            if(!isColliding)
            {
                GameObject go = Instantiate(unitData.ragdollModel, transform.position, transform.rotation);
                isColliding = true;
                Destroy(go, 15);
            }
            _UI.CheckPopulousUI();
            int daysSurvived = _currentDay - startingDay;
            GameEvents.ReportOnCreatureKilled(unitID.ToString(), _attacker, daysSurvived);
            _UM.RemoveSelectedUnit(this);
            Destroy(gameObject);
        }
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
            detectionRadius = detectionRadius * 2;
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
                detectionRadius = detectionRadius / 2;
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
            GameEvents.ReportOnCreatureKilled(unitID.ToString(), "Unknown", daysSurvived);
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
