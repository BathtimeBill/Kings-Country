using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : GameBehaviour
{
    public string title;
    public string message;

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
    public void OnButtonHoverPopulous()
    {
        _Tool.SetAndShowPopulousTooltip();
    }
    public void OnButtonExit()
    {
        _Tool.HideTooltip();
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
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            _Tool.HideTooltip();
        }    
    }
}
