using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CreateAssetMenu(fileName = "Level Data", menuName = "BGG/Level Data", order = 6)]
public class LevelData : ScriptableObject
{
    public LevelID id;
    public new string name;
    [TextArea]
    public string description;
    public LevelNumber number;
    public DifficultyRating difficultyRating;
    public int days;
    public int spawnPoints;
    public Sprite icon;
    public SceneAsset levelScene;
    public bool unlocked;
    [Space]
    public List<BuildingID> availableBuildings;
    public List<ToolID> availableTrees;
    public List<HumanID> availableHumans;
    [Space]
    public int enemySpawnLocations;
    [BV.ListName("Day: ")]
    public List<SpawnAmounts> spawnAmounts;


    #region Editor
    [CustomEditor(typeof(SpawnAmounts))]
    public class SpawnAmountsEditor : Editor
    {
        private SerializedProperty _dialogues;

        private void OnEnable()
        {
            // do this only once here
            _dialogues = serializedObject.FindProperty("dialogue");
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            serializedObject.Update();

            // Ofcourse you also want to change the list size here
            _dialogues.arraySize = EditorGUILayout.IntField("Size", _dialogues.arraySize);

            for (int i = 0; i < _dialogues.arraySize; i++)
            {
                var dialogue = _dialogues.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(dialogue, new GUIContent("Dialogue " + i), true);
            }

            // Note: You also forgot to add this
            serializedObject.ApplyModifiedProperties();
        }
    }
    #endregion
}