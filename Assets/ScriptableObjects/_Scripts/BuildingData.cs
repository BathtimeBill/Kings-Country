using UnityEngine;

[CreateAssetMenu(fileName = "New Building Data", menuName = "BGG/Building Data", order = 8)]
public class BuildingData : ScriptableObject
{
    public BuildingID id;
    public new string name;
    [TextArea(3, 5)]
    public string description;
    [Header("Stats")]
    public int health;
    public int damage;
    public int speed;
    public int cost;
    [Header("Non Stats")]
    public Sprite icon;
    public Sprite selectionIcon;
    public Sprite mapIcon;
    public Color mapIconColour;
    public AudioClip[] sounds;
}