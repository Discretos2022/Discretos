using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateform_2D_v9.NetWorkEngine_3._0.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    class ConnectServer
    {

        public List<ButtonV3> connectionButtons;

        private ButtonV3 Connect;
        private ButtonV3 Back;

        public static bool IsConnected;

        public TextBox textBoxIP;
        public TextBox textBoxPort;

        public ButtonV3 PortButton;
        public ButtonV3 IPButton;

        private State serverState = State.Connection;

        private int timeOut = 0;
        public bool connection = false;

        private int animTime = 0;

        public ConnectServer()
        {

            connectionButtons = new List<ButtonV3>();

            Connect = new ButtonV3();
            Back = new ButtonV3();

            PortButton = new ButtonV3();
            IPButton = new ButtonV3();

            InitButton();

            connectionButtons.Add(Connect);
            connectionButtons.Add(Back);
            connectionButtons.Add(PortButton);
            connectionButtons.Add(IPButton);

            textBoxIP = new TextBox(15, 4, 4, false, "", true, false);
            textBoxIP.SetPosition(0, 252, ButtonV3.Position.centerX);

            textBoxPort = new TextBox(5, 4, 4, false, "7777", true, true);
            textBoxPort.SetPosition(0, 402, ButtonV3.Position.centerX);
            textBoxPort.Update();

        }


        public void Update(GameState state, GameTime gameTime, Screen screen)
        {

            if (connection)
                timeOut += 1;

            if (Client.IsConnected())
            {
                
                Main.inWorldMap = true;
                Main.inLevel = false;
                Camera.Zoom = 1f;
                Main.gameState = GameState.Multiplaying;

            }

            #region ConnectButton

            Connect.Update(gameTime, screen);

            if (Connect.IsSelected())
                Connect.SetColor(Color.Gray, Color.Black);
            else
                Connect.SetColor(Color.White, Color.Black);

            if (!IsValidIP(textBoxIP.GetText()) || !IsValidPort(textBoxPort.GetText()) || Client.state == Client.ClientState.Connecting)
                Connect.SetColor(Color.DarkGray, Color.Gray);  // 0.6f       0.2f

            if (IsValidIP(textBoxIP.GetText()) && IsValidPort(textBoxPort.GetText()))
                if (Connect.IsCliqued())
                {

                    if (Client.state == Client.ClientState.Disconnected)
                    {

                        if(textBoxPort.GetText() == "")
                            Client.Connect(textBoxIP.GetText(), int.Parse("7777"));
                        else
                            Client.Connect(textBoxIP.GetText(), int.Parse(textBoxPort.GetText()));

                    }
                    else
                        Console.WriteLine("You already connected ! ©");

                }

            if (Client.IsConnected())
            {
                Main.inWorldMap = true;
                Main.inLevel = false;
                Camera.Zoom = 1f;
                Main.gameState = GameState.Multiplaying;
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
                if(textBoxPort.isSelected)
                    PortButton.SetTexture(Main.PortBox, new Rectangle(106, 0, 52, 16));
                else
                    PortButton.SetTexture(Main.PortBox, new Rectangle(0, 0, 52, 16));
            }


            if (PortButton.IsCliqued())
            { textBoxPort.isSelected = true; textBoxIP.isSelected = false; }

            #endregion

            #region IPButton

            IPButton.Update(gameTime, screen);

            if (IPButton.IsSelected())
                IPButton.SetTexture(Main.IPBox, new Rectangle(121, 0, 120, 16));
            else
            {
                if (textBoxIP.isSelected)
                    IPButton.SetTexture(Main.IPBox, new Rectangle(242, 0, 120, 16));
                else
                    IPButton.SetTexture(Main.IPBox, new Rectangle(0, 0, 120, 16));
            }


            if (IPButton.IsCliqued())
            { textBoxIP.isSelected = true; textBoxPort.isSelected = false; }

            #endregion

            if(textBoxIP.isSelected)
                textBoxIP.Update();
            if (textBoxPort.isSelected)
                textBoxPort.Update();

            if (MouseInput.isSimpleClickLeft())
            {
                if (!IPButton.IsSelected())
                    textBoxIP.isSelected = false;
                if (!PortButton.IsSelected())
                    textBoxPort.isSelected = false;
            }

            if(textBoxPort.GetText() != "")
            {
                if (int.Parse(textBoxPort.GetText()) > Main.MaxPort)
                    textBoxPort.SetColor(Color.Red, Color.Black);
                else
                    textBoxPort.SetColor(Color.White, Color.Black);
            }
            
            if (!IsValidIP(textBoxIP.GetText()))
                textBoxIP.SetColor(Color.Red, Color.Black);
            else
                textBoxIP.SetColor(Color.White, Color.Black);

        }


        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            Writer.DrawText(Main.UltimateFont, "connection", new Vector2((1920 / 2) - (Main.UltimateFont.MeasureString("connection").X * 8f + 9 * 8f) / 2, 25 - 15), new Color(60, 60, 60), Color.LightGray, 0f, Vector2.Zero, 8f, SpriteEffects.None, 0f, 6f, spriteBatch, Color.Black, false);


            for (int i = 0; i < connectionButtons.Count; i++)
            {
                connectionButtons[i].Draw(spriteBatch);
            }


            Writer.DrawText(Main.UltimateFont, "ip : " + NetPlay.LocalIPAddress(), new Vector2(1640, 1040), Color.Black, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f, 2f, spriteBatch);

            textBoxIP.Draw(spriteBatch);

            textBoxPort.Draw(spriteBatch);

            if(Client.state == Client.ClientState.Connecting)
            {

                animTime += 1;

                string connecting = "";

                if (animTime == 60)
                    animTime = 0;

                if (animTime >= 0 && animTime < 20)
                    connecting = "connecting.";
                else if (animTime >= 20 && animTime < 40)
                    connecting = "connecting..";
                else if (animTime >= 40 && animTime < 60)
                    connecting = "connecting...";

                Writer.DrawText(Main.UltimateFont, connecting, new Vector2(10, 5), Color.Black, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f, 2f, spriteBatch);

            }
            else
            {
                if (Client.IsTimeOut())
                    Writer.DrawText(Main.UltimateFont, "connection failed", new Vector2(10, 5), Color.Black, Color.Red, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f, 2f, spriteBatch);
            }



        }

        public bool IsValidIP(string IP)
        {
            int num = 0;
            int point = 0;

            if (IP.Length < 7) return false;

            if (IP.ToCharArray()[0] == '.' || IP.ToCharArray()[IP.Length - 1] == '.') return false;

            for (int i = 0; i < IP.Length; i++)
            {
                if (IP.ToCharArray()[i] == '.')
                    point += 1;

                if (char.IsDigit(IP.ToCharArray()[i]))
                    num += 1;

            }

            if (num < 4) return false;
            if (point < 3) return false;

            point = 0;
            num = 0;

            for (int i = 0; i < IP.Length; i++)
            {
                if (char.IsDigit(IP.ToCharArray()[i]))
                { num += 1; point = 0; }

                if (IP.ToCharArray()[i] == '.')
                { point += 1; num = 0; }

                if (point > 1 || num > 3)
                    return false;

            }
            return true;
        }

        public bool IsValidPort(string Port)
        {

            if (Port == "")
                return true;

            if (int.Parse(Port) <= Main.MaxPort)
                return true;

            return false;
        }

        public enum State
        {
            Connection = 0,
            WaitPlayer = 1,
        };


        public void InitButton()
        {

            IPButton.SetTexture(Main.IPBox, new Rectangle(0, 0, 120, 16));
            IPButton.SetColor(Color.White, Color.Black);
            IPButton.SetFont(Main.UltimateFont);
            IPButton.SetScale(5);
            IPButton.SetFrontThickness(4);
            IPButton.SetAroundButton(Connect, Connect);
            IPButton.SetPosition(0, 250, ButtonV3.Position.centerX);

            PortButton.SetTexture(Main.PortBox, new Rectangle(0, 0, 52, 16));
            PortButton.SetColor(Color.White, Color.Black);
            PortButton.SetFont(Main.UltimateFont);
            PortButton.SetScale(5);
            PortButton.SetFrontThickness(4);
            PortButton.SetAroundButton(Connect, Connect);
            PortButton.SetPosition(0, 400, ButtonV3.Position.centerX);



            Connect.SetText("connect");
            Connect.SetColor(Color.White, Color.Black);
            Connect.SetFont(Main.UltimateFont);
            Connect.SetScale(5);
            Connect.IsMajuscule(false);
            Connect.SetFrontThickness(4);
            Connect.SetAroundButton(Back, Back);
            Connect.SetPosition(0, 600, ButtonV3.Position.centerX);



            Back.SetText("back");
            Back.SetColor(Color.White, Color.Black);
            Back.SetFont(Main.UltimateFont);
            Back.SetScale(5);
            Back.IsMajuscule(false);
            Back.SetFrontThickness(4);
            Back.SetAroundButton(Connect, Connect);
            Back.SetPosition(0, 700, ButtonV3.Position.centerX);



        }

    }
}
