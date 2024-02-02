using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LoadingScreen : GameBehaviour
{
    public Image image;
    public Sprite[] loadingImages;
    void Start()
    {
        image.sprite = loadingImages[GetRandomInt()];
    }

    int GetRandomInt()
    {
        int i = Random.Range(0, loadingImages.Length);
        return i;
    }
}
