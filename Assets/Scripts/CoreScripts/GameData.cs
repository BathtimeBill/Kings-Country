using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    public Settings settings;
    public LevelSettings levelSettings;

    [Header("Unit Data")]
    public List<UnitData> unitData;
    public UnitData GetUnit(UnitID _id) => unitData.Find(x => x.id == _id);

    [Header("Tool Data")]
    public List<ToolData> toolData;

    #region Tool Functions
    public ToolData GetTool(ToolID _id) => toolData.Find(x => x.id == _id);
    public bool CanUseTool(ToolID _id) => GetTool(_id).wildlifePrice <= _GM.wildlife && GetTool(_id).maegenPrice <= _GM.maegen;
    #endregion

    #region Level Functions
    public Level getCurrentLevel => levelSettings.levels.Find(x => x.levelID == _GM.thisLevel);

    public int maxWave => getCurrentLevel.waves;

    public string currentLevelName => StringX.SplitCamelCase(getCurrentLevel.levelID.ToString());

    public int enemySpawnLocations => getCurrentLevel.enemySpawnLocations;
    #endregion
}
