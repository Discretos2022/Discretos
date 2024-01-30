using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    public class Light
    {

        public Vector2 Position;
        public float Intensity;
        public float Radius;
        public Color Color;

        public Light(Vector2 position, float intensity, float radius, Color col = default)
        {
            Position = position;
            Intensity = intensity;
            Radius = radius;
            Color = col;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            float Scale = (Radius) * (1000) / (500) / 1000;

            spriteBatch.Draw(Main.Light_1_1000x1000, Position - new Vector2(Radius), null, Color, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, 0f);
        }

    }
}
