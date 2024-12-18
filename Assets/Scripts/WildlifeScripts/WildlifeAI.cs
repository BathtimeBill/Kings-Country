using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public class WildlifeAI : GameBehaviour
{
    [Header("Wildlife Type")]
    public WildlifeID wildlifeID;
    private WildlifeData wildlifeData;
    private float health;
    private float walkRadius;
    private float runRadius;

    [Header("Components")]
    public Animator animator;
    public NavMeshAgent navAgent;
    public AudioSource audioSource;
    private AudioClip audioClip;
    public Outline outline;
    
    private void Start()
    {
        Setup();
        StartCoroutine(Move());
        outline.enabled = false;
    }

    private void Update()
    {
        animator.SetBool("HasStopped", navAgent.velocity == Vector3.zero);
    }
    
    private void Setup()
    {
        wildlifeData = _DATA.GetWildlife(wildlifeID);
        navAgent.speed = wildlifeData.baseSpeed;
        health = wildlifeData.baseHealth;
        walkRadius = wildlifeData.walkRadius;
        runRadius = wildlifeData.runRadius;
    }
    
    IEnumerator Move()
    {
        navAgent.speed = wildlifeData.baseSpeed;
        navAgent.SetDestination(SpawnX.GetSpawnPositionInRadius(transform.position, walkRadius));
        yield return new WaitForSeconds(25f);
        StartCoroutine(Move());
    }

    IEnumerator Run()
    {
        navAgent.speed = wildlifeData.runSpeed;
        Vector3 randomDirection = Random.insideUnitSphere * runRadius;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, runRadius, 1);
        Vector3 finalPosition = hit.position;

        while (Vector3.Distance(transform.position, finalPosition) > 0.1f)
        {
            navAgent.SetDestination(finalPosition);
            yield return null;
        }
        animator.SetBool("IsPanicked", false);
        yield return new WaitForSeconds(.5f);
        StartCoroutine(Move());
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<UnitWeaponCollider>() == null)
            return;
        
        other.gameObject.SetActive(false);
        animator.SetBool("IsPanicked", true);
        StopAllCoroutines();
        
        health -= other.GetComponent<UnitWeaponCollider>().Damage;
        if (health <= 0)
        {
            Die();
            _SM.PlaySound(audioSource, wildlifeData.dieSounds);
        }
        else
        {
            StartCoroutine(Run());
            _SM.PlaySound(audioSource, wildlifeData.distressSounds);
        }
    }

    private void Die()
    {
        StopAllCoroutines();
        navAgent.SetDestination(transform.position);
        animator.SetTrigger("Die");
        if (_GAME.timeSinceWildlifeKilled >= 30)
        {
            Vector3 wildlifeLocation = new Vector3(0, 50, 0);
            GameObject ws = Instantiate(_GAME.warningSprite, transform.position + wildlifeLocation, Quaternion.Euler(90f, 0f, 0f));
            Destroy(ws, 5);
        }
        GameEvents.ReportOnWildlifeKilled(gameObject);
        //POLISH rather than disappear, fade in particles and sink into the ground
        Destroy(gameObject, 15);
    }
    
    private void OnFertileSoilUpgrade() => health = wildlifeData.baseHealth * 2;
    private void OnWildlifeOutlineButton(bool _holding) => outline.enabled = _holding;
    
    private void OnEnable()
    {
        GameEvents.OnFertileSoilUpgrade += OnFertileSoilUpgrade;
        InputManager.OnWildlifeOutlineButton += OnWildlifeOutlineButton;
    }
    
    private void OnDisable()
    {
        GameEvents.OnFertileSoilUpgrade -= OnFertileSoilUpgrade;
        InputManager.OnWildlifeOutlineButton -= OnWildlifeOutlineButton;
    }
}
