using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetworkEngine_5._0.Client;
using NetworkEngine_5._0.Server;
using Plateform_2D_v9.Core;
using Plateform_2D_v9.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateform_2D_v9.Enemy
{
    public class SwordMan : EnemyV3
    {

        private Animation Walk;
        private int time;

        public SwordMan(Vector2 Position) : base(Position)
        {

            ID = EnemyType.swordman;

            PV = 3;

            Walk = new Animation(Main.Enemy[(int)ID], 4, 1, 0.10f);
            Velocity = new Vector2(0.8f, 0);
            BaseVelocity = new Vector2(0.8f, 0);
            KnockBack = new Vector2(1, 2); // 2, 0
            Walk.Start();

        }

        public override void Update(GameTime gameTime)
        {

            Walk.Update(gameTime);

            PlayerV2 p = Handler.playersV2[1];

            for (int i = 2; i <= 4; i++)
            {

                if (Handler.playersV2.ContainsKey(i))
                {
                    if (Vector2.Distance(p.Position, Position) > Vector2.Distance(Handler.playersV2[i].Position, Position))
                        p = Handler.playersV2[i];
                }
                
            }

            if (Math.Abs(p.Position.X - Position.X) > 20)
            {
                if (p.Position.X < Position.X && Velocity.Y == 0)
                {
                    Velocity.X = -Math.Abs(Velocity.X);
                    isLeft = true;
                }

                else if (p.Position.X > Position.X && Velocity.Y == 0)
                {
                    Velocity.X = -(-Math.Abs(Velocity.X));
                    isLeft = false;
                }
            }

            

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


                Handler.actors.Add(new ItemV2(pos, id1, v1));
                Handler.actors.Add(new ItemV2(pos, id2, v2));
                Handler.actors.Add(new ItemV2(pos, id3, v3));
                Handler.actors.Add(new ItemV2(pos, id4, v4));

                if (NetPlay.IsMultiplaying)
                {

                    if (NetPlay.MyPlayerID() == 1)
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


        public override Rectangle GetAttackRectangle()
        {
            if (!isLeft)
                return new Rectangle((int)Position.X + 14, (int)Position.Y + 2, 8, 13);
            else
                return new Rectangle((int)Position.X - 8, (int)Position.Y + 2, 8, 13);
        }

        

        public override void UpdateHitbox()
        {
            hitbox = new Hitbox((int)Math.Round(Position.X), (int)Math.Round(Position.Y), 15, 28);
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

                Acceleration.Y = -KnockBack.Y;
                Velocity.X = KnockBack.X;

                Walk.Stop();

                //Velocity.X *= -1.0f;
                //isLeft = false;
            }
        }
        public override void RightStaticCollisionAction()
        {
            if (!isLeft)
            {

                Acceleration.Y = -KnockBack.Y;
                Velocity.X = -KnockBack.X;

                Walk.Stop();

                //Velocity.X *= -1.0f;
                //isLeft = true;
            }
        }
        public override void DownStaticCollisionAction()
        {
            if (isLeft) Velocity.X = -BaseVelocity.X;
            if (!isLeft) Velocity.X = BaseVelocity.X;
            Walk.Start();
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


    }

}
