using UnityEngine;
using UnityEngine.Serialization;

public class UnitWeaponCollider : GameBehaviour
{
    public UnitType unitType;
    [FormerlySerializedAs("creatureID")] [BV.DrawIf("unitType", UnitType.Guardian)]
    public GuardianID guardianID;
    [FormerlySerializedAs("humanID")] [BV.DrawIf("unitType", UnitType.Human)]
    public EnemyID enemyID;
    [BV.DrawIf("unitType", UnitType.Site)]
    public SiteID siteID;
    [BV.DrawIf("unitType", UnitType.Tool)]
    public ToolID toolID;

    private int damage;
    private string unitID;
    public int Damage => damage;
    public string UnitID => unitID;
    
    private Collider collider;
    public void ToggleCollider(bool _active) => collider.enabled = _active;

    void Start()
    {
        Setup();
    }

    private void Setup()
    {
        collider = GetComponent<Collider>();
        switch (unitType)
        {
            case UnitType.Guardian:
                damage = _DATA.GetUnit(guardianID).damage;
                unitID = _DATA.GetUnit(guardianID).id.ToString();
                break;
            case UnitType.Human:
                damage = _DATA.GetEnemy(enemyID).damage;
                unitID = _DATA.GetEnemy(enemyID).id.ToString();
                break;
            case UnitType.Site:
                damage = _DATA.GetSiteData(siteID).damage;
                unitID = _DATA.GetSiteData(siteID).id.ToString();
                break;
            case UnitType.Tool:
                damage = _DATA.GetTool(toolID).damage * _DATA.GetTool(toolID).upgradeLevel;
                unitID = _DATA.GetTool(toolID).id.ToString();
                break;
        }
    }
}
