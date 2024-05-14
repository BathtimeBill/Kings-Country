using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action OnUnitKilled = null;

    public static event Action<int> OnWildlifeValueChange = null;
    public static event Action OnWildlifeKilled = null;
    public static event Action OnTreePlaced = null;
    public static event Action OnTreeDestroyed = null;
    public static event Action OnUnitMove = null;
    public static event Action OnUnitArrivedAtHorgr = null;
    public static event Action OnUnitArrivedAtHut = null;
    public static event Action OnTreeHit = null;

    public static event Action OnFyrePlaced = null;
    public static event Action OnStormerPlaced = null;
    public static event Action OnRunePlaced = null;


    public static event Action OnBeaconDestroyed = null;
    public static event Action OnRuneDestroyed = null;

    public static event Action OnGameOver = null;
    public static event Action OnGameWin = null;

    public static event Action OnJustStragglers = null;
    public static event Action OnCollectMaegenButton = null;

    public static event Action OnWaveOver = null;
    public static event Action OnContinueButton = null;
    public static event Action OnStartNextRound = null;

    public static event Action OnWispDestroy = null;

    public static event Action OnEnemyKilled = null;

    public static event Action OnNextTutorial = null;

    public static event Action OnBorkrskinnUpgrade = null;
    public static event Action OnJarnnefiUpgrade = null;
    public static event Action OnFlugafotrUpgrade = null;
    public static event Action OnRuneUpgrade = null;
    public static event Action OnPopulousUpgrade = null;
    public static event Action OnBeaconUpgrade = null;
    public static event Action OnFertileSoilUpgrade = null;
    public static event Action OnTowerUpgrade = null;
    public static event Action OnStormerUpgrade = null;
    public static event Action OnTreeUpgrade = null;
    public static event Action OnWinfallUpgrade = null;
    public static event Action OnHomeTreeUpgrade = null;

    public static event Action OnUpgradeSelected = null;

    public static event Action OnAttackSelected = null;
    public static event Action OnDefendSelected = null;
    public static event Action OnFormationSelected = null;

    public static event Action OnHomeTreeSelected = null;
    public static event Action OnHorgrSelected = null;
    public static event Action OnHutSelected = null;

    public static event Action OnHomeTreeDeselected = null;
    public static event Action OnHorgrDeselected = null;
    public static event Action OnHutDeselected = null;

    public static event Action OnToggleOutline = null;

    public static event Action OnMineSpawned = null;
    public static event Action OnLordSpawned = null;
    public static event Action OnSpySpawned = null;

    public static event Action OnPerkButtonPressed = null;
    public static event Action<Tool> OnToolButtonPressed = null;
    public static event Action<UnitData> OnUnitButtonPressed = null;


    public static void ReportOnPerkButtonPressed()
    {
        OnPerkButtonPressed?.Invoke();
    }
    public static void ReportOnToolButtonPressed(Tool _tool)
    {
        OnToolButtonPressed?.Invoke(_tool);
    }
    public static void ReportOnUnitButtonPressed(UnitData _unit)
    {
        OnUnitButtonPressed?.Invoke(_unit);
    }
    public static void ReportOnSpySpawned()
    {
        OnSpySpawned?.Invoke();
    }
    public static void ReportOnMineSpawned()
    {
        OnMineSpawned?.Invoke();
    }
    public static void ReportOnLordSpawned()
    {
        OnLordSpawned?.Invoke();
    }
    public static void ReportOnToggleOutline()
    {
        OnToggleOutline?.Invoke();
    }
    public static void ReportOnHomeTreeDeselected()
    {
        OnHomeTreeDeselected?.Invoke();
    }
    public static void ReportOnHorgrDeselected()
    {
        OnHorgrDeselected?.Invoke();
    }
    public static void ReportOnHutDeselected()
    {
        OnHutDeselected?.Invoke();
    }
    public static void ReportOnHomeTreeSelected()
    {
        OnHomeTreeSelected?.Invoke();
    }
    public static void ReportOnHorgrSelected()
    {
        OnHorgrSelected?.Invoke();
    }
    public static void ReportOnHutSelected()
    {
        OnHutSelected?.Invoke();
    }
    public static void ReportOnUnitKilled()
    {
        OnUnitKilled?.Invoke();
    }
    public static void ReportOnWispDestroy()
    {
        OnWispDestroy?.Invoke();
    }
    public static void ReportOnAttackSelected()
    {
        OnAttackSelected?.Invoke();
    }
    public static void ReportOnDefendSelected()
    {
        OnDefendSelected?.Invoke();
    }
    public static void ReportOnFormationSelected()
    {
        OnFormationSelected?.Invoke();
    }
    public static void ReportOnEnemyKilled()
    {
        OnEnemyKilled?.Invoke();
    }
    public static void ReportOnNextTutorial()
    {
        OnNextTutorial?.Invoke();
    }
    public static void ReportOnCollectMaegenButton()
    {
        OnCollectMaegenButton?.Invoke();
    }
    public static void ReportOnJustStragglers()
    {
        OnJustStragglers?.Invoke();
    }
    public static void ReportOnGameWin()
    {
        OnGameWin?.Invoke();
    }
    public static void ReportOnHomeTreeUpgrade()
    {
        OnHomeTreeUpgrade?.Invoke();
    }
    public static void ReportOnWinfallUpgrade()
    {
        OnWinfallUpgrade?.Invoke();
    }
    public static void ReportOnTreeUpgrade()
    {
        OnTreeUpgrade?.Invoke();
    }
    public static void ReportOnStormerUpgrade()
    {
        OnStormerUpgrade?.Invoke();
    }
    public static void ReportOnTowerUpgrade()
    {
        OnTowerUpgrade?.Invoke();
    }
    public static void ReportOnStartNextRound()
    {
        OnStartNextRound?.Invoke();
    }
    public static void ReportOnUpgradeSelected()
    {
        OnUpgradeSelected?.Invoke();
    }
    public static void ReportOnContinueButton()
    {
        OnContinueButton?.Invoke();
    }
    public static void ReportOnWaveOver()
    {
        OnWaveOver?.Invoke();
    }
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
    public static void ReportOnUnitArrivedAtHut()
    {
        OnUnitArrivedAtHut?.Invoke();
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
    public static void ReportOnFyrePlaced()
    {
        OnFyrePlaced?.Invoke();
    }
    public static void ReportOnRunePlaced()
    {
        OnRunePlaced?.Invoke();
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

    public static void ReportOnWildlifeValueChanged(int _value)
    {
        OnWildlifeValueChange?.Invoke(_value);
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
