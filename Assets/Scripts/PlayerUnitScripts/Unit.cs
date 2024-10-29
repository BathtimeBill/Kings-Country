using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Unit : GameBehaviour
{
    [Header("Unit Type")]
    public CreatureID unitID;
    [Header("CombatMode")]
    public CombatMode combatMode;
    public Vector3 defendPosition;
    public float tickRate;

    [Header("UI")]
    public Transform healthBarFill;
    public SpriteRenderer combatModeIcon;
    public TMPro.TMP_Text groupNumber;
    public GameObject selectionCircle;

    [Header("Stats")] 
    public float health;
    public float maxHealth;
    public float unitSpeed;
    public float projectileSpeed = 1000;
    public float focusSpeed;
    public float stoppingDistance;
    [Header("Components")]
    public NavMeshAgent navAgent;
    public Animator animator;
    [Header("AI")]
    public UnitState state;
    public float detectionRadius;
    public Transform closestEnemy;
    public float distanceToClosestEnemy;
    public GameObject pointer;
    public bool hasStopped = false;
    public GameObject trackTarget;
    public float spawnInMoveDistance;

    [Header("Death Objects")]
    public GameObject deadSatyr;
    public GameObject bloodParticle1;
    [Header("Relevant Game Objects")]
    public GameObject targetDest;
    public GameObject weaponCollider;
    public GameObject deadPrefab;
    public GameObject explosionPrefab;
    public GameObject entWalkPrefab;
    public GameObject rangedPrefab;
    public GameObject towerPrefab;
    public GameObject rangedSpawnLocation;
    public GameObject healingParticle;
    [Header("Bools")]
    public bool isSelected;
    public bool inCombat;
    public bool isMoving;
    public bool isMovingCheck;
    public bool isTooCloseToTower;
    public bool isOutOfBounds;
    public bool idleSetDest;
    public bool hitByArrow;
    public bool isFirstPerson;
    //public float isMovingCheckTime;
    private Vector3 attackDestination;
    [Header("Audio")]
    public GameObject SFXPool;
    public int soundPoolCurrent;
    public AudioSource[] soundPool;
    public AudioSource vocalSource;
    public AudioSource spellSource;
    private Transform[] rangedAttackLocations;
    [Header("Perks")]
    public bool isUpgraded;

    private Sprite attackSprite;
    private Sprite defendSprite;
    private UnitData unitData => _DATA.GetUnit(unitID);

    private int startingDay;



    void Start()
    {
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

        //Other
        attackSprite = _ICONS.damageIcon;
        defendSprite = _ICONS.healthIcon;
        combatModeIcon.sprite = attackSprite;
        ChangeGroupNumber("");
    }

    IEnumerator WaitForIsMovingCheck()
    {
        yield return new WaitForSeconds(0.5f);
        if(GetComponentInChildren<TestSatyrAnimation>().currentSpeed == 0)
        {
            state = UnitState.Idle;
        }    

    }

    void Update()
    {
        if (isFirstPerson)
            return;

        if (!_EM.allEnemiesDead)
        {
            closestEnemy = GetClosestEnemy();
            distanceToClosestEnemy = Vector3.Distance(closestEnemy.transform.position, transform.position);
            if (closestEnemy.gameObject.tag == "Lord")
            {
                if(unitID != CreatureID.Fidhain || unitID != CreatureID.Goblin)
                {
                    navAgent.stoppingDistance = stoppingDistance * 2;
                }
            }
            if (closestEnemy.gameObject.tag != "Lord")
            {
                navAgent.stoppingDistance = stoppingDistance;
            }

        }


        switch (state)
        {
            case UnitState.Idle:
                if (!_EM.allEnemiesDead)
                {
                    if (combatMode != CombatMode.Defend)
                    {
                        if (distanceToClosestEnemy < detectionRadius)
                        {
                            state = UnitState.Attack;
                        }
                    }
                    else
                    {
                        navAgent.SetDestination(defendPosition);
                        if (distanceToClosestEnemy < detectionRadius)
                        {
                            state = UnitState.Attack;
                        }
                    }
                    if (unitID == CreatureID.Goblin || unitID == CreatureID.Fidhain)
                    {
                        navAgent.stoppingDistance = 4;
                    }

                }

                //animator.SetBool("inCombat", false);
                break;

            case UnitState.Attack:
                //navAgent.angularSpeed = 500;
                if (_EM.allEnemiesDead)
                {
                    state = UnitState.Idle;
                }
                if (distanceToClosestEnemy >= detectionRadius)
                {
                    if (hitByArrow == false)
                        state = UnitState.Moving;
                }
                if (unitID == CreatureID.Goblin)
                {
                    navAgent.stoppingDistance = 50;
                }
                //else
                //{
                //    navAgent.stoppingDistance = stoppingDistance;
                //}
                if (unitID == CreatureID.Fidhain)
                {
                    navAgent.stoppingDistance = 20;
                }
                if (!_EM.allEnemiesDead)
                {
                    if (distanceToClosestEnemy < detectionRadius)
                    {
                        navAgent.SetDestination(closestEnemy.transform.position);
                        SmoothFocusOnEnemy();
                    }
                    else
                    {
                        if (hitByArrow == true)
                        {
                            navAgent.SetDestination(closestEnemy.transform.position);
                            SmoothFocusOnEnemy();
                        }
                    }
                }
                //animator.SetBool("inCombat", true);

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

        if (isSelected)
        {
            if (Input.GetMouseButtonDown(1))
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

                    StartCoroutine(WaitForSetDestination());
                }
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                if(isSelected)
                {
                    print("Suicide");
                    if (_HM.units.Contains(this))
                    {
                        _HM.units.Remove(this);
                    }
                    if (_HUTM.units.Contains(this))
                    {
                        _HUTM.units.Remove(this);
                    }
                    _UM.Deselect(this);
                    _UM.unitList.Remove(this);
                    GameObject go;
                    go = Instantiate(deadSatyr, transform.position, transform.rotation);
                    Destroy(go, 15);
                    _UI.CheckPopulousUI();
                    int daysSurvived = _currentDay - startingDay;
                    GameEvents.ReportOnCreatureKilled(unitID.ToString(), "Unknown", daysSurvived);
                    CheckIfUnitIsInGroup();
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            ToggleSelectionCircle(false);
        }

        if (Input.GetKeyDown(KeyCode.T))
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
    }
    IEnumerator Tick()
    {
        print("Tick");
        switch (state)
        {
            case UnitState.Idle:
                if (!_EM.allEnemiesDead)
                {
                    if (combatMode != CombatMode.Defend)
                    {
                        if (distanceToClosestEnemy < detectionRadius)
                        {
                            state = UnitState.Attack;
                        }
                    }
                    else
                    {
                        navAgent.SetDestination(defendPosition);
                        if (distanceToClosestEnemy < detectionRadius)
                        {
                            state = UnitState.Attack;
                        }
                    }
                    if (unitID == CreatureID.Goblin || unitID == CreatureID.Fidhain)
                    {
                        navAgent.stoppingDistance = 4;
                    }

                }

                //animator.SetBool("inCombat", false);
                break;

            case UnitState.Attack:
                //navAgent.angularSpeed = 500;
                if (_EM.allEnemiesDead)
                {
                    state = UnitState.Idle;
                }
                if (distanceToClosestEnemy >= detectionRadius)
                {
                    if (hitByArrow == false)
                        state = UnitState.Moving;
                }
                if (unitID == CreatureID.Goblin)
                {
                    navAgent.stoppingDistance = 50;
                }
                //else
                //{
                //    navAgent.stoppingDistance = stoppingDistance;
                //}
                if (unitID == CreatureID.Fidhain)
                {
                    navAgent.stoppingDistance = 20;
                }
                if (!_EM.allEnemiesDead)
                {
                    if (distanceToClosestEnemy < detectionRadius)
                    {
                        navAgent.SetDestination(closestEnemy.transform.position);
                        SmoothFocusOnEnemy();
                    }
                    else
                    {
                        if (hitByArrow == true)
                        {
                            navAgent.SetDestination(closestEnemy.transform.position);
                            SmoothFocusOnEnemy();
                        }
                    }
                }
                //animator.SetBool("inCombat", true);

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
        yield return new WaitForSeconds(tickRate);
        StartCoroutine(Tick());
    }
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
    public void PlaySound(AudioClip _clip)
    {
        if (soundPoolCurrent == soundPool.Length - 1)
            soundPoolCurrent = 0;
        else
            soundPoolCurrent += 1;

        soundPool[soundPoolCurrent].clip = _clip;
        soundPool[soundPoolCurrent].pitch = Random.Range(0.8f, 1.2f);
        soundPool[soundPoolCurrent].Play();
    }
    private void SmoothFocusOnEnemy()
    {
        var targetRotation = Quaternion.LookRotation(closestEnemy.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, focusSpeed * Time.deltaTime);
    }


    #region Health
    private void IncreaseHealth(float _health)
    {
        health += _health;
        if(health > maxHealth) health = maxHealth;
        AdjustHealthBar();
    }

    private void DecreaseHealth(float _health)
    {
        health -= _health;
        AdjustHealthBar();
    }

    private void SetHealth(float _health)
    {
        health = _health;
        AdjustHealthBar();
    }

    private float CalculateHealth()
    {
        return MathX.MapTo01(health, 0, maxHealth);
    }
    #endregion

    #region UI
    private void AdjustHealthBar() => healthBarFill.DOScaleX(CalculateHealth(), 0.2f);
    public void ChangeGroupNumber(string _groupNumber) => groupNumber.text = _groupNumber;
    private void ChangeCombatModeIcon(Sprite _icon) => combatModeIcon.sprite = _icon;
    private void ToggleSelectionCircle(bool _on) => selectionCircle.SetActive(_on);
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
        if(other.tag == "Heal")
        {
            if(unitID != CreatureID.Mistcalf)
            {
                IncreaseHealth(100);
            }
        }
        if (other.tag == "Maegen")
        {
            _GM.maegen += 1;
        }
        if (other.tag == "Horgr")
        {
            if (!_HM.units.Contains(this))
                _HM.units.Add(this);
            GameEvents.ReportOnUnitArrivedAtHorgr();
        }
        if (other.tag == "Hut")
        {
            if (!_HUTM.units.Contains(this))
                _HUTM.units.Add(this);
            GameEvents.ReportOnUnitArrivedAtHut();
        }
        if(other.tag == "Tower")
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
        if (other.tag == "Horgr")
        {
            _HM.units.Remove(this);
        }
        if(other.tag == "Hut")
        {
            _HUTM.units.Remove(this);
        }
        if (other.tag == "Tower")
        {
            isTooCloseToTower = false;
        }
        //if (other.tag == "Boundry")
        //{
        //    isOutOfBounds = true;
        //}
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
        GameObject go = Instantiate(bloodParticle1, transform.position + new Vector3(0, 5, 0), transform.rotation);
        //go.transform.rotation = Quaternion.Inverse(transform.rotation);
        DecreaseHealth(damage);
        Die(attacker);
    }

    private void Die(string _attacker)
    {
        bool isColliding = false;
        if (health <= 0)
        {
            if (_HM.units.Contains(this))
            {
                _HM.units.Remove(this);
            }
            if(_HUTM.units.Contains(this))
            {
                _HUTM.units.Remove(this);
            }
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
            CheckIfUnitIsInGroup();
            Destroy(gameObject);
        }
    }
    void CheckIfUnitIsInGroup()
    {
        if (_UM.controlGroup01.Contains(this))
        {
            _UM.controlGroup01.Remove(this);
        }
        if (_UM.controlGroup02.Contains(this))
        {
            _UM.controlGroup02.Remove(this);
        }
        if (_UM.controlGroup03.Contains(this))
        {
            _UM.controlGroup03.Remove(this);
        }
        if (_UM.controlGroup04.Contains(this))
        {
            _UM.controlGroup04.Remove(this);
        }
        if (_UM.controlGroup05.Contains(this))
        {
            _UM.controlGroup05.Remove(this);
        }
        if (_UM.controlGroup06.Contains(this))
        {
            _UM.controlGroup06.Remove(this);
        }
        if (_UM.controlGroup07.Contains(this))
        {
            _UM.controlGroup07.Remove(this);
        }
        if (_UM.controlGroup08.Contains(this))
        {
            _UM.controlGroup08.Remove(this);
        }
        if (_UM.controlGroup09.Contains(this))
        {
            _UM.controlGroup09.Remove(this);
        }
        if (_UM.controlGroup10.Contains(this))
        {
            _UM.controlGroup10.Remove(this);
        }
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

    IEnumerator RangedAttack()
    {

        while (Vector3.Distance(transform.position, attackDestination) > 50f)
        {
            navAgent.SetDestination(attackDestination);
            //animator.SetBool("isAttacking", false);
            yield return null;
        }
        while (Vector3.Distance(transform.position, attackDestination) < 50f)
        {
            transform.LookAt(attackDestination);
            //animator.SetBool("isAttacking", true);

            yield return null;
        }
        yield return null;

        //animator.SetBool("isAttacking", false);
        StartCoroutine(RangedAttack());

    }
  
    public Transform GetClosestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        Transform trans = null;

        foreach (GameObject go in _EM.enemies)
        {
            float currentDistance;
            currentDistance = Vector3.Distance(transform.position, go.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                trans = go.transform;
            }
        }
            return trans;
    }

    public void EnableCollider()
    {
        weaponCollider.SetActive(true);
    }
    public void DisableCollider()
    {
        weaponCollider.SetActive(false);
    }
    //public void MouseOverEnemy()
    //{
    //    mouseOverEnemy = true;
    //}
    //public void MouseOffEnemy()
    //{
    //    mouseOverEnemy = false;
    //}

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

    public void OnBorkrskinnUpgrade()
    {
        Setup();
        //StartCoroutine(SetupOld());
        //switch (unitType)
        //{
        //    case UnitType.SatyrUnit:
        //        maxHealth = _GM.GetPercentageIncrease(_GM.satyrHealth, 0.3f);
        //        health = maxHealth;
        //        break;

        //    case UnitType.LeshyUnit:
        //        maxHealth = _GM.GetPercentageIncrease(_GM.leshyHealth, 0.3f);
        //        health = maxHealth;
        //        break;
        //    case UnitType.OrcusUnit:
        //        maxHealth = _GM.GetPercentageIncrease(_GM.orcusHealth, 0.3f);
        //        health = maxHealth;
        //        break;
        //    case UnitType.VolvaUnit:
        //        maxHealth = _GM.GetPercentageIncrease(_GM.skessaHealth, 0.3f);
        //        health = maxHealth;
        //        break;
        //    case UnitType.GoblinUnit:
        //        maxHealth = _GM.GetPercentageIncrease(_GM.goblinHealth, 0.3f);
        //        health = maxHealth;
        //        break;
        //    case UnitType.GolemUnit:
        //        maxHealth = _GM.GetPercentageIncrease(_GM.golemHealth, 0.3f);
        //        health = maxHealth;
        //        break;
        //    case UnitType.DryadUnit:
        //        maxHealth = _GM.GetPercentageIncrease(_GM.dryadHealth, 0.3f);
        //        health = maxHealth;
        //        break;
        //}
    }
    public void OnFlugafotrUpgrade()
    {
        Setup();
        //StartCoroutine(SetupOld());
        //switch (unitType)
        //{
        //    case UnitType.SatyrUnit:
        //        navAgent.speed = _GM.GetPercentageIncrease(_GM.satyrSpeed, 0.3f);
        //        break;

        //    case UnitType.LeshyUnit:
        //        navAgent.speed = _GM.GetPercentageIncrease(_GM.leshySpeed, 0.3f);
        //        break;
        //    case UnitType.OrcusUnit:
        //        navAgent.speed = _GM.GetPercentageIncrease(_GM.orcusSpeed, 0.3f);
        //        break;
        //    case UnitType.VolvaUnit:
        //        navAgent.speed = _GM.GetPercentageIncrease(_GM.skessaSpeed, 0.3f);
        //        break;
        //    case UnitType.HuldraUnit:
        //        navAgent.speed = _GM.GetPercentageIncrease(_GM.huldraSpeed, 0.3f);
        //        break;
        //    case UnitType.GoblinUnit:
        //        navAgent.speed = _GM.GetPercentageIncrease(_GM.goblinSpeed, 0.3f);
        //        break;
        //    case UnitType.GolemUnit:
        //        navAgent.speed = _GM.GetPercentageIncrease(_GM.golemSpeed, 0.3f);
        //        break;
        //}
    }
    IEnumerator WaitForSetDestination()
    {
        yield return new WaitForEndOfFrame();
        navAgent.SetDestination(targetDest.transform.position);
    }
    private void OnContinueButton()
    {
        if(unitID != CreatureID.Mistcalf)
        {
            SetHealth(maxHealth);
        }
    }
    public void OnAttackSelected()
    {
        if(isSelected)
        {
            ChangeCombatModeIcon(attackSprite);
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
    }
    public void OnDefendSelected()
    {
        if (isSelected)
        {
            if (combatMode != CombatMode.Defend)
            {
                if (unitID != CreatureID.Goblin)
                {
                    detectionRadius = detectionRadius / 2;
                }
                ChangeCombatModeIcon(defendSprite);
            }
            defendPosition = transform.position;
            combatMode = CombatMode.Defend;
        }
    }
    private void OnEnable()
    {
        GameEvents.OnAttackSelected += OnAttackSelected;
        GameEvents.OnDefendSelected += OnDefendSelected;
        GameEvents.OnBorkrskinnUpgrade += OnBorkrskinnUpgrade;
        GameEvents.OnFlugafotrUpgrade += OnFlugafotrUpgrade;
        GameEvents.OnContinueButton += OnContinueButton;
    }

    private void OnDisable()
    {
        GameEvents.OnAttackSelected -= OnAttackSelected;
        GameEvents.OnDefendSelected -= OnDefendSelected;
        GameEvents.OnBorkrskinnUpgrade -= OnBorkrskinnUpgrade;
        GameEvents.OnFlugafotrUpgrade -= OnFlugafotrUpgrade;
        GameEvents.OnContinueButton -= OnContinueButton;
    }


    #region Old and Remove
    /*IEnumerator SetupOld()
    {
        yield return new WaitForEndOfFrame();
        switch (unitID)
        {
            case CreatureID.Satyr:
                if (_PERK.HasPerk(PerkID.BarkSkin))
                {
                    maxHealth = _GM.GetPercentageIncrease(_GM.satyrHealth, 0.3f);
                }

                if (_PERK.HasPerk(PerkID.FlyFoot))
                {
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.satyrSpeed, 0.3f);
                }

                detectionRadius = 50;

                break;

            case CreatureID.Leshy:
                if (_PERK.HasPerk(PerkID.BarkSkin))
                {
                    maxHealth = _GM.GetPercentageIncrease(_GM.leshyHealth, 0.3f);
                }

                if (_PERK.HasPerk(PerkID.FlyFoot))
                {
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.leshySpeed, 0.3f);
                }
                detectionRadius = 50;
                break;
            case CreatureID.Orcus:
                if (_PERK.HasPerk(PerkID.BarkSkin))
                {
                    maxHealth = _GM.GetPercentageIncrease(_GM.orcusHealth, 0.3f);
                }

                if (_PERK.HasPerk(PerkID.FlyFoot))
                {
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.orcusSpeed, 0.3f);
                }
                detectionRadius = 50;

                break;
            case CreatureID.Skessa:
                if (_PERK.HasPerk(PerkID.BarkSkin))
                {
                    maxHealth = _GM.GetPercentageIncrease(_GM.skessaHealth, 0.3f);
                }

                if (_PERK.HasPerk(PerkID.FlyFoot))
                {
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.skessaSpeed, 0.3f);
                }
                detectionRadius = 50;
                break;
            case CreatureID.Huldra:
                if (_PERK.HasPerk(PerkID.BarkSkin))
                {
                    maxHealth = _GM.GetPercentageIncrease(_GM.huldraHealth, 0.3f);
                }

                if (_PERK.HasPerk(PerkID.FlyFoot))
                {
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.huldraSpeed, 0.3f);
                }
                detectionRadius = 50;
                break;
            case CreatureID.Goblin:
                if (_PERK.HasPerk(PerkID.BarkSkin))
                {
                    maxHealth = _GM.GetPercentageIncrease(_GM.goblinHealth, 0.3f);
                }

                if (_PERK.HasPerk(PerkID.FlyFoot))
                {
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.goblinSpeed, 0.3f);
                }
                detectionRadius = 50;
                break;
            case CreatureID.Tower:
                if (_PERK.HasPerk(PerkID.BarkSkin))
                {
                    health = 130;
                    maxHealth = 130;
                }

                break;
            case CreatureID.Mistcalf:
                if (_PERK.HasPerk(PerkID.BarkSkin))
                {
                    maxHealth = _GM.GetPercentageIncrease(_GM.golemHealth, 0.3f);
                }

                if (_PERK.HasPerk(PerkID.FlyFoot))
                {
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.golemSpeed, 0.3f);
                }
                detectionRadius = 75;

                break;
            case CreatureID.Fidhain:
                if (_PERK.HasPerk(PerkID.BarkSkin))
                {
                    maxHealth = _GM.GetPercentageIncrease(_GM.dryadHealth, 0.3f);
                }

                if (_PERK.HasPerk(PerkID.FlyFoot))
                {
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.dryadSpeed, 0.3f);
                }
                detectionRadius = 50;
                break;

        }
        health = maxHealth;
        slider.value = CalculateHealth();
    }
    */

    #endregion

}
