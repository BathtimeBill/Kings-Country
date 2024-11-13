using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Rune : GameBehaviour
{
    public bool hasUpgrade;
    public GameObject colliderObject;

    public void OnDayOver(int _day)
    {
        if(!_DATA.HasPerk(PerkID.Rune))
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        GameEvents.OnDayOver += OnDayOver;
    }

    private void OnDisable()
    {
        GameEvents.OnDayOver -= OnDayOver;
    }
}
