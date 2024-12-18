using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Guardian Data", menuName = "SSS/Guardian Data", order = 3)]
public class UnitData : ScriptableObject
{
    public GuardianID id;
    public SiteID home;
    public new string name;
    [TextArea(3, 5)]
    public string description;
    public bool towerUnit = false;
    [Header("Stats")]
    public int health;
    public int damage;
    public int speed;
    public int cost;
    public float detectRange = 50f;
    public float stoppingDistance = 4f;
    public float attackRange = 15f;
    [Header("Non Stats")]
    public Sprite icon;
    public Sprite mapIcon;
    [Header("Audio")]
    public AudioClip[] voiceSounds;
    public AudioClip[] footstepSounds;
    public AudioClip[] attackSounds;
    public AudioClip[] hitSounds;
    public AudioClip[] dieSounds;
    [Header("Models")]
    public GameObject playModel;
    public GameObject ragdollModel;
    public GameObject spawnParticles;
    public GameObject hitParticles;
    public GameObject dieParticles;
}