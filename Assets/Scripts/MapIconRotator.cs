using UnityEngine;

public class MapIconRotator : GameBehaviour
{
    public UnitType unitType;
    [DrawIf("unitType", UnitType.Creature)]
    public CreatureID creatureID;
    [DrawIf("unitType", UnitType.Human)]
    public HumanID humanID;

    private GameObject playerCam;
    private Quaternion targetRotation;
    private float targetScale;
    private SpriteRenderer sprite;


    private void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        SetIconData();
        playerCam = GameObject.FindGameObjectWithTag("PlayerCam");
        targetScale = transform.localScale.z - transform.localScale.z*2;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, targetScale);
    }

    private void Update()
    {
        if (!_PS.miniMapRotation)
            return;

        targetRotation = Quaternion.Euler(0, playerCam.transform.localEulerAngles.y, 0);
        transform.rotation = targetRotation;
    }

    public void SetIconData()
    {
        if(unitType == UnitType.Human)
        {
            sprite.sprite = _DATA.GetEnemyUnit(humanID).mapIcon;
            sprite.color = _DATA.GetEnemyUnit(humanID).mapIconColour;
        }
        else if(unitType == UnitType.Creature)
        {
            sprite.sprite = _DATA.GetUnit(creatureID).mapIcon;
            sprite.color = _DATA.GetUnit(creatureID).mapIconColour;
        }
    }

}
