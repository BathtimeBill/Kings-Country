using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Serialization;

public class UnitButton : InteractableButton
{
    [FormerlySerializedAs("unitData")] public GuardianData guardianData;
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
        button.interactable = guardianData.cost <= _amount;
        cost.color = guardianData.cost > _amount ? Color.red : Color.white;
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
        GameEvents.ReportOnUnitButtonPressed(guardianData);
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

    public void SetupButton(GuardianData _guardianData)
    {
        guardianData = _guardianData;
        icon.sprite = guardianData.icon;
        title.text = guardianData.name;
        cost.text = guardianData.cost.ToString();

        if (tooltip != null)
        {
            tooltip.SetValues(guardianData.name, guardianData.description);
        }
    }
}
