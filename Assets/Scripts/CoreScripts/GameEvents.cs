using System;
using UnityEngine;

public static class GameEvents
{

    public static event Action<GameState> OnGameStateChanged = null;
    public static event Action OnTutorialFinished = null;
    
    public static event Action<int> OnWildlifeValueChange = null;
    public static event Action<GameObject> OnWildlifeKilled = null;
    public static event Action<TreeID> OnTreePlaced = null;
    public static event Action<TreeID> OnTreeDestroyed = null;
    public static event Action OnUnitMove = null;
    public static event Action OnUnitArrivedAtHorgr = null;
    public static event Action OnUnitArrivedAtHut = null;
    public static event Action OnTreeHit = null;

    public static event Action OnFyrePlaced = null;
    public static event Action OnStormerPlaced = null;
    public static event Action OnRunePlaced = null;

    public static event Action OnRuneDestroyed = null;

    public static event Action OnGameOver = null;
    public static event Action<LevelID, int, int> OnLevelWin = null;

    public static event Action OnJustStragglers = null;
    public static event Action OnCollectMaegenButton = null;

    public static event Action<int> OnDayBegin = null;
    public static event Action<int> OnDayOver = null;
    public static event Action OnContinueButton = null;

    public static event Action OnWispDestroy = null;
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

    public static event Action<PerkID> OnPerkSelected = null;

    public static event Action<CombatID> OnCombatSelected = null;
    public static event Action OnFormationSelected = null;

    public static event Action OnHomeTreeSelected = null;
    public static event Action OnHorgrSelected = null;
    public static event Action OnHutSelected = null;
    public static event Action<SiteID, bool> OnSiteSelected = null;

    public static event Action OnToggleOutline = null;

    public static event Action OnMineSpawned = null;
    public static event Action OnLordSpawned = null;
    public static event Action OnSpySpawned = null;

    public static event Action OnPerkButtonPressed = null;
    public static event Action<TreeID> OnTreeButtonPressed = null;
    public static event Action<ToolID> OnToolButtonPressed = null;
    public static event Action<UnitData> OnUnitButtonPressed = null;
    public static event Action OnGroundClicked = null;
    public static event Action<int> OnMaegenChange = null;
    
    public static event Action<GameObject> OnObjectSelected = null; 
    
    //Title Events
    public static event Action OnLevelStart = null;

    public static void ReportOnGameStateChanged(GameState _gameState) => OnGameStateChanged?.Invoke(_gameState);
    public static void ReportOnTutorialFinished() => OnTutorialFinished?.Invoke();
    public static void ReportOnPerkButtonPressed() => OnPerkButtonPressed?.Invoke();
    public static void ReportOnTreeButtonPressed(TreeID _treeID) => OnTreeButtonPressed?.Invoke(_treeID);
    public static void ReportOnToolButtonPressed(ToolID _toolID) => OnToolButtonPressed?.Invoke(_toolID);
    public static void ReportOnUnitButtonPressed(UnitData _unit) => OnUnitButtonPressed?.Invoke(_unit);
    public static void ReportOnSpySpawned() => OnSpySpawned?.Invoke();
    public static void ReportOnMineSpawned() => OnMineSpawned?.Invoke();
    public static void ReportOnLordSpawned() => OnLordSpawned?.Invoke();
    
    public static void ReportOnToggleOutline() => OnToggleOutline?.Invoke();
    
    public static void ReportOnSiteSelected(SiteID _ID, bool _selected)=> OnSiteSelected?.Invoke(_ID, _selected);
    
    public static void ReportOnWispDestroy()
    {
        OnWispDestroy?.Invoke();
    }

    public static void ReportCombatButton(CombatID _combatID) => OnCombatSelected?.Invoke(_combatID);
    
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
    public static void ReportOnGameWin(LevelID _levelID, int _score, int _maegen)
    {
        OnLevelWin?.Invoke(_levelID, _score, _maegen);
    }
    
