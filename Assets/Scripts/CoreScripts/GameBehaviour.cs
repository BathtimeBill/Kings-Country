using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : BV.Behaviour
{
    protected static GameManager _GM { get { return GameManager.instance; } }
    protected static HorgrManager _HM { get { return HorgrManager.instance; } }
    protected static HutManager _HUTM { get { return HutManager.instance; } }
    protected static MusicManager _MM { get { return MusicManager.instance; } }
    protected static EnemyManager _EM { get { return EnemyManager.instance; } }
    protected static UIManager _UI { get { return UIManager.instance; } }
    protected static TutorialManager _TUTM { get { return TutorialManager.instance; } }
    protected static PlayerControls _PC { get { return PlayerControls.instance; } }
    protected static SoundManager _SM { get { return SoundManager.instance; } }
    protected static TreePlacement _TPlace { get { return TreePlacement.instance; } }
    protected static RunePlacement _RPlace { get { return RunePlacement.instance; } }
    protected static ForestManager _FM { get { return ForestManager.instance; } }
    protected static TooltipManager _Tool { get { return TooltipManager.instance; } }
    protected static SpyManager _SPYM { get { return SpyManager.instance; } }
    protected static PerkManager _PERK { get { return PerkManager.instance; } }
    protected static LevelManager _LEVEL { get { return LevelManager.instance; } }
    protected static GameData _DATA { get { return GameData.instance; } }
    protected static SaveManager _GAMESAVE { get { return SaveManager.instance; } }
    protected static UpgradeManager _UPGRADE { get { return UpgradeManager.instance; } }

    public bool hasInput => _GM.gameState == GameState.Play || _GM.gameState == GameState.Build;
    public bool isPaused => _GM.gameState == GameState.Pause;
    public bool gameFinished => _GM.gameState == GameState.Finish;
    public bool buildPhase => _GM.gameState == GameState.Build;

    public Settings _SETTINGS => _DATA.settings;
    public Tweening _TWEENING => _DATA.settings.tweening;
    public Colours _COLOUR => _DATA.settings.colours;
    public Icons _ICONS => _DATA.settings.icons;
    public PlayerSettings _PS => _DATA.settings.playerSettings;
}



public enum UpgradeID
{
    Satyr,
    Orcus,
    Leshy,
    Skessa,
    Goblin,
    Fidhain,
    Huldra,
    Milcalf,
    HomeTree,
    Willow,
    ExplosiveTree,
    Rune,
    Fyre,
    Stormer,
    Bear,
}
public enum ToolID
{
    Rune,
    Fyre,
    Stormer,
    Tree,
    Willow,
    Ficus,
}
public enum CombatID
{
    Attack,
    Defend,
    Formation
}
public enum PerkID
{
    BarkSkin,
    FlyFoot,
    Power,
    Tower,
    Rune,
    Fyre,
    Stormer,
    Tree,
    Fertile,
    Populous,
    Winfall,
    HomeTree,
}

public enum BuildingID
{
    HomeTree,
    Hut,
    Hogyr,
    Unknown,
    Unknown2
}
public enum TreeType
{
    Pine,
    Deciduous,
}
public enum GameState
{
    Play,
    Pause,
    Build,
    Win,
    Intro,
    Finish,
    Transitioning,
}
public enum PlayMode
{
    DefaultMode,
    TreeMode,
    RuneMode,
    FyreMode,
    StormerMode,
}
public enum EnemyState
{
    Work,
    Attack,
    Flee,
    Beacon,
    Horgr,
}
public enum UnitState
{
    Idle,
    Attack,
    Moving,
    Track,
}
public enum CombatMode
{
    Move,
    AttackMove,
    Defend,
}
public enum CreatureID
{
    Satyr,
    Orcus,
    Leshy,
    Huldra,
    Skessa,
    Goblin,
    Mistcalf,
    Fidhain,
    Tower,
    SpitTower,
    Unknown
}

public enum HumanID
{
    Logger,
    Lumberjack,
    LogCutter,
    Wathe,
    Hunter,
    Bjornjeger,
    Dreng,
    Berserkr,
    Knight,
    Dog,
    Mine,
    Spy,
    Lord
}

public enum UnitType
{
    Creature,
    Human,
    Building
}

public enum EnemyType
{
    Hunter,
    Warrior,
    Woodcutter,
    Special,
}

public enum LevelID
{
    Ironwood,
    WormturnRoad,
    JotenheimPass,
    OswynsCrossing,
    SteinnporpGates,
    None,
}

public enum SeasonID
{
    Spring, Summer, Autumn, Winter
}

public enum ThreatID
{ 
    Dog,
    Mine,
    Spy,
    Lord
}


public enum WildlifeID
{
    Rabbit,
    Deer,
    Boar,
    Bear,
}
public enum WoodcutterType
{
    Logger,
    Lumberjack,
    LogCutter,
}
public enum HunterType
{
    Wathe,
    Hunter,
    Bjornjeger,
}
public enum WarriorType
{
    Dreng,
    Berserkr,
    Knight,
}
public enum LevelNumber
{
    One,
    Two,
    Three,
    Four,
    Five,
}
public enum LevelButton
{
    One,
    Two,
    Three,
    Four,
    Five,
}

public enum UpgradeType
{
    Unit, Tool, Wildlife,
}

public enum UpgradeCategoryID
{
    HomeTree,Hut,Hogyr,Tree,Tool,Wildlife
}

public enum DifficultyRating
{
    OneStar, TwoStar, ThreeStar, FourStar, FiveStar,
}

public enum ErrorID
{
    TooClose,
    TooFar,
    InsufficientMaegen,
    InsufficientResources,
    MaxPopulation,
    ToolCooldown,
    TooManyTrees,
    CantPlaceTrees,
    ForestUnderAttack,
    WildlifeUnderAttack,
    ClaimSite,
    TooCloseToTower,
    OutOfBounds,
    SpyClose
}