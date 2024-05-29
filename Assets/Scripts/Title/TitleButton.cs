using UnityEngine;
using UnityEngine.EventSystems;

public class TitleButton : InteractableButton
{
    public GameObject buttonTextLabel;
    public override void Start()
    {
        base.Start();
        Vector3 temp = buttonTextLabel.transform.localScale;
        temp.x = 0;
        buttonTextLabel.transform.localScale = temp;
    }
    public void SetInteractable(bool _interactable)
    {
        button.interactable = _interactable;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        _SM.PlaySound(_SM.buttonClickSound);
        TweenX.TweenTextScale(buttonTextLabel.transform, 1);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        //_SM.PlaySound(_SM.buttonClickSound);
        TweenX.TweenTextScale(buttonTextLabel.transform, 0);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        _SM.PlaySound(_SM.buttonClickSound);
    }

    public void TweenText(bool _in)
    {

    }
}
