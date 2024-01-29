using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class WarriorManager : GameBehaviour
{
    public GameObject dreng;
    public GameObject berserkr;
    public GameObject knight;

    public int minCooldown;
    public int maxCooldown;
    void Start()
    {
        StartCoroutine(SpawnWarrior());
    }

    IEnumerator SpawnWarrior()
    {
        yield return new WaitForEndOfFrame();
        SpawnLoop();
        yield return new WaitForSeconds(Random.Range(minCooldown, maxCooldown));
        StartCoroutine(SpawnWarrior());
    }


    private void SpawnLoop()
    {
        int rndSpawn = Random.Range(0, _EM.spawnPoints.Count);
        if (_GM.agroWave)
        {
            for (int i = 0; i < _EM.drengAmount; i++)
            {
                Instantiate(dreng, _EM.spawnPoints[rndSpawn].transform.position, transform.rotation);
            }
            for (int i = 0; i < _EM.bezerkerAmount; i++)
            {
                Instantiate(berserkr, _EM.spawnPoints[rndSpawn].transform.position, transform.rotation);
            }
            for (int i = 0; i < _EM.knightAmount; i++)
            {
                Instantiate(knight, _EM.spawnPoints[rndSpawn].transform.position, transform.rotation);
            }
        }
    }
}
