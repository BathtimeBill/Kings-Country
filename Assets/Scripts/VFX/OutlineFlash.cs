using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineFlash : GameBehaviour
{
    public Outline outline;
    Color originalColour;
    public float timerLength;
    public bool isInvincible = true;


    private void Awake()
    {
        outline = GetComponentInChildren<Outline>();
        isInvincible = true;
        timerLength = 5;
    }
    private void Start()
    {
        originalColour = outline.OutlineColor;
        StartCoroutine(WaitToTurnOffInvincible());
        StartCoroutine(OutlineFlashColour());
    }

    IEnumerator WaitToTurnOffInvincible()
    {
        yield return new WaitForSeconds(timerLength);
        isInvincible = false;
    }

    public void BeginFlash()
    {
        StartCoroutine(OutlineFlashWhite());
        StartCoroutine(ReturnToNormalColour());
    }
    IEnumerator ReturnToNormalColour()
    {
        yield return new WaitForSeconds(1.1f);
        outline.enabled = true;
        outline.OutlineColor = originalColour;
    }
    public IEnumerator OutlineFlashWhite()
    {

        outline.OutlineColor = Color.white;
        yield return new WaitForSeconds(0.1f);

        outline.enabled = false;

        yield return new WaitForSeconds(0.1f);

        outline.enabled = true;

        yield return new WaitForSeconds(0.1f);

        outline.enabled = false;

        yield return new WaitForSeconds(0.1f);

        outline.enabled = true;

        yield return new WaitForSeconds(0.1f);

        outline.enabled = false;

        yield return new WaitForSeconds(0.1f);

        outline.enabled = true;

        yield return new WaitForSeconds(0.1f);

        outline.enabled = false;
        yield return new WaitForSeconds(0.1f);

        outline.enabled = true;

        yield return new WaitForSeconds(0.1f);

        outline.enabled = false;

        yield return new WaitForSeconds(0.1f);

        outline.enabled = true;
        outline.OutlineColor = originalColour;

    }
    public IEnumerator OutlineFlashColour()
    {
        outline.OutlineColor = originalColour;
        yield return new WaitForSeconds(0.1f);

        outline.enabled = false;

        yield return new WaitForSeconds(0.1f);

        outline.enabled = true;

        if(isInvincible)
        {
            StartCoroutine(OutlineFlashColour());
        }

    }
}
