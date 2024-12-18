using UnityEngine;
using UnityEngine.UI;
using Coffee.UIExtensions;

public class ToggleButton : GameBehaviour
{
    public SiteID baseID;
    public Image image;
    public string title;
    public string description;
    public UnitPanel togglePanel;
    Toggle toggle;

    private void Start()
    {
        toggle = GetComponent<Toggle>();
        if (toggle != null)
            toggle.onValueChanged.AddListener((bool on) => PressedToggle(on));
    }

    void PressedToggle(bool _on)
    {
        if (!_on)
        {
            image.color = Color.white;
            return;
        }

        image.color = _SETTINGS.colours.titleColor;
        togglePanel.ShowPanel(this.baseID);

        if (baseID == SiteID.HomeTree)
        {
            GameEvents.ReportOnSiteSelected(SiteID.HomeTree, true);
            GameEvents.ReportOnSiteSelected(SiteID.Hut, false);
            GameEvents.ReportOnSiteSelected(SiteID.Horgr, false);
        }
        if (baseID == SiteID.Hut)
        {
            GameEvents.ReportOnSiteSelected(SiteID.HomeTree, false);
            GameEvents.ReportOnSiteSelected(SiteID.Hut, true);
            GameEvents.ReportOnSiteSelected(SiteID.Horgr, false);
        }
        if (baseID == SiteID.Horgr)
        {
            GameEvents.ReportOnSiteSelected(SiteID.HomeTree, false);
            GameEvents.ReportOnSiteSelected(SiteID.Hut, false);
            GameEvents.ReportOnSiteSelected(SiteID.Horgr, true);
        }
    }

    public void SetInteractable(bool _interactable)
    {
        if(toggle != null)
            toggle.interactable = _interactable;
    }

    public void SetShiny(bool _shiny)
    {
        if(GetComponent<ShinyEffectForUGUI>() != null)
            GetComponent<ShinyEffectForUGUI>().enabled = _shiny;
    }
}
