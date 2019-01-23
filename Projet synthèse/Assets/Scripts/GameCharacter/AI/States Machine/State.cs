using Assets.Scripts.GameCharacter.HumanPlayer;

namespace Assets.Scripts.GameCharacter.AI.States_Machine
{
    public abstract class State
    {
        protected IAICharacter targetPlayer;

        protected State(IAICharacter _targetPlayer)
        {
            targetPlayer = _targetPlayer;
        }

        public State ChangeToAppropriateState()
        {
            ManageBonusUsage(targetPlayer.GetCurrentBonus());

            if (targetPlayer.IsStunned() && !targetPlayer.HasBall())
            {
                if (!(this is Stunned))
                    return new Stunned(targetPlayer);
            }
            else
            {
                if (targetPlayer.HasBall())
                {
                    if (!(this is BallPossessor))
                        return new BallPossessor(targetPlayer);
                }
                else if (targetPlayer.IsBallPossessedByAnother())
                {
                    if (targetPlayer.IsBallCarrierOnSameTeam())
                    {
                        if (!(this is Attacker))
                            return new Attacker(targetPlayer);
                    }
                    else if (targetPlayer.IsClosestTeamMemberOfBall())
                    {
                        if (!(this is BallCarrierDefender))
                            return new BallCarrierDefender(targetPlayer);
                    }
                    else
                    {
                        if (!(this is Defender))
                            return new Defender(targetPlayer);
                    }
                }
                else
                {
                    if (!(this is UnpossessedBall))
                        return new UnpossessedBall(targetPlayer);
                }
            }

            return this;
        }

        public abstract void Update();

        private void ManageBonusUsage(PickUpBonus.Bonuses _currentBonus)
        {
            switch (_currentBonus)
            {
                case PickUpBonus.Bonuses.SPEED_BOOST:
                case PickUpBonus.Bonuses.STAR:
                    if (targetPlayer.HasBall())
                        targetPlayer.UseBonus(); //Use to protect ball carrier aka self
                    break;

                case PickUpBonus.Bonuses.HOMING_MISSILE:
                case PickUpBonus.Bonuses.MISSILE:
                case PickUpBonus.Bonuses.SPRING:
                    if (!(targetPlayer.HasBall() || targetPlayer.IsBallCarrierOnSameTeam()))
                    {
                        if (targetPlayer.IsBallCloseToOwnGoal())
                            targetPlayer.TurnTowards(AIPlayerBehaviour.Destination.CARRIER);
                            targetPlayer.UseBonus(); //Use if an opponent is close enough with the ball to score
                    }
                    break;

                case PickUpBonus.Bonuses.LIGHTNING_BOLT:
                    if (!(targetPlayer.HasBall() || targetPlayer.IsBallCarrierOnSameTeam()))
                    {
                        if (targetPlayer.IsBallCloseToOwnGoal())
                        targetPlayer.UseBonus(); //Use if an opponent is close enough with the ball to score
                    }
                    break;

                case PickUpBonus.Bonuses.MR_FREEZE:
                case PickUpBonus.Bonuses.POLARITY_REVERSER:
                    if (!(targetPlayer.HasBall() || targetPlayer.IsBallCarrierOnSameTeam()))
                    {
                            targetPlayer.UseBonus(); //Use if an opponent has the ball
                    }
                    break;

                case PickUpBonus.Bonuses.MAGNET_UPGRADE:
                    targetPlayer.UseBonus(); //Just use it...
                    break;

                case PickUpBonus.Bonuses.NONE:
                    break;
            }
        }

    }
}