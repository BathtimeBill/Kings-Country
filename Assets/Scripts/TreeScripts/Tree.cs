using System.Collections;
using UnityEngine.AI;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tree : GameBehaviour
{
    public AudioSource audioSource;
    [Header("General")]
    public TreeID treeID = TreeID.Tree;
    private TreeData treeData;
    [ReadOnly] public float health;
    public GameObject maegenWisp;
    public GameObject maegen1;
    public GameObject maegen5;
    public GameObject maegen8;
    public int energyMultiplier;
    private bool runeBuff = false;
    public Animator animator;
    public GameObject chopParticle;

    [Header("Forest Decor")]
    public int amountOfDecor;
    public GameObject[] forestDecor;
    public float spawnRadius;
   
    void Start()
    {
        treeData = _DATA.GetTree(treeID);
        _GAME.trees.Add(gameObject);
        health = treeData.health;
        StartCoroutine(WaitForDecorSpawn());
        transform.rotation = Quaternion.Euler(0, MathX.RandomInt(0, 359), 0);
        _UI.CheckTreeUI();
    }
    IEnumerator WaitForDecorSpawn()
    {
        yield return new WaitForSeconds(5);
        for (int i = 0; i < amountOfDecor; i++)
        {
            ForestDecorSpawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        UnitWeaponCollider uwc = other.GetComponent<UnitWeaponCollider>();
        if (!uwc)
            return;

        var collisionPoint = other.ClosestPoint(transform.position);
        switch (uwc.enemyID)
        {
            case EnemyID.Logger:
            case EnemyID.Lumberjack:
                Chop();
                health -= uwc.Damage;
                GameObject party = Instantiate(chopParticle, collisionPoint, transform.rotation);
                Destroy(party, 2f);
                break;
            case EnemyID.Mine:
            case EnemyID.Dog:
                health = 0;
                break;
        }
        if (health <= 0)
            Die();
        
        if (other.CompareTag("Rune"))
            runeBuff = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rune"))
            runeBuff = false;
    }
    private void OnTriggerStay(Collider other)
    {
        UnitWeaponCollider uwc = other.GetComponent<UnitWeaponCollider>();
        if (!uwc)
            return;

        switch (uwc.enemyID)
        {
            case EnemyID.LogCutter:
                health -= uwc.Damage;
                if (health <= 0)
                    Die();
                break;
        }
    }
    
    private void Chop()
    {
        animator.SetTrigger("Chop");
        AudioX.PlayRandomSound(treeData.hitSounds, audioSource);
        if(_GAME.timeSinceAttack >= 30)
        {
            Vector3 treeLocation = new Vector3(0, 50, 0);
            GameObject ws = Instantiate(_GAME.warningSprite, transform.position + treeLocation, Quaternion.Euler(90f, 0f, 0f));
            Destroy(ws, 5);
        }
        GameEvents.ReportOnTreeHit();
    }
    
    private void Die()
    {
        _GAME.trees.Remove(gameObject);
        GameObject fallTree = Instantiate(treeData.fallModel, transform.position, transform.rotation);
        fallTree.transform.localScale = transform.localScale;
        GameEvents.ReportOnTreeDestroy(treeID);
        Destroy(gameObject);
    }

    public void ForestDecorSpawn()
    {
        Vector3 randomDirection = transform.position + Random.insideUnitSphere * spawnRadius;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, spawnRadius, 1);
        Instantiate(forestDecor[Random.Range(0, forestDecor.Length)], hit.position, Quaternion.Euler(0, Random.Range(0, 359), 0));
    }
    IEnumerator AddMaegen()
    {
        _UI.CheckEldyr();

        if(runeBuff == false)
        {
            _GAME.maegen++;
            Instantiate(maegen1, transform.position, Quaternion.Euler(-90f, 0, 0));
        }
        if(runeBuff == true)
        {
            if(_DATA.HasPerk(PerkID.Rune))
            {
                _GAME.maegen += 8;
                Instantiate(maegen8, transform.position, Quaternion.Euler(-90f, 0, 0));
            }
            else
            {
                _GAME.maegen += 5;
                Instantiate(maegen5, transform.position, Quaternion.Euler(-90f, 0, 0));
            }
        }
        
        yield return null;

        yield return new WaitForSeconds(5 / energyMultiplier);

        Debug.Log(5 / energyMultiplier);
        StartCoroutine(AddMaegen());
    }

    private void OnContinueButton()
    {
        StartCoroutine(WispSpawnDelay());
        //godRays.SetActive(true);
    }
    private IEnumerator WispSpawnDelay()
    {
        for (int i = 0; i < energyMultiplier; i++)
        {
            Instantiate(maegenWisp, transform.position, transform.rotation);
            if (i < energyMultiplier - 1)
            {
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
    
    private void OnTreeUpgrade()
    {
        energyMultiplier *= 2;
    }

    private void OnEnable()
    {
        GameEvents.OnContinueButton += OnContinueButton;
        GameEvents.OnTreeUpgrade += OnTreeUpgrade;
    }

    private void OnDisable()
    {
        GameEvents.OnContinueButton -= OnContinueButton;
        GameEvents.OnTreeUpgrade -= OnTreeUpgrade;
    }
}
