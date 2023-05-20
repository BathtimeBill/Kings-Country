using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action OnWildlifeKilled = null;
    public static event Action OnTreePlaced = null;
    public static event Action OnTreeDestroyed = null;
    public static event Action OnUnitMove = null;
    public static event Action OnUnitArrivedAtHorgr = null;
    public static event Action OnTreeHit = null;

    public static event Action OnStormerPlaced = null;

    public static event Action OnBeaconPlaced = null;

    public static event Action OnBeaconDestroyed = null;
    public static event Action OnRuneDestroyed = null;

    public static event Action OnGameOver = null;


    public static event Action OnBorkrskinnUpgrade = null;
    public static event Action OnJarnnefiUpgrade = null;
    public static event Action OnFlugafotrUpgrade = null;
    public static event Action OnRuneUpgrade = null;
    public static event Action OnPopulousUpgrade = null;
    public static event Action OnBeaconUpgrade = null;
    public static event Action OnFertileSoilUpgrade = null;



    public static void ReportOnTreeHit()
    {
        OnTreeHit?.Invoke();
    }
    public static void ReportOnStormerPlaced()
    {
        OnStormerPlaced?.Invoke();
    }
    public static void ReportOnUnitArrivedAtHorgr()
    {
        OnUnitArrivedAtHorgr?.Invoke();
    }
    public static void ReportOnFertileSoilUpgrade()
    {
        OnFertileSoilUpgrade?.Invoke();
    }
    public static void ReportOnBeaconUpgrade()
    {
        OnBeaconUpgrade?.Invoke();
    }
    public static void ReportOnRuneUpgrade()
    {
        OnRuneUpgrade?.Invoke();
    }
    public static void ReportOnPopulousUpgrade()
    {
        OnPopulousUpgrade?.Invoke();
    }
    public static void ReportOnBeaconDestroyed()
    {
        OnBeaconDestroyed?.Invoke();
    }
    public static void ReportOnBeaconPlaced()
    {
        OnBeaconPlaced?.Invoke();
    }
    public static void ReportOnRuneDestroyed()
    {
        OnRuneDestroyed?.Invoke();
    }

    public static void ReportOnGameOver()
    {
        OnGameOver?.Invoke();
    }
    public static void ReportOnBorkrskinnUpgrade()
    {
        OnBorkrskinnUpgrade?.Invoke();
    }
    public static void ReportOnJarnnefiUpgrade()
    {
        OnJarnnefiUpgrade?.Invoke();
    }
    public static void ReportOnFlugafotrUpgrade()
    {
        OnFlugafotrUpgrade?.Invoke();
    }
    public static void ReportOnUnitMove()
    {
        OnUnitMove?.Invoke();
    }

    public static void ReportOnWildlifeKilled()
    {
        OnWildlifeKilled?.Invoke();
    }
    public static void ReportOnTreePlaced()
    {
        OnTreePlaced?.Invoke();
    }
    public static void ReportOnTreeDestroy()
    {
        OnTreeDestroyed?.Invoke();
    }
}
