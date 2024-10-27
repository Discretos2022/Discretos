using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateform_2D_v9.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{

    /// <summary>
    /// Version 2 des Items
    /// </summary>
    /// <param name="r1"></param>
    /// <param name="r2"></param>
    /// <returns></returns>
    class ItemV2 : Actor
    {

        private int time = 0;

        private Texture2D texture;

        public ItemID ID;


        public ItemV2(Vector2 Position, ItemID ID, Vector2 Acceleration)
            : base(Position)
        {
            actorType = ActorType.Item;
            this.ID = ID;

            this.Acceleration = Acceleration;
            Velocity.X = Acceleration.X;

            texture = Main.SpriteSheetItem[(int)ID];

            canBreakBlock = false;
            canOpenDoor = false;

        }

        public override void Update(GameTime gameTime)
        {
            if (isOnGround)
                this.Wind = Vector2.Zero;
            if(!isOnGround)
                this.Wind = Play.Wind;
            if (Play.Wind.X >= 1.5f || Play.Wind.X <= -1.5f && isOnGround)
                this.Wind = Play.Wind / 4;

            this.OldPosition.X = Position.X;
            this.OldPosition.Y = Position.Y;

            Position.Y += Wind.Y;

            ApplyPhysic();

            if ((int)Velocity.Y > 0 && isOnSlope == TileV2.SlopeType.None)
                isOnGround = false;

            time++;

            if (time >= 240 || IsSquish()) // 4s
                Handler.actors.Remove(this);

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, Position, Color.White);
        }

        public void ApplyPhysic()
        {

            Acceleration.Y += 0.35f;


            Velocity.Y = Acceleration.Y;

            if (Velocity.Y > 10)
                Velocity.Y = 10;

            if (Acceleration.Y > 10)
                Acceleration.Y = 10;

            OldPosition.Y = Position.Y;

        }

        public override Rectangle GetAttackRectangle()
        {
            throw new NotImplementedException();
        }

        public override Vector2 GetPosForCamera()
        {
            return Position;
        }

        public override Rectangle GetRectangle()
        {
            return hitbox.rectangle;
        }

        public override bool HasLowerState()
        {
            throw new NotImplementedException();
        }

        public override bool IsLower()
        {
            throw new NotImplementedException();
        }

        public override bool IsSquish()
        {
            if (LeftCollision && RightCollision)
                goto L_1;
            else if (UpCollision && DownCollision)
                goto L_1;


            LeftCollision = false;
            RightCollision = false;
            UpCollision = false;
            DownCollision = false;

            return false;

        L_1:
            {

                //Console.WriteLine(Left + " ; " + Right + "  :  " + Down + " ; " + Up);

                LeftCollision = false;
                RightCollision = false;
                UpCollision = false;
                DownCollision = false;

                return true;
            }
        }


        public enum ItemID
        {

            diamond = 1,
            copper = 2,
            iron = 3,
            amethyst = 4,
            rubis = 5,
            saphir = 6,

        }

        /** COLLISION V3 **/

        public override void LeftDisplacement(GameTime gameTime)
        {
            if(Velocity.X < 0) Position.X += Velocity.X;
            if (Wind.X < 0) Position.X += Wind.X;
            
            base.LeftDisplacement(gameTime);
        }

        public override void RightDisplacement(GameTime gameTime)
        {
            if (Velocity.X > 0) Position.X += Velocity.X;
            if (Wind.X > 0) Position.X += Wind.X;
            
            base.RightDisplacement(gameTime);
        }

        public override void DownDisplacement(GameTime gameTime)
        {
            if (Velocity.Y > 0) Position.Y += (int)Velocity.Y;
            UpdateHitbox();
        }

        public override void UpDisplacement(GameTime gameTime)
        {
            if (Velocity.Y < 0) Position.Y += (int)Velocity.Y;
            UpdateHitbox();
        }


        public override void LeftStaticCollisionAction()
        {
            Velocity.X *= -0.5f;
        }
        public override void RightStaticCollisionAction()
        {
            Velocity.X *= -0.5f;
        }
        public override void DownStaticCollisionAction()
        {
            Velocity.X = 0;
        }
        public override void UpStaticCollisionAction()
        {

        }


        public override void LeftDynamicCollisionAction(MovingBlock block)
        {
            if (block.Velocity.X < 0 && Velocity.X < 0)
                Velocity.X *= -0.2f;
            else if (block.Velocity.X > 0 && Velocity.X < 0)
                Velocity.X *= -0.8f;
            else if (block.Velocity.X > 0 && Velocity.X > 0)
                Velocity.X *= -1.2f;
        }
        public override void RightDynamicCollisionAction(MovingBlock block)
        {
            if (block.Velocity.X < 0 && Velocity.X < 0)
                Velocity.X *= -1.2f;
            else if (block.Velocity.X < 0 && Velocity.X > 0)
                Velocity.X *= -0.8f;
            else if (block.Velocity.X > 0 && Velocity.X > 0)
                Velocity.X *= -0.2f;
        }
        public override void DownDynamicCollisionAction(MovingBlock block)
        {
            Velocity.X = 0;
        }
        public override void UpDynamicCollisionAction(MovingBlock block)
        {

        }


        public override void UpdateHitbox()
        {
            hitbox = new Hitbox((int)Position.X, (int)Position.Y, 7, 5);
        }


    }
}
