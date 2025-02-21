using UnityEngine;
using UnityEditor;
public class DebugManager : GameBehaviour
{
    public GameObject debugPanel;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backslash))
            debugPanel.SetActive(!debugPanel.activeSelf);
            
        if (Input.GetKeyDown(KeyCode.O))
            GameEvents.ReportOnDayOver(_CurrentDay);

        if(Input.GetKeyDown(KeyCode.K))
            StartCoroutine(_EM.KillAllEnemies());

        if (Input.GetKeyDown(KeyCode.L))
            _GAME.WildlifeInstantiate();

        if (Input.GetKeyDown(KeyCode.M))
            _GAME.IncreaseMaegen(6);
        
        if (Input.GetKeyDown(KeyCode.P))
            _EM.SpawnEnemy(_DATA.GetEnemy(EnemyID.Dog).playModel, _EM.RandomSpawnPoint.position);

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (FindFirstObjectByType<ExperienceMeter>())
                FindFirstObjectByType<ExperienceMeter>().IncreaseExperience(100);
        }
        
        if(Input.GetKeyDown(KeyCode.H))
            _EM.SpawnSpecificEnemy();

        if (Input.GetKeyDown(KeyCode.U))
        {
            GameObject go = GameObject.FindFirstObjectByType<Enemy>().gameObject;
            go.GetComponent<Enemy>().Die(go.GetComponent<Enemy>(), GuardianID.Orcus.ToString(), DeathID.Launch);
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
