using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Hut : SiteOfPower
{
    [Header("Hut Specific")]
    public float enemyTimeLeft;
    public float enemyMaxTimeLeft;
    public float unitTimeLeft;
    public float unitMaxTimeLeft;
    public float claimRate;
    public bool playerHasControl;
    public GameObject playerControlFX;
    public GameObject enemyControlFX;
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
        StartCoroutine(SpawnEnemyUnits());
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
                if (enemies.Count > units.Count)
                {
                    enemyTimeLeft += claimRate * Time.deltaTime;
                }
            }
            if (other.tag == "Unit" || other.tag == "LeshyUnit")
            {
                if (enemies.Count < units.Count)
                {
                    enemyTimeLeft -= claimRate * Time.deltaTime;
                }
            }
        }
        else
        {
            if (other.name == "Wathe(Clone)" || other.name == "Hunter(Clone)" || other.name == "Bjornjeger(Clone)")
            {
                if (enemies.Count > units.Count)
                {
                    enemyTimeLeft -= claimRate * Time.deltaTime;
                }
            }
            if (other.tag == "Unit" || other.tag == "LeshyUnit")
            {
                if (enemies.Count < units.Count)
                {
                    enemyTimeLeft += claimRate * Time.deltaTime;
                }
            }
        }
        if (enemyTimeLeft <= 0)
        {

            if (!playerHasControl)
            {
                if (units.Count != 0)
                    playerHasControl = true;
                mapIcon.color = neutralColour;
            }
            else
            {
                if (enemies.Count != 0)
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
                playerOwns = true;
            }
            else
            {
                enemyTimeLeft = enemyMaxTimeLeft;
                mapIcon.color = enemySliderColour;
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
    float CalculateTimeLeft()
    {
        return enemyTimeLeft / enemyMaxTimeLeft;
    }
    
    public void ClaimHut()
    {
        enemyTimeLeft = enemyMaxTimeLeft;
        mapIcon.color = unitSliderColour;
        playerControlFX.SetActive(true);
        playerOwns = true;
        playerHasControl = true;
        StopCoroutine(SpawnEnemyUnits());
    }
    
    IEnumerator SpawnEnemyUnits()
    {
        yield return new WaitForSeconds(Random.Range(spawnRates.min, spawnRates.max));
        if (!playerOwns)
        {
            _EM.SpawnHutEnemy(spawnLocation.transform.position);
        }
        StartCoroutine(SpawnEnemyUnits());
    }
    
    private void OnContinueButton()
    {
        ClaimHut();
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        GameEvents.OnContinueButton += OnContinueButton;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameEvents.OnContinueButton -= OnContinueButton;
    }
}
