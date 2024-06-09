using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTool : UpgradeObject
{
    public ToolID toolID;

    private void OnMouseEnter()
    {
        _UPGRADE.ChangeUpgrade(this);
        PlaySelectSound();
    }

    private void PlaySelectSound()
    {
        if (GetComponent<AudioSource>() == null)
            return;
        if (_DATA.GetTool(toolID).toolSounds == null)
            return;

        GetComponent<AudioSource>().clip = ArrayX.GetRandomItemFromArray(_DATA.GetTool(toolID).toolSounds);
        GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
        GetComponent<AudioSource>().Play();
    }
}
