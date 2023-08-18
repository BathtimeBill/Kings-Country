using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Hunter : GameBehaviour
{
    [Header("Hunter Type")]
    public HunterType type;

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


    [Header("Components")]
    NavMeshAgent navAgent;
    public Animator animator;
    public GameObject[] wildlife;
    public Transform closestWildlife;
    public float distanceFromClosestWildlife;
    public Transform closestUnit;
    public float distanceFromClosestUnit;

    [Header("Hut")]
    public GameObject horgr;
    public float distanceFromClosestHorgr;
    public bool hasArrivedAtHorgr;

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
        horgr = GameObject.FindGameObjectWithTag("HutRally");
    }

    void Update()
    {
        //Tracks the closest animal and player unit.
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
            distanceFromClosestUnit = 25;
        }
        if (wildlife.Length > 0)
        {
            distanceFromClosestWildlife = Vector3.Distance(closestWildlife.transform.position, transform.position);
        }
        else
        {
            distanceFromClosestWildlife = 50;
        }


        if (wildlife.Length == 0 || UnitSelection.Instance.unitList.Count == 0)
        {

            animator.SetBool("allWildlifeDead", true);
        }
        if (wildlife.Length > 0 || UnitSelection.Instance.unitList.Count > 0)
        {

            animator.SetBool("allWildlifeDead", false);
        }
        if (distanceFromClosestUnit < 30 && UnitSelection.Instance.unitList.Count != 0)
        {
            state = EnemyState.Attack;
        }

        switch (state)
        {
            case EnemyState.Work:
                Hunt();
               
                break;

            case EnemyState.Attack:
                
                if(UnitSelection.Instance.unitList.Count == 0 || distanceFromClosestUnit > 30)
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
                    navAgent.SetDestination(horgr.transform.position);
                else
                {
                    if (_HUTM.units.Count > 0)
                    {
                        animator.SetBool("hasStoppedHorgr", false);
                        Attack();
                    }
                    if (_HUTM.units.Count == 0)
                    {
                        animator.SetBool("hasStoppedHorgr", true);
                        navAgent.SetDestination(transform.position);
                    }

                }
                break;
        }

        if (distanceFromClosestHorgr > distanceFromClosestWildlife)
        {
            state = EnemyState.Work;
        }
        if (distanceFromClosestHorgr <= distanceFromClosestWildlife)
        {
            state = EnemyState.Horgr;
        }

        if (navAgent.velocity != Vector3.zero)
        {
            animator.SetBool("hasStopped", true);
        }
        if (navAgent.velocity == Vector3.zero)
        {
            animator.SetBool("hasStopped", false);
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
        if (other.tag == "Beacon")
        {
            animator.SetTrigger("Cheer" + RandomCheerAnim());
            hasArrivedAtBeacon = true;
        }
        if (other.tag == "Hut")
        {
            if (!_HUTM.enemies.Contains(gameObject))
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
            GameObject go;
            go = Instantiate(deadHunterFire, transform.position, transform.rotation);
            go.GetComponentInChildren<Rigidbody>().AddForce(transform.up * 2000);
            go.GetComponentInChildren<Rigidbody>().AddForce(transform.forward * -16000);
            Destroy(go, 15);

            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hut")
        {
            _HUTM.enemies.Remove(gameObject);
            state = EnemyState.Attack;
            hasArrivedAtHorgr = false;
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
        audioSource.clip = _SM.GetGruntSounds();
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
        GameObject bloodParticle;
        bloodParticle = Instantiate(bloodParticle1, transform.position, transform.rotation);
        health -= damage;
        Die();
    }
    private void Die()
    {
        if (health <= 0)
        {
            if (_HUTM.enemies.Contains(gameObject))
            {
                _HUTM.enemies.Remove(gameObject);
            }
            DropMaegen();
            GameObject go;
            go = Instantiate(deathObject, transform.position, transform.rotation);
            Destroy(go, 15);
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
        if (_HUTM.enemies.Contains(gameObject))
        {
            _HUTM.enemies.Remove(gameObject);
        }

        DropMaegen();
        float thrust = 20000f;
        GameObject go;
        go = Instantiate(deathObject, transform.position, transform.rotation);
        go.GetComponentInChildren<Rigidbody>().AddForce(transform.up * thrust);
        go.GetComponentInChildren<Rigidbody>().AddForce(transform.forward * -thrust);
        go.GetComponent<RagdollSound>().hasBeenLaunched = true;
        Destroy(go, 25);
        GameEvents.ReportOnEnemyKilled();
        Destroy(gameObject);
    }
    private void Setup()
    {
        switch (type)
        {
            case HunterType.Wathe:
                navAgent.speed = 6;
                health = 80;
                maxHealth = 80;
                break;

            case HunterType.Hunter:
                navAgent.speed = 7;
                health = 150;
                maxHealth = 150;
                break;

            case HunterType.Bjornjeger:
                navAgent.speed = 4;
                health = 200;
                maxHealth = 200;
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
            if (distanceFromClosestWildlife < 30)
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
    private void OnBeaconDestroyed()
    {
        state = EnemyState.Work;
    }
    private void OnArrivedAtHut()
    {
        state = EnemyState.Attack;
    }

    private void OnEnable()
    {
        //GameEvents.OnBeaconPlaced += OnBeaconPlaced;
        GameEvents.OnBeaconDestroyed += OnBeaconDestroyed;
        GameEvents.OnUnitArrivedAtHut += OnArrivedAtHut;
    }

    private void OnDisable()
    {
        //GameEvents.OnBeaconPlaced -= OnBeaconPlaced;
        GameEvents.OnBeaconDestroyed -= OnBeaconDestroyed;
        GameEvents.OnUnitArrivedAtHut -= OnArrivedAtHut;
    }
}
