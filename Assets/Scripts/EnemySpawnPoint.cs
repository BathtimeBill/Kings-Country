using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : GameBehaviour
{
    private void Start()
    {
        _EM.spawnPoints.Add(gameObject);
    }
}
