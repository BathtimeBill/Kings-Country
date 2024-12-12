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
        if (hasArrivedAtHut)
        {
            if (distanceToClosestUnit < attackRange)
            {
                targetObject = closestUnit;
                ChangeState(EnemyState.Attack);
            }
            else
            {
                targetObject = transform;
                ChangeState(EnemyState.DefendSite);
            }
        }
        else if (TargetWithinRange)
        {
            ChangeState(EnemyState.Attack);
        }
        else if (_HutExists)
        {
            if (_WildlifeExist && distanceToHut > distanceToWildlife)
            {
                targetObject = ObjectX.GetClosest(gameObject, _GM.currentWildlife).transform;
            }
            else if(_WildlifeExist && distanceToHut <= distanceToWildlife && !spawnedFromSite)
            {
                targetObject = hutTargetPoint.transform;
            }
            else
            {
                targetObject = hutTargetPoint.transform;
            }
            ChangeState(EnemyState.Work);
        }
        else if (_WildlifeExist)
        {
            targetObject = ObjectX.GetClosest(gameObject, _GM.currentWildlife).transform;
            ChangeState(EnemyState.Work);
        }
        else if (distanceToClosestUnit < attackRange)
        {
            targetObject = closestUnit;
            ChangeState(EnemyState.Attack);
        }
        else
        {
            targetObject = transform;
            ChangeState(EnemyState.Idle);
        }
    }
    
    public override void HandleWorkState()
    {
        if (_NoWildlife && _NoGuardians && !_HutExists)
        {
            ChangeState(EnemyState.Idle);
        }
    }

    public override void HandleIdleState()
    {
        enemyAnimation.PlayAttackAnimation(false);
        if(_WildlifeExist || !hasArrivedAtHut)
            ChangeState(EnemyState.Work);
    }
    
    public override void HandleAttackState()
    {
        enemyAnimation.PlayHoldAnimation(false);
        if (!TargetWithinRange)
            ChangeState(EnemyState.Work);
    }

    public override void HandleDefendState()
    {
        if (_HUT.HasUnits())
        {
            targetObject = closestUnit;
            ChangeState(EnemyState.Attack);
        }
        else
        {
            enemyAnimation.PlayHoldAnimation(true);
            StandStill(); 
            ChangeState(EnemyState.DefendSite);
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
