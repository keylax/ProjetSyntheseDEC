namespace Assets.Scripts.GameCharacter.AI.States_Machine
{
    class Attacker : State
    {
        public Attacker(IAICharacter _targetPlayer)
            : base(_targetPlayer)
        { }

        public override void Update()
        {
            if (targetPlayer.IsWithinShootingDistanceOfGoal())
            {
                targetPlayer.TurnTowards(AIPlayerBehaviour.Destination.GOAL);
                targetPlayer.SetMagnetToPush();
                targetPlayer.MoveToDestination(AIPlayerBehaviour.Destination.SELF);
            }
            else
            {
                targetPlayer.SetMagnetToPull();
                targetPlayer.MoveToDestination(AIPlayerBehaviour.Destination.CLOSEST_OFFENSIVE_STRATEGIC_POINT);
            }
        }

    }
}