namespace Assets.Scripts.GameCharacter.AI.States_Machine
{
    class Defender : State
    {
        public Defender(IAICharacter targetPlayer)
            : base(targetPlayer)
        {
        }

        public override void Update()
        {
            targetPlayer.SetMagnetToPush();
            targetPlayer.MoveToDestination(AIPlayerBehaviour.Destination.CLOSEST_NON_CARRIER);
        }

    }
}