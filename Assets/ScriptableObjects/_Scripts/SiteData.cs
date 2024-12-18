using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Site Data", menuName = "SSS/Site Data", order = 8)]
public class SiteData : ScriptableObject
{
    public SiteID id;
    public new string name;
    [TextArea(3, 5)]
    public string description;

    public GameObject sitePrefab;
    public List<GuardianID> siteGuardians;
    public List<EnemyID> siteEnemies;
    [Header("Stats")]
    public int health;
    public int damage;
    public BV.Range spawnRate;
    public float claimTime;
    public float claimRate = 3f;
    [Header("Non Stats")]
    public Sprite icon;
    public Sprite selectionIcon;
    public Sprite mapIcon;
    public Color mapIconColour;
    public AudioClip[] sounds;
}