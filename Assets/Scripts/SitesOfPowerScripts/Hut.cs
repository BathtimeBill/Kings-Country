using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Hut : GameBehaviour
{
    public float enemyTimeLeft;
    public float enemyMaxTimeLeft;
    public float unitTimeLeft;
    public float unitMaxTimeLeft;
    public float claimRate;
    public bool playerHasControl;
    public GameObject playerControlFX;
    public GameObject enemyControlFX;
    public GameObject spawnLocation;
    public GameObject skessa;
    public GameObject huldra;
    public GameObject selectionCircle;
    public Slider slider;
    public Image fill;
    public SpriteRenderer mapIcon;
    public Color enemySliderColour = Color.red;
    public Color unitSliderColour = Color.blue;
    public Color neutralColour = Color.white;
    public float radius;
    public Vector3 centre;
    public AudioSource audioSource;

    void Start()
    {
        ClaimHut();
        centre = transform.position;
        audioSource = GetComponent<AudioSource>();
        _HUTM.hutObject = gameObject;
        _HUTM.spawnLocation = spawnLocation;
    }

    private void Update()
    {
        slider.value = CalculateTimeLeft();
        if (playerHasControl)
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
        if (!playerHasControl)
        {
            if (other.name == "Wathe(Clone)" || other.name == "Hunter(Clone)" || other.name == "Bjornjeger(Clone)")
            {
                if (_HUTM.enemies.Count > _HUTM.units.Count)
                {
                    enemyTimeLeft += claimRate * Time.deltaTime;
                }
            }
            if (other.tag == "Unit" || other.tag == "LeshyUnit")
            {
                if (_HUTM.enemies.Count < _HUTM.units.Count)
                {
                    enemyTimeLeft -= claimRate * Time.deltaTime;
                }
            }
        }
        else
        {
            if (other.name == "Wathe(Clone)" || other.name == "Hunter(Clone)" || other.name == "Bjornjeger(Clone)")
            {
                if (_HUTM.enemies.Count > _HUTM.units.Count)
                {
                    enemyTimeLeft -= claimRate * Time.deltaTime;
                }
            }
            if (other.tag == "Unit" || other.tag == "LeshyUnit")
            {
                if (_HUTM.enemies.Count < _HUTM.units.Count)
                {
                    enemyTimeLeft += claimRate * Time.deltaTime;
                }
            }
        }
        if (enemyTimeLeft <= 0)
        {

            if (!playerHasControl)
            {
                if (_HUTM.units.Count != 0)
                    playerHasControl = true;
                mapIcon.color = neutralColour;
            }
            else
            {
                if (_HUTM.enemies.Count != 0)
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
                _HUTM.playerOwns = true;
                _HUTM.hasBeenClaimed = true;
            }
            else
            {
                enemyTimeLeft = enemyMaxTimeLeft;
                mapIcon.color = enemySliderColour;
                enemyControlFX.SetActive(true);
                _HUTM.enemyOwns = true;
            }
        }
        else
        {
            _HUTM.playerOwns = false;
            _HUTM.enemyOwns = false;
            enemyControlFX.SetActive(false);
            playerControlFX.SetActive(false);
        }
    }
    float CalculateTimeLeft()
    {
        return enemyTimeLeft / enemyMaxTimeLeft;
    }

    public void SpawnSkessa()
    {
        Instantiate(skessa, spawnLocation.transform.position, Quaternion.Euler(0, 0, 0));
    }
    public void ClaimHut()
    {
        enemyTimeLeft = enemyMaxTimeLeft;
        mapIcon.color = unitSliderColour;
        playerControlFX.SetActive(true);
        _HUTM.playerOwns = true;
        playerHasControl = true;
    }
    private void OnContinueButton()
    {
        ClaimHut();
    }
    private void OnHutSelected()
    {
        selectionCircle.SetActive(true);
    }
    private void OnHutDeselected()
    {
        selectionCircle.SetActive(false);
    }
    private void OnEnable()
    {
        GameEvents.OnHutDeselected += OnHutDeselected;
        GameEvents.OnHutSelected += OnHutSelected;
        GameEvents.OnContinueButton += OnContinueButton;
    }

    private void OnDisable()
    {
        GameEvents.OnHutDeselected -= OnHutDeselected;
        GameEvents.OnHutSelected -= OnHutSelected;
        GameEvents.OnContinueButton -= OnContinueButton;
    }
}
