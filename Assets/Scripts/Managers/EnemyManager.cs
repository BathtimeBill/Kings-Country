using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public List<GameObject> enemies;
    public List<GameObject> spawnPoints;

    public List<SpawnAmounts> spawnAmounts;

    [Header("Waves")]
    public int loggerAmount;
    public int lumberjackAmount;
    public int watheAmount;
    public int longbowAmount;
    public int crossbowAmount;
    public int drengAmount;
    public int bezerkerAmount;
    public int knightAmount;
    [Header("1")]
    public int LoggerAmount1;
    public int LumberjackAmount1;
    public int WatheAmount1;
    public int LongbowAmount1;
    public int CrossbowAmount1;
    public int DrengAmount1;
    public int BezerkerAmount1;
    public int KnightAmount1;
    [Header("2")]
    public int LoggerAmount2;
    public int LumberjackAmount2;
    public int WatheAmount2;
    public int LongbowAmount2;
    public int CrossbowAmount2;
    public int DrengAmount2;
    public int BezerkerAmount2;
    public int KnightAmount2;
    [Header("3")]
    public int LoggerAmount3;
    public int LumberjackAmount3;
    public int WatheAmount3;
    public int LongbowAmount3;
    public int CrossbowAmount3;
    public int DrengAmount3;
    public int BezerkerAmount3;
    public int KnightAmount3;
    [Header("4")]
    public int LoggerAmount4;
    public int LumberjackAmount4;
    public int WatheAmount4;
    public int LongbowAmount4;
    public int CrossbowAmount4;
    public int DrengAmount4;
    public int BezerkerAmount4;
    public int KnightAmount4;
    [Header("5")]
    public int LoggerAmount5;
    public int LumberjackAmount5;
    public int WatheAmount5;
    public int LongbowAmount5;
    public int CrossbowAmount5;
    public int DrengAmount5;
    public int BezerkerAmount5;
    public int KnightAmount5;
    [Header("6")]
    public int LoggerAmount6;
    public int LumberjackAmount6;
    public int WatheAmount6;
    public int LongbowAmount6;
    public int CrossbowAmount6;
    public int DrengAmount6;
    public int BezerkerAmount6;
    public int KnightAmount6;
    [Header("7")]
    public int LoggerAmount7;
    public int LumberjackAmount7;
    public int WatheAmount7;
    public int LongbowAmount7;
    public int CrossbowAmount7;
    public int DrengAmount7;
    public int BezerkerAmount7;
    public int KnightAmount7;
    [Header("8")]
    public int LoggerAmount8;
    public int LumberjackAmount8;
    public int WatheAmount8;
    public int LongbowAmount8;
    public int CrossbowAmount8;
    public int DrengAmount8;
    public int BezerkerAmount8;
    public int KnightAmount8;
    [Header("9")]
    public int LoggerAmount9;
    public int LumberjackAmount9;
    public int WatheAmount9;
    public int LongbowAmount9;
    public int CrossbowAmount9;
    public int DrengAmount9;
    public int BezerkerAmount9;
    public int KnightAmount9;
    [Header("10")]
    public int LoggerAmount10;
    public int LumberjackAmount10;
    public int WatheAmount10;
    public int LongbowAmount10;
    public int CrossbowAmount10;
    public int DrengAmount10;
    public int BezerkerAmount10;
    public int KnightAmount10;
    [Header("11")]
    public int LoggerAmount11;
    public int LumberjackAmount11;
    public int WatheAmount11;
    public int LongbowAmount11;
    public int CrossbowAmount11;
    public int DrengAmount11;
    public int BezerkerAmount11;
    public int KnightAmount11;
    [Header("12")]
    public int LoggerAmount12;
    public int LumberjackAmount12;
    public int WatheAmount12;
    public int LongbowAmount12;
    public int CrossbowAmount12;
    public int DrengAmount12;
    public int BezerkerAmount12;
    public int KnightAmount12;
    [Header("13")]
    public int LoggerAmount13;
    public int LumberjackAmount13;
    public int WatheAmount13;
    public int LongbowAmount13;
    public int CrossbowAmount13;
    public int DrengAmount13;
    public int BezerkerAmount13;
    public int KnightAmount13;
    [Header("14")]
    public int LoggerAmount14;
    public int LumberjackAmount14;
    public int WatheAmount14;
    public int LongbowAmount14;
    public int CrossbowAmount14;
    public int DrengAmount14;
    public int BezerkerAmount14;
    public int KnightAmount14;
    [Header("15")]
    public int LoggerAmount15;
    public int LumberjackAmount15;
    public int WatheAmount15;
    public int LongbowAmount15;
    public int CrossbowAmount15;
    public int DrengAmount15;
    public int BezerkerAmount15;
    public int KnightAmount15;

    public bool allEnemiesDead => enemies.Count == 0;

    //void Update()
    //{
    //    enemies = GameObject.FindGameObjectsWithTag("Enemy");
    //}

    private void Start()
    {
        CheckWave();
    }

    public IEnumerator KillAllEnemies()
    {
        //int enemyCount = enemies.Count;
        for(int i=0; i< enemies.Count; i++)
        {
            if (enemies[i].GetComponent<Enemy>() != null)
                enemies[i].GetComponent<Enemy>().Die(enemies[i].GetComponent<Enemy>().unitID.ToString(), "MassKilling");
            yield return new WaitForEndOfFrame();
        }
        _GM.canFinishWave = false;
        CheckEnemiesLeft();
    }

    private void CheckEnemiesLeft()
    {
        if (!allEnemiesDead)
            return;

        GameEvents.ReportOnWaveOver();
    }

    IEnumerator CheckForEnemiesLeft()
    {
        yield return new WaitForEndOfFrame();
        if (enemies.Count == 0 && _GM.canFinishWave)
        {
            GameEvents.ReportOnWaveOver();
            _GM.canFinishWave = false;
            StopCoroutine(CheckForEnemiesLeft());
        }

        StartCoroutine(CheckForEnemiesLeft());
    }

    private void OnUnitKilled(string _unitID, string _killedBy)
    {
        if (!_DATA.IsHumanUnit(_unitID))
            return;

        print("Human was killed");
        CheckEnemiesLeft();
    }

    private void OnCollectMaegenButton()
    {
        StartCoroutine(CheckForEnemiesLeft());
    }

    private void CheckWave()
    {
        //loggerAmount = spawnAmounts[_GM.currentWave].loggerAmount;
        //lumberjackAmount = spawnAmounts[_GM.currentWave].lumberjackAmount;
        //watheAmount = spawnAmounts[_GM.currentWave].watheAmount;
        //longbowAmount = spawnAmounts[_GM.currentWave].longbowAmount;
        //crossbowAmount = spawnAmounts[_GM.currentWave].crossbowAmount;
        //drengAmount = spawnAmounts[_GM.currentWave].drengAmount;
        //bezerkerAmount = spawnAmounts[_GM.currentWave].bezerkerAmount;
        //knightAmount = spawnAmounts[_GM.currentWave].knightAmount;
        if (_GM.currentWave <= 1)
        {
            loggerAmount = LoggerAmount1;
            lumberjackAmount = LumberjackAmount1;
            watheAmount = WatheAmount1;
            longbowAmount = LongbowAmount1;
            crossbowAmount = CrossbowAmount1;
            drengAmount = DrengAmount1;
            bezerkerAmount = BezerkerAmount1;
            knightAmount = KnightAmount1;
        }
        if (_GM.currentWave == 2)
        {
            loggerAmount = LoggerAmount2;
            lumberjackAmount = LumberjackAmount2;
            watheAmount = WatheAmount2;
            longbowAmount = LongbowAmount2;
            crossbowAmount = CrossbowAmount2;
            drengAmount = DrengAmount2;
            bezerkerAmount = BezerkerAmount2;
            knightAmount = KnightAmount2;
        }
        if (_GM.currentWave == 3)
        {
            loggerAmount = LoggerAmount3;
            lumberjackAmount = LumberjackAmount3;
            watheAmount = WatheAmount3;
            longbowAmount = LongbowAmount3;
            crossbowAmount = CrossbowAmount3;
            drengAmount = DrengAmount3;
            bezerkerAmount = BezerkerAmount3;
            knightAmount = KnightAmount3;
        }
        if (_GM.currentWave == 4)
        {
            loggerAmount = LoggerAmount4;
            lumberjackAmount = LumberjackAmount4;
            watheAmount = WatheAmount4;
            longbowAmount = LongbowAmount4;
            crossbowAmount = CrossbowAmount4;
            drengAmount = DrengAmount4;
            bezerkerAmount = BezerkerAmount4;
            knightAmount = KnightAmount4;
        }
        if (_GM.currentWave == 5)
        {
            loggerAmount = LoggerAmount5;
            lumberjackAmount = LumberjackAmount5;
            watheAmount = WatheAmount5;
            longbowAmount = LongbowAmount5;
            crossbowAmount = CrossbowAmount5;
            drengAmount = DrengAmount5;
            bezerkerAmount = BezerkerAmount5;
            knightAmount = KnightAmount5;
        }
        if (_GM.currentWave == 6)
        {
            loggerAmount = LoggerAmount6;
            lumberjackAmount = LumberjackAmount6;
            watheAmount = WatheAmount6;
            longbowAmount = LongbowAmount6;
            crossbowAmount = CrossbowAmount6;
            drengAmount = DrengAmount6;
            bezerkerAmount = BezerkerAmount6;
            knightAmount = KnightAmount6;
        }
        if (_GM.currentWave == 7)
        {
            loggerAmount = LoggerAmount7;
            lumberjackAmount = LumberjackAmount7;
            watheAmount = WatheAmount7;
            longbowAmount = LongbowAmount7;
            crossbowAmount = CrossbowAmount7;
            drengAmount = DrengAmount7;
            bezerkerAmount = BezerkerAmount7;
            knightAmount = KnightAmount7;
        }
        if (_GM.currentWave == 8)
        {
            loggerAmount = LoggerAmount8;
            lumberjackAmount = LumberjackAmount8;
            watheAmount = WatheAmount8;
            longbowAmount = LongbowAmount8;
            crossbowAmount = CrossbowAmount8;
            drengAmount = DrengAmount8;
            bezerkerAmount = BezerkerAmount8;
            knightAmount = KnightAmount8;
        }
        if (_GM.currentWave == 9)
        {
            loggerAmount = LoggerAmount9;
            lumberjackAmount = LumberjackAmount9;
            watheAmount = WatheAmount9;
            longbowAmount = LongbowAmount9;
            crossbowAmount = CrossbowAmount9;
            drengAmount = DrengAmount9;
            bezerkerAmount = BezerkerAmount9;
            knightAmount = KnightAmount9;
        }
        if (_GM.currentWave == 10)
        {
            loggerAmount = LoggerAmount10;
            lumberjackAmount = LumberjackAmount10;
            watheAmount = WatheAmount10;
            longbowAmount = LongbowAmount10;
            crossbowAmount = CrossbowAmount10;
            drengAmount = DrengAmount10;
            bezerkerAmount = BezerkerAmount10;
            knightAmount = KnightAmount10;
        }
        if (_GM.currentWave == 11)
        {
            loggerAmount = LoggerAmount11;
            lumberjackAmount = LumberjackAmount11;
            watheAmount = WatheAmount11;
            longbowAmount = LongbowAmount11;
            crossbowAmount = CrossbowAmount11;
            drengAmount = DrengAmount11;
            bezerkerAmount = BezerkerAmount11;
            knightAmount = KnightAmount11;
        }
        if (_GM.currentWave == 12)
        {
            loggerAmount = LoggerAmount12;
            lumberjackAmount = LumberjackAmount12;
            watheAmount = WatheAmount12;
            longbowAmount = LongbowAmount12;
            crossbowAmount = CrossbowAmount12;
            drengAmount = DrengAmount12;
            bezerkerAmount = BezerkerAmount12;
            knightAmount = KnightAmount12;
        }
        if (_GM.currentWave == 13)
        {
            loggerAmount = LoggerAmount13;
            lumberjackAmount = LumberjackAmount13;
            watheAmount = WatheAmount13;
            longbowAmount = LongbowAmount13;
            crossbowAmount = CrossbowAmount13;
            drengAmount = DrengAmount13;
            bezerkerAmount = BezerkerAmount13;
            knightAmount = KnightAmount13;
        }
        if (_GM.currentWave == 14)
        {
            loggerAmount = LoggerAmount14;
            lumberjackAmount = LumberjackAmount14;
            watheAmount = WatheAmount14;
            longbowAmount = LongbowAmount14;
            crossbowAmount = CrossbowAmount14;
            drengAmount = DrengAmount14;
            bezerkerAmount = BezerkerAmount14;
            knightAmount = KnightAmount14;
        }
        if (_GM.currentWave == 15)
        {
            loggerAmount = LoggerAmount15;
            lumberjackAmount = LumberjackAmount15;
            watheAmount = WatheAmount15;
            longbowAmount = LongbowAmount15;
            crossbowAmount = CrossbowAmount15;
            drengAmount = DrengAmount15;
            bezerkerAmount = BezerkerAmount15;
            knightAmount = KnightAmount15;
        }
    }

    void OnStartNextRound()
    {
        StartCoroutine(WaitToCheckWave());
    }
    IEnumerator WaitToCheckWave()
    {
        yield return new WaitForEndOfFrame();
        CheckWave();
    }
    private void OnEnable()
    {
        GameEvents.OnUnitKilled += OnUnitKilled;
        //GameEvents.OnContinueButton += OnContinueButton;
        GameEvents.OnCollectMaegenButton += OnCollectMaegenButton;
        GameEvents.OnWaveBegin += OnStartNextRound;

    }

    

    private void OnDisable()
    {
        GameEvents.OnUnitKilled -= OnUnitKilled;
        //GameEvents.OnContinueButton -= OnContinueButton;
        GameEvents.OnCollectMaegenButton -= OnCollectMaegenButton;
        GameEvents.OnWaveBegin -= OnStartNextRound;
    }
}

[System.Serializable]   
public class SpawnAmounts
{
    public int loggerAmount;
    public int lumberjackAmount;
    public int watheAmount;
    public int longbowAmount;
    public int crossbowAmount;
    public int drengAmount;
    public int bezerkerAmount;
    public int knightAmount;
}