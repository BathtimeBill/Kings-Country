using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
public class Tree : GameBehaviour
{
    public AudioSource audioSource;
    public bool startingTree;

    [Header("General")]
    public TreeType type;
    public float health;
    public GameObject maegen1;
    public GameObject maegen5;
    public GameObject maegen8;
    public GameObject fallenTreePine;
    public GameObject fallenTreeDesiduous;
    public GameObject treeMesh;
    public float energyMultiplier;
    public bool runeBuff = false;
    public Animator animator;
    public GameObject chopParticle;

    [Header("Forest Decor")]
    public int amountOfDecor;
    public GameObject[] forestDecor;
    public float spawnRadius;
    public GameObject[] meshes;


 
    void Start()
    {
        DecideType();
        int amount;
        health = 100;
        
        StartCoroutine(AddMaegen());
        for (amount = 0; amount < amountOfDecor; amount++)
        {
            ForestDecorSpawn();
        }
        if(startingTree)
        {
            energyMultiplier = 1;
        }
        else
        {
            //audioSource.Play();
            //energyMultiplier = _TPlace.energyMultiplier;
        }
        transform.rotation = Quaternion.Euler(0, RandomYAxisRotation(), 0);
        _UI.CheckTreeUI();
    }
    private int RandomYAxisRotation()
    {
        int rndRotation;
        rndRotation = Random.Range(0, 359);
        return rndRotation;
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

    private void DecideType()
    {
        int rnd = Random.Range(0, meshes.Length);
        if(rnd == 0)
        {
            type = TreeType.Pine;
            
        }
        if(rnd == 1)
        {
            type = TreeType.Deciduous;
        }
        meshes[rnd].SetActive(true);
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
        GameObject fallTree;
        if (type == TreeType.Pine)
        {
            fallTree = Instantiate(fallenTreePine, treeMesh.transform.position, transform.rotation);
            fallTree.transform.localScale = transform.localScale;
        }
        if(type == TreeType.Deciduous)
        {
            fallTree = Instantiate(fallenTreeDesiduous, treeMesh.transform.position, transform.rotation);
            fallTree.transform.localScale = transform.localScale;
        }
        
        
        
        GameEvents.ReportOnTreeDestroy();
        Destroy(gameObject);
    }

    public void ForestDecorSpawn()
    {
        int rndForestSpawn = Random.Range(0, forestDecor.Length);
        Vector3 randomDirection = transform.position + Random.insideUnitSphere * spawnRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, 1);
        Vector3 finalPosition = hit.position;
        Instantiate(forestDecor[rndForestSpawn], hit.position, transform.rotation);
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
            if(_UM.rune)
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
}
