using UnityEngine;
using DG.Tweening;
using System.Collections;

public class StormerTool : Tool
{
    public float stormerLength = 60;
    public float wetSpeed = 20;
    public float drySpeed = 40;
    public TerrainLayer grassTexture;
    public TerrainLayer dirtTexture;
    public TerrainLayer rockTexture;
    public GameObject rain;
    public GameObject lightning;
    public GameObject lightningSound;
    
    private Light sun;
    private GameObject[] enemies;
    private Tween grassSmoothnessTween;
    private Tween dirtSmoothnessTween;
    private Tween rockSmoothnessTween;
    private Tween sunIntensityTween;
    
    public override void Start()
    {
        base.Start();
        lightning.SetActive(false);
        TweenGrassSmoothness(0, 0.1f);
        TweenDirtSmoothness(0, 0.1f);
        TweenRockSmoothness(0, 0.1f);
        TweenSunIntensity(1f, 0.1f);
        sun = FindFirstObjectByType<DayNightSwitch>().sun.GetComponent<Light>();
    }
    
    public override void Select()
    {
        base.Select();
    }

    public override void Deselect()
    {
        base.Deselect();
    }

    public override void Use()
    {
        //base.Use();
        StartCoroutine(StartStormer());
        GameEvents.ReportOnStormerPlaced();
        _CAMERA.CameraShake(_SETTINGS.cameraShake.stormerShakeIntensity);
    }
    
    private IEnumerator StartStormer()
    {
        TurnOnRain();
        StartCoroutine(SpawnLightning());
        yield return new WaitForSeconds(stormerLength);
        TurnOffRain();
        Deselect();
        StopAllCoroutines();
    }
    
    private IEnumerator SpawnLightning()
    {
        yield return new WaitForSeconds(Random.Range(1, 5));
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(enemies.Length > 0)
        {
            lightning.transform.position = ArrayX.GetRandomItemFromArray(enemies).transform.position;
            lightning.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            lightning.SetActive(false);
            //TODO better audio system
            Instantiate(lightningSound, lightning.transform.position, transform.rotation);
        }
        StartCoroutine(SpawnLightning());
    }
    
    private void TurnOnRain()
    {
        TweenGrassSmoothness(0.8f, wetSpeed);
        TweenDirtSmoothness(0.15f, wetSpeed);
        TweenRockSmoothness(0.7f, wetSpeed);
        TweenSunIntensity(0, wetSpeed);
        _SM.weatherAudioSource.volume = 1;
        _SM.weatherAudioSource.clip = _SM.rainSound;
        _SM.weatherAudioSource.Play();
        rain.SetActive(true);
    }
    private void TurnOffRain()
    {
        TweenGrassSmoothness(0, drySpeed);
        TweenDirtSmoothness(0, drySpeed);
        TweenRockSmoothness(0, drySpeed);
        TweenSunIntensity(1f, drySpeed);
        _SM.weatherAudioSource.volume = 0.5f;
        _SM.weatherAudioSource.clip = _SM.forestSoundDay;
        _SM.weatherAudioSource.Play();
        rain.SetActive(false);
    }
    
    private Tween TweenGrassSmoothness(float amount, float time)
    {
        grassSmoothnessTween = DOTween.To(() => grassTexture.smoothness, (x) => grassTexture.smoothness = x, amount, time);
        return grassSmoothnessTween;
    }
    private Tween TweenDirtSmoothness(float amount, float time)
    {
        dirtSmoothnessTween = DOTween.To(() => dirtTexture.smoothness, (x) => dirtTexture.smoothness = x, amount, time);
        return dirtSmoothnessTween;
    }
    private Tween TweenRockSmoothness(float amount, float time)
    {
        rockSmoothnessTween = DOTween.To(() => rockTexture.smoothness, (x) => rockTexture.smoothness = x, amount, time);
        return rockSmoothnessTween;
    }
    private Tween TweenSunIntensity(float amount, float time)
    {
        sunIntensityTween = DOTween.To(() => sun.intensity, (x) => sun.intensity = x, amount, time);
        return sunIntensityTween;
    }
    
    private void OnStormerUpgrade() => stormerLength *= 2;
    
    private void OnEnable()
    {
        GameEvents.OnStormerUpgrade += OnStormerUpgrade;
    }

    private void OnDisable()
    {
        GameEvents.OnStormerUpgrade -= OnStormerUpgrade;
    }
}
