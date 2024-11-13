using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wildlife Data", menuName = "SSS/Wildlife Data", order = 6)]
public class WildlifeData : ScriptableObject
{
    public WildlifeID id;
    public new string name;
    [TextArea(3, 5)]
    public string description;
    [Tooltip("Is available when the tree count reaches this")]
    public int avalaibleAt;
    public float baseHealth = 100f;
    public float baseSpeed = 1f;
    public float runSpeed = 12f;
    public float walkRadius = 5f;
    public float runRadius = 100f;
    [Space(20)]
    public GameObject spawnParticle;
    public Sprite icon;
    public Sprite mapIcon;
    public GameObject playModel;
    public AudioClip[] sounds;
    public AudioClip[] distressSounds;
    public AudioClip[] dieSounds;
}