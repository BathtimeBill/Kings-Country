using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OverworldLevelButton : GameBehaviour
{
    public LevelID thisLevel;
    private OverWorldManager overworldManager;
    private bool selected = false;

    private void Awake()
    {
        overworldManager = FindObjectOfType<OverWorldManager>();
    }

    private void Start()
    {
        ExecuteNextFrame(() =>
        {
            if (_DATA.levelAvailable(thisLevel))
                myRenderer.material.color = _COLOUR.mapUnlockedColor;
            else
                myRenderer.material.color = _COLOUR.mapLockedColor;
        });
    }

    private void OnMouseEnter()
    {
        if (!selected)
            TweenColor(_DATA.levelAvailable(thisLevel) ? _COLOUR.mapUnlockedHighlightColor : _COLOUR.mapLockedHighlightColor);
        _SM.PlaySound(_SM.buttonClickSound);
    }

    private void OnMouseExit()
    {
        if(!selected)
            TweenColor(_DATA.levelAvailable(thisLevel) ? _COLOUR.mapUnlockedColor : _COLOUR.mapLockedColor);
    }

    private void OnMouseDown()
    {
        selected = !selected;
        overworldManager.ShowLevel(selected ? thisLevel : LevelID.None);
    }

    public void TweenColor(Color _color)
    {
        myRenderer.material.DOColor(_color, _TWEENING.mapTweenTime).SetEase(_TWEENING.mapTweenEase);
    }

    public void TweenScale(bool _show)
    {
        transform.DOScale(_show ? new Vector3(1.1f, 1.1f, 3f) : Vector3.one, _TWEENING.mapTweenTime).SetEase(_TWEENING.mapTweenEase);
    }

    public void SetSelected(bool _selected)
    {
        selected = _selected;
    }

    public void ShowLevel()
    {
        
    }

    public void LevelLocked()
    {

    }
}
