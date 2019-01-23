using Assets.Scripts.GameController;
using Assets.Scripts.Observer;
using UnityEngine;

namespace Assets.Scripts.GameObjectsBehavior
{
    public class BallBehaviour : SubjectObjectBehaviour
    {
        private const int MAXIMUM_BALL_VELOCITY = 45;

        public GameObject possessor;
        public GameObject scoreboard;

        private bool isPossessed;

        public bool IsPossessed
        {
            get { return isPossessed; }
            set
            {
                isPossessed = value;
            }
        }

        //Returns a negative if there is no ground below it
        public float DistanceFromGround()
        {
            float distanceFromGround = -1f;

            RaycastHit groundHit;

            if (Physics.Raycast(transform.position, -Vector3.up, out groundHit))
            {
                distanceFromGround = groundHit.distance;
            }

            return distanceFromGround;
        }

        void Start()
        {
            AddObserver(CurrentGame.currentGameObserver);
            IsPossessed = false;
            startPosition = transform.position;
        }

        void Update()
        {
            if (transform.position.y < -15)
            {
                if (!isPossessed)
                {
                    Spawn();
                }
            }

            if (GetComponent<Rigidbody>() != null)
            {
                Vector3 ballSpeed = GetComponent<Rigidbody>().velocity;

                if (ballSpeed.sqrMagnitude > MAXIMUM_BALL_VELOCITY * MAXIMUM_BALL_VELOCITY)
                {
                    GetComponent<Rigidbody>().velocity = ballSpeed.normalized * MAXIMUM_BALL_VELOCITY;
                }
            }
        }

        public void OnCollisionEnter(Collision _collision)
        {
            if (_collision.transform.tag == ObjectTags.GoalTag)
            {
                SoundManager.PlaySFX("AIRHORN");
                Spawn();

                if (_collision.transform.name == "RedGoal")
                {
                    scoreboard.transform.GetComponent<UpdateScore>().AddBluePoint();
                    NotifyAllObservers(Subject.NotifyReason.BLUE_GOAL, null);
                }
                else if (_collision.transform.name == "BlueGoal")
                {
                    scoreboard.transform.GetComponent<UpdateScore>().AddRedPoint();
                    NotifyAllObservers(Subject.NotifyReason.RED_GOAL, null);
                }
            }
        }

        public override void Spawn()
        {
            base.Spawn();
            GetComponent<Collider>().enabled = true;
        }

    }
}