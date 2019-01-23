using System.Collections.Generic;

namespace Assets.Scripts.Game_Objects
{
    public class TileGoal : Tile
    {
        private Tile.Direction firstEntranceDirection;

        /// <summary>
        /// Gets or sets the first entrance direction.
        /// </summary>
        /// <value>
        /// The first entrance direction.
        /// </value>
        public Tile.Direction FirstEntranceDirection
        {
            get { return firstEntranceDirection; }
            set { firstEntranceDirection = value; }
        }
    }
}