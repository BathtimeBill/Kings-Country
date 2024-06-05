using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PerkPanel : GameBehaviour
{
    public CanvasGroup canvasGroup;
    public TMP_Text title;
    public TMP_Text description;
    public List<PerkID> perkOptions; 
    public string defaultText = "";

    public void SendPerkId(PerkID upgradeID)
    {
        for(int i=0;i< perkOptions.Count;i++)
        {
            if (perkOptions[i] == upgradeID)
                _PERK.AddPerk(perkOptions[i]);
            else
                _PERK.AddBackPerk(perkOptions[i]);
        }
        perkOptions.Clear();
    }

    public void PointerEnter(PerkButton _perkButton)
    {
        PerkData perk = _perkButton.perkData;
        title.text = perk.name;
        description.text = perk.description;
    }

    public void PointerExit()
    {
        title.text = "";
        description.text = "";
    }
}
