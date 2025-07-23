using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Plateform_2D_v9
{
    class BackgroundV2
    {

        private static Vector2 Pos;

        private static Vector2 BasePosition;
        private static Vector2 CameraBasePosition;

        private static int Paralaxe = 2;


        public static int shift = 150;


        private static List<Layer> layers = new List<Layer>();


        public static void Update()
        {

        }


        public static void Draw(SpriteBatch spriteBatch)
        {

            for (int i = 0; i < layers.Count; i++)
            {
                if(Main.gameState == GameState.Playing)
                    layers[i].Draw(spriteBatch, new Vector2((Main.camera.Position.X - (1920 / 8)) * 4, (Main.camera.Position.Y - (1080 / 8)) * 4) + BasePosition, CameraBasePosition);
                else
                    layers[i].Draw(spriteBatch, new Vector2(0, 0), Vector2.Zero);
            }


            Paralaxe = 2; // 2

        }

        public static void SetBackgroundPos(float x, float y)
        {
            //Pos.X = (x - 220) / Paralaxe;
            //Pos.Y = (y - 300) / Paralaxe;

        }

        public static void SetBaseBackgroundPos(float x, float y)
        {
            BasePosition = new Vector2(x, y);
        }

        public static void AddLayer(Layer layer)
        {
            layers.Add(layer);
        }


        public static void Reset()
        {
            BasePosition = new Vector2(0, 0);
            CameraBasePosition = new Vector2(0, 0);
            layers.Clear();
        }

        public static void SetBasePosition(Vector2 position)
        {
            BasePosition = position;
        }

        public static void SetCameraBasePosition(Vector2 position)
        {
            CameraBasePosition = position;
        }


    }




    public class Layer
    {

        private int backgroundID;
        private float velocity;
        private bool infinite;

        public Layer(int backgroundID, float velocity = 1f, bool infinite = true)
        {

            this.backgroundID = backgroundID;
            this.velocity = velocity;
            this.infinite = infinite;

        }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPos, Vector2 shift)
        {

            cameraPos -= shift;

            if (infinite)
            {

                Vector2 p = -new Vector2(cameraPos.X % (1920 / velocity), cameraPos.Y % (1080 / velocity));
                spriteBatch.Draw(Main.BackgroundTexture[backgroundID],  p * velocity, null, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
                spriteBatch.Draw(Main.BackgroundTexture[backgroundID], p * velocity + new Vector2(1920, 0), null, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);

            }

        }



    }


}
