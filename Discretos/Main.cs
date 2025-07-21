using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Plateform_2D_v9.Enemy;
using Plateform_2D_v9.NetCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

/// <summary>
/// Les Aventures De Discretos 𝕯𝖎𝖘𝖈𝖗𝖊𝖙𝖔𝖘
/// Version : 0.0.0.9
/// Build : 8
/// SIEDEL Joshua © 2022-2025
/// Copyright © 2022-2024 SIEDEL Joshua
/// </summary>

namespace Plateform_2D_v9
{
    public class Main : Game
    {

        public GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        
        public static Texture2D[] Tileset;
        public static Texture2D[] Wallset;
        public static Texture2D[] Object;
        public static Texture2D[] SpriteSheetItem;
        public static Texture2D[] Enemy;
        public static Texture2D[] BackgroundTexture;
        public static Texture2D[] Screens;

        public static Texture2D Bounds;

        public static Texture2D Player;
        public static Texture2D Player_Down;
        public static Texture2D Player_Basic_Attack;
        public static Texture2D Player_Jump;
        public static Texture2D Squish_Player;
        public static Texture2D Player_Ladder;

        public static Texture2D effect;

        public static Texture2D BlackBar;
        public static Texture2D Banner;
        public static Texture2D Mouse_Icon_1;
        public static Texture2D Cadenas;

        public static Texture2D PortBox;
        public static Texture2D IPBox;

        public static Texture2D ObjectInterface;

        public static Texture2D SnowParticle;
        public static Texture2D RainParticle;

        public static Texture2D TileMap;
        public static Texture2D WorldMapImg;
        public static Texture2D Cascade;
        public static Texture2D Propeller;

        public static Texture2D Light_1_1000x1000;


        //[ThreadStatic]

        public static string text = "Launching...";

        private char caracter; 

        public static bool MapLoaded = false;
        public static bool ContentLoaded = false;

        public static int TILESIZE = 16;

        public static int LevelPlaying = 0;
        public static bool distortionShaderEnable = false;
        public static bool lightShaderEnable = false;

        public static bool camActived = true;

        private Screen screen;
        public static  Camera camera;
        private Render render;
        public static int factor = 1;


        public static float screenHeight = 768;

        public static Effect refractionEffect;
        public static Effect LightEffect;

        // some default control values for the refractions.
        public static float speed = .02f;
        public static float waveLength = .08f;
        public static float frequency = .2f;
        public static float refractiveIndex = 1.3f;
        public static Vector2 refractionVector = new Vector2(0f, 0f);

        public static float refractionVectorRange = 100;

        private float amount = 1;
        private float dir = -1;

        public static int Money;
        public static int PlayerPos;

        // GameState
        public static GameState gameState;
        public static PlayState playState = PlayState.InWorldMap;
        private State state;
        private Handler handler;
        private Menu menu;
        private Play play;
        private Settings settings;
        private MultiplayerMode multiplayer;
        private CreateServer createServer;
        private ConnectServer connectServer;
        public static bool isPaused;

        public static SpriteFont UltimateFont = null;
        Texture2D SuperFont;
        List<Rectangle> glyphRect = new List<Rectangle>();
        List<Rectangle> croppingList = new List<Rectangle>();
        List<char> charList = new List<char>();
        List<Vector3> Vector3List = new List<Vector3>();

        public static SpriteFont ScoreFont = null;
        Texture2D ScoreF;

        public static string Version = "0.0.0.9";
        public static string Build = "8";
        public static string Platform = ""; // win x86
        public static string State = "debug";

        public static string IP = NetPlay.LocalIPAddress();
        public static int MaxPort = 65535;

        Color color;
        int timer;

        public static bool inLevel;
        public static bool inWorldMap;

        public static bool Debug = false;
        public static bool DebugTime = false;

        public static double math = 1;

        public static double FPS;
        public static double UPS = 60;

        public static int ScreenWidth;
        public static int ScreenHeight;

        public static Keys Up = Keys.W;
        public static Keys Left = Keys.A;
        public static Keys Down = Keys.S;
        public static Keys Right = Keys.D;
        public static Keys Attack = Keys.Enter;

        public static Buttons UpPad = Buttons.B;
        public static Buttons LeftPad = Buttons.DPadLeft;
        public static Buttons DownPad = Buttons.DPadDown;
        public static Buttons RightPad = Buttons.DPadRight;
        public static Buttons AttackPad = Buttons.RightTrigger;

        public static String playerControlsName = "wasd";
        public static String keyboardName = "qwertz";


        public static float ScreenRatioComparedWith1080p;
        public static float ScreenRatio;                              //  16/9    4/3     ...

        public static int ResolutionX = 1920;
        public static int ResolutionY = 1080;

        /// <summary>
        /// Floutage pour les resolutions inferieur à 1920x1080.
        /// </summary>
        public static bool PixelPerfect = true;

