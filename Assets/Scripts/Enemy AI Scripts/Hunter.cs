using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Hunter : GameBehaviour
{
    public bool invincible = true;
    [Header("Hunter Type")]
    public HunterType type;
    [Header("Tick")]
    public float seconds = 0.5f;
    [Header("Stats")]
    public float health;
    public float maxHealth;
    public EnemyState state;
    public bool hasArrivedAtBeacon;
    public GameObject fyreBeacon;
    public GameObject deadHunterFire;
    [Header("Death Objects")]
    public GameObject deathObject;
    public GameObject bloodParticle1;
    private float damping = 5;
    public float stoppingDistance;
    public GameObject maegenPickup;
    public int maxRandomDropChance;
    public float speed;


    [Header("Components")]
    NavMeshAgent navAgent;
    public Animator animator;
    public GameObject[] wildlife;
    public Transform closestWildlife;
    public float distanceFromClosestWildlife;
    public Transform closestUnit;
    public float distanceFromClosestUnit;
    public float range;

    [Header("Hut")]
    public GameObject horgr;
    public float distanceFromClosestHorgr;
    public bool hasArrivedAtHorgr;
    public bool hutSwitch;
    public bool spawnedFromBuilding;

    [Header("Audio")]
    public GameObject SFXPool;
    public int soundPoolCurrent;
    public AudioSource[] soundPool;
    public AudioSource audioSource;


    private void Awake()
    {
        soundPool = SFXPool.GetComponents<AudioSource>();
        state = EnemyState.Work;
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        Setup();
    }

    private void Start()
    {
        _EM.enemies.Add(gameObject);
        horgr = GameObject.FindGameObjectWithTag("HutRally");
        speed = navAgent.speed;
        StartCoroutine(Tick());
        StartCoroutine(WaitForInvincible());
    }
    IEnumerator Tick()
    {//Tracks the closest animal and player unit.
        wildlife = GameObject.FindGameObjectsWithTag("Wildlife");
        closestUnit = GetClosestUnit();
        closestWildlife = GetClosestWildlife();
        distanceFromClosestHorgr = Vector3.Distance(horgr.transform.position, transform.position);


        if (UnitSelection.Instance.unitList.Count != 0)
        {
            distanceFromClosestUnit = Vector3.Distance(closestUnit.transform.position, transform.position);
        }
        else
        {
            distanceFromClosestUnit = 200000;
        }
        if (wildlife.Length > 0)
        {
            distanceFromClosestWildlife = Vector3.Distance(closestWildlife.transform.position, transform.position);
        }
        else
        {
            distanceFromClosestWildlife = 200000;
        }


        if (wildlife.Length == 0 || UnitSelection.Instance.unitList.Count == 0)
        {

            animator.SetBool("allWildlifeDead", true);
        }
        if (wildlife.Length > 0 || UnitSelection.Instance.unitList.Count > 0)
        {

            animator.SetBool("allWildlifeDead", false);
        }
        if (distanceFromClosestUnit < range && UnitSelection.Instance.unitList.Count != 0)
        {
            state = EnemyState.Attack;
        }

        switch (state)
        {
            case EnemyState.Work:
                Hunt();

                break;

            case EnemyState.Attack:
                hutSwitch = false;
                if (UnitSelection.Instance.unitList.Count == 0 || distanceFromClosestUnit > range)
                {
                    state = EnemyState.Work;
                }
                Attack();
                break;

            case EnemyState.Beacon:
                navAgent.stoppingDistance = 0;
                if (!hasArrivedAtBeacon)
                    navAgent.SetDestination(fyreBeacon.transform.position);
                else
                    navAgent.SetDestination(transform.position);
                break;

            case EnemyState.Horgr:
                if (!hasArrivedAtHorgr)
                {
                    navAgent.SetDestination(horgr.transform.position);
                    navAgent.stoppingDistance = range / 2;
                }
                else
                {
                    navAgent.stoppingDistance = range;
                    if (_HUTM.units.Count > 0)
                    {
                        animator.SetBool("hasStoppedHorgr", false);
                        Attack();
                    }
                    if (_HUTM.units.Count == 0)
                    {
                        if (hutSwitch == false)
                        {
                            animator.SetBool("hasStoppedHorgr", true);
                            navAgent.SetDestination(transform.position);
                            hutSwitch = true;
                        }

                    }
                }
                break;
        }

        if (distanceFromClosestHorgr > distanceFromClosestWildlife)
        {
            state = EnemyState.Work;
        }
        if (distanceFromClosestHorgr <= distanceFromClosestWildlife && !spawnedFromBuilding)
        {
            state = EnemyState.Horgr;
        }

        if (navAgent.velocity != Vector3.zero || distanceFromClosestUnit >= 10)
        {
            animator.SetBool("hasStopped", true);
        }
        if (navAgent.velocity == Vector3.zero || distanceFromClosestUnit < 10)
        {
            animator.SetBool("hasStopped", false);
        }
        yield return new WaitForSeconds(seconds);
        StartCoroutine(Tick());
    }
    //void Update()
    //{
    //    //Tracks the closest animal and player unit.
    //    wildlife = GameObject.FindGameObjectsWithTag("Wildlife");
    //    closestUnit = GetClosestUnit();
    //    closestWildlife = GetClosestWildlife();
    //    distanceFromClosestHorgr = Vector3.Distance(horgr.transform.position, transform.position);


    //    if (UnitSelection.Instance.unitList.Count != 0)
    //    {
    //        distanceFromClosestUnit = Vector3.Distance(closestUnit.transform.position, transform.position);
    //    }
    //    else
    //    {
    //        distanceFromClosestUnit = 25;
    //    }
    //    if (wildlife.Length > 0)
    //    {
    //        distanceFromClosestWildlife = Vector3.Distance(closestWildlife.transform.position, transform.position);
    //    }
    //    else
    //    {
    //        distanceFromClosestWildlife = 50;
    //    }


    //    if (wildlife.Length == 0 || UnitSelection.Instance.unitList.Count == 0)
    //    {

    //        animator.SetBool("allWildlifeDead", true);
    //    }
    //    if (wildlife.Length > 0 || UnitSelection.Instance.unitList.Count > 0)
    //    {

    //        animator.SetBool("allWildlifeDead", false);
    //    }
    //    if (distanceFromClosestUnit < 30 && UnitSelection.Instance.unitList.Count != 0)
    //    {
    //        state = EnemyState.Attack;
    //    }

    //    switch (state)
    //    {
    //        case EnemyState.Work:
    //            Hunt();

    //            break;

    //        case EnemyState.Attack:
    //            hutSwitch = false;
    //            if (UnitSelection.Instance.unitList.Count == 0 || distanceFromClosestUnit > 30)
    //            {
    //                state = EnemyState.Work;
    //            }
    //            Attack();
    //            break;

    //        case EnemyState.Beacon:
    //            navAgent.stoppingDistance = 0;
    //            if (!hasArrivedAtBeacon)
    //                navAgent.SetDestination(fyreBeacon.transform.position);
    //            else
    //                navAgent.SetDestination(transform.position);
    //            break;

    //        case EnemyState.Horgr:
    //            if (!hasArrivedAtHorgr)
    //                navAgent.SetDestination(horgr.transform.position);
    //            else
    //            {
    //                if (_HUTM.units.Count > 0)
    //                {
    //                    animator.SetBool("hasStoppedHorgr", false);
    //                    Attack();
    //                }
    //                if (_HUTM.units.Count == 0)
    //                {
    //                    if(hutSwitch==false)
    //                    {
    //                        animator.SetBool("hasStoppedHorgr", true);
    //                        navAgent.SetDestination(transform.position);
    //                        hutSwitch = true;
    //                    }

    //                }
    //            }
    //            break;
    //    }

    //    if (distanceFromClosestHorgr > distanceFromClosestWildlife)
    //    {
    //        state = EnemyState.Work;
    //    }
    //    if (distanceFromClosestHorgr <= distanceFromClosestWildlife && !spawnedFromBuilding)
    //    {
    //        state = EnemyState.Horgr;
    //    }

    //    if (navAgent.velocity != Vector3.zero)
    //    {
    //        animator.SetBool("hasStopped", true);
    //    }
    //    if (navAgent.velocity == Vector3.zero)
    //    {
    //        animator.SetBool("hasStopped", false);
    //    }
    //}
    private void FixedUpdate()
    {
        if(distanceFromClosestUnit < range)
        {
            SmoothFocusOnEnemy();
        }
    }
    private void SmoothFocusOnEnemy()
    {
        if(closestUnit != null)
        {
            var targetRotation = Quaternion.LookRotation(closestUnit.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2 * Time.deltaTime);
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
            if(type != HunterType.Bjornjeger)
            {
                Launch();
            }
            else
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
        if (other.tag == "Hut")
        {
            if (!_HUTM.enemies.Contains(gameObject) && spawnedFromBuilding == false)
            {
                _HUTM.enemies.Add(gameObject);
                StartCoroutine(WaitForHorgr());
            }

        }
        if (other.tag == "Explosion" || other.tag == "Explosion2")
        {
            if (_HUTM.enemies.Contains(gameObject))
            {
                _HUTM.enemies.Remove(gameObject);
            }
            _EM.enemies.Remove(gameObject);
            GameObject go;
            go = Instantiate(deadHunterFire, transform.position, transform.rotation);
            go.GetComponentInChildren<Rigidbody>().AddForce(transform.up * 2000);
            go.GetComponentInChildren<Rigidbody>().AddForce(transform.forward * -16000);
            Destroy(go, 15);

            Destroy(gameObject);
        }
        if (other.tag == "Spit")
        {
            navAgent.speed = speed / 2;
        }
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.tag == "Spit")
    //    {
    //        TakeDamage(_GM.spitDamage * Time.deltaTime);
    //    }
    //}
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hut")
        {
            if(spawnedFromBuilding == false)
            {
                _HUTM.enemies.Remove(gameObject);
                state = EnemyState.Attack;
                hasArrivedAtHorgr = false;
            }

        }
        if (other.tag == "Spit")
        {
            navAgent.speed = speed;
        }
    }
    IEnumerator WaitForHorgr()
    {
        Debug.Log("Hut coroutine");
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
        if(!invincible)
        {
            audioSource.clip = _SM.GetGruntSounds();
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.Play();
            GameObject bloodParticle;
            bloodParticle = Instantiate(bloodParticle1, transform.position + new Vector3(0, 5, 0), transform.rotation);
            health -= damage;
            Die();
        }
    }
    IEnumerator WaitForInvincible()
    {
        invincible = true;
        yield return new WaitForSeconds(5);
        invincible = false;
    }
    private void Die()
    {
        bool isColliding = false;
        if (health <= 0)
        {
            _EM.enemies.Remove(gameObject);
            if (_HUTM.enemies.Contains(gameObject))
            {
                _HUTM.enemies.Remove(gameObject);
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
        if(rnd == 1)
        {
            Instantiate(maegenPickup, transform.position, transform.rotation);
        }
    }

    public void Launch()
    {
        bool isColliding = false;
        if (_HUTM.enemies.Contains(gameObject))
        {
            _HUTM.enemies.Remove(gameObject);
        }
        _EM.enemies.Remove(gameObject);
        DropMaegen();
        float thrust = 20000f;
        if(!isColliding)
        {
            isColliding = true;
            GameObject go;
            go = Instantiate(deathObject, transform.position, transform.rotation);
            go.GetComponentInChildren<Rigidbody>().AddForce(transform.up * thrust);
            go.GetComponentInChildren<Rigidbody>().AddForce(transform.forward * -thrust);
            go.GetComponent<RagdollSound>().hasBeenLaunched = true;
            Destroy(go, 25);
        }

        GameEvents.ReportOnEnemyKilled();
        Destroy(gameObject);
    }
    private void Setup()
    {
        navAgent.stoppingDistance = range;
        switch (type)
        {
            case HunterType.Wathe:
                navAgent.speed = 9;
                health = _GM.watheHealth;
                maxHealth = _GM.watheHealth;
                break;

            case HunterType.Hunter:
                navAgent.speed = 11;
                health = _GM.hunterHealth;
                maxHealth = _GM.hunterHealth;
                break;

            case HunterType.Bjornjeger:
                navAgent.speed = 8;
                health = _GM.bjornjeggerHealth;
                maxHealth = _GM.bjornjeggerHealth;
                break;
        }
    }

    public void PlayFootstepSound()
    {
        PlaySound(_SM.GetHumanFootstepSound());
    }
    public void PlaySound(AudioClip _clip)
    {
        if (soundPoolCurrent == soundPool.Length - 1)
            soundPoolCurrent = 0;
        else
            soundPoolCurrent += 1;

        soundPool[soundPoolCurrent].clip = _clip;
        soundPool[soundPoolCurrent].Play();
    }
    public void Hunt()
    {
        if(wildlife.Length == 0)
        {
            navAgent.SetDestination(closestUnit.transform.position);
            if (UnitSelection.Instance.unitList.Count == 0)
            {
                navAgent.SetDestination(transform.position);
            }
        }
        else
        {
            if (distanceFromClosestWildlife < range)
            {
                transform.LookAt(closestWildlife.transform.position);
            }
            navAgent.SetDestination(closestWildlife.transform.position);
        }
    }
    public void Attack()
    {
        var lookPos = closestUnit.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        navAgent.SetDestination(closestUnit.transform.position);
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
    public Transform GetClosestWildlife()
    {
        float closestDistance = Mathf.Infinity;
        Transform trans = null;

        foreach (GameObject go in wildlife)
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
    //private void OnBeaconPlaced()
    //{
    //    fyreBeacon = GameObject.FindGameObjectWithTag("Beacon");
    //    state = EnemyState.Beacon;
    //}

    private void OnArrivedAtHut()
    {
        state = EnemyState.Attack;
    }

    private void OnEnable()
    {
        GameEvents.OnUnitArrivedAtHut += OnArrivedAtHut;
    }

    private void OnDisable()
    {
        GameEvents.OnUnitArrivedAtHut -= OnArrivedAtHut;
    }
}
