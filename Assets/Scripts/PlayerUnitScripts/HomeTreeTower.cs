using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeTreeTower : GameBehaviour
{
    public GameObject firingPoint;
    public GameObject projectile1;
    public Vector3 closestEnemy;
    public float projectileSpeed;
    public float distanceToClosestEnemy;


    public GameObject woodParticle;
    public GameObject explosionParticle;
    public GameObject spawnParticleEffect;

    public bool towerUpgrade;

    void Start()
    {
        StartCoroutine(ShootProjectile());
    }

    // Update is called once per frame
    void Update()
    {

        if (!_EM.allEnemiesDead)
        {
            closestEnemy = GetClosestEnemy().transform.position;
            transform.LookAt(closestEnemy);
            distanceToClosestEnemy = Vector3.Distance(closestEnemy, transform.position);
        }
        else
            distanceToClosestEnemy = 100;

    }

    IEnumerator ShootProjectile()
    {
        if (distanceToClosestEnemy < 65)
        {
            GameObject go = Instantiate(projectile1, transform.position, transform.rotation);
            go.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);

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

    private void OnTowerUpgrade()
    {

    }

    private void OnEnable()
    {
        GameEvents.OnTowerUpgrade += OnTowerUpgrade;
    }

    private void OnDisable()
    {
        GameEvents.OnTowerUpgrade -= OnTowerUpgrade;
    }
}
