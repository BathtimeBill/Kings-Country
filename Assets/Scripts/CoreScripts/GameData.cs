using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    public Settings settings;

    private void Start()
    {
        for (int i = 0; i < _DATA.perkData.Count; i++)
            availablePerks.Add(_DATA.perkData[i].id);

        //if (currentLevel.id == LevelID.Ironwood && availablePerks.Contains(PerkID.Tower))
        //    availablePerks.Remove(PerkID.Tower);
    }

    #region Creature Units
    [Header("Creature Unit Data")]
    [BV.EnumList(typeof(CreatureID))]
    public List<UnitData> unitData;
    public UnitData GetUnit(CreatureID _id) => unitData.Find(x => x.id == _id);
    public bool IsTowerUnit(CreatureID _id) => unitData.Find(x => x.id == _id && x.towerUnit);
    public bool IsCreatureUnit(string _id) => unitData.Find(x => x.id.ToString() == _id);
    #endregion;

    #region Human Units
    [Header("Human Unit Data")]
    [BV.EnumList(typeof(HumanID))]
    public List<EnemyData> humanData;
    public EnemyData GetUnit(HumanID _id) => humanData.Find(x => x.id == _id);
    public EnemyType GetUnitType(HumanID _id) => GetUnit(_id).type;
    public bool IsHumanUnit(string _id) => humanData.Find(x => x.id.ToString() == _id);
    #endregion;

    #region Building Units
    [Header("Building Unit Data")]
    [BV.EnumList(typeof(BuildingID))]
    public List<BuildingData> buildingData;
    public BuildingData GetUnit(BuildingID _id) => buildingData.Find(x => x.id == _id);
    public bool IsBuildingUnit(string _id) => buildingData.Find(x => x.id.ToString() == _id);
    #endregion;

    #region Tools
    [Header("Tool Data")]
    [BV.EnumList(typeof(ToolID))]
    public List<ToolData> toolData;
    public ToolData GetTool(ToolID _id) => toolData.Find(x => x.id == _id);
    public bool CanUseTool(ToolID _id) => GetTool(_id).wildlifePrice <= _GM.wildlife && GetTool(_id).maegenPrice <= _GM.maegen;
    #endregion

    #region Wildlife
    [Header("Wildlife Data")]
    [BV.EnumList(typeof(WildlifeID))]
    public List<WildlifeData> wildlifeData;
    public WildlifeData GetWildlife(WildlifeID _id) => wildlifeData.Find(x => x.id == _id);
    #endregion

    #region Perks
    [Header("Perk Data")]
    [BV.EnumList(typeof(PerkID))]
    public List<PerkData> perkData;
    public List<PerkID> availablePerks;
    public List<PerkID> currentPerks;
    public PerkData GetPerk(PerkID _id) => perkData.Find(x=> x.id == _id);
    public bool HasPerk(PerkID perkID) => currentPerks.Contains(perkID);
    public void RemovePerk(PerkID perkID) => availablePerks.Remove(perkID);
    public void AddBackPerk(PerkID perkID) => availablePerks.Add(perkID);
    public PerkID GetRandomPerk() => ListX.GetRandomItemFromList(availablePerks);
    public bool CanObtainPerk => availablePerks.Count > 1;
    public void AddPerk(PerkID perkID)
    {
        if (perkID == PerkID.Fyre)
            _DATA.GetTool(ToolID.Fyre).damage += _DATA.GetTool(ToolID.Fyre).damage;

        currentPerks.Add(perkID);
    }
    #endregion

    #region Level Functions
    [Header("Level Data")]
    [BV.EnumList(typeof(LevelID))]
    public List<LevelData> levelData;
    public LevelData currentLevel => levelData.Find(x => x.id == _GM.thisLevel);
    public LevelData GetLevel(LevelID _id) => levelData.Find(x => x.id == _id);
    public LevelID currentLevelID => currentLevel.id;
    public bool LevelContains(LevelID _levelID, HumanID _humanID) => GetLevel(_levelID).availableHumans.Contains(_humanID);
    public bool LevelContains(LevelID _levelID, BuildingID _buildingID) => GetLevel(_levelID).availableBuildings.Contains(_buildingID);
    public bool levelAvailable(LevelID _id) => _SAVE.GetLevelSaveData(_id).unlocked && GetLevel(_id).canBePlayed;

    public int levelMaxDays => currentLevel.days;

    public string currentLevelName => StringX.SplitCamelCase(currentLevel.id.ToString());

    public int enemySpawnLocations => currentLevel.enemySpawnLocations;
    #endregion

    #region Editor