        public static bool WaterShader = false;

        Vector3 HullCameraPosition = new Vector3(0f, 0f, 1f);
        Vector3 HullCameraLookAt = new Vector3(0, 0, 0);

        public Stopwatch UpdateTime;
        public float elapsedTime;

        public Stopwatch FpsTime;
        public float fpsTime;


        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            Window.AllowUserResizing = true;

            Window.IsBorderless = false;

            //TargetElapsedTime = TimeSpan.FromSeconds(1f / 144f);   /// Limite le jeu a 60 fps
            //MaxElapsedTime = TimeSpan.FromSeconds(1);
            //IsFixedTimeStep = true;    /// true : saute les images trop lourdes 
            //graphics.SynchronizeWithVerticalRetrace = true;

            //InactiveSleepTime = TimeSpan.FromSeconds(1f / 60f);  /// si le jeu est inactif

            graphics.PreferHalfPixelOffset = false;

            graphics.IsFullScreen = false;

            graphics.HardwareModeSwitch = false;
            
            graphics.ApplyChanges();

        }

        protected override void Initialize()
        {

            #region Screen Parametre

            // 1440x1080 -> 4/3
            this.graphics.PreferredBackBufferWidth = 1920 / 2;      //(1920/5) * 5;      //500;         640* 360
            this.graphics.PreferredBackBufferHeight = 1080 / 2;     //(1080/5) * 5;     //250;

            graphics.ApplyChanges();

            ScreenWidth = 1920;
            ScreenHeight = 1080;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            screen = new Screen(this, ScreenWidth, ScreenHeight);             //1024, 768);
            render = new Render(this);
            camera = new Camera(screen);

            //ScreenRatio = GraphicsDevice.DisplayMode.AspectRatio;
            ScreenRatioComparedWith1080p = 1920f / GraphicsDevice.DisplayMode.Width;

            #endregion

            
            Tileset = new Texture2D[12 + 1];
            Wallset = new Texture2D[200];
            Object = new Texture2D[14 + 1];
            SpriteSheetItem = new Texture2D[7];
            Enemy = new Texture2D[3 + 1];
            BackgroundTexture = new Texture2D[20];
            Screens = new Texture2D[3 + 1];


            LoadImg();


            ScoreFont = FontManager.InitFont(FontManager.Font.ScoreFont, ScoreF);
            UltimateFont = FontManager.InitFont(FontManager.Font.UltimateFont, SuperFont);

            ButtonManager.InitButtonManager();

            gameState = new GameState();
            handler = new Handler();
            play = new Play(handler, this);
            menu = new Menu(this);
            settings = new Settings(this);
            multiplayer = new MultiplayerMode();
            createServer = new CreateServer();
            connectServer = new ConnectServer();
            state = new State(spriteBatch, menu, settings, play, multiplayer, createServer, connectServer, this);

            gameState = GameState.Menu;

            //LevelSelector();


            LightManager.Init();
            LightManager.InitHullSystem(this);
            Handler.InitPlayersList();

            WorldMap.CreateWorldMapData();
            LevelData.CreateLevelData();


            Console.WriteLine(NetPlay.LocalIPAddress());


            UpdateTime = new Stopwatch();
            FpsTime = new Stopwatch();

            Console.WriteLine((int)Enum.Parse<TileV2.BlockID>("snow"));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //LoadImg();
        }

