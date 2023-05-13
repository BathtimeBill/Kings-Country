using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaegenPickup : GameBehaviour
{
    public GameObject maegenParticle;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Unit" || other.tag == "LeshyUnit")
        {
            Instantiate(maegenParticle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
