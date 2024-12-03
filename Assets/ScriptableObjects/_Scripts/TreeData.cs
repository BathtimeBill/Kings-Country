using UnityEngine;

[CreateAssetMenu(fileName = "New Tree", menuName = "SSS/Tree", order = 2)]
public class TreeData : ScriptableObject
{
    public TreeID id;
    public new string name;
    [TextArea]
    public string description;
    public Sprite icon;
    public GameObject playModel;
    public GameObject fallModel;
    [Header("Stats")]
    public float health = 100f;
    public int maegenPrice;
    public int wildlifePrice;
    public int upgradeLevel = 1;
    public bool unlocked = false;
    [Header("Sound")]
    public AudioClip[] placeSounds;
    public AudioClip[] growSounds;
    public AudioClip[] hitSounds;
    public AudioClip[] fallSounds;
    public GameObject instantiateParticles;
    public GameObject destroyedParticles;
    public BV.Range sizeRange;
}
