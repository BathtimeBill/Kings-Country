using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorgrManager : Singleton<HorgrManager>
{
    public List<GameObject> enemies;
    public List<GameObject> units;

    public GameObject[] spawnLocations;
    public GameObject horgr;
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
            _GM.maegen += 5;
            Instantiate(maegen5, horgrLocation, Quaternion.Euler(-90f, 0, 0));
        }
        if(enemyOwns)
        {
            _GM.maegen -= 5;
            Instantiate(maegenLoss, horgrLocation, Quaternion.Euler(-90f, 0, 0));
        }

        yield return new WaitForSeconds(Random.Range(20, 30));

        StartCoroutine(AddMaegen());
    }
}
