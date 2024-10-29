using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractableButton : GameBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public Button button;
    public Image icon;
    public Image highlight;
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

    public virtual void SetActivated(bool _activated)
    {
        if (highlight == null)
            return;

        highlight.color = _activated ? _COLOUR.highlightedColor : _COLOUR.transparentColor;
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
    public float fillSpeed = 3f;
    private bool filling = false;

    private float oscillationSpeed = 2f;
    private float intensityIncreaseRate = 1f;
    private Vector3 originalPosition;
    private float timer = 0f;

    public override void Start()
    {
        base.Start();
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (!interactable)
            return;
        
        if (filling)
        {
            fillImage.fillAmount += Time.deltaTime / fillSpeed;
            if (fillImage.fillAmount >= 1)
            {
                OnButtonFilled();
            }
            // Calculate the vertical oscillation
            float yOffset = Mathf.Cos(timer * oscillationSpeed) * intensityIncreaseRate;
            Vector3 newPosition = originalPosition + new Vector3(0f, yOffset, 0f);
            transform.localPosition = newPosition;
            timer += Time.deltaTime;
        }
        else
        {
            fillImage.fillAmount -= Time.deltaTime * fillSpeed;
            transform.localPosition = originalPosition;
            timer = 0;
        }
        oscillationSpeed = fillImage.fillAmount * 40;
        intensityIncreaseRate = fillImage.fillAmount * fillSpeed;
    }

    public virtual void OnButtonFilled()
    {
        filling = false;
        fillImage.fillAmount = 0;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        filling = true;
        fillImage.fillAmount = 0.15f;
        if (GetComponent<AudioSource>() != null)
            GetComponent<AudioSource>().Play();
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        StopFill();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {

    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        StopFill();
    }

    private void StopFill()
    {
        filling = false;
        if (GetComponent<AudioSource>() != null)
            GetComponent<AudioSource>().Stop();
    }
}