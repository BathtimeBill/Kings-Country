using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradePanel : GameBehaviour
{
    public CanvasGroup canvasGroup;
    public TMP_Text title;
    public TMP_Text description;
    public List<UpgradeID> upgradeOptions; 
    public string defaultText = "";

    public void SendUpgradeId(UpgradeID upgradeID)
    {
        for(int i=0;i< upgradeOptions.Count;i++)
        {
            if (upgradeOptions[i] == upgradeID)
                _UM.AddUpgrade(upgradeOptions[i]);
            else
                _UM.AddBackUpgrade(upgradeOptions[i]);
        }
        upgradeOptions.Clear();
    }

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
}
