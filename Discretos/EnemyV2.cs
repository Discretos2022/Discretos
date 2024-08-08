using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetworkEngine_5._0.Client;
using NetworkEngine_5._0.Server;
using Plateform_2D_v9.Core;
using Plateform_2D_v9.NetCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    class EnemyV2 : Actor
    {

        private new readonly float Gravity = 0.35f;   //0.3f

        private readonly int invincibleTime = 7;
        private int time;

        private Animation Walk;

        private bool isLeft;


        public EnemyV2(Vector2 Position, int ID)
            : base(new Vector2(Position.X, Position.Y))
        {
            this.actorType = ActorType.Enemy;
            this.ID = ID;
            this.PV = 3;

            InitEnemy();

        }

        public override void Update(GameTime gameTime)
        {

            if(isOnGround)
                this.Wind = Play.Wind / 6;
            else
                this.Wind = Play.Wind;

            Walk.Update(gameTime);


            OldPosition.X = Position.X;
            OldPosition.Y = Position.Y;

            if (time > 0)
            {
                time--;
            }

            if (time <= 0)
                Hited = false;

            ActorCollision();

            ApplyPhysic();

            if ((int)Velocity.Y > 0 && isOnSlope == TileV2.SlopeType.None)
                isOnGround = false;

            if (PV <= 0 || IsSquish())
            {

                Vector2 pos = new Vector2(Position.X, Position.Y);
                int id1 = Util.random.Next(1, 7);
                int id2 = Util.random.Next(1, 7);
                int id3 = Util.random.Next(1, 7);
                int id4 = Util.random.Next(1, 7);
                Vector2 v1 = new Vector2((float)-Util.random.NextDouble(), (float)Util.random.Next(-2, 0));
                Vector2 v2 = new Vector2((float)-Util.random.NextDouble(), (float)Util.random.Next(-2, 0));
                Vector2 v3 = new Vector2((float)-Util.random.NextDouble(), (float)Util.random.Next(-2, 0));
                Vector2 v4 = new Vector2((float)-Util.random.NextDouble(), (float)Util.random.Next(-2, 0));

                /*Handler.actors.Add(new ItemV2(new Vector2(Position.X, Position.Y), (int)Util.random.Next(1, 7), new Vector2((float)-Util.random.NextDouble(), (float)Util.random.Next(-2, 0))));
                Handler.actors.Add(new ItemV2(new Vector2(Position.X, Position.Y), (int)Util.random.Next(1, 7), new Vector2((float)-Util.random.NextDouble(), (float)Util.random.Next(-2, 0))));
                Handler.actors.Add(new ItemV2(new Vector2(Position.X, Position.Y), (int)Util.random.Next(1, 7), new Vector2((float)Util.random.NextDouble(), (float)Util.random.Next(-2, 0))));
                Handler.actors.Add(new ItemV2(new Vector2(Position.X, Position.Y), (int)Util.random.Next(1, 7), new Vector2((float)Util.random.NextDouble(), (float)Util.random.Next(-2, 0))));*/

                Handler.actors.Add(new ItemV2(pos, id1, v1));
                Handler.actors.Add(new ItemV2(pos, id2, v2));
                Handler.actors.Add(new ItemV2(pos, id3, v3));
                Handler.actors.Add(new ItemV2(pos, id4, v4));

                if (NetPlay.IsMultiplaying)
                {

                    if(NetPlay.MyPlayerID() == 1)
                    {
                        ServerSender.SendCreatedItem(pos.X, pos.Y, id1, v1.X, v1.Y);
                        ServerSender.SendCreatedItem(pos.X, pos.Y, id2, v2.X, v2.Y);
                        ServerSender.SendCreatedItem(pos.X, pos.Y, id3, v3.X, v3.Y);
                        ServerSender.SendCreatedItem(pos.X, pos.Y, id4, v4.X, v4.Y);
                    }
                    else
                    {
                        ClientSender.SendCreatedItem(pos.X, pos.Y, id1, v1.X, v1.Y);
                        ClientSender.SendCreatedItem(pos.X, pos.Y, id2, v2.X, v2.Y);
                        ClientSender.SendCreatedItem(pos.X, pos.Y, id3, v3.X, v3.Y);
                        ClientSender.SendCreatedItem(pos.X, pos.Y, id4, v4.X, v4.Y);
                    }

                }

                Handler.RemoveActor(this);

            }


        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            DEBUG.DebugCollision(GetRectangle(), Color.Blue, spriteBatch);

            DEBUG.DebugCollision(GetAttackRectangle(), Color.Red, spriteBatch);

            //spriteBatch.DrawString(Main.UltimateFont, PV.ToString(), Position + new Vector2(0, -20), Color.Black);


            if (isLeft)
                Walk.Draw(spriteBatch, Position + new Vector2(-14, -3), SpriteEffects.FlipHorizontally);
            else
                Walk.Draw(spriteBatch, Position + new Vector2(-15, -3));


            if (Main.Debug)
            {
                Writer.DrawText(Main.UltimateFont, "velocity " + Velocity.X, Position + new Vector2(20, 0), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);
                Writer.DrawText(Main.UltimateFont, "Acceleration " + Acceleration.X, Position + new Vector2(20, 10), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);

            }

        }

        public void InitEnemy()
        {
            switch (ID)
            {
                case 1:
                    Walk = new Animation(Main.Enemy[ID], 4, 1, 0.15f);
                    Velocity = new Vector2(0.6f, 0); // 0.6f, 0
                    BaseVelocity = new Vector2(0.6f, 0);
                    KnockBack = new Vector2(3, 2);
                    Walk.Start();
                    break;

                case 2:
                    Walk = new Animation(Main.Enemy[ID], 4, 1, 0.10f);
                    Velocity = new Vector2(0.8f, 0);
                    BaseVelocity = new Vector2(0.8f, 0);
                    KnockBack = new Vector2(1, 2); // 2, 0
                    Walk.Start();
                    break;

                default:
                    //rectangle = new Rectangle(0, 0, 0, 0);
                    break;
            }
        }

        public override Rectangle GetRectangle()
        {
            return hitbox.rectangle;
        }

        public override Rectangle GetAttackRectangle()
        {
            switch (ID)
            {
                case 1:
                    if (!isLeft)
                        return new Rectangle((int)Position.X + 12, (int)Position.Y + 13, 14, 6);
                    else
                        return new Rectangle((int)Position.X - 12, (int)Position.Y + 13, 14, 6);

                case 2:
                    if (!isLeft)
                        return new Rectangle((int)Position.X + 14, (int)Position.Y + 2, 8, 13);
                    else
                        return new Rectangle((int)Position.X - 8, (int)Position.Y + 2, 8, 13);

                default:
                    return new Rectangle(0, 0, 0, 0);

            }
        }

        public override bool HasLowerState()
        {
            return false;
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
                LeftCollision = false; RightCollision = false; UpCollision = false; DownCollision = false;
                return true;
            }
        }

        public void ActorCollision()
        {

            /// (new) system : Dictonary of players
            for (int i = 1; i <= Handler.playersV2.Count; i++)
            {
                Actor actor = Handler.playersV2[i];

                if (actor.actorType == ActorType.Player)
                {
                    if (hitbox.rectangle.Intersects(actor.GetAttackRectangle()))
                    {
                        RemovePV(1);
                        Acceleration.Y = -KnockBack.Y;


                        if (hitbox.rectangle.X <= actor.hitbox.rectangle.X && isLeft)
                        {
                            Velocity.X *= -1;
                            isLeft = false;
                        }
                        else if (hitbox.rectangle.X >= actor.hitbox.rectangle.X && !isLeft)
                        {
                            Velocity.X *= -1;
                            isLeft = true;
                        }

                        if (hitbox.rectangle.X >= actor.hitbox.rectangle.X)
                            Velocity.X = KnockBack.X;
                        else if (hitbox.rectangle.X < actor.hitbox.rectangle.X)
                            Velocity.X = -KnockBack.X;


                    }
                }
            }
        }

        public override void RemovePV(int PV)
        {
            //if (!Hited)
            //{
                this.PV -= PV;
                this.Hited = true;
                time = invincibleTime;
            //}
            
        }

        public void ApplyPhysic()
        {

            Acceleration.Y += Gravity;

            if (Velocity.X > BaseVelocity.X) Velocity.X -= 0.2f;
            if (Velocity.X < -BaseVelocity.X) Velocity.X += 0.2f;


            Velocity.Y = Acceleration.Y;

            if (Velocity.Y > 10)
                Velocity.Y = 10;

            if (Acceleration.Y > 10)
                Acceleration.Y = 10;

            OldPosition.Y = Position.Y;

        }

        public override Vector2 GetPosForCamera()
        {
            return Position;
        }




        /** COLLISION V3 **/

        public override void LeftDisplacement(GameTime gameTime)
        {
            if (Velocity.X < 0) Position.X += Velocity.X;
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

            if (isOnSlope == TileV2.SlopeType.LeftDown && Velocity.X < 0)
                Position.Y += BaseVelocity.X;

            else if (isOnSlope == TileV2.SlopeType.RightDown && Velocity.X > 0)
                Position.Y += BaseVelocity.X + 1;

            UpdateHitbox();
        }

        public override void UpDisplacement(GameTime gameTime)
        {
            if (Velocity.Y < 0) Position.Y += (int)Velocity.Y;
            UpdateHitbox();
        }


        public override void LeftStaticCollisionAction()
        {
            if (isLeft)
            {
                Velocity.X *= -1.0f;
                isLeft = false;
            }
        }
        public override void RightStaticCollisionAction()
        {
            if (!isLeft)
            {
                Velocity.X *= -1.0f;
                isLeft = true;
            }
        }
        public override void DownStaticCollisionAction()
        {
            if (isLeft) Velocity.X = -BaseVelocity.X;
            if (!isLeft) Velocity.X = BaseVelocity.X;
        }
        public override void UpStaticCollisionAction()
        {

        }


        public override void LeftDynamicCollisionAction(MovingBlock block)
        {
            if (isLeft)
            {
                Velocity.X *= -1.0f;
                isLeft = false;
            }
        }
        public override void RightDynamicCollisionAction(MovingBlock block)
        {
            if (!isLeft)
            {
                Velocity.X *= -1.0f;
                isLeft = true;
            }
        }
        public override void DownDynamicCollisionAction(MovingBlock block)
        {

        }
        public override void UpDynamicCollisionAction(MovingBlock block)
        {

        }


        public override void UpdateHitbox()
        {

            switch (ID)
            {
                case 1:
                    hitbox = new Hitbox((int)Math.Round(Position.X), (int)Math.Round(Position.Y), 15, 28);
                    break;
                case 2:
                    hitbox = new Hitbox((int)Math.Round(Position.X), (int)Math.Round(Position.Y), 14, 28);
                    break;
                default:
                    hitbox = new Hitbox(0, 0, 0, 0);
                    break;
            }

        }


    }
}
