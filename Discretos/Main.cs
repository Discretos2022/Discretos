using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Les Aventures De Discretos
/// Version : 0.0.0.9
/// Build : 8
/// SIEDEL Joshua © 2022-2024
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

        public static Texture2D Bounds;

        public static Texture2D Player;
        public static Texture2D Player_Down;
        public static Texture2D Player_Basic_Attack;
        public static Texture2D Squish_Player;
        public static Texture2D effect;

        public static Texture2D BlackBar;
        public static Texture2D Banner;
        public static Texture2D Screen1;
        public static Texture2D Mouse_Icon_1;
        public static Texture2D Cadenas;

        public static Texture2D PortBox;
        public static Texture2D IPBox;

        public static Texture2D SnowParticle;

        public static Texture2D TileMap;
        public static Texture2D WorldMapImg;

        public static Texture2D Light_1_1000x1000;


        //[ThreadStatic]

        public static string text = "Launching...";

        private char caracter; 

        public static bool MapLoaded = false;
        public static bool ContentLoaded = false;

        public static int TILESIZE = 16;

        public static bool[] SolidTile = new bool[100];
        public static bool[] SolidTileTop = new bool[100];
        public static bool[] LevelItem = new bool[100];

        public static int LevelPlaying = 0;

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
        private Multiplay multiplay;
        public static bool isPaused;

        public static SpriteFont UltimateFont = null;
        Texture2D SuperFont;
        List<Rectangle> glyphRect = new List<Rectangle>();
        List<Rectangle> croppingList = new List<Rectangle>();
        List<char> charList = new List<char>();
        List<Vector3> Vector3List = new List<Vector3>();

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


        public Stopwatch UpdateTime;
        public float elapsedTime;


        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            Window.AllowUserResizing = true;

            Window.IsBorderless = false;

            //TargetElapsedTime = TimeSpan.FromSeconds(1f / 60f);   /// Limite le jeu a 60 fps
            //MaxElapsedTime = TimeSpan.FromSeconds(1f / 1f);
            //IsFixedTimeStep = false;    /// true : saute les images trop lourdes 
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

            
            this.graphics.PreferredBackBufferWidth = 1920 / 2;      //(1920/5) * 5;      //500;         640* 360
            this.graphics.PreferredBackBufferHeight = 1080 / 2;     //(1080/5) * 5;     //250;

            graphics.ApplyChanges();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            screen = new Screen(this, 1920, 1080);             //1024, 768);
            render = new Render(this, spriteBatch);
            camera = new Camera(screen);

            ScreenWidth = 1920;
            ScreenHeight = 1080;

            //ScreenRatio = GraphicsDevice.DisplayMode.AspectRatio;
            ScreenRatioComparedWith1080p = 1920f / GraphicsDevice.DisplayMode.Width;

            #endregion

            
            Tileset = new Texture2D[10];
            Wallset = new Texture2D[200];
            Object = new Texture2D[8];
            SpriteSheetItem = new Texture2D[7];
            Enemy = new Texture2D[3];
            BackgroundTexture = new Texture2D[20];


            SolidTile[0] = false; // vide
            SolidTile[1] = true; // block de terre
            SolidTileTop[2] = true; // platform
            SolidTile[3] = true; // block de sable
            SolidTile[4] = true; // block de neige
            SolidTile[5] = true; // block de nuage
            SolidTile[6] = true; // brick
            SolidTile[7] = true; // cendre
            SolidTile[8] = true; // movingblock
            SolidTileTop[9] = true; // platform brick break


            LoadImg();

            InitFont();

            ButtonManager.InitButtonManager();

            gameState = new GameState();
            handler = new Handler();
            play = new Play(handler, this);
            menu = new Menu(this);
            settings = new Settings(this);
            multiplayer = new MultiplayerMode();
            createServer = new CreateServer();
            connectServer = new ConnectServer();
            multiplay = new Multiplay(handler, this);
            state = new State(spriteBatch, menu, settings, play, multiplayer, createServer, connectServer, multiplay, this);

            gameState = GameState.Menu;

            //LevelSelector();


            LightManager.Init();
            Handler.InitPlayersList();

            WorldMap.CreateWorldMapData();
            WorldMap.LoadWorldMap();
            LevelData.CreateLevelData();


            Console.WriteLine(NetPlay.LocalIPAddress());


            UpdateTime = new Stopwatch();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            LoadImg();
        }

        public void LoadImg()
        {
            Player = Content.Load<Texture2D>("Images\\Player\\Player");
            Player_Down = Content.Load<Texture2D>("Images\\Player\\Down_Player");
            Player_Basic_Attack = Content.Load<Texture2D>("Images\\Player\\Player_Basic_Attack");
            Squish_Player = Content.Load<Texture2D>("Images\\Player\\Squish_Player");

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

            Bounds = Content.Load<Texture2D>("Images\\Enemy\\Bounds");


            BlackBar = Content.Load<Texture2D>("Images\\BlackBar");
            Banner = Content.Load<Texture2D>("Images\\Banner");
            Screen1 = Content.Load<Texture2D>("Images\\Screen\\Screen_1");
            Mouse_Icon_1 = Content.Load<Texture2D>("Images\\Mouse_icon_1");
            Cadenas = Content.Load<Texture2D>("Images\\Object\\Cadenas");

            PortBox = Content.Load<Texture2D>("Images\\Interface\\PortBox");
            IPBox = Content.Load<Texture2D>("Images\\Interface\\IPBox");

            SnowParticle = Content.Load<Texture2D>("Images\\Snow_Effect");

            TileMap = Content.Load<Texture2D>("Images\\Map\\TileMap");
            WorldMapImg = Content.Load<Texture2D>("Images\\Map\\WorldMap");

            SuperFont = Content.Load<Texture2D>("Images\\SuperFont");


            //TileMap = new Texture2D(GraphicsDevice, 10, 20);
            //TileMap = Texture2D.FromFile(GraphicsDevice, "Content\\Images\\Map\\TileMap.jpg");

            refractionEffect = Content.Load<Effect>("Shaders\\RefractionEffect");


            LightEffect = Content.Load<Effect>("Shaders\\LightEffect");

            Light_1_1000x1000 = Content.Load<Texture2D>("Shaders\\Light_1 1000x1000");

            ContentLoaded = true;
        }

        protected override void Update(GameTime gameTime)
        {

            if (gameState == GameState.Menu && KeyInput.getKeyState().IsKeyDown(Keys.F5) && !KeyInput.getOldKeyState().IsKeyDown(Keys.F5))
                LevelData.CreateLevelData();

            //Console.WriteLine(GraphicsDevice.PresentationParameters.Bounds.Width);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            state.Update(gameState, gameTime, screen, this);

            camera.UpdateBackground();
            Background.Update();

            ShaderManager.Update(gameState);
            

            KeyInput.Update();
            MouseInput.Update(screen);
            GamePadInput.Update(PlayerIndex.One);
            GamePadInput.Update(PlayerIndex.Two);
            GamePadInput.Update(PlayerIndex.Three);
            GamePadInput.Update(PlayerIndex.Four);

            if (KeyInput.getKeyState().IsKeyDown(Keys.P) && !KeyInput.getOldKeyState().IsKeyDown(Keys.P) && Debug)
            {

                Handler.actors.Add(new ItemV2(new Vector2(MouseInput.GetLevelPos(graphics.IsFullScreen, camera).X, MouseInput.GetLevelPos(graphics.IsFullScreen, camera).Y), (int)Util.random.Next(1, 7), new Vector2((float)-Util.random.NextDouble(), (float)Util.random.Next(-2, 0))));
                Handler.actors.Add(new ItemV2(new Vector2(MouseInput.GetLevelPos(graphics.IsFullScreen, camera).X, MouseInput.GetLevelPos(graphics.IsFullScreen, camera).Y), (int)Util.random.Next(1, 7), new Vector2((float)-Util.random.NextDouble(), (float)Util.random.Next(-2, 0))));
                Handler.actors.Add(new ItemV2(new Vector2(MouseInput.GetLevelPos(graphics.IsFullScreen, camera).X, MouseInput.GetLevelPos(graphics.IsFullScreen, camera).Y), (int)Util.random.Next(1, 7), new Vector2((float)Util.random.NextDouble(), (float)Util.random.Next(-2, 0))));
                Handler.actors.Add(new ItemV2(new Vector2(MouseInput.GetLevelPos(graphics.IsFullScreen, camera).X, MouseInput.GetLevelPos(graphics.IsFullScreen, camera).Y), (int)Util.random.Next(1, 7), new Vector2((float)Util.random.NextDouble(), (float)Util.random.Next(-2, 0))));

                Handler.actors.Add(new EnemyV2(new Vector2(MouseInput.GetLevelPos(graphics.IsFullScreen, camera).X, MouseInput.GetLevelPos(graphics.IsFullScreen, camera).Y), 1));

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

            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            UpdateTime.Reset();
            UpdateTime.Start();

            //ScreenRatioComparedWith1080p = 1920f / GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            if (PixelPerfect)
                ScreenRatioComparedWith1080p = 1920f / ResolutionX;
            else
                ScreenRatioComparedWith1080p = 1;

            //ScreenRatioComparedWith1080p = 1;

            //Console.WriteLine(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);

            bool texFilter = false;

            GraphicsDevice.Clear(Color.Black);

            #region Background

            screen.Set(Screen.BackTarget);
            
            render.Begin5(false, gameTime, spriteBatch, null);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            math += Math.PI / 100;


            Background.Draw(spriteBatch);

            //Background.SetBackground(3);

            //camera.UpdateBackground();

            render.End();
            screen.UnSet();

            #endregion

            #region InCamera
            

            screen.Set(Screen.LevelTarget);
            render.Begin5(false, gameTime, spriteBatch, camera);
            GraphicsDevice.Clear(new Color(0,0,0,0));

            //this.Transform = Matrix.CreateScale(2.5f, 2.5f, 1f) * Matrix.CreateRotationZ(0f) * Matrix.CreateTranslation(new Vector3(0f, 0f, 0f));

            DEBUG.DebugCollision(new Rectangle((int)camera.Position.X - 1920/4/2, (int)camera.Position.Y - 1080/4/2, 1920/4, 1080/4), Color.RoyalBlue, spriteBatch);

            state.DrawInCamera(spriteBatch, gameState, gameTime);

            render.End();
            screen.UnSet();


            #region Light

            /// Level Light
            screen.Set(Screen.LightMaskLevel);
            render.BeginAdditive(false, gameTime, spriteBatch, camera);
            GraphicsDevice.Clear(new Color(0,0,0,0f));


            LightManager.AmbianteLightR = new Color(1, 0, 0, 0.1f);
            LightManager.AmbianteLightG = new Color(0, 1, 0, 0.1f);
            LightManager.AmbianteLightB = new Color(0, 0, 1, 0.4f);


            //if (LightManager.lights.Count < 1)
            //    LightManager.lights.Add(new Light(new Vector2(Util.random.Next(0, 2000), Util.random.Next(0, 200)), 1f, Util.NextFloat(5f, 100f), new Color(Util.NextFloat(0f, 10f), Util.NextFloat(0f, 100f), Util.NextFloat(0f, 10f))));

            //if(Handler.playersV2.Count != 0)
            //    LightManager.lights[0].Position = Handler.playersV2[1].Position + new Vector2(Handler.playersV2[1].GetRectangle().Width / 2, Handler.playersV2[1].GetRectangle().Height / 2);

            //LightManager.lights[0].Radius = 50f;

            LightManager.Draw(spriteBatch);

            //spriteBatch.Draw(Main.Bounds, new Rectangle(0,0, 2000, 2000), Color.Red);

            render.End();
            screen.UnSet();

            /// Background Light
            screen.Set(Screen.LightMaskBackground);
            render.BeginAdditive(false, gameTime, spriteBatch, camera);
            GraphicsDevice.Clear(new Color(0, 0, 0, 0f));

            spriteBatch.Draw(Main.Bounds, new Rectangle(0, 0, 1920 / 2, 1080 / 2), new Color(1, 0, 0, 0.0f));
            spriteBatch.Draw(Main.Bounds, new Rectangle(0, 0, 1920 / 2, 1080 / 2), new Color(0, 1, 0, 0.0f));
            spriteBatch.Draw(Main.Bounds, new Rectangle(0, 0, 1920 / 2, 1080 / 2), new Color(0, 0, 1, 0.2f));


            //if (LightManager.lights.Count < 1)
            //    LightManager.lights.Add(new Light(new Vector2(Util.random.Next(0, 2000), Util.random.Next(0, 200)), 1f, Util.NextFloat(5f, 100f), new Color(Util.NextFloat(0f, 10f), Util.NextFloat(0f, 100f), Util.NextFloat(0f, 10f))));

            //if (Handler.playersV2.Count != 0)
            //    LightManager.lights[0].Position = Handler.playersV2[1].Position + new Vector2(Handler.playersV2[1].GetRectangle().Width / 2, Handler.playersV2[1].GetRectangle().Height / 2);

            //LightManager.lights[0].Radius = 50f;

            //for (int i = 0; i < LightManager.lights.Count; i++)
            //{
            //    LightManager.lights[i].Draw(spriteBatch);
            //}

            render.End();
            screen.UnSet();

            #endregion




            #endregion

            #region OffCamera

            screen.Set(Screen.FontTarget);
            render.Begin5(false, gameTime, spriteBatch, null);
            GraphicsDevice.Clear(new Color(0, 0, 0, 0));

            state.Draw(spriteBatch, gameState, gameTime);

            if(MouseInput.IsActived)
                spriteBatch.Draw(Mouse_Icon_1, new Vector2(MouseInput.GetRectangle(screen).X, MouseInput.GetRectangle(screen).Y), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            /*for (int i = 0; i < 2000; i++)
            {
                render.DrawLine(Bounds, new Vector2(MouseInput.GetRectangle(screen).X, MouseInput.GetRectangle(screen).Y), 4000f, i * (float)(Math.PI / 1000), spriteBatch, Color.White * 0.05f, 10);
            }*/

            if(DebugTime)
                spriteBatch.DrawString(UltimateFont, "general draw : " + elapsedTime + "ms", new Vector2(10, 200), Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);


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
                spriteBatch.Draw(Bounds, new Rectangle(1800, 0 + 25, 50, 50), Color.Red);
            else
                spriteBatch.Draw(Bounds, new Rectangle(1800, 0 + 25, 50, 50), Color.Green);

            ///Writer.DrawSuperText(UltimateFont, "test", new Vector2(200, 200), Color.Black, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f, 4f, spriteBatch);

            //Writer.DrawText(UltimateFont, "LES AVENTURES DE DISCRETOS", new Vector2(200, 400), Color.Black, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f, spriteBatch);
            //Writer.DrawText(UltimateFont, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", new Vector2(200, 600), Color.Black, color, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f, 4f, spriteBatch);

            //Writer.DrawText(UltimateFont, "abcdefghijklmnopqrstuvwxyz", new Vector2(200, 650), Color.Black, color, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f, 3f, spriteBatch);

            //Writer.DrawText(UltimateFont, "1234567890", new Vector2(200, 720), Color.Black, color, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f, 4f, spriteBatch);

            //Writer.DrawText(UltimateFont, "!:?", new Vector2(200, 790), Color.Black, color, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f, 4f, spriteBatch);

            //Writer.DrawText(UltimateFont, "Les Aventures de Discretos ?", new Vector2(200, 650), Color.Black, color, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f, 3f, spriteBatch, false);

            render.End();
            screen.UnSet();


            #endregion

            screen.Present(render, gameTime, spriteBatch);

            UpdateTime.Stop();

            elapsedTime = ((float)UpdateTime.Elapsed.TotalMilliseconds);

            base.Draw(gameTime);
        }


        public void InitFont()
        {
            glyphRect.Add(new Rectangle(600, 0, 9, 16));   //SPACE
            glyphRect.Add(new Rectangle(533, 0, 1, 16));   //!
            glyphRect.Add(new Rectangle(583, 0, 1, 16));   //.

            glyphRect.Add(new Rectangle(456, 0, 6, 16));   //0
            glyphRect.Add(new Rectangle(463, 0, 5, 16));   //1
            glyphRect.Add(new Rectangle(469, 0, 7, 16));   //2
            glyphRect.Add(new Rectangle(477, 0, 6, 16));   //3
            glyphRect.Add(new Rectangle(484, 0, 7, 16));   //4
            glyphRect.Add(new Rectangle(492, 0, 7, 16));   //5
            glyphRect.Add(new Rectangle(500, 0, 7, 16));   //6
            glyphRect.Add(new Rectangle(508, 0, 7, 16));   //7
            glyphRect.Add(new Rectangle(516, 0, 6, 16));   //8
            glyphRect.Add(new Rectangle(523, 0, 6, 16));   //9

            glyphRect.Add(new Rectangle(530, 0, 2, 16));   //:
            glyphRect.Add(new Rectangle(535, 0, 7, 16));   //?

            glyphRect.Add(new Rectangle(0, 0, 11, 16));    //A
            glyphRect.Add(new Rectangle(12, 0, 9, 16));    //B
            glyphRect.Add(new Rectangle(22, 0, 11, 16));   //C
            glyphRect.Add(new Rectangle(34, 0, 10, 16));   //D
            glyphRect.Add(new Rectangle(45, 0, 9, 16));    //E
            glyphRect.Add(new Rectangle(55, 0, 9, 16));    //F
            glyphRect.Add(new Rectangle(65, 0, 11, 16));   //G
            glyphRect.Add(new Rectangle(77, 0, 9, 16));    //H
            glyphRect.Add(new Rectangle(87, 0, 7, 16));    //I
            glyphRect.Add(new Rectangle(95, 0, 10, 16));   //J
            glyphRect.Add(new Rectangle(106, 0, 9, 16));   //K
            glyphRect.Add(new Rectangle(116, 0, 8, 16));   //L
            glyphRect.Add(new Rectangle(125, 0, 11, 16));  //M
            glyphRect.Add(new Rectangle(137, 0, 9, 16));   //N
            glyphRect.Add(new Rectangle(147, 0, 9, 16));   //O
            glyphRect.Add(new Rectangle(157, 0, 9, 16));   //P
            glyphRect.Add(new Rectangle(167, 0, 9, 16));   //Q
            glyphRect.Add(new Rectangle(177, 0, 9, 16));   //R
            glyphRect.Add(new Rectangle(187, 0, 9, 16));   //S
            glyphRect.Add(new Rectangle(197, 0, 9, 16));   //T
            glyphRect.Add(new Rectangle(207, 0, 9, 16));   //U
            glyphRect.Add(new Rectangle(217, 0, 11, 16));  //V
            glyphRect.Add(new Rectangle(229, 0, 15, 16));  //W
            glyphRect.Add(new Rectangle(245, 0, 10, 16));  //X
            glyphRect.Add(new Rectangle(256, 0, 11, 16));  //Y
            glyphRect.Add(new Rectangle(268, 0, 9, 16));   //Z

            glyphRect.Add(new Rectangle(278, 0, 6, 16));   //a
            glyphRect.Add(new Rectangle(285, 0, 5, 16));   //b
            glyphRect.Add(new Rectangle(291, 0, 6, 16));   //c
            glyphRect.Add(new Rectangle(298, 0, 5, 16));   //d
            glyphRect.Add(new Rectangle(304, 0, 5, 16));   //e
            glyphRect.Add(new Rectangle(310, 0, 5, 16));   //f
            glyphRect.Add(new Rectangle(316, 0, 6, 16));   //g
            glyphRect.Add(new Rectangle(323, 0, 5, 16));   //h
            glyphRect.Add(new Rectangle(329, 0, 5, 16));   //i
            glyphRect.Add(new Rectangle(335, 0, 5, 16));   //j
            glyphRect.Add(new Rectangle(341, 0, 5, 16));   //k
            glyphRect.Add(new Rectangle(347, 0, 5, 16));   //l
            glyphRect.Add(new Rectangle(352, 0, 7, 16));   //m
            glyphRect.Add(new Rectangle(360, 0, 5, 16));   //n
            glyphRect.Add(new Rectangle(366, 0, 7, 16));   //o
            glyphRect.Add(new Rectangle(374, 0, 5, 16));   //p
            glyphRect.Add(new Rectangle(380, 0, 7, 16));   //q
            glyphRect.Add(new Rectangle(388, 0, 5, 16));   //r
            glyphRect.Add(new Rectangle(394, 0, 6, 16));   //s
            glyphRect.Add(new Rectangle(401, 0, 5, 16));   //t
            glyphRect.Add(new Rectangle(407, 0, 7, 16));   //u
            glyphRect.Add(new Rectangle(414, 0, 7, 16));   //v
            glyphRect.Add(new Rectangle(422, 0, 11, 16));  //w
            glyphRect.Add(new Rectangle(434, 0, 6, 16));   //x
            glyphRect.Add(new Rectangle(441, 0, 7, 16));   //y
            glyphRect.Add(new Rectangle(449, 0, 6, 16));   //z


            glyphRect.Add(new Rectangle(600, 0, 8, 16));   //...

            
            charList.Add(' ');
            charList.Add('!');
            charList.Add('.');

            charList.Add('0');
            charList.Add('1');
            charList.Add('2');
            charList.Add('3');
            charList.Add('4');
            charList.Add('5');
            charList.Add('6');
            charList.Add('7');
            charList.Add('8');
            charList.Add('9');

            charList.Add(':');
            charList.Add('?');

            charList.Add('A');
            charList.Add('B');
            charList.Add('C');
            charList.Add('D');
            charList.Add('E');
            charList.Add('F');
            charList.Add('G');
            charList.Add('H');
            charList.Add('I');
            charList.Add('J');
            charList.Add('K');
            charList.Add('L');
            charList.Add('M');
            charList.Add('N');
            charList.Add('O');
            charList.Add('P');
            charList.Add('Q');
            charList.Add('R');
            charList.Add('S');
            charList.Add('T');
            charList.Add('U');
            charList.Add('V');
            charList.Add('W');
            charList.Add('X');
            charList.Add('Y');
            charList.Add('Z');

            charList.Add('a');
            charList.Add('b');
            charList.Add('c');
            charList.Add('d');
            charList.Add('e');
            charList.Add('f');
            charList.Add('g');
            charList.Add('h');
            charList.Add('i');
            charList.Add('j');
            charList.Add('k');
            charList.Add('l');
            charList.Add('m');
            charList.Add('n');
            charList.Add('o');
            charList.Add('p');
            charList.Add('q');
            charList.Add('r');
            charList.Add('s');
            charList.Add('t');
            charList.Add('u');
            charList.Add('v');
            charList.Add('w');
            charList.Add('x');
            charList.Add('y');
            charList.Add('z');

            charList.Add('§');

            int numberCaractere = charList.Count;


            /// NE CHANGE RIEN
            for (int i = 0; i < numberCaractere; i++)
                croppingList.Add(new Rectangle(0, 0, 16, 16));

            Vector3List.Add(new Vector3(0, -4, 0));//SPACE
            Vector3List.Add(new Vector3(0, -8, 0));//!
            Vector3List.Add(new Vector3(0, -8, 0));//.

            Vector3List.Add(new Vector3(0, -3, 0));//0
            Vector3List.Add(new Vector3(0, -4, 0));//1
            Vector3List.Add(new Vector3(0, -2, 0));//2
            Vector3List.Add(new Vector3(0, -3, 0));//3
            Vector3List.Add(new Vector3(0, -2, 0));//4
            Vector3List.Add(new Vector3(0, -2, 0));//5
            Vector3List.Add(new Vector3(0, -2, 0));//6
            Vector3List.Add(new Vector3(0, -2, 0));//7
            Vector3List.Add(new Vector3(0, -3, 0));//8
            Vector3List.Add(new Vector3(0, -3, 0));//9

            Vector3List.Add(new Vector3(0, -7, 0));//:
            Vector3List.Add(new Vector3(0, -3, 0));//?

            Vector3List.Add(new Vector3(0, 2, 0));//A
            Vector3List.Add(new Vector3(0, 0, 0));//B
            Vector3List.Add(new Vector3(0, 2, 0));//C
            Vector3List.Add(new Vector3(0, 1, 0));//D
            Vector3List.Add(new Vector3(0, 0, 0));//E
            Vector3List.Add(new Vector3(0, 0, 0));//F
            Vector3List.Add(new Vector3(0, 2, 0));//G
            Vector3List.Add(new Vector3(0, 0, 0));//H
            Vector3List.Add(new Vector3(0, -2, 0));//I
            Vector3List.Add(new Vector3(0, 1, 0));//J
            Vector3List.Add(new Vector3(0, 0, 0));//K
            Vector3List.Add(new Vector3(0, -1, 0));//L
            Vector3List.Add(new Vector3(0, 2, 0));//M
            Vector3List.Add(new Vector3(0, 0, 0));//N
            Vector3List.Add(new Vector3(0, 0, 0));//O
            Vector3List.Add(new Vector3(0, 0, 0));//P
            Vector3List.Add(new Vector3(0, 0, 0));//Q
            Vector3List.Add(new Vector3(0, 0, 0));//R
            Vector3List.Add(new Vector3(0, 0, 0));//S
            Vector3List.Add(new Vector3(0, 0, 0));//T
            Vector3List.Add(new Vector3(0, 0, 0));//U
            Vector3List.Add(new Vector3(0, 2, 0));//V
            Vector3List.Add(new Vector3(0, 6, 0));//W
            Vector3List.Add(new Vector3(0, 1, 0));//X
            Vector3List.Add(new Vector3(0, 2, 0));//Y
            Vector3List.Add(new Vector3(0, 0, 0));//Z
            
            Vector3List.Add(new Vector3(0, -3, 0));//a
            Vector3List.Add(new Vector3(0, -4, 0));//b
            Vector3List.Add(new Vector3(0, -3, 0));//c
            Vector3List.Add(new Vector3(0, -4, 0));//d
            Vector3List.Add(new Vector3(0, -4, 0));//e
            Vector3List.Add(new Vector3(0, -4, 0));//f
            Vector3List.Add(new Vector3(0, -3, 0));//g
            Vector3List.Add(new Vector3(0, -4, 0));//h
            Vector3List.Add(new Vector3(0, -4, 0));//i
            Vector3List.Add(new Vector3(0, -4, 0));//j
            Vector3List.Add(new Vector3(0, -4, 0));//k
            Vector3List.Add(new Vector3(0, -5, 0));//l
            Vector3List.Add(new Vector3(0, -2, 0));//m
            Vector3List.Add(new Vector3(0, -4, 0));//n
            Vector3List.Add(new Vector3(0, -2, 0));//o
            Vector3List.Add(new Vector3(0, -4, 0));//p
            Vector3List.Add(new Vector3(0, -2, 0));//q
            Vector3List.Add(new Vector3(0, -4, 0));//r
            Vector3List.Add(new Vector3(0, -3, 0));//s
            Vector3List.Add(new Vector3(0, -4, 0));//t
            Vector3List.Add(new Vector3(0, -3, 0));//u
            Vector3List.Add(new Vector3(0, -2, 0));//v
            Vector3List.Add(new Vector3(0, 2, 0));//w
            Vector3List.Add(new Vector3(0, -3, 0));//x
            Vector3List.Add(new Vector3(0, -2, 0));//y
            Vector3List.Add(new Vector3(0, -3, 0));//z


            Vector3List.Add(new Vector3(0, 0, 0));//...
            /// NE CHANGE RIEN


            UltimateFont = new SpriteFont(SuperFont, glyphRect, croppingList, charList, 0, 12f, Vector3List, '§');


            for (int i = 0; i < numberCaractere; i++)
            {
                glyphRect.Remove(glyphRect[0]);
                charList.Remove(charList[0]);
                croppingList.Remove(croppingList[0]);
                Vector3List.Remove(Vector3List[0]);
            }


        }


        public static void LevelSelector()
        {

            Console.WriteLine("{LEVEL SELECTOR} VER 2.0");
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



            Handler.Initialize();



            //Handler.players.Add(playerV2);

            Handler.Level = null;
            Handler.Level = new TileV2[LevelData.getLevel(LevelPlaying).GetLength(1), LevelData.getLevel(LevelPlaying).GetLength(0)];

            Handler.Walls = null;
            Handler.Walls = new Wall[LevelData.GetWallType(LevelPlaying).GetLength(1), LevelData.GetWallType(LevelPlaying).GetLength(0)];

            ThreadPool.QueueUserWorkItem(new WaitCallback(Level.LoadLevel), 1);

        }

        public static void LevelSelector(int level)
        {

            Console.WriteLine("{LEVEL SELECTOR} VER 3.0");

            LevelPlaying = level;

            Handler.Initialize();

            LightManager.Init();

            Handler.playersV2[1].Init();

            //playerV2 = new PlayerV2(Vector2.Zero, 1);

            //Handler.players.Add(playerV2);
            //Handler.players.Add(new PlayerV2(Vector2.Zero, 1));

            Handler.Level = null;
            Handler.Level = new TileV2[LevelData.getLevel(LevelPlaying).GetLength(1), LevelData.getLevel(LevelPlaying).GetLength(0)];

            Handler.Walls = null;
            Handler.Walls = new Wall[LevelData.GetWallType(LevelPlaying).GetLength(1), LevelData.GetWallType(LevelPlaying).GetLength(0)];

            ThreadPool.QueueUserWorkItem(new WaitCallback(Level.LoadLevel), 1);

            //while (!MapLoaded)
            //{
                //Console.WriteLine("Loading...");
            //}

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


        public static void Draw2dRefractionTechnique(string technique, Texture2D texture, Texture2D displacementTexture, Rectangle screenRectangle, float refractionSpeed, float refractiveIndex, float frequency, float sampleWavelength, Vector2 refractionVector, float refractionVectorRange, Vector2 windDirection, bool useWind, GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 displacement;
            double time = gameTime.TotalGameTime.TotalSeconds * refractionSpeed;
            if (useWind)
                displacement = -(Vector2.Normalize(windDirection) * (float)time);
            else
                displacement = new Vector2((float)Math.Cos(time), (float)Math.Sin(time));

            // Set an effect parameter to make the displacement texture scroll in a giant circle.
            refractionEffect.CurrentTechnique = refractionEffect.Techniques[technique];
            refractionEffect.Parameters["DisplacementTexture"].SetValue(displacementTexture);
            refractionEffect.Parameters["DisplacementMotionVector"].SetValue(displacement);
            refractionEffect.Parameters["SampleWavelength"].SetValue(sampleWavelength);
            refractionEffect.Parameters["Frequency"].SetValue(frequency);
            refractionEffect.Parameters["RefractiveIndex"].SetValue(refractiveIndex);
            // for the very last little test.
            refractionEffect.Parameters["RefractionVector"].SetValue(refractionVector);
            refractionEffect.Parameters["RefractionVectorRange"].SetValue(refractionVectorRange);

            refractionEffect.Parameters["SpriteTexture"].SetValue(texture);
            //refractionEffect.Parameters["pixelisation"].SetValue(0.5f);
            refractionEffect.Parameters["MousePosX"].SetValue(0.3f);
            //refractionEffect.Parameters["MousePosY"].SetValue(MouseInput.GetLevelPos(false, camera).Y);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, refractionEffect);
            spriteBatch.Draw(texture, screenRectangle, Color.White/**0.5f**/);
            spriteBatch.End();

            //DisplayName(screenRectangle, technique, useWind);
        }

        public void DisplayName(Rectangle screenRectangle, string technique, bool useWind)
        {
            spriteBatch.Begin();
            var offset = screenRectangle;
            offset.Location += new Point(20, 20);
            spriteBatch.DrawString(UltimateFont, technique, offset.Location.ToVector2(), Color.White);
            if (useWind)
            {
                offset.Location += new Point(0, 30);
                spriteBatch.DrawString(UltimateFont, "wind on", offset.Location.ToVector2(), Color.White);
            }
            spriteBatch.End();
        }

    }

}