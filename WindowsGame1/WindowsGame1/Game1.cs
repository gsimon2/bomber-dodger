using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;





/* GAME DESCIPRION. 
 * 
 * 
 */
namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //effects how the bombs bounce off things
        //increase to make objects come closer before detecting a Collision
        //decrease to increase the gap between objects when detecting a collision
        const int boundingFactor = 12;

        Texture2D background;
        Rectangle mainFrame;

        Texture2D PlayerAnimation;
        Vector2 playerPosition = new Vector2(200 ,200);
        Vector2 PlayerDrawPosition;
        int PlayerHeight = 280;
        int PlayerWidth = 200;
        int PlayerXCenter = 120;
        int PlayerTop = 0; // 0 for moving right, 280 for moving right
        Rectangle DisplayedPlayer;
        float elapsed = 0f;
        float delay = 100f;  //delay between changing animations in miliseconds
        int frames = 0;
        float SoundElapsed = 0;
        string strScore = "Score: ";
        SpriteFont Font1;
        SpriteFont Font2;
        int CurrentScore = 0;
        int BombSpeedFactor = 5;
        int playerLives = 3;
        bool gameOver = false;

        Texture2D heart;


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D bomb1;
        Texture2D bomb2;

        Texture2D sun;
        Texture2D tree;
        Texture2D goldCoin;
        int goldCoinWidth;
        int goldCoinHeight;

        Vector2 sunPosition;
        Vector2 treePosition;

        KeyboardState keyState;
        Vector2 bombPosition1;
        Vector2 bombPosition2;
        Vector2 goldCoinposition;
        float bombAngle1 = 0;
        float bombAngle2 = 0;
        Vector2 bombSpeed1 = new Vector2(175.0f, 175.0f);
        Vector2 bombSpeed2 = new Vector2(250.0f, 250.0f);
        int bomb1Height;  
        int bomb1Width;
        int bomb2Height;
        int bomb2Width;

        int sunHeight;
        int sunWidth;

        int TreeHeight;
        int TreeWidth;

        SoundEffect soundEffect;
        SoundEffect soundEffect2;

        Song backgroundMusic;

        Texture2D explosion;
        int explosionWidth;
        int explosionHeight;
        Vector2 explosionPosition;
        bool bomb1explode;
        bool bomb2explode;
        float explodetime = 0f;
        int HighScore = 0;
        Vector2 tempspeed1;
        Vector2 tempspeed2;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            keyState = Keyboard.GetState();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures
            spriteBatch = new SpriteBatch(GraphicsDevice);


            // Load background content
            background = Content.Load<Texture2D>("Background1");
            mainFrame = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            // load animated walking
            PlayerAnimation = Content.Load<Texture2D>("scottpilgrim_multiple_scaled");
            DisplayedPlayer = new Rectangle(PlayerXCenter - PlayerWidth/2, 0, PlayerWidth, PlayerHeight);

            //font for displaying score
            Font1 = Content.Load<SpriteFont>("Score");

            //font for displaying end of game text
            Font2 = Content.Load<SpriteFont>("EndText");

            // Load hearts
            heart = Content.Load<Texture2D>("heart");

            bomb1 = Content.Load<Texture2D>("bomb_trimmed");
            bomb2 = Content.Load<Texture2D>("bomb_trimmed");


            goldCoin = Content.Load<Texture2D>("goldCoin1");
            goldCoinWidth = goldCoin.Bounds.Width;
            goldCoinHeight = goldCoin.Bounds.Height;
            sun = Content.Load<Texture2D>("sun");
            tree = Content.Load<Texture2D>("tree");
            explosion = Content.Load<Texture2D>("explosion");

            //load sound info
            soundEffect = Content.Load<SoundEffect>("Bomb_Exploding");
            soundEffect2 = Content.Load<SoundEffect>("Windows Logon");
            backgroundMusic = Content.Load<Song>("StarWarsMusic");
            MediaPlayer.Volume = .2f;
            MediaPlayer.Play(backgroundMusic);

            SoundElapsed = ((float)soundEffect.Duration.Milliseconds + ((float)soundEffect.Duration.Seconds) * 1000);


            goldCoinposition.X = 650;
            goldCoinposition.Y = 100;
            sunPosition.X = 100;
            sunPosition.Y = 100;

            treePosition.X = 100;
            treePosition.Y = 380;


            bombPosition1.X = 250;
            bombPosition1.Y = 100;

            bombPosition2.X = graphics.GraphicsDevice.Viewport.Width - bomb2.Width/2;
            bombPosition2.Y = graphics.GraphicsDevice.Viewport.Height - bomb2.Height/2;
            bomb1Height = bomb1.Bounds.Height;
            bomb1Width = bomb1.Bounds.Width;
            bomb2Height = bomb2.Bounds.Height;
            bomb2Width = bomb2.Bounds.Width;

            explosionWidth = explosion.Bounds.Width;
            explosionHeight = explosion.Bounds.Height;
            bomb1explode = false;
            bomb2explode = false;

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            // Allow the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                this.Exit();
           
            //Update Player Animatin
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            SoundElapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            explodetime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(elapsed >= delay)
            {
                UpdatePlayerAnimation();
            }

            //Update angles
            bombAngle1 += 0.02f;
            bombAngle2 -= 0.015f;

             // Move the sprite around
            UpdateSprite(gameTime, ref bombPosition1, ref bombSpeed1);
            UpdateSprite(gameTime, ref bombPosition2, ref bombSpeed2);
            UpdateInput();
            CheckForCollision();
            if ((bomb1explode || bomb2explode) && (!gameOver))
                Explode();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);


            // Draw the background
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            spriteBatch.Draw(background, mainFrame, Color.White);
            spriteBatch.End();

            // Draw Score Text
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            spriteBatch.DrawString(Font1, strScore + CurrentScore, new Vector2(675, 450), Color.White);
            spriteBatch.End();

            // Draw hearts for lives
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            if (playerLives >= 1)
                spriteBatch.Draw(heart, new Vector2(575, 450), Color.White);
            if (playerLives >= 2)
                spriteBatch.Draw(heart, new Vector2(600, 450), Color.White);
            if (playerLives >= 3)
                spriteBatch.Draw(heart, new Vector2(625, 450), Color.White);
            spriteBatch.End();

            // Draw the Sun
            //spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            //Vector2 sunOrigin = new Vector2(sun.Width / 2, sun.Height / 2);
            //spriteBatch.Draw(sun, sunPosition, null, Color.White, 0, sunOrigin, .2f, SpriteEffects.None, 1);
            //spriteBatch.End();


            // Draw the gold coin
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            Vector2 goldCoinOrigin = new Vector2(goldCoin.Width / 2, goldCoin.Height / 2);
            spriteBatch.Draw(goldCoin, goldCoinposition, null, Color.White, 0, goldCoinOrigin, .6f, SpriteEffects.None, 1);
            spriteBatch.End();

            //Draw tree
            /*
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            Vector2 treeOrigin = new Vector2(tree.Width / 2, tree.Height / 2);
            spriteBatch.Draw(tree, treePosition, null, Color.White, 0, treeOrigin, .6f, SpriteEffects.None, 1);
            spriteBatch.End();
             * */



            //Animated Walking
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            Vector2 origin = new Vector2(PlayerAnimation.Width / 2, PlayerAnimation.Height / 2);
            PlayerDrawPosition = new Vector2(playerPosition.X + PlayerWidth / boundingFactor + 120, playerPosition.Y + PlayerHeight / boundingFactor);
            spriteBatch.Draw(PlayerAnimation, PlayerDrawPosition, DisplayedPlayer, Color.White, 0, origin, .2f, SpriteEffects.None, 1);
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            origin = new Vector2(bomb1.Width / 2, bomb1.Height / 2);
            spriteBatch.Draw(bomb1, bombPosition1, null, Color.White, bombAngle1, origin, .2f, SpriteEffects.None, 1);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            origin = new Vector2(bomb2.Width / 2, bomb2.Height / 2);
            spriteBatch.Draw(bomb2, bombPosition2, null, Color.Gray, bombAngle2, origin, .2f, SpriteEffects.None, 1);
            spriteBatch.End();

            //Draw end of game text
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            if (playerLives <= 0)
            {
                if (CurrentScore > HighScore)
                    HighScore = CurrentScore;
                spriteBatch.DrawString(Font2, "Game Over!!\nTotal Score: " + CurrentScore, new Vector2(200, 100), Color.Crimson);
                spriteBatch.DrawString(Font2, "High Score: " + HighScore, new Vector2(200, 250), Color.Crimson);
                spriteBatch.DrawString(Font2, "Press Both Side Arrows to Restart.", new Vector2(200, 300), Color.Crimson);
            }
            spriteBatch.End();

            if(bomb1explode || bomb2explode)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                Vector2 explosionOrigin = new Vector2(explosion.Width / 2, explosion.Height / 2);
                spriteBatch.Draw(explosion, explosionPosition, null, Color.White, 0, explosionOrigin, .6f, SpriteEffects.None, 1);
                spriteBatch.End();
            }
            
            base.Draw(gameTime);
        }

        void UpdateSprite(GameTime gameTime, ref Vector2 spritePosition, ref Vector2 spriteSpeed)
        {
            // Move the sprite by speed, scaled by elapsed time 
            spritePosition += spriteSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            int MaxX = graphics.GraphicsDevice.Viewport.Width - bomb1.Width / boundingFactor;
            int MinX = bomb1.Width / boundingFactor;
            int MaxY = graphics.GraphicsDevice.Viewport.Height - bomb1.Height / boundingFactor;
            int MinY = bomb1.Height / boundingFactor;

            // Check for bounce 
            if (spritePosition.X > MaxX)
            {
                spriteSpeed.X *= -1;
                spritePosition.X = MaxX;
            }
            else if (spritePosition.X < MinX)
            {
                spriteSpeed.X *= -1;
                spritePosition.X = MinX;
            }
            if (spritePosition.Y > MaxY)
            {
                spriteSpeed.Y *= -1;
                spritePosition.Y = MaxY;
            }
            else if (spritePosition.Y < MinY)
            {
                spriteSpeed.Y *= -1;
                spritePosition.Y = MinY;
            }
        }

        private void UpdateInput()
        {

            int MaxX = graphics.GraphicsDevice.Viewport.Width - (PlayerWidth / boundingFactor);
            int MinX = PlayerWidth / boundingFactor;
            int MaxY = graphics.GraphicsDevice.Viewport.Height - (PlayerHeight / boundingFactor);
            int MinY = PlayerHeight / boundingFactor;


            keyState = Keyboard.GetState();

            if (!gameOver)
            {
                if (keyState.IsKeyDown(Keys.Left) && playerPosition.X >= MinX)
                {
                    playerPosition.X -= 6.0f;
                    PlayerTop = 280;
                    DisplayedPlayer = new Rectangle(PlayerXCenter - PlayerWidth / 2, PlayerTop, PlayerWidth, PlayerHeight);
                }
                if (keyState.IsKeyDown(Keys.Right) && playerPosition.X <= MaxX)
                {
                    playerPosition.X += 6.0f;
                    PlayerTop = 0;
                    DisplayedPlayer = new Rectangle(PlayerXCenter - PlayerWidth / 2, PlayerTop, PlayerWidth, PlayerHeight);
                }
                if (keyState.IsKeyDown(Keys.Up) && playerPosition.Y >= MinY)
                {
                    playerPosition.Y -= 6.0f;
                }
                if (keyState.IsKeyDown(Keys.Down) && playerPosition.Y <= MaxY)
                {
                    playerPosition.Y += 6.0f;
                }

                //detect not moving
                if (!keyState.IsKeyDown(Keys.Down) && !keyState.IsKeyDown(Keys.Up) && !keyState.IsKeyDown(Keys.Right) && !keyState.IsKeyDown(Keys.Left))
                {
                    frames = 0;
                    elapsed = 0f;
                }
            }
            if (gameOver)
            {
                if (keyState.IsKeyDown(Keys.Left) && keyState.IsKeyDown(Keys.Right))
                 {
                     gameOver = false;
                     Restart();                     
                 }
            }
        }

        void CheckForCollision()
        {
            BoundingBox Bomb1BoundingBox = new BoundingBox(new Vector3(bombPosition1.X - (bomb1Width / boundingFactor), bombPosition1.Y - (bomb1Height / boundingFactor), 0), new Vector3(bombPosition1.X + (bomb1Width / boundingFactor), bombPosition1.Y + (bomb1Height / boundingFactor), 0));
            BoundingBox Bomb2BoundingBox = new BoundingBox(new Vector3(bombPosition2.X - (bomb2Width / boundingFactor), bombPosition2.Y - (bomb2Height / boundingFactor), 0), new Vector3(bombPosition2.X + (bomb2Width / boundingFactor), bombPosition2.Y + (bomb2Height / boundingFactor), 0));
            BoundingBox PlayerBoundingBox = new BoundingBox(new Vector3(playerPosition.X - (PlayerWidth / boundingFactor), playerPosition.Y - (PlayerHeight / boundingFactor), 0), new Vector3(playerPosition.X + (PlayerWidth / boundingFactor), playerPosition.Y + (PlayerHeight / boundingFactor), 0));
            BoundingBox goldCoinBoundingBox = new BoundingBox(new Vector3(goldCoinposition.X - (goldCoinWidth / boundingFactor), goldCoinposition.Y - (goldCoinHeight / boundingFactor), 0), new Vector3(goldCoinposition.X + (goldCoinWidth / boundingFactor), goldCoinposition.Y + (goldCoinHeight / boundingFactor), 0));

            
            //bombs collide
            if (Bomb1BoundingBox.Intersects(Bomb2BoundingBox))
            {
                bombSpeed1 *= -1;
                bombSpeed2 *= -1;
            }

            //bombs and player collide
            if (Bomb1BoundingBox.Intersects(PlayerBoundingBox) && !bomb2explode && !bomb1explode)
            {
                bomb1explode = true;
                playerLives--;
                Bombdetonation();
                tempspeed1.X = bombSpeed1.X;
                tempspeed1.Y = bombSpeed1.Y;
                tempspeed2.X = bombSpeed2.X;
                tempspeed2.Y = bombSpeed2.Y;
                explodetime = 0f;
            }
            if (Bomb2BoundingBox.Intersects(PlayerBoundingBox) && !bomb2explode && !bomb1explode)
            {
                bomb2explode = true;
                playerLives--;
                Bombdetonation();
                tempspeed1.X = bombSpeed1.X;
                tempspeed1.Y = bombSpeed1.Y;
                tempspeed2.X = bombSpeed2.X;
                tempspeed2.Y = bombSpeed2.Y;
                explodetime = 0f;
            }

            //Player and gold coin collide
            if (PlayerBoundingBox.Intersects(goldCoinBoundingBox))
            {
                Random r = new Random();
                goldCoinposition.X = r.Next(0, GraphicsDevice.Viewport.Width);
                goldCoinposition.Y = r.Next(0, GraphicsDevice.Viewport.Height);
                CurrentScore++;
                IncrementBombSpeeds();
            }
        }
        void IncrementBombSpeeds()
        {
            if (bombSpeed1.X > 0)
                bombSpeed1.X += BombSpeedFactor;
            else
                bombSpeed1.X -= BombSpeedFactor;

            if (bombSpeed1.Y > 0)
                bombSpeed1.Y += BombSpeedFactor;
            else
                bombSpeed1.Y -= BombSpeedFactor;
            if (bombSpeed2.X > 0)
                bombSpeed2.X += BombSpeedFactor;
            else
                bombSpeed2.X -= BombSpeedFactor;

            if (bombSpeed2.Y > 0)
                bombSpeed2.Y += BombSpeedFactor;
            else
                bombSpeed2.Y -= BombSpeedFactor;
        }

        void UpdatePlayerAnimation()
        {
            elapsed = 0f;
            if (frames >= 7)
                frames = 0;
            else
                frames++;

            switch (frames)
            {
                case 0:
                    PlayerXCenter = 120;
                    break;
                case 1:
                    PlayerXCenter = 336;
                    break;
                case 2:
                    PlayerXCenter = 548;
                    break;
                case 3:
                    PlayerXCenter = 763;
                    break;
                case 4:
                    PlayerXCenter = 971;
                    break;
                case 5:
                    PlayerXCenter = 1186;
                    break;
                case 6:
                    PlayerXCenter = 1409;
                    break;
                case 7:
                    PlayerXCenter = 1626;
                    break;
            }

            DisplayedPlayer = new Rectangle(PlayerXCenter - PlayerWidth / 2, PlayerTop, PlayerWidth, PlayerHeight);
        }

        void Bombdetonation()
        {
            //Prevents sound from being played again until the last instance is almost done
            if (SoundElapsed > ((float)soundEffect.Duration.Milliseconds + ((float)soundEffect.Duration.Seconds - 2) * 1000))
            {
                if(!soundEffect.IsDisposed)
                    soundEffect.Play((float)0.8, 0, 1);
                SoundElapsed = 0;
            }

            
            
        }

       void Explode()
        {
           if (explodetime > 1000)
           {
               bomb1explode = false;
               bomb2explode = false;
               bombSpeed1.X = tempspeed1.X;
               bombSpeed1.Y = tempspeed1.Y;
               bombSpeed2.X = tempspeed2.X;
               bombSpeed2.Y = tempspeed2.Y;   
           }
           else
           {
               bombSpeed1 = new Vector2(0.0f, 0.0f);
               bombSpeed2 = new Vector2(0.0f, 0.0f);
               if (bomb1explode)
               {
                   explosionPosition.X = bombPosition1.X;
                   explosionPosition.Y = bombPosition1.Y;
               }
               if (bomb2explode)
               {
                   explosionPosition.X = bombPosition2.X;
                   explosionPosition.Y = bombPosition2.Y;
               }        
           }
           if (playerLives <= 0)
           {
               //Do something with ending game
               bombSpeed1 = new Vector2(0, 0);
               bombSpeed2 = new Vector2(0, 0);
               goldCoinposition = new Vector2(2000, 2000);
               gameOver = true;
           }
            
        }
        void Restart()
       {
           MediaPlayer.Stop();
           MediaPlayer.Play(backgroundMusic);
           playerLives = 3;
           CurrentScore = 0;
           playerPosition = new Vector2(50, 50);
           Random r = new Random();
           goldCoinposition.X = r.Next(0, GraphicsDevice.Viewport.Width);
           goldCoinposition.Y = r.Next(0, GraphicsDevice.Viewport.Height);

           bombPosition1.X = 250;
           bombPosition1.Y = 100;

           bombPosition2.X = graphics.GraphicsDevice.Viewport.Width - bomb2.Width / 2;
           bombPosition2.Y = graphics.GraphicsDevice.Viewport.Height - bomb2.Height / 2;

           float bombAngle1 = 0;
           float bombAngle2 = 0;
           Vector2 bombSpeed1 = new Vector2(175.0f, 175.0f);
           Vector2 bombSpeed2 = new Vector2(250.0f, 250.0f);
       }

 
    }

}
