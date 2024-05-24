using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    public Settings settings;

    [Header("Tool Data")]
    public List<ToolData> toolData;

    #region Tool Functions
    public ToolData GetTool(ToolID _id) => toolData.Find(x => x.id == _id);
    public bool CanUseTool(ToolID _id) => GetTool(_id).wildlifePrice <= _GM.wildlife && GetTool(_id).maegenPrice <= _GM.maegen;
    #endregion
}
