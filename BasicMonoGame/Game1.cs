using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MonoGameGameHW
{
    /// <summary>
    /// The three states of the game
    /// </summary>
    public enum GameState
    {
        Menu,
        Game,
        GameOver
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Textures
        private Texture2D collectibleTexture;
        private Texture2D playerTexture;
        private Texture2D enemyTexture;
        private SpriteFont titleText;
        private SpriteFont regularText;

        private GameState state;
        private Player player;
        private Player enemy;
        private List<Collectible> collectibles;
        private int currentLevel;
        private double timer;
        private KeyboardState previousKbState;
        KeyboardState kb;
        private Random rng;
        private int numCollectibles;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Sets the window size
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
        }

        protected override void Initialize()
        {
            rng = new Random();
            state = GameState.Menu;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            collectibleTexture=this.Content.Load<Texture2D>("Coin");
            playerTexture = this.Content.Load<Texture2D>("MarioKart");
            enemyTexture = this.Content.Load<Texture2D>("Bowser");

            titleText = this.Content.Load<SpriteFont>("TitleVerdana60");
            regularText = this.Content.Load<SpriteFont>("Verdana16");


            collectibles = new List<Collectible>();

            player =new Player(
                (_graphics.PreferredBackBufferWidth / 2) - playerTexture.Width,
                (_graphics.PreferredBackBufferHeight / 2) -playerTexture.Height,
                80,
                80,
                playerTexture);

            enemy=new Player(0,0,100,100, enemyTexture);
                
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            kb = Keyboard.GetState();

            // Traverses between different GameStates depending on user-input
            switch (state)
            {
                case GameState.Menu:
                    {
                        if(kb.IsKeyDown(Keys.Enter))
                        {
                            ResetGame();

                            state = GameState.Game;
                        }

                        break;
                    }
                case GameState.Game:
                    {
                        timer -= gameTime.ElapsedGameTime.TotalSeconds;

                        // Processes user input and keeps player visible on screen
                        player.Update(gameTime);
                        
                        // Checks if player collects a coin
                        for(int i=0; i<collectibles.Count; i++)
                        {
                            if (collectibles[i].CheckCollision(player))
                            {
                                collectibles[i].Active = false;
                                player.LevelScore += 10;
                                player.TotalScore += 10;
                            }
                        }

                        // Checks if player collided with enemy
                        if(player.Position.Intersects(enemy.Position))
                        {
                            state = GameState.GameOver;
                        }

                        if(timer<=0)
                        {
                            state = GameState.GameOver;
                        }

                        bool allCollected = true; ;
                        
                        // Checks to see if all collectibles were collected
                        for(int i=0; i<collectibles.Count; i++)
                        {
                            if (collectibles[i].Active)
                            {
                                allCollected = false;
                            }
                        }

                        if (allCollected)
                        {
                            NextLevel();
                        }
                        

                        break;
                    }
                case GameState.GameOver:
                    {
                        if (kb.IsKeyDown(Keys.Space))
                        {
                            state = GameState.Menu;
                        }

                        break;
                    }

            }

            previousKbState = kb;
        }
                        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Draws different items to the screen dependent on current GameState
            switch(state)
            {
                case GameState.Menu:
                    {
                        // Title Text
                        _spriteBatch.DrawString(
                            titleText,
                            "COIN-KART",
                            new Vector2(((_graphics.PreferredBackBufferWidth / 3)-100), _graphics.PreferredBackBufferHeight / 3),
                            Color.White);

                        // Instructional Text
                        _spriteBatch.DrawString(
                            regularText,
                            "Escape Bowser and Collect all the Coins!\n         Press ENTER to Start",
                            new Vector2((_graphics.PreferredBackBufferWidth / 4), (_graphics.PreferredBackBufferHeight / 3)+150),
                            Color.White);

                        break;
                    }
                case GameState.Game:
                    {
                        // Draws player to the window
                        player.Draw(_spriteBatch);

                        // Draws enemy to the window
                        MoveEnemy(_spriteBatch);

                        // Draws each collectible to the window
                        for(int i=0; i < collectibles.Count; i++)
                        {
                            collectibles[i].Draw(_spriteBatch);
                        }

                        // Draws U.I elements to the window
                        // - Player data
                        _spriteBatch.DrawString(
                            regularText,
                            "Current Level: " + currentLevel.ToString() + "\nScore: " + player.LevelScore.ToString(),
                            new Vector2(10, 10),
                            Color.White);

                        // - Timer
                        _spriteBatch.DrawString(
                            regularText,
                            "Time: " + String.Format("{0:0.00}", timer),
                            new Vector2(10, 60),
                            Color.White);

                        break;
                    }
                case GameState.GameOver:
                    {
                        // Presents game over message
                        _spriteBatch.DrawString(
                            titleText,
                            "GAME OVER",
                            new Vector2(((_graphics.PreferredBackBufferWidth / 3) - 100), _graphics.PreferredBackBufferHeight / 3),
                            Color.White);

                        // Displays player data
                        _spriteBatch.DrawString(
                            regularText,
                            "You Reached Level " + currentLevel + ". Total Score: " + player.TotalScore + "\nPress SPACE to return to the Main Menu",
                            new Vector2((_graphics.PreferredBackBufferWidth / 4), (_graphics.PreferredBackBufferHeight / 3) + 150),
                            Color.White) ;

                        break;
                    }                  
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Sets up and resets all necessary components for the player to move on to
        /// the next level
        /// </summary>
        public void NextLevel()
        {
            // Increments level and resets timer
            currentLevel++;
            timer = 10;
            player.LevelScore = 0;
            enemy.X = 0;

            // Places player at the center of the screen
            player.X = (_graphics.PreferredBackBufferWidth / 2) - player.Width;
            player.Y = (_graphics.PreferredBackBufferHeight / 2) - player.Height;

            // Clears list of collectibles and increases the amount of them for
            // the next level
            if(collectibles.Count>0)
            {
                numCollectibles = collectibles.Count;
            }
            else
            {
                numCollectibles = 5;
            }
            
            collectibles.Clear();
            numCollectibles += 3;

            // Adds collectibles for next level to a list with random positions
            for(int i=0; i<numCollectibles; i++)
            {
                int x = rng.Next(0, _graphics.PreferredBackBufferWidth - 50);
                int y = rng.Next(0, _graphics.PreferredBackBufferHeight - 50);

                Collectible temp = new Collectible(x,y,40,40,collectibleTexture);
                temp.Y = rng.Next(0, _graphics.PreferredBackBufferHeight-50);

                collectibles.Add(temp);
            }
        }

        /// <summary>
        /// Sets up initial state of the game when player moves from
        /// the Menu to the Game
        /// </summary>
        public void ResetGame()
        {
            currentLevel = 0;
            player.TotalScore = 0;
            enemy.X = 0;
            collectibles.Clear();
            NextLevel();
        }

        /// <summary>
        /// Checks to see if the key in question was pressed this frame and not
        /// pressed the last frame
        /// </summary>
        /// <param name="key">The key in question</param>
        /// <returns>True if the key was just pressed in the current frame, false otherwise</returns>
        public bool SingleKeyPress(Keys key)
        {

            if (kb.IsKeyDown(key) && previousKbState.IsKeyUp(key))
            {
                return true;
            }

            return false;
        }

        public void MoveEnemy(SpriteBatch sb)
        {
            // Moves enemy towards the player
            if(enemy.X>player.X)
            {
                enemy.X -= 2;
            }
            else
            {
                enemy.X += 2;
            }

            if(enemy.Y>player.Y)
            {
                enemy.Y -= 2;
            }
            else
            {
                enemy.Y += 2;
            }

            enemy.Draw(sb);
        }

    }
}