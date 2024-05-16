using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Level Settings", menuName = "BGG/Level Settings", order = 6)]
public class LevelSettings : ScriptableObject
{
    public List<Level> levels;
}
[System.Serializable]
public class Level
{
    public LevelID levelID;
    public LevelNumber number;
    public List<BuildingID> availableBuildings;
    public List<WoodcutterType> availableLoggers;
    public List<HunterType> availableHunters;
    public List<WarriorType> availableWarriors;
    public int enemySpawnLocations;
    public int waves;
}