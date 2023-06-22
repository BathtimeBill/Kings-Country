using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrow : GameBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
