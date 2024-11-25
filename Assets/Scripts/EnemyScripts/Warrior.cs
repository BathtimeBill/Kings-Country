using System.Collections;
using UnityEngine;

public class Warrior : Enemy
{
    [Header("Tick")]
    public float seconds = 0.5f;
    [Header("Stats")]
    public EnemyState state;
    public float stoppingDistance;

    [Header("Components")]
    public Transform closestUnit;
    public float distanceFromClosestUnit;

    [Header("Horgr")]
    private float distanceFromClosestHorgr;
    public bool hasArrivedAtHorgr;
    public bool horgrSwitch;
    public bool spawnedFromBuilding;


    #region Startup
    public override void Awake()
    {
        base.Awake();
        state = EnemyState.Attack;
    }
    public override void Start()
    {
        base.Start();
        StartCoroutine(Tick());
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

        if (_UM.unitList.Count != 0)
        {
            distanceFromClosestUnit = Vector3.Distance(closestUnit.transform.position, transform.position);
        }

        switch (state)
        {
            case EnemyState.Work:
                agent.SetDestination(transform.position);

                break;

            case EnemyState.Attack:
                animator.SetBool("hasStoppedHorgr", false);
                horgrSwitch = false;
                if (_UM.unitList.Count == 0)
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

                if (_horgrExists)
                {
                    distanceFromClosestHorgr = Vector3.Distance(_HORGR.transform.position, transform.position);
                    if (distanceFromClosestHorgr < distanceFromClosestUnit && !spawnedFromBuilding)
                    {
                        state = EnemyState.ClaimSite;
                    }
                    if (distanceFromClosestHorgr >= distanceFromClosestUnit)
                    {
                        state = EnemyState.Attack;
                    }
                }
                break;
            case EnemyState.Flee:

                break;
            case EnemyState.Beacon:
                agent.SetDestination(transform.position);
                break;
            case EnemyState.ClaimSite:
                if (!hasArrivedAtHorgr)
                    agent.SetDestination(_HORGR.transform.position);
                else
                {
                    if (_horgrExists && _HORGR.HasUnits())
                    {
                        animator.SetBool("hasStoppedHorgr", false);
                        state = EnemyState.Attack;
                        horgrSwitch = false;
                    }
                    else
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
                if (_UM.unitList.Count == 0)
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
            if (_UM.unitList.Count == 0)
            {
                var targetRotation = Quaternion.LookRotation(_HOME.transform.position - transform.position);
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
        if (!_horgrExists)
            return;
        
        if(other.CompareTag("Horgr"))
        {
            if (_HORGR.ContainsEnemy(this))
                return;
            
            if(!spawnedFromBuilding)
            {
                _HORGR.AddEnemy(this);
                StartCoroutine(WaitForHorgr());
            }
        }
    }
    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (!_horgrExists)
            return;
        
        if (other.CompareTag("Horgr"))
        {
            if(spawnedFromBuilding == false)
            {
                _HORGR.RemoveEnemy(this);
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
        base.TakeDamage(damage, _damagedBy);
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

    public void FindUnit()
    {
        if (_NoGuardians)
        {
            state = EnemyState.Work;
        }
        agent.SetDestination(closestUnit.transform.position);
    }

    public void FindHomeTree()
    {
        agent.SetDestination(_HOME.transform.position);
    }

    private void OnArrivedAtHorgr()
    {
        state = EnemyState.Attack;
    }
    public override void Win()
    {
        state = EnemyState.Cheer;
        StopCoroutine(Tick());
    }
    private void OnEnable()
    {
        GameEvents.OnUnitArrivedAtHorgr += OnArrivedAtHorgr;
    }

    private void OnDisable()
    {
        GameEvents.OnUnitArrivedAtHorgr -= OnArrivedAtHorgr;
    }
}
