using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateform_2D_v9.Enemy
{
    public abstract class EnemyV3 : Actor
    {


        private new readonly float Gravity;   //0.3f

        private readonly int invincibleTime = 7;
        private int time;

        public bool isLeft;

        public bool Hited;

        public EnemyType ID;
        public int PV;



        public EnemyV3(Vector2 Position) : base(Position)
        {

            this.actorType = ActorType.Enemy;

        }

        public override abstract void Update(GameTime gameTime);

        public override abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        public override abstract Rectangle GetAttackRectangle();

        /// <summary>
        /// Need revision
        /// </summary>
        /// <returns></returns>
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
            return false;
        }

        public override bool IsLower()
        {
            return false;
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
                LeftCollision = false; RightCollision = false; UpCollision = false; DownCollision = false;
                return true;
            }
        }

        public void RemovePV(int PV)
        {
            this.PV -= PV;
        }

        public override abstract void UpdateHitbox();

        public override void UpDisplacement(GameTime gameTime)
        {
            if (Velocity.Y < 0) Position.Y += (int)Velocity.Y;
            UpdateHitbox();
        }

        public override void DownDisplacement(GameTime gameTime)
        {
            if (Velocity.Y > 0) Position.Y += (int)Velocity.Y;

            if (isOnSlope == TileV2.SlopeType.LeftDown && Velocity.X < 0)
                Position.Y += BaseVelocity.X;

            else if (isOnSlope == TileV2.SlopeType.RightDown && Velocity.X > 0)
                Position.Y += BaseVelocity.X + 1;

            UpdateHitbox();
        }


        public enum EnemyType
        {
            spearknight = 1,
            swordman = 2,
            newknight = 3,
        }

    }
}
