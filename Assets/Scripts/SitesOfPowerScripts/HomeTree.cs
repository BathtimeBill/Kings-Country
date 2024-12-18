using UnityEngine;

public class HomeTree : SiteOfPower
{
    public float health;
    public float maxHealth;
    public GameObject hitParticle;
    public GameObject deathParticle;
    public AudioSource audioSource;
    private AudioClip audioclip;
    public GameObject homeTreeTower;
    private void Start()
    {
        maxHealth = siteData.health;
        health = maxHealth;
    }
    
    //If the Home Tree collides with the enemy weapons, it takes damage.
    private void OnTriggerEnter(Collider other)
    {
        UnitWeaponCollider uwc = other.GetComponent<UnitWeaponCollider>();
        if (uwc == null || uwc.unitType == UnitType.Guardian)
            return;
        
        TakeDamage(uwc.Damage, other.transform.position);
        other.enabled = false;
    }
    //Manages the amount of damage taken by an enemy weapon. Updates the health slider, plays an impact sound, and checks to see if the Home Tree is dead.
    private void TakeDamage(float damage, Vector3 spawn)
    {
        health -= damage;
        healthBar.AdjustHealthBar(health, maxHealth, GetCaptureColor(siteState));
        audioSource.clip = _SM.GetChopSounds();
        audioSource.Play();
        Instantiate(hitParticle, spawn, transform.rotation);
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
        _GAME.maegen += 15;
    }

    private void OnHomeTreeUpgrade()
    {
        homeTreeTower.SetActive(true);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameEvents.OnWinfallUpgrade += OnWinfallUpgrade;
        GameEvents.OnHomeTreeUpgrade += OnHomeTreeUpgrade;
    }

    

    protected override void OnDisable()
    {
        base.OnDisable();
        GameEvents.OnWinfallUpgrade -= OnWinfallUpgrade;
        GameEvents.OnHomeTreeUpgrade -= OnHomeTreeUpgrade;
    }

}
