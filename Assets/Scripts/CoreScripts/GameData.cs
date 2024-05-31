using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    public Settings settings;

    [Header("Unit Data")]
    public List<UnitData> unitData;
    public UnitData GetUnit(UnitID _id) => unitData.Find(x => x.id == _id);

    #region Tool Functions
    [Header("Tool Data")]
    public List<ToolData> toolData;
    public ToolData GetTool(ToolID _id) => toolData.Find(x => x.id == _id);
    public bool CanUseTool(ToolID _id) => GetTool(_id).wildlifePrice <= _GM.wildlife && GetTool(_id).maegenPrice <= _GM.maegen;
    #endregion

    #region Perks
    [Header("Perk Data")]
    public List<PerkData> perkData;
    public PerkData GetPerk(PerkID _id) => perkData.Find(x=> x.id == _id);

    #endregion

    #region Level Functions
    [Header("Level Data")]
    public List<LevelData> levelData;
    public LevelData currentLevel => levelData.Find(x => x.id == _GM.thisLevel);
    public LevelID currentLevelID => currentLevel.id;

    public int levelMaxDays => currentLevel.days;

    public string currentLevelName => StringX.SplitCamelCase(currentLevel.id.ToString());

    public int enemySpawnLocations => currentLevel.enemySpawnLocations;
    #endregion
}
