using UnityEngine;
using System.Collections;
using DG.Tweening;
public class FidhainTower : Guardian
{
    [Header("Fidhain Tower Specific")]
    public GameObject firingPoint;
    public GameObject[] projectiles;
    public float projectileSpeed = 1000f;
    public float fireRate = 2;
    public Renderer towerRadius;
    public float towerOpacity = 0.4f;

    private int currentProjectile = 0;
    public override void Start()
    {
        base.Start();
        towerRadius.material.DOFade(0, 0);
        StartCoroutine(ShootProjectile());
    }
    
    IEnumerator ShootProjectile()
    {
        yield return new WaitForSeconds(fireRate);
        if (distanceToClosestEnemy < unitData.detectRange)
        {
            animator.SetTrigger("Spit");
            PlaySound(unitData.attackSounds);
            currentProjectile = ArrayX.IncrementCounter(currentProjectile, projectiles);
            projectiles[currentProjectile].transform.position = firingPoint.transform.position;
            projectiles[currentProjectile].GetComponent<Rigidbody>().AddForce(firingPoint.transform.forward * projectileSpeed);
            DisableAfterTime(projectiles[currentProjectile], fireRate);
            if(_DATA.HasPerk(PerkID.Tower))
            {

            }
        }
        StartCoroutine(ShootProjectile());
    }
    
    private void OnMouseOver()
    {
        towerRadius.material.DOFade(towerOpacity, _TWEENING.focusTweenTime);
    }
    private void OnMouseExit()
    {
        towerRadius.material.DOFade(0, _TWEENING.focusTweenTime);
    }
}
