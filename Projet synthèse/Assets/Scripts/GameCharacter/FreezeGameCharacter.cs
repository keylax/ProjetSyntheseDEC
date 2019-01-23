using UnityEngine;

namespace Assets.Scripts.GameCharacter
{
    public class FreezeGameCharacter : MonoBehaviour
    {
        private bool isFrozen = false;
        private const float freezeTimeInSeconds = 3;
        private float timeLeftBeforeUnfreeze = 0;

        private IGameCharacter linkedCharacter;

        private void Start()
        {
            linkedCharacter = GetComponent<IGameCharacter>();
        }

        private void Update () 
        {
            if (isFrozen)
            {
                timeLeftBeforeUnfreeze -= Time.deltaTime;

                if (timeLeftBeforeUnfreeze < 0)
                {
                    UnFreezePlayer();
                }
            }
        }

        public void FreezePlayer()
        {
            if (GetComponent<Invincibility>().isInvincible == false)
            {
                isFrozen = true;
                timeLeftBeforeUnfreeze = freezeTimeInSeconds;
                linkedCharacter.FreezeGameCharacter();
            }
        }

        private void UnFreezePlayer()
        {
            isFrozen = false;
            linkedCharacter.UnFreezeGameCharacter();
        }

    }
}