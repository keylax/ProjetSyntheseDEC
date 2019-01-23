using Assets.Scripts.GameCharacter.AI;
using UnityEngine;

namespace Assets.Scripts.GameController
{
    public class GameGoal
    {
        public GameObject scorer { get; set; }
        public GameObject assist { get; set; }
        public float goalTimeStamp { get; set; }
        public Vector3 scorerPosition;
        public AIPlayerBehaviour.Team team { get; set; }

        public GameGoal(GameObject _scorer, GameObject _assist, AIPlayerBehaviour.Team _team, Vector3 _scorerPosition, float _goalTimeStamp)
        {
            scorer = _scorer;
            assist = _assist;
            goalTimeStamp = _goalTimeStamp;
            scorerPosition = _scorerPosition;
            team = _team;
        }
    }
}
