using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Hunter : Enemy
{
    [Header("Hunter Specific")] 
    public GameObject arrowObject;
    private Arrow arrow;
    public Transform firingPoint;

    private bool hasArrivedAtHut;
    private float distanceToWildlife;
    private float distanceToHut;
    private GameObject hutTargetPoint;

    #region Startup
    public override void Start()
    {
        //arrow = arrowObject.GetComponent<Arrow>();
        //arrowObject.SetActive(false);
        InitializeHutTargetPoint();
        base.Start();
    }

    private void InitializeHutTargetPoint()
    {
        if (_HutExists)
        {
            Vector3 randomHutPos = SpawnX.GetSpawnPositionInRadius(_HUT.transform.position, _HUT.GetComponent<SphereCollider>().radius);
            hutTargetPoint = new GameObject("HutTargetPoint");
            hutTargetPoint.transform.position = randomHutPos;
        }
    }
    #endregion
    
    #region AI
    public override void UpdateDistances()
    {
        distanceToWildlife = _WildlifeExist ? Vector3.Distance(ObjectX.GetClosest(gameObject, _GM.currentWildlife).transform.position, transform.position) : 20000;
        distanceToHut = _HutExists ? Vector3.Distance(hutTargetPoint.transform.position, transform.position) : 20000;
        base.UpdateDistances();
    }

    public override void DetermineState()
    {
        if (_HutExists && !spawnedFromSite)
        {
            if (hasArrivedAtHut)
            {
                targetObject = (distanceToClosestUnit < attackRange) ? closestUnit : transform;
                HandleDefendState();
            }
            else
            {
                if (_WildlifeExist)
                {
                    targetObject = (distanceToHut > distanceToWildlife) ? ObjectX.GetClosest(gameObject, _GM.currentWildlife).transform : hutTargetPoint.transform;
                }
                else
                {
                    targetObject = hutTargetPoint.transform;
                }
                HandleWorkState();
            }
        }
        else if (_WildlifeExist)
        {
            targetObject = ObjectX.GetClosest(gameObject, _GM.currentWildlife).transform;
            HandleWorkState();
        }
        else
        {
            targetObject = transform;
            HandleIdleState();
        }
    }
    
    public override void HandleWorkState()
    {
        ChangeState(EnemyState.Work);
        enemyAnimation.PlayHoldAnimation(false);
    }

    public override void HandleIdleState()
    {
        ChangeState(EnemyState.Idle);
        enemyAnimation.PlayAttackAnimation(false);
    }

    public override void HandleDefendState()
    {
        if (_HUT.HasUnits())
        {
            targetObject = closestUnit;
            HandleTargetState();
        }
        else
        {
            enemyAnimation.PlayHoldAnimation(true);
            StandStill(); 
            ChangeState(EnemyState.DefendSite);
        }
    }

    public override void CheckAttackState()
    {
        if (_HutExists && targetObject == hutTargetPoint.transform || _NoWildlife)
        {
            canAttack = false;
            HandleIdleState();
        }
        else
        {
            canAttack = distanceToTarget <= attackRange && targetObject != transform;
            base.CheckAttackState();
        }
    }

    private IEnumerator WaitForHut()
    {
        yield return new WaitForSeconds(1f);
        ChangeState(EnemyState.DefendSite);
        hasArrivedAtHut = true;
    }
    
    public override void Attack(int _attack)
    {
        if (!_inGame)
            return;

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
    #endregion

    #region Triggers
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag("Hut") && !_HUT.ContainsEnemy(this) && !spawnedFromSite)
        {
            _HUT.AddEnemy(this);
            StartCoroutine(WaitForHut());
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.CompareTag("Hut") && !spawnedFromSite)
        {
            _HUT.RemoveEnemy(this);
            hasArrivedAtHut = false;
            ChangeState(EnemyState.Work);
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
        Destroy(hutTargetPoint);
        base.Die(_thisUnit, _killedBy, _deathID);
    }
    #endregion
}
