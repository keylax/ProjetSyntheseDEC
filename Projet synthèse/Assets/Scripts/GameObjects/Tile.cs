using System.Collections.Generic;
using System.Xml.Serialization;

namespace Assets.Scripts.Game_Objects
{
    [XmlInclude(typeof(TileBallSpawn))]
    [XmlInclude(typeof(TileBlock))]
    [XmlInclude(typeof(TileBonus))]
    [XmlInclude(typeof(TileBottomLessPit))]
    [XmlInclude(typeof(TileEmpty))]
    [XmlInclude(typeof(TileFloor))]
    [XmlInclude(typeof(TileGoal))]
    [XmlInclude(typeof(TilePlayerSpawn))]
    [XmlInclude(typeof(TileRamp))]
    [XmlInclude(typeof(TileUnpassableTerrain))]

    public abstract class Tile
    {
        public enum Direction { Up, Down, Left, Right }

        protected int x;
        protected int y;
        protected int level;

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>
        /// The new x.
        /// </value>
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>
        /// The new y.
        /// </value>
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>
        /// The new level.
        /// </value>
        public int Level
        {
            get { return level; }
            set { level = value; }
        }
    }
}