        public void LoadImg()
        {
            Player = Content.Load<Texture2D>("Images\\Player\\Player");
            Player_Down = Content.Load<Texture2D>("Images\\Player\\Down_Player");
            Player_Basic_Attack = Content.Load<Texture2D>("Images\\Player\\Player_Basic_Attack");
            Player_Jump = Content.Load<Texture2D>("Images\\Player\\Player_Jump");
            Squish_Player = Content.Load<Texture2D>("Images\\Player\\Squish_Player");
            Player_Ladder = Content.Load<Texture2D>("Images\\Player\\Player_Ladder");

            effect = Content.Load<Texture2D>("Images\\effect");

            for (int i = 0; i < Tileset.Length; i++)
                Tileset[i] = Content.Load<Texture2D>("Images\\Tile\\Tile_" + i);

            for (int i = 0; i < Wallset.Length; i++)
                if (File.Exists("Content"+ Path.DirectorySeparatorChar + "Images" + Path.DirectorySeparatorChar + "WallTile" + Path.DirectorySeparatorChar + $"Wall_{i}.xnb"))
                    Wallset[i] = Content.Load<Texture2D>("Images\\WallTile\\Wall_" + i);

            for (int i = 1; i < Object.Length; i++)
                Object[i] = Content.Load<Texture2D>("Images\\Object\\Object_" + i);

            for (int i = 1; i < SpriteSheetItem.Length; i++)
                SpriteSheetItem[i] = Content.Load<Texture2D>("Images\\Object\\Item_" + i);

            for (int i = 1; i < Enemy.Length; i++)
                Enemy[i] = Content.Load<Texture2D>("Images\\Enemy\\Enemy_" + i);

            for (int i = 1; i < BackgroundTexture.Length; i++)
                if (File.Exists("Content" + Path.DirectorySeparatorChar + "Images" + Path.DirectorySeparatorChar + "Background" + Path.DirectorySeparatorChar + $"Background_{i}.xnb"))
                    BackgroundTexture[i] = Content.Load<Texture2D>($"Images\\Background\\Background_{i}");

            for (int i = 1; i < Screens.Length; i++)
                Screens[i] = Content.Load<Texture2D>("Images\\Screen\\Screen_" + i);

            Bounds = Content.Load<Texture2D>("Images\\Enemy\\Bounds");


            BlackBar = Content.Load<Texture2D>("Images\\BlackBar");
            Banner = Content.Load<Texture2D>("Images\\Banner");
            Mouse_Icon_1 = Content.Load<Texture2D>("Images\\Mouse_icon_1");
            Cadenas = Content.Load<Texture2D>("Images\\Object\\Cadenas");

            PortBox = Content.Load<Texture2D>("Images\\Interface\\PortBox");
            IPBox = Content.Load<Texture2D>("Images\\Interface\\IPBox");

            ObjectInterface = Content.Load<Texture2D>("Images\\Interface\\Object_Interface");

            SnowParticle = Content.Load<Texture2D>("Images\\Snow_Effect");
            RainParticle = Content.Load<Texture2D>("Images\\Rain_Effect");

            TileMap = Content.Load<Texture2D>("Images\\Map\\TileMap");
            WorldMapImg = Content.Load<Texture2D>("Images\\Map\\WorldMap");
            Cascade = Content.Load<Texture2D>("Images\\Map\\Cascade");
            Propeller = Content.Load<Texture2D>("Images\\Map\\Propeller");

            SuperFont = Content.Load<Texture2D>("Images\\SuperFont");

            ScoreF = Content.Load<Texture2D>("Images\\ScoreFont");


            //TileMap = new Texture2D(GraphicsDevice, 10, 20);
            // -> TileMap = Texture2D.FromFile(GraphicsDevice, "Content\\Images\\Map\\TileMap.jpg");

            refractionEffect = Content.Load<Effect>("Shaders\\RefractionEffect");

            LightEffect = Content.Load<Effect>("Shaders\\LightEffect");

            Light_1_1000x1000 = Content.Load<Texture2D>("Shaders\\Light_1 1000x1000");

            ContentLoaded = true;
        }


