using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    sealed class Writer
    {

        public static void DrawText(SpriteFont font, String text, Vector2 position, Color backColor, Color frontColor, float rotation, Vector2 origine, float Scale, SpriteEffects spriteEffects, float LayerDepth, SpriteBatch spriteBatch)
        {

            spriteBatch.DrawString(font, text, position + new Vector2(4, 4), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
            spriteBatch.DrawString(font, text, position + new Vector2(-4, 4), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
            spriteBatch.DrawString(font, text, position + new Vector2(-4, -4), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
            spriteBatch.DrawString(font, text, position + new Vector2(4, -4), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);

            spriteBatch.DrawString(font, text, position + new Vector2(4, 0), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
            spriteBatch.DrawString(font, text, position + new Vector2(0, 4), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
            spriteBatch.DrawString(font, text, position + new Vector2(0, -4), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
            spriteBatch.DrawString(font, text, position + new Vector2(-4, 0), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);

            spriteBatch.DrawString(font, text, position, frontColor, rotation, origine, Scale, spriteEffects, LayerDepth);

        }

        public static void DrawText(SpriteFont font, String text, Vector2 position, Color backColor, Color frontColor, float rotation, Vector2 origine, float Scale, SpriteEffects spriteEffects, float LayerDepth, float frontThickness, SpriteBatch spriteBatch, bool isEdgeRounded = false)
        {
            if (!isEdgeRounded)
            {
                spriteBatch.DrawString(font, text, position + new Vector2(frontThickness, frontThickness), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
                spriteBatch.DrawString(font, text, position + new Vector2(-frontThickness, frontThickness), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
                spriteBatch.DrawString(font, text, position + new Vector2(-frontThickness, -frontThickness), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
                spriteBatch.DrawString(font, text, position + new Vector2(frontThickness, -frontThickness), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
            }


            spriteBatch.DrawString(font, text, position + new Vector2(frontThickness, 0), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
            spriteBatch.DrawString(font, text, position + new Vector2(0, frontThickness), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
            spriteBatch.DrawString(font, text, position + new Vector2(0, -frontThickness), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
            spriteBatch.DrawString(font, text, position + new Vector2(-frontThickness, 0), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);

            spriteBatch.DrawString(font, text, position, frontColor, rotation, origine, Scale, spriteEffects, LayerDepth);

        }

        /// <summary>
        /// version 1.1
        /// With right reflect
        /// </summary>
        /// <param name="font"></param>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="backColor"></param>
        /// <param name="frontColor"></param>
        /// <param name="rotation"></param>
        /// <param name="origine"></param>
        /// <param name="Scale"></param>
        /// <param name="spriteEffects"></param>
        /// <param name="LayerDepth"></param>
        /// <param name="frontThickness"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="isEdgeRounded"></param>
        public static void DrawText(SpriteFont font, String text, Vector2 position, Color backColor, Color frontColor, float rotation, Vector2 origine, float Scale, SpriteEffects spriteEffects, float LayerDepth, float frontThickness, SpriteBatch spriteBatch, Color colorRightReflect, bool isEdgeRounded = false)
        {

            /// Right reflect
            if (!isEdgeRounded)
            {
                spriteBatch.DrawString(font, text, position + new Vector2(frontThickness * 2, frontThickness), colorRightReflect, rotation, origine, Scale, spriteEffects, LayerDepth);
                spriteBatch.DrawString(font, text, position + new Vector2(frontThickness * 2, -frontThickness), colorRightReflect, rotation, origine, Scale, spriteEffects, LayerDepth);
            }
            spriteBatch.DrawString(font, text, position + new Vector2(frontThickness * 2, 0), colorRightReflect, rotation, origine, Scale, spriteEffects, LayerDepth);


            if (!isEdgeRounded)
            {

                /// Right reflect
                spriteBatch.DrawString(font, text, position + new Vector2(frontThickness * 2, frontThickness), colorRightReflect, rotation, origine, Scale, spriteEffects, LayerDepth);
                spriteBatch.DrawString(font, text, position + new Vector2(frontThickness * 2, -frontThickness), colorRightReflect, rotation, origine, Scale, spriteEffects, LayerDepth);

                spriteBatch.DrawString(font, text, position + new Vector2(frontThickness, frontThickness), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
                spriteBatch.DrawString(font, text, position + new Vector2(-frontThickness, frontThickness), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
                spriteBatch.DrawString(font, text, position + new Vector2(-frontThickness, -frontThickness), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
                spriteBatch.DrawString(font, text, position + new Vector2(frontThickness, -frontThickness), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);

                

            }

            

            spriteBatch.DrawString(font, text, position + new Vector2(frontThickness, 0), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
            spriteBatch.DrawString(font, text, position + new Vector2(0, frontThickness), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
            spriteBatch.DrawString(font, text, position + new Vector2(0, -frontThickness), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
            spriteBatch.DrawString(font, text, position + new Vector2(-frontThickness, 0), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);

            spriteBatch.DrawString(font, text, position, frontColor, rotation, origine, Scale, spriteEffects, LayerDepth);

        }


        public static void DrawSuperText(SpriteFont font, String text, Vector2 position, Color backColor, Color frontColor, float rotation, Vector2 origine, float Scale, SpriteEffects spriteEffects, float LayerDepth, float frontThickness, SpriteBatch spriteBatch, bool isEdgeRounded = false)
        {

            //if (!isEdgeRounded)
            //{
            //    spriteBatch.DrawString(font, text, position + new Vector2(frontThickness, frontThickness), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
            //    spriteBatch.DrawString(font, text, position + new Vector2(-frontThickness, frontThickness), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
            //    spriteBatch.DrawString(font, text, position + new Vector2(-frontThickness, -frontThickness), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
            //    spriteBatch.DrawString(font, text, position + new Vector2(frontThickness, -frontThickness), backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
            //}

            char[] caracters = new char[text.Length];

            for (int i = 0; i < caracters.Length; i++)
            {
                caracters[i] = text[i];

                string str = text.Substring(0, i) + "  ";
                float num = Main.UltimateFont.MeasureString(str).X;

                Vector2 pos;
                pos = new Vector2(num * Scale, 0);

                int r = Util.random.Next(-2, 2);

                frontColor = new Color(Util.random.Next(0, 255), Util.random.Next(0, 255), Util.random.Next(0, 255));

                spriteBatch.DrawString(font, caracters[i].ToString(), position + new Vector2(frontThickness, 0 + 2 * r) + pos, backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
                spriteBatch.DrawString(font, caracters[i].ToString(), position + new Vector2(0, frontThickness + 2 * r) + pos, backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
                spriteBatch.DrawString(font, caracters[i].ToString(), position + new Vector2(0, -frontThickness + 2 * r) + pos, backColor, rotation, origine, Scale, spriteEffects, LayerDepth);
                spriteBatch.DrawString(font, caracters[i].ToString(), position + new Vector2(-frontThickness, 0 + 2 * r) + pos, backColor, rotation, origine, Scale, spriteEffects, LayerDepth);

                spriteBatch.DrawString(font, caracters[i].ToString(), position + new Vector2(0, 2 * r) + pos, frontColor, rotation, origine, Scale, spriteEffects, LayerDepth);
            }
            

        }


    }

}