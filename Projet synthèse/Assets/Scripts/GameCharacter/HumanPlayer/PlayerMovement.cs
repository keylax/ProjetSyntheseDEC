using Assets.Scripts.GameController;
using Assets.Scripts.Observer;
using UnityEngine;

namespace Assets.Scripts.GameCharacter.HumanPlayer
{
    public class PlayerMovement : SubjectMonobehaviour
    {
        public const float MAX_SPEED = 20f;

        private Rigidbody playerRigidbody;
        public float forceMultiplier = 1000f;
        public float rotationMultiplier = 30f;
        public GameObject cameraContainer;
        public Camera camera;
        public GameObject playerSpawn;
        public float deathDistance = -35f;

        private float boostTimer;
        private float currentMaxSpeed;

        void Start()
        {
            AddObserver(CurrentGame.currentGameObserver);
            playerRigidbody = GetComponent<Rigidbody>();
            Spawn();
            boostTimer = 0;
            currentMaxSpeed = MAX_SPEED;
        }

        public void Update()
        {
            float forwardInput = InputManager.GetPlayerVertical(tag);
            float strafeInput = InputManager.GetPlayerHorizontal(tag);
            float steeringInputController = InputManager.GetPlayerSteering(tag);

            if (transform.position.y < deathDistance)
            {
                transform.GetChild(3).GetComponent<Collider>().enabled = true;
                Spawn();
                NotifyAllObservers(Subject.NotifyReason.FELL_IN_PIT, gameObject);
            }
            else
            {
                playerRigidbody.AddForce(transform.forward * forwardInput * forceMultiplier * Time.deltaTime);
                playerRigidbody.AddForce(transform.right * strafeInput * forceMultiplier * Time.deltaTime);

                if (playerRigidbody.velocity.magnitude > currentMaxSpeed)
                {
                    playerRigidbody.velocity = playerRigidbody.velocity.normalized * currentMaxSpeed;
                }

                transform.Rotate(Vector3.up * steeringInputController * rotationMultiplier * Time.deltaTime);
            }

            if (boostTimer > 0)
            {
                boostTimer -= Time.deltaTime;
            }
            else
            {
                if (currentMaxSpeed != MAX_SPEED)
                {
                    currentMaxSpeed /= 1.5f;
                }
                GetComponent<HumanPlayerBehaviour>().SetColorBackToNormal();

            }
        }

        public void Spawn()
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = playerSpawn.transform.position;
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            transform.GetChild(3).GetComponent<Collider>().enabled = true;
        }

        public void BoostSpeed()
        {
            boostTimer = 5;
            currentMaxSpeed *= 1.5f;
            playerRigidbody.velocity += transform.forward * currentMaxSpeed / 3;
        }

    }
}