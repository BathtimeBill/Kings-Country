using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoggerAxe : MonoBehaviour
{
    public Collider axeCollider;
    public AudioSource audioSource;
    public void EnableCollider()
    {
        axeCollider.enabled = true;
    }
    public void DisableCollider()
    {
        axeCollider.enabled = false;
    }

    public void Footstep()
    {
        gameObject.GetComponentInParent<Logger>().PlayFootstepSound();
    }
    public void WarriorFootstep()
    {
        gameObject.GetComponentInParent<Warrior>().PlayFootstepSound();
    }
    public void KnightFootstep()
    {
        gameObject.GetComponentInParent<Warrior>().PlayKnightFootstepSound();
    }

    private void Update()
    {
        if(gameObject.GetComponentInParent<Logger>().woodcutterType == WoodcutterType.LogCutter)
        {
            if(gameObject.GetComponentInParent<Logger>().hasStopped)
            {
                audioSource.volume = 1;
            }
            else
            {
                audioSource.volume = 0;
            }
        }
    }
}
