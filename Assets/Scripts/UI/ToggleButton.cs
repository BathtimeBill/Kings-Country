using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleButton : GameBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BaseID baseID;
    public string title;
    public string description;
    public TogglePanel combatPanel;
    Toggle toggle;
    public void Start()
    {
        toggle = GetComponent<Toggle>();
        if(toggle != null )
            toggle.onValueChanged.AddListener((bool on) => PressedToggle(on));
    }

    void PressedToggle(bool _on)
    {
        print(name);
        if(baseID == BaseID.HomeTree)
        {
            _SM.PlaySound(_SM.attackSound);
        }
        if(baseID == BaseID.Hut)
        {
            _SM.PlaySound(_SM.defendSound);
        }
        if (baseID == BaseID.Hogyr)
        {
            _SM.PlaySound(_SM.formationSound);
        }
    }

    public void SetInteractable(bool _interactable)
    {
        if(toggle != null)
            toggle.interactable = _interactable;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        combatPanel.PointerEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        combatPanel.PointerExit();
    }
}
