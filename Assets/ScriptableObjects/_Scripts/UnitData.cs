using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Data", menuName = "BGG/Unit Data", order = 3)]
public class UnitData : ScriptableObject
{
    public UnitID id;
    public string unitName;
    [TextArea(3, 5)]
    public string description;
    public Stats stats;
    public Sprite icon;

    public Stats GetStats()
    {
        return stats;
    }
}

[System.Serializable]
public class Stats
{
    public int health;
    public int damage;
    public int speed;
    public int price;
}
