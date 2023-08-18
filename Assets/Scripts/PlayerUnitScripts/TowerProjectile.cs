using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectile : GameBehaviour

{
    public GameObject particleEffect;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Tree" || collision.collider.tag != "Unit" || collision.collider.tag != "Wildlife")
        {
            Instantiate(particleEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
