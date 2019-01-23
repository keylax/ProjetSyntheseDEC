using UnityEngine;

namespace Assets.Scripts.GameObjectsBehavior
{
    public class ObjectBehavior : MonoBehaviour
    {
        public Vector3 startPosition;

        public virtual void SetStartPosition(Vector3 _position)
        {
            startPosition = _position;
        }

        public virtual void Spawn()
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = startPosition;
        }

    }
}