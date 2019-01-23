using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Game_Objects
{
    public class Map
    {
        private List<List<Tile>> activeMap;
        private int height;
        private int width;


        /// <summary>
        /// Gets or sets the height of the map.
        /// </summary>
        /// <value>
        /// The new height.
        /// </value>
        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        /// <summary>
        /// Gets or sets the width of the map.
        /// </summary>
        /// <value>
        /// The new width.
        /// </value>
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        /// <summary>
        /// Gets or sets the active map this used to deserialise the map and get its content.
        /// </summary>
        /// <value>
        /// The new active map.
        /// </value>
        public List<List<Tile>> ActiveMap
        {
            get { return activeMap; }
            set { activeMap = value; }
        }
    }
}