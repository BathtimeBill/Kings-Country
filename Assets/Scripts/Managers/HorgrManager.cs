using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorgrManager : Singleton<HorgrManager>
{
    public List<GameObject> enemies;
    public List<GameObject> units;

    public GameObject[] spawnLocations;
    public GameObject horgr;
    public GameObject horgrObject;
    public GameObject spawnParticle;
    public Vector3 horgrLocation;

    public GameObject spawnLocation;

    public GameObject skessa;
    public GameObject huldra;

    public GameObject maegen5;
    public GameObject maegenLoss;

    public bool playerOwns;
    public bool enemyOwns;

    private new void Awake()
    {
        //horgrLocation = go.transform.position;
        //horgrObject = GameObject.FindGameObjectWithTag("Horgr");
        //spawnLocation = horgrObject.GetComponent<Horgr>().spawnLocation;
    }

    void Start()
    {
        GameObject go = Instantiate(horgr, spawnLocations[RandomSpawnLocation()].transform.position, Quaternion.Euler(-90, 0, 0));
        //StartCoroutine(AddMaegen());
        //StartCoroutine(WaitToReferenceHorgr());
    }
    
    private int RandomSpawnLocation()
    {
        int rndSpn = Random.Range(0, spawnLocations.Length);
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


    public void SpawnSkessaManager()
    {
        horgrObject = GameObject.FindGameObjectWithTag("Horgr");
        if (playerOwns)
        {
            if (_GM.maegen >= 3 && _GM.populous < _GM.maxPopulous)
            {
                Instantiate(skessa, spawnLocation.transform.position, Quaternion.Euler(0, 0, 0));
                Instantiate(spawnParticle, spawnLocation.transform.position, Quaternion.Euler(-90, 0, 0));
                _GM.maegen -= 3;
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
        horgrObject = GameObject.FindGameObjectWithTag("Horgr");
        if (playerOwns)
        {
            if (_GM.maegen >= 10 && _GM.populous < _GM.maxPopulous)
            {
                Instantiate(huldra, spawnLocation.transform.position, Quaternion.Euler(0, 0, 0));
                Instantiate(spawnParticle, spawnLocation.transform.position, Quaternion.Euler(-90, 0, 0));
                _GM.maegen -= 10;
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
