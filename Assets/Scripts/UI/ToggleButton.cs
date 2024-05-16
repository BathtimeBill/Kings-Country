using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : GameBehaviour
{
    public BuildingID baseID;
    public Image image;
    public string title;
    public string description;
    public TogglePanel togglePanel;
    Toggle toggle;
    public void Start()
    {
        toggle = GetComponent<Toggle>();
        if(toggle != null )
            toggle.onValueChanged.AddListener((bool on) => PressedToggle(on));
    }

    void PressedToggle(bool _on)
    {
        if (!_on)
        {
            image.color = Color.white;
            return;
        }

        image.color = _UI.UISettings.titleColor;
        togglePanel.ShowPanel(this.baseID);
    }

    public void SetInteractable(bool _interactable)
    {
        if(toggle != null)
            toggle.interactable = _interactable;
    }
}
