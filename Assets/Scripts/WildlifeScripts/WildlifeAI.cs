using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class WildlifeAI : GameBehaviour
{
    [Header("Wildlife Type")]
    public WildlifeID type;
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
    public Outline outline;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        Setup();
    }

    void Start()
    {
        StartCoroutine(Move());
        outline.enabled = false;
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
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            outline.enabled = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            outline.enabled = false;
        }
    }
    IEnumerator Move()
    {
        switch (type)
        {
            case WildlifeID.Rabbit:
                navAgent.speed = 1.5f;
                break;

            case WildlifeID.Deer:
                navAgent.speed = 2;
                break;

            case WildlifeID.Boar:
                navAgent.speed = 2;
                break;

        }

        Vector3 randomDirection = transform.position + Random.insideUnitSphere * walkRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        Vector3 finalPosition = hit.position;
        navAgent.SetDestination(finalPosition);
        //while (Vector3.Distance(transform.position, finalPosition) > 0.1f)
        //{
              //navAgent.SetDestination(finalPosition);
        //    yield return null;
        //}
        yield return new WaitForSeconds(25f);

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
                case WildlifeID.Rabbit:

                    break;

                case WildlifeID.Deer:
                    audioClip = _SM.GetDeerDistressSound();
                    break;

                case WildlifeID.Boar:
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
                case WildlifeID.Rabbit:

                    break;

                case WildlifeID.Deer:
                    audioClip = _SM.GetDeerDistressSound();
                    break;

                case WildlifeID.Boar:
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
            case WildlifeID.Rabbit:
                navAgent.speed = 1.5f;
                maxHealth = _DATA.HasPerk(PerkID.Fertile) ? 50 : 25;
                health = maxHealth;
                break;

            case WildlifeID.Deer:
                navAgent.speed = 2;
                maxHealth = _DATA.HasPerk(PerkID.Fertile) ? 160 : 80;
                health = maxHealth;
                break;

            case WildlifeID.Boar:
                navAgent.speed = 2;
                maxHealth = _DATA.HasPerk(PerkID.Fertile) ? 200 : 100;
                health = maxHealth;
                break;

        }
    }
    private void Die()
    {

        GameObject go;
        go = Instantiate(deadPrefab, transform.position, transform.rotation);
        Destroy(go, 15);
        if (_GM.timeSinceWildlifeKilled >= 30)
        {
            Vector3 wildlifeLocation = new Vector3(0, 50, 0);
            GameObject ws = Instantiate(_GM.warningSprite, transform.position + wildlifeLocation, Quaternion.Euler(90f, 0f, 0f));
            Destroy(ws, 5);
        }
        GameEvents.ReportOnWildlifeKilled();
        Destroy(gameObject);
    }
    private void OnFertileSoilUpgrade()
    {
        switch (type)
        {
            case WildlifeID.Rabbit:
                maxHealth = 50;
                health = maxHealth;
                break;

            case WildlifeID.Deer:
                maxHealth = 160;
                health = maxHealth;
                break;

            case WildlifeID.Boar:
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
