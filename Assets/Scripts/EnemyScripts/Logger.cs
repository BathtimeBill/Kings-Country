using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Analytics;

public class Logger : Enemy
{
    public bool invincible = true;
    [Header("Woodcutter Type")]
    public WoodcutterType woodcutterType;
    [Header("Tick")]
    public float seconds = 0.5f;
    [Header("Stats")]
    public float loggerStoppingDistance;
    public bool hasStopped = false;

    [Header("AI")]
    public EnemyState state;
    public Transform closestUnit;
    public float distanceFromClosestUnit;

    [Header("Beacon")]
    public GameObject fyreBeacon;
    public bool hasArrivedAtBeacon;


    [Header("Trees")]
    public Transform closestTree;
    public GameObject axeObject;
    public float distanceFromClosestTree;
    public GameObject homeTree;

    [Header("Audio")]
    public GameObject SFXPool;
    public int soundPoolCurrent;
    public AudioSource[] soundPool;
    public AudioSource audioSource;
    private AudioClip audioClip;

    #region Startup
    public override void Awake()
    {
        base.Awake();
        soundPool = SFXPool.GetComponents<AudioSource>();
        
    }
    public override void Start()
    {
        base.Start();
        state = EnemyState.Work;
        homeTree = GameObject.FindGameObjectWithTag("Home Tree");
        if (_GM.gameState == GameState.Lose)
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

        if (_UM.unitList.Count != 0)
        {
            closestUnit = GetClosestUnit();
            distanceFromClosestUnit = Vector3.Distance(closestUnit.transform.position, transform.position);
        }
        if (_GM.trees.Count != 0)
        {
            closestTree = GetClosestTree();
            distanceFromClosestTree = Vector3.Distance(closestTree.transform.position, transform.position);
        }

        //if (UnitSelection.Instance.unitList.Count == 0)
        //{
        //    state = EnemyState.Work;
        //}

        switch (state)
        {

            case EnemyState.Work:

                if (distanceFromClosestUnit < 30)
                {
                    state = EnemyState.Attack;
                }

                if (_GM.trees.Count == 0)
                {
                    if (woodcutterType != WoodcutterType.LogCutter)
                    {
                        agent.stoppingDistance = 7;
                    }

                    //SmoothFocusOnTree();
                    FindHomeTree();
                }
                else
                {
                    agent.stoppingDistance = loggerStoppingDistance;
                    if (distanceFromClosestTree < 30)
                    {
                        SmoothFocusOnTree();
                    }
                    FindTree();
                }

                break;

            case EnemyState.Attack:
                if(_UM.unitList.Count > 0)
                {
                    if (distanceFromClosestUnit >= 30)
                    {
                        state = EnemyState.Work;
                    }
                    FindUnit();
                    SmoothFocusOnEnemy();
                }
                //else
                //{
                //    state = EnemyState.Cheer;
                //}

                break;
            case EnemyState.Beacon:
                if (!hasArrivedAtBeacon)
                    agent.SetDestination(fyreBeacon.transform.position);
                else
                    agent.SetDestination(transform.position);

                break;
            case EnemyState.Cheer:
                agent.SetDestination(transform.position);

                break;
        }

        if (agent.velocity != Vector3.zero)
        {
            animator.SetBool("hasStopped", false);
            hasStopped = false;
        }
        if (agent.velocity == Vector3.zero)
        {
            animator.SetBool("hasStopped", true);
            hasStopped = true;
        }
        yield return new WaitForSeconds(seconds);
        StartCoroutine(Tick());
    }
    #endregion

    #region Damage

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        /*
        if (other.tag == "Spit")
        {
            TakeDamage(_GM.spitDamage);
        }
        if (other.tag == "SpitExplosion")
        {
            TakeDamage(_GM.spitExplosionDamage);
        }*/
        if (other.tag == "Beacon")
        {
            if(woodcutterType != WoodcutterType.LogCutter)
            {
                animator.SetTrigger("Cheer" + RandomCheerAnim());
                hasArrivedAtBeacon = true;
            }
        }
        if (other.tag == "Hand") //TODO what is the hand?
        {
            Die(this, "Hand", DeathID.Regular);
        }
    }
    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
    private int RandomCheerAnim()
    {
        int rnd = Random.Range(1, 3);
        return rnd;
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
    private void SmoothFocusOnEnemy()
    {
        var targetRotation = Quaternion.LookRotation(closestUnit.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
    }
    private void SmoothFocusOnTree()
    {

        if (_GM.trees.Count == 0)
        {
            var targetRotation = Quaternion.LookRotation(homeTree.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
        }
        else
        {
            var targetRotation = Quaternion.LookRotation(closestTree.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
        }
    }

    public override void TakeDamage(int damage, string _damagedBy)
    {
        if(!invincible)
        {
            if (woodcutterType != WoodcutterType.LogCutter)
            {
                audioSource.clip = _SM.GetGruntSounds();
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.Play();
            }
            else
            {
                audioSource.clip = _SM.GetChopSounds();
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.Play();
            }
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
    public override void DropMaegen()
    {
        int rnd;
        if (_TUTORIAL.isTutorial && _TUTORIAL.tutorialStage == 8)
        {
            rnd = 1;
        }
        else
        {
            rnd = Random.Range(1, maxRandomDropChance);
        }
        if (rnd == 1)
        {
            Instantiate(_SETTINGS.general.maegenPickup, transform.position, transform.rotation);
        }
    }
    //public override void Launch()
    //{
    //    base.Launch();
    //}

    

    public Transform GetClosestTree()
    {
        
        float closestDistance = Mathf.Infinity;
        Transform trans = null;

        foreach (GameObject go in _GM.trees)
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

    public void FindTree()
    {
        agent.SetDestination(closestTree.transform.position);
    }
    public void FindUnit()
    {
        if(_UM.unitList.Count == 0 && _GM.trees.Count > 0)
        {
            state = EnemyState.Work;
        }
        agent.SetDestination(closestUnit.transform.position);
    }
    public void FindHomeTree()
    {
        Log("Finding Home Tree");
        agent.SetDestination(homeTree.transform.position);
    }
    private void OnGameOver()
    {
        StopCoroutine(Tick());
        state = EnemyState.Cheer;
        animator.SetTrigger("Cheer" + RandomCheerAnim());
        agent.SetDestination(transform.position);
    }
    private void OnEnable()
    {
        GameEvents.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= OnGameOver;
    }
}
