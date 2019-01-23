using Assets.Scripts.GameCharacter.HumanPlayer;
using Assets.Scripts.GameController;
using UnityEngine;

namespace Assets.Scripts.GameCharacter.AI
{
    public class AIPickUpBonus : MonoBehaviour
    {
        private const int NUMBER_OF_VOTES_IN_BONUS_RANDOMIZATION = 29;
    
        public PickUpBonus.Bonuses activeBonus;
        public int scoreDifferential;
        public GameObject scoreBoard;

        void Start()
        {
            activeBonus = PickUpBonus.Bonuses.NONE;
            scoreDifferential = 0;
        }

        void OnTriggerEnter(Collider _collider)
        {
            if (_collider.gameObject.CompareTag("Bonus"))
            {
                AIPlayerBehaviour.Team bonusTeam = _collider.gameObject.transform.GetComponent<Bonus>().team;
                if (activeBonus == PickUpBonus.Bonuses.NONE && bonusTeam == transform.GetComponent<IGameCharacter>().GetTeam())
                {
                    _collider.transform.GetComponent<Collider>().enabled = false;
                    _collider.transform.GetComponent<MeshRenderer>().enabled = false;
                    GenerateBonus();
                }
            }
        }

        private void GenerateBonus()
        {
            scoreBoard.GetComponent<UpdateScore>().GetScoreDifferential(GetComponent<IGameCharacter>().GetTeam());

            int bonus;

            if (scoreDifferential < -1)
            {
                bonus = Random.Range(1, 13);
                if (bonus == 1)
                {
                    activeBonus = PickUpBonus.Bonuses.POLARITY_REVERSER;
                }
                else if (bonus == 2)
                {
                    activeBonus = PickUpBonus.Bonuses.MAGNET_UPGRADE;
                }
                else if (bonus > 2 && bonus < 6)
                {
                    activeBonus = PickUpBonus.Bonuses.SPRING;
                }
                else if (bonus > 5 && bonus < 10)
                {
                    activeBonus = PickUpBonus.Bonuses.SPEED_BOOST;
                }
                else if (bonus > 9 && bonus < 12)
                {
                    activeBonus = PickUpBonus.Bonuses.HOMING_MISSILE;
                }
                else
                {
                    activeBonus = PickUpBonus.Bonuses.MISSILE;
                }
            }
            else
            {
                //9 is the number of bonuses
                bonus = Random.Range(1, NUMBER_OF_VOTES_IN_BONUS_RANDOMIZATION + (scoreDifferential * 9));

                //Static numbers are decided by vote chart decided preemtively
                if (bonus > 0 && bonus < 4 + scoreDifferential)
                {
                    activeBonus = PickUpBonus.Bonuses.POLARITY_REVERSER;
                }
                else if (bonus > 3 + scoreDifferential && bonus < 7 + scoreDifferential)
                {
                    activeBonus = PickUpBonus.Bonuses.MAGNET_UPGRADE;
                }
                else if (bonus > 6 + scoreDifferential && bonus < 8 + scoreDifferential)
                {
                    activeBonus = PickUpBonus.Bonuses.LIGHTNING_BOLT;
                }
                else if (bonus > 7 + scoreDifferential && bonus < 9 + scoreDifferential)
                {
                    activeBonus = PickUpBonus.Bonuses.MR_FREEZE;
                }
                else if (bonus > 8 + scoreDifferential && bonus < 14 + scoreDifferential)
                {
                    activeBonus = PickUpBonus.Bonuses.SPRING;
                }
                else if (bonus > 13 + scoreDifferential && bonus < 16 + scoreDifferential)
                {
                    activeBonus = PickUpBonus.Bonuses.STAR;
                }
                else if (bonus > 15 + scoreDifferential && bonus < 21 + scoreDifferential)
                {
                    activeBonus = PickUpBonus.Bonuses.SPEED_BOOST;
                }
                else if (bonus > 20 + scoreDifferential && bonus < 25)
                {
                    activeBonus = PickUpBonus.Bonuses.HOMING_MISSILE;
                }
                else
                {
                    activeBonus = PickUpBonus.Bonuses.MISSILE;
                }
            }
        }

    }
}