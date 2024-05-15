using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    [BV.EnumList(typeof(UpgradeID))]
    public List<Upgrade> upgrades;

    public List<UpgradeID> availableUpgrades;

    public bool borkrskinn;
    public bool jarnnefi;
    public bool flugafotr;
    public bool rune;
    public bool beacon;
    public bool stormer;
    public bool fertileSoil;
    public bool populous = false;
    public bool tower;
    public bool tree;
    public bool winfall;
    public bool homeTree;

    private void Start()
    {
        for(int i=0; i<upgrades.Count;i++)
        {
            availableUpgrades.Add(upgrades[i].id);
        }
    }

    public void RemoveUpgrade(UpgradeID upgradeID)
    {
        availableUpgrades.Remove(upgradeID);
    }

    public void AddUpgrade(UpgradeID upgradeID)
    {
        availableUpgrades.Add(upgradeID);
    }

    public UpgradeID GetRandomUpgrade()
    {
        return ListX.GetRandomItemFromList(availableUpgrades);
    }

    public Upgrade GetUpgrade(UpgradeID upgradeID)
    {
        return upgrades.Find(x => x.id == upgradeID);
    }


    public void BorkrskinnUpgrade()
    {
        if(borkrskinn == false)
        {
            borkrskinn = true;
            GameEvents.ReportOnBorkrskinnUpgrade();
        }
        else
        {
            _UI.SetErrorMessageInsufficientMaegen();
            _PC.Error();
        }
    }
    public void JarnnefiUpgrade()
    {
        if (jarnnefi == false && _GM.maegen > 599)
        {
            _GM.maegen -= 600;
            jarnnefi = true;
            GameEvents.ReportOnJarnnefiUpgrade();
            _UI.audioSource.clip = _SM.upgradeSound;
            _UI.audioSource.Play();
        }
        else
        {
            _UI.SetErrorMessageInsufficientMaegen();
            _PC.Error();
        }
    }
    public void FlugafotrUpgrade()
    {
        if (flugafotr == false && _GM.maegen > 599)
        {
            _GM.maegen -= 600;
            flugafotr = true;
            GameEvents.ReportOnFlugafotrUpgrade();
            _UI.audioSource.clip = _SM.upgradeSound;
            _UI.audioSource.Play();
        }
        else
        {
            _UI.SetErrorMessageInsufficientMaegen();
            _PC.Error();
        }
    }

    public void PopulousUpgrade()
    {
        if(populous == false)
        {
            if(_GM.maegen > 1199)
            {
                Debug.Log("Populous upgrade");
                _GM.maegen -= 1200;
                populous = true;
                _UI.audioSource.clip = _SM.upgradeSound;
                _UI.audioSource.Play();
                GameEvents.ReportOnPopulousUpgrade();
            }
            else
            {
                _UI.SetErrorMessageInsufficientMaegen();
                _PC.Error();
            }
        }
    }

    public void RuneUpgrade()
    {
        if (rune == false && _GM.maegen > 1199)
        {
            _GM.maegen -= 1200;
            rune = true;
            GameEvents.ReportOnRuneUpgrade();
            _UI.audioSource.clip = _SM.upgradeSound;
            _UI.audioSource.Play();
        }
        else
        {
            _UI.SetErrorMessageInsufficientMaegen();
            _PC.Error();
        }
    }

    public void BeaconUpgrade()
    {
        if(beacon == false && _GM.maegen > 799)
        {
            _GM.maegen -= 800;
            beacon = true;
            GameEvents.ReportOnBeaconUpgrade();
            _UI.audioSource.clip = _SM.upgradeSound;
            _UI.audioSource.Play();
        }
        else
        {
            _UI.SetErrorMessageInsufficientMaegen();
            _PC.Error();
        }
    }

    public void FertileSoil()
    {
        if(fertileSoil == false && _GM.maegen > 999)
        {
            _GM.maegen -= 1000;
            fertileSoil = true;
            GameEvents.ReportOnFertileSoilUpgrade();
            _UI.audioSource.clip = _SM.upgradeSound;
            _UI.audioSource.Play();
        }
        else
        {
            _UI.SetErrorMessageInsufficientMaegen();
            _PC.Error();
        }
    }


}

[System.Serializable]
public class Upgrade
{
    public UpgradeID id;
    public string name;
    public string description;
    public Sprite icon;
}
