using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    class Wall
    {

        private int w = 16;
        private int h = 16;

        public Vector2 Position;
        private Vector2 PosInLevel;

        public WallID ID;
        private int variante;

        private Rectangle Frame;

        public Wall(Vector2 Position, WallID id, int variante)
        {
            this.Position = Position;
            this.variante = variante;
            this.ID = id;

            Frame = GetFrame(this.variante);

            w = 16;
            h = 16;

            PosInLevel = Position / 16;

        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Main.Wallset[(int)ID], GetPosition(), Frame, Color.White);
        }

        public void Break()
        {
            
        }

        public Vector2 GetPosition() { return Position; }

        public Rectangle GetRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, w, h);
        }


        public Rectangle GetFrame(int variante)
        {

            Vector2 frame;

            switch (variante)
            {
                case 1: frame = new Vector2(0, 0); break;
                case 2: frame = new Vector2(1, 0); break;
                case 3: frame = new Vector2(2, 0); break;
                case 4: frame = new Vector2(0, 1); break;
                case 5: frame = new Vector2(1, 1); break;
                case 6: frame = new Vector2(2, 1); break;
                case 7: frame = new Vector2(0, 2); break;
                case 8: frame = new Vector2(1, 2); break;
                case 9: frame = new Vector2(2, 2); break;

                case 10: frame = new Vector2(0, 3); break;

                case 11: frame = new Vector2(3, 0); break;
                case 12: frame = new Vector2(4, 0); break;
                case 13: frame = new Vector2(3, 1); break;
                case 14: frame = new Vector2(4, 1); break;

                case 15: frame = new Vector2(3, 2); break;
                case 16: frame = new Vector2(4, 2); break;
                case 17: frame = new Vector2(3, 3); break;
                case 18: frame = new Vector2(4, 3); break;

                default: frame = new Vector2(0, 0); break;
            }


            return new Rectangle((int)(frame.X * 16 + 1 * frame.X), (int)(frame.Y * 16 + 1 * frame.Y), 16, 16);



        }


        public enum WallID
        {
            none = 0,
            snow = 4,
            brick_gray = 6,
        }

        
    }
}
