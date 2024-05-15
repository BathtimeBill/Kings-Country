using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButtons : GameBehaviour
{
    public UpgradeID upgradeType;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonPressed()
    {
        //GameEvents.ReportOnUpgradeSelected();
        switch (upgradeType)
        {
            case UpgradeID.BarkSkin:
                //GameEvents.ReportOnBorkrskinnUpgrade();
                _UM.borkrskinn = true;
                break;

            case UpgradeID.FlyFoot:
                //GameEvents.ReportOnFlugafotrUpgrade();
                _UM.flugafotr = true;
                break;

            case UpgradeID.Power:
                //GameEvents.ReportOnJarnnefiUpgrade();
                _UM.jarnnefi = true;
                break;

            case UpgradeID.Tower:
                //GameEvents.ReportOnTowerUpgrade();
                _UM.tower = true;
                break;

            case UpgradeID.Rune:
                //GameEvents.ReportOnRuneUpgrade();
                _UM.rune = true;
                break;

            case UpgradeID.Fyre:
                //GameEvents.ReportOnBeaconUpgrade();
                _UM.beacon = true;
                break;

            case UpgradeID.Stormer:
                //GameEvents.ReportOnStormerUpgrade();
                _UM.stormer = true;
                break;

            case UpgradeID.Tree:
                //GameEvents.ReportOnTreeUpgrade();
                _UM.tree = true;
                break;

            case UpgradeID.Fertile:
                GameEvents.ReportOnFertileSoilUpgrade();
                _UM.fertileSoil = true;
                break;

            case UpgradeID.Populous:
                GameEvents.ReportOnPopulousUpgrade();
                _UM.populous = true;
                break;

            case UpgradeID.Winfall:
                //GameEvents.ReportOnWinfallUpgrade();
                _UM.winfall = true;
                break;

            case UpgradeID.HomeTree:
                //GameEvents.ReportOnHomeTreeUpgrade();
                _UM.homeTree = true;
                break;
        }

    }
}
