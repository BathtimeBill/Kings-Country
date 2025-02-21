using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DayNightSwitch : GameBehaviour
{
    private Tween SunRotationTween;
    public float duration;
    public GameObject sun;
    public Light moon;
    public Vector3 dayRoation;
    public Vector3 nightRotation;
    public float nightLight;
    public float dayLight;
    public Color nightColor;
    public Color dayColor;
    public Vector3 buttonRotationDay;
    public Vector3 buttonRotationNight;

    Tweener sunTweener;
    Tween ambientColourTween;
    Tween moonIntensityTween;
    Tween ambientLightIntensityTween;
    Tweener buttonRotationTween;


    //public void TweenAmbientLight(float amount, float time)
    //{
    //    KillTweener(ambientLight);
    //    ambientLight = RenderSettings.ambientIntensity.DO 
    //}

    public void TweenSunRotation(Vector3 endVector, float time)
    {
        TweenX.KillTweener(sunTweener);
        sunTweener = sun.transform.DORotate(endVector, time).SetEase(Ease.InOutSine); 
    }

    public void TweenButtonRotation(Vector3 endRotation, float time)
    {
        TweenX.KillTweener(buttonRotationTween);
        buttonRotationTween = _UI.dayNightButton.transform.DORotate(endRotation, time).SetEase(Ease.InOutBack);
    }

    public Tween TweenAmbientColour(Color color, float time)
    {
        ambientColourTween = DOTween.To(() => RenderSettings.ambientSkyColor, (x) => RenderSettings.ambientSkyColor = x, color, time);
        return ambientColourTween;
    }

    public Tween AmbientLightIntensityTween(float amount, float time)
    {
        ambientLightIntensityTween = DOTween.To(() => RenderSettings.ambientIntensity, (x) => RenderSettings.ambientIntensity = x, amount, time);
        return ambientLightIntensityTween;
    }
    public Tween MoonIntensityTween(float amount, float time)
    {
        moonIntensityTween = DOTween.To(() => moon.intensity, (x) => moon.intensity = x, amount, time);
        return moonIntensityTween;
    }


    public void OnContinueButton()
    {
        TweenButtonRotation(buttonRotationNight, duration);
        MoonIntensityTween(nightLight, duration);
        TweenSunRotation(nightRotation, duration);
        AmbientLightIntensityTween(nightLight, duration);
        TweenAmbientColour(nightColor, duration);
        _SM.PlaySound(_SM.transitionToNightSound);
        _SM.weatherAudioSource.volume = 0.5f;
        _SM.weatherAudioSource.Stop();
        _SM.weatherAudioSource.clip = _SM.forestSoundNight;
        _SM.weatherAudioSource.Play();
    }
    public void OnDayBegin(int _day)
    {
        TweenButtonRotation(buttonRotationDay, duration);
        MoonIntensityTween(0, duration);
        TweenSunRotation(dayRoation, duration);
        AmbientLightIntensityTween(dayLight, duration);
        TweenAmbientColour(dayColor, duration);
        _SM.PlaySound(_SM.transitionToDaySound);
        _SM.weatherAudioSource.volume = 0.5f;
        _SM.weatherAudioSource.Stop();
        _SM.weatherAudioSource.clip = _SM.forestSoundDay;
        _SM.weatherAudioSource.Play();
    }

    private void OnEnable()
    {
        GameEvents.OnContinueButton += OnContinueButton;
        GameEvents.OnDayBegin += OnDayBegin;
    }

    private void OnDisable()
    {
        GameEvents.OnContinueButton -= OnContinueButton;
        GameEvents.OnDayBegin -= OnDayBegin;
    }
}
