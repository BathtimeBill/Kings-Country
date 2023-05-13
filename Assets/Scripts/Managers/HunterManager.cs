using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterManager : GameBehaviour
{
    public Transform[] spawnPoints;

    public GameObject wathe;
    public GameObject hunter;
    public GameObject bjornjeger;

    void Start()
    {
        StartCoroutine(SpawnHunter());
    }

    IEnumerator SpawnHunter()
    {
        int rndSpawn = Random.Range(0, spawnPoints.Length);
        if (_GM.currentWave > 0 && _GM.currentWave < 3 && _GM.agroWave)
        {
            Instantiate(wathe, spawnPoints[rndSpawn].position, transform.rotation);
        }
        if (_GM.currentWave >= 3 && _GM.currentWave < 5 && _GM.agroWave)
        {
            Instantiate(wathe, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(hunter, spawnPoints[rndSpawn].position, transform.rotation);
        }
        if (_GM.currentWave >= 5 && _GM.currentWave < 7 && _GM.agroWave)
        {
            Instantiate(wathe, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(wathe, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(hunter, spawnPoints[rndSpawn].position, transform.rotation);
        }
        if (_GM.currentWave >= 7 && _GM.currentWave < 10 && _GM.agroWave)
        {
            Instantiate(wathe, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(hunter, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(wathe, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(hunter, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(bjornjeger, spawnPoints[rndSpawn].position, transform.rotation);
        }
        if (_GM.currentWave >= 10 && _GM.agroWave)
        {
            Instantiate(wathe, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(wathe, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(hunter, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(bjornjeger, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(bjornjeger, spawnPoints[rndSpawn].position, transform.rotation);
            Instantiate(bjornjeger, spawnPoints[rndSpawn].position, transform.rotation);
        }
        yield return new WaitForSeconds(Random.Range(15, 25));
        StartCoroutine(SpawnHunter());
    }
}
