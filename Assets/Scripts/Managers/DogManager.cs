using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogManager : GameBehaviour
{
    public GameObject dog;
    public List<GameObject> spawnLocations;
    void Start()
    {
        StartCoroutine(WaitToAssignSpawnLocations());
    }
    IEnumerator WaitToAssignSpawnLocations()
    {
        yield return new WaitForSeconds(0.1f);
        spawnLocations = _EM.spawnPoints;
    }
    int RandomSpawnLocation()
    {
        int i = Random.Range(0, spawnLocations.Count);
        return i;
    }
    private void OnStartNextRound()
    {
        if (_GM.currentWave == 1 && _GM.trees.Count > 5)
        {
            int rndCoin = Random.Range(1, 2);
            if(rndCoin == 1)
            {
                Instantiate(dog, spawnLocations[RandomSpawnLocation()].transform.position, transform.rotation);
            }
        }
        if (_GM.currentWave == 2 && _GM.trees.Count > 10)
        {
            int rndCoin = Random.Range(0, 1);
            if (rndCoin == 1)
            {
                Instantiate(dog, spawnLocations[RandomSpawnLocation()].transform.position, transform.rotation);
            }
        }
        if (_GM.currentWave == 3 && _GM.trees.Count > 15)
        {
            int rndCoin = Random.Range(0, 1);
            if (rndCoin == 1)
            {
                Instantiate(dog, spawnLocations[RandomSpawnLocation()].transform.position, transform.rotation);
            }
        }
        if (_GM.currentWave == 4 && _GM.trees.Count > 20)
        {
            int rndCoin = Random.Range(0, 1);
            if (rndCoin == 1)
            {
                Instantiate(dog, spawnLocations[RandomSpawnLocation()].transform.position, transform.rotation);
            }
        }
        if (_GM.currentWave == 5 && _GM.trees.Count > 20)
        {
            int rndCoin = Random.Range(0, 1);
            if (rndCoin == 1)
            {
                Instantiate(dog, spawnLocations[RandomSpawnLocation()].transform.position, transform.rotation);
            }
        }
        if (_GM.currentWave == 6 && _GM.trees.Count > 20)
        {
            int rndCoin = Random.Range(0, 1);
            if (rndCoin == 1)
            {
                Instantiate(dog, spawnLocations[RandomSpawnLocation()].transform.position, transform.rotation);
            }
        }
    }

    private void OnEnable()
    {
        GameEvents.OnStartNextRound += OnStartNextRound;
    }

    private void OnDisable()
    {
        GameEvents.OnStartNextRound -= OnStartNextRound;
    }
}
