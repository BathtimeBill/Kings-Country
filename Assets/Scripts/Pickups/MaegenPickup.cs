using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaegenPickup : GameBehaviour
{
    public GameObject maegenWisp;


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Unit" || other.tag == "LeshyUnit")
    //    {
    //        Instantiate(maegenParticle, transform.position, transform.rotation);
    //        Destroy(gameObject);
    //    }
    //}

    private void OnContinueButton()
    {
        Instantiate(maegenWisp, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        GameEvents.OnContinueButton += OnContinueButton;
    }

    private void OnDisable()
    {
        GameEvents.OnContinueButton -= OnContinueButton;
    }
}
