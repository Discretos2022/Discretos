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

        public ObjectBounds Door;

        private int time = 0;

        private Animation FlagAnimation; /// CheckPoint
        private Animation BasicAnimation;

        private Vector2 NumAnimation = new Vector2(0f, 0f);
        private float numX = 0.1f;
        private float numY = 0.1f;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Position"></param>
        /// <param name="ID">ID de l'objet.</param>
        /// <param name="isLocked">Si ID = 4 (Porte)</param>
        /// <param name="numOfTriggerObject"></param>
        /// <param name="heightLadder">Si ID = 7 (Echelle)</param>
        public Object(Vector2 Position, int ID, bool isLocked = false, int numOfTriggerObject = -1, int ladderHeight = 0)
            : base(Position)
        {
            actorType = ActorType.Object;
            this.ID = ID;
            this.isLocked = isLocked;
            this.NumOfTriggerObject = numOfTriggerObject;
            this.Velocity = Vector2.Zero;
            this.ladderHeight = ladderHeight;

            NumAnimation.X = Util.NextFloat(-0.8f, 0.8f);

            if(ID == 5)
                NumAnimation.Y = Util.NextFloat(-3, 3);

            Init();

        }


        public void Init()
        {
            switch (ID)
            {
                case 1:
                    BasicAnimation = new Animation(Main.Object[ID], 4, 1, 0.1f);
                    BasicAnimation.Start();
                    light = new Light(Position + new Vector2(GetRectangle().Width / 2, GetRectangle().Height / 2), 1f, 20f, Color.White);
                    LightManager.lights.Add(light);
                    break;

                case 2:
                    BasicAnimation = new Animation(Main.Object[ID], 4, 1, 0.1f);
                    BasicAnimation.Start();
                    light = new Light(Position + new Vector2(GetRectangle().Width / 2, GetRectangle().Height / 2), 1f, 20f, new Color(255, 100, 100));
                    LightManager.lights.Add(light);
                    break;

                case 3:
                    BasicAnimation = new Animation(Main.Object[ID], 4, 3, 0.1f, 1);
                    BasicAnimation.Start();
                    FlagAnimation = new Animation(Main.Object[ID], 4, 3, 0.1f, 2);
                    FlagAnimation.Start();
                    break;

                case 4:
                    Door = new ObjectBounds(Position + new Vector2(6, 0), 1, 4, 32);
                    hitbox.isEnabled = true;
                    //Handler.solids.Add(Door);
                    break;

                case 5:
                    BasicAnimation = new Animation(Main.Object[ID], 18, 1, 0.05f);
                    BasicAnimation.Start();
                    break;

                case 6:
                    BasicAnimation = new Animation(Main.Object[ID], 8, 1, 0.05f);
                    BasicAnimation.Start();

                    break;

                case 7:
                    Handler.ladder.Add(this);
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

            this.OldPosition.X = Position.X;
            
            //Position.X += Velocity.X;
            //HorizontaleCollision();
            //HorizontaleCollision();

            this.OldPosition.Y = Position.Y;

            //Position.Y += Velocity.Y;
            //ApplyPhysic();
            //VerticaleCollision();

            NumAnimation.Y += numX;
            NumAnimation.X += numY;

            if (NumAnimation.Y > 3 || NumAnimation.Y < -3)
                numX *= -1;

            if (NumAnimation.X > 0.8f || NumAnimation.X < -0.8f)
                numY *= -1;

            if (ID != 3 && ID != 4 && ID != 7)
                BasicAnimation.Update(gameTime);

            PlayerAndEnemyCollision();

            //if (!isLocked)
                //Handler.solids.Remove(Door);

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


        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //spriteBatch.Draw(texture, Position, Color.White);

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

        }

        public void PlayerAndEnemyCollision()
        {
            open = false;

            for (int i = 1; i <= Handler.playersV2.Count; i++)
            {
                Actor player = Handler.playersV2[i];

                if (Collision.RectVsRect(GetRectangle(), player.GetRectangle()))
                    open = true;
                if (Collision.RectVsRect(GetRectangle(), player.GetRectangle()))
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
                    if (Collision.RectVsRect(GetRectangle(), a.GetRectangle()))
                        open = true;
                    if (Collision.RectVsRect(GetRectangle(), a.GetRectangle()))
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

            Position.Y += (int)Velocity.Y;

        }

        /*public void HorizontaleCollision()
        {
            for (int i = 0; i < Handler.solids.Count; i++)
            {

                Solid tile = Handler.solids[i];

                if (Main.SolidTile[Handler.solids[i].getType()]) //&& !Main.tiles[i].IsSlope())
                {

                    if (Collision.SolidVsActor(this, tile))
                    {

                        if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.X) == Collision.Direction.Left)
                        {
                            Position.X = Collision.SolidVsActorResolution(Collision.Direction.Left, this, tile).X;
                            Left = true;
                            Velocity.X = 0;
                        }

                        else if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.X) == Collision.Direction.Right)
                        {
                            Position.X = Collision.SolidVsActorResolution(Collision.Direction.Right, this, tile).X;
                            Right = true;
                            Velocity.X = 0;
                        }
                    }
                }
            }


            Vector2 Point1 = new Vector2(GetRectangle().X, GetRectangle().Y);
            Vector2 Point2 = new Vector2(GetRectangle().X + GetRectangle().Width - 0, GetRectangle().Y);
            Vector2 Point3 = new Vector2(GetRectangle().X, GetRectangle().Y + GetRectangle().Height - 1);

            int xMin = (int)Point1.X / 16;
            int xMax = (int)Point2.X / 16;

            int yMin = (int)Point1.Y / 16;
            int yMax = (int)Point3.Y / 16;

            if (Handler.Level != null)
            {

                if (xMin < 0)
                    xMin = 0;
                if (xMax > Handler.Level.GetLength(0))
                    xMax = Handler.Level.GetLength(0);
                if (yMin < 0)
                    yMin = 0;
                if (yMax >= Handler.Level.GetLength(1))
                    yMax = Handler.Level.GetLength(1) - 1;

                for (int j = yMin; j <= yMax; j++)
                {
                    for (int i = xMin; i <= xMax; i++)
                    {

                        Solid tile = Handler.Level[i, j];

                        if (Collision.SolidVsActor(this, tile) && Main.SolidTile[tile.getType()])
                        {

                            if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.X) == Collision.Direction.Left)
                            {
                                Position.X = Collision.SolidVsActorResolution(Collision.Direction.Left, this, tile).X;
                                Left = true;
                                //Console.WriteLine("Left");
                            }

                            if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.X) == Collision.Direction.Right)
                            {
                                Position.X = Collision.SolidVsActorResolution(Collision.Direction.Right, this, tile).X;
                                Right = true;
                                //Console.WriteLine("Right");
                            }

                        }
                    }
                }


            }
        }*/

        /*public void VerticaleCollision()
        {
            for (int i = 0; i < Handler.solids.Count; i++)
            {

                Solid tile = Handler.solids[i];

                if (Main.SolidTile[Handler.solids[i].getType()]) //&& !Main.tiles[i].IsSlope())
                {

                    if (Collision.SolidVsActor(this, tile))
                    {

                        if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.Y) == Collision.Direction.Up)
                        {

                            Position.Y = Collision.SolidVsActorResolution(Collision.Direction.Up, this, tile).Y;

                            Up = true;
                            Velocity.Y = 0;
                            Acceleration.Y = 0;

                        }

                        else if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.Y) == Collision.Direction.Down)
                        {
                            Position.Y = Collision.SolidVsActorResolution(Collision.Direction.Down, this, tile).Y;

                            Down = true;
                            Acceleration.Y = 0;
                            Velocity.Y = 0;
                            Velocity.X = 0;
                        }
                    }
                }
            }


            Vector2 Point1 = new Vector2(GetRectangle().X, GetRectangle().Y);
            Vector2 Point2 = new Vector2(GetRectangle().X + GetRectangle().Width, GetRectangle().Y);
            Vector2 Point3 = new Vector2(GetRectangle().X, GetRectangle().Y + GetRectangle().Height);

            int xMin = (int)Point1.X / 16;
            int xMax = (int)Point2.X / 16;

            int yMin = (int)Point1.Y / 16;
            int yMax = (int)Point3.Y / 16;

            if (Handler.Level != null)
            {

                if (xMin < 0)
                    xMin = 0;
                if (xMax > Handler.Level.GetLength(0))
                    xMax = Handler.Level.GetLength(0);
                if (yMin < 0)
                    yMin = 0;
                if (yMax >= Handler.Level.GetLength(1))
                    yMax = Handler.Level.GetLength(1) - 1;

                for (int j = yMin; j <= yMax; j++)
                {
                    for (int i = xMin; i <= xMax; i++)
                    {

                        Solid tile = Handler.Level[i, j];

                        if (Collision.SolidVsActor(this, tile) && Main.SolidTile[tile.getType()])
                        {


                            if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.Y) == Collision.Direction.Up)
                            {

                                Console.WriteLine("Up");


                                Position.Y = Collision.SolidVsActorResolution(Collision.Direction.Up, this, tile).Y;

                                Up = true;
                                Velocity.Y = 0;
                                Acceleration.Y = 0;

                                //SetRidingTile(null);



                            }

                            else if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.Y) == Collision.Direction.Down)
                            {
                                Position.Y = Collision.SolidVsActorResolution(Collision.Direction.Down, this, tile).Y;

                                Down = true;
                                Acceleration.Y = 0;
                                Velocity.Y = 0;
                                Velocity.X = 0;

                                //Console.WriteLine("Down");
                            }
                        }


                    }
                }


            }



        }*/

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
            switch (ID)
            {
                case 1:
                    return new Rectangle((int)Position.X + 3, (int)Position.Y + 1, 10, 14);
                case 2:
                    return new Rectangle((int)Position.X + 1, (int)Position.Y, 13, 14);
                case 3:
                    return new Rectangle((int)Position.X, (int)Position.Y, 16, 32);
                case 4:
                    if(isLocked)
                        return new Rectangle((int)Position.X + 5, (int)Position.Y, 6, 32);
                    else
                        return new Rectangle((int)Position.X - 5 + 5, (int)Position.Y, 24, 32);
                case 5:
                    return new Rectangle((int)Position.X + 5, (int)Position.Y, 7, 15);
                case 6:
                    return new Rectangle((int)Position.X + 1, (int)Position.Y + 6, 14, 10);
                case 7:
                    return new Rectangle((int)Position.X + 4, (int)Position.Y, 14, 16 * ladderHeight);
                default:
                    return Rectangle.Empty;
            }
        }

        public override bool HasLowerState()
        {
            throw new NotImplementedException();
        }

        public override bool IsLower()
        {
            throw new NotImplementedException();
        }

        public override bool IsRiding()
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

        public override void SetRidingTile(TileV2 tile)
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

            switch (ID)
            {
                case 1:
                    hitbox.rectangle = new Rectangle((int)Position.X + 3, (int)Position.Y + 1, 10, 14);
                    break;
                case 2:
                    hitbox.rectangle = new Rectangle((int)Position.X + 1, (int)Position.Y, 13, 14);
                    break;
                case 3:
                    hitbox.rectangle = new Rectangle((int)Position.X, (int)Position.Y, 16, 32);
                    break;
                case 4:
                    //if (isLocked)
                        hitbox.rectangle = new Rectangle((int)Position.X + 5, (int)Position.Y, 6, 32);
                    //else
                        //hitbox.rectangle = new Rectangle((int)Position.X - 5 + 5, (int)Position.Y, 24, 32);
                    break;
                case 5:
                    hitbox.rectangle = new Rectangle((int)Position.X + 5, (int)Position.Y, 7, 15);
                    break;
                case 6:
                    hitbox.rectangle = new Rectangle((int)Position.X + 1, (int)Position.Y + 6, 14, 10);
                    break;
                case 7:
                    hitbox.rectangle = new Rectangle((int)Position.X + 4, (int)Position.Y, 14, 16 * ladderHeight);
                    break;
                default:
                    hitbox.rectangle = new Rectangle(0, 0, 0, 0);
                    break;
            }

        }

    }
}
