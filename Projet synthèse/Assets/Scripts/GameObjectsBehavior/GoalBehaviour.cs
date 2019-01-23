using Assets.Scripts.Game_Objects;
using UnityEngine;

namespace Assets.Scripts.GameObjectsBehavior
{
    public class GoalBehaviour : ObjectBehavior
    {
        private static readonly Vector3 TURN_RIGHT_VECTOR = new Vector3(0, -90, 0);
        private static readonly Vector3 TURN_LEFT_VECTOR = new Vector3(0, 90, 0);

        public Tile.Direction facingDirection;

        public void Start()
        {
            if (facingDirection == Tile.Direction.Right)
            {
                transform.Rotate(TURN_RIGHT_VECTOR);
            }
            if (facingDirection == Tile.Direction.Left)
            {
                transform.Rotate(TURN_LEFT_VECTOR);
            }
        }

        public void SetDirection(Tile.Direction direction)
        {
            facingDirection = direction;
        }

    }
}
