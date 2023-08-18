using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public GameObject[] enemies;

    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    IEnumerator CheckForEnemiesLeft()
    {
        Debug.Log("Checking for enemies");
        yield return new WaitForEndOfFrame();
        if (enemies.Length == 0 && _GM.canFinishWave)
        {
            GameEvents.ReportOnWaveOver();
            _GM.canFinishWave = false;
            _UI.nextRoundButton.interactable = true;
            _GM.downTime = true;
            _UI.treeToolImage.sprite = _UI.usableTreeTool;
        }
    }

    private void OnEnemyKilled()
    {
        StartCoroutine(CheckForEnemiesLeft());
    }

    private void OnCollectMaegenButton()
    {
        StartCoroutine(CheckForEnemiesLeft());
    }

    private void OnEnable()
    {
        GameEvents.OnEnemyKilled += OnEnemyKilled;
        //GameEvents.OnContinueButton += OnContinueButton;
        GameEvents.OnCollectMaegenButton += OnCollectMaegenButton;

    }

    private void OnDisable()
    {
        GameEvents.OnEnemyKilled -= OnEnemyKilled;
        //GameEvents.OnContinueButton -= OnContinueButton;
        GameEvents.OnCollectMaegenButton -= OnCollectMaegenButton;
    }
}
