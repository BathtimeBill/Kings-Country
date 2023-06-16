using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButtons : GameBehaviour
{
    public Upgrade upgradeType;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonPressed()
    {
        GameEvents.ReportOnUpgradeSelected();
        switch (upgradeType)
        {
            case Upgrade.BarkSkin:
                GameEvents.ReportOnBorkrskinnUpgrade();
                _UM.borkrskinn = true;
                break;

            case Upgrade.FlyFoot:
                GameEvents.ReportOnFlugafotrUpgrade();
                _UM.flugafotr = true;
                break;

            case Upgrade.Power:
                GameEvents.ReportOnJarnnefiUpgrade();
                _UM.jarnnefi = true;
                break;

            case Upgrade.Tower:
                GameEvents.ReportOnTowerUpgrade();
                _UM.tower = true;
                break;

            case Upgrade.Rune:
                GameEvents.ReportOnRuneUpgrade();
                _UM.jarnnefi = true;
                break;

            case Upgrade.Beacon:
                GameEvents.ReportOnBeaconUpgrade();
                _UM.beacon = true;
                break;

            case Upgrade.Stormer:
                GameEvents.ReportOnStormerUpgrade();
                _UM.stormer = true;
                break;

            case Upgrade.Tree:
                GameEvents.ReportOnTreeUpgrade();
                _UM.tree = true;
                break;

            case Upgrade.Fertile:
                GameEvents.ReportOnFertileSoilUpgrade();
                _UM.fertileSoil = true;
                break;

            case Upgrade.Populous:
                GameEvents.ReportOnPopulousUpgrade();
                _UM.populous = true;
                break;

            case Upgrade.Winfall:
                GameEvents.ReportOnWinfallUpgrade();
                _UM.winfall = true;
                break;

            case Upgrade.HomeTree:
                GameEvents.ReportOnHomeTreeUpgrade();
                _UM.homeTree = true;
                break;
        }

    }
}
