using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeTree : GameBehaviour
{
    public float health;
    public float maxHealth;
    public Slider slider;
    public GameObject selectionCircle;
    public GameObject hitParticle;
    public GameObject spawnParticle;
    public GameObject particleSpawnPoint;
    public GameObject satyr;
    public GameObject orcus;
    public GameObject leshy;
    public GameObject spawnLocation;
    public GameObject deathParticle;
    public AudioSource audioSource;
    private AudioClip audioclip;
    public GameObject homeTreeTower;
    void Start()
    {
        maxHealth = 250;
        health = maxHealth;
        CalculateHealth();
    }
    //This returns the Home Tree's health level to be represented by its slider.
    float CalculateHealth()
    {
        return health / maxHealth;
    }
    //Checks to see if the player has enough resources and then spawns a Satyr unit in front of the Home Tree. Is called when a button is pressed in the UI.
    public void SpawnSatyr()
    {
        if(_GM.maegen >0)
        {
            if(_GM.populous < _GM.maxPopulous)
            {
                _GM.maegen -= 1;
                Instantiate(satyr, spawnLocation.transform.position, spawnLocation.transform.rotation);
                Instantiate(spawnParticle, spawnLocation.transform.position, Quaternion.Euler(-90, 0, 0));
                _UI.CheckPopulousUI();
                CheckForNumberOfUnitsForTutorial();
            }
            else
            {
                _UI.SetErrorMessageMaxPop();
                _PC.Error();
            }
        }
        else
        {
            _UI.SetErrorMessageInsufficientMaegen();
            _PC.Error();
        }
    }
    //Checks to see if the player has enough resources and then spawns a Orcus unit in front of the Home Tree. Is called when a button is pressed in the UI.
    public void SpawnOrcus()
    {
        if (_GM.maegen > 3)
        {
            if(_GM.populous < _GM.maxPopulous)
            {
                _GM.maegen -= 4;
                Instantiate(orcus, spawnLocation.transform.position, spawnLocation.transform.rotation);
                Instantiate(spawnParticle, spawnLocation.transform.position, Quaternion.Euler(-90, 0, 0));
                _UI.CheckPopulousUI();
                CheckForNumberOfUnitsForTutorial();
            }
            else
            {
                _UI.SetErrorMessageMaxPop();
                _PC.Error();
            }
        }
        else
        {
            _UI.SetErrorMessageInsufficientMaegen();
            _PC.Error();
        }
    }
    //Checks to see if the player has enough resources and then spawns a Leshy unit in front of the Home Tree. Is called when a button is pressed in the UI.
    public void SpawnLeshy()
    {
        if (_GM.maegen > 5)
        {
            if(_GM.populous < _GM.maxPopulous)
            {
                _GM.maegen -= 6;
                Instantiate(leshy, spawnLocation.transform.position, spawnLocation.transform.rotation);
                Instantiate(spawnParticle, spawnLocation.transform.position, Quaternion.Euler(-90, 0, 0));
                _UI.CheckPopulousUI();
                CheckForNumberOfUnitsForTutorial();
            }
            else
            {
                _UI.SetErrorMessageMaxPop();
                _PC.Error();
            }
        }
        else
        {
            _UI.SetErrorMessageInsufficientMaegen();
            _PC.Error();
        }
    }
    private void CheckForNumberOfUnitsForTutorial()
    {
        if (_TUTM.isTutorial)
        {
            if (_GM.populous >= 2 && _TUTM.tutorialStage == 4)
            {
                GameEvents.ReportOnNextTutorial();
            }
        }
    }
    private void SummonSound()
    {
        audioclip = _SM.summonSound;
        audioSource.clip = audioclip;
        audioSource.Play();
    }

    //If the Home Tree collides with the enemy weapons, it takes damage.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Axe1")
        {
            TakeDamage(10);
            other.enabled = false;
        }
        if (other.tag == "Axe2")
        {
            TakeDamage(20);
            other.enabled = false;
        }
        if (other.tag == "Sword2")
        {
            TakeDamage(35);
            other.enabled = false;
        }
        if (other.tag == "Sword3")
        {
            TakeDamage(50);
            other.enabled = false;
        }
    }
    //Manages the amount of damage taken by an enemy weapon. Updates the health slider, plays an impact sound, and checks to see if the Home Tree is dead.
    public void TakeDamage(float damage)
    {
        audioSource.clip = _SM.GetChopSounds();
        audioSource.Play();
        Instantiate(hitParticle, particleSpawnPoint.transform.position, transform.rotation);
        health -= damage;
        slider.value = slider.value = CalculateHealth();
        if(health < 1)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            GameEvents.ReportOnGameOver();
            rb.useGravity = true;
            deathParticle.SetActive(true);
            //rb.freezePosition.y = false;
            health = 10000;
        }
    }

    private void OnWinfallUpgrade()
    {
        _GM.maegen += 15;
    }

    private void OnHomeTreeUpgrade()
    {
        homeTreeTower.SetActive(true);
    }
    private void OnHomeTreeSelected()
    {
        selectionCircle.SetActive(true);
    }
    private void OnHomeTreeDeselected()
    {
        selectionCircle.SetActive(false);
    }
    private void OnEnable()
    {
        GameEvents.OnHomeTreeDeselected += OnHomeTreeDeselected;
        GameEvents.OnHomeTreeSelected += OnHomeTreeSelected;
        GameEvents.OnWinfallUpgrade += OnWinfallUpgrade;
        GameEvents.OnHomeTreeUpgrade += OnHomeTreeUpgrade;
    }

    private void OnDisable()
    {
        GameEvents.OnHomeTreeDeselected -= OnHomeTreeDeselected;
        GameEvents.OnHomeTreeSelected -= OnHomeTreeSelected;
        GameEvents.OnWinfallUpgrade -= OnWinfallUpgrade;
        GameEvents.OnHomeTreeUpgrade -= OnHomeTreeUpgrade;
    }
}
