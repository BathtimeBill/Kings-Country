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
        FadeX.FadeInPanel(canvasGroup);
    }

    public void FadeOut()
    {
        FadeX.FadeOutPanel(canvasGroup);
    }
}
