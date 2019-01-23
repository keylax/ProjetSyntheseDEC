using System.Collections.Generic;
using Assets.Scripts.GameCharacter;
using UnityEngine;
using Assets.Scripts.GameCharacter.AI;
using Assets.Scripts.AchievementModule;
using Assets.Scripts.GameCharacter.HumanPlayer;
using Assets.Scripts.GameObjectsBehavior;
using Assets.Scripts.GameStatisticsModule;
using Assets.Scripts.Observer;
using Assets.Scripts.OfflineQueryStack;
using UnityEngine.UI;

namespace Assets.Scripts.GameController
{
    public class GamestatesController : SubjectMonobehaviour
    {
        public enum GameStates
        {
            IN_MENUS,
            GAMEPLAY,
            FACEOFF,
            GAMEOVER,
            OVERTIME
        };

        [Header("These parameters are mostly set by the controller in StartGame.cs")]
        public static GameObject gameController;
        public static GameStates currentState;
        public static GameObject playerList;
        public static GameObject redGoal;
        public static GameObject blueGoal;
        public static GameObject ball;
        public static GameObject scoreBoard;

        private static bool wasStartPressedDuringGameover;
        private BallBehaviour ballBehaviour;
        private static float faceoffCountdown = 4f;

        private Dictionary<string, int> gameStats = new Dictionary<string, int>();
        private AchievementModule.AchievementModule achievementModule = new OnlineAchievement();
        private GameStatisticsModule.GameStatisticsModule gameStatisticsModule = new OnlineGameStatistics();
        private OfflineStack offlineStack = new OfflineStack();

        private void Start()
        {
            gameController = gameObject;
            ballBehaviour = ball.transform.GetComponent<BallBehaviour>();
            if (ConnectedUser.connectedUser != null && ConnectedUser.connectedUser.isOnline)
            {
                AddObserver(CurrentGame.currentGameObserver);
            }
        }

        private void Update()
        {
            if (IsGameplay())
            {
                GameplayUpdate();
            }
            else if (IsFaceoff())
            {
                if (faceoffCountdown < 0)
                {
                    ChangeGameState(GameStates.GAMEPLAY);
                }
                else
                {
                    FaceOffUpdate();
                }
            }
            else if (IsOvertime())
            {
                OvertimeUpdate();
            }
            else if (IsGameover())
            {
                GameoverUpdate();
            }

        }

        private void UpdateLastPossessor()
        {
            if (ballBehaviour.IsPossessed)
            {
                if (CurrentGame.lastBallPossessor == null)
                {
                    CurrentGame.lastBallPossessor = ballBehaviour.possessor;
                }
                else if (CurrentGame.lastBallPossessor != ballBehaviour.possessor)
                {
                    CurrentGame.secondLastBallPossessor = CurrentGame.lastBallPossessor;

                    CurrentGame.lastBallPossessor = ballBehaviour.possessor;

                    if (ballBehaviour.possessor.GetComponent<IGameCharacter>().GetTeam() != CurrentGame.lastBallPossessor.GetComponent<IGameCharacter>().GetTeam())
                    {
                        NotifyAllObservers(Subject.NotifyReason.BALL_INTERCEPTED, ballBehaviour.possessor);
                    }
                }
                else if (CurrentGame.lastBallPossessor.tag == ObjectTags.Player1Tag)
                {
                    CurrentGame.Statistics.AddTimeToPossessionTime(Time.deltaTime);
                    CurrentGame.lastBallPossessor = ballBehaviour.possessor;
                }
            }
        }

        public static void ChangeGameState(GameStates _newState)
        {
            switch (_newState)
            {
                case GameStates.FACEOFF:
                    InitializeFaceoff();
                    break;

                case GameStates.GAMEPLAY:
                    InitializeGameplay();
                    break;

                case GameStates.OVERTIME:
                    InitializeOvertime();
                    break;

                case GameStates.GAMEOVER:
                    InitializeGameover();
                    break;
            }
        }

        private void MakePlayersStareAtBall()
        {
            for (int i = 0; i < playerList.transform.childCount; i++)
            {
                playerList.transform.GetChild(i)
                    .transform.LookAt(new Vector3(ball.transform.position.x,
                        playerList.transform.GetChild(i).transform.position.y, ball.transform.position.z));
            }
        }

        private void FaceOffUpdate()
        {
            faceoffCountdown -= Time.deltaTime;
            int countdownInSeconds = (int)faceoffCountdown;

            if (countdownInSeconds != 0)
            {
                scoreBoard.transform.GetChild(7).GetComponent<Text>().text = countdownInSeconds.ToString();
            }
            else
            {
                scoreBoard.transform.GetChild(7).GetComponent<Text>().text = "GO!";
            }

            MakePlayersStareAtBall();
        }

        private static void InitializeFaceoff()
        {
            currentState = GameStates.FACEOFF;
            SoundManager.PlaySFX("Smash321");

            for (int i = 0; i < playerList.transform.childCount; i++)
            {
                if (playerList.transform.GetChild(i).tag.Substring(0, 6) == "Player")
                {
                    playerList.transform.GetChild(i).GetComponent<PlayerMovement>().Spawn();
                }
                else if (playerList.transform.GetChild(i).tag == ObjectTags.AITag)
                {
                    playerList.transform.GetChild(i).GetComponent<AIPlayerBehaviour>().Spawn();
                    playerList.transform.GetChild(i).GetComponent<AIPlayerBehaviour>().enabled = false;
                    playerList.transform.GetChild(i).GetComponent<NavMeshAgent>().enabled = false;
                    playerList.transform.GetChild(i).GetComponent<Collider>().enabled = true;
                }
            }
            ball.transform.GetComponent<BallBehaviour>().Spawn();
            faceoffCountdown = 4f;
            scoreBoard.transform.GetChild(7).gameObject.SetActive(true);
        }

