using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Analytics;

public class Warrior : Enemy
{
    public bool invincible = true;
    [Header("Hunter Type")]
    public WarriorType type;
    [Header("Tick")]
    public float seconds = 0.5f;
    [Header("Stats")]
    public EnemyState state;
    public float stoppingDistance;
    public GameObject homeTree;
    public bool hasArrivedAtBeacon;
    public GameObject fyreBeacon;
    public GameObject deadWarriorFire;

    [Header("Death Objects")]
    private float damping = 5;

    [Header("Components")]
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

    #region Startup
    public override void Awake()
    {
        base.Awake();
        soundPool = SFXPool.GetComponents<AudioSource>();
        state = EnemyState.Attack;
    }
    public override void Start()
    {
        base.Start();
        homeTree = GameObject.FindGameObjectWithTag("Home Tree");
        horgr = GameObject.FindGameObjectWithTag("HorgrRally");
        if(_GM.gameState == GameState.Lose)
        {
            OnGameOver();
        }
        else
        {
            StartCoroutine(Tick());
        }
        StartCoroutine(WaitForInvincible());
    }
    #endregion

    #region AI
    IEnumerator Tick()
    {
        if (_GM.gameState == GameState.Lose)
        {
            StopAllCoroutines();
        }
        closestUnit = GetClosestUnit();

        if (UnitSelection.Instance.unitList.Count != 0)
        {
            distanceFromClosestUnit = Vector3.Distance(closestUnit.transform.position, transform.position);
        }
        distanceFromClosestHorgr = Vector3.Distance(horgr.transform.position, transform.position);



        switch (state)
        {
            case EnemyState.Work:
                agent.SetDestination(transform.position);

                break;

            case EnemyState.Attack:
                animator.SetBool("hasStoppedHorgr", false);
                horgrSwitch = false;
                if (UnitSelection.Instance.unitList.Count == 0)
                {
                    agent.stoppingDistance = 10;
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
                        agent.stoppingDistance = 8;
                    }
                    else
                    {
                        agent.stoppingDistance = stoppingDistance;
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
                    agent.SetDestination(fyreBeacon.transform.position);
                else
                    agent.SetDestination(transform.position);
                break;
            case EnemyState.Horgr:
                if (!hasArrivedAtHorgr)
                    agent.SetDestination(horgr.transform.position);
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
                            agent.SetDestination(transform.position);
                            horgrSwitch = true;
                            print("Setting Destination");
                        }

                    }
                }
                break;
            case EnemyState.Cheer:
                agent.SetDestination(transform.position);

                break;
        }
        if (agent.velocity != Vector3.zero || distanceFromClosestUnit >= 10)
        {
            animator.SetBool("hasStopped", false);
        }
        if (agent.velocity == Vector3.zero || distanceFromClosestUnit < 10)
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
        if(closestUnit != null)
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
        if(other.tag == "Horgr")
        {
            if(!_HM.enemies.Contains(gameObject) && spawnedFromBuilding == false)
            {
                _HM.enemies.Add(gameObject);
                StartCoroutine(WaitForHorgr());
            }
        }
    }
    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.tag == "Horgr")
        {
            if(spawnedFromBuilding == false)
            {
                _HM.enemies.Remove(gameObject);
                hasArrivedAtHorgr = false;
            }

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
    public override void TakeDamage(int damage, string _damagedBy)
    {
        if(!invincible)
        {
            state = EnemyState.Attack;
            audioSource.clip = _SM.GetGruntSounds();
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.Play();
            base.TakeDamage(damage, _damagedBy);
        }
    }
    IEnumerator WaitForInvincible()
    {
        invincible = true;
        yield return new WaitForSeconds(5);
        invincible = false;
    }
    public override void Die(Enemy _thisUnit, string _killedBy, DeathID _deathID)
    {
        base.Die(_thisUnit, _killedBy, _deathID);
    }
    #endregion

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
        agent.SetDestination(closestUnit.transform.position);
    }

    public void FindHomeTree()
    {
        agent.SetDestination(homeTree.transform.position);
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

    private void OnArrivedAtHorgr()
    {
        state = EnemyState.Attack;
    }
    private void OnGameOver()
    {
        state = EnemyState.Cheer;
        StopCoroutine(Tick());
        animator.SetTrigger("Cheer" + RandomCheerAnim());
        agent.SetDestination(transform.position);
    }
    private void OnEnable()
    {
        GameEvents.OnUnitArrivedAtHorgr += OnArrivedAtHorgr;
        GameEvents.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnUnitArrivedAtHorgr -= OnArrivedAtHorgr;
        GameEvents.OnGameOver -= OnGameOver;
    }
}
