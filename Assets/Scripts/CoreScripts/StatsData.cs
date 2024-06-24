using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsData : Singleton<StatsData>
{
    private void OnUnitKilled(string _unitID, string _killedBy)
    {
        //_SAVE.SetKillCount(_unitID, _killedBy, 1);
    }

    public void IncrementKill(string _unitID, string _killedID)
    {

    }

    private void OnWaveOver()
    {

    }

    private void OnEnable()
    {
        GameEvents.OnUnitKilled += OnUnitKilled;
        GameEvents.OnWaveOver += OnWaveOver;
    }

    

    private void OnDisable()
    {
        GameEvents.OnUnitKilled -= OnUnitKilled;
        GameEvents.OnWaveOver -= OnWaveOver;
    }
}
