using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{

    /// <summary>
    /// Version 3.1
    /// </summary>

    class ButtonV3
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
        private bool isReleased;
        private bool IsMaj;

        private float Scale;
        private float FrontThickness;

        /// AutoScale for dimension
        private float adjustement;
        private int dividAdjustement;

        /// <summary>
        /// Version 3.1
        /// For selection with Keyboard or Manette.
        /// </summary>
        public ButtonV3 UpButton;
        public ButtonV3 DownButton;
        public ButtonV3 RightButton;
        public ButtonV3 LeftButton;

        /// <summary>
        /// Version 3.1
        /// </summary>
        private Position SpecialPosition;
        private Vector2 NewPosition;
        private bool isMultiClic;
        private Rectangle SourceImg;

        /// <summary>
        /// Version 3.2
        /// </summary>
        public Color outlineColor;
        public int outlineThickness;
        public Texture2D outlineTex;


        public ButtonV3()
        {

        }


        public void Update(GameTime gameTime, Screen screen)
        {
            if (Text != null)
                Dimension = new Rectangle((int)Pos.X - (int)FrontThickness, (int)Pos.Y - 0 - (int)FrontThickness, (int)(Main.UltimateFont.MeasureString(Text).X * Scale + 9 * (int)Scale) + (int)FrontThickness * 2, (int)((Main.UltimateFont.MeasureString(Text).Y * Scale) / dividAdjustement) + (int)FrontThickness * 2);

            if (MouseInput.IsActived)
            {
                /// IsCliqued
                if (Dimension.Intersects(MouseInput.GetRectangle(screen)) && MouseInput.isSimpleClickLeft())
                { isCliqued = true; MouseInput.UpdateInput(); }
                else
                    isCliqued = false;

                /// IsSelected
                if (Dimension.Intersects(MouseInput.GetRectangle(screen)))
                    isSelected = true;
                else
                    isSelected = false;

                /// Discretos LD
                /// IsReleased
                if (Dimension.Intersects(MouseInput.GetRectangle(screen)) && MouseInput.getMouseState().LeftButton == ButtonState.Released && MouseInput.getOldMouseState().LeftButton != ButtonState.Released)
                    isReleased = true;
                else
                    isReleased = false;

            }

            else if (!MouseInput.IsActived)
            {
                /// IsCliqued
                if (isSelected && ((KeyInput.isSimpleClick(Keys.Enter, Keys.Space)) || GamePadInput.isSimpleClick(PlayerIndex.One, Buttons.B)))
                    isCliqued = true;
                else
                    isCliqued = false;
            }

            /// version 3.1
            if (isSelected)
            {
                if (KeyInput.isSimpleClick(Keys.Down) || GamePadInput.isSimpleClick(PlayerIndex.One, Buttons.DPadDown))
                    if (DownButton != null)
                    { MouseInput.IsActived = false; DownButton.isSelected = true; this.isSelected = false; KeyInput.Update(); GamePadInput.Update(PlayerIndex.One); }

                if (KeyInput.isSimpleClick(Keys.Up) || GamePadInput.isSimpleClick(PlayerIndex.One, Buttons.DPadUp))
                    if (UpButton != null)
                    { MouseInput.IsActived = false; UpButton.isSelected = true; this.isSelected = false; KeyInput.Update(); GamePadInput.Update(PlayerIndex.One); }

                if (KeyInput.isSimpleClick(Keys.Right) || GamePadInput.isSimpleClick(PlayerIndex.One, Buttons.DPadRight))
                    if (RightButton != null)
                    { MouseInput.IsActived = false; RightButton.isSelected = true; this.isSelected = false; KeyInput.Update(); GamePadInput.Update(PlayerIndex.One); }

                if (KeyInput.isSimpleClick(Keys.Left) || GamePadInput.isSimpleClick(PlayerIndex.One, Buttons.DPadLeft))
                    if (LeftButton != null)
                    { MouseInput.IsActived = false; LeftButton.isSelected = true; this.isSelected = false; KeyInput.Update(); GamePadInput.Update(PlayerIndex.One); }

            }

            if (isCliqued)
                if (!isMultiClic)
                    isSelected = false;

        }

        public void Draw(SpriteBatch spriteBatch, bool isEdgeRounded = false, bool withRightReflect = false, Color colorRightReflect = default)
        {
            //Dimension = new Rectangle((int)Pos.X - (int)FrontThickness, (int)Pos.Y - 0 - (int)FrontThickness, (int)(Main.UltimateFont.MeasureString(Text).X * Scale + 9 * (int)Scale) + (int)FrontThickness * 2, (int)((Main.UltimateFont.MeasureString(Text).Y * Scale) / dividAdjustement) + (int)FrontThickness * 2);

            DEBUG.DebugCollision(Dimension, Color.Cyan, spriteBatch);

            if (Texture != null)
            {
                Dimension = new Rectangle((int)Pos.X, (int)Pos.Y, (int)(SourceImg.Width * Scale), (int)(SourceImg.Height * Scale));
                Dimension = new Rectangle((int)Pos.X, (int)Pos.Y, (int)(SourceImg.Width * Scale), (int)(SourceImg.Height * Scale));

                /// Version 3.2
                if(outlineTex != null)
                    spriteBatch.Draw(outlineTex, new Rectangle((int)Pos.X - outlineThickness, (int)Pos.Y - outlineThickness, (int)(SourceImg.Width * Scale) + outlineThickness * 2, (int)(SourceImg.Height * Scale) + outlineThickness * 2), outlineColor);

                spriteBatch.Draw(Texture, Dimension, SourceImg, FrontColor);
            }

            if (Text != null)
            {
                ///version 3.1
                if (withRightReflect)
                    Writer.DrawText(Font, Text, Pos + new Vector2(0, -adjustement), BackColor, FrontColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f, FrontThickness, spriteBatch, colorRightReflect, isEdgeRounded);
                else
                    Writer.DrawText(Font, Text, Pos + new Vector2(0, -adjustement), BackColor, FrontColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f, FrontThickness, spriteBatch, isEdgeRounded);
            }

        }


        public void UpdatePosition()
        {

            if(Text != null)
                Dimension = new Rectangle((int)Pos.X - (int)FrontThickness, (int)Pos.Y - 0 - (int)FrontThickness, (int)(Main.UltimateFont.MeasureString(Text).X * Scale + 9 * (int)Scale) + (int)FrontThickness * 2, (int)((Main.UltimateFont.MeasureString(Text).Y * Scale) / dividAdjustement) + (int)FrontThickness * 2);

            if (Texture != null)
                Dimension = new Rectangle((int)Pos.X, (int)Pos.Y, (int)(SourceImg.Width * Scale), (int)(SourceImg.Height * Scale));

            if (SpecialPosition == Position.noCenter)
                Pos = new Vector2(Pos.X, Pos.Y);
            else if (SpecialPosition == Position.centerX)
                Pos = new Vector2(Main.ScreenWidth / 2 - Dimension.Width / 2 + NewPosition.X, Pos.Y);
            else if (SpecialPosition == Position.centerY)
                Pos = new Vector2(Pos.X, Main.ScreenHeight / 2 - Dimension.Height / 2 + NewPosition.Y);
            else if (SpecialPosition == Position.centerXY)
                Pos = new Vector2(Main.ScreenWidth / 2 - Dimension.Width / 2 + NewPosition.X, Main.ScreenHeight / 2 - Dimension.Height / 2 + NewPosition.Y);

            if (Text != null)
                Dimension = new Rectangle((int)Pos.X - (int)FrontThickness, (int)Pos.Y - 0 - (int)FrontThickness, (int)(Main.UltimateFont.MeasureString(Text).X * Scale + 9 * (int)Scale) + (int)FrontThickness * 2, (int)((Main.UltimateFont.MeasureString(Text).Y * Scale) / dividAdjustement) + (int)FrontThickness * 2);

            if (Texture != null)
                Dimension = new Rectangle((int)Pos.X, (int)Pos.Y, (int)(SourceImg.Width * Scale), (int)(SourceImg.Height * Scale));

        }


        public void SetText(string text)
        {
            this.Text = text;
            UpdatePosition();
        }

        public void SetFont(SpriteFont font)
        {
            this.Font = font;
        }

        public void SetColor(Color frontColor, Color backColor)
        {
            this.FrontColor = frontColor;
            this.BackColor = backColor;
        }

        public void SetScale(float scale)
        {
            this.Scale = scale;
        }

        public void SetFrontThickness(float frontThickness)
        {
            FrontThickness = frontThickness;
        }

        public void SetPosition(float x = 0, float y = 0, Position specialPosition = 0)
        {

            NewPosition.X = x;
            NewPosition.Y = y;
            Pos.X = x;
            Pos.Y = y;
            SpecialPosition = specialPosition;

            UpdatePosition();

            NewPosition = Vector2.Zero;

        }

        public void IsMajuscule(bool isMaj)
        {
            this.IsMaj = isMaj;

            if (!IsMaj)
                adjustement = ((Main.UltimateFont.MeasureString(Text).Y * Scale) + 0 * (int)Scale) / 2;
            else
                adjustement = 0;

            if (!IsMaj)
                dividAdjustement = 2;
            else
                dividAdjustement = 1;

        }

        public enum Position
        {
            noCenter = 0,
            centerX = 1,
            centerY = 2,
            centerXY = 3,
        }


        public Vector2 GetPosition()
        {
            return Pos;
        }

        public int GetWidth()
        {
            return Dimension.Width;
        }

        public int GetHeight()
        {
            return Dimension.Height;
        }

        public bool IsSelected()
        {
            return isSelected;
        }

        public bool IsCliqued()
        {
            return isCliqued;
        }


        /// <summary>
        /// Version 3.1
        /// For selection with Keyboard or Manette.
        /// </summary>
        public void SetAroundButton(ButtonV3 _upButton = null, ButtonV3 _downButton = null, ButtonV3 _rightButton = null, ButtonV3 _leftButton = null)
        {
            UpButton = _upButton;
            DownButton = _downButton;
            RightButton = _rightButton;
            LeftButton = _leftButton;
        }

        /// <summary>
        /// Version 3.1
        /// </summary>
        public void SetIsSelected(bool param)
        {
            isSelected = param;
        }

        public void SetIsMultiClic(bool param)
        {
            isMultiClic = param;
        }

        public bool IsMultiClic()
        {
            return isMultiClic;
        }

        public bool IsReleased()
        {
            return isReleased;
        }

        public Texture2D GetTexture()
        {
            return Texture;
        }

        public void SetTexture(Texture2D _tex, Rectangle _source)
        {
            Texture = _tex;
            SourceImg = _source;
        }

        /// <summary>
        /// Version 3.2
        /// </summary>
        /// <param name="_texture"></param>
        /// <param name="_color"></param>
        /// <param name="_thickness"></param>
        public void SetOutline(Texture2D _texture, Color _color, int _thickness)
        {
            outlineTex = _texture;
            outlineColor = _color;
            outlineThickness = _thickness;
        }

        public void SetOutlineColor(Color _color)
        {
            outlineColor = _color;
        }

    }
}
