using UnityEngine;

public class MapIconRotator : GameBehaviour
{
    public UnitType unitType;
    [BV.DrawIf("unitType", UnitType.Creature)]
    public CreatureID creatureID;
    [BV.DrawIf("unitType", UnitType.Human)]
    public HumanID humanID;
    [BV.DrawIf("unitType", UnitType.Building)]
    public BuildingID buildingID;

    private GameObject playerCam;
    private Quaternion targetRotation;
    private float targetScale;
    private SpriteRenderer sprite;


    private void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
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
            case UnitType.Creature:
                sprite.sprite = _DATA.GetUnit(creatureID).mapIcon;
                sprite.color = _COLOUR.GetColour(_DATA.GetUnit(creatureID).id);
                sprite.transform.localScale = Vector3.one * _SETTINGS.miniMap.creatureIconSize;
                break;
            case UnitType.Human:
                sprite.sprite = _DATA.GetUnit(humanID).mapIcon;
                sprite.color = _COLOUR.GetColour(_DATA.GetUnit(humanID).id);
                sprite.transform.localScale = Vector3.one * _SETTINGS.miniMap.humanIconSize;
                break;
            case UnitType.Building:
                sprite.sprite = _DATA.GetUnit(buildingID).mapIcon;
                sprite.color = _COLOUR.GetColour(_DATA.GetUnit(buildingID).id); 
                sprite.transform.localScale = Vector3.one * _SETTINGS.miniMap.buildingIconSize;
                break;
        }
    }

}