#if UNITY_EDITOR
    [Header("Unit CSV Files")]
    public TextAsset creatureDataSheet;
    public TextAsset humanDataSheet;
    public void LoadUnitDataFromFile()
    {
        string[,] grid = CSVReader.GetCSVGrid("/Assets/_Balancing/" + creatureDataSheet.name + ".csv");
        UnitData unit = new UnitData();
        List<string> keys = new List<string>();

        //First create a list for holding our key values
        for (int y = 0; y < grid.GetUpperBound(1); ++y)
        {
            keys.Add(grid[0, y]);
        }

        //Loop through the columns, adding the value to the appropriate key
        for (int x = 1; x < grid.GetUpperBound(0); x++)
        {
            Dictionary<string, string> columnData = new Dictionary<string, string>();
            for (int k = 0; k < keys.Count; k++)
            {
                columnData.Add(keys[k], grid[x, k]);
                //Debug.Log("Key: " + keys[k] + ", Value: " + grid[x, k]);
            }

            //Loop through the dictionary using the key values
            foreach (KeyValuePair<string, string> item in columnData)
            {
                // Gets a unit data based off the ID and updates the data
                if (item.Key.Contains("ID"))
                    unit.id = EnumX.ToEnum<CreatureID>(item.Value);
                if (item.Key.Contains("Name"))
                    unit.name = item.Value;
                if (item.Key.Contains("Description"))
                    unit.description = item.Value;
                if (item.Key.Contains("Health"))
                {
                    int temp = int.TryParse(item.Value, out temp) ? temp : 100;
                    unit.health = temp;
                }
                if (item.Key.Contains("Damage"))
                {
                    int temp = int.TryParse(item.Value, out temp) ? temp : 20;
                    unit.damage = temp;
                }
                if (item.Key.Contains("Speed"))
                {
                    int temp = int.TryParse(item.Value, out temp) ? temp : 10;
                    unit.speed = temp;
                }
                if (item.Key.Contains("Cost"))
                {
                    int temp = int.TryParse(item.Value, out temp) ? temp : 5;
                    unit.cost = temp;
                }
            }
            UpdateUnit(unit);
        }
    }

    private void UpdateUnit(UnitData _unitData)
    {
        UnitData unit = GetUnit(_unitData.id);
        unit.name = _unitData.name;
        unit.description = _unitData.description;
        unit.health = _unitData.health;
        unit.damage = _unitData.damage;
        unit.speed = _unitData.speed;
        unit.cost = _unitData.cost;

        //flag the object as "dirty" in the editor so it will be saved
        EditorUtility.SetDirty(unit);

        // Prompt the editor database to save dirty assets, committing your changes to disk.
        AssetDatabase.SaveAssets();
    }

    public void LoadHumanDataFromFile()
    {
        string[,] grid = CSVReader.GetCSVGrid("/Assets/_Balancing/" + humanDataSheet.name + ".csv");
        EnemyData enemy = new EnemyData();
        List<string> keys = new List<string>();

        //First create a list for holding our key values
        for (int y = 0; y < grid.GetUpperBound(1); ++y)
        {
            keys.Add(grid[0, y]);
        }

        //Loop through the columns, adding the value to the appropriate key
        for (int x = 1; x < grid.GetUpperBound(0); x++)
        {
            Dictionary<string, string> columnData = new Dictionary<string, string>();
            for (int k = 0; k < keys.Count; k++)
            {
                columnData.Add(keys[k], grid[x, k]);
                //Debug.Log("Key: " + keys[k] + ", Value: " + grid[x, k]);
            }

            //Loop through the dictionary using the key values
            foreach (KeyValuePair<string, string> item in columnData)
            {
                // Gets a unit data based off the ID and updates the data
                if (item.Key.Contains("ID"))
                    enemy.id = EnumX.ToEnum<HumanID>(item.Value);
                if (item.Key.Contains("Name"))
                    enemy.name = item.Value;
                if (item.Key.Contains("Description"))
                    enemy.description = item.Value;
                if (item.Key.Contains("Health"))
                {
                    int temp = int.TryParse(item.Value, out temp) ? temp : 100;
                    enemy.health = temp;
                }
                if (item.Key.Contains("Damage"))
                {
                    int temp = int.TryParse(item.Value, out temp) ? temp : 20;
                    enemy.damage = temp;
                }
                if (item.Key.Contains("Speed"))
                {
                    int temp = int.TryParse(item.Value, out temp) ? temp : 10;
                    enemy.speed = temp;
                }
            }
            UpdateHumans(enemy);
        }
    }

    private void UpdateHumans(EnemyData _enemyData)
    {
        EnemyData enemy = GetUnit(_enemyData.id);
        enemy.name = _enemyData.name;
        enemy.description = _enemyData.description;
        enemy.health = _enemyData.health;
        enemy.damage = _enemyData.damage;
        enemy.speed = _enemyData.speed;

        //flag the object as "dirty" in the editor so it will be saved
        EditorUtility.SetDirty(enemy);

        // Prompt the editor database to save dirty assets, committing your changes to disk.
        AssetDatabase.SaveAssets();
    }


    [CustomEditor(typeof(GameData))]
    public class GameDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GameData gameData = (GameData)target;
            DrawDefaultInspector();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Load Creature Data from file?"))
            {
                if (EditorUtility.DisplayDialog("Load Creature Spreadsheet Data", "Are you sure you want to load data? This will overwrite any existing data", "Yes", "No"))
                {
                    gameData.LoadUnitDataFromFile();
                    EditorUtility.SetDirty(gameData);
                }
            }
            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Load Human Data from file?"))
            {
                if (EditorUtility.DisplayDialog("Load Human Spreadsheet Data", "Are you sure you want to load data? This will overwrite any existing data", "Yes", "No"))
                {
                    gameData.LoadHumanDataFromFile();
                    EditorUtility.SetDirty(gameData);
                }
            }
            GUILayout.EndHorizontal();
        }
    }
#endif
    #endregion
}
