using UnityEngine;

public class MapIcon : GameBehaviour
{
    public UnitType unitType;
    [BV.DrawIf("unitType", UnitType.Guardian)]
    public CreatureID creatureID;
    [BV.DrawIf("unitType", UnitType.Human)]
    public HumanID humanID;
    [BV.DrawIf("unitType", UnitType.Site)]
    public SiteID siteID;
    [BV.DrawIf("unitType", UnitType.Tree)]
    public TreeID treeID;
    public SpriteRenderer sprite;

    private GameObject playerCam;
    private Quaternion targetRotation;
    private float targetScale;

    public void ChangeMapIconColor(Color _color) => sprite.color = _color;
    private void Start()
    {
        if (!_SETTINGS.miniMap.showIcons)
            sprite.enabled = false;
        else
            SetIconData();
        playerCam = GameObject.FindGameObjectWithTag("PlayerCam");
        targetScale = transform.localScale.z - transform.localScale.z*2;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, targetScale);
    }

    private void Update()
    {
        if (!_SETTINGS.miniMap.showIcons)
            return;

        if (!_PLAYER.miniMapRotation)
            return;

        targetRotation = Quaternion.Euler(0, playerCam.transform.localEulerAngles.y, 0);
        transform.rotation = targetRotation;
    }

    public void SetIconData()
    {
        switch(unitType)
        {
            case UnitType.Guardian:
                sprite.sprite = _DATA.GetUnit(creatureID).mapIcon;
                sprite.color = _COLOUR.GetColour(_DATA.GetUnit(creatureID).id);
                sprite.transform.localScale = Vector3.one * _SETTINGS.miniMap.creatureIconSize;
                break;
            case UnitType.Human:
                sprite.sprite = _DATA.GetUnit(humanID).mapIcon;
                sprite.color = _COLOUR.GetColour(_DATA.GetUnit(humanID).id);
                sprite.transform.localScale = Vector3.one * _SETTINGS.miniMap.humanIconSize;
                break;
            case UnitType.Site:
                sprite.sprite = _DATA.GetSiteData(siteID).mapIcon;
                sprite.color = _COLOUR.GetColour(_DATA.GetSiteData(siteID).id); 
                sprite.transform.localScale = Vector3.one * _SETTINGS.miniMap.buildingIconSize;
                break;
            case UnitType.Tree:
                sprite.sprite = _DATA.GetTree(treeID).icon;
                sprite.color = _COLOUR.GetColour(_DATA.GetTree(treeID).id); 
                sprite.transform.localScale = Vector3.one * _SETTINGS.miniMap.treeIconSize;
                break;
        }
    }

}
