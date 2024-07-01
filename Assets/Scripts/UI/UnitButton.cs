using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UnitButton : InteractableButton
{
    public UnitData unitData;
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
    }

    

    public void Cooldown(float _timeLeft)
    {
        cooldownFill.fillAmount = MathX.MapTo01(_timeLeft, 0, _GM.treeCooldown);
    }

    
    public void FadeInButton()
    {
        TweenX.KillTweener(fadeTweener);
        fadeTweener = canvasGroup.DOFade(1, fadeTime);
    }

    public void FadeOutButton()
    {
        TweenX.KillTweener(fadeTweener);
        fadeTweener = canvasGroup.DOFade(fadeToValue, fadeTime);
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

    #region overrides
    public override void ClickedButton()
    {
        GameEvents.ReportOnUnitButtonPressed(unitData);
        if (_GM.gameState == GameState.Play)
            unitPanel.StartCooldowns();
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        unitPanel.PointerEnter(this);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        unitPanel.PointerExit();
    }
    #endregion


    #region Editor
    public override void SetupButton()
    {
        icon.sprite = unitData.icon;
        title.text = unitData.name;
        cost.text = unitData.cost.ToString();

        if (tooltip != null)
        {
            tooltip.SetValues(unitData.name, unitData.description);
        }
    }

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
