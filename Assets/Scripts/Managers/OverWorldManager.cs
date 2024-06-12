using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class OverWorldManager : Singleton<OverWorldManager>
{
    public Transform mapModel;
    public float mapTweenTime = 1f;
    public Ease mapEase;
    public Light mapLight;
    public ParticleSystem mapLightParticle;

    private void Start()
    {
        mapModel.DOLocalMoveY(9,0);
    }

    public void ToggleInMap(bool _show)
    {
        if(_show)
            mapLightParticle.Stop();
        else
            mapLightParticle.Play();
        mapLight.DOIntensity(_show ? 200 : 2000, mapTweenTime).SetEase(mapEase);
        mapModel.DOLocalMoveY(_show ? 16 : 9, mapTweenTime).SetEase(mapEase);
    }
}
