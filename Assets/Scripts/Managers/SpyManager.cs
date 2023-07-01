using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class SpyManager : GameBehaviour
{
    public GameObject spy;
    public GameObject spawnParticle;
    public float spawnInterval;
    public float placementRadius;

    void Start()
    {
        StartCoroutine(SpawnSpy());
    }

    IEnumerator SpawnSpy()
    {
        Vector3 randomLocation = transform.position + Random.insideUnitSphere * placementRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomLocation, out hit, placementRadius, 1);
        Vector3 finalPosition = hit.position;

        if (_GM.currentWave == 0)
        {
            spawnInterval = Random.Range(500, 600);
        }
        if (_GM.currentWave == 1)
        {
            Instantiate(spy, hit.position, transform.rotation);
            Instantiate(spawnParticle, hit.position, transform.rotation);
            spawnInterval = Random.Range(430, 530);
        }
        if (_GM.currentWave == 2)
        {
            Instantiate(spy, hit.position, transform.rotation);
            Instantiate(spawnParticle, hit.position, transform.rotation);
            spawnInterval = Random.Range(400, 500);
        }
        if (_GM.currentWave == 3)
        {
            Instantiate(spy, hit.position, transform.rotation);
            Instantiate(spawnParticle, hit.position, transform.rotation);
            spawnInterval = Random.Range(350, 430);
        }
        if (_GM.currentWave == 4)
        {
            Instantiate(spy, hit.position, transform.rotation);
            Instantiate(spawnParticle, hit.position, transform.rotation);
            spawnInterval = Random.Range(300, 400);
        }
        if (_GM.currentWave == 5)
        {
            Instantiate(spy, hit.position, transform.rotation);
            Instantiate(spawnParticle, hit.position, transform.rotation);
            spawnInterval = Random.Range(250, 350);
        }
        if (_GM.currentWave == 6)
        {
            Instantiate(spy, hit.position, transform.rotation);
            Instantiate(spawnParticle, hit.position, transform.rotation);
            spawnInterval = Random.Range(200, 300);
        }
        if (_GM.currentWave == 7)
        {
            Instantiate(spy, hit.position, transform.rotation);
            Instantiate(spawnParticle, hit.position, transform.rotation);
            spawnInterval = Random.Range(150, 250);
        }
        if (_GM.currentWave == 8)
        {
            Instantiate(spy, hit.position, transform.rotation);
            Instantiate(spawnParticle, hit.position, transform.rotation);
            spawnInterval = Random.Range(100, 200);
        }
        if (_GM.currentWave == 9)
        {
            Instantiate(spy, hit.position, transform.rotation);
            Instantiate(spawnParticle, hit.position, transform.rotation);
            spawnInterval = Random.Range(50, 150);
        }
        if (_GM.currentWave == 10)
        {
            Instantiate(spy, hit.position, transform.rotation);
            Instantiate(spawnParticle, hit.position, transform.rotation);
            spawnInterval = Random.Range(25, 100);
        }
        yield return new WaitForSeconds(spawnInterval);
        StartCoroutine(SpawnSpy());
    }
}
