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
        Color unitCol = Color.white;
        if(_DATA.IsCreatureUnit(_unitID))
            unitCol = _DATA.GetUnit(EnumX.ToEnum<CreatureID>(_unitID)).mapIconColour;
        if(_DATA.IsHumanUnit(_unitID))
            unitCol = _DATA.GetUnit(EnumX.ToEnum<HumanID>(_unitID)).mapIconColour;
        string col = "<color=#" + ColorX.GetColorHex(unitCol) + ">";

        ChangeLogLine(col + _unitID + "</color> was spawned in ");
    }

    private void OnUnitKilled(string _unitID, string _killedBy)
    {
        Color unitCol = Color.white;
        if (_DATA.IsCreatureUnit(_unitID))
            unitCol = _DATA.GetUnit(EnumX.ToEnum<CreatureID>(_unitID)).mapIconColour;
        if (_DATA.IsHumanUnit(_unitID))
            unitCol = _DATA.GetUnit(EnumX.ToEnum<HumanID>(_unitID)).mapIconColour;
        string unitKillCol = "<color=#" + ColorX.GetColorHex(unitCol) + ">";

        Color unitKilledCol = Color.white;
        if (_DATA.IsCreatureUnit(_killedBy))
            unitKilledCol = _DATA.GetUnit(EnumX.ToEnum<CreatureID>(_killedBy)).mapIconColour;
        if (_DATA.IsHumanUnit(_killedBy))
            unitKilledCol = _DATA.GetUnit(EnumX.ToEnum<HumanID>(_killedBy)).mapIconColour;
        string unitKilledColor = "<color=#" + ColorX.GetColorHex(unitKilledCol) + ">";

        ChangeLogLine(unitKillCol + _unitID + "</color> was killed by "+ unitKilledColor + _killedBy);
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
        GameEvents.OnWaveOver       += OnDayOver;
        GameEvents.OnTreePlaced     += OnTreePlaced;
    }

    private void OnDisable()
    {
        GameEvents.OnUnitSpawned    -= OnUnitSpawned;
        GameEvents.OnUnitKilled     -= OnUnitKilled;
        GameEvents.OnWaveOver       -= OnDayOver;
        GameEvents.OnTreePlaced     -= OnTreePlaced;
    }
}
