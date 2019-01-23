namespace Assets.Scripts.GameCharacter.AI.States_Machine
{
    class BallCarrierDefender : State
    {

        public BallCarrierDefender(IAICharacter _targetPlayer)
            : base(_targetPlayer)
        { }

        public override void Update()
        {
            targetPlayer.SetMagnetToPush();
            targetPlayer.MoveToDestination(AIPlayerBehaviour.Destination.CARRIER);
        }

    }
}