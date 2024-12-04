using System.Collections;
using UnityEngine;

public class Warrior : Enemy
{
    [Header("Horgr")]
    private float distanceFromClosestHorgr;
    public bool hasArrivedAtHorgr;
    public bool horgrSwitch;
    
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
            StopAllCoroutines();

        SetClosestUnit();

        if (agent.velocity != Vector3.zero || distanceFromClosestUnit >= 10)
        {
            animator.SetBool("hasStopped", false);
        }
        if (agent.velocity == Vector3.zero || distanceFromClosestUnit < 10)
        {
            animator.SetBool("hasStopped", true);
        }
        HandleState();
        yield return new WaitForSeconds(tickRate);
        StartCoroutine(Tick());
    }
    
    public override void HandleWorkState()
    {
        agent.SetDestination(transform.position);
    }

    public override void HandleRelaxState()
    {
    }

    public override void HandleAttackState()
    {
        animator.SetBool("hasStoppedHorgr", false);
        horgrSwitch = false;
        if (_NoGuardians)
        {
            FindHomeTree();
            SmoothFocusOnEnemy();
        }
        else
        {
            FindUnit();
            if (distanceFromClosestUnit < attackRange)
            {
                SmoothFocusOnEnemy();
            }
        }

        if (_HorgrExists)
        {
            distanceFromClosestHorgr = Vector3.Distance(_HORGR.transform.position, transform.position);
            if (distanceFromClosestHorgr < distanceFromClosestUnit && !spawnedFromSite)
            {
                state = EnemyState.ClaimSite;
            }
            if (distanceFromClosestHorgr >= distanceFromClosestUnit)
            {
                state = EnemyState.Attack;
            }
        }
    }

    public override void HandleClaimState()
    {
        if (!hasArrivedAtHorgr)
            agent.SetDestination(_HORGR.transform.position);
        else
        {
            if (_HorgrExists && _HORGR.HasUnits())
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
    }

    public override void HandleVictoryState()
    {
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
        if (!_HorgrExists)
            return;
        
        if(other.CompareTag("Horgr"))
        {
            if (_HORGR.ContainsEnemy(this))
                return;
            
            if(!spawnedFromSite)
            {
                _HORGR.AddEnemy(this);
                StartCoroutine(WaitForHorgr());
            }
        }
    }
    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (!_HorgrExists)
            return;
        
        if (other.CompareTag("Horgr"))
        {
            if(spawnedFromSite == false)
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
        state = EnemyState.Victory;
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
