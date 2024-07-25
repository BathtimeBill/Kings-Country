using TMPro;
using UnityEngine.EventSystems;

public class UnitUpgradeButton : HoldButton
{
    public UpgradePanel upgradePanel;
    public TMP_Text costText;
    public TMP_Text notEnoughText;
    private bool enoughMaegen;
    string notEnough = "Not Enough";

    public override void Start()
    {
        base.Start();
        CheckMaegen();
        fillImage.fillAmount = 0;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        upgradePanel.ShowStatsUpgrade();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(enoughMaegen)
            base.OnPointerDown(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        upgradePanel.SetStatValues();
    }

    public override void OnButtonFilled()
    {
        base.OnButtonFilled();
        _UPGRADE.UpgradeObject();
        upgradePanel.titleManager.progressManager.DecreaseCurrentMaegen();
        CheckMaegen();
    }

    private void CheckMaegen()
    {
        enoughMaegen = _SAVE.GetCurrentMaegen >= upgradePanel.upgradeCost;
        if (enoughMaegen)
        {
            SetInteractable(true);
            costText.color = _SETTINGS.colours.highlightedColor;
            notEnoughText.text = "";
            base.ChangeHighlightColor(_SETTINGS.colours.highlightedColor);
        }
        else
        {
            SetInteractable(false);
            costText.color = _SETTINGS.colours.cooldownColor;
            notEnoughText.text = notEnough;
            base.ChangeHighlightColor(_SETTINGS.colours.cooldownColor);
        }
    }
}
