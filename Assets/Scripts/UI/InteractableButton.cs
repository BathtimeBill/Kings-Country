using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractableButton : GameBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public Button button;
    public Image icon;

    void Awake()
    {
        button = myButton;
        button.onClick.AddListener(() => ClickedButton());
    }
    public virtual void Start()
    {

    }

    public virtual void ClickedButton() 
    {
    }

    public void SetInteractable(bool _interactable)
    {
        if (button == null)
            return;

        button.interactable = _interactable;

        if (GetComponent<Coffee.UIExtensions.ShinyEffectForUGUI>() != null)
            GetComponent<Coffee.UIExtensions.ShinyEffectForUGUI>().enabled = _interactable;
    }

    public virtual void SetupButton()
    {
        //button.colors.highlightedColor = UICharInfo.
    }

    public virtual void OnPointerEnter(PointerEventData eventData) {}
    public virtual void OnPointerExit(PointerEventData eventData) { }
    public virtual void OnPointerClick(PointerEventData eventData) {}
    public virtual void OnPointerDown(PointerEventData eventData) {}
    public virtual void OnPointerUp(PointerEventData eventData) {}

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

