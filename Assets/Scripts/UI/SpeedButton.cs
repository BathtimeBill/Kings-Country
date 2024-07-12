using UnityEngine;
using UnityEngine.EventSystems;

public class SpeedButton : InteractableButton
{
    public bool isFast;
    public Sprite normalSpeedIcon;
    public Sprite fastSpeedIcon;
    public InGamePanel speedPanel;

    public override void Start()
    {
        base.Start();
    }

    #region overrides
    public override void ClickedButton()
    {
        if(isFast== false)
        {
            isFast = true;
            icon.sprite = normalSpeedIcon;
            _GM.SpeedGame();
        }
        else
        {
            isFast = false;
            icon.sprite = fastSpeedIcon;
            _GM.SetGame();
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        speedPanel.PointerEnter(eventData);
        _SM.PlaySound(_SM.buttonClickSound);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        speedPanel.PointerExit(eventData);
        //_SM.PlaySound(_SM.buttonClickSound);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        _SM.PlaySound(_SM.buttonClickSound);
    }
    #endregion
}
