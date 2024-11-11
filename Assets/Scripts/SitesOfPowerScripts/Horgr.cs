using UnityEngine.UI;
using UnityEngine;

public class Horgr : SiteOfPower
{
    [Header("Horgr Specific")]
    public bool playerHasControl;
    public Image fill;
    public Color enemySliderColour = Color.red;
    public Color unitSliderColour = Color.blue;
    public Color neutralColour = Color.white;
    
    private void OnTriggerStay(Collider other)
    {
        if(!playerHasControl)
        {
            if (other.name == "Dreng(Clone)" || other.name == "Berserkr(Clone)" || other.name == "Knight(Clone)")
            {
                if (enemies.Count > units.Count)
                {
                    currentClaimTime += claimRate * Time.deltaTime;
                }
            }
            if (other.tag == "Unit" || other.tag == "LeshyUnit")
            {
                if (enemies.Count < units.Count)
                {
                    currentClaimTime -= claimRate * Time.deltaTime;
                }
            }
        }
        else
        {
            if (other.name == "Dreng(Clone)" || other.name == "Berserkr(Clone)" || other.name == "Knight(Clone)")
            {
                if (enemies.Count > units.Count)
                {
                    currentClaimTime -= claimRate * Time.deltaTime;
                }
            }
            if (other.tag == "Unit" || other.tag == "LeshyUnit")
            {
                if (enemies.Count < units.Count)
                {
                    currentClaimTime += claimRate * Time.deltaTime;
                }
            }
        }
        if(currentClaimTime <= 0)
        {

            if (!playerHasControl)
            {
                if(units.Count != 0)
                playerHasControl = true;
                mapIcon.ChangeMapIconColor(neutralColour);
            }
            else
            {
                if(enemies.Count != 0)
                playerHasControl = false;
                mapIcon.ChangeMapIconColor(neutralColour);
            }
        }
        if (currentClaimTime >= enemyMaxTimeLeft)
        {
            if (playerHasControl)
            {
                currentClaimTime = enemyMaxTimeLeft;
                mapIcon.ChangeMapIconColor(unitSliderColour);
                playerControlFX.SetActive(true);
                playerOwns = true;
                //hasBeenClaimed = true;
            }
            else
            {
                currentClaimTime = enemyMaxTimeLeft;
                mapIcon.ChangeMapIconColor(enemySliderColour);
                enemyControlFX.SetActive(true);
                //enemyOwns = true;
            }
        }
        else
        {
            playerOwns = false;
            enemyControlFX.SetActive(false);
            playerControlFX.SetActive(false);
        }
    }
}
