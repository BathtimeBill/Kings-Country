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
    public string GetStats()
    {
        return "<sprite name=\"IconHealth\">" + stats.health + " <sprite name=\"IconAttack\">" + stats.attack + " <sprite name=\"IconSpeed\">" + stats.speed;
    }
}

[System.Serializable]
public class Stats
{
    public int health;
    public int attack;
    public int defence;
    public int speed;
    public int cost;
}
