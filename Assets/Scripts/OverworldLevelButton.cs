using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OverworldLevelButton : GameBehaviour
{
    public LevelID thisLevel;

    private void Start()
    {
        
    }

    private void OnMouseEnter()
    {
        ShowLevel(true);
    }

    private void OnMouseExit()
    {
        ShowLevel(false);
    }

    public void ShowLevel(bool _show)
    {
        transform.DOScale(_show ? new Vector3(1.1f,1.1f,3f): Vector3.one, _TWEENING.mapTweenTime).SetEase(_TWEENING.mapTweenEase);
        myRenderer.material.DOColor(_show ? _COLOUR.mapHighlightColor : _COLOUR.mapUnlockedColor, _TWEENING.mapTweenTime).SetEase(_TWEENING.mapTweenEase);
    }
}
