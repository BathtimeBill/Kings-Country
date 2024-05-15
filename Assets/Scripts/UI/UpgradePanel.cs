using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradePanel : GameBehaviour
{
    public CanvasGroup canvasGroup;
    public TMP_Text title;
    public TMP_Text description;
    public string defaultText = "";
    public GameObject selectedButton;

    public void PointerEnter(UpgradeButton _upgradeButton)
    {
        Upgrade up = _upgradeButton.upgrade;
        title.text = up.name;
        description.text = up.description;
    }

    public void PointerExit()
    {
        title.text = "";
        description.text = "";
    }

    public void SetSelected(GameObject _go)
    {
        selectedButton = _go;
    }
}
