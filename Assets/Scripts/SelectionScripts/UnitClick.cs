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
                    UnitSelection.Instance.ShiftClickSelect(hit.collider.gameObject);
                    
                }
                else
                {
                    if(UnitSelection.Instance.canDoubleClick)
                    {
                        UnitType thisUnitType = hit.collider.gameObject.GetComponent<Unit>().unitType;
                        foreach(GameObject go in UnitSelection.Instance.unitList)
                        {
                            if(go.gameObject.GetComponent<Unit>().unitType == thisUnitType)
                            {
                                UnitSelection.Instance.DoubleClickSelect(go);
                            }
                        }
                    }
                    else
                    {
                        UnitSelection.Instance.ClickSelect(hit.collider.gameObject);
                    }
                }
                UnitSelection.Instance.StartCoroutine(UnitSelection.Instance.DoubleClick());
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    if(_PC.mouseOverMap == false)
                    UnitSelection.Instance.DeselectAll();
                }
            }



        }
    }
}
