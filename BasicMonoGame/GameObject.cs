using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.CompilerServices;

namespace MonoGameGameHW
{
    internal class GameObject
    {
        // Fields --
        private Texture2D texture;
        private Rectangle position;

        // Properties --

        /// <summary>
        /// X-value of the texture
        /// </summary>
        public int X { get { return position.X; } set { position.X = value; } }

        /// <summary>
        /// Y-value of the texture
        /// </summary>
        public int Y { get { return position.Y; } set { position.Y = value; } }

        /// <summary>
        /// Width of the texture
        /// </summary>
        public int Width { get { return position.Width; } }

        /// <summary>
        /// Height of the texture
        /// </summary>
        public int Height { get { return position.Height; } }

        /// <summary>
        /// Position of the texture
        /// </summary>
        public Rectangle Position { get { return position; } }

        // Constructor --

        /// <summary>
        /// Creates a new GameObject with a position and a texture
        /// </summary>
        /// <param name="x">X-value of the texture</param>
        /// <param name="y">Y-value of the texture</param>
        /// <param name="width">Width of the texture</param>
        /// <param name="height">Height of the texture</param>
        /// <param name="texture">The texture of the object</param>
        public GameObject(int x, int y, int width, int height, Texture2D texture)
        {
            this.texture = texture;
            this.position = new Rectangle(x, Y, width, height);
        }

        // Methods --

        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// Draws the object's texture at specified position
        /// </summary>
        /// <param name="sb">SpriteBatch used to draw texture to the screen</param>
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Color.White);
        }
        

    }
}
