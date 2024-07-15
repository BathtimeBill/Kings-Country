using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class EffectsManager : Singleton<EffectsManager>
{
    public Volume globalVolume;
    private FilmGrain grain;
    private ChromaticAberration chromaticAberration;
    private Vignette vignette;

    private float startVignetteValue;

    private void Start()
    {
        globalVolume.profile.TryGet(out vignette);
        startVignetteValue = vignette.intensity.value;
    }

    public void SetChromatic(float _value)
    {
        globalVolume.profile.TryGet(out chromaticAberration);
        chromaticAberration.intensity.value = _value;
    }

    public void SetGrain(float _value)
    {
        globalVolume.profile.TryGet(out grain);
        grain.intensity.value = _value;
    }

    public void SetVignette(float _value)
    {
        globalVolume.profile.TryGet(out vignette);
        vignette.intensity.value = _value;
    }

    public void TweenVignette(float _value)
    {
        globalVolume.profile.TryGet(out vignette);
        DOTween.To(() => vignette.intensity.value, (x) => vignette.intensity.value = x, _value, _TWEENING.effectsTweenTime);
    }

    public void TweenVignetteReset()
    {
        globalVolume.profile.TryGet(out vignette);
        DOTween.To(() => vignette.intensity.value, (x) => vignette.intensity.value = x, startVignetteValue, _TWEENING.effectsTweenTime);
    }

    public void ResetVignette()
    {
        globalVolume.profile.TryGet(out vignette);
        vignette.intensity.value = startVignetteValue;
    }
}
