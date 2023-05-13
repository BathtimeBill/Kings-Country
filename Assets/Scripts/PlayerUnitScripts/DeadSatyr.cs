using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadSatyr : GameBehaviour
{
    public GameObject ragdoll;
    public void SpawnRagdoll()
    {
        Instantiate(ragdoll, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
