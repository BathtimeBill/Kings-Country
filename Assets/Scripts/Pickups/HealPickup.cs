using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPickup : GameBehaviour
{
    public GameObject healParticle;

    //When a player unit enters the heal pickup's trigger, the heal particle is spawned.
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Unit" || other.tag == "LeshyUnit")
        {
            Instantiate(healParticle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
