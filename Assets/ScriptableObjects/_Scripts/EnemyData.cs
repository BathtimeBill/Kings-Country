using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "SSS/Enemy Data", order = 4)]
public class EnemyData : ScriptableObject
{
    public EnemyID id;
    public EnemyType type;
    public new string name;
    [TextArea(3, 5)]
    public string description;
    [Header("Stats")]
    public int health;
    public int damage;
    public float speed;
    public float attackRange = 10;
    public float stoppingDistance = 1;
    public float invincibleTime = 5f;
    [Header("Non Stats")]
    public Sprite icon;
    public Sprite mapIcon;
    [Header("Audio")]
    public AudioClip[] voiceSounds;
    public AudioClip[] footstepSounds;
    public AudioClip[] attackSounds;
    public AudioClip[] hitSounds;
    public AudioClip[] dieSounds;
    public AudioClip spawnSound;
    [Header("Models")]
    public GameObject playModel;
    public GameObject ragdollModel;
    public GameObject hitParticles;
    public GameObject dieParticles;
}