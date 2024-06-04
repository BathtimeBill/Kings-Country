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
    private NavMeshAgent navAgent;
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
    [Header("Stats")]
    public float health;
    public float maxHealth;
    public float stoppingDistance;
    public GameObject homeTree;
    public float speed;

    private void Start()
    {
        _EM.enemies.Add(gameObject);
        navAgent = GetComponent<NavMeshAgent>();
        homeTree = GameObject.FindGameObjectWithTag("Home Tree");
        maxHealth = _GM.lordHealth;
        health = maxHealth;
        navAgent.speed = speed;
        slider.value = CalculateHealth();
        audioSource = GetComponent<AudioSource>();  
        StartCoroutine(Tick());
        StartCoroutine(Bark());
    }

    IEnumerator Tick()
    {
        yield return new WaitForSeconds(0.2f);
        if(UnitSelection.Instance.unitList.Count > 0)
        {
            closestUnit = GetClosestUnit();
            distanceFromClosestUnit = Vector3.Distance(closestUnit.transform.position, transform.position);
            navAgent.SetDestination(closestUnit.transform.position);
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
            navAgent.SetDestination(homeTree.transform.position);
        }



        if (navAgent.velocity != Vector3.zero)
        {
            horseAnimator.SetBool("hasStopped", false);
        }
        if (navAgent.velocity == Vector3.zero)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerWeapon")
        {
            TakeDamage(_GM.satyrDamage);
        }
        if (other.tag == "PlayerWeapon2")
        {
            TakeDamage(_GM.orcusDamage);
        }
        if (other.tag == "PlayerWeapon3")
        {
            TakeDamage(_GM.leshyDamage);
        }
        if (other.tag == "PlayerWeapon4")
        {
            TakeDamage(_GM.skessaDamage);
        }
        if (other.tag == "PlayerWeapon5")
        {
            TakeDamage(_GM.goblinDamage);
        }
        if (other.tag == "PlayerWeapon6")
        {
            TakeDamage(_GM.golemDamage);
        }
        if (other.tag == "Spit")
        {
            TakeDamage(_GM.spitDamage);
        }
        if (other.tag == "SpitExplosion")
        {
            TakeDamage(_GM.spitExplosionDamage);
        }
        if (other.tag == "Explosion")
        {
            TakeDamage(100);
        }
        if (other.tag == "Explosion2")
        {
            TakeDamage(200);
        }
        if (other.tag == "Spit")
        {
            navAgent.speed = speed / 2;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Spit")
        {
            navAgent.speed = speed;
        }
    }
    public void TakeDamage(float damage)
    {
        audioSource.clip = _SM.GetGruntSounds();
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
        Vector3 forward = new Vector3(0, 180, 0);

        GameObject bloodParticle;
        bloodParticle = Instantiate(bloodParticle1, transform.position, Quaternion.LookRotation(forward));
        health -= damage;
        slider.value = CalculateHealth();
        if (health <= 0) 
            Die();
    }
    public override void Die()
    {
        _EM.enemies.Remove(gameObject);
        GameObject go;
        GameObject go2;
        go = Instantiate(deathObject, transform.position, transform.rotation);
        go2 = Instantiate(deathObject2, rider.transform.position, rider.transform.rotation);
        go2.transform.localScale = rider.transform.localScale;
        Destroy(go, 15);
        GameEvents.ReportOnEnemyKilled();
        Destroy(gameObject);
    }
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