        protected override void Update(GameTime gameTime)
        {
            if (KeyInput.getKeyState().IsKeyDown(Keys.F9) && !KeyInput.getOldKeyState().IsKeyDown(Keys.F9))
                WorldMap.CreateWorldMapData();

            if (gameState == GameState.Menu && KeyInput.getKeyState().IsKeyDown(Keys.F5) && !KeyInput.getOldKeyState().IsKeyDown(Keys.F5))
                LevelData.CreateLevelData();

            //Console.WriteLine(GraphicsDevice.PresentationParameters.Bounds.Width);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            state.Update(gameState, gameTime, screen, this);

            camera.UpdateBackground();
            Background.Update();

            KeyInput.Update();
            MouseInput.Update(screen);
            GamePadInput.Update(PlayerIndex.One);
            GamePadInput.Update(PlayerIndex.Two);
            GamePadInput.Update(PlayerIndex.Three);
            GamePadInput.Update(PlayerIndex.Four);

            if (KeyInput.getKeyState().IsKeyDown(Keys.E) && !KeyInput.getOldKeyState().IsKeyDown(Keys.E) && Debug)
            {

                //Handler.actors.Add(new ItemV2(new Vector2(MouseInput.GetLevelPos(graphics.IsFullScreen, camera).X, MouseInput.GetLevelPos(graphics.IsFullScreen, camera).Y), (int)Util.random.Next(1, 7), new Vector2((float)-Util.random.NextDouble() * 10, (float)Util.random.Next(-20, 0)))); // -2 -> -20
                //Handler.actors.Add(new ItemV2(new Vector2(MouseInput.GetLevelPos(graphics.IsFullScreen, camera).X, MouseInput.GetLevelPos(graphics.IsFullScreen, camera).Y), (int)Util.random.Next(1, 7), new Vector2((float)-Util.random.NextDouble() * 10, (float)Util.random.Next(-20, 0))));
                //Handler.actors.Add(new ItemV2(new Vector2(MouseInput.GetLevelPos(graphics.IsFullScreen, camera).X, MouseInput.GetLevelPos(graphics.IsFullScreen, camera).Y), (int)Util.random.Next(1, 7), new Vector2((float)Util.random.NextDouble() * 10, (float)Util.random.Next(-20, 0))));
                //Handler.actors.Add(new ItemV2(new Vector2(MouseInput.GetLevelPos(graphics.IsFullScreen, camera).X, MouseInput.GetLevelPos(graphics.IsFullScreen, camera).Y), (int)Util.random.Next(1, 7), new Vector2((float)Util.random.NextDouble() * 10, (float)Util.random.Next(-20, 0))));

                //Handler.actors.Add(new ItemV2(new Vector2(MouseInput.GetLevelPos(graphics.IsFullScreen, camera).X, MouseInput.GetLevelPos(graphics.IsFullScreen, camera).Y), (int)Util.random.Next(1, 7), new Vector2((float)-Util.random.NextDouble(), (float)Util.random.Next(-1, 0)))); // -2 -> -20
                //Handler.actors.Add(new ItemV2(new Vector2(MouseInput.GetLevelPos(graphics.IsFullScreen, camera).X, MouseInput.GetLevelPos(graphics.IsFullScreen, camera).Y), (int)Util.random.Next(1, 7), new Vector2((float)-Util.random.NextDouble(), (float)Util.random.Next(-1, 0))));
                //Handler.actors.Add(new ItemV2(new Vector2(MouseInput.GetLevelPos(graphics.IsFullScreen, camera).X, MouseInput.GetLevelPos(graphics.IsFullScreen, camera).Y), (int)Util.random.Next(1, 7), new Vector2((float)Util.random.NextDouble(), (float)Util.random.Next(-1, 0))));
                //Handler.actors.Add(new ItemV2(new Vector2(MouseInput.GetLevelPos(graphics.IsFullScreen, camera).X, MouseInput.GetLevelPos(graphics.IsFullScreen, camera).Y), (int)Util.random.Next(1, 7), new Vector2((float)Util.random.NextDouble(), (float)Util.random.Next(-1, 0))));

                //Handler.actors.Add(new EnemyV2(new Vector2(MouseInput.GetLevelPos(graphics.IsFullScreen, camera).X, MouseInput.GetLevelPos(graphics.IsFullScreen, camera).Y), 1));

                Handler.actors.Add(new BlackSwordMan(new Vector2(MouseInput.GetLevelPos(graphics.IsFullScreen, camera).X, MouseInput.GetLevelPos(graphics.IsFullScreen, camera).Y)));

            }

            if (((KeyInput.getKeyState().IsKeyDown(Keys.F12) && !KeyInput.getOldKeyState().IsKeyDown(Keys.F12)) || (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.BigButton) && !GamePadInput.GetOldPadState(PlayerIndex.One).IsButtonDown(Buttons.BigButton))) && gameState != GameState.Menu)
            {
                Background.SetBaseBackgroundPos(0, 0);
                gameState = GameState.Menu;
                MapLoaded = false;
            }


            if (KeyInput.getKeyState().IsKeyDown(Keys.F11) && !KeyInput.getOldKeyState().IsKeyDown(Keys.F11))
            {
                if (graphics.IsFullScreen == true)
                    graphics.IsFullScreen = false;
                else
                    graphics.IsFullScreen = true;

                graphics.ApplyChanges();


                if (graphics.IsFullScreen)
                {
                    graphics.PreferredBackBufferWidth = 1920;
                    graphics.PreferredBackBufferHeight = 1080;
                    graphics.ApplyChanges();
                    settings.Fullscreen.SetText("fullscreen : yes");
                }
                else
                {
                    graphics.PreferredBackBufferWidth = 1920 / 2;
                    graphics.PreferredBackBufferHeight = 1080 / 2;
                    graphics.ApplyChanges();
                    settings.Fullscreen.SetText("fullscreen : no");
                }

            }

            if (KeyInput.getKeyState().IsKeyDown(Keys.F1) && !KeyInput.getOldKeyState().IsKeyDown(Keys.F1) && Main.Debug)
                Debug = false;
            else if (KeyInput.getKeyState().IsKeyDown(Keys.F1) && !KeyInput.getOldKeyState().IsKeyDown(Keys.F1) && !Main.Debug)
                Debug = true;

            if (KeyInput.getKeyState().IsKeyDown(Keys.F2) && !KeyInput.getOldKeyState().IsKeyDown(Keys.F2) && Main.DebugTime)
                DebugTime = false;
            else if (KeyInput.getKeyState().IsKeyDown(Keys.F2) && !KeyInput.getOldKeyState().IsKeyDown(Keys.F2) && !Main.DebugTime)
                DebugTime = true;


            //FPS = 1000.0d / gameTime.ElapsedGameTime.TotalMilliseconds;


