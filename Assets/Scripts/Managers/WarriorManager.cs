using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorManager : GameBehaviour
{
    public Transform[] spawnPoints;

    public GameObject dreng;
    public GameObject berserkr;
    public GameObject knight;

    void Start()
    {
        StartCoroutine(SpawnWarrior());
    }

    IEnumerator SpawnWarrior()
    {
        int rndSpawn = Random.Range(0, spawnPoints.Length);

        if (_GM.currentWave >= 2 && _GM.currentWave < 4 && _GM.agroWave)
        {
            Instantiate(dreng, spawnPoints[rndSpawn].position, transform.rotation);
        }
        if (_GM.currentWave >= 4 && _GM.currentWave < 5 && _GM.agroWave)
        {
            Instantiate(dreng, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(berserkr, spawnPoints[rndSpawn].position, transform.rotation);
        }
        if (_GM.currentWave >= 5 && _GM.currentWave < 7 && _GM.agroWave)
        {
            Instantiate(berserkr, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(dreng, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(berserkr, spawnPoints[rndSpawn].position, transform.rotation);
        }
        if (_GM.currentWave >= 7 && _GM.currentWave < 10 && _GM.agroWave)
        {
            Instantiate(knight, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(berserkr, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(dreng, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(berserkr, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(knight, spawnPoints[rndSpawn].position, transform.rotation);

        }
        if (_GM.currentWave >= 10 && _GM.agroWave)
        {


            Instantiate(berserkr, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(knight, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(dreng, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(knight, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(berserkr, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(dreng, spawnPoints[rndSpawn].position, transform.rotation);

        }
        yield return new WaitForSeconds(Random.Range(15, 25));
        StartCoroutine(SpawnWarrior());
    }
}
