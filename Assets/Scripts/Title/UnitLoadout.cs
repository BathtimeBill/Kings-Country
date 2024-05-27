using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLoadout : MonoBehaviour
{
    public UnitID unitID;
    public UnitLoadouts unitLoadouts;

    private void OnMouseEnter()
    {
        unitLoadouts.ChangeUnit(unitID);
    }
}
