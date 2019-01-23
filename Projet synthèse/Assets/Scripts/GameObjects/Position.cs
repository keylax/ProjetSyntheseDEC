using System;

namespace Assets.Scripts.Game_Objects
{
    public class Position : IEquatable<Position>
    {
        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>
        /// The new x.
        /// </value>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>
        /// The new y.
        /// </value>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>
        /// The new level.
        /// </value>
        public int Level { get; set; }

        /// <summary>
        /// Checks if all of the coordinates of obj are equal to this' coordinates.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public bool Equals(Position obj)
        {
            Position compare = (Position)obj;

            if (compare.X != X) return false;
            if (compare.Y != Y) return false;
            if (compare.Level != Level) return false;

            return true;
        }
    }
}