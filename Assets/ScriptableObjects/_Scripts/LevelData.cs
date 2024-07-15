using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "BGG/Level Data", order = 6)]
public class LevelData : ScriptableObject
{
    public LevelID id;
    public new string name;
    [Tooltip("The name of the unity scene")]
    public string levelReference;
    [TextArea]
    public string description;
    public DifficultyRating difficultyRating;
    public int days;
    public int spawnPoints;
    public Sprite icon;
    public bool unlocked;
    [BV.EnumList(typeof(SeasonID))]
    public List<SeasonID> unlockedSeasons;
    [Space]
    public List<BuildingID> availableBuildings;
    public List<ToolID> availableTrees;
    public List<HumanID> availableHumans;
    [Space]
    public int enemySpawnLocations;
    [BV.EnumList(typeof(DayID))]
    public List<SpawnAmounts> spawnAmounts;

    #region Editor
#if UNITY_EDITOR
    [Header("CSV File")]
    public TextAsset csvFile;
    public void LoadDataFromFile()
    {
        string[,] grid = CSVReader.GetCSVGrid("/Assets/_Balancing/" + csvFile.name + ".csv");
        spawnAmounts.Clear();
        spawnAmounts = new List<SpawnAmounts>();
        List<string> keys = new List<string>();

        //First create a list for holding our key values
        for (int y = 0; y < grid.GetUpperBound(0); ++y)
        {
            keys.Add(grid[y, 0]);
        }

        //Loop through the rows, adding the value to the appropriate key
        for (int x = 1; x < grid.GetUpperBound(1); x++)
        {
            Dictionary<string, string> columnData = new Dictionary<string, string>();
            for (int k = 0; k < keys.Count; k++)
            {
                columnData.Add(keys[k], grid[k, x]);
                //Debug.Log("Key: " + keys[k] + ", Value: " + grid[x, k]);
            }

            SpawnAmounts sa = new SpawnAmounts();
            //Loop through the dictionary using the key values
            foreach (KeyValuePair<string, string> item in columnData)
            {
                //Gets a unit data based off the ID and updates the spawn amounts data
                if (item.Key.Contains(HumanID.Logger.ToString()))
                    sa.logger = int.TryParse(item.Value, out sa.logger) ? sa.logger : 0;

                if (item.Key.Contains(HumanID.Lumberjack.ToString()))
                    sa.lumberjack = int.TryParse(item.Value, out sa.lumberjack) ? sa.lumberjack : 0;

                if (item.Key.Contains(HumanID.LogCutter.ToString()))
                    sa.logcutter = int.TryParse(item.Value, out sa.logcutter) ? sa.logcutter : 0;

                if (item.Key.Contains(HumanID.Wathe.ToString()))
                    sa.wathe = int.TryParse(item.Value, out sa.wathe) ? sa.wathe : 0;

                if (item.Key.Contains(HumanID.Poacher.ToString()))
                    sa.poacher = int.TryParse(item.Value, out sa.poacher) ? sa.poacher : 0;

                if (item.Key.Contains(HumanID.Bjornjeger.ToString()))
                    sa.bjornjeger = int.TryParse(item.Value, out sa.bjornjeger) ? sa.bjornjeger : 0;

                if (item.Key.Contains(HumanID.Dreng.ToString()))
                    sa.dreng = int.TryParse(item.Value, out sa.dreng) ? sa.dreng : 0;

                if (item.Key.Contains(HumanID.Berserkr.ToString()))
                    sa.berserkr = int.TryParse(item.Value, out sa.berserkr) ? sa.berserkr : 0;

                if (item.Key.Contains(HumanID.Knight.ToString()))
                    sa.knight = int.TryParse(item.Value, out sa.knight) ? sa.knight : 0;
            }
            UpdateLevel(sa);
        }
    }

    private void UpdateLevel(SpawnAmounts _spawnAmount)
    {
        spawnAmounts.Add(_spawnAmount);

        //flag the object as "dirty" in the editor so it will be saved
        EditorUtility.SetDirty(this);

        // Prompt the editor database to save dirty assets, committing your changes to disk.
        AssetDatabase.SaveAssets();
    }


    [CustomEditor(typeof(LevelData))]
    public class LevelDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            LevelData gameData = (LevelData)target;
            DrawDefaultInspector();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Load Data from file?"))
            {
                if (EditorUtility.DisplayDialog("Load Spreadsheet Data", "Are you sure you want to load data? This will overwrite any existing data", "Yes", "No"))
                {
                    gameData.LoadDataFromFile();
                    EditorUtility.SetDirty(gameData);
                }
            }
            GUILayout.EndHorizontal();
        }
    }
#endif
    #endregion
}