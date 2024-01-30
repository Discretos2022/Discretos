using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Plateform_2D_v9
{
    class MultiplayerMode
    {

        //Button
        //private ButtonV1 CreateServer;
        //private ButtonV1 Connect;


        private ButtonV3 CreateServer;
        private ButtonV3 Connect;
        private ButtonV3 Back;

        public List<ButtonV3> buttons;

        public MultiplayerMode()
        {

            //CreateServer = new ButtonV1(Vector2.Zero, new Rectangle(0, 0, 0, 0), Color.White, Color.Black, Main.UltimateFont, null, "heberger et jouer");
            //Connect = new ButtonV1(Vector2.Zero, new Rectangle(0, 0, 0, 0), Color.White, Color.Black, Main.UltimateFont, null, "se connecter");



            CreateServer = new ButtonV3();
            Connect = new ButtonV3();
            Back = new ButtonV3();

            InitButton();

            buttons = new List<ButtonV3>();

            buttons.Add(CreateServer);
            buttons.Add(Connect);
            buttons.Add(Back);

        }


        public void Update(GameState state, GameTime gameTime, Screen screen)
        {

            #region CreateServerButton

            CreateServer.Update(gameTime, screen);

            if (CreateServer.IsSelected())
            {
                CreateServer.SetColor(Color.Gray, Color.Black);
            }
            else
            {
                CreateServer.SetColor(Color.White, Color.Black);
            }

            if (CreateServer.IsCliqued())
            {
                Main.inWorldMap = true;
                Main.inLevel = false;
                Camera.Zoom = 1f;
                Main.gameState = GameState.CreateServer;

            }

            #endregion

            #region ConnectButton

            Connect.Update(gameTime, screen);

            if (Connect.IsSelected())
            {
                Connect.SetColor(Color.Gray, Color.Black);
            }
            else
            {
                Connect.SetColor(Color.White, Color.Black);
            }

            if (Connect.IsCliqued())
            {
                Main.inWorldMap = true;
                Main.inLevel = false;
                Camera.Zoom = 1f;
                Main.gameState = GameState.ConnectToServer;
            }

            #endregion

            #region ConnectButton

            Back.Update(gameTime, screen);

            if (Back.IsSelected())
                Back.SetColor(Color.Gray, Color.Black);
            else
                Back.SetColor(Color.White, Color.Black);

            if (Back.IsCliqued())
            {
                Main.gameState = GameState.Menu;
            }

            #endregion


        }


        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            //CreateServer.Draw(Main.UltimateFont, new Vector2((1920 / 2) - CreateServer.getDimension().Width / 2, 300), 5, 4f, spriteBatch, false, 8 * 5 - 4);

            //if(!NetPlay.clientsock.IsDisconnected())
            //Connect.Draw(Main.UltimateFont, new Vector2((1920 / 2) - Connect.getDimension().Width / 2, 400), 5, 4f, spriteBatch, false, 8 * 5 - 4);


            Writer.DrawText(Main.UltimateFont, "multiplayer", new Vector2((1920 / 2) - (Main.UltimateFont.MeasureString("multiplayer").X * 8f + 9 * 8f) / 2, 25 - 15), new Color(60,60,60), Color.LightGray, 0f, Vector2.Zero, 8f, SpriteEffects.None, 0f, 6f, spriteBatch, Color.Black, false);


            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Draw(spriteBatch);
            }


            Writer.DrawText(Main.UltimateFont, "ip : " + Main.IP, new Vector2(1640, 1040), Color.Black, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f, 2f, spriteBatch);

        }


        public void InitButton()
        {

            CreateServer.SetText("host the game");
            CreateServer.SetColor(Color.White, Color.Black);
            CreateServer.SetFont(Main.UltimateFont);
            CreateServer.SetScale(5);
            CreateServer.IsMajuscule(false);
            CreateServer.SetFrontThickness(4);
            CreateServer.SetAroundButton(Back, Connect);
            CreateServer.SetPosition(0, 400, ButtonV3.Position.centerX);

            Connect.SetText("join via ip");
            Connect.SetColor(Color.White, Color.Black);
            Connect.SetFont(Main.UltimateFont);
            Connect.SetScale(5);
            Connect.IsMajuscule(false);
            Connect.SetFrontThickness(4);
            Connect.SetAroundButton(CreateServer, CreateServer);
            Connect.SetPosition(0, CreateServer.GetPosition().Y + 100, ButtonV3.Position.centerX);

            Back.SetText("back");
            Back.SetColor(Color.White, Color.Black);
            Back.SetFont(Main.UltimateFont);
            Back.SetScale(5);
            Back.IsMajuscule(false);
            Back.SetFrontThickness(4);
            Back.SetAroundButton(Connect, CreateServer);
            Back.SetPosition(0, Connect.GetPosition().Y + 100, ButtonV3.Position.centerX);

        }


    }
}
