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
        if (_HutExists)
        {
            Vector3 randomHutPos = SpawnX.GetSpawnPositionInRadius(_HUT.transform.position, _HUT.GetComponent<SphereCollider>().radius);
            hutTargetPoint = new GameObject("HutTargetPoint");
            hutTargetPoint.transform.position = randomHutPos;
        }
        base.Start();
    }
    #endregion
    
    #region AI

    public override void SetState()
    {
        distanceToWildlife = _WildlifeExist ? Vector3.Distance(ObjectX.GetClosest(gameObject, _GM.currentWildlife).transform.position, transform.position) : 20000;
        distanceToHut = _HutExists ? Vector3.Distance(hutTargetPoint.transform.position, transform.position) : 20000;
        SetClosestUnit();
        
        if (hasArrivedAtHut)
        {
            targetObject = transform;
            ChangeState(EnemyState.DefendSite);
        }
        else if (distanceFromClosestUnit < attackRange && _GuardiansExist)
        {
            targetObject = closestUnit;
            ChangeState(EnemyState.Attack);
        }
        else if (distanceToHut > distanceToWildlife)
        {
            targetObject = ObjectX.GetClosest(gameObject, _GM.currentWildlife).transform;
            ChangeState(EnemyState.Work);
        }
        else if (distanceToHut <= distanceToWildlife && !spawnedFromSite)
        {
            targetObject = hutTargetPoint.transform;
            ChangeState(EnemyState.ClaimSite);
        }
        
        HandleState();
    }
    
    public override void HandleWorkState()
    {
        if (_NoWildlife && _NoGuardians && !_HutExists)
        {
            ChangeState(EnemyState.Idle);
            return;
        }
        base.HandleWorkState();
    }

    public override void HandleIdleState()
    {
        if(_WildlifeExist || _GuardiansExist && !hasArrivedAtHut)
            ChangeState(EnemyState.Work);
        else
            base.HandleIdleState();
    }
    
    public override void HandleClaimState()
    {
        if(hasArrivedAtHut)
            ChangeState(EnemyState.DefendSite);
        base.HandleClaimState();
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
            ChangeState(EnemyState.DefendSite);
        }
    }

    private IEnumerator WaitForHut()
    {
        ChangeState(EnemyState.DefendSite);
        yield return new WaitForSeconds(2f);
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
            SetState();
            hasArrivedAtHut = false;
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
