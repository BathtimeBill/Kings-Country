using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUnit : UpgradeObject
{
    public UnitID unitID;

    private void OnMouseEnter()
    {
        _UPGRADE.ChangeUpgrade(this);
        PlaySelectSound();
    }

    private void PlaySelectSound()
    {
        if (GetComponent<AudioSource>() == null)
            return;
        if (_DATA.GetUnit(unitID).voiceSounds == null)
            return;

        GetComponent<AudioSource>().clip = ArrayX.GetRandomItemFromArray(_DATA.GetUnit(unitID).voiceSounds);
        GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
        GetComponent<AudioSource>().Play();
    }
}
