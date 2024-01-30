using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

using Plateform_2D_v9.NetWorkEngine_3._0.Server;

namespace Plateform_2D_v9
{
    class CreateServer
    {

        public List<ButtonV3> multiSettingsButtons;
        public List<ButtonV3> waitingPlayersButtons;

        private ButtonV3 LaunchServer; /// Start
        private ButtonV3 Back;
        private ButtonV3 Cancel;
        private ButtonV3 Play;

        //private bool serverlaunched = false;

        //private string message = "";

        public string serverInfo = "";

        public TextBox textBoxPort;
        public ButtonV3 PortButton;

        private State serverState = State.SelectePort;

        public int numOfPlayer = 2;
        //public Client playerOne;


        public CreateServer()
        {

            multiSettingsButtons = new List<ButtonV3>();
            waitingPlayersButtons = new List<ButtonV3>();

            PortButton = new ButtonV3();

            LaunchServer = new ButtonV3();
            Back = new ButtonV3();
            Cancel = new ButtonV3();
            Play = new ButtonV3();

            InitButton();

            multiSettingsButtons.Add(PortButton);
            multiSettingsButtons.Add(LaunchServer);
            multiSettingsButtons.Add(Back);

            waitingPlayersButtons.Add(Cancel);
            waitingPlayersButtons.Add(Play);

            textBoxPort = new TextBox(5, 4, 4, false, "7777", true, true);
            textBoxPort.SetPosition(0, 302, ButtonV3.Position.centerX);
            textBoxPort.Update();

        }


        public void Update(GameState state, GameTime gameTime, Screen screen)
        {

            if(serverState == State.SelectePort)
            {
                int port = 0;

                if (textBoxPort.GetText().Length == 0)
                    port = 7777;
                else
                    port = int.Parse(textBoxPort.GetText());

                #region LaunchServerButton

                LaunchServer.Update(gameTime, screen);

                if (LaunchServer.IsSelected())
                    LaunchServer.SetColor(Color.Gray, Color.Black);
                else
                    LaunchServer.SetColor(Color.White, Color.Black);

                if (port > Main.MaxPort)
                    LaunchServer.SetColor(Color.DarkGray, Color.Gray);

                if (LaunchServer.IsCliqued() && !Server.IsLaunched())
                {

                    if (port > Main.MaxPort)
                        textBoxPort.SetColor(Color.Red, Color.Black);
                    else
                    {
                        Server.Start(4, port); // Le server est compté dedans !
                        textBoxPort.SetColor(Color.White, Color.Black);
                        serverState = State.WaitPlayer;

                    }

                }

                #endregion

                #region BackButton

                Back.Update(gameTime, screen);

                if (Back.IsSelected())
                    Back.SetColor(Color.Gray, Color.Black);
                else
                    Back.SetColor(Color.White, Color.Black);

                if (Back.IsCliqued())
                    Main.gameState = GameState.MultiplayerMode;

                #endregion

                #region PortButton

                PortButton.Update(gameTime, screen);

                if (PortButton.IsSelected())
                    PortButton.SetTexture(Main.PortBox, new Rectangle(53, 0, 52, 16));
                else
                {
                    if (textBoxPort.isSelected)
                        PortButton.SetTexture(Main.PortBox, new Rectangle(106, 0, 52, 16));
                    else
                        PortButton.SetTexture(Main.PortBox, new Rectangle(0, 0, 52, 16));
                }


                if (PortButton.IsCliqued())
                { textBoxPort.isSelected = true; }

                #endregion

                if (port > Main.MaxPort)
                    textBoxPort.SetColor(Color.Red, Color.Black);
                else
                    textBoxPort.SetColor(Color.White, Color.Black);

                if (textBoxPort.isSelected)
                    textBoxPort.Update();

                if (MouseInput.isSimpleClickLeft())
                {
                    if (!PortButton.IsSelected())
                        textBoxPort.isSelected = false;
                }

            }

            else if(serverState == State.WaitPlayer)
            {

                #region CancelButton

                Cancel.Update(gameTime, screen);

                if (Cancel.IsSelected())
                    Cancel.SetColor(Color.Gray, Color.Black);
                else
                    Cancel.SetColor(Color.White, Color.Black);

                if (Cancel.IsCliqued())
                {

                    Server.Stop();

                    serverState = State.SelectePort;

                }

                #endregion

                #region PlayButton

                Play.Update(gameTime, screen);

                if (Play.IsSelected())
                    Play.SetColor(Color.Gray, Color.Black);
                else
                    Play.SetColor(Color.White, Color.Black);

                if(Server.numOfClient == 1)
                    Play.SetColor(Color.DarkGray, Color.Gray);

                if (Play.IsCliqued())
                {
                    if (Server.numOfClient >= 2)
                    {
                        Main.playState = PlayState.InWorldMap;
                        Camera.Zoom = 1f;
                        Main.gameState = GameState.Multiplaying;
                    }

                }

                #endregion


            }

        }


        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if(serverState == State.SelectePort)
            {

                Writer.DrawText(Main.UltimateFont, "multiplayer settings", new Vector2((1920 / 2) - (Main.UltimateFont.MeasureString("multiplayer settings").X * 8f + 9 * 8f) / 2, 25 - 15), new Color(60, 60, 60), Color.LightGray, 0f, Vector2.Zero, 8f, SpriteEffects.None, 0f, 6f, spriteBatch, Color.Black, false);


                for (int i = 0; i < multiSettingsButtons.Count; i++)
                {
                    multiSettingsButtons[i].Draw(spriteBatch);
                }


                Writer.DrawText(Main.UltimateFont, "ip : " + Main.IP, new Vector2(1640, 1040), Color.Black, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f, 2f, spriteBatch);

                textBoxPort.Draw(spriteBatch);

                serverInfo = "select port for connection. the max is 65535. default port is 7777.";

                Writer.DrawText(Main.UltimateFont, "info : " + serverInfo, new Vector2(20, 900), Color.Black, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f, 3f, spriteBatch, true);

            }

