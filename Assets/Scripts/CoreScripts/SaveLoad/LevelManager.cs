using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public LevelID currentLevel;

    public Level getCurrentLevel => _DATA.levelSettings.levels.Find(x => x.levelID == currentLevel);

    public int maxWave => getCurrentLevel.waves;

    public string currentLevelName => StringX.SplitCamelCase(getCurrentLevel.levelID.ToString());

    public int enemySpawnLocations => getCurrentLevel.enemySpawnLocations; 

}
