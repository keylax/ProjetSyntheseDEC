using Assets.Scripts.Projectiles;
using UnityEngine;
using Assets.Scripts.GameObjectsBehavior;

namespace Assets.Scripts.GameCharacter.HumanPlayer
{
    public class StunPlayer : MonoBehaviour
    {
        public bool stunned;
        private float timeLeftStunned = 0;

        public void Update()
        {
            if (timeLeftStunned > 0)
            {
                timeLeftStunned -= Time.deltaTime;
            }
            else
            {
                stunned = false;
                GetComponent<PlayerMovement>().enabled = true;
                GetComponent<BonusShooting>().enabled = true;
                transform.GetChild(4).GetComponent<HumanMagnetManager>().enabled = true;
                GetComponent<VisualEffects>().RemoveThunderBolt();
            }
        }

        public void StunThePlayer(float _stunTimer)
        {
            SoundManager.PlaySFX("Impact1");
            if (GetComponent<Invincibility>().isInvincible == false)
            {
                transform.GetChild(0).GetChild(0).GetComponent<CameraShake>().Shake();
                stunned = true;
                timeLeftStunned = _stunTimer;
                GetComponent<Rigidbody>().velocity /= 2;
            }
        }
    }
}