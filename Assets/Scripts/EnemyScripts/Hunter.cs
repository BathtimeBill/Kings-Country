using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Hunter : Enemy
{
    [Header("Hunter Specific")] 
    public GameObject arrowObject;
    private Arrow arrow;
    public Transform firingPoint;
    [Header("Stats")]
    private float damping = 5;

    [FormerlySerializedAs("claimSite")] [Header("Hut")]
    public GameObject hutObject;
    public float distanceFromClosestHut;
    public bool hasArrivedAtHut;
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
        //arrow = arrowObject.GetComponent<Arrow>();
        //arrowObject.SetActive(false);
        StartCoroutine(Tick());
    }
    #endregion
    
    #region AI

    IEnumerator Tick()
    {
        if (_GM.gameState == GameState.Lose)
            StopAllCoroutines();

        /*if (distanceFromClosestUnit < attackRange && _GuardiansExist)
        {
            ChangeState(EnemyState.Attack);
        }
        else if (distanceFromClosestHut > distanceFromClosestWildlife)
        {
            ChangeState(EnemyState.Work);
        }
        else if (distanceFromClosestHut <= distanceFromClosestWildlife && !spawnedFromSite)
        {
            ChangeState(EnemyState.ClaimSite);
        }*/
        

        //if(distanceFromClosestUnit < stoppingDistance)
        //    ChangeState(EnemyState.Attack);
        
        HandleState();
        
        yield return new WaitForSeconds(tickRate);
        
        //if(!_NoGuardians)
            StartCoroutine(Tick());
    }
    
    public override void HandleWorkState()
    {
        base.HandleWorkState();

        if (_NoWildlife && _NoGuardians && !_HutExists)
        {
            ChangeState(EnemyState.Relax);
            return;
        }

        if (_WildlifeExist)
        {
            targetObject = ObjectX.GetClosest(gameObject, _GM.currentWildlife).transform;
        }
        else if (_HutExists)
        {
            hutObject = _HUT.gameObject;
            distanceFromClosestHut = Vector3.Distance(hutObject.transform.position, transform.position);
            if(distanceFromClosestHut <= distanceFromTarget && !spawnedFromSite)
                ChangeState(EnemyState.ClaimSite);
        }
        else if (_GuardiansExist)
        {
            SetClosestUnit();
            targetObject = closestUnit;
        }
        
        distanceFromTarget = Vector3.Distance(targetObject.transform.position, transform.position);
        if (distanceFromTarget < attackRange)
        {
            transform.LookAt(targetObject.transform.position);
            ChangeState(EnemyState.Attack);
        }
        else
        {
            agent.SetDestination(targetObject.transform.position);
        }
    }

    public override void HandleRelaxState()
    {
        base.HandleRelaxState();
        if(_WildlifeExist || _GuardiansExist)
            ChangeState(EnemyState.Work);
    }

    public override void HandleAttackState()
    {
        base.HandleAttackState();
        hutSwitch = false;
        //if (_NoGuardians || distanceFromTarget > attackRange)
        //{ 
        //    state = EnemyState.Work;
        //    return;
        //}
        FaceTarget();
    }

    public override void HandleClaimState()
    {
        base.HandleClaimState();
        if (!hasArrivedAtHut)
        {
            agent.SetDestination(hutObject.transform.position);
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
                    //animator.SetBool("hasStoppedHorgr", true);
                    enemyAnimation.PlayIdleAnimation();
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
        if(distanceFromTarget < attackRange)
        {
            SmoothFocusOnEnemy();
        }
    }
    private void SmoothFocusOnEnemy()
    {
        if(targetObject != null)
        {
            var targetRotation = Quaternion.LookRotation(targetObject.transform.position - transform.position);
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
                hasArrivedAtHut = false;
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
        hasArrivedAtHut = true;
    }
    #endregion

    public void FaceTarget()
    {
        var lookPos = targetObject.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }

    public override void Attack(int _attack)
    {
        if (!_inGame)
            return;

        print("Firing Arrow");
        //checks if there are any animals in the scene then calculates the distance from the hunter enemy to that animal.
        //if(_WildlifeExist)
        //    distanceFromTarget = Vector3.Distance(targetObject.transform.position, transform.position);
        
        //Checks weather the hunter is shooting at an animal or a player unit and then orients the arrow towards that result.
        //Transform closestTarget = distanceFromTarget < distanceFromClosestUnit ? targetObject : closestUnit;
        
        //arrowObject.transform.position = firingPoint.transform.position;
        //arrowObject.transform.rotation = firingPoint.transform.rotation;
        //arrowObject.SetActive(true);
        GameObject ar = Instantiate(arrowObject , firingPoint.transform.position, firingPoint.transform.rotation);
        ar.SetActive(true);
        ar.GetComponent<Arrow>().Setup(targetObject);
        //DisableAfterTime(arrowObject, 1);
        PlaySound(unitData.attackSounds[0]);
    }

    /*public override void CheckState()
    {
        if (_HUT == null) 
            return;
        
        if (_HUT.ContainsEnemy(GetComponent<Enemy>()) && _HUT.HasEnemies())
            animator.SetBool("allWildlifeDead", true);
    }*/

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
