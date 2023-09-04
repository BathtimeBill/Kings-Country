using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class ForestManager : Singleton<ForestManager>
{
    [Header("General")]
    public float rockPlacementRadius;
    public GameObject levelRocks;
    public int amountOfRocks;
    public int totalTrees;
    public float numberOfWildlifeToSpawn;
    [Header("Animals")]
    public GameObject rabbit;
    public GameObject deer;
    public GameObject boar;
    public List<GameObject> spawnableAnimals;
    [Header("Wildlife Spawning")]
    public int minSpawnRate;
    public int maxSpawnRate;
    public float wildlifeSpawnRadius;
    public GameObject wildlifeSpawnParticle;
    public Vector3 spawnLocation;


    void Start()
    {
        //int amount;
        spawnLocation = new Vector3(5, 0, 5);
        CheckTreesForWildlife();
        WildlifeInstantiate();
        _GM.wildlife = CheckWildlife();
        //for (amount = 0; amount < amountOfRocks; amount++)
        //{
        //    SpawnRocks();
        //}
    }

    //This function checks how many animals are in the scene.
    public int CheckWildlife()
    {
        int wildlife = GameObject.FindGameObjectsWithTag("Wildlife").Length;
        return wildlife;
    }

    //This checks how many trees are in the scene and adjusts the spawn rate of the wildlife accordingly.
    public void CheckTreesForWildlife()
    {
        if (_GM.wildlife < 6)
        {
            if (spawnableAnimals.Count == 0)
            {
                spawnableAnimals.Add(rabbit);
            }
        }
        if (_GM.wildlife >= 6 && _GM.wildlife < 15)
        {
            if (spawnableAnimals.Count == 1)
            {
                spawnableAnimals.Add(deer);
            }
        }
        if (_GM.wildlife >= 15)
        {
            if (spawnableAnimals.Count == 2)
            {
                spawnableAnimals.Add(boar);
            }
        }
    }

    //This finds a random location within a sphere from the home tree and spawns an animal there.
    public void WildlifeInstantiate()
    {
        int rnd = Random.Range(0, spawnableAnimals.Count);
        Vector3 randomLocation = transform.position + Random.insideUnitSphere * wildlifeSpawnRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomLocation, out hit, wildlifeSpawnRadius, 1);
        Vector3 finalPosition = hit.position;
        Instantiate(spawnableAnimals[rnd], hit.position, transform.rotation);
        Instantiate(wildlifeSpawnParticle, hit.position, transform.rotation);
    }

    //At the start of the game, rocks are spawned in procedurally with a random scale and rotation.
    private void SpawnRocks()
    {
        Vector3 randomLocation = transform.position + Random.insideUnitSphere * rockPlacementRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomLocation, out hit, rockPlacementRadius, 1);
        Vector3 finalPosition = hit.position;
        GameObject rock;
        rock = Instantiate(levelRocks, hit.position, transform.rotation);
        float rndScale = Random.Range(1, 6);
        rock.transform.rotation = Random.rotation;
        rock.transform.localScale = Vector3.one * rndScale;
    }

    //This coroutine spawns different animals in based on how many animals are already in the scene. Starting with rabbits, then deer, and finally, boar. It loops for the duration of the game.
    IEnumerator SpawnWildlife()
    {
        CheckTreesForWildlife();
        

        if (_GM.wildlife < 6)
        {
            if(spawnableAnimals.Count == 0)
            {
                spawnableAnimals.Add(rabbit);
            }
            WildlifeInstantiate();
            _GM.wildlife = CheckWildlife();
        }
        if(_GM.wildlife >= 6 && _GM.wildlife < 15)
        {
            if (spawnableAnimals.Count == 1)
            {
                spawnableAnimals.Add(deer);
            }
            WildlifeInstantiate();
            _GM.wildlife = CheckWildlife();
        }
        if (_GM.wildlife >= 15)
        {
            if (spawnableAnimals.Count == 2)
            {
                spawnableAnimals.Add(boar);
            }
            WildlifeInstantiate();
            _GM.wildlife = CheckWildlife();
        }

        _UI.CheckWildlifeUI();
        if(_UM.fertileSoil)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnRate / 2, maxSpawnRate / 2));
        }
        else
        {
            yield return new WaitForSeconds(Random.Range(minSpawnRate, maxSpawnRate));
        }

        StartCoroutine(SpawnWildlife());
    }

    //After an animal is killed, this waits until the end of the frame to update the UI.
    private void OnWildlifeKilled()
    {
        StartCoroutine(WaitToCheckWildlife());
    }

    IEnumerator WaitToCheckWildlife()
    {
        yield return new WaitForSeconds(0.1f);
        _GM.wildlife = CheckWildlife();
        _UI.CheckWildlifeUI();
    }


    private void OnContinueButton()
    {
        for (int i = 0; i < numberOfWildlifeToSpawn; i++)
        {
            CheckTreesForWildlife();
            WildlifeInstantiate();
        }
        _GM.wildlife = CheckWildlife();
        StartCoroutine(WaitToCheckWildlife());
    }
    private void OnWaveOver()
    {
        totalTrees = _GM.trees.Count;
        if (_UM.fertileSoil)
        {
            numberOfWildlifeToSpawn = totalTrees / 3f;
        }
        else
        {
            numberOfWildlifeToSpawn = totalTrees / 5f;
        }
        
        numberOfWildlifeToSpawn = Mathf.Round(numberOfWildlifeToSpawn * 1) / 1;
    }

    private void OnEnable()
    {
        GameEvents.OnWildlifeKilled += OnWildlifeKilled;
        GameEvents.OnContinueButton += OnContinueButton;
        GameEvents.OnWaveOver += OnWaveOver;
    }

    private void OnDisable()
    {
        GameEvents.OnWildlifeKilled -= OnWildlifeKilled;
        GameEvents.OnContinueButton -= OnContinueButton;
        GameEvents.OnWaveOver -= OnWaveOver;
    }
}
