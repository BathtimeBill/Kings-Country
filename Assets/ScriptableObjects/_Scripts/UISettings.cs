using UnityEngine;

[CreateAssetMenu(fileName = "UI Settings", menuName = "BGG/UI Settings", order = 1)]
public class UISettings : ScriptableObject
{
    public Color highlightedColor;
    public Color cooldownColor;
    public Color upgradeIconsColor;
    public Color titleColor;
    public Color descriptionColor;
    public Gradient treePercentageGradient;
}
