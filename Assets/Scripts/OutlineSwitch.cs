using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineSwitch : GameBehaviour
{
    public Outline outlineScript;
    public Outline outlineScript2;

    private void OnToggleOutline()
    {
        outlineScript.enabled = !outlineScript.enabled;
        outlineScript2.enabled = !outlineScript2.enabled;
    }
    private void OnEnable()
    {
        GameEvents.OnToggleOutline += OnToggleOutline;

    }

    private void OnDisable()
    {
        GameEvents.OnToggleOutline -= OnToggleOutline;

    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        OnToggleOutline();
    //    }
    //}
}
