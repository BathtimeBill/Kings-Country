using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections;

public class UnitButton : InteractableButton
{
    public UnitData unitData;
    public Image icon;
    public Image cooldownFill;
    public TMP_Text title;
    public TMP_Text cost;
    public Tooltip tooltip;
    public CanvasGroup canvasGroup;
    public UnitPanel unitPanel;
    float fadeTime = 0.3f;
    float fadeToValue = 0.2f;
    Tweener fadeTweener;

    public override void Start()
    {
        base.Start();
        button.onClick.AddListener(() => PressedButton());
    }

    void PressedButton()
    {
        GameEvents.ReportOnUnitButtonPressed(unitData);
        if(_GM.gameState != GameState.Build)
        unitPanel.StartCooldowns();
    }

    public void Cooldown(float _timeLeft)
    {
        cooldownFill.fillAmount = MathX.MapTo01(_timeLeft, 0, _GM.treeCooldown);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        unitPanel.PointerEnter(this);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        unitPanel.PointerExit();
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
        cost.text = unitData.cost.ToString();
        
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

    private void OnMaegenChange(int _amount)
    {
        button.interactable = unitData.cost <= _amount;
        cost.color = unitData.cost > _amount ? Color.red : Color.white;
    }

    private void OnEnable()
    {
        GameEvents.OnMaegenChange += OnMaegenChange;
    }

    private void OnDisable()
    {
        GameEvents.OnMaegenChange -= OnMaegenChange;
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
