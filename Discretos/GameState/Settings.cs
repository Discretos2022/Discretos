using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    class Settings
    {

        /// Settings Menu
        public ButtonV3 General;
        public ButtonV3 Video;
        public ButtonV3 Audio;
        public ButtonV3 Controls;
        public ButtonV3 Back;

        /// Settings General

        /// Settings Video
        public ButtonV3 VerticalSync;
        public ButtonV3 Fullscreen;
        public ButtonV3 Resolution;
        public ButtonV3 PixelPerfect;
        public ButtonV3 Apply;

        /// Settings Controls
        public ButtonV3 PlayerControls;


        public SettingState settingState;

        public List<ButtonV3> settingsButtons;

        public String settingsInfo = "";


        public Settings(Main main)
        {

            General = new ButtonV3();
            Video = new ButtonV3();
            Audio = new ButtonV3();
            Controls = new ButtonV3();
            Back = new ButtonV3();

            VerticalSync = new ButtonV3();
            Fullscreen = new ButtonV3();
            Resolution = new ButtonV3();
            PixelPerfect = new ButtonV3();
            Apply = new ButtonV3();

            PlayerControls = new ButtonV3();

            settingsButtons = new List<ButtonV3>();

            ButtonManager.settingsButtons.Add(General);
            ButtonManager.settingsButtons.Add(Video);
            ButtonManager.settingsButtons.Add(Audio);
            ButtonManager.settingsButtons.Add(Controls);

            ButtonManager.videoSettingsButtons.Add(VerticalSync);
            ButtonManager.videoSettingsButtons.Add(Fullscreen);
            ButtonManager.videoSettingsButtons.Add(Resolution);
            ButtonManager.videoSettingsButtons.Add(PixelPerfect);
            ButtonManager.videoSettingsButtons.Add(Apply);

            ButtonManager.controlsSettingsButtons.Add(PlayerControls);

            settingState = SettingState.SettingMenu;

            InitButton(main);

        }


        public void Update(GameState state, GameTime gameTime, Screen screen, Main main)
        {
            settingsInfo = "";
            if (settingState == SettingState.SettingMenu)
            {
                UpdateSettingsMenu(gameTime, screen);
            }

            else if (settingState == SettingState.Video)
            {
                UpdateSettingsVideo(gameTime, screen, main);
            }

            else if (settingState == SettingState.Controls)
            {
                UpdateSettingsControls(gameTime, screen);
            }



            Back.Update(gameTime, screen);


            if (Back.IsCliqued() || GamePadInput.isSimpleClick(PlayerIndex.One, Buttons.A))
            {
                if(settingState == SettingState.SettingMenu)
                {
                    Main.gameState = GameState.Menu;
                    BackgroundV2.Reset();
                    BackgroundV2.AddLayer(new Layer(2));
                    ButtonManager.mainMenuButtons[2].SetIsSelected(true);
                }
                else if(settingState == SettingState.Video)
                {
                    settingState = SettingState.SettingMenu;
                    BackgroundV2.Reset();
                    BackgroundV2.AddLayer(new Layer(3));
                    Back.SetIsSelected(false);
                    General.SetIsSelected(true);
                }
                else if (settingState == SettingState.Controls)
                {
                    settingState = SettingState.SettingMenu;
                    BackgroundV2.Reset();
                    BackgroundV2.AddLayer(new Layer(3));
                    Back.SetIsSelected(false);
                    General.SetIsSelected(true);
                }

                settingsInfo = "";
            }

            if (Back.IsSelected())
                Back.SetColor(Color.Gray, Color.Black);
            else
                Back.SetColor(Color.White, Color.Black);

            if (settingState == SettingState.SettingMenu)
                Back.SetAroundButton(Controls, General, null, null);
            else if (settingState == SettingState.Video)
                Back.SetAroundButton(Apply, VerticalSync, null, null);
            else if (settingState == SettingState.Controls)
                Back.SetAroundButton(PlayerControls, PlayerControls, null, null);

            if (settingState == SettingState.Video)
                Back.SetPosition(0, Apply.GetPosition().Y + Apply.GetHeight() * 2 + 40 * 2, ButtonV3.Position.centerX);

        }


        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            
            if (settingState == SettingState.SettingMenu)
                Writer.DrawText(Main.UltimateFont, "SETTINGS", new Vector2((1920 / 2) - (Main.UltimateFont.MeasureString("SETTINGS").X * 6f + 50) / 2, 25), Color.Black, Color.White, 0f, Vector2.Zero, 6f, SpriteEffects.None, 0f, 6f, spriteBatch, false);
            else if (settingState == SettingState.General)
                Writer.DrawText(Main.UltimateFont, "GENERAL", new Vector2((1920 / 2) - (Main.UltimateFont.MeasureString("GENERAL").X * 6f + 50) / 2, 25), Color.Black, Color.White, 0f, Vector2.Zero, 6f, SpriteEffects.None, 0f, 6f, spriteBatch, false);
            else if(settingState == SettingState.Video)
            {
                Writer.DrawText(Main.UltimateFont, "VIDEO", new Vector2((1920 / 2) - (Main.UltimateFont.MeasureString("VIDEO").X * 6f + 50) / 2, 25), Color.Black, Color.White, 0f, Vector2.Zero, 6f, SpriteEffects.None, 0f, 6f, spriteBatch, false);

                VerticalSync.Draw(spriteBatch);
                Fullscreen.Draw(spriteBatch);
                Resolution.Draw(spriteBatch);
                PixelPerfect.Draw(spriteBatch);
                Apply.Draw(spriteBatch);

            }
            else if (settingState == SettingState.Controls)
            {
                Writer.DrawText(Main.UltimateFont, "CONTROLS", new Vector2((1920 / 2) - (Main.UltimateFont.MeasureString("CONTROLS").X * 6f + 50) / 2, 25), Color.Black, Color.White, 0f, Vector2.Zero, 6f, SpriteEffects.None, 0f, 6f, spriteBatch, false);

                PlayerControls.Draw(spriteBatch);

            }


            for (int i = 0; i < ButtonManager.settingsButtons.Count; i++)
            {

                ButtonV3 button = ButtonManager.settingsButtons[i];

                if (settingState == SettingState.SettingMenu)
                    button.Draw(spriteBatch);

            }

            Back.Draw(spriteBatch);

            Writer.DrawText(Main.UltimateFont, "info : " + settingsInfo, new Vector2(20, 900), Color.Black, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f, 3f, spriteBatch, true);

        }


        public void UpdateSettingsMenu(GameTime gameTime, Screen screen)
        {
            //General.SetPosition(0, 300, ButtonV3.Position.centerX);
            //Video.SetPosition(0, General.GetPosition().Y + General.GetHeight() + 40, ButtonV3.Position.centerX);
            //Audio.SetPosition(0, Video.GetPosition().Y + Video.GetHeight() + 40, ButtonV3.Position.centerX);
            //Controls.SetPosition(0, Audio.GetPosition().Y + Audio.GetHeight() + 40, ButtonV3.Position.centerX);
            Back.SetPosition(0, Controls.GetPosition().Y + Controls.GetHeight() + 40, ButtonV3.Position.centerX);

            //VerticalSync.SetPosition(0, VerticalSync.GetPosition().Y + VerticalSync.GetHeight() + 40, ButtonV3.Position.centerX);
            //Fullscreen.SetPosition(0, VerticalSync.GetPosition().Y + VerticalSync.GetHeight() + 40, ButtonV3.Position.centerX);
            //Resolution.SetPosition(0, Fullscreen.GetPosition().Y + VerticalSync.GetHeight() + 40, ButtonV3.Position.centerX);
            //PixelPerfect.SetPosition(0, Resolution.GetPosition().Y + Resolution.GetHeight() + 40, ButtonV3.Position.centerX);
            //Apply.SetPosition(0, PixelPerfect.GetPosition().Y + PixelPerfect.GetHeight() + 40, ButtonV3.Position.centerX);

            //PlayerControls.SetPosition(0, 300, ButtonV3.Position.centerX);


            for (int i = 0; i < ButtonManager.settingsButtons.Count; i++)
            {

                ButtonV3 button = ButtonManager.settingsButtons[i];

                button.Update(gameTime, screen);

                if (button.IsSelected())
                    button.SetColor(Color.Gray, Color.Black);
                else
                    button.SetColor(Color.White, Color.Black);
            }

            if (Video.IsCliqued())
            {
                settingState = SettingState.Video;
                BackgroundV2.Reset();
                BackgroundV2.AddLayer(new Layer(4));

                if (!MouseInput.IsActived)
                    VerticalSync.SetIsSelected(true);

            }

            if (Controls.IsCliqued())
            {
                settingState = SettingState.Controls;
                BackgroundV2.Reset();
                BackgroundV2.AddLayer(new Layer(6));
                BackgroundV2.AddLayer(new Layer(7));
                BackgroundV2.AddLayer(new Layer(8));
                BackgroundV2.AddLayer(new Layer(9));
                BackgroundV2.AddLayer(new Layer(10));
                BackgroundV2.AddLayer(new Layer(11));

                if (!MouseInput.IsActived)
                    PlayerControls.SetIsSelected(true);

            }


            if (KeyInput.isSimpleClick(Keys.Up, Keys.Down) || GamePadInput.isSimpleClick(PlayerIndex.One, Buttons.DPadUp, Buttons.DPadDown))
            {
                for (int i = 0; i < ButtonManager.settingsButtons.Count; i++)
                {
                    if (ButtonManager.settingsButtons[i].IsSelected())
                        goto L_2;

                }

                if (Back.IsSelected())
                    goto L_2;

                ButtonManager.settingsButtons[0].SetIsSelected(true);

                MouseInput.IsActived = false;

            L_2:;
            }


        }

        public void UpdateSettingsVideo(GameTime gameTime, Screen screen, Main main)
        {

            #region Vsync Button
            if (VerticalSync.IsSelected())
            {
                VerticalSync.SetColor(Color.Gray, Color.Black);
                settingsInfo = "synchronize the frame rate of the game with the monitor";
            }   
            else
                VerticalSync.SetColor(Color.White, Color.Black);

            if (VerticalSync.IsCliqued())
            {
                if (main.graphics.SynchronizeWithVerticalRetrace)
                {
                    main.graphics.SynchronizeWithVerticalRetrace = false;
                    VerticalSync.SetText("vsync : enabled");
                }
                else if (!main.graphics.SynchronizeWithVerticalRetrace)
                {
                    main.graphics.SynchronizeWithVerticalRetrace = true;
                    VerticalSync.SetText("vsync : disabled");
                }
                //VerticalSync.SetPosition(0, 300, ButtonV3.Position.centerX);

            }

            //Console.WriteLine(VerticalSync.GetPosition());

            //VerticalSync.SetPosition(0, 300, ButtonV3.Position.centerX);
            VerticalSync.Update(gameTime, screen);
            #endregion

            #region Fullscreen Button
            if (Fullscreen.IsSelected())
            {
                Fullscreen.SetColor(Color.Gray, Color.Black);
                if (!main.graphics.IsFullScreen)
                    settingsInfo = "toggle fullscreen. press apply for apply changes.";
                else
                    settingsInfo = "toggle in windowed mode. press apply for apply changes.";
            }
            else
                Fullscreen.SetColor(Color.White, Color.Black);

            if (Fullscreen.IsCliqued())
            {
                if (main.graphics.IsFullScreen)
                {
                    main.graphics.IsFullScreen = false;
                    //main.graphics.ApplyChanges();
                }
                else if (!main.graphics.IsFullScreen)
                {
                    main.graphics.IsFullScreen = true;
                    //main.graphics.ApplyChanges();
                }

            }

            if (main.graphics.IsFullScreen)
                Fullscreen.SetText("fullscreen : yes");
            else
                Fullscreen.SetText("fullscreen : no");

            //Fullscreen.SetPosition(0, VerticalSync.GetPosition().Y + VerticalSync.GetHeight() + 40, ButtonV3.Position.centerX);

            Fullscreen.Update(gameTime, screen);
            #endregion

            #region Resolution Button
            if (Resolution.IsSelected())
            {
                Resolution.SetColor(Color.Gray, Color.Black);
                if(Main.ResolutionX < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                    settingsInfo = "game screen resolution. this screen resolution is not recommended.";
                else
                    settingsInfo = "game screen resolution. this screen resolution is recommended.";
            }
            else
                Resolution.SetColor(Color.White, Color.Black);
                
            if (!Main.PixelPerfect)
            {
                Resolution.SetColor(Color.DarkGray, Color.Gray); // 0.6f      0.2f
                if (Resolution.IsSelected())
                    settingsInfo = "this parametre is not available. pixelperfect is not actived";
            }
                

            if (Resolution.IsCliqued() && Main.PixelPerfect)
            {
                if (Main.ResolutionX == 1920 && Main.ResolutionY == 1080)
                {
                    Main.ResolutionX = 1600;
                    Main.ResolutionY = 900;
                }
                else if(Main.ResolutionX == 1600 && Main.ResolutionY == 900)
                {
                    Main.ResolutionX = 1280;
                    Main.ResolutionY = 720;
                }
                else if (Main.ResolutionX == 1280 && Main.ResolutionY == 720)
                {
                    Main.ResolutionX = 1920;
                    Main.ResolutionY = 1080;
                }

                Resolution.SetText("resolution : " + Main.ResolutionX + "x" + Main.ResolutionY);

            }

            //Resolution.SetPosition(0, Fullscreen.GetPosition().Y + VerticalSync.GetHeight() + 40, ButtonV3.Position.centerX);

            Resolution.Update(gameTime, screen);
            #endregion

            #region PixelPerfect Button
            if (PixelPerfect.IsSelected())
            {
                PixelPerfect.SetColor(Color.Gray, Color.Black);
                settingsInfo = "no zoom in low resolution.if this is enabled. screen may be blurry.";
            }
            else
                PixelPerfect.SetColor(Color.White, Color.Black);

            if (PixelPerfect.IsCliqued())
            {
                if (Main.PixelPerfect)
                {
                    Main.PixelPerfect = false;
                }
                else if (!Main.PixelPerfect)
                {
                    Main.PixelPerfect = true;
                }

            }

            if (Main.PixelPerfect)
                PixelPerfect.SetText("pixelperfect : enabled");
            else
                PixelPerfect.SetText("pixelperfect : disabled");

            //PixelPerfect.SetPosition(0, Resolution.GetPosition().Y + Resolution.GetHeight() + 40, ButtonV3.Position.centerX);

            PixelPerfect.Update(gameTime, screen);
            #endregion

            #region Apply Button
            if (Apply.IsSelected())
            {
                Apply.SetColor(Color.Gray, Color.Black);
                settingsInfo = "apply changes.";
            }   
            else
                Apply.SetColor(Color.White, Color.Black);

            if (Apply.IsCliqued())
            {
                main.graphics.ApplyChanges();

                if (main.graphics.IsFullScreen)
                {
                    main.graphics.PreferredBackBufferWidth = 1920;
                    main.graphics.PreferredBackBufferHeight = 1080;
                    main.graphics.ApplyChanges();
                }
                else
                {
                    main.graphics.PreferredBackBufferWidth = 1920/2;
                    main.graphics.PreferredBackBufferHeight = 1080/2;
                    main.graphics.ApplyChanges();
                }
                
            }
                

            //Apply.SetPosition(0, PixelPerfect.GetPosition().Y + PixelPerfect.GetHeight() + 40, ButtonV3.Position.centerX);
            Apply.Update(gameTime, screen);
            #endregion

            if (KeyInput.isSimpleClick(Keys.Up, Keys.Down) || GamePadInput.isSimpleClick(PlayerIndex.One, Buttons.DPadUp, Buttons.DPadDown))
            {
                for (int i = 0; i < ButtonManager.videoSettingsButtons.Count; i++)
                {
                    if (ButtonManager.videoSettingsButtons[i].IsSelected())
                        goto L_2;

                }

                if (Back.IsSelected())
                    goto L_2;

                ButtonManager.videoSettingsButtons[0].SetIsSelected(true);

                MouseInput.IsActived = false;

            L_2:;
            }

        }

        public void UpdateSettingsControls(GameTime gameTime, Screen screen)
        {

            #region PlayerControls Button
            if (PlayerControls.IsSelected())
            {
                PlayerControls.SetColor(Color.Gray, Color.Black);
                settingsInfo = "this " + Main.playerControlsName + " paramter is used for " + Main.keyboardName + " keyboard.";
            }
            else
                PlayerControls.SetColor(Color.White, Color.Black);

            if (PlayerControls.IsCliqued())
            {

                if(Main.playerControlsName == "wasd")
                    Main.SetControls("zqsd");
                else if (Main.playerControlsName == "zqsd")
                    Main.SetControls("arrow");
                else if (Main.playerControlsName == "arrow")
                    Main.SetControls("wasd");

                PlayerControls.SetText("player controls : " + Main.playerControlsName);

                //PlayerControls.SetPosition(0, 300, ButtonV3.Position.centerX);

            }

            //PlayerControls.SetPosition(0, 300, ButtonV3.Position.centerX);
            PlayerControls.Update(gameTime, screen);
            #endregion


            if (KeyInput.isSimpleClick(Keys.Up, Keys.Down) || GamePadInput.isSimpleClick(PlayerIndex.One, Buttons.DPadUp, Buttons.DPadDown))
            {
                for (int i = 0; i < ButtonManager.controlsSettingsButtons.Count; i++)
                {
                    if (ButtonManager.controlsSettingsButtons[i].IsSelected())
                        goto L_2;

                }

                if (Back.IsSelected())
                    goto L_2;

                ButtonManager.controlsSettingsButtons[0].SetIsSelected(true);

                MouseInput.IsActived = false;

            L_2:;
            }

        }

        public void InitButton(Main main)
        {

            General.SetText("general");
            General.SetColor(Color.White, Color.Black);
            General.SetFont(Main.UltimateFont);
            General.SetScale(5);
            General.IsMajuscule(false);
            General.SetFrontThickness(4);
            General.SetAroundButton(Back, Video);

            Video.SetText("video");
            Video.SetColor(Color.White, Color.Black);
            Video.SetFont(Main.UltimateFont);
            Video.SetScale(5);
            Video.IsMajuscule(false);
            Video.SetFrontThickness(4);
            Video.SetAroundButton(General, Audio);

            Audio.SetText("audio");
            Audio.SetColor(Color.White, Color.Black);
            Audio.SetFont(Main.UltimateFont);
            Audio.SetScale(5);
            Audio.IsMajuscule(false);
            Audio.SetFrontThickness(4);
            Audio.SetAroundButton(Video, Controls);

            Controls.SetText("controls");
            Controls.SetColor(Color.White, Color.Black);
            Controls.SetFont(Main.UltimateFont);
            Controls.SetScale(5);
            Controls.IsMajuscule(false);
            Controls.SetFrontThickness(4);
            Controls.SetAroundButton(Audio, Back);

            Back.SetText("back");
            Back.SetColor(Color.White, Color.Black);
            Back.SetFont(Main.UltimateFont);
            Back.SetScale(5);
            Back.IsMajuscule(false);
            Back.SetFrontThickness(4);

            if(main.graphics.SynchronizeWithVerticalRetrace)
                VerticalSync.SetText("vsync : enabled");
            else if(!main.graphics.SynchronizeWithVerticalRetrace)
                VerticalSync.SetText("vsync : disabled");
            VerticalSync.SetColor(Color.White, Color.Black);
            VerticalSync.SetFont(Main.UltimateFont);
            VerticalSync.SetScale(5);
            VerticalSync.IsMajuscule(false);
            VerticalSync.SetFrontThickness(4);
            VerticalSync.SetAroundButton(Back, Fullscreen);
            VerticalSync.SetIsMultiClic(true);

            if (main.graphics.IsFullScreen)
                Fullscreen.SetText("fullscreen : yes");
            else if (!main.graphics.IsFullScreen)
                Fullscreen.SetText("fullscreen : no");
            Fullscreen.SetColor(Color.White, Color.Black);
            Fullscreen.SetFont(Main.UltimateFont);
            Fullscreen.SetScale(5);
            Fullscreen.IsMajuscule(false);
            Fullscreen.SetFrontThickness(4);
            Fullscreen.SetAroundButton(VerticalSync, Resolution);
            Fullscreen.SetIsMultiClic(true);

            Resolution.SetText("resolution : " + Main.ResolutionX + "x" + Main.ResolutionY);
            Resolution.SetColor(Color.White, Color.Black);
            Resolution.SetFont(Main.UltimateFont);
            Resolution.SetScale(5);
            Resolution.IsMajuscule(false);
            Resolution.SetFrontThickness(4);
            Resolution.SetAroundButton(Fullscreen, PixelPerfect);
            Resolution.SetIsMultiClic(true);

            if (Main.PixelPerfect)
                PixelPerfect.SetText("pixelperfect : enabled");
            else if (!main.graphics.IsFullScreen)
                PixelPerfect.SetText("pixelperfect : disabled");
            PixelPerfect.SetColor(Color.White, Color.Black);
            PixelPerfect.SetFont(Main.UltimateFont);
            PixelPerfect.SetScale(5);
            PixelPerfect.IsMajuscule(false);
            PixelPerfect.SetFrontThickness(4);
            PixelPerfect.SetAroundButton(Resolution, Apply);
            PixelPerfect.SetIsMultiClic(true);

            Apply.SetText("apply");
            Apply.SetColor(Color.White, Color.Black);
            Apply.SetFont(Main.UltimateFont);
            Apply.SetScale(5);
            Apply.IsMajuscule(false);
            Apply.SetFrontThickness(4);
            Apply.SetAroundButton(PixelPerfect, Back);
            Apply.SetIsMultiClic(true);

            PlayerControls.SetText("player controls : wasd");
            PlayerControls.SetColor(Color.White, Color.Black);
            PlayerControls.SetFont(Main.UltimateFont);
            PlayerControls.SetScale(5);
            PlayerControls.IsMajuscule(false);
            PlayerControls.SetFrontThickness(4);
            PlayerControls.SetAroundButton(Back, Back);
            PlayerControls.SetIsMultiClic(true);

            General.SetPosition(0, 300, ButtonV3.Position.centerX);
            Video.SetPosition(0, General.GetPosition().Y + General.GetHeight() + 40, ButtonV3.Position.centerX);
            Audio.SetPosition(0, Video.GetPosition().Y + Video.GetHeight() + 40, ButtonV3.Position.centerX);
            Controls.SetPosition(0, Audio.GetPosition().Y + Audio.GetHeight() + 40, ButtonV3.Position.centerX);
            Back.SetPosition(0, Controls.GetPosition().Y + Controls.GetHeight() + 40, ButtonV3.Position.centerX);
            VerticalSync.SetPosition(0, 300, ButtonV3.Position.centerX);
            Fullscreen.SetPosition(0, VerticalSync.GetPosition().Y + VerticalSync.GetHeight() + 40, ButtonV3.Position.centerX);
            Resolution.SetPosition(0, Fullscreen.GetPosition().Y + Fullscreen.GetHeight() + 40, ButtonV3.Position.centerX);
            PixelPerfect.SetPosition(0, Resolution.GetPosition().Y + Resolution.GetHeight() + 40, ButtonV3.Position.centerX);
            Apply.SetPosition(0, PixelPerfect.GetPosition().Y + PixelPerfect.GetHeight() + 40, ButtonV3.Position.centerX);
            PlayerControls.SetPosition(0, 300, ButtonV3.Position.centerX);

        }


        public enum SettingState
        {
            SettingMenu = 0,
            General = 1,
            Video = 2,
            Audio = 3,
            Controls = 4,
        };

    }

}
