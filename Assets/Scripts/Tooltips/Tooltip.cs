using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : GameBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string title;
    public string message;
    public UIPivotPosition pivotPosition;
    public void SetValues(string _title, string _message)
    {
        title = _title;
        message = _message;
    }
    
    public void OnPointerEnter(PointerEventData data)
    {
        StartCoroutine(TooltipDelay());
    }
    public void OnPointerExit(PointerEventData data)
    {
        _Tool.HideTooltip();
        StopAllCoroutines();
    }
    IEnumerator TooltipDelay()
    {
        yield return new WaitForSeconds(_Tool.tooltipDelay);
        _Tool.SetAndShowTooltip(message, title, pivotPosition);
    }
}
