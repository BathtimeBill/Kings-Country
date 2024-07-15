public class UnitWeaponCollider : GameBehaviour
{
    public UnitType unitType;
    [BV.DrawIf("unitType", UnitType.Creature)]
    public CreatureID creatureID;
    [BV.DrawIf("unitType", UnitType.Human)]
    public HumanID humanID;
    [BV.DrawIf("unitType", UnitType.Building)]
    public BuildingID buildingID;

    private int damage;
    private string unitID;
    public int Damage => damage;
    public string UnitID => unitID;

    void Start()
    {
        Setup();
    }

    private void Setup()
    {
        switch (unitType)
        {
            case UnitType.Creature:
                damage = _DATA.GetUnit(creatureID).damage;
                unitID = _DATA.GetUnit(creatureID).id.ToString();
                break;
            case UnitType.Human:
                damage = _DATA.GetUnit(humanID).damage;
                unitID = _DATA.GetUnit(humanID).id.ToString();
                break;
            case UnitType.Building:
                damage = _DATA.GetUnit(buildingID).damage;
                unitID = _DATA.GetUnit(buildingID).id.ToString();
                break;
        }
    }
}
