using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class LeshyAnimation : GameBehaviour
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
    void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponentInParent<NavMeshAgent>();
        StartCoroutine(Tick());
    }

    private void Update()
    {
        animator.SetFloat("z", Mathf.Clamp(currentSpeed, 0, 3));
    }
    IEnumerator Tick()
    {
        currentSpeed = Vector3.Distance(oldPosition, transform.position) * 100 * Time.deltaTime;
        oldPosition = transform.position;
        if (_EM.enemies.Count > 0)
        {
            closestUnit = GetClosestEnemy();
            distanceFromClosestUnit = Vector3.Distance(closestUnit.transform.position, transform.position);
            if (distanceFromClosestUnit <= 15)
            {
                animator.SetLayerWeight(1, 1);
                if (isCloseToEnemy == false)
                {
                    isCloseToEnemy = true;
                    StartCoroutine(AttackLoop());
                }
            }
            else
            {
                isCloseToEnemy = false;
                StopCoroutine(AttackLoop());
                animator.SetLayerWeight(1, 0);
            }
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }

        yield return new WaitForSeconds(tickInterval);

        StartCoroutine(Tick());
    }

    IEnumerator AttackLoop()
    {
        RandomAnimation();
        yield return new WaitForSeconds(attackSpeed);
        StartCoroutine(AttackLoop());
    }
    public void RandomAnimation()
    {
        animator.SetTrigger("Attack" + GetRandomAnimation());
    }

    int GetRandomAnimation()
    {
        int i = Random.Range(1, 3);
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
