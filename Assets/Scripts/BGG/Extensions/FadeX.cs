using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static class FadeX
{
    public static void FadeIn(CanvasGroup _canvasGroup, float _tweenTime, Ease _ease, bool _interactable = true, Action _onSuccess = null)
    {
        if (_canvasGroup.alpha == 1) return;

        _canvasGroup.DOFade(1, _tweenTime).SetUpdate(true).OnComplete(() => _onSuccess.Invoke());
        _canvasGroup.interactable = _interactable;
        _canvasGroup.blocksRaycasts = _interactable;
    }
    public static void FadeIn(CanvasGroup _canvasGroup, float _tweenTime = 0.5f, bool _interactable = true, Action _onSuccess = null)
    {
        if (_canvasGroup.alpha == 1) return;

        _canvasGroup.DOFade(1, _tweenTime).SetUpdate(true).OnComplete(() => _onSuccess.Invoke());
        _canvasGroup.interactable = _interactable;
        _canvasGroup.blocksRaycasts = _interactable;
    }

    public static void FadeOut(CanvasGroup _canvasGroup, float _tweenTime, Ease _ease, bool _interactable = true, Action _onSuccess = null)
    {
        if (_canvasGroup.alpha == 0) return;

        _canvasGroup.DOFade(0, _tweenTime).SetUpdate(true).OnComplete(() => _onSuccess.Invoke());
        _canvasGroup.interactable = _interactable;
        _canvasGroup.blocksRaycasts = _interactable;
    }
    public static void FadeOut(CanvasGroup _canvasGroup, float _tweenTime = 0.5f, bool _interactable = false, Action _onSuccess = null)
    {
        if (_canvasGroup.alpha == 0) return;

        _canvasGroup.DOFade(0, _tweenTime).SetUpdate(true).OnComplete(()=> _onSuccess.Invoke());
        _canvasGroup.interactable = _interactable;
        _canvasGroup.blocksRaycasts = _interactable;
    }

    /// <summary>
    /// Makes a panel instantly opaque and optionally sets interactable (true by default)
    /// </summary>
    /// <param name="_canvasGroup">The canvas group to fade</param>
    /// <param name="_interactable">Are we interactable?</param>
    public static void InstantOpaque(CanvasGroup _canvasGroup, bool _interactable = true)
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = _interactable;
        _canvasGroup.blocksRaycasts = _interactable;
    }

    /// <summary>
    /// Makes a panel instantly transparent and optionally sets interactable (false by default)
    /// </summary>
    /// <param name="_canvasGroup">The canvas group to fade</param>
    /// <param name="_interactable">Are we interactable?</param>
    public static void InstantTransparent(CanvasGroup _canvasGroup, bool _interactable = false)
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = _interactable;
        _canvasGroup.blocksRaycasts = _interactable;
    }
}
