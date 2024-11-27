using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameLog : GameBehaviour
{
    public CanvasGroup canvasGroup;
    public List<TMP_Text> logLines;
    public int currentLogLine = 0;

    private void Start()
    {
        for(int i=0; i< logLines.Count; i++)
        {
            logLines[i].text = "";
            logLines[i].alpha = 0;
        }

        OnPlayLog(_SAVE.GetPlayLog);
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
    private void OnHumanSpawned(HumanID _unitID)
    {
        string str = _unitID.ToString();
        ObjectID id = EnumX.ToEnum<ObjectID>(str);
        string unit = GetName(id);
        ChangeLogLine(unit + " was spawned in ");
    }
    private void OnOnGuardianSpawned(CreatureID _unitID)
    {
        string str = _unitID.ToString();
        ObjectID id = EnumX.ToEnum<ObjectID>(str);
        string unit = GetName(id);
        ChangeLogLine(unit + " was spawned in ");
    }

    private void OnUnitKilled(string _unitID, string _killedBy, int _daysSurvived)
    {
        string unit = GetName(EnumX.ToEnum<ObjectID>(_unitID));
        //string killer = GetName(EnumX.ToEnum<ObjectID>(_killedBy));
        ChangeLogLine(unit + " was killed by " + _killedBy + " after " + _daysSurvived + " days");
    }

    private void OnHumanKilled(Enemy _unitID, string _killedBy)
    {
        string unit = GetName(EnumX.ToEnum<ObjectID>(_unitID.unitID.ToString()));
        string killer = GetName(EnumX.ToEnum<ObjectID>(_killedBy));
        ChangeLogLine(unit + " was killed by " + killer);
    }

    private void OnDayOver(int _day) => ChangeLogLine("Day " + _day + " Complete");

    private void OnTreePlaced(ToolID _treeID) => ChangeLogLine("You placed a " + _treeID);

    private void OnPlayLog(bool _show) => canvasGroup.alpha = _show ? 1 : 0;
    
    private void OnEnable()
    {
        GameEvents.OnHumanSpawned    += OnHumanSpawned;
        GameEvents.OnGuardianSpawned    += OnOnGuardianSpawned;
        GameEvents.OnCreatureKilled     += OnUnitKilled;
        GameEvents.OnHumanKilled += OnHumanKilled;
        GameEvents.OnDayOver       += OnDayOver;
        GameEvents.OnTreePlaced     += OnTreePlaced;
        GameEvents.OnPlayLog += OnPlayLog;
    }

    private void OnDisable()
    {
        GameEvents.OnHumanSpawned -= OnHumanSpawned;
        GameEvents.OnGuardianSpawned -= OnOnGuardianSpawned;
        GameEvents.OnCreatureKilled     -= OnUnitKilled;
        GameEvents.OnHumanKilled -= OnHumanKilled;
        GameEvents.OnDayOver       -= OnDayOver;
        GameEvents.OnTreePlaced     -= OnTreePlaced;
        GameEvents.OnPlayLog -= OnPlayLog;
    }
}
