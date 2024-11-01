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
    public GameObject godRay;
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
        int cost = _DATA.GetUnit(CreatureID.Satyr).cost;

        if (_GM.maegen < cost)
        {
            _UI.SetError(ErrorID.InsufficientMaegen);
            return;
        }

        if(_GM.populous < _GM.maxPopulous)
        {
            _GM.DecreaseMaegen(cost);
            Instantiate(satyr, spawnLocation.transform.position, spawnLocation.transform.rotation);
            Instantiate(spawnParticle, spawnLocation.transform.position, Quaternion.Euler(-90, 0, 0));
            _UI.CheckPopulousUI();
        }
        else
        {
            _UI.SetError(ErrorID.MaxPopulation);
        }
    }
    //Checks to see if the player has enough resources and then spawns a Orcus unit in front of the Home Tree. Is called when a button is pressed in the UI.
    public void SpawnOrcus()
    {
        int cost = _DATA.GetUnit(CreatureID.Orcus).cost;

        if (_GM.maegen < cost)
        {
            _UI.SetError(ErrorID.InsufficientMaegen);
            return;
        }

        if(_GM.populous < _GM.maxPopulous)
        {
            _GM.DecreaseMaegen(cost);
            Instantiate(orcus, spawnLocation.transform.position, spawnLocation.transform.rotation);
            Instantiate(spawnParticle, spawnLocation.transform.position, Quaternion.Euler(-90, 0, 0));
            _UI.CheckPopulousUI();
        }
        else
        {
            _UI.SetError(ErrorID.MaxPopulation);
        }
    }
    //Checks to see if the player has enough resources and then spawns a Leshy unit in front of the Home Tree. Is called when a button is pressed in the UI.
    public void SpawnLeshy()
    {
        int cost = _DATA.GetUnit(CreatureID.Leshy).cost;
        if (_GM.maegen < cost)
        {
            _UI.SetError(ErrorID.InsufficientMaegen);
            return;
        }

        if(_GM.populous < _GM.maxPopulous)
        {
            _GM.DecreaseMaegen(cost);
            Instantiate(leshy, spawnLocation.transform.position, spawnLocation.transform.rotation);
            Instantiate(spawnParticle, spawnLocation.transform.position, Quaternion.Euler(-90, 0, 0));
            _UI.CheckPopulousUI();
        }
        else
        {
            _UI.SetError(ErrorID.MaxPopulation);
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
            TakeDamage(10, other.transform.position);
            other.enabled = false;
        }
        if (other.tag == "Axe2")
        {
            TakeDamage(20, other.transform.position);
            other.enabled = false;
        }
        if (other.tag == "Sword2")
        {
            TakeDamage(35, other.transform.position);
            other.enabled = false;
        }
        if (other.tag == "Sword3")
        {
            TakeDamage(50, other.transform.position);
            other.enabled = false;
        }
        if(other.tag == "LordWeapon")
        {
            TakeDamage(50, other.transform.position);
            other.enabled = false;
        }
    }
    //Manages the amount of damage taken by an enemy weapon. Updates the health slider, plays an impact sound, and checks to see if the Home Tree is dead.
    public void TakeDamage(float damage, Vector3 spawn)
    {
        audioSource.clip = _SM.GetChopSounds();
        audioSource.Play();
        Instantiate(hitParticle, spawn, transform.rotation);
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


    private void OnUnitButtonPressed(UnitData _unitData)
    {
        switch(_unitData.id)
        {
            case CreatureID.Satyr:
                SpawnSatyr();
                break;
            case CreatureID.Orcus:
                SpawnOrcus();
                break;
            case CreatureID.Leshy:
                SpawnLeshy();
                break;
        }
    }

    private void OnGameStateChanged(GameState _gameState)
    {
        if(_inGame)
            slider.gameObject.SetActive(true);
        else
            slider.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.OnHomeTreeDeselected += OnHomeTreeDeselected;
        GameEvents.OnHomeTreeSelected += OnHomeTreeSelected;
        GameEvents.OnWinfallUpgrade += OnWinfallUpgrade;
        GameEvents.OnHomeTreeUpgrade += OnHomeTreeUpgrade;
        GameEvents.OnUnitButtonPressed += OnUnitButtonPressed;
        GameEvents.OnGameStateChanged += OnGameStateChanged;
    }

    

    private void OnDisable()
    {

        GameEvents.OnHomeTreeDeselected -= OnHomeTreeDeselected;
        GameEvents.OnHomeTreeSelected -= OnHomeTreeSelected;
        GameEvents.OnWinfallUpgrade -= OnWinfallUpgrade;
        GameEvents.OnHomeTreeUpgrade -= OnHomeTreeUpgrade;
        GameEvents.OnUnitButtonPressed -= OnUnitButtonPressed;
        GameEvents.OnGameStateChanged -= OnGameStateChanged;
    }

}
