using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class HorgrManager : Singleton<HorgrManager>
{
    public List<GameObject> enemies;
    public List<GameObject> units;

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
        StartCoroutine(WaitToSpawn());
    }
    IEnumerator WaitToSpawn()
    {
        yield return new WaitForEndOfFrame();
        GameObject go = Instantiate(horgr, spawnLocations[RandomSpawnLocation()].transform.position, Quaternion.Euler(-90, 180, 0));
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
                _UI.SetError(ErrorID.InsufficientResources);
            }
        }
       else
        {
            _UI.SetError(ErrorID.ClaimSite);
        }
    }
    public void SpawnHuldraManager()
    {
        horgrObject = GameObject.FindGameObjectWithTag("Horgr");
        if (playerOwns)
        {
            if (_GM.maegen >= _GM.huldraPrice && _GM.populous < _GM.maxPopulous)
            {
                Instantiate(huldra, spawnLocation.transform.position, Quaternion.Euler(0, 0, 0));
                Instantiate(spawnParticle, spawnLocation.transform.position, Quaternion.Euler(-90, 0, 0));
                _GM.DecreaseMaegen(_GM.huldraPrice);
                _UI.CheckPopulousUI();
                if(_TUTM.isTutorial && _TUTM.tutorialStage == 12)
                {
                    GameEvents.ReportOnNextTutorial();  
                }
            }
            else
            {
                _UI.SetError(ErrorID.InsufficientResources);
            }
        }
        else
        {
            _UI.SetError(ErrorID.ClaimSite);
        }
    }
    public void SpawnGolemManager()
    {
        horgrObject = GameObject.FindGameObjectWithTag("Horgr");
        if (playerOwns)
        {
            if (_GM.maegen >= _GM.golemPrice && _GM.populous < _GM.maxPopulous)
            {
                Instantiate(golem, spawnLocation.transform.position, Quaternion.Euler(0, 0, 0));
                Instantiate(spawnParticle, spawnLocation.transform.position, Quaternion.Euler(-90, 0, 0));
                _GM.DecreaseMaegen(_GM.golemPrice);
                _UI.CheckPopulousUI();
            }
            else
            {
                _UI.SetError(ErrorID.InsufficientResources);
            }
        }
        else
        {
            _UI.SetError(ErrorID.ClaimSite);
        }
    }
    void OnContinueButton()
    {
        playerOwns = true;
        enemyOwns = false;
    }


    private void OnUnitButtonPressed(UnitData _unitData)
    {
        switch (_unitData.id)
        {
            case UnitID.HuldraUnit:
                SpawnHuldraManager();
                break;
            case UnitID.GolemUnit:
                SpawnGolemManager();
                break;
        }
    }


    private void OnEnable()
    {
        GameEvents.OnContinueButton += OnContinueButton;
        GameEvents.OnUnitButtonPressed += OnUnitButtonPressed;
    }

    private void OnDisable()
    {
        GameEvents.OnContinueButton -= OnContinueButton;
        GameEvents.OnUnitButtonPressed -= OnUnitButtonPressed;
    }
}
