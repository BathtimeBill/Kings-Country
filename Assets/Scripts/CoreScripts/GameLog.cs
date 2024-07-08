using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameLog : GameBehaviour
{
    public List<TMP_Text> logLines;
    public int currentLogLine = 0;

    private void Start()
    {
        for(int i=0; i< logLines.Count; i++)
        {
            logLines[i].text = "";
            logLines[i].alpha = 0;
        }
    }

    private void ChangeLogLine(string _message)
    {
        logLines[currentLogLine].transform.SetAsFirstSibling();
        logLines[currentLogLine].text = _message;
        TweenLogLine(logLines[currentLogLine]);
        currentLogLine = ListX.IncrementCounter(currentLogLine, logLines);
    }

    private void TweenLogLine(TMP_Text _text)
    {
        _text.DOFade(1, _TWEENING.logTweenTime).SetEase(_TWEENING.logEase).OnComplete(() => _text.DOFade(0, _TWEENING.logTweenTime).SetEase(_TWEENING.logEase).SetDelay(_TWEENING.logTweenDelay));
    }
    private void OnUnitSpawned(string _unitID)
    {
        string unit = GetName(EnumX.ToEnum<ObjectID>(_unitID));
        ChangeLogLine(unit + " was spawned in ");
    }

    private void OnUnitKilled(string _unitID, string _killedBy)
    {
        string unit = GetName(EnumX.ToEnum<ObjectID>(_unitID));
        string killer = GetName(EnumX.ToEnum<ObjectID>(_killedBy));
        ChangeLogLine(unit + " was killed by " + killer);
    }

    private void OnEnemyUnitKilled(Enemy _unitID, string _killedBy)
    {
        string unit = GetName(EnumX.ToEnum<ObjectID>(_unitID.ToString()));
        string killer = GetName(EnumX.ToEnum<ObjectID>(_killedBy));
        ChangeLogLine(unit + " was killed by " + killer);
    }

    private void OnDayOver()
    {
        ChangeLogLine("Day Complete");
    }

    private void OnTreePlaced(ToolID _treeID)
    {
        ChangeLogLine("You placed a " + _treeID);
    }

    private void OnEnable()
    {
        GameEvents.OnUnitSpawned    += OnUnitSpawned;
        GameEvents.OnUnitKilled     += OnUnitKilled;
        GameEvents.OnEnemyUnitKilled += OnEnemyUnitKilled;
        GameEvents.OnDayOver       += OnDayOver;
        GameEvents.OnTreePlaced     += OnTreePlaced;
    }

    

    private void OnDisable()
    {
        GameEvents.OnUnitSpawned    -= OnUnitSpawned;
        GameEvents.OnUnitKilled     -= OnUnitKilled;
        GameEvents.OnEnemyUnitKilled -= OnEnemyUnitKilled;
        GameEvents.OnDayOver       -= OnDayOver;
        GameEvents.OnTreePlaced     -= OnTreePlaced;
    }
}
