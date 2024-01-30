using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    public abstract class Solid
    {

        public Vector2 Position;
        public Vector2 OldPosition;
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public Vector2 Wind;
        public Vector2 Gravity;

        public int type;

        public bool LeftCollision;
        public bool RightCollision;


        public bool isStatic;
        public bool isSlope;
        public bool isBreakable;
        public bool isBroken;
        public bool isTouched;    /// For Breackable Block
        public bool isInvisible;

        public int SlopeType;


        public Solid(Vector2 Position, int type)
        {
            this.Position = Position;
            //this.type = type;
        }


        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch render, GameTime gameTime);

        public abstract Rectangle GetRectangle();

        public abstract Vector2 GetPosition();


        public int getType()
        {
            return type;
        }


        public Vector2 GetVelocity()
        {
            return new Vector2(Velocity.X, Velocity.Y);
        }

        public abstract void Break();

    }
}
