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
    [Header("Animator")]
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();    
    }

    //This function spawns an arrow object from the left hand of the hunter enemy. It's triggered from the attack animation.
    public void SpawnArrow()
    {
        if (!_inGame)
            return;

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

        GameObject projectileInstance;
        projectileInstance = Instantiate(projectilePrefab, arrowSpawnPoint.transform.position, transform.rotation);
        projectileInstance.GetComponent<EnemyProjectile>().target = closestTarget.gameObject;
        Destroy(projectileInstance, 1);
        PlayBowReleaseSound();
    }
    public void CheckForEnemies()
    {
        if (_HUTM.enemies.Contains(transform.parent.gameObject))
        {
            //Debug.Log(name + " is in the enemy list");
            if (_HUTM.units.Count == 0)
                animator.SetBool("allWildlifeDead", true);
        }
        else
        {
            //Debug.Log(name + " is NOT the enemy list");
        }
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
