using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BounceInTween : MonoBehaviour
{
    private Tween bounceInTween;

    public Vector3 originalSize;
    public Vector3 scaleTo;

    void Start()
    {
        originalSize = transform.localScale;
        transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

        OnScale();
    }

    private void OnScale()
    {
        transform.DOScale(originalSize, 0.4f).SetEase(Ease.InOutBounce);
    }
}
