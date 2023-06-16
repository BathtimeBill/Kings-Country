using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public GameObject[] enemies;


    private void Start()
    {
        StartCoroutine(CheckForEnemiesLeft());
    }
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
    }

    IEnumerator CheckForEnemiesLeft()
    {
        if (enemies.Length == 0 && _GM.canFinishWave)
        {
            GameEvents.ReportOnWaveOver();
            _GM.canFinishWave = false;
            _UI.nextRoundButton.interactable = true;
            _GM.downTime = true;
            _UI.treeToolImage.sprite = _UI.usableTreeTool;
        }
        yield return new WaitForSeconds(5);
        StartCoroutine(CheckForEnemiesLeft());
    }
}
