using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Logger : GameBehaviour
{
    [Header("Woodcutter Type")]
    public WoodcutterType woodcutterType;

    [Header("Stats")]
    public float health;
    public float maxHealth;
    public float loggerStoppingDistance;
    public bool hasStopped = false;

    [Header("AI")]
    public EnemyState state;
    private NavMeshAgent navAgent;
    public Animator animator;
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

    [Header("Death Objects")]
    public GameObject bloodParticle1;
    public GameObject deadLogger;
    public GameObject deadLoggerFire;
    public GameObject maegenPickup;
    public int maxRandomDropChance;

    [Header("Audio")]
    public GameObject SFXPool;
    public int soundPoolCurrent;
    public AudioSource[] soundPool;
    public AudioSource audioSource;
    private AudioClip audioClip;

    private void Awake()
    {
        soundPool = SFXPool.GetComponents<AudioSource>();
        navAgent = GetComponent<NavMeshAgent>();
        Setup();
    }
    void Start()
    {
        state = EnemyState.Work;
        homeTree = GameObject.FindGameObjectWithTag("Home Tree");
    }

 
    void Update()
    {


        if (UnitSelection.Instance.unitList.Count != 0)
        {
            closestUnit = GetClosestUnit();
            distanceFromClosestUnit = Vector3.Distance(closestUnit.transform.position, transform.position);
        }
        if(_GM.trees.Length != 0)
        {
            closestTree = GetClosestTree();
            distanceFromClosestTree = Vector3.Distance(closestTree.transform.position, transform.position);
        }
        //else
        //{
        //    closestTree = homeTree;
        //}

        if (UnitSelection.Instance.unitList.Count == 0)
        {
            state = EnemyState.Work;
        }

        switch (state)
        {
            case EnemyState.Work:

                if (distanceFromClosestUnit < 30)
                {
                    state = EnemyState.Attack;
                }

                if (_GM.trees.Length == 0)
                {
                    if(woodcutterType != WoodcutterType.LogCutter)
                    {
                        navAgent.stoppingDistance = 7;
                    }

                    //SmoothFocusOnTree();
                    FindHomeTree();
                }
                else
                {
                    navAgent.stoppingDistance = loggerStoppingDistance;
                    if (distanceFromClosestTree < 30)
                    {
                        SmoothFocusOnTree();
                    }
                    FindTree();
                }

                break;

            case EnemyState.Attack:
                if (distanceFromClosestUnit >= 30)
                {
                    state = EnemyState.Work;
                }
                FindUnit();
                SmoothFocusOnEnemy();
                if (closestUnit.tag == "LeshyUnit")
                {
                    if(woodcutterType != WoodcutterType.LogCutter)
                        navAgent.stoppingDistance = 6;
                    
                }
                else
                {
                    navAgent.stoppingDistance = loggerStoppingDistance;
                }
                break;
            case EnemyState.Beacon:
                if (!hasArrivedAtBeacon)
                    navAgent.SetDestination(fyreBeacon.transform.position);
                else
                    navAgent.SetDestination(transform.position);

                break;
        }

        if (navAgent.velocity != Vector3.zero)
        {
            animator.SetBool("hasStopped", false);
            hasStopped = false;
        }
        if (navAgent.velocity == Vector3.zero)
        {
            animator.SetBool("hasStopped", true);
            hasStopped = true;
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerWeapon")
        {
            TakeDamage(_GM.satyrDamage);
        }
        if (other.tag == "PlayerWeapon2")
        {
            TakeDamage(_GM.orcusDamage);
        }
        if (other.tag == "PlayerWeapon3")
        {
            Launch();
        }
        if (other.tag == "PlayerWeapon4")
        {
            TakeDamage(_GM.skessaDamage);
        }
        if (other.tag == "Beacon")
        {
            if(woodcutterType != WoodcutterType.LogCutter)
            {
                animator.SetTrigger("Cheer" + RandomCheerAnim());
                hasArrivedAtBeacon = true;
            }
        }
        if(other.tag == "Explosion" || other.tag == "Explosion2")
        {
            GameObject go;
            go = Instantiate(deadLoggerFire, transform.position, transform.rotation);
            go.GetComponentInChildren<Rigidbody>().AddForce(transform.up * 2000);
            go.GetComponentInChildren<Rigidbody>().AddForce(transform.forward * -16000);
            Destroy(go, 15);
            Destroy(gameObject);
        }

    }

    private int RandomCheerAnim()
    {
        int rnd = Random.Range(1, 3);
        return rnd;
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
    private void SmoothFocusOnEnemy()
    {
        var targetRotation = Quaternion.LookRotation(closestUnit.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
    }
    private void SmoothFocusOnTree()
    {

        if (_GM.trees.Length == 0)
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

    public void TakeDamage(float damage)
    {
        if(woodcutterType != WoodcutterType.LogCutter)
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
        Vector3 forward = new Vector3(0, 180, 0);

        GameObject bloodParticle;
        bloodParticle = Instantiate(bloodParticle1, transform.position, Quaternion.LookRotation(forward));
        health -= damage;
        Die();
    }

    private void Die()
    {
        if(health <= 0)
        {
            DropMaegen();
            GameObject go;
            go = Instantiate(deadLogger, transform.position, transform.rotation);
            Destroy(go, 15);
            GameEvents.ReportOnEnemyKilled();
            Destroy(gameObject);
        }
    }
    private void DropMaegen()
    {
        int rnd;
        if (_TUTM.isTutorial && _TUTM.tutorialStage == 8)
        {
            rnd = 1;
        }
        else
        {
            rnd = Random.Range(1, maxRandomDropChance);
        }
        if (rnd == 1)
        {
            Instantiate(maegenPickup, transform.position, transform.rotation);
        }
    }
    public void Launch()
    {
        DropMaegen();
        if (_HUTM.enemies.Contains(gameObject))
        {
            _HUTM.enemies.Remove(gameObject);
        }
        float thrust = 20000f;
        GameObject go;
        go = Instantiate(deadLogger, transform.position, transform.rotation);
        go.GetComponentInChildren<Rigidbody>().AddForce(transform.up * thrust);
        go.GetComponentInChildren<Rigidbody>().AddForce(transform.forward * -thrust);
        go.GetComponent<RagdollSound>().hasBeenLaunched = true;
        Destroy(go, 25);
        GameEvents.ReportOnEnemyKilled();
        Destroy(gameObject);
    }

    private void Setup()
    {
        switch (woodcutterType)
        {
            case WoodcutterType.Logger:
                
                navAgent.speed = 5;
                health = 100;
                maxHealth = 100;
                break;

            case WoodcutterType.Lumberjack:
                navAgent.speed = 6;
                health = 180;
                maxHealth = 180;
                break;
            case WoodcutterType.LogCutter:
                navAgent.speed = 3;
                navAgent.stoppingDistance = 10;
                health = 250;
                maxHealth = 250;
                break;
        }
    }

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
        
        if(UnitSelection.Instance.unitList == null)
        {
            return null;
        }
        else
        return trans;
    }
    //public void EnableCollider()
    //{
    //    axeObject.GetComponent<Collider>().enabled = true;
    //}
    //public void DisableCollider()
    //{
    //    axeObject.GetComponent<Collider>().enabled = false;
    //}
    public void FindTree()
    {
        navAgent.SetDestination(closestTree.transform.position);
    }
    public void FindUnit()
    {
        if(UnitSelection.Instance.unitList.Count == 0)
        {
            state = EnemyState.Work;
        }
        navAgent.SetDestination(closestUnit.transform.position);
    }
    public void FindHomeTree()
    {
        navAgent.SetDestination(homeTree.transform.position);
    }
    //private void OnBeaconPlaced()
    //{
    //    if(woodcutterType != WoodcutterType.LogCutter)
    //    {
    //        fyreBeacon = GameObject.FindGameObjectWithTag("Beacon");
    //        state = EnemyState.Beacon;
    //    }
    //}
    private void OnBeaconDestroyed()
    {
        if (woodcutterType != WoodcutterType.LogCutter)
        {
            state = EnemyState.Work;
        }
    }
    private void OnEnable()
    {
        //GameEvents.OnBeaconPlaced += OnBeaconPlaced;
        GameEvents.OnBeaconDestroyed += OnBeaconDestroyed;
    }

    private void OnDisable()
    {
        //GameEvents.OnBeaconPlaced -= OnBeaconPlaced;
        GameEvents.OnBeaconDestroyed -= OnBeaconDestroyed;
    }
}
