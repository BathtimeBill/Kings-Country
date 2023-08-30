using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Unit : GameBehaviour
{
    [Header("Unit Type")]
    public UnitType unitType;

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
    [Header("Bools")]
    public bool isSelected;
    public bool inCombat;
    public bool isMoving;
    public bool isMovingCheck;
    public bool isTooCloseToTower;
    public bool isOutOfBounds;
    //public float isMovingCheckTime;
    private Vector3 attackDestination;
    [Header("Audio")]
    public GameObject SFXPool;
    public int soundPoolCurrent;
    public AudioSource[] soundPool;
    public AudioSource vocalSource;
    public AudioSource spellSource;
    private Transform[] rangedAttackLocations;


    void Start()
    {
        isOutOfBounds = true;
        soundPool = SFXPool.GetComponents<AudioSource>();
        pointer = GameObject.FindGameObjectWithTag("Pointer");
        Setup();
        UnitSelection.Instance.unitList.Add(gameObject);
        health = maxHealth;
        slider.value = CalculateHealth();
    }

    IEnumerator WaitForIsMovingCheck()
    {
        yield return new WaitForSeconds(1.5f);
        if(isMoving == false)
        {
            state = UnitState.Idle;
        }    

    }

    void Update()
    {
        if (_EM.enemies.Length != 0)
        {
            closestEnemy = GetClosestEnemy();
            distanceToClosestEnemy = Vector3.Distance(closestEnemy.transform.position, transform.position);
        }

        switch (state)
        {
            case UnitState.Idle:
                if (_EM.enemies.Length > 0)
                {
                    if (distanceToClosestEnemy < detectionRadius)
                    {
                        state = UnitState.Attack;
                    }
                    else
                    {
                        navAgent.SetDestination(transform.position);
                    }
                }

                animator.SetBool("inCombat", false);
                break;

            case UnitState.Attack:
                if (_EM.enemies.Length == 0 || distanceToClosestEnemy >= detectionRadius)
                {
                    state = UnitState.Idle;
                }
                if (unitType == UnitType.GoblinUnit)
                {
                    navAgent.stoppingDistance = 30;
                }
                else
                {
                    navAgent.stoppingDistance = stoppingDistance;
                }
                if(_EM.enemies.Length != 0)
                {
                    navAgent.SetDestination(closestEnemy.transform.position);
                    SmoothFocusOnEnemy();
                }
                animator.SetBool("inCombat", true);

                break;
            case UnitState.Moving:
                if (isMovingCheck == false)
                {
                    isMovingCheck = true;
                    StartCoroutine(WaitForIsMovingCheck());
                }
                if (unitType == UnitType.LeshyUnit)
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
                if (unitType == UnitType.GoblinUnit)
                {
                    navAgent.stoppingDistance = 4;
                }
                break;
            case UnitState.Track:
                if(trackTarget != null)
                {
                    navAgent.SetDestination(trackTarget.transform.position);
                    if(unitType == UnitType.GoblinUnit)
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
                    state = UnitState.Attack;
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
                Destroy(gameObject);
            }
        }
        else
        {
            selectionCircle.SetActive(false);
        }


        if (navAgent.velocity != Vector3.zero)
        {
            animator.SetBool("isMoving", true);
            isMoving = true;
        }
        if (navAgent.velocity == Vector3.zero)
        {
            animator.SetBool("isMoving", false);
            isMoving = false;
            isMovingCheck = false;
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Vector3 offset = new Vector3(0, -1.5f, 0);
            if (unitType == UnitType.HuldraUnit && isSelected)
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
                    _UI.SetErrorMessageTooCloseToTower();
                    _PC.Error();
                }
                if (isOutOfBounds == true && isTooCloseToTower == false)
                {
                    _UI.SetErrorMessageOutOfBounds();
                    _PC.Error();
                }
            }

        }
    }
    
    public void PlayFootstepSound()
    {
        PlaySound(_SM.GetForestFootstepSound());
    }
    public void PlayFlapSound()
    {
        PlaySound(_SM.GetFlapSound());
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
            if(unitType != UnitType.LeshyUnit)
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
            if (unitType != UnitType.LeshyUnit)
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
            TakeDamage(_GM.arrow1Damage);
            Destroy(other.gameObject);
        }
        if (other.tag == "Arrow2")
        {
            TakeDamage(_GM.arrow2Damage);
            Destroy(other.gameObject);
        }
        if(other.tag == "Heal")
        {
            health += 100;
            slider.value = slider.value = CalculateHealth();
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
        if (other.tag == "Boundry")
        {
            isOutOfBounds = false;
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
        if (other.tag == "Boundry")
        {
            isOutOfBounds = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Axe3")
        {
            health -= 0.5f * Time.deltaTime;
            slider.value = slider.value = CalculateHealth();
            if(health < 1)
            {
                Die();
            }
        }
        if(other.tag == "Rune")
        {
            if(_UM.rune)
            {
                health += 8 * Time.deltaTime;
            }
            else
            {
                health += 4 * Time.deltaTime;
            }

            slider.value = slider.value = CalculateHealth();
        }
    }
    public void TakeDamage(float damage)
    {
        state = UnitState.Attack;
        Instantiate(bloodParticle1, transform.position, transform.rotation = Quaternion.Inverse(transform.rotation));
        health -= damage;
        Die();
        slider.value = slider.value = CalculateHealth();
    }

    private void Die()
    {
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
            GameObject go;
            go = Instantiate(deadSatyr, transform.position, transform.rotation);
            Destroy(go, 15);
            _UI.CheckPopulousUI();
            Destroy(gameObject);
        }
    }

    IEnumerator Attack()
    {

        while (Vector3.Distance(transform.position, attackDestination) > 4f)
        {
            navAgent.SetDestination(attackDestination);
            animator.SetBool("isAttacking", false);
            yield return null;
        }
        while (Vector3.Distance(transform.position, attackDestination) < 4f)
        {
            transform.LookAt(attackDestination);
            animator.SetBool("isAttacking", true);

            yield return null;
        }
        yield return null;

        animator.SetBool("isAttacking", false);
        StartCoroutine(Attack());

    }

    IEnumerator RangedAttack()
    {

        while (Vector3.Distance(transform.position, attackDestination) > 50f)
        {
            navAgent.SetDestination(attackDestination);
            animator.SetBool("isAttacking", false);
            yield return null;
        }
        while (Vector3.Distance(transform.position, attackDestination) < 50f)
        {
            transform.LookAt(attackDestination);
            animator.SetBool("isAttacking", true);

            yield return null;
        }
        yield return null;

        animator.SetBool("isAttacking", false);
        StartCoroutine(RangedAttack());

    }
    void Setup()
    {
        switch (unitType)
        {
            case UnitType.SatyrUnit:
                if(_UM.borkrskinn)
                {
                    health = _GM.GetPercentageIncrease(_GM.satyrHealth, 0.3f);
                    maxHealth = _GM.GetPercentageIncrease(_GM.satyrHealth, 0.3f);
                }
                else
                {
                    health = _GM.satyrHealth;
                    maxHealth = _GM.satyrHealth;
                }
                if(_UM.flugafotr)
                {
                    navAgent.speed = 21;
                }
                else
                {
                    navAgent.speed = 16;
                }
                detectionRadius = 50;

                break;

            case UnitType.LeshyUnit:
                if (_UM.borkrskinn)
                {
                    health = _GM.GetPercentageIncrease(_GM.leshyHealth, 0.3f);
                    maxHealth = _GM.GetPercentageIncrease(_GM.leshyHealth, 0.3f);

                }
                else
                {
                    health = _GM.leshyHealth;
                    maxHealth = _GM.leshyHealth;
                }
                if (_UM.flugafotr)
                {
                    navAgent.speed = 11;
                }
                else
                {
                    navAgent.speed = 8;
                }
                detectionRadius = 50;
                break;
            case UnitType.OrcusUnit:
                if (_UM.borkrskinn)
                {
                    health = _GM.GetPercentageIncrease(_GM.orcusHealth, 0.3f);
                    maxHealth = _GM.GetPercentageIncrease(_GM.orcusHealth, 0.3f);
                }
                else
                {
                    health = _GM.orcusHealth;
                    maxHealth = _GM.orcusHealth;
                }
                if (_UM.flugafotr)
                {
                    navAgent.speed = 17;
                }
                else
                {
                    navAgent.speed = 13;
                }
                detectionRadius = 50;

                break;
            case UnitType.VolvaUnit:
                if (_UM.borkrskinn)
                {
                    health = _GM.GetPercentageIncrease(_GM.skessaHealth, 0.3f);
                    maxHealth = _GM.GetPercentageIncrease(_GM.skessaHealth, 0.3f);
                }
                else
                {
                    health = _GM.skessaHealth;
                    maxHealth = _GM.skessaHealth;
                }
                if (_UM.flugafotr)
                {
                    navAgent.speed = 50;
                }
                else
                {
                    navAgent.speed = 40;
                }
                detectionRadius = 50;
                break;
            case UnitType.HuldraUnit:
                if (_UM.borkrskinn)
                {
                    health = _GM.GetPercentageIncrease(_GM.huldraHealth, 0.3f);
                    maxHealth = _GM.GetPercentageIncrease(_GM.huldraHealth, 0.3f);
                }
                else
                {
                    health = _GM.huldraHealth;
                    maxHealth = _GM.huldraHealth;
                }
                if (_UM.flugafotr)
                {
                    navAgent.speed = 40;
                }
                else
                {
                    navAgent.speed = 30;
                }
                break;
            case UnitType.GoblinUnit:
                if (_UM.borkrskinn)
                {
                    health = _GM.GetPercentageIncrease(_GM.goblinHealth, 0.3f);
                    maxHealth = _GM.GetPercentageIncrease(_GM.goblinHealth, 0.3f);
                }
                else
                {
                    health = _GM.goblinHealth;
                    maxHealth = _GM.goblinHealth;
                }
                if (_UM.flugafotr)
                {
                    navAgent.speed = 21;
                }
                else
                {
                    navAgent.speed = 16;
                }
                detectionRadius = 50;
                break;
            case UnitType.Tower:
                if (_UM.borkrskinn)
                {
                    health = 130;
                    maxHealth = 130;
                }
                else
                {
                    health = 100;
                    maxHealth = 100;
                }
                if (_UM.flugafotr)
                {
                    navAgent.speed = 0;
                }
                else
                {
                    navAgent.speed = 0;
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
            animator.SetBool("inCombat", false);
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
        
        switch (unitType)
        {
            case UnitType.SatyrUnit:
                maxHealth = _GM.GetPercentageIncrease(_GM.satyrHealth, 0.3f);
                health = maxHealth;
                break;

            case UnitType.LeshyUnit:
                maxHealth = _GM.GetPercentageIncrease(_GM.leshyHealth, 0.3f);
                health = maxHealth;
                break;
            case UnitType.OrcusUnit:
                maxHealth = _GM.GetPercentageIncrease(_GM.orcusHealth, 0.3f);
                health = maxHealth;
                break;
            case UnitType.VolvaUnit:
                maxHealth = _GM.GetPercentageIncrease(_GM.skessaHealth, 0.3f);
                health = maxHealth;
                break;
            case UnitType.GoblinUnit:
                maxHealth = _GM.GetPercentageIncrease(_GM.goblinHealth, 0.3f);
                health = maxHealth;
                break;
        }
        slider.value = slider.value = CalculateHealth();
    }
    public void OnFlugafotrUpgrade()
    {
        switch (unitType)
        {
            case UnitType.SatyrUnit:
                navAgent.speed = 21;
                break;

            case UnitType.LeshyUnit:
                navAgent.speed = 11;
                break;
            case UnitType.OrcusUnit:
                navAgent.speed = 17;
                break;
            case UnitType.VolvaUnit:
                navAgent.speed = 50;
                break;
            case UnitType.GoblinUnit:
                navAgent.speed = 21;
                break;
        }
    }
    IEnumerator WaitForSetDestination()
    {
        yield return new WaitForEndOfFrame();
        navAgent.SetDestination(targetDest.transform.position);
    }    

    private void OnContinueButton()
    {
        health = maxHealth;
        slider.value = slider.value = CalculateHealth();
    }
    private void OnEnable()
    {
        GameEvents.OnBorkrskinnUpgrade += OnBorkrskinnUpgrade;
        GameEvents.OnFlugafotrUpgrade += OnFlugafotrUpgrade;
        GameEvents.OnContinueButton += OnContinueButton;
    }

    private void OnDisable()
    {
        GameEvents.OnBorkrskinnUpgrade -= OnBorkrskinnUpgrade;
        GameEvents.OnFlugafotrUpgrade -= OnFlugafotrUpgrade;
        GameEvents.OnContinueButton -= OnContinueButton;
    }

}
