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
    public GameObject enemySpawnParticle;
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
        StartCoroutine(SpawnEnemyUnits());
    }
    IEnumerator WaitToAssignSpawnPoints()
    {
        yield return new WaitForEndOfFrame();
        int rndSpn = Random.Range(0, spawnLocations.Count);
        yield return new WaitForSeconds(0.05f);
        GameObject go = Instantiate(hut, spawnLocations[rndSpn].transform.position, Quaternion.Euler(0, 183.4f, 0));
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

    public void SpawnSkessaManager()
    {
        hutObject = GameObject.FindGameObjectWithTag("Hut");
        int cost = _DATA.GetUnit(CreatureID.Skessa).cost;
        if (playerOwns)
        {
            if (_GM.maegen >= cost && _GM.populous < _GM.maxPopulous)
            {
                Instantiate(skessa, spawnLocation.transform.position, Quaternion.Euler(0, 0, 0));
                Instantiate(spawnParticle, spawnLocation.transform.position, Quaternion.Euler(-90, 0, 0));
                _GM.DecreaseMaegen(cost);
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
    public void SpawnGoblinManager()
    {
        if (playerOwns)
        {
            int cost = _DATA.GetUnit(CreatureID.Goblin).cost;
            if (_GM.maegen >= cost && _GM.populous < _GM.maxPopulous)
            {
                Instantiate(goblin, spawnLocation.transform.position, Quaternion.Euler(0, 0, 0));
                Instantiate(spawnParticle, spawnLocation.transform.position, Quaternion.Euler(-90, 0, 0));
                _GM.DecreaseMaegen(cost);
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
    public void SpawnFidhainManager()
    {
        if (playerOwns)
        {
            int cost = _DATA.GetUnit(CreatureID.Fidhain).cost;
            if (_GM.maegen >= cost && _GM.populous < _GM.maxPopulous)
            {
                Instantiate(fidhain, spawnLocation.transform.position, Quaternion.Euler(0, 0, 0));
                Instantiate(spawnParticle, spawnLocation.transform.position, Quaternion.Euler(-90, 0, 0));
                _GM.DecreaseMaegen(cost);
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
            case CreatureID.Skessa:
                SpawnSkessaManager();
                break;
            case CreatureID.Goblin:
                SpawnGoblinManager();
                break;
            case CreatureID.Fidhain:
                SpawnFidhainManager();
                break;
        }
    }

    private void OnHumanKilled(Enemy _enemy, string _killer)
    {
        if (enemies.Contains(_enemy.gameObject))
            enemies.Remove(_enemy.gameObject);
    }

    private void OnEnable()
    {
        GameEvents.OnContinueButton += OnContinueButton;
        GameEvents.OnUnitButtonPressed += OnUnitButtonPressed;
        GameEvents.OnHumanKilled += OnHumanKilled;
    }

    private void OnDisable()
    {
        GameEvents.OnContinueButton -= OnContinueButton;
        GameEvents.OnUnitButtonPressed -= OnUnitButtonPressed;
        GameEvents.OnHumanKilled -= OnHumanKilled;
    }
}
