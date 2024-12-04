using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public List<GameObject> enemies;
    [ReadOnly] public List<GameObject> spawnPoints;
    [Header("Spawn Cooldown")]
    public BV.Range cooldown;
    [SerializeField] private List<HumanID> enemyIDList = new List<HumanID>();
    private List<SpawnAmounts> spawnAmounts;
    private SpawnAmounts currentDaySpawnAmount;
    public bool allEnemiesSpawned => enemyIDList.Count == 0;
    public int currentKillCount;
    
    public GameObject spyNotification;
    private List<GameObject> enemyPool = new List<GameObject>();
    private List<GameObject> ragdollPool = new List<GameObject>();
    [Header("Testing")]
    public HumanID testSpawnHuman;
    public bool noAutoSpawning = false;
    public void AddSpawnPoint(GameObject spawnPoint) => spawnPoints.Add(spawnPoint);
    private Transform RandomSpawnPoint => ListX.GetRandomItemFromList(spawnPoints).transform; 
    
    private void Start()
    {
        spawnAmounts = new List<SpawnAmounts>();
        spawnAmounts.Clear();
        spawnAmounts = _DATA.currentLevel.spawnAmounts;
        
        if(_currentLevel.availableHumans.Contains(HumanID.Spy))
            StartCoroutine(SpawnSpy());
    }

    public void BeginNewDay()
    {
        if (noAutoSpawning)
            return;
        
        currentDaySpawnAmount = spawnAmounts[_currentDay];
        enemies.Clear();
        FillDayEnemyList();
        StartCoroutine(SpawnEnemies());
    }

    #region Spawning

    private void SpawnEnemy(GameObject _enemy, Vector3 _location)
    {
        //POOL REDO
        GameObject go = Instantiate(_enemy, _location, Quaternion.identity);
        //GameObject go = PoolX.GetFromPool(_enemy, enemyPool);
        //go.transform.localPosition = _location;
        //go.transform.rotation = transform.rotation;
        enemies.Add(go);
    }
    IEnumerator SpawnEnemies()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < enemyIDList.Count; i++)
        {
            SpawnEnemy(_DATA.GetUnit(enemyIDList[i]).playModel, RandomSpawnPoint.position);
            //Wait a random time before spawning in the next enemy so they aren't on top of each other
            yield return new WaitForSeconds(Random.Range(0.3f, 1f));
        }

        yield return new WaitForSeconds(Random.Range(cooldown.min, cooldown.max));

        if(_agroPhase && _inDay)
            StartCoroutine(SpawnEnemies());
    }

    public void SpawnHutEnemy(Vector3 spawnLocation)
    {
        if(!_inDay) 
            return;

        int rndHuman = Random.Range(0, enemyIDList.Count);
        SpawnEnemy(_DATA.GetUnit(enemyIDList[rndHuman]).playModel, spawnLocation);
    }
    
    private void SpawnDog()  //CHECK IF VALUES ARE RIGHT
    {
        if (!_inDay)
            return;

        for (int day = 1; day <= _currentLevel.days; day++)
        {
            int requiredTreeCount = (day == 1) ? 5 : (day * 5);
            if (_currentDay == day && _GM.treeCount > requiredTreeCount)
            {
                int rndCoin = Random.Range(0, 2);
                if (rndCoin == 1)
                {
                    SpawnEnemy(_DATA.GetUnit(HumanID.Dog).playModel, RandomSpawnPoint.position);
                }
            }
        }
    }
    
    private void SpawnSpyEnemy(Vector3 spawnLocation)
    {
        SpawnEnemy(_DATA.GetUnit(HumanID.Spy).playModel, spawnLocation);
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
    
    public void RemoveEnemy(Enemy _enemy, string _killedBy, DeathID _deathID, Vector3 _position, Quaternion _rotation)
    {
        //CHECK - May need to reset ragdoll physics when getting object
        GameObject go = PoolX.GetFromPool(_enemy.unitData.ragdollModel, ragdollPool);
        go.transform.position = _position;
        go.transform.rotation = _rotation;
        Ragdoll ragdoll = go.GetComponent<Ragdoll>();
        switch (_deathID)
        {
            case DeathID.Regular:
                ragdoll.Die(ArrayX.GetRandomItemFromArray(_enemy.unitData.dieSounds));
                ragdoll.Launch(0, 0);
                break;
            case DeathID.Explosion:
                ragdoll.Die(ArrayX.GetRandomItemFromArray(_enemy.unitData.dieSounds), true);
                ragdoll.Launch(2000, -16000);
                break;
            case DeathID.Launch:
                ragdoll.Die(ArrayX.GetRandomItemFromArray(_enemy.unitData.dieSounds));
                ragdoll.Launch(20000, -20000);
                break;
        }
        //POOL REDO
        //_enemy.gameObject.SetActive(false);
        enemies.Remove(_enemy.gameObject);
        Destroy(_enemy.gameObject);
        if(!noAutoSpawning)
            GameEvents.ReportOnHumanKilled(_enemy, _killedBy);
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
        if (_EnemiesExist || _agroPhase)
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

    public void SpawnSpecificEnemy()
    {
        SpawnEnemy(_DATA.GetUnit(testSpawnHuman).playModel, RandomSpawnPoint.position);
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