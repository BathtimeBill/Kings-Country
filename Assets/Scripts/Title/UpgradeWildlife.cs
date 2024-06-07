using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeWildlife : GameBehaviour
{
    public WildlifeID wildlifeID;
    public UpgradePanel upgradePanel;

    private void OnMouseEnter()
    {
        //upgradePanel.ChangeTool(wildlifeID);
        PlaySelectSound();
    }

    private void PlaySelectSound()
    {
        if (GetComponent<AudioSource>() == null)
            return;
        if (_DATA.GetWildlife(wildlifeID).sounds == null)
            return;

        GetComponent<AudioSource>().clip = ArrayX.GetRandomItemFromArray(_DATA.GetWildlife(wildlifeID).sounds);
        GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
        GetComponent<AudioSource>().Play();
    }
}
