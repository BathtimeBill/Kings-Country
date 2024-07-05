using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class PickupManager : GameBehaviour
{
    public float placementRadius;
    public GameObject[] pickups;
    void Start()
    {
        StartCoroutine(SpawnPickups());
    }

    IEnumerator SpawnPickups()
    {
        if(!_buildPhase || !_inTutorial)
        {
            SpawnPickup();
        }
        yield return new WaitForSeconds(Random.Range(20, 50));
        StartCoroutine(SpawnPickups());
    }

    private void SpawnPickup()
    {
        int rndPickup = Random.Range(0, pickups.Length);
        Vector3 randomLocation = transform.position + Random.insideUnitSphere * placementRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomLocation, out hit, placementRadius, 1);
        Vector3 finalPosition = hit.position;
        GameObject pickup;
        pickup = Instantiate(pickups[rndPickup], hit.position, transform.rotation);
        Destroy(pickup, 45);
    }
}
