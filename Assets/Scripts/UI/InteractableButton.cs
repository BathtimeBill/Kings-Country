using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InteractableButton : MonoBehaviour
{
    [HideInInspector]
    public Button button;

    public void Start()
    {
        button = GetComponent<Button>();
    }

    public void SetupButton()
    {
        //button.colors.highlightedColor = UICharInfo.
    }

    #region Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(InteractableButton))]
    [CanEditMultipleObjects]

    public class InteractableButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            InteractableButton button = (InteractableButton)target;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Setup Button"))
            {
                button.SetupButton();
                EditorUtility.SetDirty(button);
            }
            GUILayout.EndHorizontal();

            DrawDefaultInspector();
        }
    }
#endif
    #endregion
}

