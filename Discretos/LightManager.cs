using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    public static class LightManager
    {

        public static List<Light> lights;

        public static Color AmbianteLightR;
        public static Color AmbianteLightG;
        public static Color AmbianteLightB;


        public static void Init()
        {
            lights = new List<Light>();
        } 

        public static void Update()
        {

        }

        public static void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(Main.Bounds, new Rectangle(0, 0, 2000, 500), AmbianteLightR);
            spriteBatch.Draw(Main.Bounds, new Rectangle(0, 0, 2000, 500), AmbianteLightG);
            spriteBatch.Draw(Main.Bounds, new Rectangle(0, 0, 2000, 500), AmbianteLightB);

            for (int i = 0; i < lights.Count; i++)
            {
                lights[i].Draw(spriteBatch);
            }
        }


    }
}
