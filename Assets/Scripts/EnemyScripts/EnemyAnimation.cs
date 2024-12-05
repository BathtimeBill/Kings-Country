using UnityEngine;
using System.Collections;

public class EnemyAnimation : GameBehaviour
{
    public Enemy enemy;
    public Collider weaponCollider;
    public bool isAttacking;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAttackAnimation()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(PlayAttackAnimationRoutine());
        }
    }

    IEnumerator PlayAttackAnimationRoutine()
    {
        animator.SetTrigger("Attack");

        //Wait for them to enter the Attacking state
        while (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            yield return null;

        //Now wait for them to finish
        while (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            yield return null;
        
        CheckAttack();
    }
    public void CheckAttack()
    {
        isAttacking = false;
    }

    public void PlayWalkAnimation(float _speed) => animator.SetFloat("Speed", _speed);
    public void PlayImpactAnimation() => animator.SetTrigger("Impact");
    public void PlayIdleAnimation() => animator.SetTrigger("Idle");
    public void PlayVictoryAnimation() => animator.SetTrigger("Cheer" + RandomCheerAnim());
    public int RandomCheerAnim(int _count = 3) => Random.Range(1, _count);

    #region Animation Events
    public void EnableWeaponCollider() => weaponCollider.enabled = true;
    public void DisableWeaponCollider() => weaponCollider.enabled = false;
    public void Attack(int _attack) => enemy.Attack(_attack);
    public void Footstep(string _foot) => enemy.Footstep(_foot);
    public void CheckState() => enemy.CheckState();
    //Note that the draw sound needs to be in the appropriate array element
    public void PlayBowDrawSound() => enemy.PlaySound(enemy.unitData.attackSounds[1]);  
    #endregion
}
