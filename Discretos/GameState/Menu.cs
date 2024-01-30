using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Plateform_2D_v9
{
    class Menu
    {

        private Main main;

        private ButtonV2 SinglePlayer;
        private ButtonV2 MultiPlayer;
        private ButtonV2 Settings;
        private ButtonV2 Quit;


        private ButtonV3 SinglePlayer2;
        private ButtonV3 MultiPlayer2;
        private ButtonV3 Settings2;
        private ButtonV3 Quit2;



        private List<ButtonV2> buttons = new List<ButtonV2>();
        private List<ButtonV3> buttons2 = new List<ButtonV3>();

        private int selectedButton = 0;
        private bool useMouse = true;

        private Color grayColor = new Color(60, 60, 60);


        public Menu(Main main)
        {

            this.main = main;

            SinglePlayer = new ButtonV2(Vector2.Zero, new Rectangle(0, 0, 0, 0), Color.White, Color.Black, Main.UltimateFont, null, "single player");
            MultiPlayer = new ButtonV2(Vector2.Zero, new Rectangle(0, 0, 0, 0), Color.White, Color.Black, Main.UltimateFont, null, "multiplayer");
            Settings = new ButtonV2(Vector2.Zero, new Rectangle(0, 0, 0, 0), Color.White, Color.Black, Main.UltimateFont, null, "settings");
            Quit = new ButtonV2(Vector2.Zero, new Rectangle(0, 0, 0, 0), Color.White, Color.Black, Main.UltimateFont, null, "quit");

            buttons.Add(SinglePlayer);
            buttons.Add(MultiPlayer);
            buttons.Add(Settings);
            buttons.Add(Quit);


            SinglePlayer2 = new ButtonV3();
            MultiPlayer2 = new ButtonV3();
            Settings2 = new ButtonV3();
            Quit2 = new ButtonV3();

            buttons2.Add(SinglePlayer2);
            buttons2.Add(MultiPlayer2);
            buttons2.Add(Settings2);
            buttons2.Add(Quit2);

            ButtonManager.mainMenuButtons = buttons2;

            InitButton();


        }

        public void Update(GameState state, GameTime gameTime, Screen screen)
        {

            SinglePlayer.setPos(ButtonV2.Position.centerX, y: 300);
            SinglePlayer.setScale(5);

            MultiPlayer.setPos(ButtonV2.Position.centerX, y: 400);
            MultiPlayer.setScale(5);

            Settings.setPos(ButtonV2.Position.centerX, y: 500);
            Settings.setScale(5);

            Quit.setPos(ButtonV2.Position.centerX, y: 600);
            Quit.setScale(5);


            if (!useMouse)
                buttons[selectedButton].SetSelected(true);

            for (int i = 0; i < buttons.Count; i++)
                if (i != selectedButton)
                    buttons[i].SetSelected(false);

            for (int i = 0; i < buttons.Count; i++)
                if (i != selectedButton)
                    buttons[i].SetCliqued(false);

            if (MouseInput.GetPos() != MouseInput.GetOldPos())
                useMouse = true;


            if(KeyInput.getKeyState().IsKeyDown(Keys.Up) && !KeyInput.getOldKeyState().IsKeyDown(Keys.Up))
            {
                useMouse = false;

                if(selectedButton != 0)
                    selectedButton -= 1;
            }

            if (KeyInput.getKeyState().IsKeyDown(Keys.Down) && !KeyInput.getOldKeyState().IsKeyDown(Keys.Down))
            {
                useMouse = false;

                if(selectedButton != buttons.Count - 1)
                    selectedButton += 1;
            }

            if (KeyInput.getKeyState().IsKeyDown(Keys.Enter) && !KeyInput.getOldKeyState().IsKeyDown(Keys.Enter))
                buttons[selectedButton].SetCliqued(true);
            else
                buttons[selectedButton].SetCliqued(false);


            #region SinglePlayerButton

            //SinglePlayer.Update(screen);

            //if (SinglePlayer.IsSelected())
            //{
            //    SinglePlayer.SetColor(Color.Gray, Color.Black);
            //    MultiPlayer.SetSelected(false);
            //    Quit.SetSelected(false);
            //    Settings.SetSelected(false);
            //}
            //else
            //{
            //    SinglePlayer.SetColor(Color.White, Color.Black);
            //}

            //if (SinglePlayer.IsCliqued())
            //{

            //    if(Handler.players.Count != 2)
            //    {
            //        Handler.AddPlayerV2(1);
            //    }


            //    Main.inWorldMap = true;
            //    Main.inLevel = false;
            //    Camera.Zoom = 1f;
            //    Main.gameState = GameState.Playing;

            //}

            //#endregion


            //#region MultiPlayerButton

            //MultiPlayer.Update(screen);

            //if (MultiPlayer.IsSelected())
            //{
            //    MultiPlayer.SetColor(Color.Red, Color.Black);
            //    SinglePlayer.SetSelected(false);
            //    Quit.SetSelected(false);
            //    Settings.SetSelected(false);
            //}
            //else
            //{
            //    MultiPlayer.SetColor(Color.White, Color.Black);
            //    MultiPlayer.SetText("multiplayer");
            //}

            //if (MultiPlayer.IsCliqued())
            //{
            //    //MultiPlayer.SetText("not exist");
            //    Background.SetBackground(3);
            //    Main.gameState = GameState.MultiplayerMode;
            //}

            //#endregion


            //#region SettingsButton

            //Settings.Update(screen);

            //if (Settings.IsSelected())
            //{
            //    Settings.SetColor(Color.Blue, Color.Black);
            //    SinglePlayer.SetSelected(false);
            //    MultiPlayer.SetSelected(false);
            //    Quit.SetSelected(false);
            //}
            //else
            //{
            //    Settings.SetColor(Color.White, Color.Black);
            //    Settings.SetText("settings");
            //}

            //if (Settings.IsCliqued())
            //{
            //    Main.gameState = GameState.Settings;
            //    Background.SetBackground(3);
            //}


            //#endregion


            //#region QuitButton

            //Quit.Update(screen);

            //if (Quit.IsSelected())
            //{
            //    Quit.SetColor(Color.Gold, Color.Black);
            //    SinglePlayer.SetSelected(false);
            //    MultiPlayer.SetSelected(false);
            //    Settings.SetSelected(false);
            //}
            //else
            //{
            //    Quit.SetColor(Color.White, Color.Black);
            //    Quit.SetText("quit");
            //}

            //if (Quit.IsCliqued())
            //    main.QuitGame();

            #endregion




            #region SinglePlayerButton2

            SinglePlayer2.Update(gameTime, screen);

            if (SinglePlayer2.IsSelected())
                SinglePlayer2.SetColor(Color.Gray, grayColor);
            else
                SinglePlayer2.SetColor(Color.White, grayColor);

            if (SinglePlayer2.IsCliqued())
            {

                if (Handler.players.Count != 2)
                {
                    Handler.AddPlayerV2(1);
                }


                Main.inWorldMap = true;
                Main.inLevel = false;
                Camera.Zoom = 1f;
                Main.gameState = GameState.Playing;

            }

            #endregion


            #region MultiPlayerButton2

            MultiPlayer2.Update(gameTime, screen);

            if (MultiPlayer2.IsSelected())
                MultiPlayer2.SetColor(Color.Red, grayColor);
            else
                MultiPlayer2.SetColor(Color.White, grayColor);

            if (MultiPlayer2.IsCliqued())
            {
                //MultiPlayer2.SetText("not exist");
                Background.SetBackground(3);
                Main.gameState = GameState.MultiplayerMode;
            }

            #endregion


            #region SettingsButton2

            Settings2.Update(gameTime, screen);

            if (Settings2.IsSelected())
                Settings2.SetColor(Color.Blue, grayColor);
            else
                Settings2.SetColor(Color.White, grayColor);

            if (Settings2.IsCliqued())
            {
                Main.gameState = GameState.Settings;
                Background.SetBackground(3);

                if (!MouseInput.IsActived)
                    ButtonManager.settingsButtons[0].SetIsSelected(true);

            }


            #endregion


            #region QuitButton2

            Quit2.Update(gameTime, screen);

            if (Quit2.IsSelected())
                Quit2.SetColor(Color.Gold, grayColor);
            else
                Quit2.SetColor(Color.White, grayColor);

            if (Quit2.IsCliqued())
                main.QuitGame();

            #endregion


            if (KeyInput.isSimpleClick(Keys.Up, Keys.Down) || GamePadInput.isSimpleClick(PlayerIndex.One, Buttons.DPadUp, Buttons.DPadDown))
            {
                for (int i = 0; i < buttons2.Count; i++)
                {
                    if (buttons2[i].IsSelected())
                        goto L_2;

                }

                SinglePlayer2.SetIsSelected(true);
                MouseInput.IsActived = false;

            L_2:;
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {

            //SinglePlayer.Draw(Main.UltimateFont, 4f, spriteBatch, false, 8 * 5 - 4);
            //MultiPlayer.Draw(Main.UltimateFont, 4f, spriteBatch, false, 8 * 5 - 4);
            //Settings.Draw(Main.UltimateFont, 4f, spriteBatch, false, 8 * 5 - 4);
            //Quit.Draw(Main.UltimateFont, 4f, spriteBatch, false, 8 * 5 - 4);

            spriteBatch.Draw(Main.Banner, new Vector2(1920 / 2 - Main.Banner.Width * 8 / 2, 25), null, Color.White, 0f, new Vector2(0, 0), 8f, SpriteEffects.None, 0f);
            spriteBatch.Draw(Main.Screen1, new Vector2(0, 0), null, Color.White, 0f, new Vector2(0, 0), 4f, SpriteEffects.None, 0f);

            Writer.DrawText(Main.UltimateFont, "version " + Main.Version + " build " + Main.Build + " " + Main.State + " " + Main.Platform, new Vector2(10, 1040), Color.Black, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f, 2f, spriteBatch);
            Writer.DrawText(Main.UltimateFont, "ip : " + Main.IP, new Vector2(1640, 1040), Color.Black, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f, 2f, spriteBatch);


            for (int i = 0; i < buttons2.Count; i++)
            {
                buttons2[i].Draw(spriteBatch, false, true, Color.Black);
            }



        }


        public void InitButton()
        {
            SinglePlayer2.SetText("single player");
            SinglePlayer2.SetColor(Color.White, Color.Black);
            SinglePlayer2.SetFont(Main.UltimateFont);
            SinglePlayer2.SetScale(5);
            SinglePlayer2.IsMajuscule(false);
            SinglePlayer2.SetFrontThickness(4);
            SinglePlayer2.SetAroundButton(null, MultiPlayer2);

            MultiPlayer2.SetText("multiplayer");
            MultiPlayer2.SetColor(Color.White, Color.Black);
            MultiPlayer2.SetFont(Main.UltimateFont);
            MultiPlayer2.SetScale(5);
            MultiPlayer2.IsMajuscule(false);
            MultiPlayer2.SetFrontThickness(4);
            MultiPlayer2.SetAroundButton(SinglePlayer2, Settings2);

            Settings2.SetText("settings");
            Settings2.SetColor(Color.White, Color.Black);
            Settings2.SetFont(Main.UltimateFont);
            Settings2.SetScale(5);
            Settings2.IsMajuscule(false);
            Settings2.SetFrontThickness(4);
            Settings2.SetAroundButton(MultiPlayer2, Quit2);

            Quit2.SetText("quit");
            Quit2.SetColor(Color.White, new Color(60,60,60));
            Quit2.SetFont(Main.UltimateFont);
            Quit2.SetScale(5);
            Quit2.IsMajuscule(false);
            Quit2.SetFrontThickness(4);
            Quit2.SetAroundButton(Settings2, null);


            SinglePlayer2.SetPosition(0, 340, ButtonV3.Position.centerX);
            MultiPlayer2.SetPosition(0, 440, ButtonV3.Position.centerX);
            Settings2.SetPosition(0, 540, ButtonV3.Position.centerX);
            Quit2.SetPosition(0, 640, ButtonV3.Position.centerX);

        }

    }
}