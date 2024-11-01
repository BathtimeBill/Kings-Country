using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Rune : GameBehaviour
{
    public bool hasUpgrade;
    public GameObject colliderObject;

    public void OnWaveOver()
    {
        if(!_DATA.HasPerk(PerkID.Rune))
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        GameEvents.OnDayOver += OnWaveOver;
    }

    private void OnDisable()
    {
        GameEvents.OnDayOver -= OnWaveOver;
    }
}
