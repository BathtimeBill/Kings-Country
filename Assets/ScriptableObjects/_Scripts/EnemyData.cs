using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "BGG/Enemy Data", order = 4)]
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
    public AudioClip[] voiceSounds;
    [Header("Models")]
    public GameObject playModel;
    public GameObject ragdollModel;
    public GameObject ragdollFireModel;
    public GameObject bloodParticles;
}