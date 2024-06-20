public class UnitWeaponCollider : GameBehaviour
{
    public UnitType unitType;
    [DrawIf("unitType", UnitType.Creature)]
    public CreatureID creatureID;
    [DrawIf("unitType", UnitType.Human)]
    public HumanID humanID;
    [DrawIf("unitType", UnitType.Building)]
    public BuildingID buildingID;

    private int damage;
    public int Damage => damage;

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
                break;
            case UnitType.Human:
                damage = _DATA.GetUnit(humanID).damage;
                break;
            case UnitType.Building:
                damage = _DATA.GetUnit(buildingID).damage;
                break;
        }
    }

    private void OnEnable()
    {
        
    }
}