        private static void InitializeGameplay()
        {
            currentState = GameStates.GAMEPLAY;

            for (int i = 0; i < playerList.transform.childCount; i++)
            {
                if (playerList.transform.GetChild(i).tag == ObjectTags.AITag)
                {
                    playerList.transform.GetChild(i).GetComponent<AIPlayerBehaviour>().enabled = true;
                    playerList.transform.GetChild(i).GetComponent<NavMeshAgent>().enabled = true;
                }
            }
            scoreBoard.transform.GetChild(7).gameObject.SetActive(false);
        }

        private static void InitializeOvertime()
        {
            currentState = GameStates.OVERTIME;

            scoreBoard.transform.GetChild(6).GetComponent<Text>().text = "Overtime";
            scoreBoard.transform.GetChild(6).position = new Vector3(Screen.width / 1.85f, scoreBoard.transform.GetChild(6).position.y, scoreBoard.transform.GetChild(6).position.z);
        }

        private static void InitializeGameover()
        {
            wasStartPressedDuringGameover = false;
            Time.timeScale = 0;
            for (int i = 0; i < 4; i++)
            {
                scoreBoard.transform.GetChild(i).gameObject.SetActive(false);
            }

            if (CurrentGame.GetRedScore() > CurrentGame.GetBlueScore())
            {
                scoreBoard.transform.GetChild(4).gameObject.SetActive(true);
                CurrentGame.winningTeam = AIPlayerBehaviour.Team.RED;
            }
            else if (CurrentGame.GetBlueScore() > CurrentGame.GetRedScore())
            {
                scoreBoard.transform.GetChild(5).gameObject.SetActive(true);
                CurrentGame.winningTeam = AIPlayerBehaviour.Team.BLUE;
            }


            currentState = GameStates.GAMEOVER;
        }

        private void GameplayUpdate()
        {
            UpdateLastPossessor();
        }

        private void GameoverUpdate()
        {
            if (IsStartPressed() && !wasStartPressedDuringGameover)
            {
                RecordStats();
                wasStartPressedDuringGameover = true;
                CurrentGame.ResetGame();
                currentState = GameStates.IN_MENUS;
                Application.LoadLevel("GameMenu");
            }
        }

        private void PrepareStatsDictionary()
        {
            if (ConnectedUser.connectedUser.isOnline)
            {
                gameStats = gameStatisticsModule.GetOverallStats(ConnectedUser.connectedUser.id);
            }
            CurrentGame.AddStatsToDictionary(ref gameStats);
        }

        private void RecordStats()
        {
            if (ConnectedUser.connectedUser != null)
            {
                Achievement.SaveAchievementProgressToFile();
                ConnectedUser.connectedUser.AddCurrentGameStatsToTotalStats();
                ConnectedUser.connectedUser.SaveStats();
                PrepareStatsDictionary();
                UpdateStats(ConnectedUser.connectedUser.isOnline);
            }

            
        }

        private void UpdateStats(bool _isOnline)
        {
            if (_isOnline)
            {
                foreach (Achievement achievement in ConnectedUser.connectedUser.UserAchievementsList)
                {
                    achievementModule.UpdateAchievement(achievement.id, ConnectedUser.connectedUser.id, achievement.progress);
                }

                gameStatisticsModule.UpdateGame(CurrentGame.winningTeam.ToString(), GameParameters.GetArenaIdByName(GameParameters.ChosenArena), ConnectionModule.ConnectionModule.HttpResquest.POST);
                gameStatisticsModule.UpdateGameStats(gameStats, ConnectedUser.connectedUser.id);
                gameStatisticsModule.UpdateUserStats(ConnectedUser.connectedUser.totalNumberOfWins, ConnectedUser.connectedUser.totalNumberOfLoses, ConnectedUser.connectedUser.name.Replace("\"", ""));
            }
            else
            {
                foreach (Achievement achievement in ConnectedUser.connectedUser.UserAchievementsList)
                {
                    offlineStack.StackAchivements(achievement.id, ConnectedUser.connectedUser.id, achievement.progress);
                }

                offlineStack.StackGame(CurrentGame.winningTeam.ToString(), GameParameters.GetArenaIdByName(GameParameters.ChosenArena).ToString());
                offlineStack.StackGameStats(gameStats, ConnectedUser.connectedUser.id);
                offlineStack.StackUserStats(ConnectedUser.connectedUser.totalNumberOfWins, ConnectedUser.connectedUser.totalNumberOfLoses);
            }
        }

        private void OvertimeUpdate()
        {
            scoreBoard.transform.GetChild(6).GetComponent<Text>().text = "OVERTIME";
            UpdateLastPossessor();
        }

        public static bool IsOvertime()
        {
            return currentState == GameStates.OVERTIME;
        }

        public static bool IsGameover()
        {
            return currentState == GameStates.GAMEOVER;
        }

        public static bool IsFaceoff()
        {
            return currentState == GameStates.FACEOFF;
        }

        public static bool IsGameplay()
        {
            return currentState == GameStates.GAMEPLAY;
        }

        private bool IsStartPressed()
        {
            if (InputManager.GetPlayerStart(ObjectTags.Player1Tag))
            {
                return true;
            }
            else if (InputManager.GetPlayerStart(ObjectTags.Player1Tag))
            {
                return true;
            }
            else if (InputManager.GetPlayerStart(ObjectTags.Player1Tag))
            {
                return true;
            }
            else if (InputManager.GetPlayerStart(ObjectTags.Player1Tag))
            {
                return true;
            }
            return false;
        }

    }
}