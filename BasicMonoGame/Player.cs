using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameGameHW
{
    internal class Player:GameObject
    {
        // Fields --
        private int levelScore;
        private int totalScore;

        // Properties --

        /// <summary>
        /// Score player achieves on the current level
        /// </summary>
        public int LevelScore { get { return levelScore; } set { levelScore = value; } }

        /// <summary>
        /// Total score player accumulates over multiple levels
        /// </summary>
        public int TotalScore { get { return totalScore; } set { totalScore = value; } }

        // Constructor --

        /// <summary>
        /// Creates a new player with an initial levelScore and totalScore of 0
        /// </summary>
        /// <param name="x">X-position of the player</param>
        /// <param name="y">Y-position of the player</param>
        /// <param name="width">Width of the player</param>
        /// <param name="height">Height of the player</param>
        /// <param name="texture">Texture used to draw the player</param>
        public Player(int x, int y, int width, int height, Texture2D texture):
            base(x, y, width, height, texture)
        {
            levelScore = 0;
            totalScore = 0;
        }

        // Methods --

        /// <summary>
        /// Changes the player's coordinates based on user-input
        /// </summary>
        /// <param name="gameTime">Game timer</param>
        public override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();

            // Directional user-input to move the player
            if(kb.IsKeyDown(Keys.W))
            {
                base.Y -= 5;
                ScreenWrap();
            }

            if(kb.IsKeyDown(Keys.A))
            {
                base.X -= 5;
                ScreenWrap();
            }

            if(kb.IsKeyDown(Keys.S))
            {
                base.Y += 5;
                ScreenWrap();
            }

            if(kb.IsKeyDown(Keys.D))
            {
                base.X += 5;
                ScreenWrap();
            }
        }

        /// <summary>
        /// Checks for collisions between the player and the edge of the screen 
        /// and adjusts player position accordingly
        /// </summary>
        public void ScreenWrap()
        {
            // Left
            if(base.X<=0)
            {
                base.X = 800 - base.Width;
            }

            // Right
            if(base.X>=800)
            {
                base.X = 5;
            }

            // Top
            if(base.Y<=0)
            {
                base.Y = 600 - base.Height;
            }

            // Bottom
            if(base.Y>=600)
            {
                base.Y = 5;
            }
        }
    }
}
