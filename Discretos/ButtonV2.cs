using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

/**
 * Button V2.0
**/


namespace Plateform_2D_v9
{
    class ButtonV2
    {

        private Vector2 Pos;
        private Rectangle Dimension;
        private Texture2D Texture;
        private SpriteFont Font;
        private string Text;
        private Color FrontColor;
        private Color BackColor;

        private bool isCliqued;
        private bool isCliquedWithClavier;
        private bool isSelected;
        private bool isSelectedWithClavier;
        private bool IsMaj;

        private int scale;

        public ButtonV2(Vector2 Pos, Rectangle dimension, Color frontcolor, Color backcolor, SpriteFont font = null, Texture2D texture = null, string text = null, bool IsMaj = false, int scale = 1)
        {
            this.Pos = Pos;
            this.Dimension = dimension;
            this.Texture = texture;
            this.Font = font;
            this.Text = text;
            this.FrontColor = frontcolor;
            this.BackColor = backcolor;
            this.IsMaj = IsMaj;
            this.scale = scale;
        }

        public void Draw(SpriteFont font, float frontThickness, SpriteBatch spriteBatch, bool isEdgeRounded = false, int adjustDimension = 0)
        {

            DEBUG.DebugCollision(Dimension, Color.Red, spriteBatch);

            if(Texture != null)
            {
                spriteBatch.Draw(Texture, Dimension, FrontColor);
            }

            if(Text != null)
            {
                //
                Writer.DrawText(font, Text, Pos, BackColor, FrontColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f, frontThickness, spriteBatch, isEdgeRounded);
            }

        }

        public void Update(Screen screen)
        {
            if(IsMaj)
                Dimension = new Rectangle((int)Pos.X, (int)Pos.Y, (int)(GetFont().MeasureString(Text).X * scale + 50), (int)(GetFont().MeasureString(Text).Y * scale + 5));
            else if(!IsMaj)
                Dimension = new Rectangle((int)Pos.X - scale, (int)Pos.Y + (8 * scale - 4), (int)(GetFont().MeasureString(Text).X * scale + 50 + scale), (int)(GetFont().MeasureString(Text).Y * scale + 5 - (8 * scale - 4)));

            /// IsCliqued
            if (Dimension.Intersects(MouseInput.GetRectangle(screen)) && MouseInput.getMouseState().LeftButton == ButtonState.Pressed && MouseInput.getOldMouseState().LeftButton != ButtonState.Pressed || isCliquedWithClavier)
                isCliqued = true;
            else
                isCliqued = false;

            /// IsSelected
            if (Dimension.Intersects(MouseInput.GetRectangle(screen)) || isSelectedWithClavier)
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

        public void SetCliqued(bool isCliquedWithClavier)
        {
            this.isCliquedWithClavier = isCliquedWithClavier;
        }

        public bool IsSelected()
        {
            return isSelected;
        }

        public void SetSelected(bool isSelectedWithClavier)
        {
            this.isSelectedWithClavier = isSelectedWithClavier;
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

        public void setScale(int newScale)
        {
            scale = newScale;
        }

        public void setPos(Vector2 newpos)
        {
            this.Pos = newpos;
        }

        public void setPos(Position newpos, float x = 0, float y = 0)
        {

            switch (newpos)
            {
                case Position.centerX:
                    this.Pos = new Vector2((Main.ScreenWidth / 2) - (this.GetFont().MeasureString(this.GetText()).X * scale + 46) / 2, y);
                    break;

                case Position.centerY:
                    if(IsMaj)
                        this.Pos = new Vector2(x, (Main.ScreenHeight / 2) - (this.GetFont().MeasureString(this.GetText()).X * scale) / 2);
                    else if (!IsMaj)
                        this.Pos = new Vector2(x, (Main.ScreenHeight / 2) - (this.GetFont().MeasureString(this.GetText()).Y * scale + (8 * scale - 4)) / 2);
                    break;

                case Position.centerXY:
                    if (IsMaj)
                        this.Pos = new Vector2((Main.ScreenWidth / 2) - (this.GetFont().MeasureString(this.GetText()).X * scale + 46) / 2, (Main.ScreenHeight / 2) - (this.GetFont().MeasureString(this.GetText()).X * scale) / 2);
                    else if (!IsMaj)
                        this.Pos = new Vector2((Main.ScreenWidth / 2) - (this.GetFont().MeasureString(this.GetText()).X * scale + 46) / 2, (Main.ScreenHeight / 2) - (this.GetFont().MeasureString(this.GetText()).Y * scale + (8 * scale - 4)) / 2);
                    break;




            }

        }

        public Rectangle getDimension()
        {
            return Dimension;
        }


        public enum Position
        {
            centerX = 1,
            centerY = 2,
            centerXY = 3,
        }


    }
}
