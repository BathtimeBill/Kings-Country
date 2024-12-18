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
            _GAME.SetGameSpeed(GameSpeed.Fast);
        }
        else
        {
            isFast = false;
            icon.sprite = fastSpeedIcon;
            _GAME.SetGameSpeed(GameSpeed.Normal);
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
