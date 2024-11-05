using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorgrManager : Singleton<HorgrManager>
{
    public List<GameObject> enemies;
    public List<Unit> units;

    public List<GameObject> spawnLocations;
    public GameObject horgr;
    public GameObject horgrObject;
    public GameObject spawnParticle;
    public GameObject enemySpawnParticle;
    public Vector3 horgrLocation;

    public GameObject spawnLocation;

    public GameObject skessa;
    public GameObject huldra;
    public GameObject golem;

    public GameObject maegen5;
    public GameObject maegenLoss;

    public bool playerOwns;
    public bool enemyOwns;
    public bool hasBeenClaimed;
    [Header("EnemyUnits")]
    public GameObject dreng;
    public GameObject berserkr;
    public GameObject knight;
    public float minSpawnRate;
    public float maxSpawnRate;

    private new void Awake()
    {
        //horgrLocation = go.transform.position;
        //horgrObject = GameObject.FindGameObjectWithTag("Horgr");
        //spawnLocation = horgrObject.GetComponent<Horgr>().spawnLocation;
    }

    void Start()
    {
        //GameObject go = Instantiate(horgr, spawnLocations[RandomSpawnLocation()].transform.position, Quaternion.Euler(-90, 180, 0));
        //StartCoroutine(AddMaegen());
        //StartCoroutine(WaitToReferenceHorgr());
        StartCoroutine(SpawnEnemyUnits());
    }

    IEnumerator SpawnEnemyUnits()
    {
        yield return new WaitForSeconds(Random.Range(minSpawnRate, maxSpawnRate));
        if (enemyOwns)
        {
            GameObject go = Instantiate(knight, spawnLocation.transform.position, Quaternion.Euler(0, 0, 0));
            Instantiate(enemySpawnParticle, spawnLocation.transform.position, Quaternion.Euler(0, 0, 0));
            go.GetComponent<Warrior>().spawnedFromBuilding = true;
            //_EM.enemies.Add(go);
        }
        StartCoroutine(SpawnEnemyUnits());
    }
    private int RandomSpawnLocation()
    {
        int rndSpn = Random.Range(0, spawnLocations.Count);
        return rndSpn;
    }

    IEnumerator AddMaegen()
    {
        if(playerOwns)
        {
            _GM.maegen += 2;
            Instantiate(maegen5, horgrLocation, Quaternion.Euler(-90f, 0, 0));
        }
        if(enemyOwns)
        {
            _GM.maegen -= 2;
            Instantiate(maegenLoss, horgrLocation, Quaternion.Euler(-90f, 0, 0));
        }

        yield return new WaitForSeconds(Random.Range(20, 30));

        StartCoroutine(AddMaegen());
    }
    IEnumerator WaitToReferenceHorgr()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Attempting to assign Horgr object...");
        //horgrObject = GameObject.FindGameObjectWithTag("Horgr");
        //spawnLocation = horgrObject.GetComponent<Horgr>().spawnLocation;
        //transform.position = spawnLocation.transform.position;
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
