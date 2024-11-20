public class Skessa : Unit
{
    public override void Attack(int _attack)
    {
        base.Attack(_attack);
        if (_EM.allEnemiesDead)
            return;
        
        //_attack = 0 : Right Hand Attack, 1 : Left Hand Attack
        if (_attack == 1)
        {
            PlaySound(unitData.attackSounds);
            ParticlesX.PlayParticles(attackParticles, rightFoot.transform.position);
        }
        else
        {
            PlaySound(unitData.attackSounds);
            ParticlesX.PlayParticles(attackParticles, leftHand.transform.position);
        }
    }
}
