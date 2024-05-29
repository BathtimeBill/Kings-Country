using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenX : GameBehaviour
{
    /// <summary>
    /// Kills a tweener that may be active
    /// </summary>
    /// <param name="_tweener">The tweener to kill</param>
    public static void KillTweener(Tweener _tweener)
    {
        if (_tweener != null)
            _tweener.Kill();
    }

    public static void TweenTextScale(Transform _textPanel, float _to)
    {
        _textPanel.DOScaleX(_to, _DATA.settings.tweening.UIButtonTweenTime).SetEase(_DATA.settings.tweening.UIButtonTweenEase);
    }
}
