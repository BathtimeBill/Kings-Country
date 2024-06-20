using UnityEngine;

[CreateAssetMenu(fileName = "New Perk", menuName = "BGG/Perk", order = 1)]
public class PerkData : ScriptableObject
{
    public PerkID id;
    public new string name;
    [TextArea]
    public string description;
    public float increaseValue = 0.3f;
    public Sprite icon;
}
