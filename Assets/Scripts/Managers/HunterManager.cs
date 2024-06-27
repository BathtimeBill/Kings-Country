using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterManager : GameBehaviour
{
    public GameObject wathe;
    public GameObject hunter;
    public GameObject bjornjeger;

    public int minCooldown;
    public int maxCooldown;

    void Start()
    {
        StartCoroutine(SpawnWoodcutter());
    }

    IEnumerator SpawnWoodcutter()
    {
        yield return new WaitForEndOfFrame();
        SpawnLoop();
        yield return new WaitForSeconds(Random.Range(minCooldown, maxCooldown));
        StartCoroutine(SpawnWoodcutter());
    }


    private void SpawnLoop()
    {
        int rndSpawn = Random.Range(0, _EM.spawnPoints.Count);
        if (_GM.agroWave)
        {
            for (int i = 0; i < _EM.GetHumanDayLimit(HumanID.Wathe); i++)
            {
                Instantiate(wathe, _EM.spawnPoints[rndSpawn].transform.position, transform.rotation);
            }
            for (int i = 0; i < _EM.GetHumanDayLimit(HumanID.Hunter); i++)
            {
                Instantiate(hunter, _EM.spawnPoints[rndSpawn].transform.position, transform.rotation);
            }
            for (int i = 0; i < _EM.GetHumanDayLimit(HumanID.Bjornjeger); i++)
            {
                Instantiate(bjornjeger, _EM.spawnPoints[rndSpawn].transform.position, transform.rotation);
            }
        }
    }
}
