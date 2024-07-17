using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "BGG/Tool", order = 2)]
public class ToolData : ScriptableObject
{
    public ToolID id;
    public new string name;
    [TextArea]
    public string description;
    public Sprite icon;
    public int maegenPrice;
    public int wildlifePrice;
    public int cooldownTime = 5;
    public int upgradeLevel = 1;
    public int damage;
    public bool unlocked;
    public AudioClip[] toolSounds;
}
