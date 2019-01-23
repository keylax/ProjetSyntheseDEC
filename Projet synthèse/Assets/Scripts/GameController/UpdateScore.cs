using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.GameCharacter.AI;

namespace Assets.Scripts.GameController
{
    public class UpdateScore : MonoBehaviour
    {
        private int redTeamScore;
        private int blueTeamScore;

        private void Start()
        {
            redTeamScore = 0;
            blueTeamScore = 0;
        }

        public void AddRedPoint()
        {
            redTeamScore++;
            transform.GetChild(3).GetComponent<Text>().text = redTeamScore.ToString();
        }

        public void AddBluePoint()
        {
            blueTeamScore++;
            transform.GetChild(2).GetComponent<Text>().text = blueTeamScore.ToString();
        }

        public int GetScoreDifferential(AIPlayerBehaviour.Team _team)
        {
            if (_team == AIPlayerBehaviour.Team.BLUE)
            {
                return redTeamScore - blueTeamScore;
            }
            else
            {
                return blueTeamScore - redTeamScore;
            }
        }

    }
}