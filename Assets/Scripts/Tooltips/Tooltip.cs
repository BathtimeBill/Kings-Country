using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tooltip : GameBehaviour
{
    public string title;
    public string message;

    public void SetValues(string _title, string _message)
    {
        title = _title;
        message = _message;
    }

    private void OnMouseEnter()
    {
        StartCoroutine(TooltipDelay());
    }
    private void OnMouseExit()
    {
        _Tool.HideTooltip();
    }

    public void OnButtonHover()
    {
        _Tool.SetAndShowTooltip(message, title);
    }
    public void OnButtonHoverTop()
    {
        StartCoroutine(TooltipDelayTop());
    }
    public void OnButtonHoverPopulous()
    {
        _Tool.SetAndShowPopulousTooltip();
    }
    public void OnButtonExit()
    {
        _Tool.HideTooltip();
        StopAllCoroutines();
    }
    public void OnButtonExitTop()
    {
        _Tool.HideTooltipTop();
        StopAllCoroutines();
    }
    public void OnButtonDelay()
    {
        StartCoroutine(TooltipDelay());
    }
    IEnumerator TooltipDelay()
    {
        yield return new WaitForSeconds(1);
        _Tool.SetAndShowTooltip(message, title);
    }
    IEnumerator TooltipDelayTop()
    {
        yield return new WaitForSeconds(0.8f);
        _Tool.SetAndShowTooltipTop(message, title);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            _Tool.HideTooltip();
        }    
    }
}
