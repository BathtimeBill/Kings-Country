using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitButtonManager : MonoBehaviour
{
    public UnitButton[] unitButtons;
    public InfoBox infoBox;

    public void PointerEnter(UnitButton _unitButton)
    {
        for(int i = 0; i < unitButtons.Length; i++)
        {
            if(_unitButton == unitButtons[i])
            {
                unitButtons[i].FadeInButton();
                infoBox.OnButtonHover(_unitButton.unitData.description, _unitButton.unitData.GetStats());
            }
            else
            {
                unitButtons[i].FadeOutButton();
            }
        }
    }

    public void PointerExit()
    {
        for (int i = 0; i < unitButtons.Length; i++)
        {
            unitButtons[i].FadeInButton();
            infoBox.OnButtonExit();
        }
    }
}
