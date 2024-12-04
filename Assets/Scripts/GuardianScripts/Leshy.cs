public class Leshy : Unit
{
    public override void Attack(int _attack)
    {
        base.Attack(_attack);
        if (_NoEnemies)
            return;
        
        //_attack = 0 : Hand Attack, 1 : Foot Stomp
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
