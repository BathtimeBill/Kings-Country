using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    protected static GameManager _GM { get { return GameManager.instance; } }
    protected static HorgrManager _HM { get { return HorgrManager.instance; } }
    protected static UpgradeManager _UM { get { return UpgradeManager.instance; } }
    protected static MusicManager _MM { get { return MusicManager.instance; } }
    protected static EnemyManager _EM { get { return EnemyManager.instance; } }
    protected static UIManager _UI { get { return UIManager.instance; } }
    protected static PlayerControls _PC { get { return PlayerControls.instance; } }
    protected static SoundManager _SM { get { return SoundManager.instance; } }
    protected static TreePlacement _TPlace { get { return TreePlacement.instance; } }
    protected static RunePlacement _RPlace { get { return RunePlacement.instance; } }
    protected static BeaconPlacement _BPlace { get { return BeaconPlacement.instance; } }
    protected static StormerPlacement _SPlace { get { return StormerPlacement.instance; } }

    protected static ForestManager _FM { get { return ForestManager.instance; } }
    protected static TooltipManager _Tool { get { return TooltipManager.instance; } }

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
}
public enum UnitType
{
    SatyrUnit,
    OrcusUnit,
    LeshyUnit,
    HuldraUnit,
    VolvaUnit,
    Tower,
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