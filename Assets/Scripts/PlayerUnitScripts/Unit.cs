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
    public Transform closestEnemy;
    public float distanceToClosestEnemy;
    public GameObject pointer;
    public bool hasStopped = false;
    [Header("Death Objects")]
    public GameObject deadSatyr;
    public GameObject bloodParticle1;
    [Header("Relevant Game Objects")]
    public GameObject[] targetDest;
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
        soundPool = SFXPool.GetComponents<AudioSource>();
        pointer = GameObject.FindGameObjectWithTag("Pointer");
        Setup();
        UnitSelection.Instance.unitList.Add(gameObject);
        health = maxHealth;
        slider.value = CalculateHealth();
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
                    if (distanceToClosestEnemy < 30)
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
                if (_EM.enemies.Length == 0 || distanceToClosestEnemy >= 30)
                {
                    state = UnitState.Idle;
                }
                if (closestEnemy.name == "LogCutter")
                {
                    navAgent.stoppingDistance = 10;
                }
                else
                {
                    navAgent.stoppingDistance = stoppingDistance;
                }
                navAgent.SetDestination(closestEnemy.transform.position);
                SmoothFocusOnEnemy();
                animator.SetBool("inCombat", true);

                break;
            case UnitState.Moving:
                if(unitType == UnitType.LeshyUnit)
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
                break;
        }

        if (isSelected)
        {
            if (Input.GetMouseButtonDown(1))
            {
                state = UnitState.Moving;
                StopAllCoroutines();
                //animator.SetBool("isAttacking", false);
                targetDest = GameObject.FindGameObjectsWithTag("Destination");
                GameEvents.ReportOnUnitMove();
                navAgent.SetDestination(targetDest[0].transform.position);
            }
        }
        else
        {
            selectionCircle.SetActive(false);
        }


        if (navAgent.velocity != Vector3.zero)
        {
            animator.SetBool("isMoving", true);
        }
        if (navAgent.velocity == Vector3.zero)
        {
            animator.SetBool("isMoving", false);
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            Vector3 offset = new Vector3(0, -1.5f, 0);
            if (unitType == UnitType.HuldraUnit && isSelected)
            {
                Instantiate(towerPrefab, transform.position + offset, Quaternion.Euler(-90,0,0));
                UnitSelection.Instance.DeselectAll();
                UnitSelection.Instance.unitList.Remove(gameObject);
                Destroy(gameObject);
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
            TakeDamage(10);
            other.enabled = false;
        }
        if (other.tag == "Axe2")
        {
            TakeDamage(20);
            other.enabled = false;
        }
        if (other.tag == "Sword2")
        {
            if(unitType != UnitType.LeshyUnit)
            {
                TakeDamage(35);
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
                TakeDamage(50);
            }
            else
            {
                TakeDamage(65);
            }
            other.enabled = false;
        }
        if (other.tag == "Arrow")
        {
            TakeDamage(15);
            Destroy(other.gameObject);
        }
        if (other.tag == "Arrow2")
        {
            TakeDamage(35);
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
            health += 5 * Time.deltaTime;
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
            UnitSelection.Instance.DeselectAll();
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
                    health = 130;
                    maxHealth = 130;
                }
                else
                {
                    health = 100;
                    maxHealth = 100;
                }
                if(_UM.flugafotr)
                {
                    navAgent.speed = 21;
                }
                else
                {
                    navAgent.speed = 16;
                }
                

                break;

            case UnitType.LeshyUnit:
                if (_UM.borkrskinn)
                {
                    health = 200;
                    maxHealth = 200;
                }
                else
                {
                    health = 260;
                    maxHealth = 260;
                }
                if (_UM.flugafotr)
                {
                    navAgent.speed = 11;
                }
                else
                {
                    navAgent.speed = 8;
                }

                break;
            case UnitType.OrcusUnit:
                if (_UM.borkrskinn)
                {
                    health = 195;
                    maxHealth = 195;
                }
                else
                {
                    health = 150;
                    maxHealth = 150;
                }
                if (_UM.flugafotr)
                {
                    navAgent.speed = 17;
                }
                else
                {
                    navAgent.speed = 13;
                }


                break;
            case UnitType.VolvaUnit:
                if (_UM.borkrskinn)
                {
                    health = 90;
                    maxHealth = 90;
                }
                else
                {
                    health = 65;
                    maxHealth = 65;
                }
                if (_UM.flugafotr)
                {
                    navAgent.speed = 50;
                }
                else
                {
                    navAgent.speed = 40;
                }
                break;
            case UnitType.HuldraUnit:
                if (_UM.borkrskinn)
                {
                    health = 65;
                    maxHealth = 65;
                }
                else
                {
                    health = 40;
                    maxHealth = 40;
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

        if (_EM.enemies == null)
        {
            return null;
        }
        else
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
                maxHealth = 130;
                health = maxHealth;
                break;

            case UnitType.LeshyUnit:
                maxHealth = 260;
                health = maxHealth;
                break;
            case UnitType.OrcusUnit:
                maxHealth = 195;
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
        }
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
