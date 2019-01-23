using Assets.Scripts.GameCharacter;
using Assets.Scripts.GameCharacter.AI;
using Assets.Scripts.GameController;
using Assets.Scripts.GameObjectsBehavior;
using Assets.Scripts.Observer;
using UnityEngine;

public class BallBlocker : SubjectMonobehaviour
{
    public enum GoalAxis { X, Z }

    public Transform Ball;
    public GoalAxis MapGoalAxis;

    private Transform goalie;

    private float collisionDelayTimer;
    private bool collisionDelayActive;
    private const float GOALIE_SPEED = 5f;

    private readonly Vector3 X_AXIS_LEFT_TRANSLATION = new Vector3(-GOALIE_SPEED, 0, 0);
    private readonly Vector3 X_AXIS_RIGHT_TRANSLATION = new Vector3(GOALIE_SPEED, 0, 0);

    private readonly Vector3 Z_AXIS_LEFT_TRANSLATION = new Vector3(0, 0, -GOALIE_SPEED);
    private readonly Vector3 Z_AXIS_RIGHT_TRANSLATION = new Vector3(0, 0, GOALIE_SPEED);

    void Start()
    {
        AddObserver(CurrentGame.currentGameObserver);
        goalie = GetComponent<Transform>();
    }

    void Update()
    {
        if (MapGoalAxis == GoalAxis.X)
        {
            if (Ball.position.x < goalie.position.x  - 0.25f)
            {
                goalie.GetComponent<Rigidbody>().velocity = X_AXIS_LEFT_TRANSLATION;
            }
            else if (Ball.position.x > goalie.position.x + 0.25f)
            {
                goalie.GetComponent<Rigidbody>().velocity = X_AXIS_RIGHT_TRANSLATION;
            }
            else
            {
                goalie.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
        else
        {
            if (Ball.position.z < goalie.position.z - 0.25f)
            {
                goalie.GetComponent<Rigidbody>().velocity = Z_AXIS_LEFT_TRANSLATION;
            }
            else if (Ball.position.z > goalie.position.z + 0.25f)
            {
                goalie.GetComponent<Rigidbody>().velocity = Z_AXIS_RIGHT_TRANSLATION;
            }
            else
            {
                goalie.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }

        if (collisionDelayActive)
        {
            if (collisionDelayTimer > 0)
            {
                collisionDelayTimer -= Time.deltaTime;
            }
        }
    }

    public void OnCollisionEnter(Collision _collision)
    {
        if (_collision.transform.tag == ObjectTags.BallTag)
        {
            if (CurrentGame.lastBallPossessor != null && collisionDelayTimer <= 0)
            {
                if (gameObject.tag == ObjectTags.RedGoalie && CurrentGame.lastBallPossessor.GetComponent<IGameCharacter>().GetTeam() == AIPlayerBehaviour.Team.BLUE)
                {
                    NotifyAllObservers(Subject.NotifyReason.SHOT_TO_GOAL, CurrentGame.lastBallPossessor);
                }
                else if (gameObject.tag == ObjectTags.BlueGoalie && CurrentGame.lastBallPossessor.GetComponent<IGameCharacter>().GetTeam() == AIPlayerBehaviour.Team.RED)
                {
                    NotifyAllObservers(Subject.NotifyReason.SHOT_TO_GOAL, CurrentGame.lastBallPossessor);
                }
                StartCollisionDelay();
            }
        }
    }

    private void StartCollisionDelay()
    {
        collisionDelayActive = true;
        collisionDelayTimer = 1f;
    }

}