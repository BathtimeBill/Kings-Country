using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractableButton : GameBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]
    public Button button;

    public virtual void Start()
    {
        button = GetComponent<Button>();
    }

    public void SetupButton()
    {
        //button.colors.highlightedColor = UICharInfo.
    }

    public virtual void OnPointerEnter(PointerEventData eventData){}

    public virtual void OnPointerExit(PointerEventData eventData){}

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

