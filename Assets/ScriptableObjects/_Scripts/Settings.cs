using System;
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

    [Header("Icons")]
    public Icons icons;

    [Header("Audio")]
    public AudioVariables audio;

    [Header("Vibration")]
    public Vibration vibration;
}

[System.Serializable]
public class Colours
{
    public Color highlightedColor;
    public Color cooldownColor;
    public Color upgradeIconsColor;
    public Color titleColor;
    public Color descriptionColor;
    public Color upgradeIncreaseColor;
    public Color upgradeDecreaseColor;
    public Gradient treePercentageGradient;

    public string GetIncreaseColorString => ColorX.GetColorHex(upgradeIncreaseColor);
    public string GetDecreaseColorString => ColorX.GetColorHex(upgradeDecreaseColor);
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

[Serializable]
public class AudioVariables
{
    public float soundVolume = 1f;
    public bool sound = true;
    public float musicVolume = 1f;
    public bool music = true;
}

[Serializable]
public class Vibration
{
    public bool vibrateOn = true;
    public float[] vibrateStrength; //Perfect, Great, Good
}

[Serializable]
public class Icons
{
    public Sprite emptyIcon;
    public Sprite maegenIcon;
    public Sprite wildlifeIcon;
    public Sprite damageIcon;
    public Sprite healthIcon;
    public Sprite speedIcon;
    public Sprite cooldownIcon;

}
