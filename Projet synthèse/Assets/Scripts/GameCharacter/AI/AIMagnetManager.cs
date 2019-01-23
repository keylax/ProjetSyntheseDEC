using UnityEngine;

namespace Assets.Scripts.GameCharacter.AI
{
    public class AIMagnetManager : MagnetManager
    {
        public void ApplyForce()
        {
            UpdateMagnetForce(1f);
        }

        private void Update()
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
        }

    }
}