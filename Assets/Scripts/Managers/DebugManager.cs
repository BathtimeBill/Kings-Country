using UnityEngine;
using UnityEditor;
public class DebugManager : GameBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            GameEvents.ReportOnDayOver(_currentDay);

        if(Input.GetKeyDown(KeyCode.K))
            StartCoroutine(_EM.KillAllEnemies());

        if (Input.GetKeyDown(KeyCode.L))
            _GM.WildlifeInstantiate();

        if (Input.GetKeyDown(KeyCode.M))
            _GM.IncreaseMaegen(6);

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (FindObjectOfType<ExperienceMeter>())
                FindObjectOfType<ExperienceMeter>().IncreaseExperience(100);
        }
    }

    public void ShowLevelColliders(bool _show)
    {
        GameObject[] col = GameObject.FindGameObjectsWithTag("LevelCollider");
        ObjectX.ToggleObjects(col, _show);
    }
    
    
    #region Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(DebugManager))]
    [CanEditMultipleObjects]

    public class DebugManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DebugManager debugManager = (DebugManager)target;
            DrawDefaultInspector();
            GUILayout.Space(20);

            //GUI.backgroundColor = Color.blue;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Show Level Colliders"))
            {
                debugManager.ShowLevelColliders(true);
                EditorUtility.SetDirty(debugManager);
            }
            if (GUILayout.Button("Hide Level Colliders"))
            {
                debugManager.ShowLevelColliders(false);
                EditorUtility.SetDirty(debugManager);
            }
           /* if (GUILayout.Button("Hide Title Labels"))
            {
                debugManager.ToggleTitleText(false);
                EditorUtility.SetDirty(debugManager);
            }*/
            GUILayout.EndHorizontal();
            
        }
    }
#endif
    #endregion
}
