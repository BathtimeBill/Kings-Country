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

    [Header("Hut")]
    public bool hasArrivedAtHut;
    public bool hutSwitch;
    private float distanceToWildlife;
    private float distanceToHut;


    #region Startup
    public override void Awake()
    {
        base.Awake();
        //state = EnemyState.Work;
    }

    public override void Start()
    {
        base.Start();
        //arrow = arrowObject.GetComponent<Arrow>();
        //arrowObject.SetActive(false);
    }
    #endregion
    
    #region AI

    public override void SetTargets()
    {
        distanceToWildlife = _WildlifeExist ? Vector3.Distance(ObjectX.GetClosest(gameObject, _GM.currentWildlife).transform.position, transform.position) : 20000;
        distanceToHut = _HutExists ? Vector3.Distance(_HUT.transform.position, transform.position) : 20000;

        if (distanceFromClosestUnit < attackRange && _GuardiansExist)
        {
            SetClosestUnit();
            targetObject = closestUnit;
            print("Targeting Guardian");
            ChangeState(EnemyState.Attack);
        }
        else if (distanceToHut > distanceToWildlife)
        {
            targetObject = ObjectX.GetClosest(gameObject, _GM.currentWildlife).transform;
            print("Targeting Wildlife");
            ChangeState(EnemyState.Work);
        }
        else if (distanceToHut <= distanceToWildlife && !spawnedFromSite)
        {
            targetObject = _HUT.transform;
            print("Targeting Hut");
            //ChangeState(EnemyState.ClaimSite);
        }
        
        agent.SetDestination(targetObject.position);
        distanceFromTarget = Vector3.Distance(targetObject.transform.position, transform.position);
        HandleState();
    }
    
    public override void HandleWorkState()
    {
        if (_NoWildlife && _NoGuardians && !_HutExists)
        {
            print("Relaxing");
            ChangeState(EnemyState.Relax);
        }
        
        if (distanceFromTarget < attackRange)
        {
            SmoothFocusOnTarget(targetObject);
            ChangeState(EnemyState.Attack);
        }
        
        
        //base.HandleWorkState();
    }

    public override void HandleRelaxState()
    {
        if(_WildlifeExist || _GuardiansExist)
            ChangeState(EnemyState.Work);
        else
            base.HandleRelaxState();
    }

    public override void HandleAttackState()
    {
        hutSwitch = false;
        base.HandleAttackState();
    }

    public override void HandleClaimState()
    {
        if (!_HutExists)
            return;
        
        base.HandleClaimState();
        if (!hasArrivedAtHut)
        {
            agent.SetDestination(_HUT.transform.position);
        }
        else
        {
            if (_HUT.HasUnits())
            {
                //animator.SetBool("hasStoppedHorgr", false);
                //FaceTarget();
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

    IEnumerator WaitForHut()
    {
        ChangeState(EnemyState.ClaimSite);
        Log("Hut coroutine");
        yield return new WaitForSeconds(2f);
        //animator.SetBool("hasStoppedHorgr", true);
        hasArrivedAtHut = true;
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
    #endregion

    #region Triggers
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (!_HutExists)
            return;
        if (other.CompareTag("Hut"))
        {
            if (_HUT.ContainsEnemy(this))
                return;
            
            if (!spawnedFromSite)
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
            if(!spawnedFromSite)
            {
                _HUT.RemoveEnemy(this);
                ChangeState(EnemyState.Attack);
                hasArrivedAtHut = false;
            }
        }
    }
    
    #endregion
    
    #region Damage
    public override void TakeDamage(int damage, string _damagedBy)
    {
        base.TakeDamage(damage, _damagedBy);
    }

    public override void Die(Enemy _thisUnit, string _killedBy, DeathID _deathID)
    {
        base.Die(_thisUnit, _killedBy, _deathID);
    }
    #endregion

    public void FaceTarget()
    {
        var lookPos = targetObject.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }

    private void OnArrivedAtHut()
    {
        ChangeState(EnemyState.Attack);
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
