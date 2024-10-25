using UnityEngine;

public class UnitClick : GameBehaviour
{
    private Camera myCam;
    public LayerMask clickable;
    public LayerMask ground;

    void Start()
    {
        myCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    _UM.ShiftClickSelect(hit.collider.gameObject.GetComponent<Unit>());
                }
                else
                {
                    if(_UM.canDoubleClick)
                    {
                        CreatureID thisUnitType = hit.collider.gameObject.GetComponent<Unit>().unitID;
                        foreach(Unit unit in _UM.unitList)
                        {
                            if(unit.unitID == thisUnitType)
                            {
                                _UM.DoubleClickSelect(unit);
                            }
                        }
                    }
                    else
                    {
                        _UM.ClickSelect(hit.collider.gameObject.GetComponent<Unit>());
                    }
                }
                _UM.StartCoroutine(_UM.DoubleClick());
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    if(_PC.mouseOverMap == false)
                    _UM.DeselectAll();
                }
            }
        }
    }
}
