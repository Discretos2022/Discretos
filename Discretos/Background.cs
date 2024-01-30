using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Plateform_2D_v9
{
    class Background
    {

        private static Vector2 Pos;
        private static Vector2 BasePosition;

        private static int Paralaxe = 2;
        private static int BackgroundID_1 = Util.random.Next(2, 3);
        private static int BackgroundID_2 = 0;
        private static int BackgroundID_3 = 0;
        private static int BackgroundID_4 = 0;
        private static int BackgroundID_5 = 0;
        private static int BackgroundID_6 = 0;

        private static Vector2 BackPosition1;
        private static Vector2 BackPosition2;
        private static Vector2 BackPosition3;
        private static Vector2 BackPosition4;
        private static Vector2 BackPosition5;
        private static Vector2 BackPosition6;

        private static Vector2 BackPosition1_2;
        private static Vector2 BackPosition2_2;
        private static Vector2 BackPosition3_2;
        private static Vector2 BackPosition4_2;
        private static Vector2 BackPosition5_2;
        private static Vector2 BackPosition6_2;


        public static void Update()
        {

            if (Main.gameState != GameState.Playing)
                Pos = new Vector2(0, 0);

            BackPosition1 = Pos;
            BackPosition2 = Pos * 2;
            BackPosition3 = Pos * 3;
            BackPosition4 = Pos * 4;
            BackPosition5 = Pos * 5;
            BackPosition6 = Pos * 6;


            #region Back2

            if (BackPosition1.X > 0)
                BackPosition1_2 = BackPosition1 - new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);

            if (BackPosition2.X > 0)
                BackPosition2_2 = BackPosition2 - new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);

            if (BackPosition3.X > 0)
                BackPosition3_2 = BackPosition3 - new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);

            if (BackPosition4.X > 0)
                BackPosition4_2 = BackPosition4 - new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);

            if (BackPosition5.X > 0)
                BackPosition5_2 = BackPosition5 - new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);

            if (BackPosition6.X > 0)
                BackPosition6_2 = BackPosition6 - new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);


            if (BackPosition1.X < 0)
                BackPosition1_2 = BackPosition1 + new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);

            if (BackPosition2.X < 0)
                BackPosition2_2 = BackPosition2 + new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);

            if (BackPosition3.X < 0)
                BackPosition3_2 = BackPosition3 + new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);

            if (BackPosition4.X < 0)
                BackPosition4_2 = BackPosition4 + new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);

            if (BackPosition5.X < 0)
                BackPosition5_2 = BackPosition5 + new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);

            if (BackPosition6.X < 0)
                BackPosition6_2 = BackPosition6 + new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);

            #endregion


            #region Back1

            if (BackPosition1_2.X > 0)
                BackPosition1 = BackPosition1_2 - new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);

            if (BackPosition2_2.X > 0)
                BackPosition2 = BackPosition2_2 - new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);

            if (BackPosition3_2.X > 0)
                BackPosition3 = BackPosition3_2 - new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);

            if (BackPosition4_2.X > 0)
                BackPosition4 = BackPosition4_2 - new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);

            if (BackPosition5_2.X > 0)
                BackPosition5 = BackPosition5_2 - new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);

            if (BackPosition6_2.X > 0)
                BackPosition6 = BackPosition6_2 - new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);


            if (BackPosition1_2.X < 0)
                BackPosition1 = BackPosition1_2 + new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);

            if (BackPosition2_2.X < 0)
                BackPosition2 = BackPosition2_2 + new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);

            if (BackPosition3_2.X < 0)
                BackPosition3 = BackPosition3_2 + new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);

            if (BackPosition4_2.X < 0)
                BackPosition4 = BackPosition4_2 + new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);

            if (BackPosition5_2.X < 0)
                BackPosition5 = BackPosition5_2 + new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);

            if (BackPosition6_2.X < 0)
                BackPosition6 = BackPosition6_2 + new Vector2(480 * 4 * Main.ScreenRatioComparedWith1080p, 0);

            #endregion


            if (Main.gameState != GameState.Playing)
            {
                BackPosition1 = Vector2.Zero;
                BackPosition2 = Vector2.Zero;
                BackPosition3 = Vector2.Zero;
                BackPosition4 = Vector2.Zero;
                BackPosition5 = Vector2.Zero;
                BackPosition6 = Vector2.Zero;
                BackPosition1_2 = Vector2.Zero;
                BackPosition2_2 = Vector2.Zero;
                BackPosition3_2 = Vector2.Zero;
                BackPosition4_2 = Vector2.Zero;
                BackPosition5_2 = Vector2.Zero;
                BackPosition6_2 = Vector2.Zero;
            }

        }


        public static void Draw(SpriteBatch spriteBatch)
        {

            //if (BackgroundID_1 > Main.BackgroundTexture.Length - 1)
            //{
            //    throw new Exception("Ce Background n'éxiste pas ! Choisir entre 1 et " + (Main.BackgroundTexture.GetLength(0) - 1));
            //}



            if(BackgroundID_1 != 0)
                spriteBatch.Draw(Main.BackgroundTexture[BackgroundID_1], new Vector2(-BackPosition1.X + BasePosition.X, -BackPosition1.Y + BasePosition.Y /*(float)Math.Cos(math) * 200*/), null, Color.White, 0f, Vector2.Zero, 4f * Main.ScreenRatioComparedWith1080p, SpriteEffects.None, 0f);
            if (BackgroundID_2 != 0)
                spriteBatch.Draw(Main.BackgroundTexture[BackgroundID_2], new Vector2(-BackPosition2.X + BasePosition.X, -BackPosition2.Y + BasePosition.Y * 2 /*(float)Math.Cos(math) * 200*/), null, Color.White, 0f, Vector2.Zero, 4f * Main.ScreenRatioComparedWith1080p, SpriteEffects.None, 0f);
            if (BackgroundID_3 != 0)
                spriteBatch.Draw(Main.BackgroundTexture[BackgroundID_3], new Vector2(-BackPosition3.X + BasePosition.X, -BackPosition3.Y + BasePosition.Y * 3 /*(float)Math.Cos(math) * 200*/), null, Color.White, 0f, Vector2.Zero, 4f * Main.ScreenRatioComparedWith1080p, SpriteEffects.None, 0f);
            if (BackgroundID_4 != 0)
                spriteBatch.Draw(Main.BackgroundTexture[BackgroundID_4], new Vector2(-BackPosition4.X + BasePosition.X, -BackPosition4.Y + BasePosition.Y * 4 /*(float)Math.Cos(math) * 200*/), null, Color.White, 0f, Vector2.Zero, 4f * Main.ScreenRatioComparedWith1080p, SpriteEffects.None, 0f);
            if (BackgroundID_5 != 0)
                spriteBatch.Draw(Main.BackgroundTexture[BackgroundID_5], new Vector2(-BackPosition5.X + BasePosition.X, -BackPosition5.Y + BasePosition.Y * 5 /*(float)Math.Cos(math) * 200*/), null, Color.White, 0f, Vector2.Zero, 4f * Main.ScreenRatioComparedWith1080p, SpriteEffects.None, 0f);
            if (BackgroundID_6 != 0)
                spriteBatch.Draw(Main.BackgroundTexture[BackgroundID_6], new Vector2(-BackPosition6.X + BasePosition.X, -BackPosition6.Y + BasePosition.Y * 6 /*(float)Math.Cos(math) * 200*/), null, Color.White, 0f, Vector2.Zero, 4f * Main.ScreenRatioComparedWith1080p, SpriteEffects.None, 0f);


            if (BackgroundID_1 != 0)
                spriteBatch.Draw(Main.BackgroundTexture[BackgroundID_1], new Vector2(-BackPosition1_2.X + BasePosition.X, -BackPosition1_2.Y + BasePosition.Y /*(float)Math.Cos(math) * 200*/), null, Color.White, 0f, Vector2.Zero, 4f * Main.ScreenRatioComparedWith1080p, SpriteEffects.None, 0f);
            if (BackgroundID_2 != 0)
                spriteBatch.Draw(Main.BackgroundTexture[BackgroundID_2], new Vector2(-BackPosition2_2.X + BasePosition.X, -BackPosition2_2.Y + BasePosition.Y * 2 /*(float)Math.Cos(math) * 200*/), null, Color.White, 0f, Vector2.Zero, 4f * Main.ScreenRatioComparedWith1080p, SpriteEffects.None, 0f);
            if (BackgroundID_3 != 0)
                spriteBatch.Draw(Main.BackgroundTexture[BackgroundID_3], new Vector2(-BackPosition3_2.X + BasePosition.X, -BackPosition3_2.Y + BasePosition.Y * 3 /*(float)Math.Cos(math) * 200*/), null, Color.White, 0f, Vector2.Zero, 4f * Main.ScreenRatioComparedWith1080p, SpriteEffects.None, 0f);
            if (BackgroundID_4 != 0)
                spriteBatch.Draw(Main.BackgroundTexture[BackgroundID_4], new Vector2(-BackPosition4_2.X + BasePosition.X, -BackPosition4_2.Y + BasePosition.Y * 4 /*(float)Math.Cos(math) * 200*/), null, Color.White, 0f, Vector2.Zero, 4f * Main.ScreenRatioComparedWith1080p, SpriteEffects.None, 0f);
            if (BackgroundID_5 != 0)
                spriteBatch.Draw(Main.BackgroundTexture[BackgroundID_5], new Vector2(-BackPosition5_2.X + BasePosition.X, -BackPosition5_2.Y + BasePosition.Y * 5 /*(float)Math.Cos(math) * 200*/), null, Color.White, 0f, Vector2.Zero, 4f * Main.ScreenRatioComparedWith1080p, SpriteEffects.None, 0f);
            if (BackgroundID_6 != 0)
                spriteBatch.Draw(Main.BackgroundTexture[BackgroundID_6], new Vector2(-BackPosition6_2.X + BasePosition.X, -BackPosition6_2.Y + BasePosition.Y * 6 /*(float)Math.Cos(math) * 200*/), null, Color.White, 0f, Vector2.Zero, 4f * Main.ScreenRatioComparedWith1080p, SpriteEffects.None, 0f);

            Paralaxe = 2;

            

        }

        public static void SetBackgroundPos(float x, float y)
        {
            Pos.X = (x - 220) / Paralaxe;
            Pos.Y = (y - 300) / Paralaxe;

        }


        public static void SetBackground(int backgroundID_1 = 0, int backgroundID_2 = 0, int backgroundID_3 = 0, int backgroundID_4 = 0, int backgroundID_5 = 0, int backgroundID_6 = 0)
        {
            BackgroundID_1 = backgroundID_1;
            BackgroundID_2 = backgroundID_2;
            BackgroundID_3 = backgroundID_3;
            BackgroundID_4 = backgroundID_4;
            BackgroundID_5 = backgroundID_5;
            BackgroundID_6 = backgroundID_6;
        }

        public static void SetBaseBackgroundPos(float x, float y)
        {
            BasePosition = new Vector2(x, y);
        }


    }
}
