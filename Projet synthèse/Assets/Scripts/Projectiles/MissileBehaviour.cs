using Assets.Scripts.GameCharacter;
using Assets.Scripts.GameCharacter.AI;
using Assets.Scripts.GameController;
using Assets.Scripts.GameObjectsBehavior;
using Assets.Scripts.Observer;
using UnityEngine;

namespace Assets.Scripts.Projectiles
{
    public class MissileBehaviour : SubjectMonobehaviour
    {

        public bool active;
        public GameObject projectileList;
        public AIPlayerBehaviour.Team teamOfSource;

        void Start()
        {
            active = false;
            AddObserver(CurrentGame.currentGameObserver);
        }

        void Update()
        {
            if (transform.position.y < -35)
            {
                active = false;
            }

            if (active == false)
            {
                gameObject.transform.position = projectileList.transform.position;
            }
        }

        void OnTriggerEnter(Collider _collider)
        {
            if (active && _collider.gameObject.name != "Magnet")
            {
                if (ObjectTags.IsPlayer(_collider.gameObject.tag))
                {
                    _collider.transform.GetComponent<IGameCharacter>().StunPlayer(2f);
                    NotifyAllObservers(Subject.NotifyReason.HIT_BY_MISSILE, _collider.gameObject);
                }
                active = false;
                GameObject prefab = Resources.Load("Explosion") as GameObject;
                GameObject explosion = Instantiate(prefab);
                explosion.transform.position = transform.position;
                Destroy(explosion, 5);
            }
        }

    }
}