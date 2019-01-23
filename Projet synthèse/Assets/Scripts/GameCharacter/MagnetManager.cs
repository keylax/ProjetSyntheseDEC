using Assets.Scripts.GameObjectsBehavior;
using UnityEngine;

namespace Assets.Scripts.GameCharacter
{
    public abstract class MagnetManager : MonoBehaviour
    {
        private const float MAXIMUM_DISTANCE_TO_POSSESS_BALL = 1.1f;
        private const float MAGNET_UPGRADE_DURATION_IN_SECONDS = 10f;
        private const float MAGNET_UPGRADE_FORCE = 6f;
        protected const float MAGNET_STANDARD_FORCE = 2f;

        private Collider collidingObject;
        private int magnetMode = 1; //1 for pulling, -1 for pushing
        private Vector3 defaultCenterOfMass;

        protected float secondsUntilNextChange;
        protected float magnetForceMultiplier = MAGNET_STANDARD_FORCE;
        protected float timeLeftToUpgrade;

        public static BallBehaviour ballBehaviour;

        void Start()
        {
            defaultCenterOfMass = transform.parent.GetComponent<Rigidbody>().centerOfMass;
        }

        protected void UpdateMagnetForce(float _magnetForce)
        {
            if (collidingObject != null && _magnetForce > 0)
            {
                float distanceFromBall = Vector3.Distance(transform.position, collidingObject.transform.position);

                if (distanceFromBall < MAXIMUM_DISTANCE_TO_POSSESS_BALL && magnetMode == 1)
                {
                    if (ballBehaviour.IsPossessed == false)
                    {
                        if (ballBehaviour.transform.parent == null)
                        {
                            collidingObject.transform.position = transform.position + transform.parent.transform.forward;
                            ballBehaviour.transform.SetParent(transform.parent);
                            Destroy(ballBehaviour.GetComponent<Rigidbody>());
                            transform.parent.GetComponent<Rigidbody>().centerOfMass = defaultCenterOfMass;
                        }

                        ballBehaviour.IsPossessed = true;
                        ballBehaviour.possessor = transform.parent.gameObject;
                    }
                }
                else
                {
                    if (ballBehaviour.possessor != null)
                    {
                        if (ballBehaviour.possessor == transform.parent.gameObject)
                        {
                            DropBall();
                        }
                    }
                    else
                    {
                        Vector3 directionToObject = transform.position - collidingObject.transform.position;
                        directionToObject.Normalize();

                        distanceFromBall = 9 - distanceFromBall;

                        distanceFromBall *= 2;

                        if (distanceFromBall < 0)
                        {
                            distanceFromBall = 0;
                        }

                        float magnetStrenghtFromDistance = distanceFromBall * magnetForceMultiplier * _magnetForce;

                        if (magnetMode == 1)
                        {
                            collidingObject.GetComponent<Rigidbody>().AddForce(magnetStrenghtFromDistance * directionToObject * magnetMode * 5, ForceMode.Force);
                        }
                        else
                        {
                            collidingObject.GetComponent<Rigidbody>().velocity = transform.forward * 100;
                            collidingObject.GetComponent<Rigidbody>().AddForce(magnetStrenghtFromDistance * directionToObject * magnetMode, ForceMode.Impulse);
                        }
                    }
                }
            }
            else if (ballBehaviour.possessor != null)
            {
                if (ballBehaviour.possessor == transform.parent.gameObject)
                {
                    DropBall();
                }
            }
        }

        private void DropBall()
        {
            ballBehaviour.transform.parent = null;
            ballBehaviour.gameObject.AddComponent<Rigidbody>();
            ballBehaviour.GetComponent<Rigidbody>().velocity = transform.parent.GetComponent<IGameCharacter>().GetCurrentVelocity();

            ballBehaviour.IsPossessed = false;
            ballBehaviour.possessor = null;
        }

        public bool InvertMagnetPolarity(bool _forceChange)
        {
            if (secondsUntilNextChange <= 0 || _forceChange)
            {
                magnetMode *= -1;
                secondsUntilNextChange = 0.5f;
                return true;
            }

            return false;
        }

        public void UpgradeMagnetForce()
        {
            timeLeftToUpgrade = MAGNET_UPGRADE_DURATION_IN_SECONDS;
            magnetForceMultiplier = MAGNET_UPGRADE_FORCE;
        }

        public bool IsMagnetPulling()
        {
            return magnetMode > 0;
        }

        public void OnTriggerEnter(Collider _collider)
        {
            if (_collider.gameObject.tag == ObjectTags.BallTag)
            {
                collidingObject = _collider;
            }
        }

        public void OnTriggerExit(Collider _collider)
        {
            if (_collider.gameObject.tag == ObjectTags.BallTag)
            {
                collidingObject = null;
            }
        }

    }
}