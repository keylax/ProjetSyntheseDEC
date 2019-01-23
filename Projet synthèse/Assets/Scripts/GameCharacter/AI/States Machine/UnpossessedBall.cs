namespace Assets.Scripts.GameCharacter.AI.States_Machine
{
    class UnpossessedBall : State
    {
        private const float MINIMUM_BALL_DISTANCE_FROM_GROUND_TO_CONSIDER_BALL_IN_THE_AIR = 2.1f;

        public UnpossessedBall(IAICharacter _targetPlayer)
            : base(_targetPlayer)
        { }

        public override void Update()
        {
            targetPlayer.SetMagnetToPull();

            if (targetPlayer.IsClosestTeamMemberOfBall() || targetPlayer.IsBallCloseToOpponentGoal())
            {
                if (targetPlayer.GetBallDistanceFromGround() > MINIMUM_BALL_DISTANCE_FROM_GROUND_TO_CONSIDER_BALL_IN_THE_AIR)
                {
                    targetPlayer.MoveToDestination(AIPlayerBehaviour.Destination.BALL_LANDING_SPOT);
                }
                else
                {
                    targetPlayer.MoveToDestination(AIPlayerBehaviour.Destination.BALL);
                }
            }
            else
            {
                if (targetPlayer.IsBallCloserToOwnGoal())
                {
                    targetPlayer.MoveToDestination(AIPlayerBehaviour.Destination.CLOSEST_DEFENSIVE_STRATEGIC_POINT);
                }
                else
                {
                    targetPlayer.MoveToDestination(AIPlayerBehaviour.Destination.CLOSEST_OFFENSIVE_STRATEGIC_POINT);
                }
            }
        }

    }
}