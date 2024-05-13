using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    protected static GameManager _GM { get { return GameManager.instance; } }
    protected static HorgrManager _HM { get { return HorgrManager.instance; } }
    protected static HutManager _HUTM { get { return HutManager.instance; } }
    protected static UpgradeManager _UM { get { return UpgradeManager.instance; } }
    protected static MusicManager _MM { get { return MusicManager.instance; } }
    protected static EnemyManager _EM { get { return EnemyManager.instance; } }
    protected static UIManager _UI { get { return UIManager.instance; } }
    protected static TutorialManager _TUTM { get { return TutorialManager.instance; } }
    protected static PlayerControls _PC { get { return PlayerControls.instance; } }
    protected static SoundManager _SM { get { return SoundManager.instance; } }
    protected static TreePlacement _TPlace { get { return TreePlacement.instance; } }
    protected static RunePlacement _RPlace { get { return RunePlacement.instance; } }
    protected static BeaconPlacement _BPlace { get { return BeaconPlacement.instance; } }
    protected static StormerPlacement _SPlace { get { return StormerPlacement.instance; } }

    protected static ForestManager _FM { get { return ForestManager.instance; } }
    protected static TooltipManager _Tool { get { return TooltipManager.instance; } }
    protected static WaveManager _WM { get { return WaveManager.instance; } }
    protected static SpyManager _SPYM { get { return SpyManager.instance; } }
    protected static SaveData _SAVE { get { return SaveData.instance; } }
    protected static ScoreManager _SCORE { get { return ScoreManager.instance; } }
    protected static OverWorldManager _OM { get { return OverWorldManager.instance; } }
    protected static SceneManagement _SCENE { get { return SceneManagement.instance; } }
    protected static PerkManager _PERK { get { return PerkManager.instance; } }
}
public enum PerkID
{
    satyr,
    orcus,
    leshy,
    willow,
    skessa,
    goblin,
    fidhain,
    oak,
    huldra,
    golem,
    explosiveTree,
    homeTree,
    rune,
    fyre,
    bear,
}
public enum ToolID
{
    Rune,
    FyreBeacon,
    Stormer
}
public enum Upgrade
{
    BarkSkin,
    FlyFoot,
    Power,
    Tower,
    Rune,
    Beacon,
    Stormer,
    Tree,
    Fertile,
    Populous,
    Winfall,
    HomeTree,
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
}
public enum PlayMode
{
    DefaultMode,
    TreeMode,
    RuneMode,
    BeaconMode,
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
public enum UnitID
{
    SatyrUnit,
    OrcusUnit,
    LeshyUnit,
    HuldraUnit,
    VolvaUnit,
    GoblinUnit,
    GolemUnit,
    DryadUnit,
    Tower,
    SpitTower,
}
public enum WildlifeType
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