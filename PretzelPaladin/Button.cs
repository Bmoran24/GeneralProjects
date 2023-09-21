using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PretzelPaladin
{
    /// <summary>
    /// Mackenna Roberts
    /// 3/6/23
    /// Class designed to make a button
    /// </summary>
    internal class Button
    {
        // Fields --
        private int x;
        private int y;
        private int width;
        private int height;
        private Rectangle rect;
        private Texture2D image;
        private MouseState ms;
        private MouseState lastMS;
        private bool isEnabled;
        private string text;
        private string stamina;
        private int damage;
        private Move move;
        // Constructors --

        /// <summary>
        /// Creates a new clickable button
        /// </summary>
        /// <param name="x">X-location</param>
        /// <param name="y">Y-location</param>
        /// <param name="width">Width of the button</param>
        /// <param name="height">Height of the button</param>
        /// <param name="image">Texture of the button</param>
        public Button(int x, int y, int width, int height, Texture2D image)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            rect = new Rectangle(x, y, width, height);
            this.image = image;
            isEnabled = false;
        }

        /// <summary>
        /// Second Constructor that corresponds the button with a move
        /// </summary>
        /// <param name="x">X-Coordinate location of button</param>
        /// <param name="y">Y-Coordinate location of button</param>
        /// <param name="width">Width of the button</param>
        /// <param name="height">Height of the button</param>
        /// <param name="image">Texture of the button</param>
        /// <param name="move">Move associated with the button</param>
        public Button(int x, int y, int width, int height, Texture2D image, Move move)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            rect = new Rectangle(x, y, width, height);
            this.image = image;
            this.text = $"{move.MoveName}";
            this.damage = move.AmountDamage;
            this.move = move;
            isEnabled = false;
        }

        /// <summary>
        /// Default button
        /// </summary>
        public Button()
        {
            isEnabled = false;
        }

        // Properties --
        /// <summary>
        /// Returns properties of the rectangle boundary of the button
        /// </summary>
        public Rectangle Location { get { return rect; } set { rect = value; } }

        /// <summary>
        /// Returns true if the button is enabled, false otherwise
        /// </summary>
        public bool Enabled { get { return isEnabled; } set { isEnabled = value; } }

        /// <summary>
        /// Returns button text
        /// </summary>
        public string Text { get { return text; } set { text = value; } }

        /// <summary>
        /// Returns the move associated with the button
        /// </summary>
        public Move Move { get { return move; } set { move = value; } }

        /// <summary>
        /// Amount of damage the move associated with the button deals
        /// </summary>
        public int Damage { get { return damage; } set { damage = value; } }

        // Methods --

        /// <summary>
        /// Tracks if the button is currently being pressed
        /// </summary>
        /// <returns>True if the button was pressed, false otherwise</returns>
        public bool IsPressed()
        {
            
            ms = Mouse.GetState();

            if(ms.X > x && ms.X < x + width && ms.Y > y && ms.Y < y + height && SingleClick()&&isEnabled)
            { 
                lastMS = ms;
                return true;
            }
            else   
            {
                lastMS = ms;
                return false; 
            }
            
        }

        /// <summary>
        /// Determines if the cursor is hovering over the button
        /// </summary>
        /// <returns>True if the mouse is hovering over the button, false otherwise</returns>
        public bool IsHover()
        {
            if (ms.X > x && ms.X < x + width && ms.Y > y && ms.Y < y + height)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Recognizes a Single Mouse Click
        /// </summary>
        /// <returns>Returns true if the player just left-clicked once, false otherwise</returns>
        public bool SingleClick()
        {
            if (ms.LeftButton == ButtonState.Released && lastMS.LeftButton == ButtonState.Pressed)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Draws the button to the window
        /// </summary>
        /// <param name="sb">Spritebatch passed in from Game1</param>
        public void Draw(SpriteBatch sb, Color color)
        {
              // Only draws the button if it's enabled
              if(isEnabled)
              {
                sb.Draw(image,
                rect,
                color);
              }
        }

        /// <summary>
        /// Draws the button with the inputted text from the second constructor
        /// </summary>
        /// <param name="sb">Spritebatch passed in from Game1</param>
        /// <param name="color">Color to draw the button</param>
        /// <param name="font">Font used to draw the text within the button</param>
        public void DrawWithText(SpriteBatch sb, Color color, SpriteFont font, Texture2D image, bool move)
        {
            if (isEnabled)
            {
                sb.Draw(image,
                rect,
                color);
                sb.DrawString(font,
                    text,
                    new Vector2(x + width/5, y + height/2 - 15),
                    Color.Black);
                
            }
        }

        
    }
}
