using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

/**
 * Button V1.0
**/


namespace Plateform_2D_v9
{
    class ButtonV1
    {

        private Vector2 Pos;
        private Rectangle Dimension;
        private Texture2D Texture;
        private SpriteFont Font;
        private string Text;
        private Color FrontColor;
        private Color BackColor;

        private bool isCliqued;
        private bool isSelected;

        public ButtonV1(Vector2 Pos, Rectangle dimension, Color frontcolor, Color backcolor, SpriteFont font = null, Texture2D texture = null, string text = null)
        {
            this.Pos = Pos;
            this.Dimension = dimension;
            this.Texture = texture;
            this.Font = font;
            this.Text = text;
            this.FrontColor = frontcolor;
            this.BackColor = backcolor;
        }

        public void Draw(SpriteFont font, Vector2 pos, float Scale, float frontThickness, SpriteBatch spriteBatch, bool isEdgeRounded = false, int adjustDimension = 0)
        {
            if(Texture != null)
            {
                spriteBatch.Draw(Texture, Dimension, FrontColor);
            }

            if(Text != null)
            {
                Dimension = new Rectangle((int)pos.X, (int)pos.Y + adjustDimension, (int)(font.MeasureString(Text).X * Scale + 50), (int)(font.MeasureString(Text).Y * Scale + 5 - adjustDimension));
                Writer.DrawText(font, Text, pos, BackColor, FrontColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f, frontThickness, spriteBatch, isEdgeRounded);
            }

        }

        public void Update(Screen screen)
        {
            /// IsCliqued
            if(Dimension.Intersects(MouseInput.GetRectangle(screen)) && MouseInput.getMouseState().LeftButton == ButtonState.Pressed && MouseInput.getOldMouseState().LeftButton != ButtonState.Pressed)
                isCliqued = true;
            else
                isCliqued = false;

            /// IsSelected
            if (Dimension.Intersects(MouseInput.GetRectangle(screen)))
                isSelected = true;
            else
                isSelected = false;


        }

        public string GetText()
        {
            return Text;
        }

        public SpriteFont GetFont()
        {
            return Font;
        }

        public bool IsCliqued()
        {
            return isCliqued;
        }


        public bool IsSelected()
        {
            return isSelected;
        }

        public void SetColor(Color frontcolor, Color backcolor)
        {
            FrontColor = frontcolor;
            BackColor = backcolor;
        }

        public void SetText(string text)
        {
            Text = text;
        }

        public Rectangle getDimension()
        {
            return Dimension;
        }


    }
}
