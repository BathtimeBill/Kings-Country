using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodcutterManager : GameBehaviour
{
    public Transform[] spawnPoints;

    public GameObject logger;
    public GameObject lumberjack;
    public GameObject logCutter;



    void Start()
    {
        if(_TUTM.isTutorial)
        {
            StartCoroutine(SpawnWoodcutterTutorial());
        }
        else
        {
            StartCoroutine(SpawnWoodcutter());
        }
    }

    IEnumerator SpawnWoodcutterTutorial()
    {
        int rndSpawn = Random.Range(0, spawnPoints.Length);
        if(_GM.currentWave == 1 && _GM.agroWave)
        {
            Instantiate(logger, spawnPoints[rndSpawn].position, transform.rotation);
        }
        if (_GM.currentWave == 2 && _GM.agroWave)
        {
            Instantiate(logger, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(lumberjack, spawnPoints[rndSpawn].position, transform.rotation);
        }
        if (_GM.currentWave == 3 && _GM.agroWave)
        {
            Instantiate(logger, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(logger, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(lumberjack, spawnPoints[rndSpawn].position, transform.rotation);
        }
        if (_GM.currentWave == 4 && _GM.agroWave)
        {
            Instantiate(logger, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(lumberjack, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(lumberjack, spawnPoints[rndSpawn].position, transform.rotation);
        }
        if (_GM.currentWave == 5 && _GM.agroWave)
        {
            Instantiate(logger, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(logger, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(lumberjack, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(lumberjack, spawnPoints[rndSpawn].position, transform.rotation);

        }

        yield return new WaitForSeconds(Random.Range(10, 20));
        StartCoroutine(SpawnWoodcutter());
    }
    IEnumerator SpawnWoodcutter()
    {
        int rndSpawn = Random.Range(0, spawnPoints.Length);
        if (_GM.currentWave > 0 && _GM.currentWave < 3 && _GM.agroWave)
        {
            Instantiate(logger, spawnPoints[rndSpawn].position, transform.rotation);
        }
        if (_GM.currentWave >= 3 && _GM.currentWave < 5 && _GM.agroWave)
        {
            Instantiate(logger, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(lumberjack, spawnPoints[rndSpawn].position, transform.rotation);
        }
        if (_GM.currentWave >= 5 && _GM.currentWave < 7 && _GM.agroWave)
        {
            Instantiate(logger, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(lumberjack, spawnPoints[rndSpawn].position, transform.rotation);
        }
        if (_GM.currentWave >= 7 && _GM.currentWave < 10 && _GM.agroWave)
        {
            Instantiate(logger, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(logger, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(lumberjack, spawnPoints[rndSpawn].position, transform.rotation);
        }
        if (_GM.currentWave >= 10 && _GM.agroWave)
        {
            Instantiate(logger, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(logger, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(lumberjack, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(lumberjack, spawnPoints[rndSpawn].position, transform.rotation);

        }

        yield return new WaitForSeconds(Random.Range(5, 10));
        StartCoroutine(SpawnWoodcutterTutorial());
    }

}
