using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Analytics;

public class Stormer : GameBehaviour
{
    public TerrainLayer grassTexture;
    public TerrainLayer dirtTexture;
    public TerrainLayer rockTexture;
    public Light sun;
    public GameObject rain;
    public GameObject[] enemies;
    public GameObject lightning;
    public GameObject lightningSound;

    public float stormerLength;
    public float wetSpeed;
    public float drySpeed;

    private Tween grassSmoothnessTween;
    private Tween dirtSmoothnessTween;
    private Tween rockSmoothnessTween;
    private Tween sunIntensityTween;

    public GameObject audioSource;

    private void Start()
    {
        TweenGrassSmoothness(0, 0.1f);
        TweenDirtSmoothness(0, 0.1f);
        TweenRockSmoothness(0, 0.1f);
        TweenSunIntensity(1f, 0.1f);
    }
    
    private void TurnOnRain()
    {
        TweenGrassSmoothness(0.8f, wetSpeed);
        TweenDirtSmoothness(0.15f, wetSpeed);
        TweenRockSmoothness(0.7f, wetSpeed);
        TweenSunIntensity(0, wetSpeed);
        rain.SetActive(true);
    }
    private void TurnOffRain()
    {
        TweenGrassSmoothness(0, drySpeed);
        TweenDirtSmoothness(0, drySpeed);
        TweenRockSmoothness(0, drySpeed);
        TweenSunIntensity(1f, drySpeed);
        rain.SetActive(false);
    }

    private int GetRandomEnemy()
    {
        int rndEnemy = Random.Range(0, enemies.Length);
        return rndEnemy;
    }
    
    IEnumerator StartStormer()
    {
        TurnOnRain();
        StartCoroutine(SpawnLightning());
        _SM.weatherAudioSource.volume = 1;
        _SM.weatherAudioSource.clip = _SM.rainSound;
        _SM.weatherAudioSource.Play();
        yield return new WaitForSeconds(stormerLength);
        TurnOffRain();
        _SM.weatherAudioSource.volume = 0.5f;
        _SM.weatherAudioSource.clip = _SM.forestSoundDay;
        _SM.weatherAudioSource.Play();
        StopAllCoroutines();
    }

    IEnumerator SpawnLightning()
    {
        yield return new WaitForSeconds(Random.Range(1, 5));
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(enemies.Length > 0 )
        {
            GameObject gm = Instantiate(lightning, enemies[GetRandomEnemy()].transform.position, transform.rotation);
            Instantiate(lightningSound, transform.position, transform.rotation);
            Destroy(gm, 0.5f);

        }
        StartCoroutine(SpawnLightning());
    }
    public Tween TweenGrassSmoothness(float amount, float time)
    {
        grassSmoothnessTween = DOTween.To(() => grassTexture.smoothness, (x) => grassTexture.smoothness = x, amount, time);
        return grassSmoothnessTween;
    }
    public Tween TweenDirtSmoothness(float amount, float time)
    {
        dirtSmoothnessTween = DOTween.To(() => dirtTexture.smoothness, (x) => dirtTexture.smoothness = x, amount, time);
        return dirtSmoothnessTween;
    }
    public Tween TweenRockSmoothness(float amount, float time)
    {
        rockSmoothnessTween = DOTween.To(() => rockTexture.smoothness, (x) => rockTexture.smoothness = x, amount, time);
        return rockSmoothnessTween;
    }
    public Tween TweenSunIntensity(float amount, float time)
    {
        sunIntensityTween = DOTween.To(() => sun.intensity, (x) => sun.intensity = x, amount, time);
        return sunIntensityTween;
    }

    public void OnStormerPlaced()
    {
        StartCoroutine(StartStormer());
        GameObject go =Instantiate(audioSource, transform.position, transform.rotation);
        go.GetComponent<AudioSource>().clip = _SM.stormerSummonSound;
        go.GetComponent<AudioSource>().Play();
    }
    public void OnStormerUpgrade()
    {
        stormerLength = stormerLength * 2;
    }

    private void OnEnable()
    {
        GameEvents.OnStormerPlaced += OnStormerPlaced;
        GameEvents.OnStormerUpgrade += OnStormerUpgrade;
    }

    private void OnDisable()
    {
        GameEvents.OnStormerPlaced -= OnStormerPlaced;
        GameEvents.OnStormerUpgrade -= OnStormerUpgrade;
    }
}
