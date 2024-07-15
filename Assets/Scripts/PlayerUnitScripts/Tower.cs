using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tower : GameBehaviour
{
    public CreatureID unitType;
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

    public GameObject parent;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        parent = gameObject.transform.parent.gameObject;
        StartCoroutine(ShootProjectile());
        UnitSelection.Instance.unitList.Add(parent);
        slider.value = CalculateHealth();
        Setup();
        Instantiate(spawnParticleEffect, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {

        if (!_EM.allEnemiesDead)
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
            if(_PERK.HasPerk(PerkID.Tower))
            {

                if(unitType == CreatureID.SpitTower)
                {
                    animator.SetTrigger("Spit");
                }
                if (unitType == CreatureID.Tower)
                {
                    GameObject go = Instantiate(projectile2, firingPoint.transform.position, firingPoint.transform.rotation);
                    go.GetComponent<Rigidbody>().AddForce(firingPoint.transform.forward * projectileSpeed);
                }
            }
            else
            {
                if (unitType == CreatureID.SpitTower)
                {
                    animator.SetTrigger("Spit");
                }
                if (unitType == CreatureID.Tower)
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
        if (other.tag == "Arrow")
        {
            TakeDamage(15);
            Destroy(other.gameObject);
        }
        if (other.tag == "Arrow2")
        {
            TakeDamage(35);
            Destroy(other.gameObject);
        }
        if(other.tag == "LordWeapon")
        {
            TakeDamage(_DATA.GetUnit(HumanID.Lord).damage);
        }
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
            if (_HM.units.Contains(parent))
            {
                _HM.units.Remove(parent);
            }

            GameObject go;
            go = Instantiate(explosionParticle, transform.position, transform.rotation);
            Destroy(go, 15);
            _UI.CheckPopulousUI();
            if(unitType == CreatureID.Tower)
            {
                UnitSelection.Instance.unitList.Remove(gameObject);
                Destroy(gameObject);
            }

            if (unitType == CreatureID.SpitTower)
            {
                UnitSelection.Instance.unitList.Remove(parent);
                Destroy(parent);
            }
        }
    }
    private void Setup()
    {
        if(unitType == CreatureID.Tower)
        {
            fireRate = 2;
            if (_PERK.HasPerk(PerkID.Tower))
            {
                maxHealth = _GM.GetPercentageIncrease(_GM.towerHealth, 0.5f);
            }
            else
            {
                maxHealth = _GM.towerHealth;
            }
        }
        if(unitType == CreatureID.SpitTower)
        {
            fireRate = 4;
            if (_PERK.HasPerk(PerkID.Tower))
            {
                maxHealth = _GM.GetPercentageIncrease(_GM.spitTowerHealth, 0.5f);
            }
            else
            {
                maxHealth = _GM.spitTowerHealth;
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
        if (_PERK.HasPerk(PerkID.Tower))
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
