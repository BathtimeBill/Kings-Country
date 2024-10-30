using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Wheel : GameBehaviour
{
    public NavMeshAgent navAgent;
    public Animator animator;
    void Start()
    {
        
    }

    void Update()
    {
        if (navAgent.velocity != Vector3.zero)
        {
            animator.SetBool("hasStopped", false);
        }
        if (navAgent.velocity == Vector3.zero)
        {
            animator.SetBool("hasStopped", true);
        }
    }
}
