using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "BGG/Settings", order = 1)]
public class Settings : ScriptableObject
{
    [Header("Camera Shakes")]
    public CameraShake cameraShake;

    [Header("Colours")]
    public Colours colours;
}

[System.Serializable]
public class Colours
{
    public Color highlightedColor;
    public Color cooldownColor;
    public Color upgradeIconsColor;
    public Color titleColor;
    public Color descriptionColor;
    public Gradient treePercentageGradient;
}

[System.Serializable]
public class CameraShake
{
    public float shakeDuration = 1.5f;
    public int shakeVibrato = 10;
    public float shakeRandomness = 90f;
    public float fyreShakeIntensity = 2.0f;
    public float stormerShakeIntensity = 3.0f;
}
