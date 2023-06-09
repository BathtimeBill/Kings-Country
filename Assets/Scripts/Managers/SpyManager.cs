using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class SpyManager : Singleton<SpyManager>
{
    public GameObject spy;
    public float spawnInterval;
    public float placementRadius;
    public GameObject notification;

    void Start()
    {
        StartCoroutine(SpawnSpy());

    }
    public void Respawn()
    {
        Vector3 randomLocation = transform.position + Random.insideUnitSphere * placementRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomLocation, out hit, placementRadius, 1);
        Vector3 finalPosition = hit.position;
        Instantiate(spy, hit.position, transform.rotation);

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
            _UI.SetErrorMessageSpy();
            _PC.Error();
            Instantiate(notification, hit.position, transform.rotation);
            spawnInterval = Random.Range(450, 550);
        }
        if (_GM.currentWave == 2)
        {
            Instantiate(spy, hit.position, transform.rotation);
            _UI.SetErrorMessageSpy();
            _PC.Error();
            Instantiate(notification, hit.position, transform.rotation);
            spawnInterval = Random.Range(400, 500);
        }
        if (_GM.currentWave == 3)
        {
            Instantiate(spy, hit.position, transform.rotation);
            _UI.SetErrorMessageSpy();
            _PC.Error();
            Instantiate(notification, hit.position, transform.rotation);
            spawnInterval = Random.Range(350, 430);
        }
        if (_GM.currentWave == 4)
        {
            Instantiate(spy, hit.position, transform.rotation);
            _UI.SetErrorMessageSpy();
            _PC.Error();
            Instantiate(notification, hit.position, transform.rotation);
            spawnInterval = Random.Range(300, 400);
        }
        if (_GM.currentWave == 5)
        {
            Instantiate(spy, hit.position, transform.rotation);
            _UI.SetErrorMessageSpy();
            _PC.Error();
            Instantiate(notification, hit.position, transform.rotation);
            spawnInterval = Random.Range(250, 350);
        }
        if (_GM.currentWave == 6)
        {
            Instantiate(spy, hit.position, transform.rotation);
            _UI.SetErrorMessageSpy();
            _PC.Error();
            Instantiate(notification, hit.position, transform.rotation);
            spawnInterval = Random.Range(200, 300);
        }
        if (_GM.currentWave == 7)
        {
            Instantiate(spy, hit.position, transform.rotation);
            _UI.SetErrorMessageSpy();
            _PC.Error();
            Instantiate(notification, hit.position, transform.rotation);
            spawnInterval = Random.Range(150, 250);
        }
        if (_GM.currentWave == 8)
        {
            Instantiate(spy, hit.position, transform.rotation);
            _UI.SetErrorMessageSpy();
            _PC.Error();
            Instantiate(notification, hit.position, transform.rotation);
            spawnInterval = Random.Range(100, 200);
        }
        if (_GM.currentWave == 9)
        {
            Instantiate(spy, hit.position, transform.rotation);
            _UI.SetErrorMessageSpy();
            _PC.Error();
            Instantiate(notification, hit.position, transform.rotation);
            spawnInterval = Random.Range(50, 150);
        }
        if (_GM.currentWave == 10)
        {
            Instantiate(spy, hit.position, transform.rotation);
            _UI.SetErrorMessageSpy();
            _PC.Error();
            Instantiate(notification, hit.position, transform.rotation);
            spawnInterval = Random.Range(25, 100);
        }
        yield return new WaitForSeconds(spawnInterval);
        StartCoroutine(SpawnSpy());
    }
}
