using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : GameBehaviour
{
    public GameObject firingPoint;
    public GameObject projectile;
    public Vector3 closestEnemy;
    public float projectileSpeed;
    public float distanceToClosestEnemy;

    void Start()
    {
        StartCoroutine(ShootProjectile());
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
            return;

    }

    IEnumerator ShootProjectile()
    {
        if(distanceToClosestEnemy < 50)
        {
            GameObject go = Instantiate(projectile, firingPoint.transform.position, firingPoint.transform.rotation);
            go.GetComponent<Rigidbody>().AddForce(firingPoint.transform.forward * projectileSpeed);
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
}
