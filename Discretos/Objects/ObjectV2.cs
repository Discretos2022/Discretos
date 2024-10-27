using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateform_2D_v9.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9.Objects
{
    /// <summary>
    /// Version 3 de LevelItem
    /// </summary>
    /// <param name="r1"></param>
    /// <param name="r2"></param>
    /// <returns></returns>
    public abstract class ObjectV2 : Actor
    {

        public ObjectID objectID;

        public ObjectV2(Vector2 Position)
            : base(Position)
        {
            actorType = ActorType.Object;
        }


        public abstract void Init();

        public override abstract void Update(GameTime gameTime);

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            DEBUG.DebugCollision(hitbox.rectangle, Color.DarkBlue, spriteBatch);
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




        /** COLLISION V3 **/


        public override void LeftDisplacement(GameTime gameTime)
        {
            
        }

        public override void RightDisplacement(GameTime gameTime)
        {
            
        }

        public override void DownDisplacement(GameTime gameTime)
        {
            
        }

        public override void UpDisplacement(GameTime gameTime)
        {
            
        }

        public override void LeftStaticCollision()
        {
            
        }

        public override void RightStaticCollision()
        {
            
        }

        public override void DownStaticCollision()
        {
            
        }

        public override void UpStaticCollision()
        {
            
        }

        public override abstract void UpdateHitbox();


        public enum ObjectID
        {
            coin = 1,
            core = 2,
            checkPoint = 3,
            wood_door = 4,
            gold_key = 5,
            spring = 6,
            wood_ladder = 7,
            wood_ladder_snow = 8,
            torch = 9,

        };


    }
}