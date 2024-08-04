using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        //private Rectangle rectangle;
        private Rectangle Attackrectangle;

        private Animation Walk;

        public bool Right = false;
        public bool Left = false;
        public bool Up = false;
        public bool Down = false;

        private bool isLeft;

        private TileV2 TileRided;

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
            //Position.X += Velocity.X;
            //Position.X += Wind.X;

            /*if (Velocity.X < 0)
                Position.X += Acceleration.X;
            if(Velocity.X > 0)
                Position.X -= Acceleration.X;*/

            //if (TileRided != null)
            //{ Position.X += TileRided.GetVelocity().X; }

            //HorizontaleCollision();
            //HorizontaleCollision();

            /*if (PV <= 0 || IsSquish())
            {
                Handler.actors.Remove(this);

                Handler.actors.Add(new ItemV2(new Vector2(Position.X, Position.Y), (int)Util.random.Next(1, 7), new Vector2((float)-Util.random.NextDouble(), (float)Util.random.Next(-2, 0))));
                Handler.actors.Add(new ItemV2(new Vector2(Position.X, Position.Y), (int)Util.random.Next(1, 7), new Vector2((float)-Util.random.NextDouble(), (float)Util.random.Next(-2, 0))));
                Handler.actors.Add(new ItemV2(new Vector2(Position.X, Position.Y), (int)Util.random.Next(1, 7), new Vector2((float)Util.random.NextDouble(), (float)Util.random.Next(-2, 0))));
                Handler.actors.Add(new ItemV2(new Vector2(Position.X, Position.Y), (int)Util.random.Next(1, 7), new Vector2((float)Util.random.NextDouble(), (float)Util.random.Next(-2, 0))));

            }*/


            OldPosition.Y = Position.Y;
            //Position.Y += Velocity.Y;

            //if (TileRided != null)
                //Position.Y += TileRided.GetVelocity().Y;

            if (time > 0)
            {
                time--;
            }

            if (time <= 0)
                Hited = false;

            ActorCollision();

            ApplyPhysic();
            //DetectOnSolid();
            //VerticaleCollision();

            if ((int)Velocity.Y > 0 && !isOnSlope)
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
                        NetworkEngine_5._0.Server.ServerSender.SendCreatedItem(pos.X, pos.Y, id1, v1.X, v1.Y);
                        NetworkEngine_5._0.Server.ServerSender.SendCreatedItem(pos.X, pos.Y, id2, v2.X, v2.Y);
                        NetworkEngine_5._0.Server.ServerSender.SendCreatedItem(pos.X, pos.Y, id3, v3.X, v3.Y);
                        NetworkEngine_5._0.Server.ServerSender.SendCreatedItem(pos.X, pos.Y, id4, v4.X, v4.Y);
                    }
                    else
                    {
                        NetworkEngine_5._0.Client.ClientSender.SendCreatedItem(pos.X, pos.Y, id1, v1.X, v1.Y);
                        NetworkEngine_5._0.Client.ClientSender.SendCreatedItem(pos.X, pos.Y, id2, v2.X, v2.Y);
                        NetworkEngine_5._0.Client.ClientSender.SendCreatedItem(pos.X, pos.Y, id3, v3.X, v3.Y);
                        NetworkEngine_5._0.Client.ClientSender.SendCreatedItem(pos.X, pos.Y, id4, v4.X, v4.Y);
                    }

                }

                //Handler.actors.Remove(this);

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


            /*if (Main.Debug)
            {
                Writer.DrawText(Main.UltimateFont, "Velocity " + Velocity.X, Position + new Vector2(20, 0), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);

            }*/



        }

        public void InitEnemy()
        {
            switch (ID)
            {
                case 1:
                    //rectangle = new Rectangle((int)Position.X, (int)Position.Y, 14, 28);
                    Walk = new Animation(Main.Enemy[ID], 4, 1, 0.15f);
                    this.Velocity = new Vector2(0.6f, 0);
                    this.KnockBack = new Vector2(3, 2);
                    Walk.Start();
                    break;

                case 2:
                    //rectangle = new Rectangle((int)Position.X, (int)Position.Y, 14, 28);
                    Walk = new Animation(Main.Enemy[ID], 4, 1, 0.10f);
                    this.Velocity = new Vector2(0.8f, 0);
                    this.KnockBack = new Vector2(2, 0);
                    Walk.Start();
                    break;

                default:
                    //rectangle = new Rectangle(0, 0, 0, 0);
                    break;
            }
        }

        public override Rectangle GetRectangle()
        {
            switch (ID)
            {
                case 1:
                    return new Rectangle((int)Position.X, (int)Position.Y, 15, 28);
                case 2:
                    return new Rectangle((int)Position.X, (int)Position.Y, 14, 28);
                default:
                    return new Rectangle(0, 0, 0, 0);
            }
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
            if (Left && Right)
                goto L_1;
            else if (Up && Down)
                goto L_1;

            Left = false;
            Right = false;
            Up = false;
            Down = false;

            return false;

        L_1:
            {
                Left = false; Right = false; Up = false; Down = false;
                return true;
            }
        }


        public void DetectOnSolid()
        {
            for (int i = 0; i < Handler.solids.Count; i++)
            {
                TileV2 tile = Handler.solids[i];

                if (tile.blockType == TileV2.BlockType.block) //&& !Main.tiles[i].IsSlope())
                {

                    if (Collision.RectVsRect(new Rectangle(GetRectangle().X, GetRectangle().Y + GetRectangle().Height + 1, GetRectangle().Width, 1), tile.GetRectangle()))
                    {
                        SetRidingTile(tile);
                        goto L_2;
                    }
                    else
                        SetRidingTile(null);



                }
            }

        L_2:;

        }


        public override void SetRidingTile(TileV2 tile)
        {
            TileRided = tile;
        }


        public override bool IsRiding()
        {
            throw new NotImplementedException();
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
                        Acceleration.X = KnockBack.X;


                        if (hitbox.rectangle.X <= actor.hitbox.rectangle.X && isLeft)
                        {
                            Velocity.X *= -1;
                            isLeft = false;
                        }

                        if (hitbox.rectangle.X >= actor.hitbox.rectangle.X && !isLeft)
                        {
                            Velocity.X *= -1;
                            isLeft = true;
                        }
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

            Acceleration.X -= 0.2f;
            Acceleration.Y += Gravity;


            Velocity.Y = Acceleration.Y;

            if (Velocity.Y > 10)
                Velocity.Y = 10;

            if (Acceleration.Y > 10)
                Acceleration.Y = 10;

            if (Acceleration.X < 0)
                Acceleration.X = 0;

            OldPosition.Y = Position.Y;

            //Position.Y += (int)Velocity.Y;

        }


        /*public void HorizontaleCollision()
        {

            /// Moving Block
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
                            Velocity.X *= -1;
                            isLeft = false;
                        }

                        else if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.X) == Collision.Direction.Right)
                        {
                            Position.X = Collision.SolidVsActorResolution(Collision.Direction.Right, this, tile).X;
                            Right = true;
                            Velocity.X *= -1;
                            isLeft = true;
                        }
                    }
                }
            }

            /// Static Block
            Vector2 Point1 = new Vector2(GetRectangle().X, GetRectangle().Y);
            Vector2 Point2 = new Vector2(GetRectangle().X + GetRectangle().Width - 0, GetRectangle().Y);
            Vector2 Point3 = new Vector2(GetRectangle().X, GetRectangle().Y + GetRectangle().Height - 0);

            int xMin = (int)Point1.X / 16;
            int xMax = (int)Point2.X / 16;

            int yMin = (int)Point1.Y / 16;
            int yMax = (int)Point3.Y / 16;

            if (Handler.Level != null)
            {

                if (xMin < 0)
                    xMin = 0;
                if (xMax > Handler.Level.GetLength(0) - 1)
                    xMax = Handler.Level.GetLength(0) - 1;
                if (yMin < 0)
                    yMin = 0;
                if (yMax >= Handler.Level.GetLength(1))
                    yMax = Handler.Level.GetLength(1) - 1;

                for (int j = yMin; j <= yMax; j++)
                {
                    for (int i = xMin; i <= xMax; i++)
                    {

                        Solid tile = Handler.Level[i, j];

                        int index = i - 1;
                        if (index < 0)
                            index = 0;

                        if (Collision.SolidVsActor(this, tile) && Main.SolidTile[tile.getType()] && !tile.isSlope && !Handler.Level[index, j].isSlope)
                        {

                            if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.X) == Collision.Direction.Left)
                            {
                                Position.X = Collision.SolidVsActorResolution(Collision.Direction.Left, this, tile).X;
                                Left = true;
                                Velocity.X *= -1;
                                isLeft = false;
                            }

                            if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.X) == Collision.Direction.Right)
                            {
                                Position.X = Collision.SolidVsActorResolution(Collision.Direction.Right, this, tile).X;
                                Right = true;
                                Velocity.X *= -1;
                                isLeft = true;
                            }

                        }
                    }
                }

            }

        }*/

        /*public void VerticaleCollision()
        {

            isOnSlope = false;

            /// Moving Block
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
                            isOnGround = true;

                        }
                    }
                }
            }

            /// Static Block
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
                if (xMax > Handler.Level.GetLength(0) - 1)
                    xMax = Handler.Level.GetLength(0) - 1;
                if (yMin < 0)
                    yMin = 0;
                if (yMax >= Handler.Level.GetLength(1))
                    yMax = Handler.Level.GetLength(1) - 1;

                float result = 0;

                for (int j = yMin; j <= yMax; j++)
                {
                    for (int i = xMin; i <= xMax; i++)
                    {

                        Solid tile = Handler.Level[i, j];

                        /// NoSlope
                        if (Collision.SolidVsActor(this, tile) && Main.SolidTile[tile.getType()] && !tile.isSlope)
                        {


                            if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.Y) == Collision.Direction.Up)
                            {

                                //Console.WriteLine("Up");


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
                                //isJump = false;
                                isOnGround = true;

                                /// Break Block if Player is on and is on solid block in the left
                                if (Position.X > tile.Position.X && Handler.Level[xMax, yMax].isBreakable && !tile.isBreakable)
                                    Handler.Level[xMax, yMax].Break();

                                //Console.WriteLine("Down");
                            }
                        }

                        /// Slope
                        if (tile.isSlope && Collision.TriangleSolidVsActor(this, tile) && Main.SolidTile[tile.getType()])
                        {

                            /// Down Slope
                            if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightDown || (Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftDown)
                            {

                                //Console.WriteLine("Collision Slope");

                                Down = true;
                                Acceleration.Y = 2;
                                Velocity.Y = 0;
                                //isJump = false;
                                isOnGround = true;
                                isOnSlope = true;

                                if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftDown && Velocity.X > 0)
                                    Acceleration.Y = 0;
                                else if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftDown && Velocity.X < 0)
                                    Acceleration.Y = 2;

                                if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightDown && Velocity.X > 0)
                                    Acceleration.Y = 2;
                                else if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightDown && Velocity.X < 0)
                                    Acceleration.Y = 0;

                                if (Handler.Level[i, j + 1].GetRectangle().Y - Collision.TriangleSolidVsActorResolution(this, tile).Y - GetRectangle().Height <= 1)
                                    Acceleration.Y = 0;

                                Position.Y = Collision.TriangleSolidVsActorResolution(this, tile).Y;

                            }

                            /// Up Slope
                            if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightUp || (Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftUp)
                            {

                                Position.Y = Collision.TriangleSolidVsActorResolution(this, tile).Y;

                                Up = true;
                                Velocity.Y = 0;
                                Acceleration.Y = 2;

                            }


                        }

                        /// Platform
                        if (Collision.SolidVsActor(this, tile) && Main.SolidTileTop[tile.getType()] && !tile.isSlope)
                        {

                            if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.Y) == Collision.Direction.Down && OldPosition.Y + GetRectangle().Height <= tile.GetRectangle().Y)
                            {


                                if (!tile.isBreakable)
                                    Position.Y = Collision.SolidVsActorResolution(Collision.Direction.Down, this, tile).Y;
                                else
                                    result = Collision.SolidVsActorResolution(Collision.Direction.Down, this, tile).Y;

                                Down = true;
                                Acceleration.Y = 0;
                                Velocity.Y = 0;
                                //isJump = false;
                                isOnGround = true;

                                if (tile.isBreakable)
                                    tile.Break();


                            }

                        }


                    }
                }

                if (result != 0)
                    Position.Y = result;

            }


        }*/


        public override Vector2 GetPosForCamera()
        {
            return Position;
        }






        /** COLLISION V3 **/

        public override void LeftDisplacement(GameTime gameTime)
        {
            if (Velocity.X < 0) Position.X += Velocity.X;
            if (Wind.X < 0) Position.X += Wind.X;
            if (Velocity.X > 0 && Acceleration.X > 0) Position.X -= Acceleration.X;
            UpdateHitbox();
        }

        public override void RightDisplacement(GameTime gameTime)
        {
            if (Velocity.X > 0) Position.X += Velocity.X;
            if (Wind.X > 0) Position.X += Wind.X;
            if (Velocity.X < 0 && Acceleration.X > 0) Position.X += Acceleration.X;
            UpdateHitbox();
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

        public override void LeftStaticCollision()
        {

            /// Static Block
            Vector2 Point1 = new Vector2(GetRectangle().X, GetRectangle().Y);
            Vector2 Point2 = new Vector2(GetRectangle().X + GetRectangle().Width - 0, GetRectangle().Y);
            Vector2 Point3 = new Vector2(GetRectangle().X, GetRectangle().Y + GetRectangle().Height - 0);

            int xMin = (int)Point1.X / 16;
            int xMax = (int)Point2.X / 16;

            int yMin = (int)Point1.Y / 16;
            int yMax = (int)Point3.Y / 16;

            if (Handler.Level != null)
            {

                if (xMin < 0)
                    xMin = 0;
                if (xMax > Handler.Level.GetLength(0) - 1)
                    xMax = Handler.Level.GetLength(0) - 1;
                if (yMin < 0)
                    yMin = 0;
                if (yMax >= Handler.Level.GetLength(1))
                    yMax = Handler.Level.GetLength(1) - 1;

                for (int j = yMin; j <= yMax; j++)
                {
                    for (int i = xMin; i <= xMax; i++)
                    {

                        TileV2 tile = Handler.Level[i, j];

                        int index = i - 1;
                        if (index < 0)
                            index = 0;

                        if (tile.hitbox.isEnabled && hitbox.rectangle.Intersects(tile.GetRectangle()) && tile.blockType == TileV2.BlockType.block && !tile.isSlope && !Handler.Level[index, j].isSlope)
                        {

                            Position.X = tile.Position.X + tile.hitbox.rectangle.Width + 1;
                            Left = true;

                            if (isLeft)
                            {
                                Velocity.X *= -1.0f;
                                isLeft = false;
                            }
                            
                            UpdateHitbox();

                        }
                    }
                }

            }

        }

        public override void RightStaticCollision()
        {

            /// Static Block
            Vector2 Point1 = new Vector2(GetRectangle().X, GetRectangle().Y);
            Vector2 Point2 = new Vector2(GetRectangle().X + GetRectangle().Width - 0, GetRectangle().Y);
            Vector2 Point3 = new Vector2(GetRectangle().X, GetRectangle().Y + GetRectangle().Height - 0);

            int xMin = (int)Point1.X / 16;
            int xMax = (int)Point2.X / 16;

            int yMin = (int)Point1.Y / 16;
            int yMax = (int)Point3.Y / 16;

            if (Handler.Level != null)
            {

                if (xMin < 0)
                    xMin = 0;
                if (xMax > Handler.Level.GetLength(0) - 1)
                    xMax = Handler.Level.GetLength(0) - 1;
                if (yMin < 0)
                    yMin = 0;
                if (yMax >= Handler.Level.GetLength(1))
                    yMax = Handler.Level.GetLength(1) - 1;

                for (int j = yMin; j <= yMax; j++)
                {
                    for (int i = xMin; i <= xMax; i++)
                    {

                        TileV2 tile = Handler.Level[i, j];

                        int index = i - 1;
                        if (index < 0)
                            index = 0;

                        if (tile.hitbox.isEnabled && hitbox.rectangle.Intersects(tile.hitbox.rectangle) && tile.blockType == TileV2.BlockType.block && !tile.isSlope && !Handler.Level[index, j].isSlope)
                        {

                            Position.X = tile.Position.X - hitbox.rectangle.Width - 1;
                            Left = true;

                            if (!isLeft)
                            {
                                Velocity.X *= -1.0f;
                                isLeft = true;
                            }
                            
                            UpdateHitbox();

                        }
                    }
                }

            }

        }

        public override void DownStaticCollision()
        {

            /// Static Block
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
                if (xMax > Handler.Level.GetLength(0) - 1)
                    xMax = Handler.Level.GetLength(0) - 1;
                if (yMin < 0)
                    yMin = 0;
                if (yMax >= Handler.Level.GetLength(1))
                    yMax = Handler.Level.GetLength(1) - 1;

                float result = 0;

                for (int j = yMin; j <= yMax; j++)
                {
                    for (int i = xMin; i <= xMax; i++)
                    {

                        TileV2 tile = Handler.Level[i, j];

                        /// NoSlope
                        if (tile.hitbox.isEnabled && hitbox.rectangle.Intersects(tile.GetRectangle()) && tile.blockType == TileV2.BlockType.block && !tile.isSlope)
                        {
                            
                            Position.Y = tile.Position.Y - hitbox.rectangle.Height;

                            Down = true;
                            Acceleration.Y = 0;
                            Velocity.Y = 0;
                            //isJump = false;
                            isOnGround = true;

                            /// Break Block if Player is on and is on solid block in the left
                            if (Position.X > tile.Position.X && Handler.Level[xMax, yMax].isBreakable && !tile.isBreakable)
                                Handler.Level[xMax, yMax].Break();

                            //Console.WriteLine("Down");

                        }

                        /// Slope
                        /*if (tile.isSlope && Collision.TriangleSolidVsActor(this, tile) && Main.SolidTile[tile.getType()])
                        {

                            /// Down Slope
                            if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightDown || (Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftDown)
                            {

                                //Console.WriteLine("Collision Slope");

                                Down = true;
                                Acceleration.Y = 2;
                                Velocity.Y = 0;
                                //isJump = false;
                                isOnGround = true;
                                isOnSlope = true;

                                if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftDown && Velocity.X > 0)
                                    Acceleration.Y = 0;
                                else if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftDown && Velocity.X < 0)
                                    Acceleration.Y = 2;

                                if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightDown && Velocity.X > 0)
                                    Acceleration.Y = 2;
                                else if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightDown && Velocity.X < 0)
                                    Acceleration.Y = 0;

                                if (Handler.Level[i, j + 1].GetRectangle().Y - Collision.TriangleSolidVsActorResolution(this, tile).Y - GetRectangle().Height <= 1)
                                    Acceleration.Y = 0;

                                Position.Y = Collision.TriangleSolidVsActorResolution(this, tile).Y;

                            }

                            /// Up Slope
                            if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightUp || (Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftUp)
                            {

                                Position.Y = Collision.TriangleSolidVsActorResolution(this, tile).Y;

                                Up = true;
                                Velocity.Y = 0;
                                Acceleration.Y = 2;

                            }


                        }*/

                        /// Platform
                        if (tile.hitbox.isEnabled && hitbox.rectangle.Intersects(tile.GetRectangle()) && tile.blockType == TileV2.BlockType.platform && !tile.isSlope)
                        {

                            if (OldPosition.Y + GetRectangle().Height <= tile.GetRectangle().Y)
                            {


                                Position.Y = tile.Position.Y - hitbox.rectangle.Height;

                                Down = true;
                                Acceleration.Y = 0;
                                Velocity.Y = 0;
                                //isJump = false;
                                isOnGround = true;

                                if (tile.isBreakable)
                                    tile.Break();


                            }

                        }


                    }
                }

                if (result != 0)
                    Position.Y = result;

            }

        }

        public override void UpStaticCollision()
        {

            /// Static Block
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
                if (xMax > Handler.Level.GetLength(0) - 1)
                    xMax = Handler.Level.GetLength(0) - 1;
                if (yMin < 0)
                    yMin = 0;
                if (yMax >= Handler.Level.GetLength(1))
                    yMax = Handler.Level.GetLength(1) - 1;

                float result = 0;

                for (int j = yMin; j <= yMax; j++)
                {
                    for (int i = xMin; i <= xMax; i++)
                    {

                        TileV2 tile = Handler.Level[i, j];

                        /// NoSlope
                        if (tile.hitbox.isEnabled && hitbox.rectangle.Intersects(tile.GetRectangle()) && tile.blockType == TileV2.BlockType.block && !tile.isSlope)
                        {

                            //Console.WriteLine("Up");


                            Position.Y = tile.Position.Y + tile.GetRectangle().Height;

                            Up = true;
                            Velocity.Y = 0;
                            Acceleration.Y = 0;

                            //SetRidingTile(null);

                        }

                        /// Slope
                        /*if (tile.isSlope && Collision.TriangleSolidVsActor(this, tile) && Main.SolidTile[tile.getType()])
                        {

                            /// Down Slope
                            if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightDown || (Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftDown)
                            {

                                //Console.WriteLine("Collision Slope");

                                Down = true;
                                Acceleration.Y = 2;
                                Velocity.Y = 0;
                                //isJump = false;
                                isOnGround = true;
                                isOnSlope = true;

                                if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftDown && Velocity.X > 0)
                                    Acceleration.Y = 0;
                                else if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftDown && Velocity.X < 0)
                                    Acceleration.Y = 2;

                                if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightDown && Velocity.X > 0)
                                    Acceleration.Y = 2;
                                else if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightDown && Velocity.X < 0)
                                    Acceleration.Y = 0;

                                if (Handler.Level[i, j + 1].GetRectangle().Y - Collision.TriangleSolidVsActorResolution(this, tile).Y - GetRectangle().Height <= 1)
                                    Acceleration.Y = 0;

                                Position.Y = Collision.TriangleSolidVsActorResolution(this, tile).Y;

                            }

                            /// Up Slope
                            if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightUp || (Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftUp)
                            {

                                Position.Y = Collision.TriangleSolidVsActorResolution(this, tile).Y;

                                Up = true;
                                Velocity.Y = 0;
                                Acceleration.Y = 2;

                            }


                        }*/

                    }
                }

                if (result != 0)
                    Position.Y = result;

            }

        }

        public override void UpdateHitbox()
        {

            switch (ID)
            {
                case 1:
                    hitbox = new Hitbox((int)Position.X, (int)Position.Y, 15, 28);
                    break;
                case 2:
                    hitbox = new Hitbox((int)Position.X, (int)Position.Y, 14, 28);
                    break;
                default:
                    hitbox = new Hitbox(0, 0, 0, 0);
                    break;
            }

        }


    }
}
