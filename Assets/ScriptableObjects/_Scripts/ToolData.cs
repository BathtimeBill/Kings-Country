using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "BGG/Tool", order = 2)]
public class ToolData : ScriptableObject
{
    public ToolID id;
    public string toolName;
    public string toolDescription;
    public Sprite toolIcon;
    public int maegenPrice;
    public int wildlifePrice;
    public int upgradeLevel;
    public bool unlocked;
}
