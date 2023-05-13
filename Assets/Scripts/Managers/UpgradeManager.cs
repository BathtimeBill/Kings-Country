using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    public bool borkrskinn;
    public GameObject borkrskinnCheck;
    public Button borkrskinnButton;
    public bool jarnnefi;
    public GameObject jarnnefiCheck;
    public Button jarnnefiButton;
    public bool flugafotr;
    public GameObject flugafotrCheck;
    public Button flugafotrButton;
    public bool rune;
    public GameObject runeCheck;
    public Button runeButton;
    public bool beacon;
    public GameObject beaconCheck;
    public Button beaconButton;
    public bool stormer;
    public GameObject stormerCheck;
    public Button stormerButton;
    public bool fertileSoil;
    public GameObject fertileSoilCheck;
    public Button fertileSoilButton;
    public bool populous = false;
    public GameObject populousCheck;
    public Button populousButton;


    public void BorkrskinnUpgrade()
    {
        if(borkrskinn == false && _GM.maegen >599)
        {
            _GM.maegen -= 600;
            borkrskinnButton.interactable = false;
            borkrskinn = true;
            GameEvents.ReportOnBorkrskinnUpgrade();
            borkrskinnCheck.SetActive(true);
            _UI.audioSource.clip = _SM.upgradeSound;
            _UI.audioSource.Play();
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
            jarnnefiButton.interactable = false;
            jarnnefi = true;
            GameEvents.ReportOnJarnnefiUpgrade();
            jarnnefiCheck.SetActive(true);
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
            flugafotrButton.interactable = false;
            flugafotr = true;
            GameEvents.ReportOnFlugafotrUpgrade();
            flugafotrCheck.SetActive(true);
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
                populousButton.interactable = false;
                populousCheck.SetActive(true);
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
            runeButton.interactable = false;
            rune = true;
            GameEvents.ReportOnRuneUpgrade();
            runeCheck.SetActive(true);
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
            beaconButton.interactable = false;
            beacon = true;
            GameEvents.ReportOnBeaconUpgrade();
            beaconCheck.SetActive(true);
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
            fertileSoilButton.interactable = false;
            fertileSoilCheck.SetActive(true);
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
