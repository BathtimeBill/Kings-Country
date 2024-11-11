using UnityEngine;

public class Hut : SiteOfPower
{
    [Header("Hut Specific")]
    public Color enemySliderColour = Color.red;
    public Color unitSliderColour = Color.blue;
    public Color neutralColour = Color.white;
    
    private void OnTriggerStay(Collider other)
    {
        bool isSpecialEnemy = other.GetComponent<Enemy>() && siteData.siteEnemies.Contains(other.GetComponent<Enemy>().unitID);
        bool isUnit = other.GetComponent<Unit>() && _DATA.IsCreatureUnit(other.GetComponent<Unit>().unitID.ToString());

        if (!playerOwns)
        {
            if (isSpecialEnemy && enemies.Count > units.Count)
            {
                currentClaimTime += claimRate * Time.deltaTime;
            }
            if (isUnit && enemies.Count < units.Count)
            {
                currentClaimTime -= claimRate * Time.deltaTime;
            }
        }
        else
        {
            if (isSpecialEnemy && enemies.Count > units.Count)
            {
                currentClaimTime -= claimRate * Time.deltaTime;
            }
            if (isUnit && enemies.Count < units.Count)
            {
                currentClaimTime += claimRate * Time.deltaTime;
            }
        }

        healthBar.AdjustHealthBar(currentClaimTime, enemyMaxTimeLeft, playerOwns);

        if (currentClaimTime <= 0)
        {
            if (!playerOwns)
            {
                if (HasUnits()) 
                    playerOwns = true;
                mapIcon.ChangeMapIconColor(neutralColour);
            }
            else
            {
                if (HasEnemies()) 
                    playerOwns = false;
                mapIcon.ChangeMapIconColor(neutralColour);
            }
        }
        else if (currentClaimTime >= enemyMaxTimeLeft)
        {
            currentClaimTime = enemyMaxTimeLeft;
            if (playerOwns)
            {
                mapIcon.ChangeMapIconColor(unitSliderColour);
                playerControlFX.SetActive(true);
            }
            else
            {
                mapIcon.ChangeMapIconColor(enemySliderColour);
                enemyControlFX.SetActive(true);
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
