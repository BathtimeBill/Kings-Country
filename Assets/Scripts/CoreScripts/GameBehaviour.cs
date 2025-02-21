using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameBehaviour : BV.Behaviour
{
    protected static GameManager _GAME { get { return GameManager.instance; } }
    protected static HomeTree _HOME { get { return GameManager.instance.homeTree; } }
    protected static Horgr _HORGR { get { return GameManager.instance.horgr; } }
    protected static Hut _HUT {get {return GameManager.instance.hut;}}
    protected static MusicManager _MM { get { return MusicManager.instance; } }
    protected static EnemyManager _EM { get { return EnemyManager.instance; } }
    protected static GuardianManager _GM { get { return GuardianManager.instance; } }
    protected static UIManager _UI { get { return UIManager.instance; } }
    protected static PlayerControls _PC { get { return PlayerControls.instance; } }
    protected static CameraController _CAMERA { get { return CameraController.instance; } }
    protected static SoundManager _SM { get { return SoundManager.instance; } }
    protected static TooltipManager _Tool { get { return TooltipManager.instance; } }
    protected static PerkManager _PERK { get { return PerkManager.instance; } }
    protected static GameData _DATA { get { return GameData.instance; } }
    protected static SaveManager _GAMESAVE { get { return SaveManager.instance; } }
    protected static UpgradeManager _UPGRADE { get { return UpgradeManager.instance; } }
    protected static SaveManager _SAVE { get { return SaveManager.instance; } }
    protected static TutorialManager _TUTORIAL { get { return _UI.tutorialManager; } }
    protected static GlossaryManager _GLOSSARY { get { return _UI.glossaryManager; } }
    protected static EffectsManager _EFFECTS { get { return EffectsManager.instance; } }

    public Settings _SETTINGS => _DATA.settings;
    public Tweening _TWEENING => _DATA.settings.tweening;
    public Colours _COLOUR => _DATA.settings.colours;
    public Icons _ICONS => _DATA.settings.icons;
    public PlayerSettings _PLAYER => _SAVE.save.playerSettings;
    public Testing _TESTING => _DATA.settings.testing;

    public bool _HasInput => _GAME.gameState == GameState.Play || _GAME.gameState == GameState.Build || _GAME.gameState == GameState.Tutorial;
    public bool _InGame => _GAME.gameState == GameState.Play || _GAME.gameState == GameState.Build;
    public bool _InTutorial => _GAME.gameState == GameState.Tutorial;
    public bool _InDay => _GAME.gameState == GameState.Play;
    public bool _HutExists => _GAME.hut != null;
    public bool _HorgrExists => _GAME.horgr != null;
    public bool _AgroPhase => _GAME.agroPhase;
    public bool _IsPaused => _GAME.gameState == GameState.Pause;
    public bool _GameFinished => _GAME.gameState == GameState.Finish;
    public bool _BuildPhase => _GAME.gameState == GameState.Build;
    public bool _TutorialComplete => _TUTORIAL.tutorialComplete;
    public int _CurrentDay => _GAME.currentDay - 1;
    public LevelData _CurrentLevel => _DATA.GetLevel(_GAME.thisLevel);
    public GameState _CurrentGameState => _GAME.gameState;
    public PlayMode _CurrentPlayMode => _GAME.playmode;
    public GameState _PreviousGameState => _GAME.previousState;
    public bool _NoTrees => _GAME.trees.Count == 0;
    public bool _TreesExist => _GAME.trees.Count > 0;
    public bool _GuardiansExist => _GM.guardianList.Count > 0;
    public bool _NoGuardians => _GM.guardianList.Count == 0;
    public bool _WildlifeExist => _GAME.currentWildlife.Count > 0;
    public bool _NoWildlife => _GAME.currentWildlife.Count == 0;
    public bool _EnemiesExist => _EM.enemies.Count > 0;
    public bool _NoEnemies => _EM.enemies.Count == 0;
    public Transform _Pointer => _PC.targetPointer.transform;

    public Experience _EXP(int i) => _SETTINGS.experience.Find(x=> x.level == i);

    /// <summary>
    /// Gets the name of a key object/unit formatted within it's colour
    /// </summary>
    /// <param name="_object">The object</param>
    /// <param name="_plural">Whether it is a plural or not</param>
    /// <param name="_toUpper">Whether it should be uppercase (default)</param>
    public string GetName(ObjectID _id, bool _plural = false, bool _toUpper = true) => _COLOUR.GetName(_id, _plural, _toUpper);

    public void Log(string message)
    {
        Debug.Log(message);
    }
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
    Tree = 30,
    Willow,
    Ficus,
}

