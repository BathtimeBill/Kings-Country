using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Lord : Enemy
{
    public Animator horseAnimator;
    public Animator knightAnimator;
    public Transform closestUnit;
    public float distanceFromClosestUnit;
    public ParticleSystem swingParticle;
    public Slider slider;
    public AudioSource audioSource;
    public AudioSource barkSource;
    public GameObject bloodParticle1;
    public GameObject deathObject;
    public GameObject deathObject2;
    public GameObject rider;
    public AudioClip[] allLordVocals;
    public List<AudioClip> lordVocals;
    public float stoppingDistance;
    public GameObject homeTree;

    #region Startup
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        homeTree = GameObject.FindGameObjectWithTag("Home Tree");
        slider.value = CalculateHealth();
        audioSource = GetComponent<AudioSource>();  
        StartCoroutine(Tick());
        StartCoroutine(Bark());
    }
    #endregion

    #region AI
    IEnumerator Tick()
    {
        yield return new WaitForSeconds(0.2f);
        if(UnitSelection.Instance.unitList.Count > 0)
        {
            closestUnit = GetClosestUnit();
            distanceFromClosestUnit = Vector3.Distance(closestUnit.transform.position, transform.position);
            agent.SetDestination(closestUnit.transform.position);
            if (distanceFromClosestUnit < 40)
            {
                knightAnimator.SetBool("unitIsClose", true);
            }
            else
            {
                knightAnimator.SetBool("unitIsClose", false);
            }
        }
        else
        {
            knightAnimator.SetBool("unitIsClose", true);
            agent.SetDestination(homeTree.transform.position);
        }



        if (agent.velocity != Vector3.zero)
        {
            horseAnimator.SetBool("hasStopped", false);
        }
        if (agent.velocity == Vector3.zero)
        {
            horseAnimator.SetBool("hasStopped", true);
        }
        StartCoroutine(Tick());
    }

    IEnumerator Bark()
    {
        yield return new WaitForSeconds(Random.Range(5, 20));
        GetRandomBark();
        if(lordVocals.Count > 0)
        {
            barkSource.Play();
            StartCoroutine(Bark());
        }

    }

    void GetRandomBark()
    {
        int i = Random.Range(0, lordVocals.Count);
        barkSource.clip = lordVocals[i];
        lordVocals.Remove(lordVocals[i]);
    }
    #endregion

    #region Damage
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        /*
        if (other.tag == "Spit")
        {
            TakeDamage(_GM.spitDamage);
        }
        if (other.tag == "SpitExplosion")
        {
            TakeDamage(_GM.spitExplosionDamage);
        }*/
        if (other.tag == "Explosion")
        {
            TakeDamage(100, "Mine");//TODO where are these numbers from and from who?
        }
        if (other.tag == "Explosion2")
        {
            TakeDamage(200, "Mine");//TODO where are these numbers from and from who?
        }
        if (other.tag == "Spit")
        {
            agent.speed = speed / 2;
        }
    }
    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.tag == "Spit")
        {
            agent.speed = speed;
        }
    }
    public override void TakeDamage(int damage, string _damagedBy)
    {
        audioSource.clip = _SM.GetGruntSounds();
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
        slider.value = CalculateHealth();
        base.TakeDamage(damage, _damagedBy);
    }
    public override void Die(Enemy _thisUnit, string _killedBy, DeathID _deathID)
    {
        //GameObject go = Instantiate(deathObject, transform.position, transform.rotation);
        GameObject go2 = Instantiate(deathObject2, rider.transform.position, rider.transform.rotation);
        go2.transform.localScale = rider.transform.localScale;
        //Destroy(go, 15);
        //Destroy(gameObject);
        base.Die(_thisUnit, _killedBy, _deathID);
    }

    #endregion

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

        if (UnitSelection.Instance.unitList == null)
        {
            return null;
        }
        else
            return trans;
    }
    float CalculateHealth()
    {
        return health / maxHealth;
    }
}
