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
                    _GM.ShiftClickSelect(hit.collider.gameObject.GetComponent<Guardian>());
                }
                else
                {
                    if(_GM.canDoubleClick)
                    {
                        GuardianID thisUnitType = hit.collider.gameObject.GetComponent<Guardian>().guardianID;
                        foreach(Guardian unit in _GM.guardianList)
                        {
                            if(unit.guardianID == thisUnitType)
                            {
                                _GM.DoubleClickSelect(unit);
                            }
                        }
                    }
                    else
                    {
                        _GM.ClickSelect(hit.collider.gameObject.GetComponent<Guardian>());
                    }
                }
                _GM.StartCoroutine(_GM.DoubleClick());
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    if(_PC.mouseOverMap == false)
                    _GM.DeselectAll();
                }
            }
        }
    }
}
