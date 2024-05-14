using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UnitButton : InteractableButton, IPointerEnterHandler, IPointerExitHandler
{
    public UnitData unitData;
    public Image icon;
    public TMP_Text title;
    public TMP_Text cost;
    public InfoBox infoBox;
    public Tooltip tooltip;
    public CanvasGroup canvasGroup;
    public UnitButtonManager buttonManager;
    float fadeTime = 0.3f;
    float fadeToValue = 0.2f;
    Tweener fadeTweener;
    //Button button;

    public void Start()
    {
        base.Start();
        //button = GetComponent<Button>();
        button.onClick.AddListener(() => PressedButton());
    }

    void PressedButton()
    {
        GameEvents.ReportOnUnitButtonPressed(unitData);
    }

    public void HoverButton()
    {
        infoBox.OnButtonHover(unitData.description, unitData.GetStats());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonManager.PointerEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonManager.PointerExit();
    }

    public void FadeInButton()
    {
        KillTweener(fadeTweener);
        fadeTweener = canvasGroup.DOFade(1, fadeTime);
    }

    public void FadeOutButton()
    {
        KillTweener(fadeTweener);
        fadeTweener = canvasGroup.DOFade(fadeToValue, fadeTime);
    }


    public void SetupButton()
    {
        icon.sprite = unitData.icon;
        title.text = unitData.unitName;
        cost.text = unitData.stats.price.ToString();
        
        if(tooltip != null)
        {
            tooltip.SetValues(unitData.name, unitData.description);
        }
    }

    void KillTweener(Tweener _tweener)
    {
        if(_tweener != null) 
           _tweener.Kill();
    }

    #region Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(UnitButton))]
    [CanEditMultipleObjects]

    public class UnitButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            UnitButton unit = (UnitButton)target;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Setup"))
            {
                unit.SetupButton();
                EditorUtility.SetDirty(unit);
            }
            GUILayout.EndHorizontal();
           
            DrawDefaultInspector();
        }
    }
#endif
    #endregion
}
