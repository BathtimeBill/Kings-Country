using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorButtons : MonoBehaviour
{
    public UnitPanel unitPanel;

    #region Editor
#if UNITY_EDITOR

    [CustomEditor(typeof(EditorButtons))]
    public class EditorButtonsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorButtons editorButtons = (EditorButtons)target;
            DrawDefaultInspector();
            GUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Show Unit Panel"))
            {
                editorButtons.unitPanel.ShowUnitPanel(true);
                EditorUtility.SetDirty(editorButtons);
            }
            if (GUILayout.Button("Hide Unit Panel"))
            {
                editorButtons.unitPanel.ShowUnitPanel(false);
                EditorUtility.SetDirty(editorButtons);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Show Home Tree Units"))
            {
                editorButtons.unitPanel.ShowPanel(SiteID.HomeTree);
                EditorUtility.SetDirty(editorButtons);
            }
            if (GUILayout.Button("Show Hut Units"))
            {
                editorButtons.unitPanel.ShowPanel(SiteID.Hut);
                EditorUtility.SetDirty(editorButtons);
            }
            if (GUILayout.Button("Show Horgr Units"))
            {
                editorButtons.unitPanel.ShowPanel(SiteID.Horgr);
                EditorUtility.SetDirty(editorButtons);
            }
            GUILayout.EndHorizontal();
            
        }
    }
#endif
    #endregion
}
