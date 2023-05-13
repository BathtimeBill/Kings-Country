using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class WildlifeAI : GameBehaviour
{
    [Header("Wildlife Type")]
    public WildlifeType type;
    [Header("Status")]
    public bool isPanicked = false;
    [Header("General")]
    public float health;
    public float maxHealth;
    public float walkRadius = 5f;
    public float runRadius = 100f;

    [Header("Components")]
    public Animator animator;
    public NavMeshAgent navAgent;
    public GameObject deadPrefab;
    public AudioSource audiosource;
    private AudioClip audioClip;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        Setup();
    }

    void Start()
    {
        StartCoroutine(Move());
    }

    void Update()
    {
        if (navAgent.velocity != Vector3.zero)
        {
            animator.SetBool("hasStopped", false);
        }
        if (navAgent.velocity == Vector3.zero)
        {
            animator.SetBool("hasStopped", true);
        }
    }
    IEnumerator Move()
    {
        switch (type)
        {
            case WildlifeType.Rabbit:
                navAgent.speed = 1.5f;
                break;

            case WildlifeType.Deer:
                navAgent.speed = 2;
                break;

            case WildlifeType.Boar:
                navAgent.speed = 2;
                break;

        }

        Vector3 randomDirection = transform.position + Random.insideUnitSphere * walkRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        Vector3 finalPosition = hit.position;
        while (Vector3.Distance(transform.position, finalPosition) > 0.1f)
        {
            navAgent.SetDestination(finalPosition);
            yield return null;
        }
        yield return new WaitForSeconds(8f);

        StartCoroutine(Move());

    }

    IEnumerator Run()
    {
        navAgent.speed = 13;
        Vector3 randomDirection = Random.insideUnitSphere * runRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, runRadius, 1);
        Vector3 finalPosition = hit.position;

        while (Vector3.Distance(transform.position, finalPosition) > 0.1f)
        {
            navAgent.SetDestination(finalPosition);
            //transform.rotation = Quaternion.LookRotation(finalPosition);
            yield return null;
        }
        isPanicked = false;
        animator.SetBool("isPanicked", false);
        yield return new WaitForSeconds(.5f);


        StartCoroutine(Move());
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Arrow")
        {
            isPanicked = true;
            animator.SetBool("isPanicked", true);
            StopAllCoroutines();
            StartCoroutine(Run());
            health -= 25;

            if (health <= 0)
            {
                Die();
            }

            Destroy(other.gameObject);

            switch (type)
            {
                case WildlifeType.Rabbit:

                    break;

                case WildlifeType.Deer:
                    audioClip = _SM.GetDeerDistressSound();
                    break;

                case WildlifeType.Boar:
                    audioClip = _SM.GetBoarDistressSound();
                    break;

            }
            audiosource.clip = audioClip;
            audiosource.Play();

        }
        if (other.tag == "Arrow2")
        {
            isPanicked = true;
            animator.SetBool("isPanicked", true);
            StopAllCoroutines();
            StartCoroutine(Run());
            health -= 50;

            if (health <= 0)
            {
                Die();
            }

            Destroy(other.gameObject);

            switch (type)
            {
                case WildlifeType.Rabbit:

                    break;

                case WildlifeType.Deer:
                    audioClip = _SM.GetDeerDistressSound();
                    break;

                case WildlifeType.Boar:
                    audioClip = _SM.GetBoarDistressSound();
                    break;

            }
            audiosource.clip = audioClip;
            audiosource.Play();

        }
    }
    private void Setup()
    {
        switch (type)
        {
            case WildlifeType.Rabbit:
                navAgent.speed = 1.5f;
                if(_UM.fertileSoil)
                {
                    maxHealth = 50;
                }
                else
                {
                    maxHealth = 25;
                }
                health = maxHealth;
                break;

            case WildlifeType.Deer:
                navAgent.speed = 2;
                if (_UM.fertileSoil)
                {
                    maxHealth = 160;
                }
                else
                {
                    maxHealth = 80;
                }
                health = maxHealth;
                break;

            case WildlifeType.Boar:
                navAgent.speed = 2;
                if (_UM.fertileSoil)
                {
                    maxHealth = 200;
                }
                else
                {
                    maxHealth = 100;
                }
                health = maxHealth;
                break;

        }
    }
    private void Die()
    {

        GameObject go;
        go = Instantiate(deadPrefab, transform.position, transform.rotation);
        Destroy(go, 15);
        GameEvents.ReportOnWildlifeKilled();
        Destroy(gameObject);
    }
    private void OnFertileSoilUpgrade()
    {
        switch (type)
        {
            case WildlifeType.Rabbit:
                maxHealth = 50;
                health = maxHealth;
                break;

            case WildlifeType.Deer:
                maxHealth = 160;
                health = maxHealth;
                break;

            case WildlifeType.Boar:
                maxHealth = 200;
                health = maxHealth;
                break;
        }
    }

    private void OnEnable()
    {
        GameEvents.OnFertileSoilUpgrade += OnFertileSoilUpgrade;
    }

    private void OnDisable()
    {
        GameEvents.OnFertileSoilUpgrade -= OnFertileSoilUpgrade;
    }
}
