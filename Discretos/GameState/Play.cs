using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetworkEngine_5._0.Client;
using NetworkEngine_5._0.Server;
using Plateform_2D_v9.NetCore;
using System;
using System.Threading;

namespace Plateform_2D_v9
{
    class Play
    {
        private Handler handler;

        private ButtonV2 Level_3;
        private ButtonV1 Level_5;
        private ButtonV1 Level_7;


        private ButtonV3 Resume;
        private ButtonV3 Restart;
        private ButtonV3 QuitLevel;


        public static Vector2 Wind = Vector2.Zero;

        public Play(Handler handler, Main main)
        {
            this.handler = handler;

            ParticleEffectV2.Actived = true;
            ParticleEffectV2.setIntensity(50);
            ParticleEffectV2.type = 1;
            ParticleEffectV2.SetScale(1f);
            ParticleEffectV2.setWind(-1);

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
            {
                Main.isPaused = false;
                if (NetPlay.IsMultiplaying && NetPlay.MyPlayerID() == 1)
                    ServerSender.SendGamePaused(Main.isPaused);
            }
            else if (KeyInput.getKeyState().IsKeyDown(Keys.P) && !KeyInput.getOldKeyState().IsKeyDown(Keys.P) && !Main.isPaused)
            {
                Main.isPaused = true;
                if (NetPlay.IsMultiplaying && NetPlay.MyPlayerID() == 1)
                    ServerSender.SendGamePaused(Main.isPaused);
            }

            if (NetPlay.IsMultiplaying)
            {
                if (Client.IsLostConnection())
                {
                    Main.gameState = GameState.ConnectToServer;
                    NetPlay.IsMultiplaying = false;
                }
            }

            if (Main.inLevel)
            {
                UpdateLevel(gameTime, screen);

                if(Main.isPaused)
                    UpdateLvPausedHUD(gameTime, screen);
            }

            if (Main.inWorldMap)
                UpdateWorldMap(gameTime, screen);

            if (NetPlay.MyPlayerID() == 1)
            {
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
                        Main.StartLevel(5);
                        Main.gameState = GameState.Playing;
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
                        Main.StartLevel(7);
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
                        Main.StartLevel(3);
                        Main.gameState = GameState.Playing;
                    }

                #endregion
            }


        }

        public void DrawInCamera(SpriteBatch spriteBatch, GameTime gameTime)
        {

            if (Main.inLevel && Main.MapLoaded)
            {

                handler.Draw(spriteBatch, gameTime);

                if (ParticleEffectV2.Actived)
                    ParticleEffectV2.Draw(spriteBatch);

                if(Main.Money > 100 && Main.LevelPlaying == 5)
                {
                    ParticleEffectV2.SetScale(1f);
                    ParticleEffectV2.setWind(Util.UpperInteger(Wind.X) * 4);
                    Wind = new Vector2(-1f, 0);           /// Take multiple of 2 for bug of tile

                    //Handler.earthquake = 1;

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

            if (Main.inLevel && !Main.MapLoaded)
                spriteBatch.Draw(Main.Bounds, new Rectangle(0, 0, 1920, 1080), Color.Black);


            if (Main.inWorldMap && NetPlay.MyPlayerID() == 1)
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

                if (Main.LevelPlaying == 5 || Main.LevelPlaying == 7)
                    ParticleEffectV2.Update(gameTime);

                if (Main.LevelPlaying == 4 || Main.LevelPlaying == 9)
                    Background.SetBackground(3);
                else if (Main.LevelPlaying == 7)
                    Background.SetBackground(16); // 4
                else if (Main.LevelPlaying == 3 || Main.LevelPlaying == 11)
                    Background.SetBackground(6, 7, 8, 9, 10, 11);
                else if (Main.LevelPlaying == 8)
                    Background.SetBackground(3);
                else if (Main.LevelPlaying == 8)
                    Background.SetBackground(6, 7, 8, 9, 10, 11);
                else if (Main.LevelPlaying == 10)
                    Background.SetBackground(6, 7, 8, 9, 10, 11);
                else if(Main.LevelPlaying == 6)
                    Background.SetBackground(15, 15, 15); //Background.SetBackground(13, 14);
                else
                    Background.SetBackground(2);

                if (Main.LevelPlaying == 10)
                    Background.SetBaseBackgroundPos(0, 200);
                else if (Main.LevelPlaying == 3)
                    Background.SetBaseBackgroundPos(0, 12);
                else
                    Background.SetBaseBackgroundPos(0, 0);

                handler.Update(gameTime);

                Main.camera.FollowObject(new Vector2(Handler.playersV2[NetPlay.MyPlayerID()].GetPosForCamera().X + Handler.playersV2[NetPlay.MyPlayerID()].GetRectangle().Width / 2, Handler.playersV2[NetPlay.MyPlayerID()].GetPosForCamera().Y + Handler.playersV2[NetPlay.MyPlayerID()].GetRectangle().Height / 2));

                for (int j = 0; j < Handler.Level.GetLength(1); j++)
                    for (int i = 0; i < Handler.Level.GetLength(0); i++)
                        if (Handler.Level[i, j].ID > 0)
                            Handler.Level[i, j].Update(gameTime);


                if (NetPlay.IsMultiplaying)
                {
                    if (NetPlay.MyPlayerID() == 1)
                    {
                        ServerSender.SendPositionPlayer(1, Handler.playersV2[1].Position.X, Handler.playersV2[1].Position.Y, Handler.playersV2[1].isRight);
                    }

                    if (NetPlay.MyPlayerID() != 1) 
                    {
                        ClientSender.SendPositionPlayer(NetPlay.MyPlayerID(), Handler.playersV2[NetPlay.MyPlayerID()].Position.X, Handler.playersV2[NetPlay.MyPlayerID()].Position.Y, Handler.playersV2[NetPlay.MyPlayerID()].isRight);
                    }

                }


            }

        }

        public void UpdateWorldMap(GameTime gameTime, Screen screen)
        {
            ParticleEffectV2.Actived = false;

            if (KeyInput.getKeyState().IsKeyDown(Keys.M) && !KeyInput.getOldKeyState().IsKeyDown(Keys.M))
            {
                Main.LevelSelector();
            }

            WorldMap.Update(gameTime);

            Main.camera.FollowObjectInWorldMap(new Vector2(WorldMap.GetLevelSelectorPos().X, WorldMap.GetLevelSelectorPos().Y));

            if (NetPlay.MyPlayerID() == 1)
            {

                if(NetPlay.IsMultiplaying)
                    ServerSender.SendWorldMapPositionPlayer((int)WorldMap.GetLevelSelectorPos().X, (int)WorldMap.GetLevelSelectorPos().Y);
            }
                

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
            {
                Main.isPaused = false;
                if (NetPlay.IsMultiplaying && NetPlay.MyPlayerID() == 1)
                    ServerSender.SendGamePaused(Main.isPaused);
            }
                

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
                LightManager.isLightEnable = false;
            }


            #endregion

        }

