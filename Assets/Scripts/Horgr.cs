using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public class Horgr : GameBehaviour
{
    public float enemyTimeLeft;
    public float enemyMaxTimeLeft;
    public float unitTimeLeft;
    public float unitMaxTimeLeft;
    public bool playerHasControl;
    public GameObject playerControlFX;
    public GameObject enemyControlFX;
    public GameObject spawnLocation;
    public GameObject skessa;
    public Slider slider;
    public Image fill;
    public SpriteRenderer mapIcon;
    public Color enemySliderColour = Color.red;
    public Color unitSliderColour = Color.blue;
    public Color neutralColour = Color.white;
    public float radius;
    public Vector3 centre;

    void Start()
    {
        centre = transform.position;
    }

    private void Update()
    {
        slider.value = CalculateTimeLeft();
        if(playerHasControl)
        {
            fill.color = unitSliderColour;
        }
        else
        {
            fill.color = enemySliderColour;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(!playerHasControl)
        {
            if (other.name == "Dreng(Clone)" || other.name == "Berserkr(Clone)" || other.name == "Knight(Clone)")
            {
                if (_HM.enemies.Count > _HM.units.Count)
                {
                    enemyTimeLeft += 1 * Time.deltaTime;
                }
            }
            if (other.tag == "Unit")
            {
                if (_HM.enemies.Count < _HM.units.Count)
                {
                    enemyTimeLeft -= 1 * Time.deltaTime;
                }
            }
        }
        else
        {
            if (other.name == "Dreng(Clone)" || other.name == "Berserkr(Clone)" || other.name == "Knight(Clone)")
            {
                if (_HM.enemies.Count > _HM.units.Count)
                {
                    enemyTimeLeft -= 1 * Time.deltaTime;
                }
            }
            if (other.tag == "Unit")
            {
                if (_HM.enemies.Count < _HM.units.Count)
                {
                    enemyTimeLeft += 1 * Time.deltaTime;
                }
            }
        }
        if(enemyTimeLeft <= 0)
        {

            if (!playerHasControl)
            {
                if(_HM.units.Count != 0)
                playerHasControl = true;
                mapIcon.color = neutralColour;
            }
            else
            {
                if(_HM.enemies.Count != 0)
                playerHasControl = false;
                mapIcon.color = neutralColour;
            }
        }
        if (enemyTimeLeft >= enemyMaxTimeLeft)
        {
            if (playerHasControl)
            {
                enemyTimeLeft = enemyMaxTimeLeft;
                mapIcon.color = unitSliderColour;
                playerControlFX.SetActive(true);
                _HM.playerOwns = true;
            }
            else
            {
                enemyTimeLeft = enemyMaxTimeLeft;
                mapIcon.color = enemySliderColour;
                enemyControlFX.SetActive(true);
                _HM.enemyOwns = true;
            }
        }
        else
        {
            _HM.playerOwns = false;
            _HM.enemyOwns = false;
            enemyControlFX.SetActive(false);
            playerControlFX.SetActive(false);
        }
    }
    float CalculateTimeLeft()
    {
        return enemyTimeLeft / enemyMaxTimeLeft;
    }

    //public void SpawnSkessa()
    //{
    //    if(playerHasControl == true)
    //    {
    //        if (_GM.maegen >= 200)
    //        {
    //            Instantiate(skessa, spawnLocation.transform.position, Quaternion.Euler(0, 0, 0));
    //            _GM.maegen -= 200;
    //            _UI.CheckPopulousUI();
    //        }
    //        else
    //        {
    //            _UI.SetErrorMessageInsufficientResources();
    //            _PC.Error();

    //        }
    //    }
    //    if (playerHasControl == false)
    //    {
    //        _UI.SetErrorMessageNeedToClaimHorgr();
    //        _PC.Error();
    //    }
    //}
}
