using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    [BV.EnumList(typeof(UpgradeID))]
    public List<Upgrade> upgrades;

    public List<UpgradeID> availableUpgrades;
    public List<UpgradeID> currentUpgrades;

    public bool hasUpgrade(UpgradeID upgradeID) => currentUpgrades.Contains(upgradeID);
    

    private void Start()
    {
        for(int i=0; i<upgrades.Count;i++)
        {
            availableUpgrades.Add(upgrades[i].id);
        }
    }

    public void RemoveUpgrade(UpgradeID upgradeID) => availableUpgrades.Remove(upgradeID);

    public void AddBackUpgrade(UpgradeID upgradeID) => availableUpgrades.Add(upgradeID);

    public void AddUpgrade(UpgradeID upgradeID) => currentUpgrades.Add(upgradeID);
    public Upgrade GetUpgrade(UpgradeID upgradeID) => upgrades.Find(x => x.id == upgradeID);

    public UpgradeID GetRandomUpgrade() => ListX.GetRandomItemFromList(availableUpgrades);

}

[System.Serializable]
public class Upgrade
{
    public UpgradeID id;
    public string name;
    public string description;
    public Sprite icon;
}
