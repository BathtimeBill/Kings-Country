using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : GameBehaviour
{
    public float speed;
    public GameObject target;

    private void FixedUpdate()
    {
        if(target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position + new Vector3(0, 2, 0), speed);
            transform.LookAt(target.transform.position);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
