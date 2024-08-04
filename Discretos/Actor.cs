using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateform_2D_v9.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    public abstract class Actor
    {

        public Vector2 Position;
        public Vector2 OldPosition;
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public Vector2 Wind;
        public float Gravity;
        public int PV;
        public Vector2 KnockBack;

        public bool isOnGround;
        public bool isOnSlope;
        public bool Hited;
        public bool isLocked;
        public bool isCollected;
        public bool CheckPointHited;

        public int NumOfTriggerObject;

        public int ID;
        public ActorType actorType;

        //public bool LeftCollision;
        //public bool RightCollision;

        public Light light;

        public Hitbox hitbox;

        public Actor(Vector2 Position)
        {
            this.Position = Position;
        }


        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch render, GameTime gameTime);

        public abstract bool IsRiding();
        public abstract void SetRidingTile(TileV2 tile);
        public abstract bool IsSquish();
        public abstract bool IsLower();
        public abstract void RemovePV(int PV);
        public abstract bool HasLowerState();

        public abstract Rectangle GetRectangle();

        public Hitbox GetHitbox()
        {
            return hitbox;
        }

        public abstract Rectangle GetAttackRectangle();

        public abstract Vector2 GetPosForCamera();

        public Vector2 GetOldPosition()
        {
            return OldPosition;
        }

        public Vector2 GetVelocity()
        {
            return Velocity;
        }

        public int GetID()
        {
            return ID;
        }

        public ActorType GetActorType()
        {
            return actorType;
        }

        public enum ActorType
        {
            Player = 1,
            Enemy = 2,
            Object = 3,
            Item = 4,
        };



        public abstract void LeftDisplacement(GameTime gameTime);
        public abstract void RightDisplacement(GameTime gameTime);
        public abstract void DownDisplacement(GameTime gameTime);
        public abstract void UpDisplacement(GameTime gameTime);

        public virtual void LeftStaticCollision() { }

        public virtual void RightStaticCollision() { }

        public virtual void DownStaticCollision() { }

        public virtual void UpStaticCollision() { }



        public virtual void LeftDynamicCollision() { }
        public virtual void RightDynamicCollision() { }
        public virtual void DownDynamicCollision() { }
        public virtual void UpDynamicCollision() { }


        public abstract void UpdateHitbox();


    }
}
