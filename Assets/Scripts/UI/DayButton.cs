using UnityEngine.EventSystems;

public class DayButton : InteractableButton
{
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
    #endregion
}
