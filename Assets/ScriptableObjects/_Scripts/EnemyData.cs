using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "SSS/Enemy Data", order = 4)]
public class EnemyData : ScriptableObject
{
    public HumanID id;
    public EnemyType type;
    public new string name;
    [TextArea(3, 5)]
    public string description;
    [Header("Stats")]
    public int health;
    public int damage;
    public int speed;
    [Header("Non Stats")]
    public Sprite icon;
    public Sprite mapIcon;
    [Header("Audio")]
    public AudioClip[] voiceSounds;
    public AudioClip spawnSounds;
    public AudioClip[] hitSounds;
    public AudioClip[] dieSounds;
    [Header("Models")]
    public GameObject playModel;
    public GameObject ragdollModel;
    public GameObject bloodParticles;
}