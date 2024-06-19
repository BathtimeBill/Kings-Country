using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Data", menuName = "BGG/Unit Data", order = 3)]
public class UnitData : ScriptableObject
{
    public CreatureID id;
    public string unitName;
    [TextArea(3, 5)]
    public string description;
    [Header("Stats")]
    public int health;
    public int damage;
    public int speed;
    public int cost;
    [Header("Non Stats")]
    public Sprite icon;
    public Sprite mapIcon;
    public Color mapIconColour;
    public AudioClip[] voiceSounds;
}