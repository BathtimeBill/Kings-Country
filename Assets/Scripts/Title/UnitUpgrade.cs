using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUpgrade : GameBehaviour
{
    public UnitID unitID;
    public UnitUpgrades unitUpgrades;

    private void OnMouseEnter()
    {
        unitUpgrades.ChangeUnit(unitID);
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
