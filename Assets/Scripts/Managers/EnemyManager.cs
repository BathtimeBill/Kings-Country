using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyManager : Singleton<EnemyManager>
{
    public List<GameObject> enemies;
    [ReadOnly] public List<GameObject> spawnPoints;
    [Header("Spawn Cooldown")]
    public BV.Range cooldown;
    [SerializeField] private List<EnemyID> enemyIDList = new List<EnemyID>();
    private List<SpawnAmounts> spawnAmounts;
    private SpawnAmounts currentDaySpawnAmount;
    public bool allEnemiesSpawned => enemyIDList.Count == 0;
    public int currentKillCount;
    
    public GameObject spyNotification;
    private List<GameObject> enemyPool = new List<GameObject>();
    private List<GameObject> ragdollPool = new List<GameObject>();
    [FormerlySerializedAs("testSpawnHuman")] [Header("Testing")]
    public EnemyID testSpawnEnemy;
    public bool noAutoSpawning = false;
    public void AddSpawnPoint(GameObject spawnPoint) => spawnPoints.Add(spawnPoint);
    public Transform RandomSpawnPoint => ListX.GetRandomItemFromList(spawnPoints).transform; 
    
    private void Start()
    {
        spawnAmounts = new List<SpawnAmounts>();
        spawnAmounts.Clear();
        spawnAmounts = _DATA.currentLevel.spawnAmounts;
        
        //if(_CurrentLevel.availableHumans.Contains(EnemyID.Spy))
         //   StartCoroutine(SpawnSpy());
    }

    public void BeginNewDay()
    {
        if (noAutoSpawning)
            return;
        
        currentDaySpawnAmount = spawnAmounts[_CurrentDay];
        enemies.Clear();
        FillDayEnemyList();
        StartCoroutine(SpawnEnemies());
    }

    #region Spawning

    public void SpawnEnemy(GameObject _enemy, Vector3 _location, bool _spawnedFromSite = false)
    {
        //POOL REDO
        GameObject go = Instantiate(_enemy, _location, Quaternion.identity);
        go.GetComponent<Enemy>().spawnedFromSite = _spawnedFromSite;
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
            SpawnEnemy(_DATA.GetEnemy(enemyIDList[i]).playModel, RandomSpawnPoint.position);
            //Wait a random time before spawning in the next enemy so they aren't on top of each other
            yield return new WaitForSeconds(Random.Range(0.3f, 1f));
        }

        yield return new WaitForSeconds(Random.Range(cooldown.min, cooldown.max));

        if(_AgroPhase && _InDay)
            StartCoroutine(SpawnEnemies());
    }

    public void SpawnSiteEnemy(Vector3 spawnLocation)
    {
        if(!_InDay) 
            return;

        int rndHuman = Random.Range(0, enemyIDList.Count);
        SpawnEnemy(_DATA.GetEnemy(enemyIDList[rndHuman]).playModel, spawnLocation, true);
    }
    
    private void SpawnDog()  //CHECK IF VALUES ARE RIGHT
    {
        if (!_InDay)
            return;

        for (int day = 1; day <= _CurrentLevel.days; day++)
        {
            int requiredTreeCount = (day == 1) ? 5 : (day * 5);
            if (_CurrentDay == day && _GAME.treeCount > requiredTreeCount)
            {
                int rndCoin = Random.Range(0, 2);
                if (rndCoin == 1)
                {
                    SpawnEnemy(_DATA.GetEnemy(EnemyID.Dog).playModel, RandomSpawnPoint.position);
                }
            }
        }
    }
    
    private void SpawnSpyEnemy(Vector3 spawnLocation)
    {
        SpawnEnemy(_DATA.GetEnemy(EnemyID.Spy).playModel, spawnLocation);
    }
    
    private IEnumerator SpawnSpy() //CHECK - Can the spawn intervals be changed to a formula or got from somewhere else?
    {
        float spySpawnInterval = 1000f;
        switch (_GAME.currentDay)
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
        //GameObject go = PoolX.GetFromPool(_enemy.unitData.ragdollModel, ragdollPool);
        //go.transform.position = _position;
        //go.transform.rotation = _rotation;
        if (_enemy.enemyData.ragdollModel != null)
        {
            GameObject go = Instantiate(_enemy.enemyData.ragdollModel, _position, _rotation);
            Ragdoll ragdoll = go.GetComponent<Ragdoll>();
            switch (_deathID)
            {
                case DeathID.Regular:
                    ragdoll.Die(ArrayX.GetRandomItemFromArray(_enemy.enemyData.dieSounds));
                    ragdoll.Launch(0, 0);
                    break;
                case DeathID.Explosion:
                    ragdoll.Die(ArrayX.GetRandomItemFromArray(_enemy.enemyData.dieSounds), true);
                    ragdoll.Launch(2000, -16000);
                    break;
                case DeathID.Launch:
                    ragdoll.Die(ArrayX.GetRandomItemFromArray(_enemy.enemyData.dieSounds));
                    ragdoll.Launch(20000, -20000);
                    break;
            }
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

    public int GetHumanDayLimit(EnemyID enemyID)
    {
        switch(enemyID)
        {
            case EnemyID.Logger:
                return currentDaySpawnAmount.logger;
            case EnemyID.Lumberjack:
                return currentDaySpawnAmount.lumberjack;
            case EnemyID.LogCutter:
                return currentDaySpawnAmount.logcutter;
            case EnemyID.Wathe:
                return currentDaySpawnAmount.wathe;
            case EnemyID.Poacher:
                return currentDaySpawnAmount.poacher;
            case EnemyID.Bjornjeger:
                return currentDaySpawnAmount.bjornjeger;
            case EnemyID.Dreng:
                return currentDaySpawnAmount.dreng;
            case EnemyID.Berserkr:
                return currentDaySpawnAmount.berserkr;
            case EnemyID.Knight:
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
        if (_EnemiesExist || _AgroPhase)
            return;

        StopCoroutine(SpawnEnemies());
        GameEvents.ReportOnDayOver(_CurrentDay);
    }

    /// <summary>
    /// Fills a list of HumanID's with those in this day then shuffles the list
    /// </summary>
    private void FillDayEnemyList()
    {
        enemyIDList.Clear();

        for (int i = 0; i < currentDaySpawnAmount.logger; i++)
            enemyIDList.Add(EnemyID.Logger);
        for (int i = 0; i < currentDaySpawnAmount.lumberjack; i++)
            enemyIDList.Add(EnemyID.Lumberjack);
        for (int i = 0; i < currentDaySpawnAmount.logcutter; i++)
            enemyIDList.Add(EnemyID.LogCutter);
        for (int i = 0; i < currentDaySpawnAmount.wathe; i++)
            enemyIDList.Add(EnemyID.Wathe);
        for (int i = 0; i < currentDaySpawnAmount.poacher; i++)
            enemyIDList.Add(EnemyID.Poacher);
        for (int i = 0; i < currentDaySpawnAmount.bjornjeger; i++)
            enemyIDList.Add(EnemyID.Bjornjeger);
        for (int i = 0; i < currentDaySpawnAmount.dreng; i++)
            enemyIDList.Add(EnemyID.Dreng);
        for (int i = 0; i < currentDaySpawnAmount.berserkr; i++)
            enemyIDList.Add(EnemyID.Berserkr);
        for (int i = 0; i < currentDaySpawnAmount.knight; i++)
            enemyIDList.Add(EnemyID.Knight);

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
        SpawnEnemy(_DATA.GetEnemy(testSpawnEnemy).playModel, RandomSpawnPoint.position);
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