            else if(serverState == State.WaitPlayer)
            {

                Writer.DrawText(Main.UltimateFont, "waiting for players", new Vector2((1920 / 2) - (Main.UltimateFont.MeasureString("waiting for players").X * 8f + 9 * 8f) / 2, 25 - 15), new Color(60, 60, 60), Color.LightGray, 0f, Vector2.Zero, 8f, SpriteEffects.None, 0f, 6f, spriteBatch, Color.Black, false);

                for (int i = 0; i < waitingPlayersButtons.Count; i++)
                {
                    waitingPlayersButtons[i].Draw(spriteBatch);
                }

                if (Server.clients[0] != null)
                    Writer.DrawText(Main.UltimateFont, "player 1 is connected", new Vector2(20, 600), Color.Black, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f, 3f, spriteBatch, true);
                if (Server.clients[1] != null)
                    Writer.DrawText(Main.UltimateFont, "player 2 is connected", new Vector2(20, 700), Color.Black, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f, 3f, spriteBatch, true);
                //if (Server.clients[3].udp.endPoint != null)
                  //  Writer.DrawText(Main.UltimateFont, "player 3 is connected", new Vector2(20, 800), Color.Black, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f, 3f, spriteBatch, true);
                //if (Server.clients[4].udp.endPoint != null)
                  //  Writer.DrawText(Main.UltimateFont, "player 4 is connected: ", new Vector2(20, 900), Color.Black, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f, 3f, spriteBatch, true);

            }


        }

        public enum State
        {
            SelecteMode = 0,  /// pas sûr
            SelectePort = 1,
            WaitPlayer = 2,
        };


        public void InitButton()
        {

            LaunchServer.SetText("start");
            LaunchServer.SetColor(Color.White, Color.Black);
            LaunchServer.SetFont(Main.UltimateFont);
            LaunchServer.SetScale(5);
            LaunchServer.IsMajuscule(false);
            LaunchServer.SetFrontThickness(4);
            LaunchServer.SetAroundButton(Back, Back);
            LaunchServer.SetPosition(0, 600, ButtonV3.Position.centerX);

            Back.SetText("back");
            Back.SetColor(Color.White, Color.Black);
            Back.SetFont(Main.UltimateFont);
            Back.SetScale(5);
            Back.IsMajuscule(false);
            Back.SetFrontThickness(4);
            Back.SetAroundButton(LaunchServer, LaunchServer);
            Back.SetPosition(0, 700, ButtonV3.Position.centerX);

            PortButton.SetTexture(Main.PortBox, new Rectangle(0, 0, 52, 16));
            PortButton.SetColor(Color.White, Color.Black);
            PortButton.SetFont(Main.UltimateFont);
            PortButton.SetScale(5);
            PortButton.SetFrontThickness(4);
            PortButton.SetAroundButton();
            PortButton.SetPosition(0, 300, ButtonV3.Position.centerX);


            Play.SetText("play");
            Play.SetColor(Color.White, Color.Black);
            Play.SetFont(Main.UltimateFont);
            Play.SetScale(5);
            Play.IsMajuscule(false);
            Play.SetFrontThickness(4);
            Play.SetAroundButton(LaunchServer, LaunchServer);
            Play.SetPosition(0, 700, ButtonV3.Position.centerX);

            Cancel.SetText("cancel");
            Cancel.SetColor(Color.White, Color.Black);
            Cancel.SetFont(Main.UltimateFont);
            Cancel.SetScale(5);
            Cancel.IsMajuscule(false);
            Cancel.SetFrontThickness(4);
            Cancel.SetAroundButton(LaunchServer, LaunchServer);
            Cancel.SetPosition(0, 800, ButtonV3.Position.centerX);

            


        }


    }
}
