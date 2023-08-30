using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineFlash : GameBehaviour
{
    public Outline outline;
    Color originalColour;
    private void Awake()
    {
        outline = GetComponentInChildren<Outline>();
        originalColour = outline.OutlineColor;
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
}
