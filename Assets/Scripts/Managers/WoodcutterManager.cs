using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodcutterManager : GameBehaviour
{
    public GameObject logger;
    public GameObject lumberjack;
    public GameObject logCutter;

    public int minCooldown;
    public int maxCooldown;

    void Start()
    {
        StartCoroutine(SpawnWoodcutter());
        //if (_TUTM.isTutorial)
        //{
        //    StartCoroutine(SpawnWoodcutterTutorial());
        //}
        //else
        //{
        //    StartCoroutine(SpawnWoodcutter());
        //}
    }

    //IEnumerator SpawnWoodcutterTutorial()
    //{
    //    int rndSpawn = Random.Range(0, _EM.spawnPoints.Count);
    //    if(_GM.currentWave == 1 && _GM.agroWave)
    //    {
    //        Instantiate(logger, _EM.spawnPoints[rndSpawn].transform.position, transform.rotation);
    //    }
    //    if (_GM.currentWave == 2 && _GM.agroWave)
    //    {
    //        Instantiate(logger, spawnPoints[rndSpawn].position, transform.rotation);
    //        Instantiate(lumberjack, spawnPoints[rndSpawn].position, transform.rotation);
    //    }
    //    if (_GM.currentWave == 3 && _GM.agroWave)
    //    {
    //        Instantiate(logger, spawnPoints[rndSpawn].position, transform.rotation);
    //        Instantiate(logger, spawnPoints[rndSpawn].position, transform.rotation);
    //        Instantiate(lumberjack, spawnPoints[rndSpawn].position, transform.rotation);
    //    }
    //    if (_GM.currentWave == 4 && _GM.agroWave)
    //    {
    //        Instantiate(logger, spawnPoints[rndSpawn].position, transform.rotation);
    //        Instantiate(lumberjack, spawnPoints[rndSpawn].position, transform.rotation);
    //        Instantiate(lumberjack, spawnPoints[rndSpawn].position, transform.rotation);
    //    }
    //    if (_GM.currentWave == 5 && _GM.agroWave)
    //    {
    //        Instantiate(logger, spawnPoints[rndSpawn].position, transform.rotation);
    //        Instantiate(logger, spawnPoints[rndSpawn].position, transform.rotation);
    //        Instantiate(lumberjack, spawnPoints[rndSpawn].position, transform.rotation);
    //        Instantiate(lumberjack, spawnPoints[rndSpawn].position, transform.rotation);

    //    }

    //    yield return new WaitForSeconds(Random.Range(10, 20));
    //    StartCoroutine(SpawnWoodcutter());
    //}
    IEnumerator SpawnWoodcutter()
    {
        yield return new WaitForEndOfFrame();
        SpawnLoop();
        yield return new WaitForSeconds(Random.Range(minCooldown, maxCooldown));
        StartCoroutine(SpawnWoodcutter());
    }


    private void SpawnLoop()
    {
        print("Spawn Loop");
        int rndSpawn = Random.Range(0, _EM.spawnPoints.Count);
        if(_GM.agroPhase)
        {
            
            for (int i = 0; i < _EM.GetHumanDayLimit(HumanID.Logger); i++)
            {
                print("SpawnThisManyLogger");
                Instantiate(logger, _EM.spawnPoints[rndSpawn].transform.position, transform.rotation);
            }
            for (int i = 0; i < _EM.GetHumanDayLimit(HumanID.Lumberjack); i++)
            {
                Instantiate(lumberjack, _EM.spawnPoints[rndSpawn].transform.position, transform.rotation);
            }
        }
    }
}
