using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ZombiePong
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background, spritesheet;

        Sprite paddle1, paddle2, ball;

        //scores
        int score1 = 0;
        int score2 = 0;

        List<Sprite> zombies = new List<Sprite>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();
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
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            background = Content.Load<Texture2D>("background");
            spritesheet = Content.Load<Texture2D>("spritesheet");

            paddle1 = new Sprite(new Vector2(20, 20), spritesheet, new Rectangle(0, 516, 25, 150), Vector2.Zero);
            paddle2 = new Sprite(new Vector2(970, 20), spritesheet, new Rectangle(32, 516, 25, 150), Vector2.Zero);
            ball = new Sprite(new Vector2(700, 350), spritesheet, new Rectangle(76, 510, 40, 40), new Vector2(200, 60));

            //Spawn zombie
            SpawnZombie(new Vector2(600, 100), new Vector2(-20, 0));
            SpawnZombie(new Vector2(100, 600), new Vector2(20, 0));
            SpawnZombie(new Vector2(100, 100), new Vector2(50, 0));
            SpawnZombie(new Vector2(600, 600), new Vector2(-50, 0));


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void SpawnZombie(Vector2 location, Vector2 velocity)
        {
            Sprite zombie = new Sprite(location, spritesheet, new Rectangle(0, 25, 160, 150), velocity);

            for (int i = 1; i < 10; i++)
            {
                zombie.AddFrame(new Rectangle(i * 165, 25, 160, 150));
            }

            zombies.Add(zombie);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            //Window Title
            Window.Title = "Player 1: " + score1 + " Player 2: " + score2;

            //Reflections
            if (paddle1.IsBoxColliding(ball.BoundingBoxRect))
            {
                ball.Velocity *= new Vector2(-1, 1);
            }

            if (paddle2.IsBoxColliding(ball.BoundingBoxRect))
            {
                ball.Velocity *= new Vector2(-1, 1);
            }

            //If the AI scores, the AI gets a point and the ball respawns and goes toward the AI.
            if (ball.Location.X < -32)
            {
                ball.Location = new Vector2(512, 384);
                ball.Velocity = new Vector2(200,60);
                score2++;
            }

            //If you score, you get a point and the ball respawns and goes toward you.
            if (ball.Location.X > 1010)
            {
                ball.Location = new Vector2(512, 384);
                ball.Velocity = new Vector2(-200, -60);
                score1++;

            }

            if (ball.Location.Y < 0 || ball.Location.Y + ball.BoundingBoxRect.Height > this.Window.ClientBounds.Height)
                ball.Velocity *= new Vector2(1, -1);

                //Mouse to move.  Also to prevent the paddle from going off screen.
                MouseState ms = Mouse.GetState();
            paddle1.Location = new Vector2(paddle1.Location.X, MathHelper.Clamp(ms.Y, 0, (float)this.Window.ClientBounds.Height-paddle1.BoundingBoxRect.Height));
            paddle2.Location = new Vector2(paddle2.Location.X, MathHelper.Clamp(ball.Center.Y, 0, (float)this.Window.ClientBounds.Height - paddle1.BoundingBoxRect.Height));


            // TODO: Add your update logic here
            ball.Update(gameTime);

            for (int i = 0; i < zombies.Count; i++)
            {
                zombies[i].Update(gameTime);

                //Direction zombies face.
                // Zombie logic goes here.. 
                if (zombies[i].Velocity.X < 0)
                    zombies[i].FlipHorizontal = false;

                if (zombies[i].Velocity.X > 0)
                    zombies[i].FlipHorizontal = true;


                if (zombies[i].Location.X < 100 || zombies[i].Location.X > 700)
                    zombies[i].Velocity *= new Vector2(-1, 1);

                zombies[i].CollisionRadius = 25;
                if (zombies[i].IsCircleColliding(ball.Center, 20))
                    ball.Velocity *= new Vector2(-1, -1);

            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            
            spriteBatch.Draw(background, Vector2.Zero, Color.White);

            paddle1.Draw(spriteBatch);
            paddle2.Draw(spriteBatch);
            ball.Draw(spriteBatch);

            for (int i = 0; i < zombies.Count; i++)
            {
                zombies[i].Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
