using System.Collections;
using UnityEngine;

public class Hunter : Enemy
{
    [Header("Hunter Specific")] 
    public GameObject arrowObject;
    private Arrow arrow;
    public Transform firingPoint;
    [Header("Stats")]
    private float damping = 5;
    public Transform closestWildlife;
    public float distanceFromClosestWildlife;

    [Header("Hut")]
    public GameObject destination;
    public float distanceFromClosestHut;
    public bool hasArrivedAtHorgr;
    public bool hutSwitch;


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
        destination = _HutExists ? _HUT.gameObject : _HOME.gameObject;
        StartCoroutine(Tick());
    }
    #endregion
    
    #region AI

    IEnumerator Tick()
    {
        if (_GM.gameState == GameState.Lose)
            StopAllCoroutines();

        SetClosestUnit();
        
        closestWildlife = _WildlifeExist ? ObjectX.GetClosest(gameObject, _GM.currentWildlife).transform : closestUnit;
        distanceFromClosestWildlife = _WildlifeExist ? Vector3.Distance(closestWildlife.transform.position, transform.position) : 200000;
        distanceFromClosestHut = Vector3.Distance(destination.transform.position, transform.position);
        
        if (distanceFromClosestUnit < attackRange && _GuardiansExist)
        {
            ChangeState(EnemyState.Attack);
        }
        if (distanceFromClosestHut > distanceFromClosestWildlife)
        {
            ChangeState(EnemyState.Work);
        }
        if (distanceFromClosestHut <= distanceFromClosestWildlife && !spawnedFromSite)
        {
            ChangeState(EnemyState.ClaimSite);
        }


        if (agent.velocity != Vector3.zero || distanceFromClosestUnit >= stoppingDistance)
        {
            animator.SetBool("hasStopped", false);
        }
        if (agent.velocity == Vector3.zero || distanceFromClosestUnit < stoppingDistance)
        {
            animator.SetBool("hasStopped", true);
        }
        
        HandleState();
        
        yield return new WaitForSeconds(tickRate);
        
        if(!_NoGuardians)
            StartCoroutine(Tick());
    }
    
    public override void HandleWorkState()
    {
        if(_NoWildlife)
        {
            if (_NoGuardians)
                state = EnemyState.Victory;
            else
                agent.SetDestination(closestUnit.transform.position);
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

    public override void HandleRelaxState()
    {
        
    }

    public override void HandleAttackState()
    {
        hutSwitch = false;
        if (_NoGuardians || distanceFromClosestUnit > attackRange)
            state = EnemyState.Work;
        FaceTarget();
    }

    public override void HandleClaimState()
    {
        if (!hasArrivedAtHorgr)
        {
            agent.SetDestination(destination.transform.position);
        }
        else
        {
            if (_HutExists && _HUT.HasUnits())
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
    }

    public override void HandleVictoryState()
    {
        
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
        if (!_HutExists)
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
        
        if (!_HutExists)
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
        if (_HUT == null) 
            return;
        
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
        state = EnemyState.Victory;
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
