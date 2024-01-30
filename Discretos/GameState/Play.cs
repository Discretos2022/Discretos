using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;

namespace Plateform_2D_v9
{
    class Play
    {
        private Main main;
        private Handler handler;

        private ParticleEffect SnowEffect;

        private ButtonV2 Level_3;
        private ButtonV1 Level_5;
        private ButtonV1 Level_7;


        private ButtonV3 Resume;
        private ButtonV3 Restart;
        private ButtonV3 QuitLevel;


        public static Vector2 Wind = Vector2.Zero;

        public Play(Handler handler, Main main)
        {
            this.main = main;
            this.handler = handler;
                                                 //9
            SnowEffect = new ParticleEffect(1, -1, 50, 1f);   //new ParticleEffect(1, 0, 8, 1f);

            SnowEffect.Actived = true;

            Level_3 = new ButtonV2(Vector2.Zero, new Rectangle(0, 0, 0, 0), Color.White, Color.Black, Main.UltimateFont, null, "lv3");
            Level_5 = new ButtonV1(Vector2.Zero, new Rectangle(0, 0, 0, 0), Color.White, Color.Black, Main.UltimateFont, null, "lv5");
            Level_7 = new ButtonV1(Vector2.Zero, new Rectangle(0, 0, 0, 0), Color.White, Color.Black, Main.UltimateFont, null, "lv7");

            Resume = new ButtonV3();
            Restart = new ButtonV3();
            QuitLevel = new ButtonV3();

            InitButton();

        }

        public void Update(GameState state, GameTime gameTime, Screen screen)
        {
            if (KeyInput.getKeyState().IsKeyDown(Keys.P) && !KeyInput.getOldKeyState().IsKeyDown(Keys.P) && Main.isPaused)
                Main.isPaused = false;
            else if (KeyInput.getKeyState().IsKeyDown(Keys.P) && !KeyInput.getOldKeyState().IsKeyDown(Keys.P) && !Main.isPaused)
                Main.isPaused = true;

            if (Main.inLevel)
            {
                UpdateLevel(gameTime, screen);

                if(Main.isPaused)
                    UpdateLvPausedHUD(gameTime, screen);
            }

            if (Main.inWorldMap)
                UpdateWorldMap(gameTime, screen);

            #region Button

            Level_5.Update(screen);

            if (Level_5.IsSelected())
            {
                Level_5.SetColor(Color.Gray, Color.Black);
            }
            else
            {
                Level_5.SetColor(Color.White, Color.Black);
            }
            if (Main.inWorldMap)
                if (Level_5.IsCliqued())
                {

                    if (NetPlay.IsMultiplaying)
                    {


                        //if (NetPlay.serversock.client != null)
                        //{
                        //    Main.MapLoaded = false;
                        //    Main.LevelSelector(5);
                        //    Main.inWorldMap = false;
                        //    Main.inLevel = true;
                        //    Camera.Zoom = 4f;
                        //    Main.gameState = GameState.Playing;

                        //    NetPlay.serversock.Send(Main.LevelPlaying);
                        //}
                    }
                    else
                    {
                        Main.MapLoaded = false;
                        Main.LevelSelector(5);
                        Main.inWorldMap = false;
                        Main.inLevel = true;
                        Camera.Zoom = 4f;
                        Main.gameState = GameState.Playing;

                        SnowEffect.Actived = false;
                        ThreadPool.QueueUserWorkItem(new WaitCallback(SnowEffect.Generate), 1);

                    }


                }

            Level_7.Update(screen);

            if (Level_7.IsSelected())
            {
                Level_7.SetColor(Color.Gray, Color.Black);
            }
            else
            {
                Level_7.SetColor(Color.White, Color.Black);
            }
            if (Main.inWorldMap)
                if (Level_7.IsCliqued())
                {
                    Main.MapLoaded = false;
                    Main.LevelSelector(7);
                    Main.inWorldMap = false;
                    Main.inLevel = true;
                    Camera.Zoom = 4f;
                    Main.gameState = GameState.Playing;

                }

            Level_3.Update(screen);

            if (Level_3.IsSelected())
            {
                Level_3.SetColor(Color.Gray, Color.Black);
            }
            else
            {
                Level_3.SetColor(Color.White, Color.Black);
            }
            if (Main.inWorldMap)
                if (Level_3.IsCliqued())
                {
                    Main.MapLoaded = false;
                    Main.LevelSelector(3);
                    Main.inWorldMap = false;
                    Main.inLevel = true;
                    Camera.Zoom = 4f;
                    Main.gameState = GameState.Playing;

                }

            #endregion

        }

