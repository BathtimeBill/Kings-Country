using UnityEngine;

public class Fidhain : Guardian
{
    [Header("Fidhain Specific")]
    public ParticleSystem spitParticles;
    public Collider spitCollider;
    public GameObject towerUnit;
    public float rangeMultiplier = 10f;
    public override void Attack(int _attack)
    {
        base.Attack(_attack);
        if (_NoEnemies)
            return;
        
        spitParticles.Play();
        spitCollider.enabled = true;
        
        PlaySound(guardianData.attackSounds);
        ParticlesX.PlayParticles(attackParticles, rightHand.transform.position);
    }

    public override void StopAttack(int _attack)
    {
        spitParticles.Stop();
        spitCollider.enabled = false;
    }
    
    public override void HandleAttackState()
    {
        stoppingDistance *= rangeMultiplier;
        base.HandleAttackState();
    }

    public override void HandleMovingState()
    {
        stoppingDistance = guardianData.stoppingDistance;
        base.HandleMovingState();
    }

    protected override void Tower()
    {
        base.Tower();
        Vector3 offset = new Vector3(0, -1.5f, 0);
        Instantiate(towerUnit, transform.position + offset, Quaternion.Euler(0, 0, 0));
    }
}
