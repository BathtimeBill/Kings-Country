using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : GameBehaviour
{
    [Header ("Projectiles")]
    public GameObject projectilePrefab;
    public GameObject arrowSpawnPoint;
    public float projectileSpeed;
    [Header("Numbers")]
    public float distanceFromWildlife;
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip audioClip;

    //This function spawns an arrow object from the left hand of the hunter enemy. It's triggered from the attack animation.
    public void SpawnArrow()
    {
        //checks if there are any animals in the scene then calculates the distance from the hunter enemy to that animal.
        if(gameObject.GetComponentInParent<Hunter>().wildlife.Length != 0)
        {
            distanceFromWildlife = Vector3.Distance(gameObject.GetComponentInParent<Hunter>().closestWildlife.transform.position, transform.position);
        }
        
        Transform closestTarget;
        //Checks weather the hunter is shooting at an animal or a player unit and then orients the arrow towards that result.
        if(gameObject.GetComponentInParent<Hunter>().distanceFromClosestWildlife < gameObject.GetComponentInParent<Hunter>().distanceFromClosestUnit)
        {
            closestTarget = gameObject.GetComponentInParent<Hunter>().closestWildlife;
        }
        else
        {
            closestTarget = gameObject.GetComponentInParent<Hunter>().closestUnit;
        }


        //Adjusts the speed of the projectile based on its distance from the target.
        if (distanceFromWildlife >= 18)
        {
            projectileSpeed = 2000;
        }
        if (distanceFromWildlife < 18 && distanceFromWildlife <= 14)
        {
            projectileSpeed = 1000;
        }
        if (distanceFromWildlife < 14 && distanceFromWildlife <= 10)
        {
            projectileSpeed = 800;
        }
        if (distanceFromWildlife < 10 && distanceFromWildlife <= 5)
        {
            projectileSpeed = 300;
        }
        if (distanceFromWildlife < 5)
        {
            projectileSpeed = 150;
        }
        GameObject projectileInstance;
        projectileInstance = Instantiate(projectilePrefab, arrowSpawnPoint.transform.position, Quaternion.Euler(90, 0, transform.parent.rotation.z));
        projectileInstance.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
        projectileInstance.transform.LookAt(closestTarget);
        Destroy(projectileInstance, 2);
        PlayBowReleaseSound();
    }


    //Plays sounds and is triggered from animations.
    public void PlayBowDrawSound()
    {
        audioClip = _SM.bowDrawSound;
        audioSource.clip = audioClip;
        audioSource.Play();
    }
    public void PlayBowReleaseSound()
    {
        audioClip = _SM.bowReleaseSound;
        audioSource.clip = audioClip;
        audioSource.Play();
    }


    public void Footstep()
    {
        gameObject.GetComponentInParent<Hunter>().PlayFootstepSound();
    }
}
