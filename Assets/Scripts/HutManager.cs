using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HutManager : Singleton<HutManager>
{

    public List<GameObject> enemies;
    public List<GameObject> units;

    public List<GameObject> spawnLocations;
    public GameObject hut;
    public GameObject hutObject;
    public GameObject spawnParticle;
    public Vector3 hutLocation;

    public GameObject spawnLocation;

    public GameObject skessa;
    public GameObject huldra;
    public GameObject goblin;
    public GameObject fidhain;

    public GameObject maegen5;
    public GameObject maegenLoss;

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
        StartCoroutine(WaitToAssignSpawnPoints());
        //GameObject go = Instantiate(hut, spawnLocations[RandomSpawnLocation()].transform.position, Quaternion.Euler(0, 183.4f, 0));
        StartCoroutine(SpawnEnemyUnits());
    }
    IEnumerator WaitToAssignSpawnPoints()
    {
        yield return new WaitForEndOfFrame();
        GameObject go = Instantiate(hut, spawnLocations[RandomSpawnLocation()].transform.position, Quaternion.Euler(0, 183.4f, 0));
    }
    private int RandomSpawnLocation()
    {
        int rndSpn = Random.Range(0, spawnLocations.Count);
        return rndSpn;
    }

    //IEnumerator AddMaegen()
    //{
    //    if (playerOwns)
    //    {
    //        _GM.maegen += 25;
    //        Instantiate(maegen5, hutLocation, Quaternion.Euler(-90f, 0, 0));
    //    }
    //    if (enemyOwns)
    //    {
    //        _GM.maegen -= 25;
    //        Instantiate(maegenLoss, hutLocation, Quaternion.Euler(-90f, 0, 0));
    //    }

    //    yield return new WaitForSeconds(Random.Range(20, 30));

    //    StartCoroutine(AddMaegen());
    //}

    IEnumerator SpawnEnemyUnits()
    {
        yield return new WaitForSeconds(Random.Range(minSpawnRate, maxSpawnRate));
        if (enemyOwns)
        {
            GameObject go = Instantiate(bjornjegger, spawnLocation.transform.position, Quaternion.Euler(0, 0, 0));
            go.GetComponent<Hunter>().spawnedFromBuilding = true;
        }
        StartCoroutine(SpawnEnemyUnits());
    }

    public void SpawnSkessaManager()
    {
        hutObject = GameObject.FindGameObjectWithTag("Hut");
        if (playerOwns)
        {
            if (_GM.maegen >= _GM.skessaPrice && _GM.populous < _GM.maxPopulous)
            {
                Instantiate(skessa, spawnLocation.transform.position, Quaternion.Euler(0, 0, 0));
                Instantiate(spawnParticle, spawnLocation.transform.position, Quaternion.Euler(-90, 0, 0));
                _GM.maegen -= _GM.skessaPrice;
                _UI.CheckPopulousUI();
            }
            else
            {
                _UI.SetErrorMessageInsufficientResources();
                _PC.Error();

            }
        }
        else
        {
            _UI.SetErrorMessageNeedToClaimHorgr();
            _PC.Error();
        }
    }
    public void SpawnGoblinManager()
    {
        if (playerOwns)
        {
            if (_GM.maegen >= _GM.goblinPrice && _GM.populous < _GM.maxPopulous)
            {
                Instantiate(goblin, spawnLocation.transform.position, Quaternion.Euler(0, 0, 0));
                Instantiate(spawnParticle, spawnLocation.transform.position, Quaternion.Euler(-90, 0, 0));
                _GM.maegen -= _GM.goblinPrice;
                _UI.CheckPopulousUI();
            }
            else
            {
                _UI.SetErrorMessageInsufficientResources();
                _PC.Error();
            }
        }
        else
        {
            _UI.SetErrorMessageNeedToClaimHorgr();
            _PC.Error();
        }
    }
    public void SpawnFidhainManager()
    {
        if (playerOwns)
        {
            if (_GM.maegen >= _GM.dryadPrice && _GM.populous < _GM.maxPopulous)
            {
                Instantiate(fidhain, spawnLocation.transform.position, Quaternion.Euler(0, 0, 0));
                Instantiate(spawnParticle, spawnLocation.transform.position, Quaternion.Euler(-90, 0, 0));
                _GM.maegen -= _GM.dryadPrice;
                _UI.CheckPopulousUI();
            }
            else
            {
                _UI.SetErrorMessageInsufficientResources();
                _PC.Error();
            }
        }
        else
        {
            _UI.SetErrorMessageNeedToClaimHorgr();
            _PC.Error();
        }
    }
}