public enum TreeID
{
    Tree,
    Willow,
    Ficus,
}
public enum CombatID
{
    Attack,
    Defend,
    Formation,
    Stop
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

public enum SiteID
{
    HomeTree,
    Hut,
    Horgr,
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
    Glossary,
    Tutorial,
    Transitioning,
    Lose,
    KillCam,
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
    Idle,
    Attack,
    DefendSite,
    Victory,
}
public enum GuardianState
{
    Idle,
    Attack,
    Moving,
    Track,
    Focus,
}
public enum CombatMode
{
    Move,
    AttackMove,
    Defend,
}
public enum GuardianID
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
    FidhainTower,
    Unknown
}

public enum EnemyID
{
    Logger, Lumberjack, LogCutter,
    Wathe, Poacher, Bjornjeger,
    Dreng, Berserkr, Knight,
    Dog, Spy, Lord,
    Mine,
    Unknown,
}

public enum UnitType
{
    Guardian,
    Human,
    Site,
    Tool,
    Tree
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

public enum GameSpeed{Normal, Fast, Slow}
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
    Poacher,
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
    HomeTree,Hut,Horgr,Tree,Tool,Wildlife
}

public enum DifficultyRating
{
    OneStar, TwoStar, ThreeStar, FourStar, FiveStar,
}

public enum PanelColourID { Black, White, Blue, Green, Red}
public enum DeathID { Regular,Launch,Explosion}

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

public enum ObjectID
{
    Resource    = 0,  Wildlife, Maegen, Populous, Day, Night, Guardian, Human, Grove, Perk,
    Tool        = 20, Tree, Rune, Fyre, Stormer, Tower, FidhainTower,
    HomeTree    = 30, Satyr, Orcus, Leshy,
    Hut         = 40, Goblin, Skessa, Fidhain,
    Horgr       = 50, Mistcalf, Huldra, Skuggi,
    Woodcutter  = 60, Logger, Lumberjack, Logcutter,
    Hunter      = 70, Poacher, Wathe, Bjornjeger,
    Warrior     = 80, Berserkr, Dreng, Knight,
    Special     = 90, Lord, Spy, Dog, Mine, Unknown
}

public enum DayID
{
    Day1, Day2, Day3, Day4, Day5, Day6, Day7, 
    Day8, Day9, Day10, Day11, Day12, Day13, Day14, 
    Day15, Day16, Day17, Day18, Day19, Day20, Day21
}

public enum GlossaryID
{
    CameraControls,
    GuardianMovement,
    Maegen,
    Trees,
    Wildlife,
    Populous,
    HomeTree,
    WitchsHut,
    Horgr,
    Powers,
    DayNightCycle,
    HumanClasses,
    Dogs,
    Mines,
    Spies,
    LordsOfTheLand,
    Combat,
    Health,
    Portal,
}

public enum TutorialID
{
    CameraMove,
    CameraRotate,
    CameraZoom,
    Maegen,
    Trees,
    PlantTree,
    Glossary,
    Wildlife,
    Guardians,
    GuardianMovement,
    Populous,
    DayNightCycle,
    Story,
}

public enum SiteState {Claimed, Neutral, Captured}