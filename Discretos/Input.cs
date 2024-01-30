using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    class MouseInput
    {
        private Main main;

        private static MouseState mouseState;
        private static MouseState oldmouseState;
        private static Rectangle MousePos;

        public static bool IsActived = true;


        public static Point WindowPosition
        {
            get { return mouseState.Position; }
        }


        private static float X;
        private static float Y;

        public MouseInput(Main main)
        {
            this.main = main;
        }


        public static void Update(Screen screen)
        {
            oldmouseState = mouseState;
            mouseState = Mouse.GetState();

            X = GetScreenPosition(screen).X / 4;
            Y = GetScreenPosition(screen).Y / 4;

            MousePos = new Rectangle((int)X / 4, (int)Y, 2, 2);

        }


        public static Vector2 GetScreenPosition(Screen screen)
        {
            Rectangle screenDestinationRectangle = screen.CalculateDestinationRectangle();

            Point windowPosition = WindowPosition;

            float sx = windowPosition.X - screenDestinationRectangle.X;
            float sy = windowPosition.Y - screenDestinationRectangle.Y;

            sx /= (float)screenDestinationRectangle.Width;
            sy /= (float)screenDestinationRectangle.Height;

            sx *= (float)screen.Width;
            sy *= (float)screen.Height;

            return new Vector2(sx, sy);
        }


        public static Rectangle GetRectangle(Screen screen)
        {
            return new Rectangle((int)GetScreenPosition(screen).X, (int)GetScreenPosition(screen).Y, 2, 2);
        }

        public static Vector2 GetPos()
        {
            return new Vector2(getMouseState().X, getMouseState().Y);
        }


        public static Vector2 GetOldPos()
        {
            return new Vector2(getOldMouseState().X, getOldMouseState().Y); 
        }

        public static MouseState getMouseState()
        {
            return mouseState;
        }

        public static Vector2 GetLevelPos(bool isFullScreen, Camera camera)
        {
            if (isFullScreen)
                return new Vector2(X + camera.Position.X - (1920 / 4) / 2, Y + camera.Position.Y - (1080 / 4) / 2);

            return new Vector2(X + camera.Position.X - (1920 / 8), Y + camera.Position.Y - (1080 / 8));
        }

        public static MouseState getOldMouseState()
        {
            return oldmouseState;
        }

        /// <summary>
        /// Discretos 9.7
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool isSimpleClickLeft()
        {
            if (getMouseState().LeftButton == ButtonState.Pressed && getOldMouseState().LeftButton != ButtonState.Pressed)
                return true;

            return false;
        }

        public static void UpdateInput()
        {
            oldmouseState = mouseState;
            mouseState = Mouse.GetState();
        }

        public static void UpdatePosition(Screen screen)
        {
            X = GetScreenPosition(screen).X / 4;
            Y = GetScreenPosition(screen).Y / 4;

            MousePos = new Rectangle((int)X / 4, (int)Y, 2, 2);
        }

    }

    
    class KeyInput
    {

        private static KeyboardState keyState;
        private static KeyboardState oldKeyState;

        public KeyInput()
        {

        }


        public static void Update()
        {
            oldKeyState = keyState;
            keyState = Keyboard.GetState();

        }

        public static KeyboardState getKeyState()
        {
            return keyState;
        }

        public static KeyboardState getOldKeyState()
        {
            return oldKeyState;
        }

        /// <summary>
        /// Discretos 9.7
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool isSimpleClick(Keys keys)
        {
            if (getKeyState().IsKeyDown(keys) && !getOldKeyState().IsKeyDown(keys))
                return true;

            return false;
        }

        /// <summary>
        /// Discretos 9.7
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool isSimpleClick(Keys keys1, Keys keys2)
        {
            if (getKeyState().IsKeyDown(keys1) && !getOldKeyState().IsKeyDown(keys1))
                return true;

            else if (getKeyState().IsKeyDown(keys2) && !getOldKeyState().IsKeyDown(keys2))
                return true;

            return false;
        }


    }


    public class GamePadInput
    {

        private static GamePadState padState1;
        private static GamePadState padState2;
        private static GamePadState padState3;
        private static GamePadState padState4;

        private static GamePadState oldPadState1;
        private static GamePadState oldPadState2;
        private static GamePadState oldPadState3;
        private static GamePadState oldPadState4;


        private static GamePadState[] padState = new GamePadState[4];
        private static GamePadState[] oldPadState = new GamePadState[4];


        public static void Update(PlayerIndex index)
        {
            oldPadState[(int)index] = padState[(int)index];
            padState[(int)index] = GamePad.GetState(index);
        }


        public static GamePadState GetPadState(PlayerIndex index)
        {
            return padState[(int)index];
        }

        public static GamePadState GetOldPadState(PlayerIndex index)
        {
            return oldPadState[(int)index];
        }

        /// <summary>
        /// Discretos 9.7
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool isSimpleClick(PlayerIndex index, Buttons button)
        {
            if (GetPadState(index).IsButtonDown(button) && !GetOldPadState(index).IsButtonDown(button))
                return true;

            return false;
        }

        public static bool isSimpleClick(PlayerIndex index, Buttons button1, Buttons button2)
        {
            if (GetPadState(index).IsButtonDown(button1) && !GetOldPadState(index).IsButtonDown(button1))
                return true;

            if (GetPadState(index).IsButtonDown(button2) && !GetOldPadState(index).IsButtonDown(button2))
                return true;

            return false;
        }


    }


}
