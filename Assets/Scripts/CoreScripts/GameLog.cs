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
        string unit = _unitID;
        if (_DATA.IsCreatureUnit(_unitID))
            unit = _COLOUR.GetColoredUnitName(_DATA.GetUnit(EnumX.ToEnum<CreatureID>(_unitID)));
        if(_DATA.IsHumanUnit(_unitID))
            unit = _COLOUR.GetColoredUnitName(_DATA.GetUnit(EnumX.ToEnum<HumanID>(_unitID)));

        ChangeLogLine(unit + " was spawned in ");
    }

    private void OnUnitKilled(string _unitID, string _killedBy)
    {
        string unit = _unitID;
        if (_DATA.IsCreatureUnit(_unitID))
            unit = _COLOUR.GetColoredUnitName(_DATA.GetUnit(EnumX.ToEnum<CreatureID>(_unitID)));
        if (_DATA.IsHumanUnit(_unitID))
            unit = _COLOUR.GetColoredUnitName(_DATA.GetUnit(EnumX.ToEnum<HumanID>(_unitID)));

        string killer = _killedBy;
        if (_DATA.IsCreatureUnit(_killedBy))
            killer = _COLOUR.GetColoredUnitName(_DATA.GetUnit(EnumX.ToEnum<CreatureID>(_killedBy)));
        if (_DATA.IsHumanUnit(_killedBy))
            killer = _COLOUR.GetColoredUnitName(_DATA.GetUnit(EnumX.ToEnum<HumanID>(_killedBy)));

        ChangeLogLine(unit + " was killed by " + killer);
    }

    private void OnEnemyUnitKilled(Enemy _unitID, string _killedBy)
    {
        string unit = _unitID.unitID.ToString();
        if (_DATA.IsCreatureUnit(unit))
            unit = _COLOUR.GetColoredUnitName(_DATA.GetUnit(EnumX.ToEnum<CreatureID>(unit)));
        if (_DATA.IsHumanUnit(unit))
            unit = _COLOUR.GetColoredUnitName(_DATA.GetUnit(EnumX.ToEnum<HumanID>(unit)));

        string killer = _killedBy;
        if (_DATA.IsCreatureUnit(_killedBy))
            killer = _COLOUR.GetColoredUnitName(_DATA.GetUnit(EnumX.ToEnum<CreatureID>(_killedBy)));
        if (_DATA.IsHumanUnit(_killedBy))
            killer = _COLOUR.GetColoredUnitName(_DATA.GetUnit(EnumX.ToEnum<HumanID>(_killedBy)));

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
