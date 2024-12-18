using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class GuardianAnimation : GameBehaviour
{
    Animator animator;
    NavMeshAgent navAgent;
    public float currentSpeed;
    public float smoothedSpeed;
    public float smoothSpeedFactor;
    public float tickInterval;
    public bool isAttacking;
    private Unit unit;
    void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponentInParent<NavMeshAgent>();
        unit = GetComponentInParent<Unit>();
        StartCoroutine(Tick());
    }

    private void Update()
    {
        smoothedSpeed = Mathf.Lerp(animator.GetFloat("z"), currentSpeed, Time.deltaTime * smoothSpeedFactor);
        animator.SetFloat("z", Mathf.Clamp(smoothedSpeed, 0, 3));
        //CHECK AFTER TESTING
        /*if (unit.unitID == CreatureID.Goblin)
        {
            if (distanceFromClosestUnit < 80 && !_EM.allEnemiesDead)
            {
                if(closestUnit != null)
                {
                    SmoothFocusOnEnemy();
                }
            }
        }*/
    }
    
    private IEnumerator Tick()
    {
        currentSpeed = navAgent.velocity.magnitude;
        if(currentSpeed == 0)
            unit.isMovingCheck = false;
        
        yield return new WaitForSeconds(tickInterval);

        StartCoroutine(Tick());
    }

    public void PlayAttack()
    {
        if (!isAttacking)
        {
            animator.SetLayerWeight(1, 1);
            animator.SetTrigger("Attack" + GetRandomAnimation());
            isAttacking = true;
        }
    }
    public void CheckAttack()
    {
        isAttacking = false;
        animator.SetLayerWeight(1, 0);
    }
    
    private int GetRandomAnimation() => Random.Range(1, 4);

    
    #region Animation Events
    //These all come from Animation Events which is why they don't show as having references
    public void EnableWeaponCollider(int _col)=>unit.attackColliders[_col].enabled = true;
    public void DisableWeaponCollider(int _col)=>unit.attackColliders[_col].enabled = false;
    public void Attack(int _attack) => unit.Attack(_attack);
    public void StopAttack(int _attack) => unit.StopAttack(_attack);
    public void Footstep(string _foot) => unit.Footstep(_foot);
    public void PlayParticle() => unit.PlayParticle();
    #endregion
}
