using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractableButton : GameBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public Button button;
    public Image icon;
    [HideInInspector]
    public bool interactable = true;

    public virtual void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => ClickedButton());
    }
    public virtual void Start() {}

    public virtual void SetInteractable(bool _interactable)
    {
        if (button == null)
            return;

        interactable = _interactable;
        button.interactable = interactable;

        if (GetComponent<Coffee.UIExtensions.ShinyEffectForUGUI>() != null)
            GetComponent<Coffee.UIExtensions.ShinyEffectForUGUI>().enabled = interactable;

        if(icon != null)
        {
            icon.color = interactable ? _COLOUR.toggleIconInactiveColor : _COLOUR.toggleIconDisabledColor;
        }
    }

    public virtual void ClickedButton() {}
    public virtual void OnPointerEnter(PointerEventData eventData) {}
    public virtual void OnPointerExit(PointerEventData eventData) {}
    public virtual void OnPointerClick(PointerEventData eventData) {}
    public virtual void OnPointerDown(PointerEventData eventData) {}
    public virtual void OnPointerUp(PointerEventData eventData) {}
    public virtual void SetupButton(){}

    public virtual void ChangeHighlightColor(Color _c)
    {
        var colors = button.colors;
        colors.highlightedColor = _c;
        button.colors = colors;
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

public class HoldButton : InteractableButton
{
    public Image fillImage;
    public float fillSpeed = 0.5f;
    bool filling = false;

    void Update()
    {
        if (!interactable)
            return;
        
        if (filling)
        {
            fillImage.fillAmount += fillSpeed * Time.deltaTime;
            if (fillImage.fillAmount >= 1)
            {
                OnButtonFilled();
            }
            //TODO
            //Improve upgrade button shake
            //currentTime -= Time.deltaTime / 3;
            //currentTime = Mathf.Clamp(currentTime, 0.01f, 1);
            //Vector3 nextPos = transform.position;
            //nextPos.y = pivot.y + height + height * Mathf.Sin(((Mathf.PI * 2) / currentTime) * timeSinceStart);
            //timeSinceStart += Time.deltaTime;
            //transform.position = nextPos;
        }
        else
        {
            fillImage.fillAmount -= (fillSpeed * 3) * Time.deltaTime;
            //transform.position = startPosition;
            //currentTime = timePeriod * 3;
        }
    }

    public virtual void OnButtonFilled()
    {
        filling = false;
        fillImage.fillAmount = 0;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        filling = true;
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        filling = false;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {

    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        filling = false;
    }
}