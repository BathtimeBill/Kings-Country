using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconPlacement : GameBehaviour
{
    public GameObject effectRadius;

    private void OnBeaconUpgrade()
    {
        effectRadius.transform.localScale = effectRadius.transform.localScale * 2;
    }

    private void OnEnable()
    {
        GameEvents.OnBeaconUpgrade += OnBeaconUpgrade;
    }

    private void OnDisable()
    {
        GameEvents.OnBeaconUpgrade -= OnBeaconUpgrade;
    }
}
