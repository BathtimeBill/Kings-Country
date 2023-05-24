using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectile : GameBehaviour

{
    public GameObject particleEffect;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit Something");
        if(collision.collider.tag == "Ground")
        {
            Instantiate(particleEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }

    }
}
