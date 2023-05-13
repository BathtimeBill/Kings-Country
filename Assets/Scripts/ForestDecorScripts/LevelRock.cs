using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelRock : GameBehaviour
{
    public MeshRenderer meshRenderer;
    public Material material;
    private Tween rockSmoothnessTween;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "River")
        {
            Destroy(gameObject);
        }
    }
    IEnumerator Rain()
    {
        yield return new WaitForSeconds(10);
        material.SetFloat("_SPECGLOSSMAP", 0.8f);
        yield return new WaitForSeconds(50);
        material.SetFloat("_SPECGLOSSMAP", 0);
    }
    public void OnStormerPlaced()
    {
        StartCoroutine(Rain());
    }

    private void OnEnable()
    {
        GameEvents.OnStormerPlaced += OnStormerPlaced;
    }

    private void OnDisable()
    {
        GameEvents.OnStormerPlaced -= OnStormerPlaced;
    }

}
