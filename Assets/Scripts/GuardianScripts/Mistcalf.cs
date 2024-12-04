public class Mistcalf : Unit
{
    public override void Attack(int _attack)
    {
        base.Attack(_attack);
        if (_NoEnemies)
            return;
        
        //_attack = 0 : Left Stomp, 1 : Right Stomp
        if (_attack == 1)
        {
            PlaySound(unitData.attackSounds);
            ParticlesX.PlayParticles(footstepRightParticles, rightFoot.transform.position);
        }
        else
        {
            PlaySound(unitData.attackSounds);
            ParticlesX.PlayParticles(footstepLeftParticles, leftHand.transform.position);
        }
    }
}
