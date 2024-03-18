using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    public class Screen
    {

        private readonly static int MinDim = 64;
        private readonly static int MaxDim = 4096;

        private Game game;
        private bool isDisposed;
        public static RenderTarget2D LevelTarget;
        public static RenderTarget2D BackTarget;
        public static RenderTarget2D FontTarget;

        public static RenderTarget2D LightMaskLevel;
        public static RenderTarget2D LightMaskBackground;

        private bool isSet;

        public static Render.ShaderEffect BackgroundShader = Render.ShaderEffect.None;
        public static Render.ShaderEffect LevelShader = Render.ShaderEffect.None;
        public static Render.ShaderEffect UIShader = Render.ShaderEffect.None;

        public int Width
        {
            get { return LevelTarget.Width; }
        }

        public int Height
        {
            get { return LevelTarget.Height; }
        }

        public Screen(Game game, int width, int height)
        {
            width = Util.Clamp(width, Screen.MinDim, Screen.MaxDim);
            height = Util.Clamp(height, Screen.MinDim, Screen.MaxDim);

            this.game = game ?? throw new ArgumentNullException("game");

            LevelTarget = new RenderTarget2D(this.game.GraphicsDevice, width, height);
            BackTarget = new RenderTarget2D(this.game.GraphicsDevice, width, height);
            FontTarget = new RenderTarget2D(this.game.GraphicsDevice, width, height);

            LightMaskLevel = new RenderTarget2D(this.game.GraphicsDevice, width, height);
            LightMaskBackground = new RenderTarget2D(this.game.GraphicsDevice, width, height);

            this.isSet = false;

        }


        public void SetResolution(int width, int height)
        {

            width = Util.Clamp(width, Screen.MinDim, Screen.MaxDim);
            height = Util.Clamp(height, Screen.MinDim, Screen.MaxDim);

            this.game = game ?? throw new ArgumentNullException("game");

            LevelTarget = new RenderTarget2D(this.game.GraphicsDevice, width, height);
            BackTarget = new RenderTarget2D(this.game.GraphicsDevice, width, height);
            FontTarget = new RenderTarget2D(this.game.GraphicsDevice, width, height);

            LightMaskLevel = new RenderTarget2D(this.game.GraphicsDevice, width, height);
            LightMaskBackground = new RenderTarget2D(this.game.GraphicsDevice, width, height);

        }


        public void Dispose()
        {

            if (this.isDisposed)
            {
                return;
            }

            LevelTarget?.Dispose();
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

            Rectangle destinationRectangle = CalculateDestinationRectangle();


            render.DrawRenderTarget(BackTarget, destinationRectangle, Color.White, gameTime, spriteBatch, default, BackgroundShader);
            render.DrawRenderTarget(LevelTarget, destinationRectangle, Color.White, gameTime, spriteBatch, default, LevelShader);

            //render.DrawRenderTarget(LightMask, destinationRectangle, Color.White, gameTime, spriteBatch, default, LightMaskShader);

            render.DrawRenderTarget(FontTarget, destinationRectangle, Color.White, gameTime, spriteBatch, default, UIShader);

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
