using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Graded_Unit
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        }


        // structures initialisation 

        struct graphic2d
        {
            public Texture2D stirling;
            public Texture2D edinburgh;
            public Texture2D loch_ness;
            public Rectangle rect;

        }

        struct sprite2d
        {
            public Texture2D imageL;
            public Texture2D imageR;
            public Vector2 origin;
            public Vector3 position;
            public Rectangle rect;
            public float size;
            public string direction; //decides which version of the character will be drawn, if he is facing forward, backwards or to the side
            public BoundingBox bbox;
            public bool moving; // Decides whether the character will be drawn as a PNG or a Gif (basically if they are moving or not)
            public string foot; //decides which foot is being put forward, used to create the illusion of animation


        }

        struct interactable
        {
            public Texture2D imageobject;
            public Vector3 position;
            public Vector2 origin;
            public Rectangle rect;
            public float size;
            public BoundingBox bbox_interactable;

        }

        struct text_box
        {
            public Texture2D UI;
            public Vector2 position;
            public int size;
            public Rectangle rect;
            public string[] preview; // the preview text for the information given
            public string[] information1; // the information text itself
            public string[] information2;  // If the information text is too long for a single line, draw a second one.
            public string[] answer;
            public string levelOption; // the level names
            public Vector2 textposition; // where the text position lies
            public Vector2 textposition2; // If a second line is needed
        }

        //global or overlapping variables
        string level;
        graphic2d background;
        sprite2d character;
        int displayheight;
        int displaywidth;
        string mode; //decides on the mode the game is in, IE if you're using questions then it will load questions. Allows us to play with visibility, as eveything can be loaded at once but only interactable if the mode is set. IE question boxes can only be clicked on if mode = questions
        int lives;
        bool win; // decides whether or not the game is won or lost
        bool gameover; // decides if the gameover is true or not.
        SpriteFont font;
        int score; //keeps track of what the number of right questions is, if it =5 then you win the level


        // the text boxes that will be implemented. its easier to manage if there are several
        // text boxes A through C will be used for questions, interactables and level selection.
        text_box option_A;
        text_box option_B;
        text_box option_C;
        text_box question_box;

        // the variables to be used in the questions portion of the game
        int current_question; // keeps track of the current question, will be used for location in questions and correct_answers
        string[] questions = new string[15]; // an array of all the question text
        string[] correct_answers = new string[15]; //an array of all the correct answers
        string current_answer; //keeps track of what your answer is
        bool chosen; //will track if the player has chosen an answer
        int extra; //I will be keeping all questions and information in 1 big array, and will find them using this. IE stirling is questions 0-4, therefore extra = 0 for stirling.
        float timesince; //keeps track of the time since a chosen answer, done to stop someone accidentally clicking a wrong answer
        float timechosen = 0; //Variable that tracks the time at which an answer was chosen.
        float timesincestep = 0; // Controls the time since last being told the controls
        float timechosenwrong; //time since a wrong answer was chosen, done to provide hint messages. 
        int current_hint; //keeps track of which interactable is being used
        int track = 0;

        //variables to be used in the exploration section

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            displaywidth = graphics.GraphicsDevice.Viewport.Width;
            displayheight = graphics.GraphicsDevice.Viewport.Height;

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

            font = Content.Load<SpriteFont>("Quartz4");

            background.stirling = Content.Load<Texture2D>("Placeholder");
            background.edinburgh = Content.Load<Texture2D>("Edinburgh  Placeholder");
            background.loch_ness = Content.Load<Texture2D>("Loch Ness  Placeholder");

            float ratio = ((float)displaywidth / background.stirling.Width);

            background.rect.Width = displaywidth;
            background.rect.Height = (int)(background.stirling.Height * ratio);
            background.rect.X = 0;
            background.rect.Y = ((displayheight - background.rect.Height) / 2);

            character.imageL = Content.Load<Texture2D>("PlayerL");
            character.imageR = Content.Load<Texture2D>("PlayerR");
            character.rect.Height = displayheight / 10;
            character.rect.Width = displaywidth / 20;

            //loading all the information for the text boxes
            option_A.UI = Content.Load<Texture2D>("Text_Box");
            option_A.rect.Height = displayheight / 3;
            option_A.rect.Width = displaywidth / 3;
            option_A.position.Y = displayheight - 500;
            option_A.position.X = 0;
            option_A.levelOption = "Stirling";
            option_A.textposition.X = option_A.position.X + 30;
            option_A.textposition.Y = option_A.position.Y + 50;
            option_A.preview = new string[15];
            option_A.answer = new string[15];
            option_A.information1 = new string[5];
            option_A.information2 = new string[5];

            option_B.UI = Content.Load<Texture2D>("Text_Box");
            option_B.rect.Height = displayheight / 3;
            option_B.rect.Width = displaywidth / 3;
            option_B.position.Y = displayheight - 500;
            option_B.position.X = displaywidth / 3;
            option_B.levelOption = "Edinburgh";
            option_B.textposition.X = option_B.position.X + 30;
            option_B.textposition.Y = option_B.position.Y + 50;
            option_B.preview = new string[15];
            option_B.answer = new string[15];
            option_B.information1 = new string[5];
            option_B.information2 = new string[5];

            option_C.UI = Content.Load<Texture2D>("Text_Box");
            option_C.rect.Height = displayheight / 3;
            option_C.rect.Width = displaywidth / 3;
            option_C.position.Y = displayheight - 500;
            option_C.position.X = displaywidth / 3 * 2;
            option_C.levelOption = "Loch Ness";
            option_C.textposition.X = option_C.position.X + 30;
            option_C.textposition.Y = option_C.position.Y + 50;
            option_C.preview = new string[15];
            option_C.answer = new string[15];
            option_C.information1 = new string[5];
            option_C.information2 = new string[5];


            question_box.UI = Content.Load<Texture2D>("Text_Box");
            question_box.rect.Height = displayheight / 3;
            question_box.rect.Width = displaywidth;
            question_box.position.Y = -displayheight + 1080;
            question_box.position.X = 0;
            question_box.levelOption = "Left Click to select a level";
            question_box.textposition.X = question_box.position.X + 75;
            question_box.textposition.Y = displayheight - 1000;
            question_box.textposition2.X = question_box.position.X + 75;
            question_box.textposition2.Y = displayheight - 900;
            question_box.preview = new string[15];

            stats();
            Question_Load();
            level_start();
        }

        void stats() //sets the initial
        {
            mode = "level_select";


            option_A.rect.X = (int)option_A.position.X;
            option_A.rect.Y = (int)option_A.position.Y;

            option_B.rect.X = (int)option_B.position.X;
            option_B.rect.Y = (int)option_B.position.Y;

            option_C.rect.X = (int)option_C.position.X;
            option_C.rect.Y = (int)option_C.position.Y;

            question_box.rect.X = (int)question_box.position.X;
            question_box.rect.Y = (int)question_box.position.Y;

            int count;
            for (count = 0; count < 15; count++)
            {
                option_A.preview[count] = "Placeholder";
                option_B.preview[count] = "Placeholder";
                option_C.preview[count] = "Placeholder";
                question_box.preview[count] = "What would you like to learn about?";
            }

            win = false;
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>

        void level_start() //resets all the variables for when you begin a new level
        {
            character.position.X = displaywidth / 2;
            character.position.Y = displayheight / 2;
            current_question = 0 + extra;
            gameover = false;
            lives = 3;
            score = 0;
            character.foot = "Left";
            timesincestep = 0;
        }

        void Question_Load() //A seperate thing for inputting all the text for the questions, because its easier to manage from here than from the initialisation
        {
            //Stirling Questions
            option_A.answer[0] = "William Wallace";
            option_B.answer[0] = "Andy Moray & William Wallace";
            option_C.answer[0] = "Robert The Bruce";
            questions[0] = "Who commanded scottish forces at the battle of stirling bridge";
            correct_answers[0] = "B";

            option_A.answer[1] = "Battle of Dunbar";
            option_B.answer[1] = "Battle of Falkirk";
            option_C.answer[1] = "The battle of Bannockburn";
            questions[1] = "At what battle did Robert the Bruce defeat King Edward II of England?";
            correct_answers[1] = "C";

            option_A.answer[2] = "John Balliol";
            option_B.answer[2] = "Robert The Bruce";
            option_C.answer[2] = "John Hastings ";
            questions[2] = "Who was named king of Scotland by Edward I";
            correct_answers[2] = "A";

            option_A.answer[3] = "Highland Light Infantry";
            option_B.answer[3] = "Royal Scots";
            option_C.answer[3] = "The Scots Guard";
            questions[3] = "What esteemed infantry regiment was originally formed to guard the King of Scotland";
            correct_answers[3] = "C";

            option_A.answer[4] = "Ladies From hell";
            option_B.answer[4] = "Satans Skirts";
            option_C.answer[4] = "The Devils Kilts";
            questions[4] = "What nickname did the germans have for scottish soldiers in WW1";
            correct_answers[4] = "A";

            //Edinburgh Questions
            option_A.answer[5] = "Castle Rock";
            option_B.answer[5] = "Edinburgh Hill";
            option_C.answer[5] = "Ben Nevis";
            questions[5] = "What is the name of the hill that edinburgh castle is built on?";
            correct_answers[5] = "C";

            option_A.answer[6] = "To spend excess ammunition";
            option_B.answer[6] = "To scare ducks";
            option_C.answer[6] = "To signal the time to dock workers";
            questions[6] = "Why did the tradition of the one oclock gun start?";
            correct_answers[6] = "C";

            option_A.answer[7] = "Glasgow";
            option_B.answer[7] = "Dunfermline";
            option_C.answer[7] = "Dundee";
            questions[7] = "What city was the capital of Scotland before Edinburgh?";
            correct_answers[7] = "B";

            option_A.answer[8] = "Robert the Bruce";
            option_B.answer[8] = "Mary Queen of Scots";
            option_C.answer[8] = "James the 6th";
            questions[8] = "Who was the first monarch of both Scotland and England?";
            correct_answers[8] = "C";

            option_A.answer[9] = "Placeholder Edinburgh";
            option_B.answer[9] = "Placeholder Edinburgh";
            option_C.answer[9] = "Placeholder Edinburgh";
            questions[9] = "Placeholder Stirling";
            correct_answers[9] = "A";

            //Loch Ness Questions
            option_A.answer[10] = "Placeholder Loch";
            option_B.answer[10] = "Placeholder Loch";
            option_C.answer[10] = "Placeholder Loch";
            questions[10] = "Placeholder Loch";
            correct_answers[10] = "A";


            option_A.answer[11] = "Placeholder Loch";
            option_B.answer[11] = "Placeholder Loch";
            option_C.answer[11] = "Placeholder Loch";
            questions[11] = "Placeholder Stirling";

            option_A.answer[12] = "Placeholder Loch";
            option_B.answer[12] = "Placeholder Loch";
            option_C.answer[12] = "Placeholder Loch";
            questions[12] = "Placeholder Stirling";

            option_A.answer[13] = "Placeholder Loch";
            option_B.answer[13] = "Placeholder Loch";
            option_C.answer[13] = "Placeholder Loch";
            questions[13] = "Placeholder Stirling";

            option_A.answer[14] = "Placeholder Loch";
            option_B.answer[14] = "Placeholder Loch";
            option_C.answer[14] = "Placeholder Loch";
            questions[14] = "Placeholder Stirling";

            option_A.preview[0] = "Stirling Bridge";
            option_A.information1[0] = "While the English held control over most of the country, there were rebellions";
            option_A.information2[0] = "Such as when William Wallace and Andy Moray defeated the english at Stirling bridge";
            option_B.preview[0] = "Robert the bruce";
            option_B.information1[0] = "When Robert the Bruce sieged Stirling Castle in 1314 Edward II came to stop him.";
            option_B.information2[0] = "Through good training and smart tactics Robert crushed the english army";
            option_C.preview[0] = "John Baliol";
            option_C.information1[0] = "When the king of Scotland died without children, Edward of England chose the new king";
            option_C.information2[0] = "The king he chose was John Baliol, who he judged as having the best claim to the crown";

            option_A.preview[1] = "Scots Guard";
            option_A.information1[1] = "Charles I originally formed the scots guard as his own personal bodyguard";
            option_A.information2[1] = "It has since joined the British army, and become a famed infantry regiment";
            option_B.preview[1] = "WW1";
            option_B.information1[1] = "World war 1 was a brutal conflict for all involved, but especially the british army";
            option_B.information2[1] = "Kilts looking like skirts caused the Germans to call scottish soldiers ladies from hell";
            option_C.preview[1] = "N/A";
            option_C.information1[1] = "What would you like to learn about?";
            option_C.information2[1] = "";



        }


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
            character.rect.X = (int)character.position.X;
            character.rect.Y = (int)character.position.Y;
            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);

            // TODO: Add your update logic here
            //Allow the player to go back to the level select at any time
            if (Keyboard.GetState().IsKeyDown(Keys.T))
            {
                mode = "level_select";
                level_start();
            }

            if (gameover == false)
            {
                if (mode == "level_select")
                {
                    //I will fully admit, i stole this from the internet. 
                    IsMouseVisible = true;



                    if (option_A.rect.Contains(mousePoint) && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        level = "Stirling";
                        mode = "Exploration";
                        extra = 0;
                        level_start();
                        timechosenwrong = -5; //done to ensure that the message informing the player to look for hints doesnt pop up instantly upon entering a level
                    }

                    if (option_B.rect.Contains(mousePoint) && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        level = "Edinburgh";
                        mode = "Exploration";
                        extra = 5;
                        level_start();
                        timechosenwrong = -5;
                    }

                    if (option_C.rect.Contains(mousePoint) && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        level = "Loch";
                        mode = "Exploration";
                        extra = 10;
                        level_start();
                        timechosenwrong = -5;
                    }


                }


                if (mode == "Exploration")
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.W)) { character.position.Y -= 5; character.direction = "up"; character.moving = true; }
                    else if (Keyboard.GetState().IsKeyDown(Keys.S)) { character.position.Y += 5; character.direction = "down"; character.moving = true; }
                    else if (Keyboard.GetState().IsKeyDown(Keys.A)) { character.position.X -= 5; character.direction = "left"; character.moving = true; }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D)) { character.position.X += 5; character.direction = "right"; character.moving = true; }
                    else { character.moving = false; }

                    if (Keyboard.GetState().IsKeyDown(Keys.Enter)) { mode = "hints"; current_hint = 0; chosen = false; }
                    if (Keyboard.GetState().IsKeyDown(Keys.H)) { mode = "hints"; current_hint = 1; chosen = false; }
                    if (Keyboard.GetState().IsKeyDown(Keys.E)) { mode = "questions"; chosen = false; }


                    if (character.moving == true)
                    {
                        if (track == 0) { character.foot = "Right"; }
                        else if (track == 1) { character.foot = "Left"; }
                        if ((float)(gameTime.TotalGameTime.TotalSeconds) - timesincestep >= 0.5) { track += 1; timesincestep = (float)gameTime.TotalGameTime.TotalSeconds; }
                        if (track == 2) { track = 0; }
                    }


                }        
                if (mode == "hints")
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Q))
                    {
                        mode = "Exploration";
                    }
                    if (option_A.rect.Contains(mousePoint) && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        chosen = true;
                        current_answer = "A";
                    }

                    if (option_B.rect.Contains(mousePoint) && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        chosen = true;
                        current_answer = "B";
                    }

                    if (option_C.rect.Contains(mousePoint) && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        chosen = true;
                        current_answer = "C";

                    }

                }

                if (mode == "questions")
                {

                    if (Keyboard.GetState().IsKeyDown(Keys.Q))
                    {
                        mode = "Exploration";
                    }
                    timesince = (float)(gameTime.TotalGameTime.TotalSeconds - timechosen);

                    if (chosen == false && timesince >= 1)
                    {
                        if (option_A.rect.Contains(mousePoint) && mouseState.LeftButton == ButtonState.Pressed)
                        {
                            chosen = true;
                            current_answer = "A";
                        }

                        if (option_B.rect.Contains(mousePoint) && mouseState.LeftButton == ButtonState.Pressed)
                        {
                            chosen = true;
                            current_answer = "B";
                        }

                        if (option_C.rect.Contains(mousePoint) && mouseState.LeftButton == ButtonState.Pressed)
                        {
                            chosen = true;
                            current_answer = "C";

                        }
                    }

                    if (chosen == true)
                    {
                        if (current_answer == correct_answers[current_question])
                        {
                            current_question += 1;
                            score += 1;
                            chosen = false;
                            timechosen = (float)(gameTime.TotalGameTime.TotalSeconds);
                        }

                        else if (current_answer != correct_answers[current_question])
                        {
                            lives -= 1;
                            chosen = false;
                            mode = "Exploration";
                            timechosenwrong = (float)(gameTime.TotalGameTime.TotalSeconds);
                        }

                    }
                }

                if (score >= 5) { gameover = true; win = true; }
                if (lives == 0) { gameover = true; }
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

            // TODO: Add your drawing code here
            base.Draw(gameTime);
            spriteBatch.Begin();
            if (gameover == false)
            {
                if (mode == "Exploration")
                {
                    //draw each of the levels
                    if (level == "Stirling") { spriteBatch.Draw(background.stirling, background.rect, Color.White); }
                    if (level == "Edinburgh") { spriteBatch.Draw(background.edinburgh, background.rect, Color.White); }
                    if (level == "Loch") { spriteBatch.Draw(background.loch_ness, background.rect, Color.White); }
                    if ((float)(gameTime.TotalGameTime.TotalSeconds) - timechosenwrong <= 5) { spriteBatch.DrawString(font, "You were wrong, look for clues and try again", question_box.textposition, Color.White); } // provide a hint message if a player answers a question wrong
                    

                    if (character.foot == "Left") { spriteBatch.Draw(character.imageL, character.rect, Color.White); } //draw the character
                    if (character.foot == "Right") { spriteBatch.Draw(character.imageR, character.rect, Color.White); }

                    
                }


                if (mode == "level_select")
                {
                    //draw each of the text boxes
                    spriteBatch.Draw(option_A.UI, option_A.rect, Color.White);
                    spriteBatch.DrawString(font, option_A.levelOption, option_A.textposition, Color.White);

                    spriteBatch.Draw(option_B.UI, option_B.rect, Color.White);
                    spriteBatch.DrawString(font, option_B.levelOption, option_B.textposition, Color.White);

                    spriteBatch.Draw(option_C.UI, option_C.rect, Color.White);
                    spriteBatch.DrawString(font, option_C.levelOption, option_C.textposition, Color.White);

                    spriteBatch.Draw(question_box.UI, question_box.rect, Color.White);
                    spriteBatch.DrawString(font, question_box.levelOption, question_box.textposition, Color.White);
                }

                if (mode == "hints")
                {
                    //draw the text boxes
                    spriteBatch.Draw(option_A.UI, option_A.rect, Color.White);
                    spriteBatch.DrawString(font, option_A.preview[current_hint], option_A.textposition, Color.White);

                    spriteBatch.Draw(option_B.UI, option_B.rect, Color.White);
                    spriteBatch.DrawString(font, option_B.preview[current_hint], option_B.textposition, Color.White);

                    spriteBatch.Draw(option_C.UI, option_C.rect, Color.White);
                    spriteBatch.DrawString(font, option_C.preview[current_hint], option_C.textposition, Color.White);

                    spriteBatch.Draw(question_box.UI, question_box.rect, Color.White);

                    //Drawing the right test within the information box
                    if (chosen == false)
                    {
                        spriteBatch.DrawString(font, question_box.preview[current_hint], question_box.textposition, Color.White);
                    }

                    else if (chosen == true && current_answer == "A")
                    {
                        spriteBatch.DrawString(font, option_A.information1[current_hint], question_box.textposition, Color.White);
                        spriteBatch.DrawString(font, option_A.information2[current_hint], question_box.textposition2, Color.White);

                    }

                    else if (chosen == true && current_answer == "B")
                    {
                        spriteBatch.DrawString(font, option_B.information1[current_hint], question_box.textposition, Color.White);
                        spriteBatch.DrawString(font, option_B.information2[current_hint], question_box.textposition2, Color.White);
                    }

                    else if (chosen == true && current_answer == "C")
                    {
                        spriteBatch.DrawString(font, option_C.information1[current_hint], question_box.textposition, Color.White);
                        spriteBatch.DrawString(font, option_C.information2[current_hint], question_box.textposition2, Color.White);
                    }

                }
                if (mode == "questions")
                {
                    //drawing the answer boxes and question boxe
                    
                    spriteBatch.Draw(option_A.UI, option_A.rect, Color.White);
                    spriteBatch.DrawString(font, option_A.answer[current_question], option_A.textposition, Color.White);

                    spriteBatch.Draw(option_B.UI, option_B.rect, Color.White);
                    spriteBatch.DrawString(font, option_B.answer[current_question], option_B.textposition, Color.White);

                    spriteBatch.Draw(option_C.UI, option_C.rect, Color.White);
                    spriteBatch.DrawString(font, option_C.answer[current_question], option_C.textposition, Color.White);

                    spriteBatch.Draw(question_box.UI, question_box.rect, Color.White);
                    spriteBatch.DrawString(font, questions[current_question], question_box.textposition, Color.White);
                }


            }

            if (gameover == true)
            {
                if (win == true)
                {
                    spriteBatch.DrawString(font, "Congrats, you win, press T to return to the level select", option_B.textposition, Color.White);
                }

                if (win == false)
                {
                    spriteBatch.DrawString(font, "Sorry, you lost, press T to return to the level select", option_B.textposition, Color.White);
                }
            }
            spriteBatch.End();
        }
    }
}

