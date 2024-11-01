using System.Collections;
using UnityEngine.AI;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Serialization;

public class Tree : GameBehaviour
{
    public AudioSource audioSource;
    [FormerlySerializedAs("_treeID")] [Header("General")]
    public ToolID treeID = ToolID.Tree;
    public float health;
    public GameObject maegenWisp;
    public GameObject maegen1;
    public GameObject maegen5;
    public GameObject maegen8;
    public GameObject fallenTreePine;
    public GameObject treeMesh;
    public int energyMultiplier;
    public bool runeBuff = false;
    public Animator animator;
    public GameObject chopParticle;

    [Header("Forest Decor")]
    public int amountOfDecor;
    public GameObject[] forestDecor;
    public float spawnRadius;
   
    void Start()
    {
        _GM.trees.Add(gameObject);
        health = 100;
        StartCoroutine(WaitForDecorSpawn());
        transform.rotation = Quaternion.Euler(0, MathX.RandomInt(0, 359), 0);
        _UI.CheckTreeUI();
    }
    IEnumerator WaitForDecorSpawn()
    {
        yield return new WaitForSeconds(5);
        for (int amount = 0; amount < amountOfDecor; amount++)
        {
            ForestDecorSpawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Axe1")
        {
            ChopSound();
            health -= 15;
            //animator.SetTrigger("Chop");
            if(health <= 0)
            {
                Die();
            }
        }
        if (other.tag == "Axe2")
        {
            ChopSound();
            health -= 35;
            Instantiate(chopParticle, transform.position, transform.rotation);
            //animator.SetTrigger("Chop");
            if (health <= 0)
            {
                Die();
            }
        }
        if (other.tag == "Rune")
        {
            runeBuff = true;
        }
        if(other.tag == "Mine")
        {
            Die();
        }
        if(other.tag == "Explosion3")
        {
            Die();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Rune")
        {
            runeBuff = false;
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Axe3")
        {
            health -= 0.5f;
            if (health <= 0)
            {
                Die();
            }
        }
    }
    
    private void ChopSound()
    {
        audioSource.clip = _SM.GetChopSounds();
        audioSource.Play();
        if(_GM.timeSinceAttack >= 30)
        {
            Vector3 treeLocation = new Vector3(0, 50, 0);
            GameObject ws = Instantiate(_GM.warningSprite, transform.position + treeLocation, Quaternion.Euler(90f, 0f, 0f));
            Destroy(ws, 5);
        }
        GameEvents.ReportOnTreeHit();
    }
    
    private void Die()
    {
        _GM.trees.Remove(gameObject);
        GameObject fallTree = Instantiate(fallenTreePine, treeMesh.transform.position, transform.rotation);
        fallTree.transform.localScale = transform.localScale * 1.3f;
        GameEvents.ReportOnTreeDestroy(treeID);
        Destroy(gameObject);
    }

    public void ForestDecorSpawn()
    {
        float rndRotation = Random.Range(0, 359);
        int rndForestSpawn = Random.Range(0, forestDecor.Length);
        Vector3 randomDirection = transform.position + Random.insideUnitSphere * spawnRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, 1);
        Vector3 finalPosition = hit.position;
        GameObject go = Instantiate(forestDecor[rndForestSpawn], hit.position, Quaternion.Euler(0, rndRotation, 0));
    }
    IEnumerator AddMaegen()
    {
        _UI.CheckEldyr();

        if(runeBuff == false)
        {
            _GM.maegen++;
            Instantiate(maegen1, transform.position, Quaternion.Euler(-90f, 0, 0));
        }
        if(runeBuff == true)
        {
            if(_DATA.HasPerk(PerkID.Rune))
            {
                _GM.maegen += 8;
                Instantiate(maegen8, transform.position, Quaternion.Euler(-90f, 0, 0));
            }
            else
            {
                _GM.maegen += 5;
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
        //int i = 0;
        //while (i < energyMultiplier)
        //{
        //    Instantiate(maegenWisp, transform.position, transform.rotation);
        //    i++;
        //}
    }
    IEnumerator WispSpawnDelay()
    {
        if (energyMultiplier == 1)
        {
            Instantiate(maegenWisp, transform.position, transform.rotation);
            yield return new WaitForSeconds(0.3f);
        }
        if(energyMultiplier == 2)
        {
            Instantiate(maegenWisp, transform.position, transform.rotation);
            yield return new WaitForSeconds(0.3f);
            Instantiate(maegenWisp, transform.position, transform.rotation);
        }
        if (energyMultiplier == 3)
        {
            Instantiate(maegenWisp, transform.position, transform.rotation);
            yield return new WaitForSeconds(0.3f);
            Instantiate(maegenWisp, transform.position, transform.rotation);
            yield return new WaitForSeconds(0.3f);
            Instantiate(maegenWisp, transform.position, transform.rotation);
        }
    }

    private void OnTreeUpgrade()
    {
        energyMultiplier = energyMultiplier * 2;
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
