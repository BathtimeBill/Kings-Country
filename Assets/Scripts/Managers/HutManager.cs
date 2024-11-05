using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HutManager : Singleton<HutManager>
{
    public List<GameObject> enemies;
    public List<Unit> units;

    public List<GameObject> spawnLocations;
    public GameObject hut;
    public GameObject spawnLocation;

    public bool playerOwns;
    public bool enemyOwns;
    public bool hasBeenClaimed;

    [Header("EnemyUnits")]
    public GameObject wathe;
    public GameObject hunter;
    public GameObject bjornjegger;
    public float minSpawnRate;
    public float maxSpawnRate;
    
    void Start()
    {
        StartCoroutine(SpawnEnemyUnits());
    }
    
    IEnumerator SpawnEnemyUnits()
    {
        yield return new WaitForSeconds(Random.Range(minSpawnRate, maxSpawnRate));
        if (enemyOwns)
        {
            _EM.SpawnHutEnemy(spawnLocation.transform.position);
        }
        StartCoroutine(SpawnEnemyUnits());
    }
    
    void OnContinueButton()
    {
        playerOwns = true;
        enemyOwns = false;
    }
    
    private void OnHumanKilled(Enemy _enemy, string _killer)
    {
        if (enemies.Contains(_enemy.gameObject))
            enemies.Remove(_enemy.gameObject);
    }

    private void OnEnable()
    {
        GameEvents.OnContinueButton += OnContinueButton;
        GameEvents.OnHumanKilled += OnHumanKilled;
    }

    private void OnDisable()
    {
        GameEvents.OnContinueButton -= OnContinueButton;
        GameEvents.OnHumanKilled -= OnHumanKilled;
    }
}
