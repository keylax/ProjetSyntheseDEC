namespace Assets.Scripts.GameCharacter.AI.States_Machine
{
    public interface IAICharacter : IGameCharacter
    {
        float GetThrowRange();

        void MoveToDestination(AIPlayerBehaviour.Destination _destination);

        IGameCharacter FindBallPossessor();

        bool IsBallCarrierOnSameTeam();

        bool IsBallCloseToOpponentGoal();

        bool IsBallCloseToOwnGoal();

        bool IsBallCloserToOwnGoal();

        IGameCharacter FindClosestTeammateToGoalIgnoringTooNearTeammates();

        bool IsClosestTeamMemberOfBall();

        bool IsBallPossessedByAnother();

        float GetBallDistanceFromGround();

        bool CanThrowAt(AIPlayerBehaviour.Destination _destination);

        void SetPathFindginEnabled(bool _newEnabledState);

        void UseBonus();

        void SetMagnetToPull();

        void SetMagnetToPush();

        void TurnTowards(AIPlayerBehaviour.Destination _locationToTurnTowards);

        bool IsWithinShootingDistanceOfGoal();

    }
}