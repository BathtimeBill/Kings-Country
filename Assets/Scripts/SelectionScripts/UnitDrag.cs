using UnityEngine;

public class UnitDrag : GameBehaviour
{
    Camera myCam;

    [SerializeField]
    RectTransform boxVisual;

    Rect selectionBox;

    Vector2 startPosition;
    Vector2 endPosition;

    void Start()
    {
        myCam = Camera.main;
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        DrawVisual();
    }

    void DrawVisual()
    {
        Vector2 boxStart = startPosition;
        Vector2 boxEnd = endPosition;

        Vector2 boxCenter = (boxStart + boxEnd) / 2;
        boxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        boxVisual.sizeDelta = boxSize;
    }

    void DrawSelection()
    {
        if (Input.mousePosition.x < startPosition.x)
        {
            selectionBox.xMin = Input.mousePosition.x;
            selectionBox.xMax = startPosition.x;
        }
        else
        {
            selectionBox.xMin = startPosition.x;
            selectionBox.xMax = Input.mousePosition.x;
        }

        if (Input.mousePosition.y < startPosition.y)
        {
            selectionBox.yMin = Input.mousePosition.y;
            selectionBox.yMax = startPosition.y;
        }
        else
        {
            selectionBox.yMin = startPosition.y;
            selectionBox.yMax = Input.mousePosition.y;
        }
    }

    void SelectUnits()
    {
        foreach (var unit in UnitSelection.Instance.unitList)
        {
            if (selectionBox.Contains(myCam.WorldToScreenPoint(unit.transform.position)))
            {
                if (unit.gameObject.tag != "Tower")
                {
                    UnitSelection.Instance.DragSelect(unit);
                }
                
            }
        }
    }

    private void OnSelectButtonPressed()
    {
        if (!_hasInput)
            return;

        startPosition = Input.mousePosition;
        selectionBox = new Rect();
    }
    private void OnSelectButtonHolding()
    {
        if (!_hasInput)
            return;

        endPosition = Input.mousePosition;
        DrawVisual();
        DrawSelection();
    }
    private void OnSelectButtonReleased()
    {
        if (!_hasInput)
            return;

        SelectUnits();
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        DrawVisual();
    }

    private void OnEnable()
    {
        InputManager.OnSelectButtonPressed += OnSelectButtonPressed;
        InputManager.OnSelectButtonHolding += OnSelectButtonHolding;
        InputManager.OnSelectButtonReleased += OnSelectButtonReleased;
    }

    private void OnDisable()
    {
        InputManager.OnSelectButtonPressed -= OnSelectButtonPressed;
        InputManager.OnSelectButtonHolding -= OnSelectButtonHolding;
        InputManager.OnSelectButtonReleased -= OnSelectButtonReleased;
    }

}
