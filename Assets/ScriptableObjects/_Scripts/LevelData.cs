using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Level Data", menuName = "BGG/Level Data", order = 6)]
public class LevelData : ScriptableObject
{
    public LevelID id;
    public LevelNumber number;
    public DifficultyRating difficultyRating;
    public List<BuildingID> availableBuildings;
    public List<ToolID> availableTrees;
    public List<WoodcutterType> availableLoggers;
    public List<HunterType> availableHunters;
    public List<WarriorType> availableWarriors;
    public int enemySpawnLocations;
    public int days;
}