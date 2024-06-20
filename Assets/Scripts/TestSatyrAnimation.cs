using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class TestSatyrAnimation : GameBehaviour
{
    Animator animator;
    NavMeshAgent navAgent;
    public float currentSpeed;
    public Vector3 oldPosition;
    public float tickInterval;
    public float attackSpeed;
    public Transform closestUnit;
    public float distanceFromClosestUnit;
    public bool isCloseToEnemy;
    public float fadeOut;
    void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponentInParent<NavMeshAgent>();
        StartCoroutine(Tick());
    }

    private void Update()
    {
        animator.SetFloat("z", Mathf.Clamp(currentSpeed, 0, 3) );
        if(GetComponentInParent<Unit>().unitID == CreatureID.Goblin)
        {
            if (distanceFromClosestUnit < 80 && _EM.enemies.Count != 0)
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
    IEnumerator Tick()
    {
        currentSpeed = Vector3.Distance(oldPosition, transform.position) * 100 * Time.deltaTime;
        oldPosition = transform.position;
        if(currentSpeed == 0)
        {
            GetComponentInParent<Unit>().isMoving = false;
            GetComponentInParent<Unit>().isMovingCheck = false;
        }
        else
        {
            GetComponentInParent<Unit>().isMoving = true;
        }
        if (_EM.enemies.Count > 0)
        {
            closestUnit = GetClosestEnemy();
            distanceFromClosestUnit = Vector3.Distance(closestUnit.transform.position, transform.position);
            if(GetComponentInParent<Unit>().unitID != CreatureID.Goblin)
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
                        CheckAttackGoblin();
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
        //else
        //{
        //    animator.SetLayerWeight(1, 0);
        //}

        yield return new WaitForSeconds(tickInterval);

        StartCoroutine(Tick());
    }

    //void FadeOutArmsLayer()
    //{
    //    float fadeOut = 1;
    //    while (fadeOut > 0) 
    //    {
    //        fadeOut -= 1 * Time.deltaTime;
    //    }

    //    animator.SetLayerWeight(1, fadeOut);
    //}
    IEnumerator FadeOutArms()
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
    IEnumerator AttackLoop()
    {
        RandomAnimation();
        yield return new WaitForSeconds(attackSpeed);
        if(distanceFromClosestUnit <= 15)
        {
            StartCoroutine(AttackLoop());
        }
    }
    public void RandomAnimation()
    {
        animator.SetTrigger("Attack" + GetRandomAnimation());

    }
    public void CheckAttack()
    {
        if (_EM.enemies.Count != 0 && distanceFromClosestUnit <= 15)
        {
            animator.SetTrigger("Attack" + GetRandomAnimation());
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
    }

    public void CheckAttackGoblin()
    {
        if (_EM.enemies.Count != 0 && distanceFromClosestUnit <= 80)
        {
            animator.SetTrigger("Attack1");
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
    }

    int GetRandomAnimation()
    {
        int i = Random.Range(1, 4);
        return i;
    }

    public Transform GetClosestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        Transform trans = null;

        foreach (GameObject go in _EM.enemies)
        {
            float currentDistance;
            currentDistance = Vector3.Distance(transform.position, go.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                trans = go.transform;
            }
        }

        if (UnitSelection.Instance.unitList == null)
        {
            return null;
        }
        else
            return trans;
    }
}