    public static void ReportOnUpgradeSelected(PerkID _id)
    {
        OnPerkSelected?.Invoke(_id);
        switch(_id)
        {
            case PerkID.BarkSkin:    OnBorkrskinnUpgrade?.Invoke(); break;
            case PerkID.Tower:       OnTowerUpgrade?.Invoke(); break;
            case PerkID.Power:       OnJarnnefiUpgrade?.Invoke(); break;
            case PerkID.Rune:        OnRuneUpgrade?.Invoke(); break;
            case PerkID.Stormer:     OnStormerUpgrade?.Invoke(); break;
            case PerkID.Fertile:     OnFertileSoilUpgrade?.Invoke(); break;
            case PerkID.FlyFoot:     OnFlugafotrUpgrade?.Invoke(); break;
            case PerkID.Fyre:        OnBeaconUpgrade?.Invoke(); break;
            case PerkID.HomeTree:    OnHomeTreeUpgrade?.Invoke(); break;
            case PerkID.Populous:    OnPopulousUpgrade?.Invoke(); break;
            case PerkID.Winfall:     OnWinfallUpgrade?.Invoke(); break;
            case PerkID.Tree:        OnTreeUpgrade?.Invoke(); break;
        }
    }
    public static void ReportOnContinueButton()
    {
        OnContinueButton?.Invoke();
    }
    public static void ReportOnDayBegin(int _day) => OnDayBegin?.Invoke(_day);

    public static void ReportOnDayOver(int _day) => OnDayOver?.Invoke(_day);

    public static void ReportOnTreeHit()
    {
        OnTreeHit?.Invoke();
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

    public static void ReportOnFyrePlaced() => OnFyrePlaced?.Invoke();
    public static void ReportOnStormerPlaced() => OnStormerPlaced?.Invoke();
    public static void ReportOnRunePlaced() => OnRunePlaced?.Invoke();
    public static void ReportOnRuneDestroyed() => OnRuneDestroyed?.Invoke();
    public static void ReportOnGameOver() => OnGameOver?.Invoke();

    public static void ReportOnUnitMove() => OnUnitMove?.Invoke();

    public static void ReportOnWildlifeValueChanged(int _value) => OnWildlifeValueChange?.Invoke(_value);
    public static void ReportOnWildlifeKilled(GameObject _wildlife) => OnWildlifeKilled?.Invoke(_wildlife);
    public static void ReportOnTreePlaced(TreeID _treeID)=> OnTreePlaced?.Invoke(_treeID);
    public static void ReportOnTreeDestroy(TreeID _treeID) => OnTreeDestroyed?.Invoke(_treeID);
    public static void ReportOnObjectSelected(GameObject _gameobject) => OnObjectSelected?.Invoke(_gameobject);
    public static void ReportOnMaegenChange(int _amount) => OnMaegenChange?.Invoke(_amount);

    public static void ReportOnGroundClicked()
    {
        OnGroundClicked?.Invoke();
        OnObjectSelected?.Invoke(null);
    }
    
    //Title
    public static void ReportOnLevelStart()
    {
        OnLevelStart?.Invoke();
    }
    
    

    #region Unit Management
    public static event Action<string, string, int> OnCreatureKilled = null;
    public static event Action<Enemy, string> OnHumanKilled = null;
    public static event Action<HumanID> OnHumanSpawned = null;
    public static event Action<CreatureID> OnGuardianSpawned = null;
    public static void ReportOnGuardianKilled(string _unitID, string _killedBy, int _daysSurvived) => OnCreatureKilled?.Invoke(_unitID, _killedBy, _daysSurvived);
    public static void ReportOnHumanKilled(Enemy _unitID, string _killedBy) => OnHumanKilled?.Invoke(_unitID, _killedBy);
    public static void ReportOnHumanSpawned(HumanID _ID) => OnHumanSpawned?.Invoke(_ID);
    public static void ReportOnGuardianSpawned(CreatureID _ID) => OnGuardianSpawned?.Invoke(_ID);
    #endregion

    #region Settings
    public static event Action<bool> OnUnitOutlines = null;
    public static event Action<bool> OnGuardianHealthBars = null;
    public static event Action<bool> OnHumanHealthBars = null;
    public static event Action<bool> OnMiniMapShow = null;
    public static event Action<bool> OnMiniMapIcons = null;
    public static event Action<bool> OnMiniMapRotation = null;
    public static event Action<bool> OnPlayLog = null;

    public static void ReportOnUnitOutlines(bool _show) => OnUnitOutlines?.Invoke(_show);
    public static void ReportOnGuardianHealthBars(bool _show) => OnGuardianHealthBars?.Invoke(_show);
    public static void ReportOnHumanHealthBars(bool _show) => OnHumanHealthBars?.Invoke(_show);
    public static void ReportOnMiniMapShow(bool _show) => OnMiniMapShow?.Invoke(_show);
    public static void ReportOnMiniMapIcons(bool _show) => OnMiniMapIcons?.Invoke(_show);
    public static void ReportOnMiniMapRotation(bool _show) => OnMiniMapRotation?.Invoke(_show);
    public static void ReportOnPlayLog(bool _show) => OnPlayLog?.Invoke(_show);
    #endregion
}
