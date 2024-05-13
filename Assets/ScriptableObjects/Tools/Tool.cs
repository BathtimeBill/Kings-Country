using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "BGG/Tool", order = 2)]
public class Tool : ScriptableObject
{
    public ToolID id;
    public string toolName;
    public string toolDescription;
    public Sprite toolIcon;
    public bool unlocked;
}
