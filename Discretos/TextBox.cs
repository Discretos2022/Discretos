using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    class TextBox
    {

        private string text = "";
        private int maxCharacters;
        private bool lockAlpha;
        private bool lockSign;

        private string[] characters;
        private List<string> oldCharacters = new List<string>();
        private List<string> listedCharacters = new List<string>();

        private Color frontColor = Color.White;
        private Color backColor = Color.Black;

        private float scale;
        private float frontThickness;
        private bool isEdgeRounded;

        public bool isSelected = false;
        private bool isHide = false;
        private int animationNum;

        private Vector2 Position;
        private float x, y;
        private ButtonV3.Position specialPosition;

        private Rectangle Dimension = Rectangle.Empty;

        public TextBox(int _maxCharacters = 20, float _scale = 1, float _frontThickness = 1, bool _isEdgeRounded = false, string _defaultText = "", bool _lockAlpha = false, bool _lockSign = false)
        {
            maxCharacters = _maxCharacters;
            characters = new string[maxCharacters];
            scale = _scale;
            frontThickness = _frontThickness;
            isEdgeRounded = _isEdgeRounded;
            lockAlpha = _lockAlpha;
            lockSign = _lockSign;

            for (int i = 0; i < _defaultText.Length; i++)
            {
                for (int j = 0; j < characters.Length; j++)
                {
                    if (characters[j] == null)
                    {
                        characters[j] = _defaultText.ToCharArray().GetValue(i).ToString();
                        break;
                    }
                        
                }
            }

        }

        public void Update()
        {

            string character;

            for (int i = 0; i < oldCharacters.Count; i++)
            {
                oldCharacters.Remove(oldCharacters[i]);
            }

            for (int i = 0; i < listedCharacters.Count; i++)
            {
                oldCharacters.Add(listedCharacters[i]);
            }

            for (int i = 0; i < listedCharacters.Count; i++)
            {
                listedCharacters.Remove(listedCharacters[i]);
            }

            if (characters[maxCharacters - 1] == null)
            {
                if (KeyInput.getKeyState().GetPressedKeys().Length != 0)
                {
                    for (int i = 0; i < KeyInput.getKeyState().GetPressedKeys().Length; i++)
                    {
                        character = KeyInput.getKeyState().GetPressedKeys().GetValue(i).ToString();

                        listedCharacters.Add(character);

                        for (int x = 0; x < oldCharacters.Count; x++)
                        {
                            if (character == oldCharacters[x])
                            {
                                goto l_1;
                            }
                                
                        }
                        
                        // Basic
                        if (character.Length == 1 && !lockAlpha)
                            for (int j = 0; j < characters.Length; j++)
                            {
                                if (characters[j] == null)
                                {
                                    if(KeyInput.getKeyState().IsKeyDown(Keys.LeftShift) || KeyInput.getKeyState().IsKeyDown(Keys.RightShift))
                                        characters[j] = character;
                                    else
                                        characters[j] = character.ToLower();
                                    goto l_1;
                                }
                            }

                        // Numeric
                        if(character.Length == 2)
                        {
                            for(int y = 0; y <= 10; y++)
                            {
                                if(character == "D" + y)
                                {
                                    for (int j = 0; j < characters.Length; j++)
                                    {
                                        if (characters[j] == null)
                                        {
                                            characters[j] = y.ToString();
                                            goto l_1;
                                        }
                                    }
                                }
                            }
                        }

                        // Point and 2 points
                        if(character == "OemPeriod" && !lockSign)
                        {
                            for (int j = 0; j < characters.Length; j++)
                            {
                                if (characters[j] == null)
                                {
                                    if(KeyInput.getKeyState().IsKeyDown(Keys.LeftShift) || KeyInput.getKeyState().IsKeyDown(Keys.RightShift))
                                        characters[j] = ":";
                                    else
                                        characters[j] = ".";
                                    goto l_1;
                                }
                            }
                        }

                        // Space
                        if (character == "Space" && !lockSign)
                        {
                            for (int j = 0; j < characters.Length; j++)
                            {
                                if (characters[j] == null)
                                {
                                    characters[j] = " ";
                                    goto l_1;
                                }
                            }
                        }

                    l_1:;

                    }

                }

            }

            if (KeyInput.getKeyState().IsKeyDown(Keys.Back) && !KeyInput.getOldKeyState().IsKeyDown(Keys.Back) && characters.Length >= 1)
                for (int i = 0; i < characters.Length; i++)
                {

                    if (characters[i] == null)
                    {

                        if(i != 0)
                            characters[i - 1] = null;

                        else if(i == 1)
                            characters[i] = null;

                        break;
                    }

                    if (i == characters.Length - 1)
                    { characters[i] = null; break; }

                }

            text = string.Concat(characters);

            animationNum++;
            if(animationNum >= 30)
            {
                animationNum = 0;

                if (isHide)
                    isHide = false;
                else
                    isHide = true;

            }

            CalculatePosition();

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CalculatePosition();

            DEBUG.DebugCollision(Dimension, Color.DarkGreen, spriteBatch);

            Writer.DrawText(Main.UltimateFont, text, new Vector2(Dimension.X, Dimension.Y), backColor, frontColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f, frontThickness, spriteBatch, isEdgeRounded);

            if (isSelected && !isHide && text.Length != maxCharacters)
            {
                int num = (int)(Main.UltimateFont.MeasureString("A").Y * scale);
                spriteBatch.Draw(Main.Bounds, new Rectangle((int)(Dimension.X + Dimension.Width - frontThickness + 2 * scale), (int)(Dimension.Y + 6 * scale - frontThickness), (int)(1 * scale + frontThickness*2), (int)(num + frontThickness * 2 - 6 * scale)), backColor);
                spriteBatch.Draw(Main.Bounds, new Rectangle((int)(Dimension.X + Dimension.Width + 2 * scale), (int)(Dimension.Y + 6 * scale), (int)(1 * scale), (int)(num - 6 * scale)), frontColor);
                
            }


        }

        public void SetColor(Color _frontColor, Color _backColor)
        {
            frontColor = _frontColor;
            backColor = _backColor;
        }

        public void SetPosition(float _x = 0, float _y = 0, ButtonV3.Position _specialPosition = 0)
        {
            x = _x;
            y = _y;
            specialPosition = _specialPosition;
        }

        public void CalculatePosition()
        {
            if (specialPosition == ButtonV3.Position.noCenter)
                Position = new Vector2(x, y);
            else if (specialPosition == ButtonV3.Position.centerX)
                Position = new Vector2(Main.ScreenWidth / 2 - Dimension.Width / 2 + x, y);
            else if (specialPosition == ButtonV3.Position.centerY)
                Position = new Vector2(x, Main.ScreenHeight / 2 - Dimension.Height / 2 + y);
            else if (specialPosition == ButtonV3.Position.centerXY)
                Position = new Vector2(Main.ScreenWidth / 2 - Dimension.Width / 2 + x, Main.ScreenHeight / 2 - Dimension.Height / 2 + y);

            Dimension = new Rectangle((int)Position.X + (int)frontThickness, (int)Position.Y - 0 - (int)frontThickness, (int)(Main.UltimateFont.MeasureString(text).X * scale + 9 * (int)scale) + (int)frontThickness * 2, (int)((Main.UltimateFont.MeasureString(text).Y * scale)) + (int)frontThickness * 2);
        }

        public String GetText()
        {
            return text;
        }

    }
}
