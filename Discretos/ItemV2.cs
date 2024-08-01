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

        public bool Right = false;
        public bool Left = false;
        public bool Up = false;
        public bool Down = false;

        private int time = 0;

        private TileV2 TileRided;

        private Texture2D texture;


        public ItemV2(Vector2 Position, int ID, Vector2 Acceleration)
            : base(Position)
        {
            actorType = ActorType.Item;
            this.ID = ID;

            this.Acceleration = Acceleration;
            Velocity.X = Acceleration.X;

            texture = Main.SpriteSheetItem[ID];

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
            //Position.X += Velocity.X;

            //if (TileRided != null)
                //Position.X += TileRided.GetVelocity().X;

            //Position.X += Wind.X;

            //HorizontaleCollision();
            //HorizontaleCollision();

            this.OldPosition.Y = Position.Y;

            if (TileRided != null)
                Position.Y += TileRided.GetVelocity().Y;

            Position.Y += Wind.Y;

            ApplyPhysic();
            DetectOnSolid();
            //VerticaleCollision();

            if ((int)Velocity.Y > 0 && !isOnSlope)
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

            //Position.Y += (int)Velocity.Y;

        }

        /*public void HorizontaleCollision()
        {

            ///Moving Block
            for (int i = 0; i < Handler.solids.Count; i++)
            {

                TileV2 tile = Handler.solids[i];

                if (tile.blockType == TileV2.BlockType.block) //&& !Main.tiles[i].IsSlope())
                {

                    if (Collision.SolidVsActor(this, tile))
                    {

                        if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.X) == Collision.Direction.Left)
                        {
                            Position.X = Collision.SolidVsActorResolution(Collision.Direction.Left, this, tile).X;
                            Left = true;
                            Velocity.X *= -tile.Velocity.X; 
                        }

                        else if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.X) == Collision.Direction.Right)
                        {
                            Position.X = Collision.SolidVsActorResolution(Collision.Direction.Right, this, tile).X;
                            Right = true;
                            Velocity.X *= -tile.Velocity.X;
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
                                Velocity.X *= -0.5f;
                                //Console.WriteLine("Left");
                            }

                            if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.X) == Collision.Direction.Right)
                            {
                                Position.X = Collision.SolidVsActorResolution(Collision.Direction.Right, this, tile).X;
                                Right = true;
                                Velocity.X *= -0.5f;
                                //Console.WriteLine("Right");
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
                            Velocity.X = 0;
                            isOnGround = true;

                        }
                    }
                }
            }

            /// Static Block
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
                                Velocity.X = 0;
                                isOnGround = true;

                                //Console.WriteLine("Down");
                            }
                        }

                        ///Slope
                        if (tile.isSlope && Collision.TriangleSolidVsActor(this, tile) && Main.SolidTile[tile.getType()])
                        {

                            /// Down Slope
                            if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightDown || (Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftDown)
                            {

                                //Console.WriteLine("Collision Slope");

                                Down = true;
                                Acceleration.Y = 2;
                                Velocity.Y = 0;
                                Velocity.X = 0;
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
                            ///          ///Provisoir///           ///
                            if (Collision.SolidVsActorDirection(this, tile, Collision.Direction.Y) == Collision.Direction.Down && OldPosition.Y + GetRectangle().Height <= tile.GetRectangle().Y)
                            {


                                if (!tile.isBreakable)
                                    Position.Y = Collision.SolidVsActorResolution(Collision.Direction.Down, this, tile).Y;
                                else
                                    result = Collision.SolidVsActorResolution(Collision.Direction.Down, this, tile).Y;

                                Down = true;
                                Acceleration.Y = 0;
                                Velocity.Y = 0;
                                Velocity.X = 0;
                                isOnGround = true;

                                if (tile.isBreakable && ID == 1)
                                    tile.Break();


                            }

                        }

                    }
                }

                if (result != 0)
                    Position.Y = result;

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
            return new Rectangle((int)Position.X, (int)Position.Y, 7, 5); // 7 5
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

                //Console.WriteLine(Left + " ; " + Right + "  :  " + Down + " ; " + Up);

                Left = false;
                Right = false;
                Up = false;
                Down = false;

                return true;
            }
        }

        public override void RemovePV(int PV)
        {
            throw new NotImplementedException();
        }

        public override void SetRidingTile(TileV2 tile)
        {
            TileRided = tile;
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



        /** COLLISION V3 **/

        public override void LeftDisplacement(GameTime gameTime)
        {
            if(Velocity.X < 0) Position.X += Velocity.X;
            if (Wind.X < 0) Position.X += Wind.X;
            UpdateHitbox();
        }

        public override void RightDisplacement(GameTime gameTime)
        {
            if (Velocity.X > 0) Position.X += Velocity.X;
            if (Wind.X > 0) Position.X += Wind.X;
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

        public override void LeftCollision()
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

                            Position.X = tile.Position.X + tile.hitbox.rectangle.Width;
                            Left = true;
                            Velocity.X *= -0.5f;
                            //Console.WriteLine("Left");

                        }
                    }
                }


            }

        }

        public override void RightCollision()
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

                            Position.X = tile.Position.X - hitbox.rectangle.Width;
                            Left = true;
                            Velocity.X *= -0.5f;
                            //Console.WriteLine("Right");

                        }
                    }
                }


            }

        }

        public override void DownCollision()
        {

            UpdateHitbox();

            isOnSlope = false;

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

                        if (tile == null)                    /////////////////// provisoir
                            break;

                        /// NoSlope
                        if (tile.hitbox.isEnabled && hitbox.rectangle.Intersects(tile.GetRectangle()) && tile.blockType == TileV2.BlockType.block && !tile.isSlope)
                        {

                            Position.Y = tile.Position.Y - hitbox.rectangle.Height;

                            Acceleration.Y = 0;
                            Velocity.Y = 0;
                            Velocity.X = 0;
                            isOnGround = true;

                            /// Break Block if Player is on and is on solid block in the left
                            if (Position.X > tile.Position.X && Handler.Level[xMax, yMax].isBreakable && !tile.isBreakable)
                                Handler.Level[xMax, yMax].Break();

                            //Console.WriteLine(tile.Position.X/16);

                            //Console.WriteLine("Down");

                        }

                        /// Slope
                        /*if (tile.isSlope && Collision.TriangleSolidVsActor(this, tile) && Main.SolidTile[(int)tile.ID])
                        {
                            /// Down Slope
                            if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightDown || (Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftDown)
                            {
                                //Console.WriteLine("Collision Slope");

                                if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightDown)
                                    PosOfDebug = Collision.TriangleSolidVsActorResolution(this, tile) + new Vector2(0, GetRectangle().Height);

                                if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftDown)
                                    PosOfDebug = Collision.TriangleSolidVsActorResolution(this, tile) + new Vector2(GetRectangle().Width - 1, GetRectangle().Height);

                                //Console.WriteLine(Collision.TriangleSolidVsActorResolution(this, tile));


                                Down = true;
                                Acceleration.Y = 2;
                                Velocity.Y = 0;
                                isJump = false;
                                isOnGround = true;
                                isOnSlope = true;

                                if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftDown && goRight)
                                    Acceleration.Y = 0;
                                else if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftDown && goLeft)
                                    Acceleration.Y = 3;

                                if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightDown && goRight)
                                    Acceleration.Y = 3;
                                else if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightDown && goLeft)
                                    Acceleration.Y = 0;

                                if (Handler.Level[i, j + 1].GetRectangle().Y - Collision.TriangleSolidVsActorResolution(this, tile).Y - GetRectangle().Height <= 1)
                                    Acceleration.Y = 0;

                                //Console.WriteLine(Handler.Level[i, j + 1].GetRectangle().Y - Collision.TriangleSolidVsActorResolution(this, tile).Y - GetRectangle().Height);

                                Position.Y = Collision.TriangleSolidVsActorResolution(this, tile).Y;
                            }

                        }*/

                        /// Platform
                        if (tile.hitbox.isEnabled && hitbox.rectangle.Intersects(tile.GetRectangle()) && tile.blockType == TileV2.BlockType.platform && !tile.isSlope)
                        {
                            ///          ///Provisoir///           ///
                            if (OldPosition.Y + GetRectangle().Height <= tile.GetRectangle().Y)
                            {

                                Position.Y = tile.Position.Y - hitbox.rectangle.Height;

                                Acceleration.Y = 0;
                                Velocity.Y = 0;
                                Velocity.X = 0;
                                isOnGround = true;

                                //if (tile.isBreakable)
                                    //tile.Break();

                            }
                        }
                    }
                }
            }

        }

        public override void UpCollision()
        {

            /// Static Block
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

                            Position.Y = Position.Y = tile.Position.Y + tile.GetRectangle().Height;

                            Up = true;
                            Velocity.Y = 0;
                            Acceleration.Y = 0;

                            //SetRidingTile(null);

                        }

                        ///Slope
                        /*if (tile.isSlope && Collision.TriangleSolidVsActor(this, tile) && Main.SolidTile[tile.getType()])
                        {

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
            hitbox = new Hitbox((int)Position.X, (int)Position.Y, 7, 5);
        }


    }
}
