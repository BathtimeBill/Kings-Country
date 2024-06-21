using UnityEngine.EventSystems;

public class SeasonToggle : Toggles
{
    public SeasonID seasonID;
    OverWorldManager overworldManager;

    private void Awake()
    {
        overworldManager = FindObjectOfType<OverWorldManager>();
    }

    public override void PressedToggle(bool _on)
    {
        base.PressedToggle(_on);
        overworldManager.ChangeSeason(seasonID);
    }

    public override void SetInteractable(bool _interactable)
    {
        base.SetInteractable(_interactable);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        overworldManager.SeasonHover(seasonID, true);
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        overworldManager.SeasonHover(seasonID, false);
        base.OnPointerExit(eventData);
    }
}
