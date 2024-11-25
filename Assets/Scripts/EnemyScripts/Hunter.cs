using System.Collections;
using UnityEngine;

public class Hunter : Enemy
{
    [Header("Hunter Specific")] 
    public GameObject arrowObject;
    private Arrow arrow;
    public Transform firingPoint;
    [Header("Tick")]
    public float seconds = 0.5f;
    [Header("Stats")]
    public EnemyState state;
    private float damping = 5;
    public float stoppingDistance;
    
    [Header("Components")]
    public Transform closestWildlife;
    public float distanceFromClosestWildlife;
    public Transform closestUnit;
    public float distanceFromClosestUnit;

    [Header("Hut")]
    public GameObject destination;
    public float distanceFromClosestHut;
    public bool hasArrivedAtHorgr;
    public bool hutSwitch;
    public bool spawnedFromSite;

    #region Startup
    public override void Awake()
    {
        base.Awake();
        state = EnemyState.Work;
    }

    public override void Start()
    {
        base.Start();
        arrow = arrowObject.GetComponent<Arrow>();
        arrowObject.SetActive(false);
        destination = _hutExists ? _HUT.gameObject : _HOME.gameObject;
        agent.stoppingDistance = attackRange;
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

        closestUnit = GetClosestUnit();// ObjectX.GetClosest(gameObject, _UM.unitList).transform;
        closestWildlife = _WildlifeExist ? ObjectX.GetClosest(gameObject, _GM.currentWildlife).transform : closestUnit;
        distanceFromClosestHut = Vector3.Distance(destination.transform.position, transform.position);
        distanceFromClosestUnit = _GuardiansExist ? Vector3.Distance(closestUnit.transform.position, transform.position) : 200000;
        distanceFromClosestWildlife = _WildlifeExist ? Vector3.Distance(closestWildlife.transform.position, transform.position) : 200000;
        
        if (distanceFromClosestUnit < attackRange && _GuardiansExist)
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
                if (_NoGuardians || distanceFromClosestUnit > attackRange)
                    state = EnemyState.Work;
                FaceTarget();
                break;

            case EnemyState.Beacon:
                agent.stoppingDistance = 0;
                agent.SetDestination(transform.position);
                break;

            case EnemyState.ClaimSite:
                if (!hasArrivedAtHorgr)
                {
                    agent.SetDestination(destination.transform.position);
                    agent.stoppingDistance = attackRange / 2;
                }
                else
                {
                    agent.stoppingDistance = attackRange;
                    if (_hutExists && _HUT.HasUnits())
                    {
                        animator.SetBool("hasStoppedHorgr", false);
                        FaceTarget();
                    }
                    else
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

        if (distanceFromClosestHut > distanceFromClosestWildlife)
        {
            state = EnemyState.Work;
        }
        if (distanceFromClosestHut <= distanceFromClosestWildlife && !spawnedFromSite)
        {
            state = EnemyState.ClaimSite;
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
        
        if(!_NoGuardians)
            StartCoroutine(Tick());
    }

    private void FixedUpdate()
    {
        if(distanceFromClosestUnit < attackRange)
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
        if (!_hutExists)
            return;
        if (other.CompareTag("Hut"))
        {
            if (_HUT.ContainsEnemy(this))
                return;
            
            if (spawnedFromSite == false)
            {
                _HUT.AddEnemy(this);
                StartCoroutine(WaitForHut());
            }
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        
        if (!_hutExists)
            return;
        
        if (other.CompareTag("Hut"))
        {
            if(spawnedFromSite == false)
            {
                _HUT.RemoveEnemy(this);
                state = EnemyState.Attack;
                hasArrivedAtHorgr = false;
            }
        }
    }

    public override void TakeDamage(int damage, string _damagedBy)
    {
        base.TakeDamage(damage, _damagedBy);
    }

    public override void Die(Enemy _thisUnit, string _killedBy, DeathID _deathID)
    {
        base.Die(_thisUnit, _killedBy, _deathID);
    }

    IEnumerator WaitForHut()
    {
        Log("Hut coroutine");
        yield return new WaitForSeconds(2f);
        animator.SetBool("hasStoppedHorgr", true);
        hasArrivedAtHorgr = true;
    }
    #endregion

    public void Hunt()
    {
        if(_NoWildlife)
        {
            agent.SetDestination(_NoGuardians ? transform.position : closestUnit.transform.position);
        }
        else
        {
            if (distanceFromClosestWildlife < attackRange)
            {
                transform.LookAt(closestWildlife.transform.position);
            }
            agent.SetDestination(closestWildlife.transform.position);
        }
    }
    public void FaceTarget()
    {
        var lookPos = closestUnit.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        agent.SetDestination(closestUnit.transform.position);
    }

    public override void Attack(int _attack)
    {
        if (!_inGame)
            return;

        //checks if there are any animals in the scene then calculates the distance from the hunter enemy to that animal.
        if(_WildlifeExist)
            distanceFromClosestWildlife = Vector3.Distance(closestWildlife.transform.position, transform.position);
        
        //Checks weather the hunter is shooting at an animal or a player unit and then orients the arrow towards that result.
        Transform closestTarget = distanceFromClosestWildlife < distanceFromClosestUnit ? closestWildlife : closestUnit;
        
        arrowObject.transform.position = firingPoint.transform.position;
        arrowObject.transform.rotation = firingPoint.transform.rotation;
        arrowObject.SetActive(true);
        arrow.Setup(closestTarget);
        DisableAfterTime(arrowObject, 1);
        PlaySound(unitData.attackSounds[0]);
    }

    public override void CheckState()
    {
        if (_HUT == null) return;
        
        if (_HUT.ContainsEnemy(GetComponent<Enemy>()) && _HUT.HasEnemies())
            animator.SetBool("allWildlifeDead", true);
    }

    private void OnArrivedAtHut()
    {
        state = EnemyState.Attack;
    }

    public override void Win()
    {
        StopCoroutine(Tick());
        state = EnemyState.Cheer;
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
