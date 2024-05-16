using UnityEngine;
using TMPro;

public class TogglePanel : MonoBehaviour
{
    [Header("Info Box")]
    public CanvasGroup canvasGroup;
    public TMP_Text titleText;
    public TMP_Text descriptionText;

    private void Start()
    {
        //canvasGroup.gameObject.SetActive(false);
    }

    public void PointerEnter(ToggleButton _toggleButton)
    {
        //canvasGroup.gameObject.SetActive(true);
        titleText.text = _toggleButton.title;
        descriptionText.text = _toggleButton.description;
    }

    public void PointerExit()
    {
        //canvasGroup.gameObject.SetActive(false);
    }
}
