using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Unity.VisualScripting;

public class Warrior : GameBehaviour
{
    [Header("Hunter Type")]
    public WarriorType type;
    [Header("Tick")]
    public float seconds = 0.5f;
    [Header("Stats")]
    public float health;
    public float maxHealth;
    public EnemyState state;
    public float stoppingDistance;
    public GameObject homeTree;
    public bool hasArrivedAtBeacon;
    public GameObject fyreBeacon;
    public GameObject deadWarriorFire;
    public float speed;

    [Header("Death Objects")]
    public GameObject deathObject;
    public GameObject bloodParticle1;
    private float damping = 5;
    public GameObject maegenPickup;
    public int maxRandomDropChance;

    [Header("Components")]
    NavMeshAgent navAgent;
    public Animator animator;
    public Transform closestUnit;
    public float distanceFromClosestUnit;

    [Header("Horgr")]
    public GameObject horgr;
    public float distanceFromClosestHorgr;
    public bool hasArrivedAtHorgr;
    public bool horgrSwitch;
    public bool spawnedFromBuilding;

    [Header("Audio")]
    public GameObject SFXPool;
    public int soundPoolCurrent;
    public AudioSource[] soundPool;
    public AudioSource audioSource;

    private void Awake()
    {
        soundPool = SFXPool.GetComponents<AudioSource>();
        state = EnemyState.Attack;
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        Setup();
    }
    void Start()
    {
        _EM.enemies.Add(gameObject);
        homeTree = GameObject.FindGameObjectWithTag("Home Tree");
        horgr = GameObject.FindGameObjectWithTag("HorgrRally");
        speed = navAgent.speed;
        StartCoroutine(Tick());
    }

