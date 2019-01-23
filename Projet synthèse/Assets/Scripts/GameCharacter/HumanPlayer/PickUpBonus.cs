using Assets.Scripts.GameCharacter.AI;
using Assets.Scripts.GameController;
using Assets.Scripts.Observer;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameCharacter.HumanPlayer
{
    public class PickUpBonus : SubjectMonobehaviour
    {
        public enum Bonuses { MISSILE, HOMING_MISSILE, SPRING, LIGHTNING_BOLT, MR_FREEZE, SPEED_BOOST, POLARITY_REVERSER, MAGNET_UPGRADE, STAR, NONE }
        
        public Bonuses activeBonus;
        public int scoreDifferential; 
        public GameObject bonusImage;
        public GameObject scoreBoard;

        void Start()
        {
            AddObserver(CurrentGame.currentGameObserver);
            activeBonus = Bonuses.NONE;
            scoreDifferential = 0;
        }

        void Update()
        {
            if (activeBonus == Bonuses.NONE)
            {
                bonusImage.transform.GetComponent<RawImage>().enabled = false;
            }
        }

        void OnTriggerEnter(Collider _collider)
        {
            if (_collider.gameObject.CompareTag("Bonus"))
            {
                AIPlayerBehaviour.Team bonusTeam = _collider.gameObject.transform.GetComponent<Bonus>().team;
                if (activeBonus == Bonuses.NONE && bonusTeam == transform.GetComponent<IGameCharacter>().GetTeam())
                {
                    _collider.transform.GetComponent<Collider>().enabled = false;
                    _collider.transform.GetComponent<MeshRenderer>().enabled = false;
                    bonusImage.transform.GetComponent<RawImage>().enabled = true;
                    GenerateBonus();
                    NotifyAllObservers(Subject.NotifyReason.BONUS_ACQUIRED, gameObject);
                }
            }
        }

        private void GenerateBonus()
        {
            scoreDifferential = scoreBoard.GetComponent<UpdateScore>().GetScoreDifferential(GetComponent<HumanPlayerBehaviour>().GetTeam());
            SoundManager.PlaySFX("BonusPickup");

            int bonus;

            if (scoreDifferential < -1)
            {
                bonus = Random.Range(1, 13);
                if (bonus == 1)
                {
                    activeBonus = Bonuses.POLARITY_REVERSER;
                    bonusImage.transform.GetComponent<RawImage>().texture = Resources.Load("BonusesImage/ReverseBonus") as Texture;
                }
                else if (bonus == 2)
                {
                    activeBonus = Bonuses.MAGNET_UPGRADE;
                    bonusImage.transform.GetComponent<RawImage>().texture = Resources.Load("BonusesImage/MagnetUpgradeBonus") as Texture;
                }
                else if (bonus > 2 && bonus < 6)
                {
                    activeBonus = Bonuses.SPRING;
                    bonusImage.transform.GetComponent<RawImage>().texture = Resources.Load("BonusesImage/SpringBonus") as Texture;
                }
                else if (bonus > 5 && bonus < 10)
                {
                    activeBonus = Bonuses.SPEED_BOOST;
                    bonusImage.transform.GetComponent<RawImage>().texture = Resources.Load("BonusesImage/BoostBonus") as Texture;
                }
                else if (bonus > 9 && bonus < 12)
                {
                    activeBonus = Bonuses.HOMING_MISSILE;
                    bonusImage.transform.GetComponent<RawImage>().texture = Resources.Load("BonusesImage/HomingBonus") as Texture;
                }
                else
                {
                    activeBonus = Bonuses.MISSILE;
                    bonusImage.transform.GetComponent<RawImage>().texture = Resources.Load("BonusesImage/MissileBonus") as Texture;
                }
            }
            else
            {
                //29 is the total votes for all bonus on default value and 9 is the number of bonuses
                bonus = Random.Range(1, 29 + (scoreDifferential * 9));

                //Static numbers are decided by vote chart decided preemtively
                if (bonus > 0 && bonus < 4 + scoreDifferential)
                {
                    activeBonus = Bonuses.POLARITY_REVERSER;
                    bonusImage.transform.GetComponent<RawImage>().texture = Resources.Load("BonusesImage/ReverseBonus") as Texture;
                }
                else if (bonus > 3 + scoreDifferential && bonus < 7 + scoreDifferential)
                {
                    activeBonus = Bonuses.MAGNET_UPGRADE;
                    bonusImage.transform.GetComponent<RawImage>().texture = Resources.Load("BonusesImage/MagnetUpgradeBonus") as Texture;
                }
                else if (bonus > 6 + scoreDifferential && bonus < 8 + scoreDifferential)
                {
                    activeBonus = Bonuses.LIGHTNING_BOLT;
                    bonusImage.transform.GetComponent<RawImage>().texture = Resources.Load("BonusesImage/LightningBonus") as Texture;
                }
                else if (bonus > 7 + scoreDifferential && bonus < 9 + scoreDifferential)
                {
                    activeBonus = Bonuses.MR_FREEZE;
                    bonusImage.transform.GetComponent<RawImage>().texture = Resources.Load("BonusesImage/FreezeBonus") as Texture;
                }
                else if (bonus > 8 + scoreDifferential && bonus < 14 + scoreDifferential)
                {
                    activeBonus = Bonuses.SPRING;
                    bonusImage.transform.GetComponent<RawImage>().texture = Resources.Load("BonusesImage/SpringBonus") as Texture;
                }
                else if (bonus > 13 + scoreDifferential && bonus < 16 + scoreDifferential)
                {
                    activeBonus = Bonuses.STAR;
                    bonusImage.transform.GetComponent<RawImage>().texture = Resources.Load("BonusesImage/StarBonus") as Texture;
                }
                else if (bonus > 15 + scoreDifferential && bonus < 21 + scoreDifferential)
                {
                    activeBonus = Bonuses.SPEED_BOOST;
                    bonusImage.transform.GetComponent<RawImage>().texture = Resources.Load("BonusesImage/BoostBonus") as Texture;
                }
                else if (bonus > 20 + scoreDifferential && bonus < 25)
                {
                    activeBonus = Bonuses.HOMING_MISSILE;
                    bonusImage.transform.GetComponent<RawImage>().texture = Resources.Load("BonusesImage/HomingBonus") as Texture;
                }
                else
                {
                    activeBonus = Bonuses.MISSILE;
                    bonusImage.transform.GetComponent<RawImage>().texture = Resources.Load("BonusesImage/MissileBonus") as Texture;
                }
            }
        }

    }
}