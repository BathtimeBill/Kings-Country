using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "BGG/Settings", order = 1)]
public class Settings : ScriptableObject
{
    [Header("Camera Shakes")]
    public CameraShake cameraShake;

    [Header("Tween Stuff")]
    public Tweening tweening;

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

[System.Serializable]
public class Tweening
{
    public float UIButtonTweenTime = 0.3f;
    public DG.Tweening.Ease UIButtonTweenEase;
    public float titlePanelTweenTime = 0.5f;
    public DG.Tweening.Ease titlePanelTweenEase;
    public float errorTweenTime = 0.2f;
    public float errorTweenDuration = 2f;
    public DG.Tweening.Ease errorTweenEase;
}
