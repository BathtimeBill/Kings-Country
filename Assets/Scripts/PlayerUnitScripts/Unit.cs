using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Unit : GameBehaviour
{
    [Header("Unit Type")]
    public UnitID unitType;
    [Header("CombatMode")]
    public CombatMode combatMode;
    public Image combatModeImage;
    public Sprite attackSprite;
    public Sprite defendSprite;
    public Vector3 defendPosition;
    public float unitSpeed;
    public float tickRate;

    [Header("Stats")] 
    public float health;
    public float maxHealth;
    public float projectileSpeed = 1000;
    public Slider slider;
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

    [Header("Death Objects")]
    public GameObject deadSatyr;
    public GameObject bloodParticle1;
    [Header("Relevant Game Objects")]
    public GameObject targetDest;
    public GameObject selectionCircle;
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


    void Start()
    {
        soundPool = SFXPool.GetComponents<AudioSource>();
        pointer = GameObject.FindGameObjectWithTag("Pointer");
        ApplyPerks();
        StartCoroutine(Setup());
        UnitSelection.Instance.unitList.Add(gameObject);

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
        if (_EM.enemies.Count != 0)
        {
            closestEnemy = GetClosestEnemy();
            distanceToClosestEnemy = Vector3.Distance(closestEnemy.transform.position, transform.position);
            if (closestEnemy.gameObject.tag == "Lord")
            {
                if(unitType != UnitID.Fidhain || unitType != UnitID.Goblin)
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
                if (_EM.enemies.Count > 0)
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
                    if (unitType == UnitID.Goblin || unitType == UnitID.Fidhain)
                    {
                        navAgent.stoppingDistance = 4;
                    }

                }

                //animator.SetBool("inCombat", false);
                break;

            case UnitState.Attack:
                //navAgent.angularSpeed = 500;
                if (_EM.enemies.Count == 0)
                {
                    state = UnitState.Idle;
                }
                if (distanceToClosestEnemy >= detectionRadius)
                {
                    if (hitByArrow == false)
                        state = UnitState.Moving;
                }
                if (unitType == UnitID.Goblin)
                {
                    navAgent.stoppingDistance = 50;
                }
                //else
                //{
                //    navAgent.stoppingDistance = stoppingDistance;
                //}
                if (unitType == UnitID.Fidhain)
                {
                    navAgent.stoppingDistance = 20;
                }
                if (_EM.enemies.Count != 0)
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
                if (unitType == UnitID.Leshy)
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
                if (unitType == UnitID.Goblin || unitType == UnitID.Fidhain)
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
                    if (unitType == UnitID.Goblin)
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
                    if (_HM.units.Contains(gameObject))
                    {
                        _HM.units.Remove(gameObject);
                    }
                    if (_HUTM.units.Contains(gameObject))
                    {
                        _HUTM.units.Remove(gameObject);
                    }
                    UnitSelection.Instance.Deselect(gameObject);
                    UnitSelection.Instance.unitList.Remove(gameObject);
                    GameObject go;
                    go = Instantiate(deadSatyr, transform.position, transform.rotation);
                    Destroy(go, 15);
                    _UI.CheckPopulousUI();
                    GameEvents.ReportOnUnitKilled();
                    CheckIfUnitIsInGroup();
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            selectionCircle.SetActive(false);
        }


        if (health > maxHealth)
        {
            health = maxHealth;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Vector3 offset = new Vector3(0, -1.5f, 0);
            if (unitType == UnitID.Huldra && isSelected || unitType == UnitID.Fidhain && isSelected)
            {
                if (_TUTM.isTutorial && _TUTM.tutorialStage == 13)
                {
                    GameEvents.ReportOnNextTutorial();
                }
                if (isTooCloseToTower == false && isOutOfBounds == false)
                {
                    Instantiate(towerPrefab, transform.position + offset, Quaternion.Euler(-90, 0, 0));
                    UnitSelection.Instance.Deselect(gameObject);
                    UnitSelection.Instance.unitList.Remove(gameObject);
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
                if (_EM.enemies.Count > 0)
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
                    if (unitType == UnitID.Goblin || unitType == UnitID.Fidhain)
                    {
                        navAgent.stoppingDistance = 4;
                    }

                }

                //animator.SetBool("inCombat", false);
                break;

            case UnitState.Attack:
                //navAgent.angularSpeed = 500;
                if (_EM.enemies.Count == 0)
                {
                    state = UnitState.Idle;
                }
                if (distanceToClosestEnemy >= detectionRadius)
                {
                    if (hitByArrow == false)
                        state = UnitState.Moving;
                }
                if (unitType == UnitID.Goblin)
                {
                    navAgent.stoppingDistance = 50;
                }
                //else
                //{
                //    navAgent.stoppingDistance = stoppingDistance;
                //}
                if (unitType == UnitID.Fidhain)
                {
                    navAgent.stoppingDistance = 20;
                }
                if (_EM.enemies.Count != 0)
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
                if (unitType == UnitID.Leshy)
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
                if (unitType == UnitID.Goblin || unitType == UnitID.Fidhain)
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
                    if (unitType == UnitID.Goblin)
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

    float CalculateHealth()
    {
        return health / maxHealth;
    }
    
    IEnumerator HitByArrowDelay()
    {
        yield return new WaitForSeconds(1);
        hitByArrow = false;
        detectionRadius = detectionRadius / 2;
        state = UnitState.Moving;
        navAgent.SetDestination(transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Axe1")
        {
            TakeDamage(_GM.axe1Damage);
            other.enabled = false;
        }
        if (other.tag == "Axe2")
        {
            TakeDamage(_GM.axe2Damage);
            other.enabled = false;
        }
        if (other.tag == "Sword2")
        {
            if(unitType != UnitID.Leshy)
            {
                TakeDamage(_GM.sword2Damage);
            }
            else
            {
                TakeDamage(50);
            }
 
            other.enabled = false;
        }
        if (other.tag == "Sword3")
        {
            if (unitType != UnitID.Leshy)
            {
                TakeDamage(_GM.sword3Damage);
            }
            else
            {
                TakeDamage(65);
            }
            other.enabled = false;
        }
        if (other.tag == "Arrow")
        {
            hitByArrow = true;
            detectionRadius = detectionRadius * 2;
            state = UnitState.Attack;
            StartCoroutine(HitByArrowDelay());
            if (unitType == UnitID.Skessa)
            {
                TakeDamage(_GM.arrow1Damage * 3);
            }
            else
            {
                TakeDamage(_GM.arrow1Damage);
            }

            Destroy(other.gameObject);
        }
        if (other.tag == "Arrow2")
        {
            hitByArrow = true;
            detectionRadius = detectionRadius * 2;
            state = UnitState.Attack;
            StartCoroutine(HitByArrowDelay());
            if (unitType == UnitID.Skessa)
            {
                TakeDamage(_GM.arrow2Damage * 3);
            }
            else
            {
                TakeDamage(_GM.arrow2Damage);
            }

            Destroy(other.gameObject);
        }
        if(other.tag == "Heal")
        {
            if(unitType != UnitID.Mistcalf)
            {
                health += 100;
                slider.value = slider.value = CalculateHealth();
            }
        }
        if (other.tag == "Maegen")
        {
            _GM.maegen += 1;
        }
        if (other.tag == "Horgr")
        {
            if (!_HM.units.Contains(gameObject))
                _HM.units.Add(gameObject);
            GameEvents.ReportOnUnitArrivedAtHorgr();
        }
        if (other.tag == "Hut")
        {
            if (!_HUTM.units.Contains(gameObject))
                _HUTM.units.Add(gameObject);
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
            TakeDamage(_GM.lordDamage);
        }
        if (other.tag == "Lord")
        {
            state = UnitState.Attack;
        }
        if (other.tag == "Explosion3")
        {
            TakeDamage(100);
        }
        if (other.tag == "Rune")
        {
            if (unitType != UnitID.Mistcalf)
            {
                healingParticle.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Horgr")
        {
            _HM.units.Remove(gameObject);
        }
        if(other.tag == "Hut")
        {
            _HUTM.units.Remove(gameObject);
        }
        if (other.tag == "Tower")
        {
            isTooCloseToTower = false;
        }
        //if (other.tag == "Boundry")
        //{
        //    isOutOfBounds = true;
        //}
        if (unitType != UnitID.Mistcalf)
        {
            healingParticle.SetActive(false);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Axe3")
        {
            health -= 0.5f * Time.deltaTime;
            slider.value = slider.value = CalculateHealth();
            if(health < 0)
            {
                Die();
            }
        }
        if(other.tag == "Rune")
        {
            if(unitType != UnitID.Mistcalf)
            {
                if (_UM.hasUpgrade(UpgradeID.Rune))
                {
                    health += _GM.runeHealRate * 2 * Time.deltaTime;
                }
                else
                {
                    health += _GM.runeHealRate * Time.deltaTime;
                }
                slider.value = slider.value = CalculateHealth();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        state = UnitState.Attack;
        GameObject go = Instantiate(bloodParticle1, transform.position + new Vector3(0, 5, 0), transform.rotation);
        //go.transform.rotation = Quaternion.Inverse(transform.rotation);
        health -= damage;
        Die();
        slider.value = slider.value = CalculateHealth();
    }

    private void Die()
    {
        bool isColliding = false;
        if (health <= 0)
        {
            if (_HM.units.Contains(gameObject))
            {
                _HM.units.Remove(gameObject);
            }
            if(_HUTM.units.Contains(gameObject))
            {
                _HUTM.units.Remove(gameObject);
            }
            UnitSelection.Instance.Deselect(gameObject);
            UnitSelection.Instance.unitList.Remove(gameObject);

            if(!isColliding)
            {
                GameObject go;
                go = Instantiate(deadSatyr, transform.position, transform.rotation);
                isColliding = true;
                Destroy(go, 15);
            }
            _UI.CheckPopulousUI();
            GameEvents.ReportOnUnitKilled();
            CheckIfUnitIsInGroup();
            Destroy(gameObject);
        }
    }
    void CheckIfUnitIsInGroup()
    {
        if (UnitSelection.Instance.controlGroup1.Contains(gameObject))
        {
            UnitSelection.Instance.controlGroup1.Remove(gameObject);
        }
        if (UnitSelection.Instance.controlGroup2.Contains(gameObject))
        {
            UnitSelection.Instance.controlGroup2.Remove(gameObject);
        }
        if (UnitSelection.Instance.controlGroup3.Contains(gameObject))
        {
            UnitSelection.Instance.controlGroup3.Remove(gameObject);
        }
        if (UnitSelection.Instance.controlGroup4.Contains(gameObject))
        {
            UnitSelection.Instance.controlGroup4.Remove(gameObject);
        }
        if (UnitSelection.Instance.controlGroup5.Contains(gameObject))
        {
            UnitSelection.Instance.controlGroup5.Remove(gameObject);
        }
        if (UnitSelection.Instance.controlGroup6.Contains(gameObject))
        {
            UnitSelection.Instance.controlGroup6.Remove(gameObject);
        }
        if (UnitSelection.Instance.controlGroup7.Contains(gameObject))
        {
            UnitSelection.Instance.controlGroup7.Remove(gameObject);
        }
        if (UnitSelection.Instance.controlGroup8.Contains(gameObject))
        {
            UnitSelection.Instance.controlGroup8.Remove(gameObject);
        }
        if (UnitSelection.Instance.controlGroup9.Contains(gameObject))
        {
            UnitSelection.Instance.controlGroup9.Remove(gameObject);
        }
        if (UnitSelection.Instance.controlGroup10.Contains(gameObject))
        {
            UnitSelection.Instance.controlGroup10.Remove(gameObject);
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
    IEnumerator Setup()
    {
        yield return new WaitForEndOfFrame();
        switch (unitType)
        {
            case UnitID.Satyr:
                if(_UM.hasUpgrade(UpgradeID.BarkSkin) && _PERK.satyrPerk == false)
                {
                    maxHealth = _GM.GetPercentageIncrease(_GM.satyrHealth, 0.3f);
                }

                if(_UM.hasUpgrade(UpgradeID.FlyFoot) && _PERK.satyrPerk == false)
                {
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.satyrSpeed, 0.3f); 
                }

                detectionRadius = 50;

                break;

            case UnitID.Leshy:
                if (_UM.hasUpgrade(UpgradeID.BarkSkin) && _PERK.leshyPerk == false)
                {
                    maxHealth = _GM.GetPercentageIncrease(_GM.leshyHealth, 0.3f);
                }

                if (_UM.hasUpgrade(UpgradeID.FlyFoot) && _PERK.leshyPerk == false)
                {
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.leshySpeed, 0.3f);
                }
                detectionRadius = 50;
                break;
            case UnitID.Orcus:
                if (_UM.hasUpgrade(UpgradeID.BarkSkin) && _PERK.orcusPerk == false)
                {
                    maxHealth = _GM.GetPercentageIncrease(_GM.orcusHealth, 0.3f);
                }

                if (_UM.hasUpgrade(UpgradeID.FlyFoot) && _PERK.orcusPerk == false)
                {
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.orcusSpeed, 0.3f);
                }
                detectionRadius = 50;

                break;
            case UnitID.Skessa:
                if (_UM.hasUpgrade(UpgradeID.BarkSkin) && _PERK.skessaPerk == false)
                {
                    maxHealth = _GM.GetPercentageIncrease(_GM.skessaHealth, 0.3f);
                }

                if (_UM.hasUpgrade(UpgradeID.FlyFoot) && _PERK.skessaPerk == false)
                {
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.skessaSpeed, 0.3f);
                }
                detectionRadius = 50;
                break;
            case UnitID.Huldra:
                if (_UM.hasUpgrade(UpgradeID.BarkSkin) && _PERK.huldraPerk == false)
                {
                    maxHealth = _GM.GetPercentageIncrease(_GM.huldraHealth, 0.3f);
                }

                if (_UM.hasUpgrade(UpgradeID.FlyFoot) && _PERK.huldraPerk == false)
                {
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.huldraSpeed, 0.3f);
                }
                detectionRadius = 50;
                break;
            case UnitID.Goblin:
                if (_UM.hasUpgrade(UpgradeID.BarkSkin) && _PERK.goblinPerk == false)
                {
                    maxHealth = _GM.GetPercentageIncrease(_GM.goblinHealth, 0.3f);
                }

                if (_UM.hasUpgrade(UpgradeID.FlyFoot) && _PERK.goblinPerk == false)
                {
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.goblinSpeed, 0.3f);
                }
                detectionRadius = 50;
                break;
            case UnitID.Tower:
                if (_UM.hasUpgrade(UpgradeID.BarkSkin))
                {
                    health = 130;
                    maxHealth = 130;
                }

                break;
            case UnitID.Mistcalf:
                if (_UM.hasUpgrade(UpgradeID.BarkSkin) && _PERK.golemPerk == false)
                {
                    maxHealth = _GM.GetPercentageIncrease(_GM.golemHealth, 0.3f);
                }

                if (_UM.hasUpgrade(UpgradeID.FlyFoot) && _PERK.golemPerk == false)
                {
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.golemSpeed, 0.3f);
                }
                detectionRadius = 75;

                break;
            case UnitID.Fidhain:
                if (_UM.hasUpgrade(UpgradeID.FlyFoot) && _PERK.fidhainPerk == false)
                {
                    maxHealth = _GM.GetPercentageIncrease(_GM.dryadHealth, 0.3f);
                }

                if (_UM.hasUpgrade(UpgradeID.FlyFoot) && _PERK.fidhainPerk == false)
                {
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.dryadSpeed, 0.3f);
                }
                detectionRadius = 50;
                break;
            
        }
        health = maxHealth;
        slider.value = CalculateHealth();
    }

    void ApplyPerks()
    {
        switch (unitType)
        {
            case UnitID.Satyr:
                if(_PERK.satyrPerk == true)
                {
                    health = _GM.GetPercentageIncrease(_GM.satyrHealth, 0.5f);
                    maxHealth = _GM.GetPercentageIncrease(_GM.satyrHealth, 0.5f);
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.satyrSpeed, 0.5f);
                    isUpgraded = true;
                }
                else
                {
                    health = _GM.satyrHealth;
                    maxHealth = _GM.satyrHealth;
                    navAgent.speed = _GM.satyrSpeed;
                }
                break;

            case UnitID.Leshy:
                if (_PERK.leshyPerk == true)
                {
                    health = _GM.GetPercentageIncrease(_GM.leshyHealth, 0.5f);
                    maxHealth = _GM.GetPercentageIncrease(_GM.leshyHealth, 0.5f);
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.leshySpeed, 0.5f);
                }
                else
                {
                    health = _GM.leshyHealth;
                    maxHealth = _GM.leshyHealth;
                    navAgent.speed = _GM.leshySpeed;
                }
                break;
            case UnitID.Orcus:
                if (_PERK.orcusPerk == true)
                {
                    health = _GM.GetPercentageIncrease(_GM.orcusHealth, 0.5f);
                    maxHealth = _GM.GetPercentageIncrease(_GM.orcusHealth, 0.5f);
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.orcusSpeed, 0.5f);
                }
                else
                {
                    health = _GM.orcusHealth;
                    maxHealth = _GM.orcusHealth;
                    navAgent.speed = _GM.orcusSpeed;
                }
                break;
            case UnitID.Skessa:
                if (_PERK.skessaPerk == true)
                {
                    health = _GM.GetPercentageIncrease(_GM.skessaHealth, 0.5f);
                    maxHealth = _GM.GetPercentageIncrease(_GM.skessaHealth, 0.5f);
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.skessaSpeed, 0.5f);
                }
                else
                {
                    health = _GM.skessaHealth;
                    maxHealth = _GM.skessaHealth;
                    navAgent.speed = _GM.skessaSpeed;
                }
                break;
            case UnitID.Huldra:
                if (_PERK.huldraPerk == true)
                {
                    health = _GM.GetPercentageIncrease(_GM.huldraHealth, 0.5f);
                    maxHealth = _GM.GetPercentageIncrease(_GM.huldraHealth, 0.5f);
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.huldraSpeed, 0.5f);
                }
                else
                {
                    health = _GM.huldraHealth;
                    maxHealth = _GM.huldraHealth;
                    navAgent.speed = _GM.huldraSpeed;
                }
                break;
            case UnitID.Goblin:
                if (_PERK.goblinPerk == true)
                {
                    health = _GM.GetPercentageIncrease(_GM.goblinHealth, 0.5f);
                    maxHealth = _GM.GetPercentageIncrease(_GM.goblinHealth, 0.5f);
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.goblinSpeed, 0.5f);
                }
                else
                {
                    health = _GM.goblinHealth;
                    maxHealth = _GM.goblinHealth;
                    navAgent.speed = _GM.goblinSpeed;
                }
                break;
            case UnitID.Mistcalf:
                if (_PERK.golemPerk == true)
                {
                    health = _GM.GetPercentageIncrease(_GM.golemHealth, 0.5f);
                    maxHealth = _GM.GetPercentageIncrease(_GM.golemHealth, 0.5f);
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.golemSpeed, 0.5f);
                }
                else
                {
                    health = _GM.golemHealth;
                    maxHealth = _GM.golemHealth;
                    navAgent.speed = _GM.golemSpeed;
                }
                break;
            case UnitID.Fidhain:
                if (_PERK.fidhainPerk == true)
                {
                    health = _GM.GetPercentageIncrease(_GM.dryadHealth, 0.5f);
                    maxHealth = _GM.GetPercentageIncrease(_GM.dryadHealth, 0.5f);
                    navAgent.speed = _GM.GetPercentageIncrease(_GM.dryadSpeed, 0.5f);
                }
                else
                {
                    health = _GM.dryadHealth;
                    maxHealth = _GM.dryadHealth;
                    navAgent.speed = _GM.dryadSpeed;
                }
                break;
        }
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
            health -= 5;
        }
    }

    public void OnBorkrskinnUpgrade()
    {
        StartCoroutine(Setup());
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
        slider.value = slider.value = CalculateHealth();
    }
    public void OnFlugafotrUpgrade()
    {
        StartCoroutine(Setup());
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
        if(unitType != UnitID.Mistcalf)
        {
            health = maxHealth;
            slider.value = slider.value = CalculateHealth();
        }
    }
    public void OnAttackSelected()
    {
        if(isSelected)
        {
            combatModeImage.sprite = attackSprite;
            if(combatMode != CombatMode.Move || combatMode != CombatMode.AttackMove)
            {
                detectionRadius = detectionRadius * 2;
                if (unitType == UnitID.Goblin)
                {
                    navAgent.speed = _GM.goblinSpeed;
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
                if (unitType != UnitID.Goblin)
                {
                    detectionRadius = detectionRadius / 2;
                }
                combatModeImage.sprite = defendSprite;
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

}