            /**if(KeyInput.getKeyState().IsKeyDown(Keys.F10) && !KeyInput.getOldKeyState().IsKeyDown(Keys.F10) && IsFixedTimeStep)
            {
                IsFixedTimeStep = false;
            }
            else if(KeyInput.getKeyState().IsKeyDown(Keys.F10) && !KeyInput.getOldKeyState().IsKeyDown(Keys.F10) && !IsFixedTimeStep)
            {
                IsFixedTimeStep = true;
            }

            if (KeyInput.getKeyState().IsKeyDown(Keys.F9) && !KeyInput.getOldKeyState().IsKeyDown(Keys.F9) && UPS == 60)
            {
                Console.WriteLine("30 FPS");
                UPS = 30;
                TargetElapsedTime = TimeSpan.FromSeconds(1f / 30f);
            }
            else if (KeyInput.getKeyState().IsKeyDown(Keys.F9) && !KeyInput.getOldKeyState().IsKeyDown(Keys.F9) && UPS == 30)
            {
                Console.WriteLine("60 FPS");
                UPS = 60;
                TargetElapsedTime = TimeSpan.FromSeconds(1f / 60f);
            }**/


            if (gameState != GameState.Settings)
                settings.settingState = Settings.SettingState.SettingMenu;

            if (MouseInput.GetPos() != MouseInput.GetOldPos())
                MouseInput.IsActived = true;


            //Screen.LevelShader = Render.ShaderEffect.LightMaskLevel;
            //Screen.BackgroundShader = Render.ShaderEffect.LightMask;
            //Screen.UIShader = Render.ShaderEffect.None;

            //Screen.LightMaskShader = Render.ShaderEffect.LightMask;

            //if (KeyInput.isSimpleClick(Keys.L))
            //Render.AddLight();


            //if (KeyInput.getKeyState().IsKeyDown(Keys.F6) && !KeyInput.getOldKeyState().IsKeyDown(Keys.F6))
            //{
            //    if (ScreenHeight == 1080)
            //    {
            //        screen.SetResolution(1920, 1200);
            //        ScreenWidth = 1920;
            //        ScreenHeight = 1200;
            //    }
            //    else if (ScreenHeight == 1200 && ScreenWidth == 1920)
            //    {
            //        screen.SetResolution(1600, 1200); // 1856×1392
            //        ScreenWidth = 1600;
            //        ScreenHeight = 1200;
            //    }
            //    else
            //    {
            //        screen.SetResolution(1920, 1080);
            //        ScreenWidth = 1920;
            //        ScreenHeight = 1080;
            //    }

            //}


            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {

            FpsTime.Stop();
            fpsTime = ((float)FpsTime.Elapsed.TotalMilliseconds);
            FpsTime.Restart();

            UpdateTime.Reset();
            UpdateTime.Start();


            #region Vertex System

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            rasterizerState.FillMode = FillMode.Solid;
            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            LightManager.basicEffect.Alpha = 1f;
            LightManager.basicEffect.VertexColorEnabled = true;

            GraphicsDevice.BlendState = BlendState.AlphaBlend;


            Viewport viewport = GraphicsDevice.Viewport;
            Matrix View = Matrix.CreateLookAt(HullCameraPosition, HullCameraLookAt, Vector3.Up);

            // **************************************************************************************************************
            Vector2 Translation;
            Translation = new Vector2((1920f / 4f) - camera.Position.X * 4.0f, (1080f / 4f) - camera.Position.Y * 4);

            Translation.X *= (GraphicsDevice.PresentationParameters.BackBufferWidth / (float)screen.Width);
            Translation.X += 240 * (GraphicsDevice.PresentationParameters.BackBufferWidth / (float)screen.Width) * 2;

            Translation.Y *= (GraphicsDevice.PresentationParameters.BackBufferHeight / (float)screen.Height);
            Translation.Y += 135 * (GraphicsDevice.PresentationParameters.BackBufferHeight / (float)screen.Height) * 2;
            // **************************************************************************************************************

            View *= Matrix.CreateTranslation(Translation.X, Translation.Y, 0);
            View *= Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 1, Camera.MinZ, Camera.MaxZ);

