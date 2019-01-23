using Assets.Scripts.GameCharacter.AI;
using Assets.Scripts.GameController;
using Assets.Scripts.GameObjectsBehavior;
using Assets.Scripts.Projectiles;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameCharacter.HumanPlayer
{
    public class HumanPlayerBehaviour : MonoBehaviour, IGameCharacter
    {
        public Transform ball;
        public Transform targetGoal;
        public AIPlayerBehaviour.Team team;
        public Text polarityText;

        private PickUpBonus bonusManager;

        private void Start()
        {
            bonusManager = transform.GetComponent<PickUpBonus>();
            transform.GetChild(4).GetComponent<HumanMagnetManager>().polarityText = polarityText;
        }

        void Update()
        {
            if (InputManager.GetPlayerFire(tag) && bonusManager.activeBonus != PickUpBonus.Bonuses.NONE)
            {
                GetComponent<BonusShooting>().ShootBonus(bonusManager.activeBonus);
                bonusManager.activeBonus = PickUpBonus.Bonuses.NONE;
            }
        }

        public AIPlayerBehaviour.Team GetTeam()
        {
            return team;
        }

        public bool HasBall()
        {
            if (ball.GetComponent<BallBehaviour>().possessor == gameObject)
            {
                return true;
            }
            return false;
        }

        public float CalculateDistanceFromBall()
        {
            return Vector3.Distance(ball.position, transform.position);
        }

        public float CalculateDistanceFromGoal()
        {
            return Vector3.Distance(targetGoal.position, transform.position);
        }

        public Transform GetAttachedBody()
        {
            return transform;
        }

        public bool IsStunned()
        {
            return GetComponent<StunPlayer>().stunned;
        }

        public PickUpBonus.Bonuses GetCurrentBonus()
        {
            return bonusManager.activeBonus;
        }

        public void FreezeGameCharacter()
        {
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<BonusShooting>().enabled = false;
            transform.GetChild(4).GetComponent<HumanMagnetManager>().enabled = false;
            transform.GetChild(3).GetChild(0).GetComponent<MeshRenderer>().material =
                Resources.Load("FreezeMaterial") as Material;
        }

        public void UnFreezeGameCharacter()
        {
            GetComponent<PlayerMovement>().enabled = true;
            GetComponent<BonusShooting>().enabled = true;
            transform.GetChild(4).GetComponent<HumanMagnetManager>().enabled = true;
            SetColorBackToNormal();
        }

        public void SetColorBackToNormal()
        {
            if (team == AIPlayerBehaviour.Team.BLUE)
            {
                transform.GetChild(3).GetChild(0).GetComponent<MeshRenderer>().material =
                   Resources.Load("BlueShip") as Material;
            }
            else if (team == AIPlayerBehaviour.Team.RED)
            {
                transform.GetChild(3).GetChild(0).GetComponent<MeshRenderer>().material =
                   Resources.Load("RedShip") as Material;
            }
        }

        public void StunPlayer(float _durationOfStun)
        {
            GetComponent<StunPlayer>().StunThePlayer(_durationOfStun);
        }

        public Vector3 GetCurrentVelocity()
        {
            return GetComponent<Rigidbody>().velocity;
        }

    }
}