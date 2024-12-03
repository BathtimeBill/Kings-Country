using System;
using UnityEngine;

public class RuneTool : Tool
{
    public GameObject runePrefab;
    public Collider runeCollider;
    public override void Select()
    {
        base.Select();
        runeCollider.enabled = true;
    }

    public override void Deselect()
    {
        base.Deselect();
        runeCollider.enabled = false;
    }

    public override void Use()
    {
        base.Use();
        GameObject runeInstance = Instantiate(runePrefab, transform.position, transform.rotation);
        _GM.DecreaseMaegen(_GM.runesMaegenCost[_GM.runesCount]);
        _GM.AddRune(runeInstance);
        GameEvents.ReportOnRunePlaced();
        Deselect();
    }

    private void OnEnable()
    {
        runeCollider.enabled = true;
    }

    private void OnDisable()
    {
        runeCollider.enabled = false;
    }
}
