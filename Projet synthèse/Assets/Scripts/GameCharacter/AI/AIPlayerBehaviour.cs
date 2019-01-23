using System.Linq;
using Assets.Scripts.GameCharacter.AI.States_Machine;
using Assets.Scripts.GameCharacter.HumanPlayer;
using Assets.Scripts.GameObjectsBehavior;
using Assets.Scripts.Projectiles;
using UnityEngine;

namespace Assets.Scripts.GameCharacter.AI
{
    public class AIPlayerBehaviour : MonoBehaviour, IAICharacter
    {
        public enum Team { RED, BLUE }

        public enum Destination { BALL, GOAL, CARRIER, CLOSEST_OFFENSIVE_STRATEGIC_POINT, CLOSEST_NON_CARRIER, CLOSEST_DEFENSIVE_STRATEGIC_POINT, CLOSE_TEAMMATE, BALL_LANDING_SPOT, SELF }

        private const float NORMAL_MAXIMUM_SPEED = 20f;
        private const float BOOSTED_MAXIMUM_SPEED = 24f;
        private const float ACCELERATION_BOOST = 3f;
        private const float MISSILE_STUN_DURATION = 8f;

        public Team team;
        public GameObject spawnPoint;

        public Transform ball;
        public Transform targetGoal;
        public Transform protectedGoal;
        public Transform strategicOffensivePointList;
        public Transform strategicDefensivePointList;

        public float distanceDifferenceNeededForPass;
        public float estimatedThrowRange;
        public bool hasBall;

        private bool isStunned;
        private NavMeshAgent agent;
        private AIPickUpBonus bonusManager;
        private State state;
        private float timeUntilStunEnd;
        private AIMagnetManager magnetManager;
        private float boostTimer;
        private bool isFrozen;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            Spawn();
            state = new UnpossessedBall(this);
            timeUntilStunEnd = 0f;
            isStunned = false;
            bonusManager = GetComponent<AIPickUpBonus>();
            magnetManager = transform.GetChild(1).GetComponent<AIMagnetManager>();
            agent.speed = NORMAL_MAXIMUM_SPEED;
        }

        void Update()
        {
            DecreaseBoostTimer();

            if (!isStunned)
            {
                magnetManager.ApplyForce();

                hasBall = ball.GetComponent<BallBehaviour>().possessor == gameObject;
            }
            else
            {
                if (transform.position.y < -35)
                {
                    timeUntilStunEnd = 0.0f;
                    Spawn();
                }

                DecreaseTimeUntilStunEnd(Time.deltaTime);
            }

            state = state.ChangeToAppropriateState();
            state.Update();
        }

        void OnDrawGizmos()
        {
            if (magnetManager != null)
            {
                if (magnetManager.IsMagnetPulling())
                {
                    Gizmos.DrawIcon(transform.position + transform.up * 4, "Pulling.png");
                }
                else
                {
                    Gizmos.DrawIcon(transform.position + transform.up * 4, "Pushing.png");
                }
            }
        }

        void OnTriggerEnter(Collider _other)
        {
            if (_other.gameObject.tag == ObjectTags.StunningProjectileTag)
            {
                StunPlayer(MISSILE_STUN_DURATION);
            }
        }

        private void DecreaseBoostTimer()
        {
            if (boostTimer > 0)
            {
                boostTimer -= Time.deltaTime;

                if (boostTimer < 0)
                {
                    agent.speed = NORMAL_MAXIMUM_SPEED;
                    agent.acceleration -= ACCELERATION_BOOST;
                    SetColorBackToNormal();
                }
            }
        }

        public void StunPlayer(float _durationOfStun)
        {
            if (!GetComponent<Invincibility>().isInvincible)
            {
                timeUntilStunEnd = _durationOfStun;
                isStunned = true;
                GetComponent<Rigidbody>().velocity = agent.velocity / 2;
                GetComponent<Rigidbody>().isKinematic = false;
            }
        }

        public Transform GetAttachedBody()
        {
            return transform;
        }

        public float CalculateDistanceFromBall()
        {
            return Vector3.Distance(ball.position, transform.position);
        }

        public float CalculateDistanceFromGoal()
        {
            return Vector3.Distance(targetGoal.position, transform.position);
        }

