using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverUI : GameBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool mouse_over = false;


    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        _UI.mouseOverUI = true;
        Debug.Log("Mouse enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
            _UI.mouseOverUI = false;
        Debug.Log("Mouse exit");
    }
}
