using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wildlife Data", menuName = "BGG/Wildlife Data", order = 6)]
public class WildlifeData : ScriptableObject
{
    public WildlifeID id;
    public new string name;
    [TextArea(3, 5)]
    public string description;
    public Sprite icon;
    public Sprite mapIcon;
    public AudioClip[] sounds;
}