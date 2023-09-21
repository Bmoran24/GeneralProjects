using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameGameHW
{
    internal class Collectible:GameObject
    {
        // Fields --
        private bool active;

        // Properties --

        /// <summary>
        /// True if the collectible is active on the window, false otherwise
        /// </summary>
        public bool Active { get { return active; } set { active = value; } }

        // Constructor --

        /// <summary>
        /// Creates a new collectible, active upon creation
        /// </summary>
        /// <param name="x">X-coordinate of the collectible</param>
        /// <param name="y">Y-coordinate of the collectible</param>
        /// <param name="width">Width of the collectible texture</param>
        /// <param name="height">Height of the collectible texture</param>
        /// <param name="texture">Texture of the collectible</param>
        public Collectible(int x, int y, int width, int height, Texture2D texture):
            base(x,y,width,height,texture)
        {
            active = true;
        }

        // Methods --

        /// <summary>
        /// Checks if another object is colliding with the collectible
        /// </summary>
        /// <param name="other">The other object in question</param>
        /// <returns>True if a collision is detected, false otherwise</returns>
        public bool CheckCollision(GameObject other)
        {
            // If the coin is inactive, no collision can occur
            if(active==false)
            {
                return false;
            }

            // Collision occurs
            if(this.Position.Intersects(other.Position))
            {
                return true;
            }

            // No collision occurs
            return false;
        }

        /// <summary>
        /// Draws the collectible to the window if it is active
        /// </summary>
        public override void Draw(SpriteBatch sb)
        {
            // Draws the collectible only if it is active
            if(active)
            {
                base.Draw(sb);
            }
        }
    }
}
