using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrow : GameBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }

}