            Matrix Projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.PresentationParameters.BackBufferWidth, viewport.Height, 0, 0, 1); // nans // Matrix.CreateScale(1, 1, 1) * 

            LightManager.basicEffect.View = View; // View
            LightManager.basicEffect.Projection = Projection;
            LightManager.basicEffect.World = Matrix.Identity;

            #endregion


            //ScreenRatioComparedWith1080p = 1920f / GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            if (PixelPerfect)
                ScreenRatioComparedWith1080p = 1920f / ResolutionX;
            else
                ScreenRatioComparedWith1080p = 1;

            //ScreenRatioComparedWith1080p = 1;

            //Console.WriteLine(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);

            bool texFilter = false;

            GraphicsDevice.Clear(Color.White);

            

            #region Background

            screen.Set(Screen.BackTarget_1);
            
            render.Begin5(false, gameTime, spriteBatch, null);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            math += Math.PI / 100;


            Background.Draw(spriteBatch);

            //Background.SetBackground(3);

            //camera.UpdateBackground();

            render.End(spriteBatch);
            screen.UnSet();

            #endregion

            #region InCamera
            

            screen.Set(Screen.LevelTarget_1);
            render.Begin5(false, gameTime, spriteBatch, camera);
            GraphicsDevice.Clear(new Color(0, 0, 0, 0));

            DEBUG.DebugCollision(new Rectangle((int)camera.Position.X - 1920 / 4 / 2, (int)camera.Position.Y - 1080 / 4 / 2, 1920 / 4, 1080 / 4), Color.RoyalBlue, spriteBatch);

            state.DrawInCamera(spriteBatch, gameState, gameTime);


            render.End(spriteBatch);
            screen.UnSet();


            /*screen.Set(Screen.WaterTarget);
            render.Begin5(false, gameTime, spriteBatch, camera);
            GraphicsDevice.Clear(new Color(0, 0, 0, 0));

            Handler.DrawWater(spriteBatch, gameTime);


            render.End(spriteBatch);
            screen.UnSet();*/




            #region Light

            /// Level Light
            screen.Set(Screen.LightMaskLevel);
            render.BeginAdditive(false, gameTime, spriteBatch, camera);
            GraphicsDevice.Clear(new Color(0,0,0,0f));


            //LightManager.AmbianteLightR = new Color(1, 0, 0, 0.1f);
            //LightManager.AmbianteLightG = new Color(0, 1, 0, 0.1f);
            //LightManager.AmbianteLightB = new Color(0, 0, 1, 0.4f);

            /*LightManager.AmbianteLightR = new Color(0, 0, 0, 0.1f);
            LightManager.AmbianteLightG = new Color(0, 0, 0, 0.1f);
            LightManager.AmbianteLightB = new Color(0, 0, 0, 0.4f);*/


            //if (LightManager.lights.Count < 1)
            //    LightManager.lights.Add(new Light(new Vector2(Util.random.Next(0, 2000), Util.random.Next(0, 200)), 1f, Util.NextFloat(5f, 100f), new Color(Util.NextFloat(0f, 10f), Util.NextFloat(0f, 100f), Util.NextFloat(0f, 10f))));

            //if(Handler.playersV2.Count != 0)
            //    LightManager.lights[0].Position = Handler.playersV2[1].Position + new Vector2(Handler.playersV2[1].GetRectangle().Width / 2, Handler.playersV2[1].GetRectangle().Height / 2);

            //LightManager.lights[0].Radius = 50f;

            if(LightManager.isLightEnable)
                LightManager.Draw(spriteBatch, new Rectangle((int)camera.Position.X - 1920 / 4 / 2, (int)camera.Position.Y - 1080 / 4 / 2, 1920 / 4, 1080 / 4));

            render.End(spriteBatch);
            screen.UnSet();


            /// Level Hull
            screen.Set(Screen.HullMaskLevel);
            render.Begin5(false, gameTime, spriteBatch, camera);
            GraphicsDevice.Clear(new Color(0, 0, 0, 0f));

            if (KeyInput.getKeyState().IsKeyDown(Keys.F8))
                LightManager.DrawHull(this, screen);

            render.End(spriteBatch);
            screen.UnSet();


            /// Background Light
            screen.Set(Screen.LightMaskBackground);
            render.BeginAdditive(false, gameTime, spriteBatch, camera);
            GraphicsDevice.Clear(new Color(0, 0, 0, 0f));

            // Ambiante Light
            spriteBatch.Draw(Main.Bounds, new Rectangle(0, 0, 1920 / 2, 1080 / 2), new Color(1, 0, 0, 0.0f));
            spriteBatch.Draw(Main.Bounds, new Rectangle(0, 0, 1920 / 2, 1080 / 2), new Color(0, 1, 0, 0.0f));
            spriteBatch.Draw(Main.Bounds, new Rectangle(0, 0, 1920 / 2, 1080 / 2), new Color(0, 0, 1, 0.2f));

            render.End(spriteBatch);
            screen.UnSet();

            #endregion




            #endregion

            #region OffCamera

            screen.Set(Screen.FontTarget_1);
            render.Begin5(false, gameTime, spriteBatch, null);
            GraphicsDevice.Clear(new Color(0, 0, 0, 0));

            state.Draw(spriteBatch, gameState, gameTime);

            if(MouseInput.IsActived)
                spriteBatch.Draw(Mouse_Icon_1, new Vector2(MouseInput.GetRectangle(screen).X, MouseInput.GetRectangle(screen).Y), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            /*for (int i = 0; i < 2000; i++)
            {
                render.DrawLine(Bounds, new Vector2(MouseInput.GetRectangle(screen).X, MouseInput.GetRectangle(screen).Y), 4000f, i * (float)(Math.PI / 1000), spriteBatch, Color.White * 0.05f, 10);
            }*/

            if (DebugTime)
            {
                spriteBatch.DrawString(UltimateFont, "general draw : " + elapsedTime + "ms", new Vector2(10, 200), Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(UltimateFont, "max fps : " + Math.Round(1.0f / (elapsedTime / 1000)), new Vector2(10, 230), Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(UltimateFont, "fps : " + Math.Round(1.0f / (fpsTime / 1000)), new Vector2(10, 260), Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);

            }


            //render.DrawLine(Bounds, new Vector2(MouseInput.GetRectangle(screen).X - 150, MouseInput.GetRectangle(screen).Y), new Vector2(MouseInput.GetRectangle(screen).X, MouseInput.GetRectangle(screen).Y), spriteBatch, Color.Yellow   * 0.5f, 10);
            //render.DrawLine(Bounds, new Vector2(MouseInput.GetRectangle(screen).X + 150, MouseInput.GetRectangle(screen).Y), new Vector2(MouseInput.GetRectangle(screen).X, MouseInput.GetRectangle(screen).Y), spriteBatch, Color.Yellow  * 0.5f, 10);
            //render.DrawLine(Bounds, new Vector2(MouseInput.GetRectangle(screen).X, MouseInput.GetRectangle(screen).Y - 150), new Vector2(MouseInput.GetRectangle(screen).X, MouseInput.GetRectangle(screen).Y), spriteBatch, Color.Yellow  * 0.5f, 10);
            //render.DrawLine(Bounds, new Vector2(MouseInput.GetRectangle(screen).X, MouseInput.GetRectangle(screen).Y + 150), new Vector2(MouseInput.GetRectangle(screen).X, MouseInput.GetRectangle(screen).Y), spriteBatch, Color.Yellow    * 0.5f, 10);
            //render.DrawLine(Bounds, new Vector2(MouseInput.GetRectangle(screen).X - 100, MouseInput.GetRectangle(screen).Y - 100), new Vector2(MouseInput.GetRectangle(screen).X, MouseInput.GetRectangle(screen).Y), spriteBatch, Color.Yellow    * 0.5f, 10);
            //render.DrawLine(Bounds, new Vector2(MouseInput.GetRectangle(screen).X + 100, MouseInput.GetRectangle(screen).Y + 100), new Vector2(MouseInput.GetRectangle(screen).X, MouseInput.GetRectangle(screen).Y), spriteBatch, Color.Yellow * 0.5f, 10);
            //render.DrawLine(Bounds, new Vector2(MouseInput.GetRectangle(screen).X - 100, MouseInput.GetRectangle(screen).Y + 100), new Vector2(MouseInput.GetRectangle(screen).X, MouseInput.GetRectangle(screen).Y), spriteBatch, Color.Yellow     * 0.5f, 10);
            //render.DrawLine(Bounds, new Vector2(MouseInput.GetRectangle(screen).X + 100, MouseInput.GetRectangle(screen).Y - 100), new Vector2(MouseInput.GetRectangle(screen).X, MouseInput.GetRectangle(screen).Y), spriteBatch, Color.Yellow  * 0.5f, 10);


            //render.DrawLine(Bounds, new Vector2(200, 200), new Vector2(MouseInput.GetRectangle(screen).X, MouseInput.GetRectangle(screen).Y), spriteBatch, Color.White * 0.5f, 10);
            //render.DrawLine(Bounds, new Vector2(1000, 200), new Vector2(MouseInput.GetRectangle(screen).X, MouseInput.GetRectangle(screen).Y), spriteBatch, Color.White * 0.5f, 10);
            //render.DrawLine(Bounds, new Vector2(200, 1000), new Vector2(MouseInput.GetRectangle(screen).X, MouseInput.GetRectangle(screen).Y), spriteBatch, Color.White * 0.5f, 10);
            //render.DrawLine(Bounds, new Vector2(50, 400), new Vector2(MouseInput.GetRectangle(screen).X, MouseInput.GetRectangle(screen).Y), spriteBatch, Color.White * 0.5f, 10);
            //render.DrawLine(Bounds, new Vector2(400, 50), new Vector2(MouseInput.GetRectangle(screen).X, MouseInput.GetRectangle(screen).Y), spriteBatch, Color.White * 0.5f, 10);
            //render.DrawLine(Bounds, new Vector2(1000, 1300), new Vector2(MouseInput.GetRectangle(screen).X, MouseInput.GetRectangle(screen).Y), spriteBatch, Color.White * 0.5f, 10);
            //render.DrawLine(Bounds, new Vector2(0, 100), new Vector2(MouseInput.GetRectangle(screen).X, MouseInput.GetRectangle(screen).Y), spriteBatch, Color.White * 0.5f, 10);
            //render.DrawLine(Bounds, new Vector2(2000, 300), new Vector2(MouseInput.GetRectangle(screen).X, MouseInput.GetRectangle(screen).Y), spriteBatch, Color.White * 0.5f, 10);
            //render.DrawLine(Bounds, new Vector2(1500, 1000), new Vector2(MouseInput.GetRectangle(screen).X, MouseInput.GetRectangle(screen).Y), spriteBatch, Color.White * 0.5f, 10);

            //InitFont();

            //spriteBatch.Draw(Main.Tileset[4], new Rectangle(500, 0, 15 * 4, 14 * 4), Color.White);


            if (timer <= 0)
            {
                color = new Color(Util.random.Next(100, 255), Util.random.Next(100, 255), Util.random.Next(100, 255));
                timer = 60;
            }

            timer--;

            if (gameTime.IsRunningSlowly)
                spriteBatch.Draw(Bounds, new Rectangle(1800, 0 + 13, 50, 50), Color.Red);
            else
                spriteBatch.Draw(Bounds, new Rectangle(1800, 0 + 13, 50, 50), Color.Green);

            ///Writer.DrawSuperText(UltimateFont, "test", new Vector2(200, 200), Color.Black, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f, 4f, spriteBatch);

            //Writer.DrawText(UltimateFont, "LES AVENTURES DE DISCRETOS", new Vector2(200, 400), Color.Black, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f, spriteBatch);
            //Writer.DrawText(UltimateFont, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", new Vector2(200, 600), Color.Black, color, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f, 4f, spriteBatch);

            //Writer.DrawText(UltimateFont, "abcdefghijklmnopqrstuvwxyz", new Vector2(200, 650), Color.Black, color, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f, 3f, spriteBatch);

            //Writer.DrawText(UltimateFont, "1234567890", new Vector2(200, 720), Color.Black, color, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f, 4f, spriteBatch);

            //Writer.DrawText(UltimateFont, "!:?", new Vector2(200, 790), Color.Black, color, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f, 4f, spriteBatch);

            //Writer.DrawText(UltimateFont, "Les Aventures de Discretos ?", new Vector2(200, 650), Color.Black, color, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f, 3f, spriteBatch, false);

            render.End(spriteBatch);
            screen.UnSet();


            #endregion


            //Screen.HullMaskLevel.SaveAsPng(new StreamWriter("hull_test.png").BaseStream, 1920, 1080);

            screen.Present(render, gameTime, spriteBatch);

            UpdateTime.Stop();

            elapsedTime = ((float)UpdateTime.Elapsed.TotalMilliseconds);
            
            base.Draw(gameTime);
        }


        public static void LevelSelector()
        {

            Console.WriteLine("{NATIVE LEVEL SELECTOR} VER 3.0");
            Console.WriteLine("ENTRER UN NIVEAU POUR LE JOUER : ");
            text = Console.ReadLine();

            LevelPlaying = Int16.Parse(text);

            if (LevelPlaying < 1 || LevelPlaying > LevelData.NumberOfLevel)
            {
                Console.WriteLine("CE NIVEAU N'EXISTE PAS !");
                Console.WriteLine("LE NIVEAU 1 SERA ALORS CHARGÉ !");
                LevelPlaying = 1;
            }

            Console.WriteLine("PRESSER UNE TOUCHE POUR JOUER !");
            Console.ReadLine();

            StartLevel(LevelPlaying);

        }

        public static void LevelSelector(int level)
        {

            Console.WriteLine("{LEVEL SELECTOR} VER 4.0");

            LevelPlaying = level;

            LightManager.Init();

            for(int i = 1; i <= 4; i++)
                if(Handler.playersV2.ContainsKey(i))
                    Handler.playersV2[i].InitLight();

            ThreadPool.QueueUserWorkItem(_ => Level.LoadLevel());

        }

        public static void StartLevel(int level)
        {
            MapLoaded = false;
            LevelSelector(level);
            inWorldMap = false;
            inLevel = true;
        }


        public static void SetControls(String ControlsName)
        {
            playerControlsName = ControlsName;

            if (ControlsName == "wasd")
            {
                keyboardName = "qwertz"; 
                Up = Keys.W;
                Left = Keys.A;
                Down = Keys.S;
                Right = Keys.D;
            }
            else if (ControlsName == "zqsd")
            {
                keyboardName = "azerty";
                Up = Keys.Z;
                Left = Keys.Q;
                Down = Keys.S;
                Right = Keys.D;
            }
            else if (ControlsName == "arrow")
            {
                keyboardName = "qwertz and azerty";
                Up = Keys.Up;
                Left = Keys.Left;
                Down = Keys.Down;
                Right = Keys.Right;
            }
        }


        public void QuitGame()
        {
            Exit();
        }


        protected override void OnExiting(object sender, EventArgs args)
        {
            //if(connectServer.player != null)
            //  connectServer.player.Disconnect();         plus tard

            base.OnExiting(sender, args);
        }

    }

}