        public void DrawInCamera(SpriteBatch spriteBatch, GameTime gameTime)
        {

            if (Main.inLevel && Main.MapLoaded)
            {

                handler.Draw(spriteBatch, gameTime);

                if (Main.LevelPlaying == 5)
                    SnowEffect.Draw(spriteBatch);

                if(Main.Money > 100 && Main.LevelPlaying == 5)
                {
                    SnowEffect.SetScale(1f);
                    SnowEffect.setWind(Util.UpperInteger(Wind.X) * 4);
                    SnowEffect.setIntensity(100); // 2000
                    Wind = new Vector2(-1f, 0);           /// Take multiple of 2 for bug of tile

                }

                if (Main.LevelPlaying != 5)
                    Wind = Vector2.Zero;

            }

            if (Main.inWorldMap && !Main.inLevel)
            {
                WorldMap.Draw(spriteBatch);
                Camera.Zoom = 4f;
            }


        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Main.inWorldMap)
            {
                Level_3.setPos(ButtonV2.Position.centerX, y: 200);
                Level_3.Draw(Main.UltimateFont, 4f, spriteBatch, false, 8 * 5 - 4);
                Level_3.setScale(5);

                Level_5.Draw(Main.UltimateFont, new Vector2((1920 / 2) - Level_5.getDimension().Width / 2, 300), 5, 4f, spriteBatch, false, 8 * 5 - 4);

                Level_7.Draw(Main.UltimateFont, new Vector2((1920 / 2) - Level_7.getDimension().Width / 2, 400), 5, 4f, spriteBatch, false, 8 * 5 - 4);

            }

            DrawHUD(spriteBatch);

            if (Main.inLevel)
                if (KeyInput.getKeyState().IsKeyDown(Keys.K) && !KeyInput.getOldKeyState().IsKeyDown(Keys.K))
                {
                    Main.inLevel = false;
                    Main.inWorldMap = true;
                    Camera.Zoom = 4f;
                }

            

        }


        public void UpdateLevel(GameTime gameTime, Screen screen)
        {

            if (Main.MapLoaded && !Main.isPaused)
            {

                if (Main.LevelPlaying == 5)
                    SnowEffect.Update(gameTime);

                if (Main.LevelPlaying == 4 || Main.LevelPlaying == 9)
                    Background.SetBackground(3);
                else if (Main.LevelPlaying == 7)
                    Background.SetBackground(4);
                else if (Main.LevelPlaying == 3)
                    Background.SetBackground(6, 7, 8, 9, 10, 11);
                else if (Main.LevelPlaying == 8)
                    Background.SetBackground(3);
                else if (Main.LevelPlaying == 8)
                    Background.SetBackground(6, 7, 8, 9, 10, 11);
                else if (Main.LevelPlaying == 10)
                    Background.SetBackground(6, 7, 8, 9, 10, 11);
                else if(Main.LevelPlaying == 6)
                    Background.SetBackground(13, 14);
                else
                    Background.SetBackground(2);

                if (Main.LevelPlaying == 10)
                    Background.SetBaseBackgroundPos(0, 200);
                else if (Main.LevelPlaying == 3)
                    Background.SetBaseBackgroundPos(0, 12);
                else
                    Background.SetBaseBackgroundPos(0, 0);

                handler.Update(gameTime);


                Main.camera.FollowObject(new Vector2(Handler.playersV2[1].GetPosForCamera().X + Handler.playersV2[1].GetRectangle().Width / 2, Handler.playersV2[1].GetPosForCamera().Y + Handler.playersV2[1].GetRectangle().Height / 2));

                for (int j = 0; j < Handler.Level.GetLength(1); j++)
                    for (int i = 0; i < Handler.Level.GetLength(0); i++)
                        if (Handler.Level[i, j].getType() > 0)
                            Handler.Level[i, j].Update(gameTime);

            }

        }

        public void UpdateWorldMap(GameTime gameTime, Screen screen)
        {

            if (KeyInput.getKeyState().IsKeyDown(Keys.M) && !KeyInput.getOldKeyState().IsKeyDown(Keys.M))
            {
                Main.MapLoaded = false;
                Main.LevelSelector();
                Main.inWorldMap = false;
                Main.inLevel = true;
                Camera.Zoom = 4f;
                Main.gameState = GameState.Playing;
            }

            Main.camera.FollowObjectInWorldMap(new Vector2(WorldMap.GetLevelSelectorPos().X, WorldMap.GetLevelSelectorPos().Y));
            WorldMap.Update(gameTime);

        }

