﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace Plateform_2D_v9
{
    public class Render
    {
        private bool isDisposed;
        private Game game;
        private BasicEffect effect;

        private Matrix Transform;


        private string technique = "RefractAntiRefractionArea";
        private Texture2D displacementTexture;
        private Vector2 displacement;
        private float sampleWavelength = 1f;
        private float frequency = 1;
        private float refractiveIndex = 1f;
        private float refractionSpeed = 0.1f;

        public static Vector2[] LightPosition = new Vector2[100];
        public static float[] LightPositionY = new float[5];
        public static float[] LightR = new float[5];
        public static float[] LightG = new float[5];
        public static float[] LightB = new float[5];
        public static float[] LightIntensity = new float[5];

        public static float EmbiantLightR;
        public static float EmbiantLightG;
        public static float EmbiantLightB;

        public static int NumOfLight;


        public Render(Game game)
        {
            if (game is null)
            {
                throw new ArgumentNullException("game");
            }

            this.game = game;

            this.isDisposed = false;

            this.effect = new BasicEffect(this.game.GraphicsDevice);
            this.effect.FogEnabled = false;
            this.effect.TextureEnabled = true;
            this.effect.LightingEnabled = false;
            this.effect.VertexColorEnabled = true;
            this.effect.World = Matrix.Identity;
            this.effect.Projection = Matrix.Identity;
            this.effect.View = Matrix.Identity;


        }


        public void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            this.isDisposed = true;
        }

        public void Begin(bool isTextureFileteringEnabled, GameTime gameTime, SpriteBatch spriteBatch, Camera camera = null, bool withEffect = true)
        {

            SamplerState sampler = SamplerState.PointClamp;
            if (isTextureFileteringEnabled)
            {
                sampler = SamplerState.LinearClamp;
            }

            Viewport vp = this.game.GraphicsDevice.Viewport;
            this.effect.Projection = Matrix.CreateOrthographicOffCenter(0, vp.Width, vp.Height, 0, 0f, 1f);
            this.effect.View = Matrix.Identity;

            if(camera != null)
            {
                camera.UpdateMatrices();
            }

            
            if(camera != null)
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, sampler, null, RasterizerState.CullCounterClockwise, null, camera._translation);
            else
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, sampler, null, RasterizerState.CullCounterClockwise, null);


        }

        /// <summary>
        /// Discretos 9.7
        /// </summary>
        /// <param name="isTextureFileteringEnabled"></param>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="camera"></param>
        /// <param name="withEffect"></param>
        public void Begin5(bool isTextureFileteringEnabled, GameTime gameTime, SpriteBatch spriteBatch, Camera camera = null, bool withEffect = true)
        {

            SamplerState sampler = SamplerState.PointClamp;
            if (isTextureFileteringEnabled)
            {
                sampler = SamplerState.LinearClamp;
            }

            Viewport vp = this.game.GraphicsDevice.Viewport;
            this.effect.Projection = Matrix.CreateOrthographicOffCenter(0, vp.Width, vp.Height, 0, 0f, 1f);
            this.effect.View = Matrix.Identity;

            if (camera != null)
            {
                camera.UpdateMatrices();
            }


            if (camera != null)
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, sampler, null, RasterizerState.CullCounterClockwise, null, camera._translation);
            else
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, sampler, null, RasterizerState.CullCounterClockwise, null);


        }

        public void BeginAdditive(bool isTextureFileteringEnabled, GameTime gameTime, SpriteBatch spriteBatch, Camera camera = null, bool withEffect = true)
        {

            SamplerState sampler = SamplerState.PointClamp;
            if (isTextureFileteringEnabled)
            {
                sampler = SamplerState.LinearClamp;
            }

            Viewport vp = this.game.GraphicsDevice.Viewport;
            this.effect.Projection = Matrix.CreateOrthographicOffCenter(0, vp.Width, vp.Height, 0, 0f, 1f);
            this.effect.View = Matrix.Identity;

            if (camera != null)
            {
                camera.UpdateMatrices();
            }


            if (camera != null)
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, sampler, null, RasterizerState.CullCounterClockwise, null, camera._translation);
            else
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, sampler, null, RasterizerState.CullCounterClockwise, null);


        }

        public void End(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
        }

        public void DrawRenderTarget(Texture2D texture, Rectangle destinationRectangle, Color color, GameTime gameTime, SpriteBatch spriteBatch, Rectangle sourceRectangle = default, ShaderEffect shader = ShaderEffect.None)
        {

            
            //GetLightProcess(texture, ref test, gameTime, spriteBatch);

            Begin5(!Main.PixelPerfect, gameTime, spriteBatch, null, false);


            switch (shader)
            {
                case ShaderEffect.None:
                    break;

                case ShaderEffect.HeatDistortion:
                    {
                        Vector2 displacement;
                        double time = gameTime.TotalGameTime.TotalSeconds * 0.02f;

                        displacement = new Vector2((float)Math.Cos(time), (float)Math.Sin(time));

                        Main.refractionEffect.Parameters["DisplacementTexture"].SetValue(Main.effect);
                        Main.refractionEffect.Parameters["DisplacementMotionVector"].SetValue(displacement);
                        Main.refractionEffect.Parameters["SampleWavelength"].SetValue(0.6f);                        /// 0.5f
                        Main.refractionEffect.Parameters["Frequency"].SetValue(0.006f);                               /// 0.1f
                        Main.refractionEffect.Parameters["RefractiveIndex"].SetValue(0.4f);                         /// 0.5f
                        // for the very last little test.
                        Main.refractionEffect.Parameters["RefractionVector"].SetValue(new Vector2(0.1f, 0.1f));     ///new Vector2(0.2f, 0.5f)
                        Main.refractionEffect.Parameters["RefractionVectorRange"].SetValue(0.1f);                   ///0.5f

                        Main.refractionEffect.CurrentTechnique = Main.refractionEffect.Techniques["RefractAntiRefractionArea"];
                        Main.refractionEffect.CurrentTechnique.Passes[0].Apply();

                        break;
                    }
                    
                case ShaderEffect.Shadow:
                    {
                        //Main.LightEffect.Parameters["x"].SetValue((float)MouseInput.GetRectangle(screen).X);
                        //Main.LightEffect.Parameters["y"].SetValue((float)MouseInput.GetRectangle(screen).Y);

                        //Main.LightEffect.Parameters["x"].SetValue((float)600);
                        //Main.LightEffect.Parameters["y"].SetValue((float)200);

                        Main.LightEffect.Parameters["p"].SetValue((float)1000);

                        Main.LightEffect.Parameters["o"].SetValue((float)1);

                        Main.LightEffect.Parameters["b"].SetValue((float).01);


                        Main.LightEffect.CurrentTechnique.Passes[2].Apply();

                        break;
                    }

                case ShaderEffect.Shadow2:
                    {
                        Main.LightEffect.Parameters["x"].SetValue((float)MouseInput.getMouseState().X);
                        Main.LightEffect.Parameters["y"].SetValue((float)MouseInput.getMouseState().Y);

                        //Main.LightEffect.Parameters["x"].SetValue((float)600);
                        //Main.LightEffect.Parameters["y"].SetValue((float)200);

                        Main.LightEffect.Parameters["p"].SetValue((float)100);  //100

                        Main.LightEffect.Parameters["o"].SetValue((float)2);    //2

                        Main.LightEffect.Parameters["b"].SetValue((float)0.3);  //0.3

                        Main.LightEffect.Parameters["ResolutionX"].SetValue((float)1920);
                        Main.LightEffect.Parameters["ResolutionY"].SetValue((float)1080);


                        Main.LightEffect.CurrentTechnique.Passes[3].Apply();

                        break;
                    }

                case ShaderEffect.LightRayTracer:
                    {

                        float ratio = 1f;

                        Main.LightEffect.Parameters["x"].SetValue((float)MouseInput.getMouseState().X);
                        Main.LightEffect.Parameters["y"].SetValue((float)MouseInput.getMouseState().Y);

                        //Main.LightEffect.Parameters["x"].SetValue((float)600);
                        //Main.LightEffect.Parameters["y"].SetValue((float)200);

                        Main.LightEffect.Parameters["p"].SetValue((float)100 / ratio);  //100

                        Main.LightEffect.Parameters["o"].SetValue((float)2 / ratio);    //2

                        Main.LightEffect.Parameters["b"].SetValue((float)0.0);  //0.3

                        Main.LightEffect.Parameters["ResolutionX"].SetValue((float)1920 / ratio);
                        Main.LightEffect.Parameters["ResolutionY"].SetValue((float)1080 / ratio);


                        Main.LightEffect.Parameters["NumOfLight"].SetValue((float)NumOfLight);


                        Main.LightEffect.Parameters["LightPosition"].SetValue((Vector2[])LightPosition);



                        Main.LightEffect.Parameters["AmbientLight"].SetValue(new Vector4(0.0f, 0.0f, 0.0f, 1));

                        Main.LightEffect.Parameters["diviseur"].SetValue((float)0.5f / ratio);


                        if (LightPosition[0] != Vector2.Zero && NumOfLight > 0)
                            LightPosition[0] = new Vector2(Mouse.GetState().X / ratio, Mouse.GetState().Y / ratio);


                        //List<Vector3> Light = new List<Vector3>();

                        //Main.LightEffect.Parameters["Light"].SetValue(Light.ToArray());


                        Main.LightEffect.CurrentTechnique.Passes[4].Apply();

                        

                        break;
                    }

                case ShaderEffect.LightMaskLevel:
                    {

                        //Main.LightEffect.Parameters["sature"].SetValue(0.0f);
                        Main.LightEffect.Parameters["lightMask"].SetValue(Screen.LightMaskLevel);

                        Main.LightEffect.CurrentTechnique.Passes[5].Apply();

                        break;
                    }

                case ShaderEffect.LightMaskBackground:
                    {

                        //Main.LightEffect.Parameters["sature"].SetValue(0.0f);
                        Main.LightEffect.Parameters["lightMask"].SetValue(Screen.LightMaskBackground);

                        Main.LightEffect.CurrentTechnique.Passes[5].Apply();

                        break;
                    }

            }


            spriteBatch.Draw(texture, destinationRectangle, null, color, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            //spriteBatch.Draw(test, destinationRectangle, null, color, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            //test = null;

            Main.refractionEffect.CurrentTechnique = null;

            End(spriteBatch);

        }


        public void DrawLine(Texture2D texture, Vector2 pos1, Vector2 pos2, SpriteBatch spriteBatch, Color color, int epaisseur = 1)
        {

            float distance = Vector2.Distance(pos1, pos2);

            float distanceX = pos2.X - pos1.X;
            float distanceY = pos2.Y - pos1.Y;

            float rotation = (float)Math.Atan2(distanceY, distanceX);

            //Console.WriteLine(distanceX + " ; " + distanceY + " ; " + distance + " ; " + rotation);

            spriteBatch.Draw(texture, new Rectangle((int)pos1.X - epaisseur / 2, (int)pos1.Y + epaisseur / 2, (int)distance, epaisseur), null, color, rotation, new Vector2(0, 0.5f), SpriteEffects.None, 0f);

        }

        public void DrawLine(Texture2D texture, Vector2 pos1, float longueur, float rotation, SpriteBatch spriteBatch, Color color, int epaisseur = 1)
        {

            //float distance = Vector2.Distance(pos1, pos2);

            //float distanceX = pos2.X - pos1.X;
            //float distanceY = pos2.Y - pos1.Y;

            //float rotation = (float)Math.Atan2(distanceY, distanceX);

            //Console.WriteLine(distanceX + " ; " + distanceY + " ; " + distance + " ; " + rotation);

            spriteBatch.Draw(texture, new Rectangle((int)pos1.X - epaisseur / 2, (int)pos1.Y + epaisseur / 2, (int)longueur, epaisseur), null, color, rotation, new Vector2(0, 0.5f), SpriteEffects.None, 0f);

        }


        public void CreateEffect(GameTime gameTime)
        {
            displacementTexture = Main.effect;
            sampleWavelength = 0.01f;
            frequency = 0.1f;
            refractiveIndex = 10f;
            refractionSpeed = 0.01f;

            Vector2 refractionVector = new Vector2(50, 50);
            float refractionVectorRange = 1f;



            if (gameTime != null)
            {
                double time = gameTime.TotalGameTime.TotalSeconds * refractionSpeed;
                displacement = new Vector2((float)Math.Cos(time), (float)Math.Sin(time));


                Main.refractionEffect.CurrentTechnique = Main.refractionEffect.Techniques[technique];
                Main.refractionEffect.Parameters["DisplacementTexture"].SetValue(displacementTexture);
                Main.refractionEffect.Parameters["DisplacementMotionVector"].SetValue(displacement);
                Main.refractionEffect.Parameters["SampleWavelength"].SetValue(sampleWavelength);
                Main.refractionEffect.Parameters["Frequency"].SetValue(frequency);
                Main.refractionEffect.Parameters["RefractiveIndex"].SetValue(refractiveIndex);
                Main.refractionEffect.Parameters["RefractionVector"].SetValue(refractionVector);
                Main.refractionEffect.Parameters["RefractionVectorRange"].SetValue(refractionVectorRange);

            }
        }


        public enum ShaderEffect
        {
            None = 0,
            HeatDistortion = 1,
            Shadow = 2,
            Shadow2 = 3,
            LightRayTracer = 4,
            LightMaskLevel = 5,
            LightMaskBackground = 6,
        }

        //public static float[] LightPositionX = new float[200];
        //public static float[] LightPositionY = new float[200];
        //public static float[] LightR = new float[200];
        //public static float[] LightG = new float[200];
        //public static float[] LightB = new float[200];
        //public static float[] LightIntensity = new float[200];


        public static void AddLight()
        {
            int i;
            if (NumOfLight != 0)
                i = NumOfLight + 1;
            else
                i = 0;

            LightPosition[i] = new Vector2(MouseInput.getMouseState().X, MouseInput.getMouseState().Y);

            NumOfLight += 1;


        }


        public void GetLightAndHullProcess(Texture2D texture, Texture2D lightMask, Texture2D hullMash, Texture2D colorMask, ref RenderTarget2D textureOUT, GameTime gameTime, SpriteBatch spriteBatch) // 
        {

            game.GraphicsDevice.SetRenderTarget(textureOUT);
            Begin(!Main.PixelPerfect, gameTime, spriteBatch, null, false);
            game.GraphicsDevice.Clear(Color.Transparent);

            Main.LightEffect.Parameters["lightMask"].SetValue(lightMask);
            Main.LightEffect.Parameters["hullMask"].SetValue(hullMash);
            Main.LightEffect.Parameters["DEBUG"].SetValue(Main.Debug);
            //Main.LightEffect.Parameters["colorMask"].SetValue(colorMask);
            Main.LightEffect.CurrentTechnique.Passes[6].Apply();


            spriteBatch.Draw(texture, new Rectangle(0, 0, texture.Width, texture.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);


            End(spriteBatch);
            game.GraphicsDevice.SetRenderTarget(null);

        }

        public void GetDistorsionProcess(Texture2D texture, ref RenderTarget2D textureOUT, GameTime gameTime, SpriteBatch spriteBatch) // 
        {

            game.GraphicsDevice.SetRenderTarget(textureOUT);
            Begin(!Main.PixelPerfect, gameTime, spriteBatch, null, false);
            game.GraphicsDevice.Clear(Color.Transparent);

            #region Distorsion

            Vector2 displacement;
            double time = gameTime.TotalGameTime.TotalSeconds * 0.02f;

            displacement = new Vector2((float)Math.Cos(time), (float)Math.Sin(time));

            Main.refractionEffect.Parameters["DisplacementTexture"].SetValue(Main.effect);
            Main.refractionEffect.Parameters["DisplacementMotionVector"].SetValue(displacement);
            Main.refractionEffect.Parameters["SampleWavelength"].SetValue(0.6f);                        /// 0.5f
            Main.refractionEffect.Parameters["Frequency"].SetValue(0.006f);                               /// 0.1f
            Main.refractionEffect.Parameters["RefractiveIndex"].SetValue(0.4f);                         /// 0.5f
            // for the very last little test.
            Main.refractionEffect.Parameters["RefractionVector"].SetValue(new Vector2(0.1f, 0.1f));     ///new Vector2(0.2f, 0.5f)
            Main.refractionEffect.Parameters["RefractionVectorRange"].SetValue(0.1f);                   ///0.5f

            Main.refractionEffect.CurrentTechnique = Main.refractionEffect.Techniques["RefractAntiRefractionArea"];
            Main.refractionEffect.CurrentTechnique.Passes[0].Apply();

            #endregion

            spriteBatch.Draw(texture, new Rectangle(0, 0, texture.Width, texture.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);


            End(spriteBatch);
            game.GraphicsDevice.SetRenderTarget(null);

        }












        #region 1.1

        public static void DrawLineV1_1(Texture2D texture, Vector2 pos1, Vector2 pos2, SpriteBatch spriteBatch, Color color, int epaisseur = 1, LineType lineType = LineType.Center)
        {

            float distance = Vector2.Distance(pos1, pos2);

            float distanceX = pos2.X - pos1.X;
            float distanceY = pos2.Y - pos1.Y;

            float rotation = (float)Math.Atan2(distanceY, distanceX);

            float centerOfPointOneOfLine = 0f;
            if (lineType == LineType.Center)
                centerOfPointOneOfLine = 0.5f;
            else if (lineType == LineType.Exterior)
                centerOfPointOneOfLine = 1f;
            else if (lineType == LineType.Interior)
                centerOfPointOneOfLine = 0f;

            spriteBatch.Draw(texture, new Rectangle(Util.UpperInteger(pos1.X), Util.UpperInteger(pos1.Y), Util.UpperInteger(distance), epaisseur), null, color, rotation, new Vector2(0, centerOfPointOneOfLine), SpriteEffects.None, 0f);

        }

        public static void DrawRectangleV1_1(Texture2D texture, Rectangle rectangle, SpriteBatch spriteBatch, Color color, int epaisseur = 1)
        {
            int correctionBog = 0;
            if (!Util.IsMultiple((int)epaisseur, 2))
                correctionBog = 1;

            /// LineType = Center       Fonctionnel
            DrawLineV1_1(texture, new Vector2(rectangle.X - epaisseur / 2, rectangle.Y), new Vector2(rectangle.X + rectangle.Width + epaisseur / 2, rectangle.Y), spriteBatch, color, epaisseur); // - up
            DrawLineV1_1(texture, new Vector2(rectangle.X + correctionBog, rectangle.Y), new Vector2(rectangle.X + correctionBog, rectangle.Y + rectangle.Height), spriteBatch, color, epaisseur); // | ?

            DrawLineV1_1(texture, new Vector2(rectangle.X - epaisseur / 2, rectangle.Y + rectangle.Height), new Vector2(rectangle.X + rectangle.Width + epaisseur / 2, rectangle.Y + rectangle.Height), spriteBatch, color, epaisseur); // - down
            DrawLineV1_1(texture, new Vector2(rectangle.X + rectangle.Width, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), spriteBatch, color, epaisseur); // |  ?
            ///

            /// LineType = Interior     Fonctionnel
            //DrawLineV1_1(texture, new Vector2(rectangle.X - epaisseur / 2, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y), spriteBatch, color, epaisseur, LineType.Interior); // - up
            //DrawLineV1_1(texture, new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.X, rectangle.Y + rectangle.Height), spriteBatch, color, epaisseur, LineType.Interior); // | ?

            //DrawLineV1_1(texture, new Vector2(rectangle.X - epaisseur, rectangle.Y + rectangle.Height), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), spriteBatch, color, epaisseur, LineType.Interior); // - down
            //DrawLineV1_1(texture, new Vector2(rectangle.X + rectangle.Width, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), spriteBatch, color, epaisseur, LineType.Interior); // |  ?
            ///

            /// LineType = Exterior     Not Fonctionnel
            //DrawLineV1_1(texture, new Vector2(rectangle.X - epaisseur / 2, rectangle.Y), new Vector2(rectangle.X + rectangle.Width + epaisseur / 2, rectangle.Y), spriteBatch, color, epaisseur, LineType.Exterior); // - up
            //DrawLineV1_1(texture, new Vector2(rectangle.X + correctionBog, rectangle.Y), new Vector2(rectangle.X + correctionBog, rectangle.Y + rectangle.Height), spriteBatch, color, epaisseur, LineType.Exterior); // | ?

            //DrawLineV1_1(texture, new Vector2(rectangle.X - epaisseur / 2, rectangle.Y + rectangle.Height), new Vector2(rectangle.X + rectangle.Width + epaisseur / 2, rectangle.Y + rectangle.Height), spriteBatch, color, epaisseur, LineType.Exterior); // - down
            //DrawLineV1_1(texture, new Vector2(rectangle.X + rectangle.Width, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), spriteBatch, color, epaisseur, LineType.Exterior); // |  ?
            ///


        }


        public static void DrawRectangleV1_1(Texture2D texture, Rectangle rectangle, SpriteBatch spriteBatch, Color color1, Color color2, Color color3, Color color4, int epaisseur = 1)
        {

            int correctionBog = 0;
            if (!Util.IsMultiple((int)epaisseur, 2))
                correctionBog = 1;

            DrawLineV1_1(texture, new Vector2(rectangle.X + correctionBog, rectangle.Y), new Vector2(rectangle.X + correctionBog, rectangle.Y + rectangle.Height), spriteBatch, color2, epaisseur); // |  left
            DrawLineV1_1(texture, new Vector2(rectangle.X + rectangle.Width, rectangle.Y), new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), spriteBatch, color4, epaisseur); // |  right

            DrawLineV1_1(texture, new Vector2(rectangle.X - epaisseur / 2, rectangle.Y), new Vector2(rectangle.X + rectangle.Width + epaisseur / 2, rectangle.Y), spriteBatch, color1, epaisseur); // - up
            DrawLineV1_1(texture, new Vector2(rectangle.X - epaisseur / 2, rectangle.Y + rectangle.Height), new Vector2(rectangle.X + rectangle.Width + epaisseur / 2, rectangle.Y + rectangle.Height), spriteBatch, color3, epaisseur); // - down

        }


        public enum LineType
        {
            Center = 0,
            Interior = 1,
            Exterior = 2,
        };

        #endregion









    }
}
