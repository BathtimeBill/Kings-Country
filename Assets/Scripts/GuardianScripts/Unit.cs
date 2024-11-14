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
    public float tickRate;
    public UnitState state;
    public GameObject pointer;
    public GameObject trackTarget;
    public float spawnInMoveDistance;

    [Header("Death Objects")]
    public GameObject deadSatyr;
    [Header("Relevant Game Objects")]
    public GameObject weaponCollider;
    public GameObject rangedPrefab;
    public GameObject towerPrefab;
    public GameObject rangedSpawnLocation;
    public GameObject healingParticle;
    [Header("Bools")]
    public bool isSelected;
    public bool inCombat;
    public bool isMovingCheck;
    public bool isTooCloseToTower;
    public bool isOutOfBounds;
    private bool hitByArrow;
    public bool isFirstPerson;
    //public float isMovingCheckTime;
    private Vector3 attackDestination;
    [Header("Audio")]
    public GameObject SFXPool;
    public int soundPoolCurrent;
    public AudioSource[] soundPool;
    public AudioSource spellSource;
    private Transform[] rangedAttackLocations;
    [Header("Perks")]
    public bool isUpgraded;

    [Header("Body")] 
    public Transform leftFoot;
    public Transform rightFoot;
    
    //Stats
    private float health;
    private float maxHealth;
    private float unitSpeed;
    private float stoppingDistance;
    private float projectileSpeed = 1000;
    private float focusSpeed = 5f;
    
    //Combat Mode
    private CombatMode combatMode;
    private Vector3 defendPosition;
    
    //AI
    private float detectionRadius;
    private Transform closestEnemy;
    private float distanceToClosestEnemy;
    public float DistanceToClosestEnemy => distanceToClosestEnemy;
    public Transform ClosestEnemy => closestEnemy;
    private UnitData unitData;
    
    //Misc
    private int startingDay;
    private GameObject bloodParticle => _DATA.GetUnit(unitID).bloodParticles;
    
    void Start()
    {
        unitData = _DATA.GetUnit(unitID);
        if(!isFirstPerson)
        {
            soundPool = SFXPool.GetComponents<AudioSource>();
            pointer = GameObject.FindGameObjectWithTag("Pointer");
            Setup();
            GameEvents.ReportOnCreatureSpawned(unitID);
            SpawnInMove();
            startingDay = _currentDay;
        }
        _UM.unitList.Add(this);
    }

    private void Setup()
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
        yield return new WaitForSeconds(0.5f);
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
        if (isFirstPerson)
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
                    if (Vector3.Distance(pointer.transform.position, transform.position) <= 11)
                    {
                        state = UnitState.Idle;
                    }
                }
                else
                {
                    if (Vector3.Distance(pointer.transform.position, transform.position) <= 5)
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
        PlaySound(_SM.GetLeshyStompSound());
    }

    //Look to move into SFXPool script
    public void PlaySound(AudioClip[] _clips)
    {
        AudioClip clip = ArrayX.GetRandomItemFromArray(_clips);
        PlaySound(clip);
    }
    public void PlaySound(AudioClip _clip)
    {
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
    void SpawnInMove()
    {
        Vector3 randomDirection = transform.position + Random.insideUnitSphere * spawnInMoveDistance;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, spawnInMoveDistance, 1);
        Vector3 finalPosition = hit.position;
        navAgent.SetDestination(finalPosition);
    }
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
        //if (other.tag == "Boundry")
        //{
        //    isOutOfBounds = false;
        //}
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
        GameObject go = Instantiate(bloodParticle, transform.position + new Vector3(0, 5, 0), transform.rotation);
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
                GameObject go;
                go = Instantiate(deadSatyr, transform.position, transform.rotation);
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

    IEnumerator Attack()
    {

        while (Vector3.Distance(transform.position, attackDestination) > 4f)
        {
            navAgent.SetDestination(attackDestination);
            //animator.SetBool("isAttacking", false);
            yield return null;
        }
        while (Vector3.Distance(transform.position, attackDestination) < 4f)
        {
            transform.LookAt(attackDestination);
            //animator.SetBool("isAttacking", true);

            yield return null;
        }
        yield return null;

        //animator.SetBool("isAttacking", false);
        StartCoroutine(Attack());

    }

    public void EnableCollider()
    {
        weaponCollider.SetActive(true);
    }
    public void DisableCollider()
    {
        weaponCollider.SetActive(false);
    }

    public void SpawnRangedAttack()
    {
        if (inCombat == false)
        {
            //animator.SetBool("inCombat", false);
        }
        else
        {
            int rndDirection = Random.Range(0, rangedAttackLocations.Length);
            spellSource.Play();
            GameObject rangedInstance;
            rangedInstance = Instantiate(rangedPrefab, rangedSpawnLocation.transform.position, transform.rotation);
            rangedInstance.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
            Destroy(rangedInstance, 3);
            navAgent.SetDestination(rangedAttackLocations[rndDirection].transform.position);
            DecreaseHealth(5);
        }
    }
    
    public void SetDestination(Transform _destination) => navAgent.SetDestination(_destination.position);
    
    private void OnContinueButton()
    {
        if(unitID != CreatureID.Mistcalf)
        {
            SetHealth(maxHealth);
        }
    }
    
    #region Animation Events

    public void PlayFootstep(string _foot)
    {
        PlaySound(unitData.footstepSounds);
        PlayParticle(_foot == "Left" ? leftFoot : rightFoot);
    }

    public void PlayParticle(Transform _transform)
    {
        //TODO pool Particles
        Instantiate(bloodParticle, _transform.position, Quaternion.Euler(0, 0, 0));
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
        Vector3 offset = new Vector3(0, -1.5f, 0);
        if (unitID == CreatureID.Huldra && isSelected || unitID == CreatureID.Fidhain && isSelected)
        {
            if (_TUTORIAL.isTutorial && _TUTORIAL.tutorialStage == 13)
            {
                GameEvents.ReportOnNextTutorial();
            }
            if (isTooCloseToTower == false && isOutOfBounds == false)
            {
                Instantiate(towerPrefab, transform.position + offset, Quaternion.Euler(-90, 0, 0));
                _UM.Deselect(this);
                _UM.unitList.Remove(this);
                Destroy(gameObject);
            }
            if (isTooCloseToTower == true)
            {
                _UI.SetError(ErrorID.TooCloseToTower);
            }
            if (isOutOfBounds == true && isTooCloseToTower == false)
            {
                _UI.SetError(ErrorID.OutOfBounds);
            }
        }
    }
    
    private void OnSuicideButton()
    {
        if(isSelected)
        {
            if (_HUT != null) _HUT.RemoveUnit(this);
            if (_HORGR != null) _HORGR.RemoveUnit(this);
            _UM.Deselect(this);
            _UM.unitList.Remove(this);
            GameObject go;
            go = Instantiate(deadSatyr, transform.position, transform.rotation);
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
