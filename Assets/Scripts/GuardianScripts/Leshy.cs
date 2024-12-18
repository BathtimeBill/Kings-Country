public class Leshy : Guardian
{
    public override void Attack(int _attack)
    {
        base.Attack(_attack);
        if (_NoEnemies)
            return;
        
        //_attack = 0 : Hand Attack, 1 : Foot Stomp
        if (_attack == 1)
        {
            PlaySound(guardianData.attackSounds);
            ParticlesX.PlayParticles(attackParticles, rightFoot.transform.position);
        }
        else
        {
            PlaySound(guardianData.attackSounds);
            ParticlesX.PlayParticles(attackParticles, leftHand.transform.position);
        }
    }
}