        public void UpdateLvPausedHUD(GameTime gameTime, Screen screen)
        {

            #region Resume

            Resume.Update(gameTime, screen);

            if (Resume.IsSelected())
                Resume.SetColor(Color.Gray, Color.Black);
            else
                Resume.SetColor(Color.White, Color.Black);

            if (Resume.IsCliqued())
                Main.isPaused = false;

            #endregion

            #region Restart

            Restart.Update(gameTime, screen);

            if (Restart.IsSelected())
                Restart.SetColor(Color.Gray, Color.Black);
            else
                Restart.SetColor(Color.White, Color.Black);

            if (Restart.IsCliqued())
            {
                Handler.playersV2[1].KillPlayer();
                Main.isPaused = false;
            }
                


            #endregion

            #region QuitLevel

            QuitLevel.Update(gameTime, screen);

            if (QuitLevel.IsSelected())
                QuitLevel.SetColor(Color.Gray, Color.Black);
            else
                QuitLevel.SetColor(Color.White, Color.Black);

            if (QuitLevel.IsCliqued())
            {
                Main.MapLoaded = false;
                Main.inWorldMap = true;
                Main.inLevel = false;
                Camera.Zoom = 4f;
                Main.gameState = GameState.Playing;
                Main.isPaused = false;
            }


            #endregion

        }

        public void DrawHUD(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Main.BlackBar, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            for(int i = 0; i < Handler.playersV2[1].PV; i++)
            {
                if (i > 9)
                    spriteBatch.Draw(Main.Object[2], new Rectangle(500 + (i - 10) * 60, 60, 15 * 4, 14 * 4), new Rectangle(0, 0, 15, 15), Color.White);
                else
                    spriteBatch.Draw(Main.Object[2], new Rectangle(500 + i * 60, 0, 15 * 4, 14 * 4), new Rectangle(0, 0, 15, 15), Color.White);
            }

            Writer.DrawText(Main.UltimateFont, "money : " + Main.Money + " $ ", new Vector2(1, 1), Color.Black, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f, 3f, spriteBatch);
            Writer.DrawText(Main.UltimateFont, "debug :  " + Main.PlayerPos + " $ ", new Vector2(1, 45), Color.Black, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f, 3f, spriteBatch);


            if (Main.Debug)
                Writer.DrawText(Main.UltimateFont, "dust : " + ParticleEffect.particles.Count, new Vector2(1000, 1), Color.Black, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f, 3f, spriteBatch);

            if (Main.isPaused && Main.inLevel)
            {
                spriteBatch.Draw(Main.Bounds, new Rectangle(0, 0, 1920, 1080), null, Color.Black * 0.5f, 0f, Vector2.Zero, SpriteEffects.None, 0f);

                Resume.Draw(spriteBatch);
                Restart.Draw(spriteBatch);
                QuitLevel.Draw(spriteBatch);

                InitButton();

            }


        }


        public void InitButton()
        {

            Color col1 = Color.White;
            Color col2 = Color.Gold;

            Resume.SetText("resume");
            Resume.SetColor(col1, col2);
            Resume.SetFont(Main.UltimateFont);
            Resume.SetScale(5);
            Resume.IsMajuscule(false);
            Resume.SetFrontThickness(4);
            Resume.SetAroundButton(null, Restart);
            Resume.SetPosition(0, -100, ButtonV3.Position.centerXY);

            Restart.SetText("restart");
            Restart.SetColor(col1, col2);
            Restart.SetFont(Main.UltimateFont);
            Restart.SetScale(5);
            Restart.IsMajuscule(false);
            Restart.SetFrontThickness(4);
            Restart.SetAroundButton(Resume, QuitLevel);
            Restart.SetPosition(0, 0, ButtonV3.Position.centerXY);

            QuitLevel.SetText("exit level");
            QuitLevel.SetColor(col1, col2);
            QuitLevel.SetFont(Main.UltimateFont);
            QuitLevel.SetScale(5);
            QuitLevel.IsMajuscule(false);
            QuitLevel.SetFrontThickness(4);
            QuitLevel.SetAroundButton(Restart);
            QuitLevel.SetPosition(0, 100, ButtonV3.Position.centerXY);


        }


    }
}