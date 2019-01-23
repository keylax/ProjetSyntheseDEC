using Assets.Scripts.GameCharacter.AI;
using Assets.Scripts.GameCharacter.HumanPlayer;
using UnityEngine;

namespace Assets.Scripts.GameCharacter
{
    public interface IGameCharacter
    {
        AIPlayerBehaviour.Team GetTeam();

        bool HasBall();

        float CalculateDistanceFromBall();

        float CalculateDistanceFromGoal();

        Transform GetAttachedBody();

        bool IsStunned();

        PickUpBonus.Bonuses GetCurrentBonus();

        void FreezeGameCharacter();

        void UnFreezeGameCharacter();

        void StunPlayer(float _durationOfStun);

        Vector3 GetCurrentVelocity();

    }
}