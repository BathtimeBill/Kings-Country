using UnityEngine;

[CreateAssetMenu(fileName = "New Perk", menuName = "BGG/Perk", order = 1)]
public class PerkData : ScriptableObject
{
    public PerkID id;
    public new string name;
    public string description;
    public Sprite icon;
}
