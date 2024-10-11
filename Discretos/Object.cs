using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateform_2D_v9.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    /// <summary>
    /// Version 2 de LevelItem
    /// </summary>
    /// <param name="r1"></param>
    /// <param name="r2"></param>
    /// <returns></returns>
    public class Object : Actor
    {

        public bool open = false;

        public int ladderHeight;

        private int time = 0;

        private Animation FlagAnimation; /// CheckPoint
        private Animation BasicAnimation;

        private Vector2 NumAnimation = new Vector2(0f, 0f);
        private float numX = 0.1f;
        private float numY = 0.1f;

        private ObjectID objectID;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Position"></param>
        /// <param name="ID">ID de l'objet.</param>
        /// <param name="isLocked">Si ID = 4 (Porte)</param>
        /// <param name="numOfTriggerObject"></param>
        /// <param name="heightLadder">Si ID = 7 (Echelle)</param>
        public Object(Vector2 Position, ObjectID ID, bool isLocked = false, int numOfTriggerObject = -1, int ladderHeight = 0)
            : base(Position)
        {
            actorType = ActorType.Object;
            objectID = ID;
            this.ID = (int)ID;
            this.isLocked = isLocked;
            this.NumOfTriggerObject = numOfTriggerObject;
            this.Velocity = Vector2.Zero;
            this.ladderHeight = ladderHeight;

            NumAnimation.X = Util.NextFloat(-0.8f, 0.8f);

            if(ID == ObjectID.gold_key)
                NumAnimation.Y = Util.NextFloat(-3, 3);

            Init();

        }


        public void Init()
        {
            switch (objectID)
            {
                case ObjectID.coin:
                    BasicAnimation = new Animation(Main.Object[ID], 4, 1, 0.1f);
                    BasicAnimation.Start();
                    light = new Light(Position + new Vector2(GetRectangle().Width / 2, GetRectangle().Height / 2), 1f, 20f, Color.White);
                    LightManager.lights.Add(light);
                    break;

                case ObjectID.core:
                    BasicAnimation = new Animation(Main.Object[ID], 4, 1, 0.1f);
                    BasicAnimation.Start();
                    light = new Light(Position + new Vector2(GetRectangle().Width / 2, GetRectangle().Height / 2), 1f, 20f, new Color(255, 100, 100));
                    LightManager.lights.Add(light);
                    break;

                case ObjectID.checkPoint:
                    BasicAnimation = new Animation(Main.Object[ID], 4, 3, 0.1f, 1);
                    BasicAnimation.Start();
                    FlagAnimation = new Animation(Main.Object[ID], 4, 3, 0.1f, 2);
                    FlagAnimation.Start();
                    break;

                case ObjectID.wood_door:
                    hitbox.isEnabled = true;
                    break;

                case ObjectID.gold_key:
                    BasicAnimation = new Animation(Main.Object[ID], 18, 1, 0.05f);
                    BasicAnimation.Start();
                    break;

                case ObjectID.spring:
                    BasicAnimation = new Animation(Main.Object[ID], 8, 1, 0.05f);
                    BasicAnimation.Start();

                    break;

                case ObjectID.wood_ladder:
                    Handler.ladder.Add(this);
                    break;

                case ObjectID.torch:
                    light = new Light(Position + new Vector2(GetRectangle().Width / 2, GetRectangle().Height / 2 -2), 1f, 60f, Color.White); // 50f
                    LightManager.lights.Add(light);
                    BasicAnimation = new Animation(Main.Object[ID], 4, 1, 0.1f);
                    BasicAnimation.Start();
                    break;

            }
        }

        public override void Update(GameTime gameTime)
        {

            if(light != null)
            {

                if(ID == 2)
                {
                    //light.Radius = (float)(Math.Abs(Math.Cos(BasicAnimation.GetFrame() / 2) * 20 + 5));
                    light.Radius = (float)(Math.Cos(Math.Abs(NumAnimation.Y / 2)) * 20 + 15);
                    light.Position = new Vector2(GetRectangle().X + GetRectangle().Width / 2, GetRectangle().Y + GetRectangle().Height / 2) + NumAnimation;

                }
                else
                    light.Position = new Vector2(GetRectangle().X + GetRectangle().Width / 2, GetRectangle().Y + GetRectangle().Height / 2);

            }

            OldPosition.X = Position.X;
            OldPosition.Y = Position.Y;

            NumAnimation.Y += numX;
            NumAnimation.X += numY;

            if (NumAnimation.Y > 3 || NumAnimation.Y < -3)
                numX *= -1;

            if (NumAnimation.X > 0.8f || NumAnimation.X < -0.8f)
                numY *= -1;

            if (ID != 3 && ID != 4 && ID != 7)
                BasicAnimation.Update(gameTime);

            PlayerAndEnemyCollision();

            if(ID == 3)
            {
                if (CheckPointHited)
                {
                    if(FlagAnimation.GetFrame() != FlagAnimation.GetSourceRectangle().Count - 1)
                        FlagAnimation.Update(gameTime);
                    else
                    { FlagAnimation.Stop(); BasicAnimation.Update(gameTime); }
                }
            }

            if (ID == 5)
                Position += Velocity;

            if(objectID == ObjectID.torch)
            {
                light.Radius = 60 + Random.Shared.Next(-3, 3);
                //light.Color = Color.Orange;
            }
                


        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            DEBUG.DebugCollision(hitbox.rectangle, Color.DarkBlue, spriteBatch);

            if (ID != 5 || !isCollected)
                NumAnimation.X = 0;

            if (ID == 1)
                BasicAnimation.Draw(spriteBatch, Position);
            else if (ID == 2)
                BasicAnimation.Draw(spriteBatch, Position + NumAnimation);
            else if (ID == 3)
            {
                if (CheckPointHited)
                {
                    if (FlagAnimation.GetFrame() != FlagAnimation.GetSourceRectangle().Count - 1)
                        FlagAnimation.Draw(spriteBatch, Position + new Vector2(-2, 0));
                    else
                        BasicAnimation.Draw(spriteBatch, Position + new Vector2(-2, 0));
                }
                else
                    spriteBatch.Draw(Main.Object[ID], Position + new Vector2(-2, 0), new Rectangle(0, 64, 32, 32), Color.White);


            }
            else if (ID == 4)
                if (open)
                {
                    if (!isLocked)
                        spriteBatch.Draw(Main.Object[ID], Position + new Vector2(5, 0), new Rectangle(0, 0, 20, 32), Color.White);
                    else
                    {
                        spriteBatch.Draw(Main.Object[ID], Position + new Vector2(5, 0), new Rectangle(21, 0, 16, 32), Color.White);
                        spriteBatch.Draw(Main.Cadenas, Position + new Vector2(0, 10), new Rectangle(0, 0, 16, 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                }
                else
                {
                    spriteBatch.Draw(Main.Object[ID], Position + new Vector2(5, 0), new Rectangle(21, 0, 16, 32), Color.White);
                    if(hitbox.isEnabled) // isLocked && 
                        spriteBatch.Draw(Main.Cadenas, Position + new Vector2(0, 10), new Rectangle(0, 0, 16, 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
            else if (ID == 5)
                if (!isCollected)
                    BasicAnimation.Draw(spriteBatch, Position + NumAnimation);
                else
                    BasicAnimation.Draw(spriteBatch, Position + new Vector2(NumAnimation.X / 4, NumAnimation.Y));

            else if(ID == 6)
            {
                BasicAnimation.Draw(spriteBatch, Position);
                if (BasicAnimation.GetFrame() == BasicAnimation.GetSourceRectangle().Count - 1)
                    BasicAnimation.Stop();
            }
            else if(ID == 7)
            {
                for (int i = 1; i <= ladderHeight; i++)
                {
                    if(i == 1)
                        spriteBatch.Draw(Main.Object[ID], Position + new Vector2(2, -2), new Rectangle(0, 0, 18, 20), Color.White);
                    else if( i == ladderHeight)
                        spriteBatch.Draw(Main.Object[ID], Position + new Vector2(2, 16 * (i - 1) - 2), new Rectangle(36, 0, 18, 20), Color.White);
                    else
                        spriteBatch.Draw(Main.Object[ID], Position + new Vector2(2, 16 * (i - 1) - 2), new Rectangle(18, 0, 18, 20), Color.White);
                }
            }
            else if (ID == 9)
            {
                //spriteBatch.Draw(Main.Object[ID], Position, Color.White);
                BasicAnimation.Draw(spriteBatch, Position);
            }

        }

        public void PlayerAndEnemyCollision()
        {
            open = false;

            for (int i = 1; i <= Handler.playersV2.Count; i++)
            {
                Actor player = Handler.playersV2[i];

                if (GetRectangle().Intersects(player.GetRectangle()))
                    open = true;
                if (GetRectangle().Intersects(player.GetRectangle()))
                    if (ID == 6 && BasicAnimation.GetFrame() >= 6)
                    {
                        BasicAnimation.Reset();
                        BasicAnimation.Start();
                    }

            }

            for (int i = 0; i < Handler.actors.Count; i++)
            {
                Actor a = Handler.actors[i];

                if (a.actorType == ActorType.Enemy)
                {
                    if (GetRectangle().Intersects(a.GetRectangle()))
                        open = true;
                    if (GetRectangle().Intersects(a.GetRectangle()))
                        if (ID == 6 && BasicAnimation.GetFrame() >= 6)
                        {
                            BasicAnimation.Reset();
                            BasicAnimation.Start();
                        }
                }

                



            }

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

        public override void RemovePV(int PV)
        {
            throw new NotImplementedException();
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

        public override void UpdateHitbox()
        {

            switch (objectID)
            {
                case ObjectID.coin:
                    hitbox.rectangle = new Rectangle((int)Position.X + 3, (int)Position.Y + 1, 10, 14);
                    break;
                case ObjectID.core:
                    hitbox.rectangle = new Rectangle((int)Position.X + 1, (int)Position.Y, 13, 14);
                    break;
                case ObjectID.checkPoint:
                    hitbox.rectangle = new Rectangle((int)Position.X, (int)Position.Y, 16, 32);
                    break;
                case ObjectID.wood_door:
                    //if (isLocked)
                        hitbox.rectangle = new Rectangle((int)Position.X + 5, (int)Position.Y, 6, 32);
                    //else
                        //hitbox.rectangle = new Rectangle((int)Position.X - 5 + 5, (int)Position.Y, 24, 32);
                    break;
                case ObjectID.gold_key:
                    hitbox.rectangle = new Rectangle((int)Position.X + 5, (int)Position.Y, 7, 15);
                    break;
                case ObjectID.spring:
                    hitbox.rectangle = new Rectangle((int)Position.X + 1, (int)Position.Y + 6, 14, 10);
                    break;
                case ObjectID.wood_ladder:
                    hitbox.rectangle = new Rectangle((int)Position.X + 4, (int)Position.Y, 14, 16 * ladderHeight);
                    break;
                case ObjectID.wood_ladder_snow:
                    hitbox.rectangle = new Rectangle((int)Position.X + 4, (int)Position.Y, 14, 16 * ladderHeight);
                    break;
                case ObjectID.torch:
                    hitbox.rectangle = new Rectangle((int)Position.X + 3, (int)Position.Y, 10, 16);
                    break;
                default:
                    hitbox.rectangle = new Rectangle(0, 0, 0, 0);
                    break;
            }

        }


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
