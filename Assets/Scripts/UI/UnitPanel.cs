using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitPanel : GameBehaviour
{
    public UnitButton[] unitButtons;
    public CanvasGroup canvasGroup;
    public TMP_Text description;
    public TMP_Text healthStat;
    public TMP_Text damageStat;
    public TMP_Text speedStat;
    public string defaultText = "";

    float cooldownTimeLeft = 0;

    public void StartCooldowns()
    {
        cooldownTimeLeft = _GM.treeCooldown;
        for (int i = 0; i < unitButtons.Length; i++)
        {
            unitButtons[i].button.interactable = false;
        }
    }

    void StopCooldowns()
    {
        for (int i = 0; i < unitButtons.Length; i++)
        {
            unitButtons[i].button.interactable = unitButtons[i].unitData.cost <= _GM.maegen;
        }
    }

    void Update()
    {
        if (cooldownTimeLeft >= 0)
        {
            cooldownTimeLeft -= Time.deltaTime;
            for (int i = 0; i < unitButtons.Length; i++)
            {
                unitButtons[i].Cooldown(cooldownTimeLeft);
            }
        }
        else
            StopCooldowns();
    }

    public void PointerEnter(UnitButton _unitButton)
    {
        for(int i = 0; i < unitButtons.Length; i++)
        {
            if(_unitButton == unitButtons[i])
            {
                unitButtons[i].FadeInButton();
                description.text = _unitButton.unitData.description;
                healthStat.text = _unitButton.unitData.health.ToString();
                damageStat.text = _unitButton.unitData.damage.ToString();
                speedStat.text = _unitButton.unitData.speed.ToString();
            }
            else
            {
                unitButtons[i].FadeOutButton();
            }
        }
    }

    public void PointerExit()
    {
        for (int i = 0; i < unitButtons.Length; i++)
        {
            unitButtons[i].FadeInButton();
            description.text = defaultText;
            healthStat.text = "-";
            damageStat.text = "-";
            speedStat.text = "-";
        }
    }
}
