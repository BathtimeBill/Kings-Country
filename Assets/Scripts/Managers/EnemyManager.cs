using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public List<GameObject> enemies;
    [HideInInspector] public List<GameObject> spawnPoints;
    [Header("Spawn Cooldown")]
    public BV.Range cooldown;

    private List<HumanID> enemyIDList;
    private List<SpawnAmounts> spawnAmounts;
    private SpawnAmounts currentDaySpawnAmount;

    public bool allEnemiesDead => enemies.Count == 0;
    public bool allEnemiesSpawned => enemyIDList.Count == 0;
    public int currentKillCount;
    
    public GameObject spyNotification;
    public void AddSpawnPoint(GameObject spawnPoint) => spawnPoints.Add(spawnPoint);

    private Transform RandomSpawnPoint => ListX.GetRandomItemFromList(spawnPoints).transform; 
    private void Start()
    {
        enemyIDList = new List<HumanID>();
        spawnAmounts = new List<SpawnAmounts>();
        spawnAmounts.Clear();
        spawnAmounts = _DATA.currentLevel.spawnAmounts;
        
        if(_currentLevel.availableHumans.Contains(HumanID.Spy))
            StartCoroutine(SpawnSpy());
    }

    public void BeginNewDay()
    {
        currentDaySpawnAmount = spawnAmounts[_currentDay];
        enemies.Clear();
        FillDayEnemyList();
        StartCoroutine(SpawnEnemies());
    }

    #region Spawning
    IEnumerator SpawnEnemies()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < enemyIDList.Count; i++)
        {
            //Get the human model from the human data
            GameObject go = Instantiate(_DATA.GetUnit(enemyIDList[i]).playModel, RandomSpawnPoint.position, transform.rotation);
            //Add to the enemies list
            enemies.Add(go);
            //Wat a random time before spawning in the next enemy so they aren't on top of each other
            yield return new WaitForSeconds(Random.Range(0.3f, 1f));
        }

        yield return new WaitForSeconds(Random.Range(cooldown.min, cooldown.max));

        if(_agroPhase)
            StartCoroutine(SpawnEnemies());
    }

    public void SpawnHutEnemy(Vector3 spawnLocation)
    {
        int rndHuman = Random.Range(0, enemyIDList.Count);
        //Get the human model from the human data
        GameObject go = Instantiate(_DATA.GetUnit(enemyIDList[rndHuman]).playModel, spawnLocation, transform.rotation);
        //Add to the enemies list
        enemies.Add(go);
    }
    
    private void SpawnDog()  //CHECK IF VALUES ARE RIGHT
    {
        for (int day = 1; day <= _currentLevel.days; day++)
        {
            int requiredTreeCount = (day == 1) ? 5 : (day * 5);
            if (_currentDay == day && _GM.treeCount > requiredTreeCount)
            {
                int rndCoin = Random.Range(0, 2);
                if (rndCoin == 1)
                {
                    Instantiate(_DATA.GetUnit(HumanID.Dog).playModel, RandomSpawnPoint.position, transform.rotation);
                }
            }
        }
    }
    
    private void SpawnSpyEnemy(Vector3 spawnLocation)
    {
        GameObject go = Instantiate(_DATA.GetUnit(HumanID.Spy).playModel, spawnLocation, transform.rotation);
        //Add to the enemies list
        enemies.Add(go);
    }
    
    private IEnumerator SpawnSpy() //CHECK - Can the spawn intervals be changed to a formula or got from somewhere else?
    {
        float spySpawnInterval = 1000f;
        switch (_GM.currentDay)
        {
            case 0:
                spySpawnInterval = Random.Range(500, 600);
                break;
            case 1:
                spySpawnInterval = Random.Range(450, 550);
                goto default;
            case 2:
                spySpawnInterval = Random.Range(400, 500);
                goto default;
            case 3:
                spySpawnInterval = Random.Range(350, 430);
                goto default;
            case 4:
                spySpawnInterval = Random.Range(300, 400);
                goto default;
            case 5:
                spySpawnInterval = Random.Range(250, 350);
                goto default;
            case 6:
                spySpawnInterval = Random.Range(200, 300);
                goto default;
            case 7:
                spySpawnInterval = Random.Range(150, 250);
                goto default;
            case 8:
                spySpawnInterval = Random.Range(100, 200);
                goto default;
            case 9:
                spySpawnInterval = Random.Range(50, 150);
                goto default;
            default:
                Vector3 spawnPos = SpawnX.GetSpawnPositionOnLevel();
                SpawnSpyEnemy(spawnPos);
                _UI.SetError(ErrorID.SpyClose);
                Instantiate(spyNotification, spawnPos, transform.rotation);
                break;
        }

        yield return new WaitForSeconds(spySpawnInterval);
        StartCoroutine(SpawnSpy());
    }


    private void SpawnLoop()
    {
        int rndSpawn = Random.Range(0, _EM.spawnPoints.Count);
        //Get a random humanID from the current day list of available humans
        HumanID human = ListX.GetRandomItemFromList(enemyIDList);
        //Get the human model from the human data
        GameObject go = Instantiate(_DATA.GetUnit(human).playModel, _EM.spawnPoints[rndSpawn].transform.position, transform.rotation);
        print("Spawning in a " + human);
        //Remove the spawned unit from the available human list
        enemyIDList.Remove(human);
        enemies.Add(go);
    }
    #endregion

    public int GetDayTotalEnemyCount()
    {
        return currentDaySpawnAmount.logger +
            currentDaySpawnAmount.lumberjack +
            currentDaySpawnAmount.logcutter +
            currentDaySpawnAmount.wathe + 
            currentDaySpawnAmount.poacher + 
            currentDaySpawnAmount.bjornjeger + 
            currentDaySpawnAmount.dreng + 
            currentDaySpawnAmount.berserkr + 
            currentDaySpawnAmount.knight;
    }

    public int GetHumanDayLimit(HumanID _humanID)
    {
        switch(_humanID)
        {
            case HumanID.Logger:
                return currentDaySpawnAmount.logger;
            case HumanID.Lumberjack:
                return currentDaySpawnAmount.lumberjack;
            case HumanID.LogCutter:
                return currentDaySpawnAmount.logcutter;
            case HumanID.Wathe:
                return currentDaySpawnAmount.wathe;
            case HumanID.Poacher:
                return currentDaySpawnAmount.poacher;
            case HumanID.Bjornjeger:
                return currentDaySpawnAmount.bjornjeger;
            case HumanID.Dreng:
                return currentDaySpawnAmount.dreng;
            case HumanID.Berserkr:
                return currentDaySpawnAmount.berserkr;
            case HumanID.Knight:
                return currentDaySpawnAmount.knight;
            default:
                return 0;
        }
    }

    public IEnumerator KillAllEnemies()
    {
        //int enemyCount = enemies.Count;
        for(int i=0; i< enemies.Count; i++)
        {
            if (enemies[i].GetComponent<Enemy>() != null)
                enemies[i].GetComponent<Enemy>().Die(enemies[i].GetComponent<Enemy>(), "MassKilling", DeathID.Regular);
            yield return new WaitForEndOfFrame();
        }
        CheckEnemiesLeft();
    }

    private void CheckEnemiesLeft()
    {
        if (!allEnemiesDead || _agroPhase)
            return;

        StopCoroutine(SpawnEnemies());
        GameEvents.ReportOnDayOver(_currentDay);
    }

    /// <summary>
    /// Fills a list of HumanID's with those in this day then shuffles the list
    /// </summary>
    private void FillDayEnemyList()
    {
        enemyIDList.Clear();

        for (int i = 0; i < currentDaySpawnAmount.logger; i++)
            enemyIDList.Add(HumanID.Logger);
        for (int i = 0; i < currentDaySpawnAmount.lumberjack; i++)
            enemyIDList.Add(HumanID.Lumberjack);
        for (int i = 0; i < currentDaySpawnAmount.logcutter; i++)
            enemyIDList.Add(HumanID.LogCutter);
        for (int i = 0; i < currentDaySpawnAmount.wathe; i++)
            enemyIDList.Add(HumanID.Wathe);
        for (int i = 0; i < currentDaySpawnAmount.poacher; i++)
            enemyIDList.Add(HumanID.Poacher);
        for (int i = 0; i < currentDaySpawnAmount.bjornjeger; i++)
            enemyIDList.Add(HumanID.Bjornjeger);
        for (int i = 0; i < currentDaySpawnAmount.dreng; i++)
            enemyIDList.Add(HumanID.Dreng);
        for (int i = 0; i < currentDaySpawnAmount.berserkr; i++)
            enemyIDList.Add(HumanID.Berserkr);
        for (int i = 0; i < currentDaySpawnAmount.knight; i++)
            enemyIDList.Add(HumanID.Knight);

        ListX.ShuffleList(enemyIDList);
        currentKillCount = 0;
    }

    private void OnDayBegin(int _day)
    {
        SpawnDog();
    }

    private void OnEnemyUnitKilled(Enemy _enemy, string _killedBy)
    {
        //print(_enemy.unitID + " was killed");
        enemies.Remove(_enemy.gameObject);
        CheckEnemiesLeft();
        currentKillCount += 1;
    }


    private void OnEnable()
    {
        GameEvents.OnHumanKilled += OnEnemyUnitKilled;
        GameEvents.OnDayBegin += OnDayBegin;
    }

    private void OnDisable()
    {
        GameEvents.OnHumanKilled -= OnEnemyUnitKilled;
        GameEvents.OnDayBegin += OnDayBegin;
    }
}

[System.Serializable]   
public class SpawnAmounts
{
    public int logger;
    public int lumberjack;
    public int logcutter;
    public int wathe;
    public int poacher;
    public int bjornjeger;
    public int dreng;
    public int berserkr;
    public int knight;
}