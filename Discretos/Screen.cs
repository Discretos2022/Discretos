using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Plateform_2D_v9
{
    public class Screen
    {

        private readonly static int MinDim = 64;
        private readonly static int MaxDim = 4096;

        private Game game;
        private bool isDisposed;
        public static RenderTarget2D LevelTarget_1;
        public static RenderTarget2D BackTarget_1;
        public static RenderTarget2D FontTarget_1;

        public static RenderTarget2D LevelTarget_2;
        public static RenderTarget2D BackTarget_2;
        public static RenderTarget2D FontTarget_2;

        public static RenderTarget2D WaterTarget;

        public static RenderTarget2D LightMaskLevel;
        public static RenderTarget2D HullMaskLevel;
        public static RenderTarget2D ColorMaskLevel;

        public static RenderTarget2D LightMaskBackground;

        private bool isSet;

        //public static Render.ShaderEffect BackgroundShader = Render.ShaderEffect.None;
        //public static Render.ShaderEffect LevelShader = Render.ShaderEffect.None;
        //public static Render.ShaderEffect UIShader = Render.ShaderEffect.None;


        RenderTarget2D LevelTargetLight = null;
        RenderTarget2D LevelTargetDistorsion = null;

        RenderTarget2D BackTargetLight = null;
        RenderTarget2D BackTargetDistorsion = null;

        RenderTarget2D WaterDistortion = null;

        public static Texture2D LevelTexture;
        public static Texture2D BackgroundTexture;


        public int Width
        {
            get { return LevelTarget_1.Width; }
        }

        public int Height
        {
            get { return LevelTarget_1.Height; }
        }

        public Screen(Game game, int width, int height)
        {
            width = Util.Clamp(width, Screen.MinDim, Screen.MaxDim);
            height = Util.Clamp(height, Screen.MinDim, Screen.MaxDim);

            this.game = game ?? throw new ArgumentNullException("game");

            LevelTarget_1 = new RenderTarget2D(this.game.GraphicsDevice, width, height);
            BackTarget_1 = new RenderTarget2D(this.game.GraphicsDevice, width, height);
            FontTarget_1 = new RenderTarget2D(this.game.GraphicsDevice, width, height);

            LevelTarget_2 = new RenderTarget2D(this.game.GraphicsDevice, width, height);
            BackTarget_2 = new RenderTarget2D(this.game.GraphicsDevice, width, height);
            FontTarget_2 = new RenderTarget2D(this.game.GraphicsDevice, width, height);

            WaterTarget = new RenderTarget2D(this.game.GraphicsDevice, width, height);

            LightMaskLevel = new RenderTarget2D(this.game.GraphicsDevice, width, height);
            HullMaskLevel = new RenderTarget2D(this.game.GraphicsDevice, width, height);
            ColorMaskLevel = new RenderTarget2D(this.game.GraphicsDevice, width, height);
            LightMaskBackground = new RenderTarget2D(this.game.GraphicsDevice, width, height);

            LevelTargetLight = new RenderTarget2D(game.GraphicsDevice, 1920, 1080);
            LevelTargetDistorsion = new RenderTarget2D(game.GraphicsDevice, 1920, 1080);

            BackTargetLight = new RenderTarget2D(game.GraphicsDevice, 1920, 1080);
            BackTargetDistorsion = new RenderTarget2D(game.GraphicsDevice, 1920, 1080);

            WaterDistortion = new RenderTarget2D(game.GraphicsDevice, 1920, 1080);

            this.isSet = false;

        }


        public void SetResolution(int width, int height)
        {

            width = Util.Clamp(width, Screen.MinDim, Screen.MaxDim);
            height = Util.Clamp(height, Screen.MinDim, Screen.MaxDim);

            this.game = game ?? throw new ArgumentNullException("game");

            LevelTarget_1 = new RenderTarget2D(this.game.GraphicsDevice, width, height);
            BackTarget_1 = new RenderTarget2D(this.game.GraphicsDevice, width, height);
            FontTarget_1 = new RenderTarget2D(this.game.GraphicsDevice, width, height);

            LightMaskLevel = new RenderTarget2D(this.game.GraphicsDevice, width, height);
            LightMaskBackground = new RenderTarget2D(this.game.GraphicsDevice, width, height);

        }


        public void Dispose()
        {

            if (this.isDisposed)
            {
                return;
            }

            LevelTarget_1?.Dispose();
            this.isDisposed = true;

        }


        public void Set(RenderTarget2D target)
        {

            if (this.isSet)
            {
                throw new Exception("Le Render Target est déjà paramètré");
            }

            this.game.GraphicsDevice.SetRenderTarget(target);

            this.isSet = true;

        }

        public void UnSet()
        {

            if (!this.isSet)
            {
                throw new Exception("Le Render Target est déjà supprimé");
            }

            this.game.GraphicsDevice.SetRenderTarget(null);
            this.isSet = false;
        }


        public void Present(Render render, GameTime gameTime, SpriteBatch spriteBatch)
        {

            BackgroundTexture = BackTarget_1;
            LevelTexture = LevelTarget_1;

            int levelRT = 1;
            int backgroundRT = 1;

            /// Image processing
            if (LightManager.isLightEnable)
            {
                //render.GetLightAndHullProcess(BackTarget, LightMaskBackground, LightMaskBackground, LightMaskBackground, ref BackTargetLight, gameTime, spriteBatch); // LightMaskLevel, HullMaskLevel, ColorMaskLevel
                //render.GetLightAndHullProcess(LevelTarget, LightMaskLevel, HullMaskLevel, ColorMaskLevel, ref LevelTargetLight, gameTime, spriteBatch);
            }
            /*else if (Main.LevelPlaying == 4)
            {
                render.GetDistorsionProcess(ref BackgroundTexture, BackTargetDistorsion, gameTime, spriteBatch);
                //render.GetDistorsionProcess(ref LevelTexture, ref LevelTargetDistorsion, gameTime, spriteBatch);

                //LevelTexture = render.GetDistorsionProcess(LevelTexture, gameTime, spriteBatch);

                //LevelTexture.SaveAsPng(new StreamWriter("LevelTest.png").BaseStream, LevelTexture.Width, LevelTexture.Height);
                //Console.WriteLine(".PNG");

            }*/


            if(Main.lightShaderEnable && LightManager.isLightEnable)
            {
                if (levelRT == 1)
                    render.GetLightAndHullProcess(ref LevelTexture, LightMaskLevel, HullMaskLevel, ColorMaskLevel, LevelTarget_2, gameTime, spriteBatch); // LightMaskLevel, HullMaskLevel, ColorMaskLevel
                else if (levelRT == -1)
                    render.GetLightAndHullProcess(ref LevelTexture, LightMaskLevel, HullMaskLevel, ColorMaskLevel, LevelTarget_1, gameTime, spriteBatch); // LightMaskLevel, HullMaskLevel, ColorMaskLevel
                levelRT = -levelRT;

                if (backgroundRT == 1)
                    render.GetLightAndHullProcess(ref BackgroundTexture, LightMaskBackground, LightMaskBackground, LightMaskBackground, BackTarget_2, gameTime, spriteBatch); // LightMaskLevel, HullMaskLevel, ColorMaskLevel
                else if (backgroundRT == -1)
                    render.GetLightAndHullProcess(ref BackgroundTexture, LightMaskBackground, LightMaskBackground, LightMaskBackground, BackTarget_1, gameTime, spriteBatch); // LightMaskLevel, HullMaskLevel, ColorMaskLevel
                backgroundRT = -backgroundRT;
            }


            if (Main.distortionShaderEnable)
            {

                for(int i = 0; i < 1; i++)
                {
                    if (levelRT == 1)
                        render.GetDistorsionProcess(ref LevelTexture, LevelTarget_2, gameTime, spriteBatch);
                    else if (levelRT == -1)
                        render.GetDistorsionProcess(ref LevelTexture, LevelTarget_1, gameTime, spriteBatch);
                    levelRT = -levelRT;

                    if (backgroundRT == 1)
                        render.GetDistorsionProcess(ref BackgroundTexture, BackTarget_2, gameTime, spriteBatch);
                    else if (backgroundRT == -1)
                        render.GetDistorsionProcess(ref BackgroundTexture, BackTarget_1, gameTime, spriteBatch);
                    backgroundRT = -backgroundRT;
                }

                

                /*render.GetDistorsionProcess(ref LevelTexture, LevelTargetDistorsion, gameTime, spriteBatch);
                render.GetDistorsionProcess(ref LevelTexture, LevelTarget_1, gameTime, spriteBatch);
                render.GetDistorsionProcess(ref LevelTexture, LevelTargetDistorsion, gameTime, spriteBatch);
                render.GetDistorsionProcess(ref LevelTexture, LevelTarget_1, gameTime, spriteBatch);
                render.GetDistorsionProcess(ref LevelTexture, LevelTargetDistorsion, gameTime, spriteBatch);
                render.GetDistorsionProcess(ref LevelTexture, LevelTarget_1, gameTime, spriteBatch);*/
            }



            //render.GetDistorsionProcess(WaterTarget, ref WaterDistortion, gameTime, spriteBatch);


            /// Image Drawing
            Rectangle destinationRectangle = CalculateDestinationRectangle();

            render.Begin5(!Main.PixelPerfect, gameTime, spriteBatch, null, false);

            /*if (LightManager.isLightEnable)
            {
                spriteBatch.Draw(BackTargetLight, destinationRectangle, Color.White);
                spriteBatch.Draw(LevelTargetLight, destinationRectangle, Color.White);
            }
            else if (Main.LevelPlaying == 4)
            {
                
            }
            else
            {
                spriteBatch.Draw(BackTarget_1, destinationRectangle, Color.White);
                spriteBatch.Draw(LevelTarget_1, destinationRectangle, Color.White);
            }*/
            //spriteBatch.Draw(WaterDistortion, destinationRectangle, Color.White);


            spriteBatch.Draw(BackgroundTexture, destinationRectangle, Color.White);
            spriteBatch.Draw(LevelTexture, destinationRectangle, Color.White);


            spriteBatch.Draw(FontTarget_1, destinationRectangle, Color.White);

            render.End(spriteBatch);

            /* Multiplayer // Horizontale splitscreen
            render.Draw(BackTarget, new Rectangle(CalculateDestinationRectangle().X, CalculateDestinationRectangle().Y + this.game.GraphicsDevice.PresentationParameters.Bounds.Height/2, CalculateDestinationRectangle().Width, CalculateDestinationRectangle().Height), Color.White, time, false);
            render.Draw(target, new Rectangle(CalculateDestinationRectangle().X, CalculateDestinationRectangle().Y + this.game.GraphicsDevice.PresentationParameters.Bounds.Height / 2, CalculateDestinationRectangle().Width, CalculateDestinationRectangle().Height), Color.White, time, false);
            render.Draw(FontTarget, new Rectangle(CalculateDestinationRectangle().X, CalculateDestinationRectangle().Y + this.game.GraphicsDevice.PresentationParameters.Bounds.Height / 2, CalculateDestinationRectangle().Width, CalculateDestinationRectangle().Height), Color.White, time, false);
            */

            /* Multiplayer // Verticale splitscreen
            render.Draw(BackTarget, new Rectangle(CalculateDestinationRectangle().X + this.game.GraphicsDevice.PresentationParameters.Bounds.Width / 2, CalculateDestinationRectangle().Y, CalculateDestinationRectangle().Width, CalculateDestinationRectangle().Height), Color.White, time, false);
            render.Draw(target, new Rectangle(CalculateDestinationRectangle().X + this.game.GraphicsDevice.PresentationParameters.Bounds.Width / 2, CalculateDestinationRectangle().Y, CalculateDestinationRectangle().Width, CalculateDestinationRectangle().Height), Color.White, time, false);
            render.Draw(FontTarget, new Rectangle(CalculateDestinationRectangle().X + this.game.GraphicsDevice.PresentationParameters.Bounds.Width / 2, CalculateDestinationRectangle().Y, CalculateDestinationRectangle().Width, CalculateDestinationRectangle().Height), Color.White, time, false);
            */

            /* Multiplayer // 4 splitscreen
            render.Draw(BackTarget, new Rectangle(CalculateDestinationRectangle().X + this.game.GraphicsDevice.PresentationParameters.Bounds.Width / 2, CalculateDestinationRectangle().Y + this.game.GraphicsDevice.PresentationParameters.Bounds.Height / 2, CalculateDestinationRectangle().Width, CalculateDestinationRectangle().Height), Color.White, time, false);
            render.Draw(target, new Rectangle(CalculateDestinationRectangle().X + this.game.GraphicsDevice.PresentationParameters.Bounds.Width / 2, CalculateDestinationRectangle().Y + this.game.GraphicsDevice.PresentationParameters.Bounds.Height / 2, CalculateDestinationRectangle().Width, CalculateDestinationRectangle().Height), Color.White, time, false);
            render.Draw(FontTarget, new Rectangle(CalculateDestinationRectangle().X + this.game.GraphicsDevice.PresentationParameters.Bounds.Width / 2, CalculateDestinationRectangle().Y + this.game.GraphicsDevice.PresentationParameters.Bounds.Height / 2, CalculateDestinationRectangle().Width, CalculateDestinationRectangle().Height), Color.White, time, false);
            */

        }


        public Rectangle CalculateDestinationRectangle()
        {
            Rectangle backbufferBounds = this.game.GraphicsDevice.PresentationParameters.Bounds;
            float backbufferAspectRatio = (float)backbufferBounds.Width / backbufferBounds.Height;
            float screenAspectRatio = (float)this.Width / this.Height;

            float rx = 0f;
            float ry = 0f;
            float rw = backbufferBounds.Width;
            float rh = backbufferBounds.Height;


            if (backbufferAspectRatio > screenAspectRatio)
            {

                rw = rh * screenAspectRatio;
                rx = ((float)backbufferBounds.Width - rw) / 2f;

            }
            else if (backbufferAspectRatio < screenAspectRatio)
            {

                rh = rw / screenAspectRatio;
                ry = ((float)backbufferBounds.Height - rh) / 2f;

            }


            Rectangle result = new Rectangle((int)rx, (int)ry, (int)rw/1, (int)rh/1);
            return result;

        }
    }
}
