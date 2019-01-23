using System.Collections.Generic;

namespace Assets.Scripts.Game_Objects
{
    public class TileRamp : Tile
    {
        private Direction upDirection;

        /// <summary>
        /// Gets or sets up direction.
        /// </summary>
        /// <value>
        /// Up direction.
        /// </value>
        public Direction UpDirection
        {
            get { return upDirection; }
            set { upDirection = value; }
        }
    }
}