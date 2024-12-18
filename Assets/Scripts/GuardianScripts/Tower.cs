using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tower : GameBehaviour
{
    public GuardianID unitType;
    public Animator animator;

    public GameObject firingPoint;
    public GameObject projectile1;
    public GameObject projectile2;
    public Vector3 closestEnemy;
    public float projectileSpeed;
    public float distanceToClosestEnemy;
    public Slider slider;

    public float health;
    public float maxHealth;
    public float fireRate;

    public GameObject woodParticle;
    public GameObject explosionParticle;
    public GameObject spawnParticleEffect;
    public MeshRenderer towerRadius;

    public bool towerUpgrade;

    public Guardian parent;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        parent = gameObject.transform.parent.gameObject.GetComponent<Guardian>();
        StartCoroutine(ShootProjectile());
        _GM.guardianList.Add(parent);
        slider.value = CalculateHealth();
        Setup();
        Instantiate(spawnParticleEffect, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {

        if (_EnemiesExist)
        {
            closestEnemy = GetClosestEnemy().transform.position;
            firingPoint.transform.LookAt(closestEnemy);
            distanceToClosestEnemy = Vector3.Distance(closestEnemy, transform.position);
        }
        else
            distanceToClosestEnemy = 100;

    }

    private void OnMouseOver()
    {
        towerRadius.enabled = true;
    }
    private void OnMouseExit()
    {
        towerRadius.enabled = false;
    }
    IEnumerator ShootProjectile()
    {

        yield return new WaitForSeconds(fireRate);

        if (distanceToClosestEnemy < 50)
        {
            if(_DATA.HasPerk(PerkID.Tower))
            {

                if(unitType == GuardianID.FidhainTower)
                {
                    animator.SetTrigger("Spit");
                }
                if (unitType == GuardianID.Tower)
                {
                    GameObject go = Instantiate(projectile2, firingPoint.transform.position, firingPoint.transform.rotation);
                    go.GetComponent<Rigidbody>().AddForce(firingPoint.transform.forward * projectileSpeed);
                }
            }
            else
            {
                if (unitType == GuardianID.FidhainTower)
                {
                    animator.SetTrigger("Spit");
                }
                if (unitType == GuardianID.Tower)
                {
                    GameObject go = Instantiate(projectile1, firingPoint.transform.position, firingPoint.transform.rotation);
                    go.GetComponent<Rigidbody>().AddForce(firingPoint.transform.forward * projectileSpeed);
                }
            }

        }


        StartCoroutine(ShootProjectile());
    }
    public Transform GetClosestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        Transform trans = null;

        foreach (GameObject go in _EM.enemies)
        {
            float currentDistance;
            currentDistance = Vector3.Distance(transform.position, go.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                trans = go.transform;
            }
        }

        if (_EM.enemies == null)
        {
            return null;
        }
        else
            return trans;
    }
    private void OnTriggerEnter(Collider other)
    {
        UnitWeaponCollider uwc = other.GetComponent<UnitWeaponCollider>();
        if (!uwc)
            return;
        
        TakeDamage(uwc.Damage);
        uwc.ToggleCollider(false);
    }
    public void TakeDamage(float damage)
    {
        Instantiate(woodParticle, transform.position, transform.rotation);
        health -= damage;
        slider.value = slider.value = CalculateHealth();
        Die();
    }
    private void Die()
    {
        if (health <= 0)
        {
            if(_HorgrExists)
                _HORGR.RemoveUnit(parent);

            GameObject go = Instantiate(explosionParticle, transform.position, transform.rotation);
            Destroy(go, 15);
            _UI.CheckPopulousUI();
            if(unitType == GuardianID.Tower)
            {
                //TODO FIX BELOW
                //UnitSelection.Instance.unitList.Remove(gameObject);
                Destroy(gameObject);
            }

            if (unitType == GuardianID.FidhainTower)
            {
                _GM.guardianList.Remove(parent);
                Destroy(parent);
            }
        }
    }
    private void Setup()
    {
        if(unitType == GuardianID.Tower)
        {
            fireRate = 2;
            if (_DATA.HasPerk(PerkID.Tower))
            {
                maxHealth = _GAME.GetPercentageIncrease(_GAME.towerHealth, 0.5f);
            }
            else
            {
                maxHealth = _GAME.towerHealth;
            }
        }
        if(unitType == GuardianID.FidhainTower)
        {
            fireRate = 4;
            if (_DATA.HasPerk(PerkID.Tower))
            {
                maxHealth = _GAME.GetPercentageIncrease(_GAME.spitTowerHealth, 0.5f);
            }
            else
            {
                maxHealth = _GAME.spitTowerHealth;
            }
        }

        health = maxHealth;
    }
    float CalculateHealth()
    {
        return health / maxHealth;
    }
    private void OnTowerUpgrade()
    {
        towerUpgrade = true;
        Setup();
    }
    public void OnContinueButton()
    {
        health = maxHealth;
        slider.value = slider.value = CalculateHealth();
    }
    private void OnEnable()
    {
        GameEvents.OnTowerUpgrade += OnTowerUpgrade;
        GameEvents.OnContinueButton += OnContinueButton;
    }

    private void OnDisable()
    {
        GameEvents.OnTowerUpgrade -= OnTowerUpgrade;
        GameEvents.OnContinueButton -= OnContinueButton;
    }

    public void Spit()
    {
        audioSource.clip = _SM.GetSpitSounds();
        audioSource.Play();
        if (_DATA.HasPerk(PerkID.Tower))
        {
            GameObject go = Instantiate(projectile2, firingPoint.transform.position, firingPoint.transform.rotation);
            go.GetComponent<Rigidbody>().AddForce(firingPoint.transform.forward * projectileSpeed);
        }
        else
        {
            GameObject go = Instantiate(projectile1, firingPoint.transform.position, firingPoint.transform.rotation);
            go.GetComponent<Rigidbody>().AddForce(firingPoint.transform.forward * projectileSpeed);
        }
    }
}
