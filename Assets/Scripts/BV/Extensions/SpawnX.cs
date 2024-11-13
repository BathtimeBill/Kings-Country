using UnityEngine;
using UnityEngine.AI;

public static class SpawnX
{
    static public Vector3 GetSpawnPositionInRadius(Vector3 _spawnOrigin, float _spawnRadius)
    {
        Vector3 randomLocation = _spawnOrigin + Random.insideUnitSphere * _spawnRadius;
        NavMesh.SamplePosition(randomLocation, out NavMeshHit hit, _spawnRadius, 1);
        return hit.position;
    }
    
    static public Vector3 GetSpawnPositionOnLevel()
    {
        float spawnRadius = 250;
        Vector3 randomLocation = Vector3.zero + Random.insideUnitSphere * spawnRadius;
        NavMesh.SamplePosition(randomLocation, out NavMeshHit hit, spawnRadius, 1);
        return hit.position;
    }
}
