using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    public class ObjectBounds : Solid
    {

        private Vector2 Position;
        private Vector2 PosInLevel;

        private Rectangle Dimension;

        private int w;
        private int h;
        private int Size;

        private bool isUnknowedTile;

        private bool Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft;

        private Animation mechanicBlock;
        private Animation platformBrickBreak;

        public int timer = 0;


        public ObjectBounds(Vector2 Position, int type, int Width, int Height)
            : base(new Vector2(Position.X, Position.Y), type)
        {
            this.Position = Position;
            this.type = type;
            this.isInvisible = true;

            w = Width;
            h = Height;

            Dimension = new Rectangle((int)Position.X, (int)Position.Y, w, h);

            PosInLevel = Position / 16;

        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            DEBUG.DebugCollision(GetRectangle(), Color.Brown, spriteBatch);
        }

        public override Rectangle GetRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, w, h);
        }

        public override Vector2 GetPosition()
        {
            return Position;
        }

        public override void Break()
        {
            Handler.solids.Remove(this);
        }
    }
}
