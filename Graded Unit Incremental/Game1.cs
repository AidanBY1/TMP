using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Graded_Unit_Incremental
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // structures initialisation 

        struct graphic2d
        {
            Texture2D backgroundart;
            Rectangle rect;
            float size;

        }

        struct sprite2d
        {
            Texture2D imagechar;
            Vector2 origin;
            Vector3 position;
            Rectangle rect;
            float size;
            string direction; //decides which version of the character will be drawn, if he is facing forward, backwards or to the side
            BoundingBox bbox;
            bool moving; // Decides whether the character will be drawn as a PNG or a Gif (basically if they are moving or not)
        }

        struct interactable
        {
            Texture2D imageobject;
            Vector3 position;
            Vector2 origin;
            Rectangle rect;
            float size;
            BoundingBox bbox_interactable;

        }

        struct text_box
        {
            Texture2D UI;
            Vector3 position;
            Vector2 origin;
            Vector3 size;
            Rectangle rect;
            string[] preview; // the preview text for the information given
            string[] information; // the information text itself
            string levelOption; // the level names

        }

        //global or overlapping variables
        string level;
        graphic2d background;
        sprite2d character;
        int displayheight;
        int displaywidth;
        string mode; //decides on the mode the game is in, IE if you're using questions then it will load questions. Allows us to play with visibility, as eveything can be loaded at once but only interactable if the mode is set. IE question boxes can only be clicked on if mode = questions
        int lives;
        string win; // done as a string instead of a bool because that allows me to load the menu. If you've neither won nor lost but the game is over then the main menu is seen
        bool gameover; // decides if the gameover is true or not.

        // the text boxes that will be implemented. its easier to manage if there are several
        // text boxes A through C will be used for questions, interactables and level selection.
        text_box option_A;
        text_box Option_B;
        text_box option_C;
        text_box information; // a larger text box to be used for exposition when you want to learn about something


        // the variables to be used in the questions portion of the game
        int current_question; // keeps track of the current question, will be used for location in questions and correct_answers
        string[] questions = new string[15]; // an array of all the question text
        string[] correct_answers = new string[15]; //an array of all the correct answers
        string current_answer; //keeps track of what your answer is
        bool chosen; //will track if the player has chosen an answer

        //variables to be used in the exploration section

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
            displaywidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            displayheight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            
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

            base.Draw(gameTime);
        }
    }
}
