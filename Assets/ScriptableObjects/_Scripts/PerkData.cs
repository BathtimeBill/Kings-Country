using UnityEngine;

[CreateAssetMenu(fileName = "New Perk", menuName = "BGG/Perk", order = 1)]
public class PerkData : ScriptableObject
{
    public PerkID ID;
    public string perkName;
    public string perkDescription;
    public Sprite perkIcon;
    public bool unlocked;
}
