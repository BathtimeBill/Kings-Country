using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Perk", menuName = "BGG/Perk", order = 1)]
public class Perks : ScriptableObject
{
    public List<Perk> perks;

    public Perk FindPerk(PerkID _id)
    {
        return perks.Find(x=> x.id == _id);
    }
}

[System.Serializable]
public class Perk
{
    public PerkID id;
    public string name;
    public string perkDescription;
    public Sprite perkIcon;
    public bool unlocked;
}