        public bool IsClosestTeamMemberOfBall()
        {
            float distanceFromBall = CalculateDistanceFromBall();

            IAICharacter teamMember;

            for (int i = 0; i < 6; i++)
            {
                teamMember = transform.parent.GetChild(i).GetComponent<IAICharacter>();

                if (teamMember != null)
                {
                    if (teamMember.GetTeam() == team && teamMember != this && teamMember.CalculateDistanceFromBall() < distanceFromBall)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public Team GetTeam()
        {
            return team;
        }

        public bool HasBall()
        {
            return hasBall;
        }

        public IGameCharacter FindBallPossessor()
        {
            IGameCharacter ballPossessor;

            for (int i = 0; i < 6; i++)
            {
                ballPossessor = transform.parent.GetChild(i).GetComponent<IGameCharacter>();

                if (ballPossessor.HasBall())
                {
                    return ballPossessor;
                }
            }

            return null; //What to do when the aiPlayer has the ball? or if nobody has it?
        }

        public bool IsBallCarrierOnSameTeam()
        {
            IGameCharacter ballCarrier = FindBallPossessor();

            if (ballCarrier == null)
                return false;

            return ballCarrier.GetTeam() == team;
        }

        public bool IsBallPossessedByAnother()
        {
            if (!hasBall && ball.GetComponent<BallBehaviour>().IsPossessed)
                return true;

            return false;
        }

        public void SetMagnetToPush()
        {
            if (magnetManager.IsMagnetPulling() && !isFrozen)
            {
                magnetManager.InvertMagnetPolarity(false);
            }
        }

        public void SetMagnetToPull()
        {
            if (!magnetManager.IsMagnetPulling() && !isFrozen)
            {
                magnetManager.InvertMagnetPolarity(false);
            }
        }

        public void MoveToDestination(Destination _destination)
        {
            if (!isFrozen)
            {
                Vector3 newDestination = FindPositionOfDestination(_destination);

                if (agent.destination != newDestination)
                {
                    if (!agent.SetDestination(newDestination))
                    {
                        Debug.Log("Somehow, setting a destination for pathfinding failed...");
                    }
                }
            }
        }

        private Vector3 FindPositionOfDestination(Destination _destination)
        {
            switch (_destination)
            {
                case Destination.BALL:
                    return ball.transform.position;

                case Destination.CARRIER:
                    return ball.position + (protectedGoal.position - ball.position).normalized * 2;

                case Destination.GOAL:
                    return targetGoal.position;

                case Destination.CLOSEST_OFFENSIVE_STRATEGIC_POINT:
                    return FindClosestStrategicPoint(strategicOffensivePointList).position;

                case Destination.CLOSEST_DEFENSIVE_STRATEGIC_POINT:
                    return FindClosestStrategicPoint(strategicDefensivePointList).position;

                case Destination.CLOSEST_NON_CARRIER:
                    return FindClosestEnemyNonCarrier().position + (ball.position - FindClosestEnemyNonCarrier().position).normalized * 2;

                case Destination.CLOSE_TEAMMATE:
                    return FindClosestTeammateToGoalIgnoringTooNearTeammates().GetAttachedBody().position;

                case Destination.BALL_LANDING_SPOT:
                    return GetBallLandingLocation();

                case Destination.SELF:
                    return transform.position;

                default:
                    Debug.Log("You entered a 'Default' case for destination, which shouldn't happen...");
                    return ball.transform.position;
            }
        }

        private Transform FindClosestEnemyNonCarrier()
        {
            Transform closestFound = null;
            float closestDistanceFound = 80000f;

            IGameCharacter aiPlayer;
            float currentPlayerDistance;

            for (int i = 0; i < 5; i++)
            {
                aiPlayer = transform.parent.GetChild(i).GetComponent<IGameCharacter>();

                if (aiPlayer.GetTeam() != team)
                {
                    currentPlayerDistance = Vector3.Distance(transform.parent.GetChild(i).transform.position, transform.position);

                    if (currentPlayerDistance < closestDistanceFound)
                    {
                        closestFound = transform.parent.GetChild(i).transform;
                    }
                }
            }

            return closestFound;
        }

        private Transform FindClosestStrategicPoint(Transform _listOfPoints)
        {
            if (Vector3.Distance(_listOfPoints.GetChild(0).transform.position, transform.position) < Vector3.Distance(_listOfPoints.GetChild(1).transform.position, transform.position))
            {
                return _listOfPoints.GetChild(0).transform;
            }

            return _listOfPoints.GetChild(1).transform;
        }

        public bool IsBallCloseToOpponentGoal()
        {
            if (Vector3.Distance(ball.position, targetGoal.position) < estimatedThrowRange)
            {
                return true;
            }

            return false;
        }

        public bool IsBallCloserToOwnGoal()
        {
            if (Vector3.Distance(ball.position, targetGoal.position) > Vector3.Distance(ball.position, protectedGoal.position))
                return true;

            return false;
        }

        public IGameCharacter FindClosestTeammateToGoalIgnoringTooNearTeammates()
        {
            IGameCharacter teamMate;

            for (int i = 0; i < 6; i++)
            {
                teamMate = transform.parent.GetChild(i).GetComponent<IGameCharacter>();

                if (teamMate != this && teamMate.GetTeam() == team && teamMate.CalculateDistanceFromBall() < estimatedThrowRange && teamMate.CalculateDistanceFromGoal() + distanceDifferenceNeededForPass < CalculateDistanceFromGoal())
                {
                    return teamMate;
                }
            }

            return null;
        }

        public void Spawn()
        {
            transform.position = spawnPoint.transform.position;
            GetComponent<Collider>().enabled = true;
        }

        public float GetThrowRange()
        {
            return estimatedThrowRange;
        }

        public float GetBallDistanceFromGround()
        {
            return ball.GetComponent<BallBehaviour>().DistanceFromGround();
        }

        private Vector3 GetBallLandingLocation()
        {
            Vector3 landingSpot = ball.position;
            Vector3 ballDirectionVector = ball.GetComponent<Rigidbody>().velocity;

            //Get approximate landing spot assuming no wall
            ballDirectionVector.y = 0f; //Ignore y velocity to avoid having landing locations underground
            landingSpot += ballDirectionVector;
            landingSpot.y -= ball.GetComponent<BallBehaviour>().DistanceFromGround();

            //Test for wall and bring landing point at it's base if there is one
            RaycastHit wallHit;
            landingSpot.y += 1.5f; //To avoid colliding with the ground, look at just above it
            if (Physics.Raycast(transform.position, ball.position - landingSpot, out wallHit, (ball.position - landingSpot).magnitude))
            {
                float firstWallHit = wallHit.distance;

                ballDirectionVector.Normalize();

                ballDirectionVector *= firstWallHit;

                landingSpot = ball.position;
                landingSpot += ballDirectionVector;
                landingSpot.y -= ball.GetComponent<BallBehaviour>().DistanceFromGround();
            }
            else
            {
                landingSpot.y -= 1.5f;
            }
            return landingSpot;
        }

        public bool IsWithinShootingDistanceOfGoal()
        {
            return agent.remainingDistance < estimatedThrowRange;
        }

        public bool CanThrowAt(Destination _destination)
        {
            Vector3 destinationLocation = FindPositionOfDestination(_destination);

            if (Physics.Raycast(ball.position, destinationLocation - ball.position, Vector3.Distance(targetGoal.position, ball.position) - 3f))
            {
                return false;
            }

            return true;
        }

        public void SetPathFindginEnabled(bool _newEnabledState)
        {
            GetComponent<Rigidbody>().isKinematic = _newEnabledState;
            agent.enabled = _newEnabledState;
        }

        private void DecreaseTimeUntilStunEnd(float _deltaTime)
        {
            timeUntilStunEnd -= _deltaTime;

            if (timeUntilStunEnd <= 0f)
            {
                RaycastHit[] hits = Physics.RaycastAll(new Vector3(transform.position.x + 1.5f, transform.position.y + 0.5f, transform.position.z + 1.5f), Vector3.down, 3f);
                bool wasHit = hits.Any(_hit => _hit.transform.gameObject.GetComponentInParent<Transform>().tag == ObjectTags.Environment);

                if (!wasHit)
                {
                    timeUntilStunEnd = 2f;
                }
                else
                {
                    isStunned = false;
                    SetPathFindginEnabled(true);
                    GetComponent<VisualEffects>().RemoveThunderBolt();
                }
            }
        }

        public bool IsStunned()
        {
            return isStunned;
        }

        public PickUpBonus.Bonuses GetCurrentBonus()
        {
            return bonusManager.activeBonus;
        }

        public void UseBonus()
        {
            GetComponent<BonusShooting>().ShootBonus(bonusManager.activeBonus);
            bonusManager.activeBonus = PickUpBonus.Bonuses.NONE;
        }

        public bool IsBallCloseToOwnGoal()
        {
            if (Vector3.Distance(ball.position, protectedGoal.position) < estimatedThrowRange)
            {
                return true;
            }

            return false;
        }

        public void TurnTowards(Destination _targetToLookTowards)
        {
            Vector3 locationToLookAt = FindPositionOfDestination(_targetToLookTowards);
            locationToLookAt.x += Random.Range(-6, 6);
            locationToLookAt.z += Random.Range(-6, 6);
            locationToLookAt.y = transform.position.y;

            SetPathFindginEnabled(false);
            transform.LookAt(locationToLookAt);
            SetPathFindginEnabled(true);
        }

        public void BoostSpeed()
        {
            boostTimer = 5;
            agent.speed = BOOSTED_MAXIMUM_SPEED;
            agent.acceleration += ACCELERATION_BOOST;
        }

        public void FreezeGameCharacter()
        {
            isFrozen = true;
            SetPathFindginEnabled(false);
            GetComponent<Rigidbody>().velocity = agent.velocity;
            transform.GetComponent<MeshRenderer>().material =
                    Resources.Load("FreezeMaterial") as Material;
        }

        public void UnFreezeGameCharacter()
        {
            isFrozen = false;
            SetPathFindginEnabled(true);
            SetColorBackToNormal();
        }

        public void SetColorBackToNormal()
        {
            if (team == Team.BLUE)
            {
                transform.GetComponent<MeshRenderer>().material =
                   Resources.Load("BlueShip") as Material;
            }
            else if (team == Team.RED)
            {
                transform.GetComponent<MeshRenderer>().material =
                   Resources.Load("RedShip") as Material;
            }
        }

        public Vector3 GetCurrentVelocity()
        {
            if (agent.enabled)
            {
                return agent.velocity + transform.forward;
            }
            else
            {
                return GetComponent<Rigidbody>().velocity + transform.forward;
            }
        }

    }
}