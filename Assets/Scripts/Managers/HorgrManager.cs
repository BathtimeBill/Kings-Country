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
    public Vector3 horgrLocation;

    public GameObject maegen5;
    public GameObject maegenLoss;

    public bool playerOwns;
    public bool enemyOwns;
    void Start()
    {
        GameObject go =Instantiate(horgr, spawnLocations[RandomSpawnLocation()].transform.position, Quaternion.Euler(-90, 0, 0));
        horgrLocation = go.transform.position;
        StartCoroutine(AddMaegen());
        StartCoroutine(WaitToReferenceHorgr());
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
            _GM.maegen += 25;
            Instantiate(maegen5, horgrLocation, Quaternion.Euler(-90f, 0, 0));
        }
        if(enemyOwns)
        {
            _GM.maegen -= 25;
            Instantiate(maegenLoss, horgrLocation, Quaternion.Euler(-90f, 0, 0));
        }

        yield return new WaitForSeconds(Random.Range(20, 30));

        StartCoroutine(AddMaegen());
    }
    IEnumerator WaitToReferenceHorgr()
    {
        yield return new WaitForEndOfFrame();
        horgrObject = GameObject.FindGameObjectWithTag("Horgr");
    }


    public void SpawnSkessaManager()
    {
        if (horgrObject.GetComponent<Horgr>().playerHasControl == true)
        {
            if (_GM.maegen >= 200)
            {
                Instantiate(horgrObject.GetComponent<Horgr>().skessa, horgrObject.GetComponent<Horgr>().spawnLocation.transform.position, Quaternion.Euler(0, 0, 0));
                _GM.maegen -= 200;
                _UI.CheckPopulousUI();
            }
            else
            {
                _UI.SetErrorMessageInsufficientResources();
                _PC.Error();

            }
        }
        if (horgrObject.GetComponent<Horgr>().playerHasControl == false)
        {
            _UI.SetErrorMessageNeedToClaimHorgr();
            _PC.Error();
        }
    }
}
