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
        if(_GM.level != LevelNumber.One)
        {
            StartCoroutine(SpawnSpy());
        }
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

        if (_GM.currentDay == 0)
        {
            spawnInterval = Random.Range(500, 600);

        }
        if (_GM.currentDay == 1)
        {
            Instantiate(spy, hit.position, transform.rotation);
            _UI.SetError(ErrorID.SpyClose);
            Instantiate(notification, hit.position, transform.rotation);
            spawnInterval = Random.Range(450, 550);
        }
        if (_GM.currentDay == 2)
        {
            Instantiate(spy, hit.position, transform.rotation);
            _UI.SetError(ErrorID.SpyClose);
            Instantiate(notification, hit.position, transform.rotation);
            spawnInterval = Random.Range(400, 500);
        }
        if (_GM.currentDay == 3)
        {
            Instantiate(spy, hit.position, transform.rotation);
            _UI.SetError(ErrorID.SpyClose);
            Instantiate(notification, hit.position, transform.rotation);
            spawnInterval = Random.Range(350, 430);
        }
        if (_GM.currentDay == 4)
        {
            Instantiate(spy, hit.position, transform.rotation);
            _UI.SetError(ErrorID.SpyClose);
            Instantiate(notification, hit.position, transform.rotation);
            spawnInterval = Random.Range(300, 400);
        }
        if (_GM.currentDay == 5)
        {
            Instantiate(spy, hit.position, transform.rotation);
            _UI.SetError(ErrorID.SpyClose);
            Instantiate(notification, hit.position, transform.rotation);
            spawnInterval = Random.Range(250, 350);
        }
        if (_GM.currentDay == 6)
        {
            Instantiate(spy, hit.position, transform.rotation);
            _UI.SetError(ErrorID.SpyClose);
            Instantiate(notification, hit.position, transform.rotation);
            spawnInterval = Random.Range(200, 300);
        }
        if (_GM.currentDay == 7)
        {
            Instantiate(spy, hit.position, transform.rotation);
            _UI.SetError(ErrorID.SpyClose);
            Instantiate(notification, hit.position, transform.rotation);
            spawnInterval = Random.Range(150, 250);
        }
        if (_GM.currentDay == 8)
        {
            Instantiate(spy, hit.position, transform.rotation);
            _UI.SetError(ErrorID.SpyClose);
            Instantiate(notification, hit.position, transform.rotation);
            spawnInterval = Random.Range(100, 200);
        }
        if (_GM.currentDay == 9)
        {
            Instantiate(spy, hit.position, transform.rotation);
            _UI.SetError(ErrorID.SpyClose);
            Instantiate(notification, hit.position, transform.rotation);
            spawnInterval = Random.Range(50, 150);
        }
        if (_GM.currentDay == 10)
        {
            Instantiate(spy, hit.position, transform.rotation);
            _UI.SetError(ErrorID.SpyClose);
            Instantiate(notification, hit.position, transform.rotation);
            spawnInterval = Random.Range(25, 100);
        }
        yield return new WaitForSeconds(spawnInterval);
        StartCoroutine(SpawnSpy());
    }
}
