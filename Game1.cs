using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;

namespace Scottish_Revolution_1._2
{
    /// <summary>
    /// Callum O'Connor     Movement & Collisions       26/05/2020
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        struct Sprite2D
        {
            public Texture2D Image;
            public Rectangle Rect;
            public Vector3 Position;
            public Vector2 Origin;
            public Vector3 Velocity;
            public float Size;
            public BoundingBox B_Box;

            // Constructor Method
            public Sprite2D(ContentManager content, string FileName, float SizeRatio)
            {
                Image = content.Load<Texture2D>(FileName);
                Size = SizeRatio;
                Origin.X = Image.Width / 2;
                Origin.Y = Image.Height / 2;
                Rect.Width = (int)(Image.Width * Size);
                Rect.Height = (int)(Image.Height * Size);

                // The rest of the variables are assigned intial values.
                B_Box = new BoundingBox(Vector3.Zero, Vector3.Zero);
                Position = Vector3.Zero;
                Velocity = Vector3.Zero;
                Rect.X = 0;
                Rect.Y = 0;
            }
        }

        // Initailizing & Declaring Variables
        Sprite2D Player, DisplayCase;
        int DisplayWidth, DisplayHeight;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
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

            DisplayWidth = graphics.GraphicsDevice.Viewport.Width;
            DisplayHeight = graphics.GraphicsDevice.Viewport.Height;

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

            // TODO: use this.Content to load your game content here

            Player = new Sprite2D(Content, "PlayerSt", 2.5f);
            DisplayCase = new Sprite2D(Content, "DisplayTable", 2.5f);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            KeyboardState KeyPress = Keyboard.GetState();
            int PlayerSpeed = 5;

            // Player Movement
            if (KeyPress.IsKeyDown(Keys.Up)) Player.Position.Y -= PlayerSpeed;
            if (KeyPress.IsKeyDown(Keys.Left)) Player.Position.X -= PlayerSpeed;

            if (KeyPress.IsKeyDown(Keys.Down)) Player.Position.Y += PlayerSpeed;
            if (KeyPress.IsKeyDown(Keys.Right)) Player.Position.X += PlayerSpeed;

            // SetUp Rect Values
            Player.Rect.X = (int)Player.Position.X;
            Player.Rect.Y = (int)Player.Position.Y;

            DisplayCase.Rect.X = (int)DisplayCase.Position.X;
            DisplayCase.Rect.Y = (int)DisplayCase.Position.Y;

            // SetUp Bounding Boxes
            Player.B_Box = new BoundingBox(new Vector3(Player.Position.X - (Player.Rect.Width / 2), Player.Position.Y - (Player.Rect.Height / 2), 0), new Vector3(Player.Position.X + (Player.Rect.Width / 2), Player.Position.Y + (Player.Rect.Height / 2), 0));

            DisplayCase.B_Box = new BoundingBox(new Vector3(DisplayCase.Position.X - (DisplayCase.Rect.Width / 2), DisplayCase.Position.Y - (DisplayCase.Rect.Height / 2), 0), new Vector3(DisplayCase.Position.X + (DisplayCase.Rect.Width / 2), DisplayCase.Position.Y + (DisplayCase.Rect.Height / 2), 0));

            // Collision - with the edge of the screen.
            if (Player.Position.Y <= (Player.Rect.Height / 2)) Player.Position.Y = Player.Rect.Height / 2;
            if (Player.Position.Y >= DisplayHeight - (Player.Rect.Height / 2)) Player.Position.Y = DisplayHeight - (Player.Rect.Height / 2);

            if (Player.Position.X <= (Player.Rect.Width / 2)) Player.Position.X = Player.Rect.Width / 2;
            if (Player.Position.X >= DisplayHeight - (Player.Rect.Width / 2)) Player.Position.X = DisplayHeight - (Player.Rect.Width / 2);

            // Collision - with other objects.
            if (Player.B_Box.Intersects(DisplayCase.B_Box)) // Insert question code
            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            spriteBatch.Draw(Player.Image, Player.Rect, null, Color.White, 0, Player.Origin, SpriteEffects.None, 0);

            spriteBatch.Draw(DisplayCase.Image, DisplayCase.Rect, null, Color.White, 0, DisplayCase.Origin, SpriteEffects.None, 0);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
