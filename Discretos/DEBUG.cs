using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    static class DEBUG
    {

        public static void DebugCollision(Rectangle rectangle, Color color, SpriteBatch spriteBatch)
        {

            if(Main.Debug)
                spriteBatch.Draw(Main.Bounds, rectangle, color);


        }

        public static void Log(string ERROR)
        {
            if (Main.Debug)
                Console.WriteLine(ERROR);
        }

    }
}
