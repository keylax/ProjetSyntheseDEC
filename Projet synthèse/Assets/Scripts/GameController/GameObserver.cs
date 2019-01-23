using UnityEngine;
using System.Linq;
using Assets.Scripts.GameCharacter.AI;
using Assets.Scripts.GameObjectsBehavior;
using Assets.Scripts.Observer;

namespace Assets.Scripts.GameController
{
    public class GameObserver : Observer.Observer
    {
        private bool goalHappened;

        public void Notify(ISubject _subject, Subject.NotifyReason _notifyReason, GameObject _player)
        {
            if (IsPlayerNull(_player))
            {
                switch (_notifyReason)
                {
                    case (Subject.NotifyReason.RED_GOAL):
                        if (CurrentGame.lastBallPossessor != null)
                        {
                            AddGoal(AIPlayerBehaviour.Team.RED);

                            if (CurrentGame.goals.Last().assist != null && CurrentGame.goals.Last().assist.tag == ObjectTags.Player1Tag)
                            {
                                CurrentGame.Statistics.AddAssist();
                            }

                            if (CurrentGame.goals.Last().scorer.tag == ObjectTags.Player1Tag)
                            {
                                CurrentGame.Statistics.AddShotToGoal();
                            }

                            if (CurrentGame.goals.Last().scorer.tag == ObjectTags.Player1Tag)
                            {
                                CheckAchievements(_notifyReason);
                            }
                            goalHappened = true;
                        }
                        break;

                    case (Subject.NotifyReason.BLUE_GOAL):
                        if (CurrentGame.lastBallPossessor != null)
                        {
                            AddGoal(AIPlayerBehaviour.Team.BLUE);
                            goalHappened = true;
                        }
                        break;


                    case (Subject.NotifyReason.GAME_OVER):
                        if (ConnectedUser.connectedUser != null)
                        {
                            if (CurrentGame.winningTeam == AIPlayerBehaviour.Team.RED)
                            {
                                ConnectedUser.connectedUser.totalNumberOfWins++;
                                if (CurrentGame.gameType == CurrentGame.GameType.QUICKMATCH)
                                {
                                    ConnectedUser.connectedUser.numberOfQuickMatchWonInARow++;
                                }
                            }
                            else
                            {
                                ConnectedUser.connectedUser.totalNumberOfLoses++;
                                if (CurrentGame.gameType == CurrentGame.GameType.QUICKMATCH)
                                {
                                    ConnectedUser.connectedUser.numberOfQuickMatchWonInARow--;
                                }
                            }

                            ConnectedUser.connectedUser.totalNumberOfGamePlayed++;
                            CheckAchievements(_notifyReason);
                        }
                        break;

                }

            }
            else
            {
                switch (_notifyReason)
                {
                    case (Subject.NotifyReason.BALL_INTERCEPTED):
                        if (_player.tag == ObjectTags.Player1Tag)
                        {
                            CurrentGame.Statistics.AddInterception();
                        }
                        break;


                    case (Subject.NotifyReason.FELL_IN_PIT):
                        if (_player.tag == ObjectTags.Player1Tag)
                        {
                            CurrentGame.Statistics.AddDeath();
                        }
                        break;

                    case (Subject.NotifyReason.MISSILE_USED):
                        if (_player.tag == ObjectTags.Player1Tag)
                        {
                            CurrentGame.Statistics.AddMissileShot();
                        }
                        break;

                    case (Subject.NotifyReason.LIGHTNINGBOLT_USED):
                        if (_player.tag == ObjectTags.Player1Tag)
                        {
                            CurrentGame.Statistics.AddLightningBoltUsed();
                        }
                        break;

                    case (Subject.NotifyReason.SPRING_USED):
                        if (_player.tag == ObjectTags.Player1Tag)
                        {
                            CurrentGame.Statistics.AddJumpFromSpring();
                        }
                        break;

                    case (Subject.NotifyReason.SHOT_TO_GOAL):
                        if (_player.tag == ObjectTags.Player1Tag)
                        {
                            CurrentGame.Statistics.AddShotToGoal();
                        }
                        break;

                    case (Subject.NotifyReason.HIT_BY_HOMINGMISSILE):
                        if (_player.tag == ObjectTags.Player1Tag)
                        {
                            CurrentGame.Statistics.SetLastHitByBonus(Time.timeSinceLevelLoad);
                        }
                        break;

                    case (Subject.NotifyReason.HIT_BY_LIGHTNINGBOLT):
                        if (_player.tag == ObjectTags.Player1Tag)
                        {
                            CurrentGame.Statistics.SetLastHitByBonus(Time.timeSinceLevelLoad);
                        }
                        break;

                    case (Subject.NotifyReason.HIT_BY_MISSILE):
                        if (_player.tag == ObjectTags.Player1Tag)
                        {
                            CurrentGame.Statistics.SetLastHitByBonus(Time.timeSinceLevelLoad);
                        }
                        break;

                    case (Subject.NotifyReason.HIT_BY_MRFREEZE):
                        if (_player.tag == ObjectTags.Player1Tag)
                        {
                            CurrentGame.Statistics.SetLastHitByBonus(Time.timeSinceLevelLoad);
                        }
                        break;

                    case (Subject.NotifyReason.HIT_BY_POLARITYREVERSER):
                        if (_player.tag == ObjectTags.Player1Tag)
                        {
                            CurrentGame.Statistics.SetLastHitByBonus(Time.timeSinceLevelLoad);
                        }
                        break;

                    case (Subject.NotifyReason.BONUS_ACQUIRED):
                        if (_player.tag == ObjectTags.Player1Tag)
                        {
                            CurrentGame.Statistics.AddBonusAcquired();
                        }
                        break;
                }

                if (_player.tag == ObjectTags.Player1Tag)
                {
                    CheckAchievements(_notifyReason);
                }
            }


            if (goalHappened)
            {
                ChangeGameState();
                goalHappened = false;
            }
        }

        private void CheckAchievements(Subject.NotifyReason _notifyReason)
        {
            if (ConnectedUser.connectedUser != null)
            {
                GamestatesController.gameController.GetComponent<AchievementsController>().CheckAchievementProgression(_notifyReason);
            }
        }

        private void AddGoal(AIPlayerBehaviour.Team _team)
        {
            GameGoal newGoal = new GameGoal(CurrentGame.lastBallPossessor, CurrentGame.secondLastBallPossessor, _team, CurrentGame.lastBallPossessor.transform.position, Time.timeSinceLevelLoad);
            CurrentGame.goals.Add(newGoal);
            CurrentGame.lastBallPossessor = null;
            CurrentGame.secondLastBallPossessor = null;
        }

        private void ChangeGameState()
        {
            if (GamestatesController.IsOvertime())
            {
                GamestatesController.ChangeGameState(GamestatesController.GameStates.GAMEOVER);
                Notify(null, Subject.NotifyReason.GAME_OVER, null);
            }
            else if (!GamestatesController.IsGameover())
            {
                GamestatesController.ChangeGameState(GamestatesController.GameStates.FACEOFF);
            }
        }

        private bool IsPlayerNull(GameObject _player)
        {
            return _player == null;
        }
    }
}