    IEnumerator Tick()
    {
        closestUnit = GetClosestUnit();

        if (UnitSelection.Instance.unitList.Count != 0)
        {
            distanceFromClosestUnit = Vector3.Distance(closestUnit.transform.position, transform.position);
        }
        distanceFromClosestHorgr = Vector3.Distance(horgr.transform.position, transform.position);



        switch (state)
        {
            case EnemyState.Work:
                navAgent.SetDestination(transform.position);

                break;

            case EnemyState.Attack:
                animator.SetBool("hasStoppedHorgr", false);
                horgrSwitch = false;
                if (UnitSelection.Instance.unitList.Count == 0)
                {
                    navAgent.stoppingDistance = 8;
                    FindHomeTree();
                    SmoothFocusOnEnemy();
                }
                else
                {
                    FindUnit();
                    if (distanceFromClosestUnit < 30)
                    {
                        SmoothFocusOnEnemy();
                    }
                    if (closestUnit.tag == "LeshyUnit")
                    {
                        navAgent.stoppingDistance = 6;
                    }
                    else
                    {
                        navAgent.stoppingDistance = stoppingDistance;
                    }
                }
                if (distanceFromClosestHorgr < distanceFromClosestUnit && !spawnedFromBuilding)
                {
                    state = EnemyState.Horgr;
                }
                if (distanceFromClosestHorgr >= distanceFromClosestUnit)
                {
                    state = EnemyState.Attack;
                }
                break;
            case EnemyState.Flee:

                break;
            case EnemyState.Beacon:
                if (!hasArrivedAtBeacon)
                    navAgent.SetDestination(fyreBeacon.transform.position);
                else
                    navAgent.SetDestination(transform.position);
                break;
            case EnemyState.Horgr:
                if (!hasArrivedAtHorgr)
                    navAgent.SetDestination(horgr.transform.position);
                else
                {
                    if (_HM.units.Count > 0)
                    {
                        animator.SetBool("hasStoppedHorgr", false);
                        state = EnemyState.Attack;
                        horgrSwitch = false;
                    }
                    if (_HM.units.Count == 0)
                    {
                        if (horgrSwitch == false)
                        {
                            animator.SetBool("hasStoppedHorgr", true);
                            navAgent.SetDestination(transform.position);
                            horgrSwitch = true;
                            print("Setting Destination");
                        }

                    }
                }
                break;
        }
        if (navAgent.velocity != Vector3.zero || distanceFromClosestUnit >= 10)
        {
            animator.SetBool("hasStopped", false);
        }
        if (navAgent.velocity == Vector3.zero || distanceFromClosestUnit < 10)
        {
            animator.SetBool("hasStopped", true);
        }
        yield return new WaitForSeconds(seconds);
        StartCoroutine(Tick());
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case EnemyState.Attack:
                if (UnitSelection.Instance.unitList.Count == 0)
                {
                    SmoothFocusOnEnemy();
                }
                else
                {
                    if (distanceFromClosestUnit < 30)
                    {
                        SmoothFocusOnEnemy();
                    }
                }
                break;
        }   
    }
    private void SmoothFocusOnEnemy()
    {
        if (UnitSelection.Instance.unitList.Count == 0)
        {
            var targetRotation = Quaternion.LookRotation(homeTree.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
        }
        else
        {
            var targetRotation = Quaternion.LookRotation(closestUnit.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
        }
    }
    private void Setup()
    {
        switch (type)
        {
            case WarriorType.Dreng:
                navAgent.speed = 9;
                health = _GM.drengHealth;
                maxHealth = _GM.drengHealth;
                break;

            case WarriorType.Berserkr:
                navAgent.speed = 20;
                health = _GM.beserkrHealth;
                maxHealth = _GM.beserkrHealth;
                break;

            case WarriorType.Knight:
                navAgent.speed = 12;
                health = _GM.knightHealth;
                maxHealth = _GM.knightHealth;
                break;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerWeapon")
        {
            TakeDamage(_GM.satyrDamage);
        }
        if (other.tag == "PlayerWeapon2")
        {
            TakeDamage(_GM.orcusDamage);
        }
        if (other.tag == "PlayerWeapon3")
        {
            TakeDamage(_GM.leshyDamage);
        }
        if (other.tag == "PlayerWeapon4")
        {
            TakeDamage(_GM.skessaDamage);
        }
        if (other.tag == "PlayerWeapon5")
        {
            TakeDamage(_GM.goblinDamage);
        }
        if (other.tag == "PlayerWeapon6")
        {
            TakeDamage(_GM.golemDamage);
        }
        if (other.tag == "Spit")
        {
            TakeDamage(_GM.spitDamage);
        }
        if (other.tag == "SpitExplosion")
        {
            TakeDamage(_GM.spitExplosionDamage);
        }
        if (other.tag == "Beacon")
        {
            animator.SetTrigger("Cheer" + RandomCheerAnim());
            hasArrivedAtBeacon = true;
        }
        if(other.tag == "Horgr")
        {
            if(!_HM.enemies.Contains(gameObject) && spawnedFromBuilding == false)
            {
                _HM.enemies.Add(gameObject);
                StartCoroutine(WaitForHorgr());
            }
        }
        if (other.tag == "Explosion")
        {
            TakeDamage(50);
            animator.SetTrigger("Impact");
            hasArrivedAtBeacon = false;
            state = EnemyState.Attack;
        }
        if (other.tag == "Explosion2")
        {
            TakeDamage(100);
            animator.SetTrigger("Impact");
            hasArrivedAtBeacon = false;
            state = EnemyState.Attack;
        }
        if (other.tag == "Spit")
        {
            navAgent.speed = speed / 2;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Horgr")
        {
            if(spawnedFromBuilding == false)
            {
                _HM.enemies.Remove(gameObject);
                hasArrivedAtHorgr = false;
            }

        }
        if (other.tag == "Spit")
        {
            navAgent.speed = speed;
        }
    }
    
    //private void OnTriggerStay(Collider other)
    //{
    //    if(other.tag == "Spit")
    //    {
    //        TakeDamage(_GM.spitDamage * Time.deltaTime);
    //    }
    //}

    IEnumerator WaitForHorgr()
    {
        Debug.Log("Horgr coroutine");
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("hasStoppedHorgr", true);
        hasArrivedAtHorgr = true;
    }
    private int RandomCheerAnim()
    {
        int rnd = Random.Range(1, 3);
        return rnd;
    }
    public void TakeDamage(float damage)
    {
        state = EnemyState.Attack;
        audioSource.clip = _SM.GetGruntSounds();
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
        GameObject bloodParticle;
        bloodParticle = Instantiate(bloodParticle1, transform.position + new Vector3(0, 5, 0), transform.rotation);
        health -= damage;
        Die();
    }
    private void Die()
    {
        bool isColliding = false;
        if (health <= 0)
        {
            _EM.enemies.Remove(gameObject);
            if (_HM.enemies.Contains(gameObject))
            {
                _HM.enemies.Remove(gameObject);
            }
            DropMaegen();
            if (!isColliding)
            {
                isColliding = true;
                GameObject go;
                go = Instantiate(deathObject, transform.position, transform.rotation);
                Destroy(go, 15);
            }
            GameEvents.ReportOnEnemyKilled();
            Destroy(gameObject);
        }
    }
    private void DropMaegen()
    {
        int rnd = Random.Range(1, maxRandomDropChance);
        if (rnd == 1)
        {
            Instantiate(maegenPickup, transform.position, transform.rotation);
        }
    }
    public void Launch()
    {
        float thrust = 20000f;
        GameObject go;
        go = Instantiate(deathObject, transform.position, transform.rotation);
        go.GetComponentInChildren<Rigidbody>().AddForce(transform.up * thrust);
        go.GetComponentInChildren<Rigidbody>().AddForce(transform.forward * -thrust);
        Destroy(go, 25);
        Destroy(gameObject);
    }
    public void PlayFootstepSound()
    {
        PlaySound(_SM.GetHumanFootstepSound());
    }
    public void PlayKnightFootstepSound()
    {
        PlaySound(_SM.GetKnightFootstepSound());
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
    public void FindUnit()
    {
        if (UnitSelection.Instance.unitList.Count == 0)
        {
            state = EnemyState.Work;
        }
        navAgent.SetDestination(closestUnit.transform.position);
    }

    public void FindHomeTree()
    {
        navAgent.SetDestination(homeTree.transform.position);
    }

    public Transform GetClosestUnit()
    {
        float closestDistance = Mathf.Infinity;
        Transform trans = null;

        foreach (GameObject go in UnitSelection.Instance.unitList)
        {
            float currentDistance;
            currentDistance = Vector3.Distance(transform.position, go.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                trans = go.transform;
            }
        }

        if (UnitSelection.Instance.unitList == null)
        {
            return null;
        }
        else
            return trans;
    }

    //private void OnBeaconPlaced()
    //{
    //    fyreBeacon = GameObject.FindGameObjectWithTag("Beacon");
    //    state = EnemyState.Beacon;
    //}
    private void OnBeaconDestroyed()
    {
        state = EnemyState.Attack;
    }

    private void OnArrivedAtHorgr()
    {
        state = EnemyState.Attack;
    }
    private void OnEnable()
    {
        //GameEvents.OnBeaconPlaced += OnBeaconPlaced;
        GameEvents.OnBeaconDestroyed += OnBeaconDestroyed;
        GameEvents.OnUnitArrivedAtHorgr += OnArrivedAtHorgr;
    }

    private void OnDisable()
    {
        //GameEvents.OnBeaconPlaced -= OnBeaconPlaced;
        GameEvents.OnBeaconDestroyed -= OnBeaconDestroyed;
        GameEvents.OnUnitArrivedAtHorgr -= OnArrivedAtHorgr;
    }
}
