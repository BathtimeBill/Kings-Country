using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeButton : InteractableButton
{
    public Upgrade upgrade;
    public Image icon;
    public UpgradePanel upgradePanel;
    public override void Start()
    {
        base.Start();
        button.onClick.AddListener(() => PressedButton());
    }

    void PressedButton()
    {
        upgradePanel.SendUpgradeId(upgrade.id);
        GameEvents.ReportOnUpgradeSelected(upgrade.id);
    }

    public void SetUpgrade(UpgradeID _id)
    {
        upgrade = _UM.GetUpgrade(_id);
        upgradePanel.upgradeOptions.Add(_id);
        icon.sprite = upgrade.icon;
    }

    public void SetInteractable(bool _interactable)
    {
        button.interactable = _interactable;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        upgradePanel.PointerEnter(this);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        upgradePanel.PointerExit();
    }
}
