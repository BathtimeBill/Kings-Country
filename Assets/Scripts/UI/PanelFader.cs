using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PanelFader : GameBehaviour
{
    CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeIn()
    {
        FadeX.FadeIn(canvasGroup);
    }

    public void FadeOut()
    {
        FadeX.FadeOut(canvasGroup);
    }
}
