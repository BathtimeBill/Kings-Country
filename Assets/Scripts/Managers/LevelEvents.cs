using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEvents : GameBehaviour
{

    [Header("Level 1")]
    public GameObject obstruction;
    public GameObject particle;
    public GameObject enemySpawn;
    public GameObject cam;
    public GameObject spawnPointSpawnPoint;
    public GameObject audioComponent;
    [Header("Level 2")]
    public GameObject[] spawnPointSpawnPoints;
    public GameObject mine;
    public bool mineExists;
    [Header("Level 3")]
    public GameObject lordOswyn;
    public bool lordHasArrived;

    //[Header("Level 4")]

    //[Header("Level 5")]



    void OnStartNextRound(int _day)
    {
        //LEVEL 1//

        if (_GM.level == LevelNumber.One && _GM.currentDay == 4)
        {
            GameEvents.ReportOnMineSpawned();
            StartCoroutine(SetGameState(8));
            cam.SetActive(true);
            audioComponent.SetActive(true);
            _GM.ChangeGameState(GameState.Transitioning);
            Instantiate(particle, spawnPointSpawnPoint.transform.position, Quaternion.Euler(-90, 0, 0));
            Instantiate(enemySpawn, spawnPointSpawnPoint.transform.position, spawnPointSpawnPoint.transform.rotation);
            Destroy(obstruction, 6);
        }

        //LEVEL 2//
        MineLogic();

        //LEVEL 3//

        if (_GM.level == LevelNumber.Three && _GM.currentDay == 3)
        {
            int dice3 = Random.Range(1, 6);
            if (dice3 >= 5 && lordHasArrived == false)
            {
                lordHasArrived = true;
                SpawnLord();
            }
            else
            {
                int dice = Random.Range(1, 6);
                if (dice == 6 && mineExists == false)
                {
                    mineExists = true;
                    SpawnMine();
                }
            }
            print(dice3.ToString());
        }
        if (_GM.level == LevelNumber.Three && _GM.currentDay == 4)
        {
            int dice3 = Random.Range(1, 6);
            if (dice3 >= 4 && lordHasArrived == false)
            {
                lordHasArrived = true;
                SpawnLord();
            }
            else
            {
                int dice = Random.Range(1, 6);
                if (dice >= 5 && mineExists == false)
                {
                    mineExists = true;
                    SpawnMine();
                }
            }
        }
        if (_GM.level == LevelNumber.Three && _GM.currentDay == 5)
        {
            int dice3 = Random.Range(1, 6);
            if (dice3 >= 3 && lordHasArrived == false)
            {
                lordHasArrived = true;
                SpawnLord();
            }
            else
            {
                int dice = Random.Range(1, 6);
                if (dice >= 4 && mineExists == false)
                {
                    mineExists = true;
                    SpawnMine();
                }
            }
        }
        if (_GM.level == LevelNumber.Three && _GM.currentDay == 6)
        {
            int dice3 = Random.Range(1, 6);
            if (dice3 >= 2 && lordHasArrived == false)
            {
                lordHasArrived = true;
                SpawnLord();
            }
            else
            {
                int dice = Random.Range(1, 6);
                if (dice >= 3 && mineExists == false)
                {
                    mineExists = true;
                    SpawnMine();
                }
            }
        }
        if (_GM.level == LevelNumber.Three && _GM.currentDay == 7)
        {
            int dice3 = Random.Range(1, 6);
            if (dice3 >= 1 && lordHasArrived == false)
            {
                lordHasArrived = true;
                SpawnLord();
            }
            else
            {
                int dice = Random.Range(1, 6);
                if (dice >= 2 && mineExists == false)
                {
                    mineExists = true;
                    SpawnMine();
                }
            }
        }
        if (_GM.level == LevelNumber.Three && _GM.currentDay == 8)
        {
            if (mineExists == false)
            {
                mineExists = true;
                SpawnMine();
            }
        }
    }

    private void MineLogic() //CHECK
    {
        if (_currentLevel.id == LevelID.WormturnRoad && !mineExists)
        {
            int currentDay = _GM.currentDay;
            if (currentDay >= 2 && currentDay <= 7)
            {
                int[] requiredRolls = { 6, 5, 4, 3, 2, 1 };
                int dice = Random.Range(1, 6);
                if (dice >= requiredRolls[currentDay - 2])
                {
                    mineExists = true;
                    SpawnMine();
                }
            }
        }
    }
    private void SpawnMine()
    {
        int i = Random.Range(0, spawnPointSpawnPoints.Length);
        StartCoroutine(SetGameState(14));
        Instantiate(mine, spawnPointSpawnPoints[i].transform.position, Quaternion.Euler(0, 0, 0));
        Instantiate(particle, spawnPointSpawnPoints[i].transform.position, Quaternion.Euler(-90, 0, 0));
        _GM.ChangeGameState(GameState.Transitioning);
    }
    private void SpawnLord()
    {
        GameEvents.ReportOnLordSpawned();
        int i = Random.Range(0, _EM.spawnPoints.Count);
        StartCoroutine(SetGameState(6));
        Instantiate(lordOswyn, _EM.spawnPoints[i].transform.position, transform.rotation);
        _GM.ChangeGameState(GameState.Transitioning);
    }
    IEnumerator SetGameState(float time)
    {
        yield return new WaitForSeconds(time);
        _GM.ChangeGameState(GameState.Play);
    }
    private void OnEnable()
    {
        GameEvents.OnDayBegin += OnStartNextRound;
    }
    private void OnDisable()
    {
        GameEvents.OnDayBegin -= OnStartNextRound;
    }


}


