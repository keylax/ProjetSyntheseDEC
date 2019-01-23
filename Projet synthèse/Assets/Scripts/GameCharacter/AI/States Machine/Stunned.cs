namespace Assets.Scripts.GameCharacter.AI.States_Machine
{
    class Stunned : State
    {
        public Stunned(IAICharacter targetPlayer)
            : base(targetPlayer)
        { }

        public override void Update()
        {
            targetPlayer.SetMagnetToPull(); //This does not matter because it won't be enabled...
            targetPlayer.SetPathFindginEnabled(false);
        }

    }
}