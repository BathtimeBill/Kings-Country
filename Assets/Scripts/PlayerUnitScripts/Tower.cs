using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tower : GameBehaviour
{
    public GameObject firingPoint;
    public GameObject projectile1;
    public GameObject projectile2;
    public Vector3 closestEnemy;
    public float projectileSpeed;
    public float distanceToClosestEnemy;
    public Slider slider;

    public float health;
    public float maxHealth;

    public GameObject woodParticle;
    public GameObject explosionParticle;
    public GameObject spawnParticleEffect;
    public MeshRenderer towerRadius;

    public bool towerUpgrade;

    void Start()
    {
        StartCoroutine(ShootProjectile());
        UnitSelection.Instance.unitList.Add(gameObject);
        slider.value = CalculateHealth();
        Setup();
        Instantiate(spawnParticleEffect, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {

        if (_EM.enemies.Length != 0)
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
        if(distanceToClosestEnemy < 50)
        {
            if(_UM.tower)
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

        yield return new WaitForSeconds(2);
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
    }
    public void TakeDamage(float damage)
    {
        Instantiate(woodParticle, transform.position, transform.rotation);
        health -= damage;
        Die();
        slider.value = slider.value = CalculateHealth();
    }
    private void Die()
    {
        if (health <= 0)
        {
            if (_HM.units.Contains(gameObject))
            {
                _HM.units.Remove(gameObject);
            }
            UnitSelection.Instance.DeselectAll();
            UnitSelection.Instance.unitList.Remove(gameObject);
            GameObject go;
            go = Instantiate(explosionParticle, transform.position, transform.rotation);
            Destroy(go, 15);
            _UI.CheckPopulousUI();
            Destroy(gameObject);
        }
    }
    private void Setup()
    {
        if(_UM.tower)
        {
            maxHealth = 300;
        }
        else
        {
            maxHealth = 200;
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
}
