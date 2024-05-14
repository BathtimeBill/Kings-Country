using UnityEngine;
using TMPro;

public class CombatPanel : MonoBehaviour
{
    [Header("Info Box")]
    public CanvasGroup canvasGroup;
    public TMP_Text titleText;
    public TMP_Text descriptionText;

    private void Start()
    {
        canvasGroup.gameObject.SetActive(false);
    }

    public void PointerEnter(CombatButton _combatButton)
    {
        canvasGroup.gameObject.SetActive(true);
        titleText.text = _combatButton.title;
        descriptionText.text = _combatButton.description;
    }

    public void PointerExit()
    {
        canvasGroup.gameObject.SetActive(false);
    }
}
