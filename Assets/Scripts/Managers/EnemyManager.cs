using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [HideInInspector] public List<GameObject> enemies;
    [HideInInspector] public List<GameObject> spawnPoints;
    [Header("Spawn Cooldown")]
    public BV.Range cooldown;

    private List<HumanID> enemyIDList;
    private List<SpawnAmounts> spawnAmounts;
    private SpawnAmounts currentDaySpawnAmount;

    public bool allEnemiesDead => enemies.Count == 0;
    public bool allEnemiesSpawned => enemyIDList.Count == 0;
    public int currentKillCount;
    public void AddSpawnPoint(GameObject spawnPoint) => spawnPoints.Add(spawnPoint);

    private void Start()
    {
        enemyIDList = new List<HumanID>();
        spawnAmounts = new List<SpawnAmounts>();
        spawnAmounts.Clear();
        spawnAmounts = _DATA.currentLevel.spawnAmounts;
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
            //Get a Random Spawn
            int rndSpawn = Random.Range(0, _EM.spawnPoints.Count);
            //Get the human model from the human data
            GameObject go = Instantiate(_DATA.GetUnit(enemyIDList[i]).playModel, _EM.spawnPoints[rndSpawn].transform.position, transform.rotation);
            //Add to the enemies list
            enemies.Add(go);
            //Wat a random time before spawning in the next enemy so they aren't on top of each other
            yield return new WaitForSeconds(Random.Range(0.3f, 1f));
        }

        yield return new WaitForSeconds(Random.Range(cooldown.min, cooldown.max));

        if(_agroPhase)
            StartCoroutine(SpawnEnemies());
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
            currentDaySpawnAmount.hunter + 
            currentDaySpawnAmount.bjornjeger + 
            currentDaySpawnAmount.dreng + 
            currentDaySpawnAmount.bezerker + 
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
                return currentDaySpawnAmount.hunter;
            case HumanID.Bjornjeger:
                return currentDaySpawnAmount.bjornjeger;
            case HumanID.Dreng:
                return currentDaySpawnAmount.dreng;
            case HumanID.Bezerker:
                return currentDaySpawnAmount.bezerker;
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
        GameEvents.ReportOnWaveOver();
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
        for (int i = 0; i < currentDaySpawnAmount.hunter; i++)
            enemyIDList.Add(HumanID.Poacher);
        for (int i = 0; i < currentDaySpawnAmount.bjornjeger; i++)
            enemyIDList.Add(HumanID.Bjornjeger);
        for (int i = 0; i < currentDaySpawnAmount.dreng; i++)
            enemyIDList.Add(HumanID.Dreng);
        for (int i = 0; i < currentDaySpawnAmount.bezerker; i++)
            enemyIDList.Add(HumanID.Bezerker);
        for (int i = 0; i < currentDaySpawnAmount.knight; i++)
            enemyIDList.Add(HumanID.Knight);

        ListX.ShuffleList(enemyIDList);
        currentKillCount = 0;
    }


    private void OnEnemyUnitKilled(Enemy _enemy, string _killedBy)
    {
        print(_enemy.unitID + " was killed");
        enemies.Remove(_enemy.gameObject);
        CheckEnemiesLeft();
        currentKillCount += 1;
    }


    private void OnEnable()
    {
        GameEvents.OnHumanKilled += OnEnemyUnitKilled;

    }

    private void OnDisable()
    {
        GameEvents.OnHumanKilled -= OnEnemyUnitKilled;
    }
}

[System.Serializable]   
public class SpawnAmounts
{
    public int logger;
    public int lumberjack;
    public int logcutter;
    public int wathe;
    public int hunter;
    public int bjornjeger;
    public int dreng;
    public int bezerker;
    public int knight;
}