        public void DrawHUD(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Main.BlackBar, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            for (int i = 0; i < 6; i++)
            {
                if(i < Handler.playersV2[NetPlay.MyPlayerID()].PV)
                    spriteBatch.Draw(Main.ObjectInterface, new Rectangle(1920 / 2 + i * 60 - 15 * 12, 8, 15 * 4, 14 * 4), new Rectangle(0, 0, 15, 15), Color.White);
                else 
                    spriteBatch.Draw(Main.ObjectInterface, new Rectangle(1920 / 2 + i * 60 - 15 * 12, 8, 15 * 4, 14 * 4), new Rectangle(15, 0, 15, 15), Color.White);
            }

            spriteBatch.Draw(Main.ObjectInterface, new Rectangle(10, 10, 15 * 4, 14 * 4), new Rectangle(30, 0, 15, 15), Color.White);
            Writer.DrawText(Main.ScoreFont, Main.Money.ToString("00000"), new Vector2(80, 18), Color.Black, Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f, 4f, spriteBatch);

            ///Writer.DrawSuperText(Main.ScoreFont, Main.Money.ToString("00000"), new Vector2(0, 18), Color.Black, Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f, 4f, spriteBatch);


            if (Main.Debug)
                Writer.DrawText(Main.UltimateFont, "dust : " + ParticleEffectV2.particles.Count, new Vector2(1500, 1), Color.Black, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f, 3f, spriteBatch);

            if (Main.isPaused && Main.inLevel)
            {
                spriteBatch.Draw(Main.Bounds, new Rectangle(0, 0, 1920, 1080), null, Color.Black * 0.5f, 0f, Vector2.Zero, SpriteEffects.None, 0f);

                Resume.Draw(spriteBatch);
                Restart.Draw(spriteBatch);
                QuitLevel.Draw(spriteBatch);

                InitButton();

            }

            if (Main.inWorldMap)
                if (WorldMap.GetNode().level != 0)
                    Writer.DrawText(Main.UltimateFont, "level " + WorldMap.GetNode().level + "-" + 1, new Vector2(50, 100), Color.Black, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f, 4f, spriteBatch);



        }


        public static void PlayLvl(int level)
        {
            Main.MapLoaded = false;
            Main.LevelSelector(level);
            Main.inWorldMap = false;
            Main.inLevel = true;
            Camera.Zoom = 4f;
            Main.gameState = GameState.Playing;
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