using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Data", menuName = "BGG/Unit Data", order = 3)]
public class UnitData : ScriptableObject
{
    public CreatureID id;
    public BuildingID home;
    public new string name;
    [TextArea(3, 5)]
    public string description;
    public bool towerUnit = false;
    [Header("Stats")]
    public int health;
    public int damage;
    public int speed;
    public int cost;
    public float detectionRadius = 50f;
    public float stoppingDistance = 4f;
    [Header("Non Stats")]
    public Sprite icon;
    public Sprite mapIcon;
    public AudioClip[] voiceSounds;
    [Header("Models")]
    public GameObject playModel;
    public GameObject ragdollModel;
    public GameObject bloodParticles;
}