using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMechanics : GameBehaviour
{
    Animator animator;
    public Animator animator2;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
            animator2.SetTrigger("Attack");
        }
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) 
        {
            animator2.SetBool("IsMoving", true);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator2.SetBool("IsSprinting", true);
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            animator2.SetBool("IsMoving", false);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator2.SetBool("IsSprinting", false);
        }
    }

}
