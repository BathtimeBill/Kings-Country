using UnityEngine.EventSystems;

public class DayButton : InteractableButton
{
    public InGamePanel dayPanel;
    public override void Start()
    {
        base.Start();
    }

    #region overrides
    public override void ClickedButton()
    {
        _GM.BeginNewDay();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        _SM.PlaySound(_SM.buttonClickSound);
        if(_BuildPhase)
            dayPanel.PointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        dayPanel.PointerExit(eventData);
        //_SM.PlaySound(_SM.buttonClickSound);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        _SM.PlaySound(_SM.buttonClickSound);
    }
    #endregion
}
