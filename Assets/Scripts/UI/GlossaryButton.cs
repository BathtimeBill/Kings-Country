using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlossaryButton : GameBehaviour
{
    public TutorialID tutorialID;

    public void Button()
    {
        _TUTM.tutorialID = tutorialID;
        _TUTM.CheckTutorial();
    }
}
