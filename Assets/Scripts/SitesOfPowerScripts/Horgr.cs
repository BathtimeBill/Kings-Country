using UnityEngine.UI;
using UnityEngine;

public class Horgr : SiteOfPower
{
    public float enemyTimeLeft;
    public float enemyMaxTimeLeft;
    public float unitTimeLeft;
    public float unitMaxTimeLeft;
    public float claimRate;
    public bool playerHasControl;
    public GameObject playerControlFX;
    public GameObject enemyControlFX;
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
        centre = transform.position;
        audioSource = GetComponent<AudioSource>();
        ClaimHorgr();
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
            if (other.name == "Dreng(Clone)" || other.name == "Berserkr(Clone)" || other.name == "Knight(Clone)")
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
        if(enemyTimeLeft <= 0)
        {

            if (!playerHasControl)
            {
                if(units.Count != 0)
                playerHasControl = true;
                mapIcon.color = neutralColour;
            }
            else
            {
                if(enemies.Count != 0)
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
                //hasBeenClaimed = true;
            }
            else
            {
                enemyTimeLeft = enemyMaxTimeLeft;
                mapIcon.color = enemySliderColour;
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

    public void ClaimHorgr()
    {
        enemyTimeLeft = enemyMaxTimeLeft;
        mapIcon.color = unitSliderColour;
        playerControlFX.SetActive(true);
        playerOwns = true;
        playerHasControl = true;
    }
    float CalculateTimeLeft()
    {
        return enemyTimeLeft / enemyMaxTimeLeft;
    }

    private void OnContinueButton()
    {
        ClaimHorgr();
    }
    private void OnHorgrSelected()
    {
        selectionCircle.SetActive(true);
    }
    private void OnHorgrDeselected()
    {
        selectionCircle.SetActive(false);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameEvents.OnHorgrDeselected += OnHorgrDeselected;
        GameEvents.OnHorgrSelected += OnHorgrSelected;
        GameEvents.OnContinueButton += OnContinueButton;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameEvents.OnHorgrDeselected -= OnHorgrDeselected;
        GameEvents.OnHorgrSelected -= OnHorgrSelected;
        GameEvents.OnContinueButton -= OnContinueButton;
    }

}
