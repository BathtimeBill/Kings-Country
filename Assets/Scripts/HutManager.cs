using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HutManager : Singleton<HutManager>
{
    public List<GameObject> enemies;
    public List<GameObject> units;

    public GameObject[] spawnLocations;
    public GameObject hut;
    public GameObject hutObject;
    public GameObject spawnParticle;
    public Vector3 hutLocation;

    public GameObject spawnLocation;

    public GameObject skessa;
    public GameObject huldra;

    public GameObject maegen5;
    public GameObject maegenLoss;

    public bool playerOwns;
    public bool enemyOwns;



    void Start()
    {
        GameObject go = Instantiate(hut, spawnLocations[RandomSpawnLocation()].transform.position, Quaternion.Euler(0, 183.4f, 0));
        StartCoroutine(AddMaegen());
    }

    private int RandomSpawnLocation()
    {
        int rndSpn = Random.Range(0, spawnLocations.Length);
        return rndSpn;
    }

    IEnumerator AddMaegen()
    {
        if (playerOwns)
        {
            _GM.maegen += 25;
            Instantiate(maegen5, hutLocation, Quaternion.Euler(-90f, 0, 0));
        }
        if (enemyOwns)
        {
            _GM.maegen -= 25;
            Instantiate(maegenLoss, hutLocation, Quaternion.Euler(-90f, 0, 0));
        }

        yield return new WaitForSeconds(Random.Range(20, 30));

        StartCoroutine(AddMaegen());
    }



    public void SpawnSkessaManager()
    {
        hutObject = GameObject.FindGameObjectWithTag("Horgr");
        if (playerOwns)
        {
            if (_GM.maegen >= 200 && _GM.populous < _GM.maxPopulous)
            {
                Instantiate(skessa, spawnLocation.transform.position, Quaternion.Euler(0, 0, 0));
                Instantiate(spawnParticle, spawnLocation.transform.position, Quaternion.Euler(-90, 0, 0));
                _GM.maegen -= 200;
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
    public void SpawnHuldraManager()
    {
        hutObject = GameObject.FindGameObjectWithTag("Horgr");
        if (playerOwns)
        {
            if (_GM.maegen >= 500 && _GM.populous < _GM.maxPopulous)
            {
                Instantiate(huldra, spawnLocation.transform.position, Quaternion.Euler(0, 0, 0));
                Instantiate(spawnParticle, spawnLocation.transform.position, Quaternion.Euler(-90, 0, 0));
                _GM.maegen -= 500;
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
