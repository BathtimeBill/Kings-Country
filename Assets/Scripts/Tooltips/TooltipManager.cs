using UnityEngine;
using TMPro;

public class TooltipManager : Singleton<TooltipManager>
{
    public GameObject tooltipBox;
    private RectTransform rectTransform;
    public TMP_Text textComponent;
    public TMP_Text titleComponent;
    private Vector3 offset;
    public Camera UICamera;
    public float tooltipDelay = 0.6f;
    private bool showing = false;
    void Start()
    {
        Cursor.visible = true;
        tooltipBox.SetActive(false);
        rectTransform = tooltipBox.GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        if (!showing)
            return;
        
        offset = Input.mousePosition;
        offset.z = 1f; //distance of the plane from the camera
        tooltipBox.transform.position = UICamera.ScreenToWorldPoint(offset);
    }

    public void SetAndShowTooltip(string message, string title, UIPivotPosition pivotPosition)
    {
        showing = true;
        textComponent.text = message;
        titleComponent.text = title;
        rectTransform.pivot = UIX.SetPivotPoints(pivotPosition);
        tooltipBox.SetActive(true);
    }
    public void HideTooltip()
    {
        showing = false;
        tooltipBox.SetActive(false);
    }
}
