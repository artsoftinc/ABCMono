using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;


namespace ABC
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    partial class ABCGame : Game
    {
        private readonly GraphicsDeviceManager Graphics;
        private SpriteBatch SpriteBatch;
        
        //текстуры
        private Texture2D Background; // фон
        
        // кнопки
        public Button ButtonNewGame = new Button();

        // используемые объекты
        private GameProcess GameProcess = new GameProcess(); // игровой процесс (объект)
        
        Song song;
        
        // настройки отрисовки
        public static float Dx = 1f;
        public static float Dy = 1f;
        private static int NominalWidth = 800;
        private static int NominalHeight = 480;
        private static float NominalWidthCounted;
        private static float NominalHeightCounted;
        public static int CurrentWidth;
        private static int CurrentHeigth;
        private static float deltaY = 0;
        private static float deltaY_1 = 0;
        public static float YTopBorder;
        public static float YBottomBorder;
        



        public ABCGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            var metric = new Android.Util.DisplayMetrics();
            Activity.WindowManager.DefaultDisplay.GetMetrics(metric);
            Content.RootDirectory = "Content";

            Graphics.IsFullScreen = true;
            Graphics.PreferredBackBufferWidth = metric.WidthPixels;
            Graphics.PreferredBackBufferHeight = metric.HeightPixels;
            CurrentWidth = Graphics.PreferredBackBufferWidth;
            CurrentHeigth = Graphics.PreferredBackBufferHeight;
            Graphics.SupportedOrientations = DisplayOrientation.Portrait | DisplayOrientation.PortraitDown; //DisplayOrientation.Portrait;
            TouchPanel.EnabledGestures = (GestureType.Flick);
            // обновить параметры экрана
            UpdateScreenAttributies();
            
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
            //создание массива букв
            
            for (char c='A'; c<='Z'; c++)
            {
               GameProcess.Letters.Add(new Letter(0, 0, c.ToString())); 
            }

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Background = Content.Load<Texture2D>("Background");

            //song = Content.Load<Song>("abcsong");
            //MediaPlayer.Play(song);
            // TODO: use this.Content to load your game content here
        }

        public void LoadData(/*string locale*/)
        {
            Content.RootDirectory = "Content/";

            ButtonNewGame.TextureButton = Content.Load<Texture2D>("ru_ButtonNewGame");
            ButtonNewGame.TextureButtonLight = Content.Load<Texture2D>("ru_ButtonNewGamePressed");

            Content.RootDirectory = "Content/Letters"; //загрузка букв
            foreach (Letter let in GameProcess.Letters)
            {
                try // пока нет всех текстур
                {
                    let.Letterpng = Content.Load<Texture2D>(@""+let.NameLetter+@"");
                }
                catch (Exception ex)
                { }
            }
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                 Exit();
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White); // заполнить фон
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied); // установить последовательный порядок отрисовки объектов

            LoadData();  //загрузка контента

            if (GameProcess.IsGameLearn) //игровой роцесс обучения - листаем буквы
            {
                SpriteBatch.Draw(Background, new Vector2(0, 0/*AbsoluteX(0), AbsoluteY(0)*/),
                                     new Rectangle(0, 0, Background.Width, Background.Height),
                                     Color.White,
                                     0, new Vector2(0, 0), 1 * Dx, SpriteEffects.None, 0); // отрисовка фона
                                                                                          
                foreach (Letter letter in GameProcess.Letters)  //отрисовка ленты букв
                {
                    if (letter.Letterpng!=null)
                    SpriteBatch.Draw(letter.Letterpng,
                                         new Vector2(/*AbsoluteX(*/letter.Screenpos.X/*)*/, /*AbsoluteY(*/letter.Screenpos.Y/*)*/),
                                         new Rectangle(0, 0, letter.Letterpng.Width, letter.Letterpng.Height), Color.White,
                                         0, new Vector2(/*letter.Letterpng.Width / 2f, letter.Letterpng.Height / 2f*/0, 0), 1 * Dx,
                                         SpriteEffects.None, 0);
                    

                }
                GameProcess.Process(gameTime);
                if (GameProcess.IsDrag)
                {
                    if (GameProcess.Delta.X > 0)  //листаем вперед
                    {
                        GameProcess.Letters[GameProcess.LetterIndex].Screenpos.X += CurrentWidth;
                        if (GameProcess.Letters[GameProcess.LetterIndex].Letterpng != null)
                            SpriteBatch.Draw(GameProcess.Letters[GameProcess.LetterIndex].Letterpng,
                                                        new Vector2(/*AbsoluteX(*/GameProcess.Letters[GameProcess.LetterIndex].Screenpos.X/*)*/,
                                                        /*AbsoluteY(*/GameProcess.Letters[GameProcess.LetterIndex].Screenpos.Y/*)*/),
                                                      new Rectangle(0, 0, GameProcess.Letters[GameProcess.LetterIndex].Letterpng.Width,
                                                      GameProcess.Letters[GameProcess.LetterIndex].Letterpng.Height), Color.White,
                                                    0, new Vector2(/*letter.Letterpng.Width / 2f, letter.Letterpng.Height / 2f*/0, 0), 1 * Dx,
                                                  SpriteEffects.None, 0);

                        GameProcess.LetterIndex += 1;
                        if (GameProcess.LetterIndex == GameProcess.Letters.Count)
                        {
                            GameProcess = new GameProcess(CurrentWidth, CurrentHeigth);
                            GameProcess.IsDrag = false;
                        }
                        else
                        {
                            GameProcess.Letters[GameProcess.LetterIndex].Screenpos.X += CurrentWidth;
                            if (GameProcess.Letters[GameProcess.LetterIndex].Letterpng != null)
                                SpriteBatch.Draw(GameProcess.Letters[GameProcess.LetterIndex].Letterpng,
                                                        new Vector2(/*AbsoluteX(*/GameProcess.Letters[GameProcess.LetterIndex].Screenpos.X/*)*/,
                                                        /*AbsoluteY(*/GameProcess.Letters[GameProcess.LetterIndex].Screenpos.Y/*)*/),
                                                      new Rectangle(0, 0, GameProcess.Letters[GameProcess.LetterIndex].Letterpng.Width,
                                                      GameProcess.Letters[GameProcess.LetterIndex].Letterpng.Height), Color.White,
                                                    0, new Vector2(/*letter.Letterpng.Width / 2f, letter.Letterpng.Height / 2f*/0, 0), 1 * Dx,
                                                  SpriteEffects.None, 0);
                            GameProcess.IsDrag = false;
                        }
                    }
                    else  //листаем назад
                    {
                        GameProcess.Letters[GameProcess.LetterIndex].Screenpos.X -= CurrentWidth;
                        if (GameProcess.Letters[GameProcess.LetterIndex].Letterpng != null)
                            SpriteBatch.Draw(GameProcess.Letters[GameProcess.LetterIndex].Letterpng,
                                                        new Vector2(/*AbsoluteX(*/GameProcess.Letters[GameProcess.LetterIndex].Screenpos.X/*)*/,
                                                        /*AbsoluteY(*/GameProcess.Letters[GameProcess.LetterIndex].Screenpos.Y/*)*/),
                                                      new Rectangle(0, 0, GameProcess.Letters[GameProcess.LetterIndex].Letterpng.Width,
                                                      GameProcess.Letters[GameProcess.LetterIndex].Letterpng.Height), Color.White,
                                                    0, new Vector2(/*letter.Letterpng.Width / 2f, letter.Letterpng.Height / 2f*/0, 0), 1 * Dx,
                                                  SpriteEffects.None, 0);
                        
                        GameProcess.LetterIndex -= 1;
                        if (GameProcess.LetterIndex == -1)
                        {
                            GameProcess = new GameProcess(CurrentWidth, CurrentHeigth, true);

                            for (int j=GameProcess.Letters.Count-1; j>=0; j--)  //отрисовка ленты букв
                            {
                                if (GameProcess.Letters[j].Letterpng != null)
                                    SpriteBatch.Draw(GameProcess.Letters[j].Letterpng,
                                                         new Vector2(/*AbsoluteX(*/GameProcess.Letters[j].Screenpos.X/*)*/,
                                                         /*AbsoluteY(*/GameProcess.Letters[j].Screenpos.Y/*)*/),
                                                         new Rectangle(0, 0, GameProcess.Letters[j].Letterpng.Width, GameProcess.Letters[j].Letterpng.Height), Color.White,
                                                         0, new Vector2(/*letter.Letterpng.Width / 2f, letter.Letterpng.Height / 2f*/0, 0), 1 * Dx,
                                                         SpriteEffects.None, 0);
                            }
                            GameProcess.IsDrag = false;
                        }
                        else
                        {
                            GameProcess.Letters[GameProcess.LetterIndex].Screenpos.X -= CurrentWidth;
                            if (GameProcess.Letters[GameProcess.LetterIndex].Letterpng != null)
                                SpriteBatch.Draw(GameProcess.Letters[GameProcess.LetterIndex].Letterpng,
                                                        new Vector2(/*AbsoluteX(*/GameProcess.Letters[GameProcess.LetterIndex].Screenpos.X/*)*/,
                                                        /*AbsoluteY(*/GameProcess.Letters[GameProcess.LetterIndex].Screenpos.Y/*)*/),
                                                      new Rectangle(0, 0, GameProcess.Letters[GameProcess.LetterIndex].Letterpng.Width,
                                                      GameProcess.Letters[GameProcess.LetterIndex].Letterpng.Height), Color.White,
                                                    0, new Vector2(/*letter.Letterpng.Width / 2f, letter.Letterpng.Height / 2f*/0, 0), 1 * Dx,
                                                  SpriteEffects.None, 0);
                            GameProcess.IsDrag = false;
                        }
                    }
                }
            }
            else //рисуем стартовое меню
            {
                SpriteBatch.Draw(Background, new Vector2(0, 0/*AbsoluteX(0), AbsoluteY(0)*/),
                                         new Rectangle(0, 0, Background.Width, Background.Height),
                                         Color.White,
                                         0, new Vector2(0, 0), 1 * Dx, SpriteEffects.None, 0); // отрисовка фона

                ButtonNewGame.Process(SpriteBatch);
                //ButtonNewGame.Update(270, 300);
                ButtonNewGame.Update((CurrentWidth / 2 - ButtonNewGame.TextureButton.Width / 2), (CurrentHeigth / 3 - ButtonNewGame.TextureButton.Height / 3));

                if (ButtonNewGame.IsEnabled)
                {
                    ButtonNewGame.Reset();
                    GameProcess = new GameProcess(CurrentWidth, CurrentHeigth);
                    GameProcess.IsGameLearn = true;

                }
            }
            
                SpriteBatch.End();


            // TODO: Add your drawing code here
            // base.Draw(gameTime);
        }



        public void UpdateScreenAttributies()
        {
            Dx = (float)CurrentWidth / NominalWidth;
            Dy = (float)CurrentHeigth / NominalHeight;

            NominalHeightCounted = CurrentHeigth / Dx;
            NominalWidthCounted = CurrentWidth / Dx;

            int check = Math.Abs(CurrentHeigth - CurrentWidth / 16 * 9);
            if (check > 10)
                deltaY = (float)check / 2; // недостающее расстояние до 16:9 по п оси Y (в абсолютных координатах)
            deltaY_1 = -(CurrentWidth / 16 * 10 - CurrentWidth / 16 * 9) / 2f;

            YTopBorder = -deltaY / Dx; // координата точки в левом верхнем углу (в вируальных координатах)
            YBottomBorder = NominalHeight + (180); // координата точки в нижнем верхнем углу (в виртуальных координатах)
        }

        // коррекция координаты X
        public static float AbsoluteX(float x)
        {
            return x * Dx;
        }

        // коррекция координаты Y
        public static float AbsoluteY(float y)
        {
            return y * Dx + deltaY;
        }
        public void DrawRectangle(Rectangle coords, Color color)
        {
            var rect = new Texture2D(GraphicsDevice, 1, 1);
            rect.SetData(new[] { color });
            SpriteBatch.Draw(rect, coords, color);
        }
    }
}
