using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    class Wall : Solid
    {

        private int w = 16;
        private int h = 16;

        private Vector2 PosInLevel;

        private int variante;

        private Rectangle Frame;

        public Wall(Vector2 Position, int type, int variante, bool isSlope = false, bool isStatic = true)
            : base(new Vector2(Position.X, Position.Y), type)
        {
            this.Position = Position;
            this.type = type;
            this.isSlope = isSlope;
            this.isInvisible = false;
            this.variante = variante;

            Frame = GetFrame(this.variante);

            w = 16;
            h = 16;

            PosInLevel = Position / 16;

        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Main.Wallset[type], GetPosition(), Frame, Color.White);
        }

        public override void Break()
        {
            
        }

        public override Vector2 GetPosition() { return Position; }

        public override Rectangle GetRectangle()
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

        
    }
}
