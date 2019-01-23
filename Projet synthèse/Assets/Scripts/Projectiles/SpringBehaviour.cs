using Assets.Scripts.GameCharacter.AI;
using Assets.Scripts.GameController;
using Assets.Scripts.GameObjectsBehavior;
using Assets.Scripts.Observer;
using UnityEngine;

namespace Assets.Scripts.Projectiles
{
    public class SpringBehaviour : SubjectMonobehaviour
    {
        public bool active;
        public GameObject projectileList;

        void Start()
        {
            active = false;
            AddObserver(CurrentGame.currentGameObserver);
        }

        void Update()
        {
            if (active == false)
            {
                gameObject.transform.position = projectileList.transform.position;
            }

            if (transform.position.y < -35)
            {
                active = false;
            }
        }

        void OnCollisionEnter(Collision _collision)
        {
            if (active)
            {
                if (ObjectTags.IsPlayer(_collision.gameObject.tag) || _collision.gameObject.tag == ObjectTags.AITag)
                {
                    if (!_collision.gameObject.GetComponent<Invincibility>().isInvincible)
                    {
                        SoundManager.PlaySFX("SpringJump");
                        if (_collision.gameObject.tag == ObjectTags.AITag)
                        {
                            _collision.transform.GetComponent<AIPlayerBehaviour>().StunPlayer(5f);
                        }

                        Rigidbody playerRigidBody = _collision.gameObject.transform.GetComponent<Rigidbody>();
                        playerRigidBody.AddForce(Vector3.up * 60000f);
                        NotifyAllObservers(Subject.NotifyReason.SPRING_USED, _collision.gameObject);
                    }
                    active = false;
                }
            }
        }

    }
}