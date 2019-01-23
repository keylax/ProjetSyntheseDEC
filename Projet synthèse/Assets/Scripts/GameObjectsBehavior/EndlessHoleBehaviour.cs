using Assets.Scripts.GameCharacter;
using UnityEngine;

namespace Assets.Scripts.GameObjectsBehavior
{
    public class EndlessHoleBehaviour : ObjectBehavior
    {
        void OnTriggerEnter(Collider _other)
        {
            if (_other.gameObject.tag == ObjectTags.BallTag)
            {
                _other.enabled = false;
            }
            else if (_other.gameObject.tag == ObjectTags.AITag)
            {
                _other.transform.GetComponent<IGameCharacter>().StunPlayer(12f);
            }
            else if (ObjectTags.ShipCollider == _other.gameObject.tag)
            {
                _other.transform.parent.GetComponent<IGameCharacter>().StunPlayer(5f);
            }
        }

    }
}