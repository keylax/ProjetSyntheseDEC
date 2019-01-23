using Assets.Scripts.GameController;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameCharacter.HumanPlayer
{
    public class HumanMagnetManager : MagnetManager
    {
        public Text polarityText;
        private const string POSITIVE_POLARITY = "Pulling";
        private const string NEGATIVE_POLARITY = "Pushing";

        public void Update()
        {
            if (secondsUntilNextChange > 0)
            {
                secondsUntilNextChange -= Time.deltaTime;
            }

            if (timeLeftToUpgrade > 0)
            {
                timeLeftToUpgrade -= Time.deltaTime;

                if (timeLeftToUpgrade <= 0)
                {
                    magnetForceMultiplier = MAGNET_STANDARD_FORCE;
                }
            }

            float playerMagnetForce = InputManager.GetPlayerMagnetForce(transform.parent.gameObject.tag);

            if (InputManager.GetPlayerChangePolarity(transform.parent.gameObject.tag))
            {
                if(InvertMagnetPolarity(false))
                {
                    InvertPolarityInUI();
                }
            }

            UpdateMagnetForce(playerMagnetForce);
        }

        public void InvertPolarityInUI()
        {
            if (polarityText.text == POSITIVE_POLARITY)
            {
                polarityText.text = NEGATIVE_POLARITY;
            }
            else if (polarityText.text == NEGATIVE_POLARITY)
            {
                polarityText.text = POSITIVE_POLARITY;
            }
        }

    }
}