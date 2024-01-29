using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OverWorldManager : Singleton<OverWorldManager>
{
    public TMP_Text maegenText;
    public int overWorldMaegen;

    public GameObject level1Button;

    private void Start()
    {
        StartCoroutine(WaitForLoadGame());

    }

    public IEnumerator WaitForLoadGame()
    {
        yield return new WaitForSeconds(0.15f);
        _SAVE.Load();
        yield return new WaitForEndOfFrame();
        overWorldMaegen = _SAVE.overworldMaegen;
        maegenText.text = overWorldMaegen.ToString();
    }
    private void OnGameWin()
    {
        overWorldMaegen += _GM.maegen;
        _SAVE.OverworldSave();
    }
    private void OnEnable()
    {
        GameEvents.OnGameWin += OnGameWin;
    }

    private void OnDisable()
    {
        GameEvents.OnGameWin -= OnGameWin;
    }
}
