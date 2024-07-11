using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.Analytics;

public class Hunter : Enemy
{
    public bool invincible = true;
    [Header("Hunter Type")]
    public HunterType type;
    [Header("Tick")]
    public float seconds = 0.5f;
    [Header("Stats")]
    public EnemyState state;
    public bool hasArrivedAtBeacon;
    public GameObject fyreBeacon;
    private float damping = 5;
    public float stoppingDistance;


    [Header("Components")]
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

    #region Startup
    public override void Awake()
    {
        base.Awake();
        soundPool = SFXPool.GetComponents<AudioSource>();
        state = EnemyState.Work;
    }

    public override void Start()
    {
        base.Start();
        horgr = GameObject.FindGameObjectWithTag("HutRally");
        agent.stoppingDistance = range;
        StartCoroutine(Tick());
        StartCoroutine(WaitForInvincible());
    }
    #endregion

    #region AI
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


        if (wildlife.Length == 0 && UnitSelection.Instance.unitList.Count == 0)
        {
            StopCoroutine(Tick());
            state = EnemyState.Cheer;
            animator.SetTrigger("Cheer" + RandomCheerAnim());
            agent.SetDestination(transform.position);
        }
        //if (wildlife.Length > 0 || UnitSelection.Instance.unitList.Count > 0)
        //{

        //    animator.SetBool("allWildlifeDead", false);
        //}
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
                agent.stoppingDistance = 0;
                if (!hasArrivedAtBeacon)
                    agent.SetDestination(fyreBeacon.transform.position);
                else
                    agent.SetDestination(transform.position);
                break;

            case EnemyState.Horgr:
                if (!hasArrivedAtHorgr)
                {
                    agent.SetDestination(horgr.transform.position);
                    agent.stoppingDistance = range / 2;
                }
                else
                {
                    agent.stoppingDistance = range;
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
                            agent.SetDestination(transform.position);
                            hutSwitch = true;
                        }

                    }
                }
                break;
            case EnemyState.Cheer:
                agent.SetDestination(transform.position);

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

        if (agent.velocity != Vector3.zero || distanceFromClosestUnit >= 10)
        {
            animator.SetBool("hasStopped", true);
        }
        if (agent.velocity == Vector3.zero || distanceFromClosestUnit < 10)
        {
            animator.SetBool("hasStopped", false);
        }
        yield return new WaitForSeconds(seconds);
        StartCoroutine(Tick());
    }

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

    #endregion

    #region Damage
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

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
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.tag == "Hut")
        {
            if(spawnedFromBuilding == false)
            {
                _HUTM.enemies.Remove(gameObject);
                state = EnemyState.Attack;
                hasArrivedAtHorgr = false;
            }

        }
    }

    public override void TakeDamage(int damage, string _damagedBy)
    {
        if (!invincible)
        {
            audioSource.clip = _SM.GetGruntSounds();
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.Play();
            base.TakeDamage(damage, _damagedBy);
        }
    }

    public override void Die(Enemy _thisUnit, string _killedBy, DeathID _deathID)
    {
        base.Die(_thisUnit, _killedBy, _deathID);
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
    
    IEnumerator WaitForInvincible()
    {
        invincible = true;
        yield return new WaitForSeconds(5);
        invincible = false;
    }
    #endregion

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
            agent.SetDestination(closestUnit.transform.position);
            if (UnitSelection.Instance.unitList.Count == 0)
            {
                agent.SetDestination(transform.position);
            }
        }
        else
        {
            if (distanceFromClosestWildlife < range)
            {
                transform.LookAt(closestWildlife.transform.position);
            }
            agent.SetDestination(closestWildlife.transform.position);
        }
    }
    public void Attack()
    {
        var lookPos = closestUnit.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        agent.SetDestination(closestUnit.transform.position);
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

    private void OnGameOver()
    {
        state = EnemyState.Cheer;
    }

    private void OnEnable()
    {
        GameEvents.OnUnitArrivedAtHut += OnArrivedAtHut;
        GameEvents.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnUnitArrivedAtHut -= OnArrivedAtHut;
        GameEvents.OnGameOver -= OnGameOver;
    }
}
