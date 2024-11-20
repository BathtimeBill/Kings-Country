using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class UnitAnimation : GameBehaviour
{
    Animator animator;
    NavMeshAgent navAgent;
    public float currentSpeed;
    public float smoothedSpeed;
    public float smoothSpeedFactor;
    public Vector3 oldPosition;
    public float tickInterval;
    public float attackSpeed;
    public Transform closestUnit;
    public float distanceFromClosestUnit;
    public bool isCloseToEnemy;
    public float fadeOut;

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
        if (unit.unitID == CreatureID.Goblin)
        {
            if (distanceFromClosestUnit < 80 && !_EM.allEnemiesDead)
            {
                if(closestUnit != null)
                {
                    SmoothFocusOnEnemy();
                }
            }
        }
    }


    private void SmoothFocusOnEnemy()
    {
        var targetRotation = Quaternion.LookRotation(closestUnit.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2 * Time.deltaTime);
    }
    //private void FixedUpdate()
    //{
    //    currentSpeed = Vector3.Distance(oldPosition, transform.position) * 100 * Time.deltaTime;
    //}
    private IEnumerator Tick()
    {
        currentSpeed = Vector3.Distance(oldPosition, transform.position) * 100 * Time.deltaTime;
        oldPosition = transform.position;
        if(currentSpeed == 0)
            unit.isMovingCheck = false;

        if (!_EM.allEnemiesDead)
        {
            closestUnit = _UM.unitList == null ? null : ObjectX.GetClosest(gameObject, _EM.enemies).transform;
            distanceFromClosestUnit = Vector3.Distance(closestUnit.transform.position, transform.position);
            if(unit.unitID != CreatureID.Goblin)
            {
                if (distanceFromClosestUnit <= 15)
                {
                    animator.SetLayerWeight(1, 1);
                    if (isCloseToEnemy == false)
                    {
                        isCloseToEnemy = true;
                        fadeOut = 1;
                        //StartCoroutine(AttackLoop());
                        CheckAttack();
                        //StopCoroutine(FadeOutArms());
                    }
                }
                else
                {
                    if (isCloseToEnemy == true)
                    {
                        isCloseToEnemy = false;
                        CheckAttack();
                        //StartCoroutine(FadeOutArms());
                    }
                }
            }
            else
            {
                if (distanceFromClosestUnit <= 80)
                {
                    animator.SetLayerWeight(1, 1);
                    if (isCloseToEnemy == false)
                    {
                        isCloseToEnemy = true;
                        fadeOut = 1;
                        //StartCoroutine(AttackLoop());
                        CheckAttack();
                        //StopCoroutine(FadeOutArms());
                    }
                }
                else
                {
                    if (isCloseToEnemy == true)
                    {
                        isCloseToEnemy = false;
                        //StartCoroutine(FadeOutArms());
                    }
                }
            }

        }
        else
        {
            CheckAttack();
        }

        yield return new WaitForSeconds(tickInterval);

        StartCoroutine(Tick());
    }
    
    private IEnumerator FadeOutArms()
    {
        animator.SetLayerWeight(1, fadeOut);
        yield return new WaitForEndOfFrame();
        if (fadeOut > 0)
        {
            fadeOut -= 0.01f;
            StartCoroutine(FadeOutArms());
            print("fading");
        }
    }
    private IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(attackSpeed);
        CheckAttack();
        StartCoroutine(AttackLoop());
    }
    private void RandomAnimation()
    {
        animator.SetTrigger("Attack" + GetRandomAnimation());

    }
    public void CheckAttack()
    {
        string attackName = unit.unitID == CreatureID.Goblin ? "Attack1" : "Attack" + GetRandomAnimation();
        float dist = unit.unitID == CreatureID.Goblin ? 80 : 15;
        if (!_EM.allEnemiesDead && distanceFromClosestUnit <= dist)
        {
            animator.SetTrigger(attackName);
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
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
