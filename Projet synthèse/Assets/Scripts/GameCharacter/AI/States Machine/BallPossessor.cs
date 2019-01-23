namespace Assets.Scripts.GameCharacter.AI.States_Machine
{
    class BallPossessor : State
    {
        public BallPossessor(IAICharacter _targetPlayer)
            : base(_targetPlayer)
        { }

        public override void Update()
        {
            if (targetPlayer.CalculateDistanceFromGoal() < targetPlayer.GetThrowRange() && targetPlayer.CanThrowAt(AIPlayerBehaviour.Destination.GOAL))
            {
                targetPlayer.SetMagnetToPush();
                targetPlayer.TurnTowards(AIPlayerBehaviour.Destination.GOAL);
                targetPlayer.MoveToDestination(AIPlayerBehaviour.Destination.BALL);
            }
            else
            {
                IGameCharacter passReceiver = targetPlayer.FindClosestTeammateToGoalIgnoringTooNearTeammates();

                if (passReceiver != null)
                {
                    targetPlayer.SetMagnetToPush();
                    targetPlayer.TurnTowards(AIPlayerBehaviour.Destination.CLOSE_TEAMMATE);
                }
                else
                {
                    targetPlayer.SetMagnetToPull();
                    targetPlayer.MoveToDestination(AIPlayerBehaviour.Destination.GOAL);
                }
            }
        }

    }
}