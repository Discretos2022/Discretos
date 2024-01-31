using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Plateform_2D_v9.NetWorkEngine_3._0.Client;
using Plateform_2D_v9.NetWorkEngine_3._0.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    class Multiplay
    {

        private Main main;
        private Handler handler;

        private ParticleEffect SnowEffect;

        private ButtonV2 Level_3;
        private ButtonV1 Level_5;
        private ButtonV1 Level_7;

        public static Vector2 Wind = Vector2.Zero;

        public Multiplay(Handler handler, Main main)
        {
            this.main = main;
            this.handler = handler;
            //9
            SnowEffect = new ParticleEffect(1, -2, 1000, 1f);   //new ParticleEffect(1, 0, 8, 1f);

            Level_3 = new ButtonV2(Vector2.Zero, new Rectangle(0, 0, 0, 0), Color.White, Color.Black, Main.UltimateFont, null, "lv3");
            Level_5 = new ButtonV1(Vector2.Zero, new Rectangle(0, 0, 0, 0), Color.White, Color.Black, Main.UltimateFont, null, "lv5");
            Level_7 = new ButtonV1(Vector2.Zero, new Rectangle(0, 0, 0, 0), Color.White, Color.Black, Main.UltimateFont, null, "lv7");

        }

        public void Update(GameState state, GameTime gameTime, Screen screen)
        {

            #region Level

            if (Main.playState == PlayState.InLevel)
            {
                if (Main.MapLoaded)
                {

                    if (Main.LevelPlaying == 5)
                        SnowEffect.Update(gameTime);

                    if (Main.LevelPlaying == 4 || Main.LevelPlaying == 9)
                        Background.SetBackground(3);
                    else if (Main.LevelPlaying == 7)
                        Background.SetBackground(4);
                    else
                        Background.SetBackground(2);


                    handler.Update(gameTime);

                    
                    //if (NetPlay.client)
                      //  ClientSend.PlayerPos();


                    Main.camera.FollowObject(new Vector2(Handler.playersV2[(int)Client.playerID].GetPosForCamera().X + Handler.playersV2[(int)Client.playerID].GetRectangle().Width / 2, Handler.playersV2[(int)Client.playerID].GetPosForCamera().Y + Handler.playersV2[(int)Client.playerID].GetRectangle().Height / 2));

                    for (int j = 0; j < Handler.Level.GetLength(1); j++)
                        for (int i = 0; i < Handler.Level.GetLength(0); i++)
                            if (Handler.Level[i, j].getType() > 0)
                                Handler.Level[i, j].Update(gameTime);


                }


                if (KeyInput.getKeyState().IsKeyDown(Keys.K) && !KeyInput.getOldKeyState().IsKeyDown(Keys.K))
                {
                    Main.playState = PlayState.InWorldMap;
                    Camera.Zoom = 1f;
                }


            }

            #endregion

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
            if (Main.playState == PlayState.InWorldMap)
                if (Level_5.IsCliqued())
                {
                        
                    Main.MapLoaded = false;
                    Main.LevelSelector(5);
                    Main.playState = PlayState.InLevel;
                    Camera.Zoom = 4f;
                    Main.gameState = GameState.Multiplaying;

                    //ServerSend.Level(2, 5);

                    //if (Server.clients[3] != null)
                    //    ServerSend.Level(3, 5);
                    //if (Server.clients[4] != null)
                    //    ServerSend.Level(4, 5);
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
            if (Main.playState == PlayState.InWorldMap)
                if (Level_7.IsCliqued())
                {
                    Main.MapLoaded = false;
                    Main.LevelSelector(7);
                    Main.playState = PlayState.InLevel;
                    Camera.Zoom = 4f;
                    Main.gameState = GameState.Multiplaying;

                    //ServerSend.Level(2, 7);

                    //if (Server.clients[3] != null)
                    //    ServerSend.Level(3, 7);
                    //if (Server.clients[4] != null)
                    //    ServerSend.Level(4, 7);

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
            if (Main.playState == PlayState.InWorldMap)
                if (Level_3.IsCliqued())
                {
                    Main.MapLoaded = false;
                    Main.LevelSelector(3);
                    Main.playState = PlayState.InLevel;
                    Camera.Zoom = 4f;
                    Main.gameState = GameState.Multiplaying;

                    //ServerSend.Level(2, 3);

                    //if(Server.clients[3] != null)
                    //    ServerSend.Level(3, 3);
                    //if (Server.clients[4] != null)
                    //    ServerSend.Level(4, 3);

                }

            #endregion

            #region WorldMap

            if (Main.playState == PlayState.InWorldMap)
            {

                if (KeyInput.getKeyState().IsKeyDown(Keys.M) && !KeyInput.getOldKeyState().IsKeyDown(Keys.M))
                {
                    Main.MapLoaded = false;
                    Main.LevelSelector();
                    Main.playState = PlayState.InLevel;
                    Camera.Zoom = 4f;
                    Main.gameState = GameState.Playing;
                }


                Main.camera.FollowObjectInWorldMap(new Vector2(WorldMap.GetLevelSelectorPos().X, WorldMap.GetLevelSelectorPos().Y));
                if((int)Client.playerID == 1)
                    WorldMap.Update(gameTime);
            }

            #endregion



            //Console.WriteLine(Handler.playersV2.Count);

        }

        public void DrawInCamera(SpriteBatch spriteBatch, GameTime gameTime)
        {

            #region Level

            if (Main.playState == PlayState.InLevel && Main.MapLoaded)
            {

                handler.Draw(spriteBatch, gameTime);

                if (Main.LevelPlaying == 5)
                    SnowEffect.Draw(spriteBatch);


                if (Main.Money > 100 && Main.LevelPlaying == 5)
                {
                    SnowEffect.SetScale(1f);
                    SnowEffect.setWind(Util.UpperInteger(Wind.X) * 4);
                    SnowEffect.setIntensity(2000); // 2000
                    Wind = new Vector2(-1f, 0);           /// Take multiple of 2 for bug of tile

                }

                if (Main.LevelPlaying != 5)
                    Wind = Vector2.Zero;

            }

            #endregion

            #region WordMap

            if (Main.playState == PlayState.InWorldMap)
            {

                WorldMap.Draw(spriteBatch);

                Camera.Zoom = 4f;
            }

            #endregion


        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Main.playState == PlayState.InWorldMap && Client.playerID == Client.PlayerID.PLayerOne)
            {
                Level_3.setPos(ButtonV2.Position.centerX, y: 200);
                Level_3.Draw(Main.UltimateFont, 4f, spriteBatch, false, 8 * 5 - 4);
                Level_3.setScale(5);

                Level_5.Draw(Main.UltimateFont, new Vector2((1920 / 2) - Level_5.getDimension().Width / 2, 300), 5, 4f, spriteBatch, false, 8 * 5 - 4);

                Level_7.Draw(Main.UltimateFont, new Vector2((1920 / 2) - Level_7.getDimension().Width / 2, 400), 5, 4f, spriteBatch, false, 8 * 5 - 4);

            }
            
            DrawHUD(spriteBatch);

            Writer.DrawText(Main.UltimateFont, "money : " + Main.Money + " $ ", new Vector2(1, 1), Color.Black, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f, 3f, spriteBatch);
            Writer.DrawText(Main.UltimateFont, "debug :  " + Main.PlayerPos + " $ ", new Vector2(1, 45), Color.Black, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f, 3f, spriteBatch);

            //Main.camera.Follow(MouseInput.GetPos());


            /*if (Main.inWorldMap)
                if (KeyInput.getKeyState().IsKeyDown(Keys.M) && !KeyInput.getOldKeyState().IsKeyDown(Keys.M))
                {
                    Main.LevelSelector();
                    Main.inLevel = true;
                    Main.inWorldMap = false;
                    Camera.Zoom = 4f;
                }*/

        }


        public void DrawHUD(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Main.BlackBar, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            if (Main.Debug)
            {
                Writer.DrawText(Main.UltimateFont, "player " + (int)Client.playerID, new Vector2(5, 150), Color.Black, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f, 3f, spriteBatch, false);
                Writer.DrawText(Main.UltimateFont, "socket : " + Client.IsConnected().ToString().ToLower(), new Vector2(5, 200), Color.Black, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f, 3f, spriteBatch, false);
                //Writer.DrawText(Main.UltimateFont, "client : " + Client.instance.tcp.socket.Client.Connected.ToString().ToLower(), new Vector2(5, 250), Color.Black, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f, 3f, spriteBatch, false);
                // Writer.DrawText(Main.UltimateFont, "stream : " + Client.instance.tcp.socket.GetStream().CanTimeout.ToString().ToLower(), new Vector2(5, 300), Color.Black, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f, 3f, spriteBatch, false);
            }
            

            if (Handler.playersV2.ContainsKey((int)Client.playerID))
                for (int i = 0; i < Handler.playersV2[(int)Client.playerID].PV; i++)
                {
                    if (i > 9)
                        spriteBatch.Draw(Main.Object[2], new Rectangle(500 + (i - 10) * 60, 60, 15 * 4, 14 * 4), new Rectangle(0, 0, 15, 15), Color.White);
                    else
                        spriteBatch.Draw(Main.Object[2], new Rectangle(500 + i * 60, 0, 15 * 4, 14 * 4), new Rectangle(0, 0, 15, 15), Color.White);
                }

        }
    }
}
