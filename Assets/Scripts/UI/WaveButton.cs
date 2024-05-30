using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WaveButton : InteractableButton
{
    public override void Start()
    {
        base.Start();
        button.onClick.AddListener(() => PressedButton());
    }

    void PressedButton()
    {
        GameEvents.ReportOnWaveBegin();
    }
    public void SetInteractable(bool _interactable)
    {
        button.interactable = _interactable;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        _SM.PlaySound(_SM.buttonClickSound);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        //_SM.PlaySound(_SM.buttonClickSound);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        _SM.PlaySound(_SM.buttonClickSound);
    }
}
