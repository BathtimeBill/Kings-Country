using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : Singleton<ToolManager>
{
    public List<ToolData> toolData;

    public ToolData GetTool(ToolID _id) => toolData.Find(x => x.id == _id);

    public bool CanUseTool(ToolID _id)
    {
        ToolData td = GetTool(_id);
        return td.wildlifePrice <= _GM.wildlife && td.maegenPrice <= _GM.maegen;
    }
}
