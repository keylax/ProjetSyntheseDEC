using UnityEngine;
using System.Collections;
using Assets.Scripts.Observer;
using UnityEngine.UI;

namespace Assets.Scripts.GameController
{
    public class TimerCountdown : SubjectMonobehaviour
    {

        public float startingTimeInSeconds = 900;

        public float timeInSeconds;

        private void Start()
        {
            AddObserver(CurrentGame.currentGameObserver);
            timeInSeconds = startingTimeInSeconds;
        }


        private void Update()
        {
            if (timeInSeconds < 0)
            {
                if (CurrentGame.GetRedScore() == CurrentGame.GetBlueScore())
                {
                    if (!GamestatesController.IsOvertime())
                    {
                        GamestatesController.ChangeGameState(GamestatesController.GameStates.OVERTIME);
                    }
                }
                else
                {
                    if (!GamestatesController.IsGameover())
                    {
                        GamestatesController.ChangeGameState(GamestatesController.GameStates.GAMEOVER);
                        NotifyAllObservers(Subject.NotifyReason.GAME_OVER, null);
                        timeInSeconds = startingTimeInSeconds;
                    }
                }
            }
            else
            {
                if (!GamestatesController.IsFaceoff())
                {
                    timeInSeconds -= Time.deltaTime;
                    float minutes = Mathf.RoundToInt(timeInSeconds) / 60;
                    float seconds = Mathf.RoundToInt(timeInSeconds) % 60;

                    if (seconds < 10)
                    {
                        transform.GetComponent<Text>().text = minutes.ToString() + ":0" + seconds.ToString();
                    }
                    else
                    {
                        transform.GetComponent<Text>().text = minutes.ToString() + ":" + seconds.ToString();
                    }
                }
            }
        }
    }
}
