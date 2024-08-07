using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateform_2D_v9.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Plateform_2D_v9
{
    public abstract class Actor
    {

        public Vector2 Position;
        public Vector2 OldPosition;
        public Vector2 Velocity;
        public Vector2 BaseVelocity;
        public Vector2 Acceleration;
        public Vector2 Wind;
        public float Gravity;
        public int PV;
        public Vector2 KnockBack;

        public bool isOnGround;
        public TileV2.SlopeType isOnSlope;
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

        public bool RightCollision = false;
        public bool LeftCollision = false;
        public bool UpCollision = false;
        public bool DownCollision = false;

        /// <summary>
        /// For espace of 1 block
        /// </summary>
        public bool tileOnUp = false;

        public MovingBlock blockUnder;

        /// <summary>
        /// For traversing platform
        /// </summary>
        public bool pressDown = false;

        public bool canBreakBlock = true;
        public bool canOpenDoor = true;


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



        public virtual void LeftDisplacement(GameTime gameTime) 
        {
            if (blockUnder != null && blockUnder.Velocity.X < 0) Position.X += blockUnder.Velocity.X;
            UpdateHitbox();
        }
        public virtual void RightDisplacement(GameTime gameTime)
        {
            if (blockUnder != null && blockUnder.Velocity.X > 0 && !LeftCollision) Position.X += blockUnder.Velocity.X;
            UpdateHitbox();
        }
        public abstract void DownDisplacement(GameTime gameTime);
        public abstract void UpDisplacement(GameTime gameTime);


        public virtual void LeftStaticCollision()
        {

            /// Static Block
            if (Handler.Level != null && Handler.Level.Length != 0 && Velocity.X + Wind.X < 0)
            {

                Vector2 Point1 = new Vector2(hitbox.rectangle.X, hitbox.rectangle.Y);
                Vector2 Point2 = new Vector2(hitbox.rectangle.X + hitbox.rectangle.Width - 0, hitbox.rectangle.Y);
                Vector2 Point3 = new Vector2(hitbox.rectangle.X, hitbox.rectangle.Y + hitbox.rectangle.Height - 1);

                int xMin = (int)Point1.X / 16;
                int xMax = (int)Point2.X / 16;

                int yMin = (int)Point1.Y / 16;
                int yMax = (int)Point3.Y / 16;

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

                        int index = i + 1;  ///
                        if (index > Handler.Level.GetLength(0))      /// For Slope BUG           (Pas encore fait dans TileV2 et EnemyV2)
                            index = Handler.Level.GetLength(0) - 1;      ///

                        if (tile == null)                        /// provisoir
                            break;


                        if (tile.hitbox.isEnabled && hitbox.rectangle.Intersects(tile.hitbox.rectangle) && Main.SolidTile[(int)tile.ID] && !tile.isSlope && !Handler.Level[index, j].isSlope)
                        {

                            Position.X = tile.Position.X + tile.hitbox.rectangle.Width;
                            LeftCollision = true;

                            LeftStaticCollisionAction();

                        }
                    }
                }
            }


            for (int i = 0; i < Handler.actors.Count; i++)
            {

                Actor actor = Handler.actors[i];

                if (this != actor)
                {

                    if (actor.actorType == ActorType.Object && actor.ID == 4 && hitbox.rectangle.Intersects(new Rectangle(actor.hitbox.rectangle.X + 1, actor.hitbox.rectangle.Y, actor.hitbox.rectangle.Width - 2, actor.hitbox.rectangle.Height)))
                    {
                        if (actor.hitbox.isEnabled || !canOpenDoor)
                        {
                            Position.X = actor.hitbox.rectangle.X + actor.hitbox.rectangle.Width - 1;
                            LeftCollision = true;

                            LeftStaticCollisionAction();
                        }

                    }

                }

            }

        }
        public virtual void RightStaticCollision()
        {

            /// Static Block
            if (Handler.Level != null && Handler.Level.Length != 0 && Velocity.X + Wind.X > 0)
            {

                Vector2 Point1 = new Vector2(hitbox.rectangle.X, hitbox.rectangle.Y);
                Vector2 Point2 = new Vector2(hitbox.rectangle.X + hitbox.rectangle.Width - 0, hitbox.rectangle.Y);
                Vector2 Point3 = new Vector2(hitbox.rectangle.X, hitbox.rectangle.Y + hitbox.rectangle.Height - 1);

                int xMin = (int)Point1.X / 16;
                int xMax = (int)Point2.X / 16;

                int yMin = (int)Point1.Y / 16;
                int yMax = (int)Point3.Y / 16;

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

                        int index = i - 1;  ///
                        if (index < 0)      /// For Slope BUG           (Pas encore fait dans TileV2 et EnemyV2)
                            index = 0;      ///

                        //if (tile == null)                        /// provisoir
                            //break;


                        if (tile.hitbox.isEnabled && hitbox.rectangle.Intersects(tile.hitbox.rectangle) && Main.SolidTile[(int)tile.ID] && !tile.isSlope && !Handler.Level[index, j].isSlope)
                        {

                            Position.X = tile.Position.X - hitbox.rectangle.Width;
                            RightCollision = true;

                            RightStaticCollisionAction();

                        }
                    }
                }
            }

            for (int i = 0; i < Handler.actors.Count; i++)
            {

                Actor actor = Handler.actors[i];

                if (this != actor)
                {

                    if (actor.actorType == ActorType.Object && actor.ID == 4 && hitbox.rectangle.Intersects(new Rectangle(actor.hitbox.rectangle.X + 1, actor.hitbox.rectangle.Y, actor.hitbox.rectangle.Width - 2, actor.hitbox.rectangle.Height)))
                    {

                        if (actor.hitbox.isEnabled || !canOpenDoor)
                        {
                            Position.X = actor.hitbox.rectangle.X - hitbox.rectangle.Width + 1;
                            RightCollision = true;

                            RightStaticCollisionAction();
                        }

                    }

                }

            }

            //if (IsSquish())
            //KillPlayer();

        }
        public virtual void DownStaticCollision()
        {

            isOnSlope = TileV2.SlopeType.None;

            /// Static Block
            Vector2 Point1 = new Vector2(hitbox.rectangle.X, hitbox.rectangle.Y);
            Vector2 Point2 = new Vector2(hitbox.rectangle.X + hitbox.rectangle.Width - 0, hitbox.rectangle.Y);
            Vector2 Point3 = new Vector2(hitbox.rectangle.X, hitbox.rectangle.Y + hitbox.rectangle.Height - 1);

            int xMin = (int)Point1.X / 16;
            int xMax = (int)Point2.X / 16;

            int yMin = (int)Point1.Y / 16 - 1;
            int yMax = (int)Point3.Y / 16 + 1;

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
                        if (tile.hitbox.isEnabled && hitbox.rectangle.Intersects(tile.hitbox.rectangle) && tile.blockType == TileV2.BlockType.block && !tile.isSlope)
                        {

                            Position.Y = tile.Position.Y - hitbox.rectangle.Height;

                            DownCollision = true;
                            Acceleration.Y = 0;
                            Velocity.Y = 0;
                            isOnGround = true;

                            DownStaticCollisionAction();

                            /// Break Block if Player is on and is on solid block in the left
                            //if (Position.X > tile.Position.X && Handler.Level[xMax, yMax].isBreakable && !tile.isBreakable)
                                //Handler.Level[xMax, yMax].Break();

                            //Console.WriteLine(tile.Position.X/16);

                            //Console.WriteLine("Down");

                        }

                        /// Slope collision
                        if(tile.hitbox.isEnabled && tile.isSlope && hitbox.rectangle.Intersects(tile.hitbox.rectangle) && tile.blockType == TileV2.BlockType.block)
                        {

                            if (tile.slopeType == TileV2.SlopeType.LeftDown && (Velocity.Y !> -Velocity.X)) //    /|
                            {

                                int posX = hitbox.rectangle.X + (hitbox.rectangle.Width / 1) - tile.hitbox.rectangle.X - 1; // - 5

                                if (posX < 0)
                                    posX = 0;

                                if (posX > 15)
                                    posX = 15;

                                if ((hitbox.rectangle.Y + hitbox.rectangle.Height) > tile.hitbox.rectangle.Y + (16 - posX))
                                {
                                    Position.Y = tile.hitbox.rectangle.Y + (16 - posX) - hitbox.rectangle.Height;

                                    DownCollision = true;
                                    Acceleration.Y = 0;
                                    Velocity.Y = 0;
                                    isOnGround = true;
                                    isOnSlope = TileV2.SlopeType.LeftDown;

                                    DownStaticCollisionAction();

                                }

                            }

                            else if (tile.slopeType == TileV2.SlopeType.RightDown && -Velocity.Y < Velocity.X) //    /|
                            {

                                int posX = hitbox.rectangle.X - tile.hitbox.rectangle.X + 1;

                                if (posX < 0)
                                    posX = 0;

                                if (posX > 15)
                                    posX = 15;

                                if ((hitbox.rectangle.Y + hitbox.rectangle.Height) > tile.hitbox.rectangle.Y + posX)
                                {
                                    Position.Y = tile.hitbox.rectangle.Y + posX - hitbox.rectangle.Height;

                                    DownCollision = true;
                                    Acceleration.Y = 0;
                                    Velocity.Y = 0;
                                    isOnGround = true;
                                    isOnSlope = TileV2.SlopeType.RightDown;

                                    DownStaticCollisionAction();

                                }

                            }

                        }

                        /// For descent of slope
                        if (tile.hitbox.isEnabled && tile.isSlope && new Rectangle(hitbox.rectangle.X, hitbox.rectangle.Y + 1, hitbox.rectangle.Width, hitbox.rectangle.Height).Intersects(tile.hitbox.rectangle) && tile.blockType == TileV2.BlockType.block)
                        {

                            if(tile.slopeType == TileV2.SlopeType.LeftDown)
                            {
                                int posX = hitbox.rectangle.X + (hitbox.rectangle.Width / 1) - tile.hitbox.rectangle.X - 1; // - 1

                                if (posX < 0)
                                    posX = 0;

                                if (posX > 15)
                                    posX = 15;

                                if ((hitbox.rectangle.Y + hitbox.rectangle.Height) > tile.hitbox.rectangle.Y + (16 - posX) - 2)
                                {
                                    isOnSlope = TileV2.SlopeType.LeftDown;
                                }
                            }

                            else if (tile.slopeType == TileV2.SlopeType.RightDown)
                            {
                                int posX = hitbox.rectangle.X - tile.hitbox.rectangle.X + 1;

                                if (posX < 0)
                                    posX = 0;

                                if (posX > 15)
                                    posX = 15;

                                if ((hitbox.rectangle.Y + hitbox.rectangle.Height) > tile.hitbox.rectangle.Y + posX - 2)
                                {
                                    isOnSlope = TileV2.SlopeType.RightDown;
                                }
                            }



                        }



                        /// Platform
                        if (tile.hitbox.isEnabled && hitbox.rectangle.Intersects(tile.hitbox.rectangle) && tile.blockType == TileV2.BlockType.platform && !tile.isSlope)
                        {
                            ///          ///Provisoir///           ///
                            if (OldPosition.Y + hitbox.rectangle.Height <= tile.hitbox.rectangle.Y && Velocity.Y >= 0 && !pressDown)
                            {

                                Position.Y = tile.Position.Y - hitbox.rectangle.Height;

                                DownCollision = true;
                                Acceleration.Y = 0;
                                Velocity.Y = 0;
                                isOnGround = true;

                                DownStaticCollisionAction();

                            }
                        }

                        UpdateHitbox();

                        if (tile.hitbox.isEnabled && new Rectangle(hitbox.rectangle.X, hitbox.rectangle.Y - 1, hitbox.rectangle.Width, 1).Intersects(tile.hitbox.rectangle))
                        {
                            if (tile.blockType == TileV2.BlockType.block)
                                tileOnUp = true;
                        }

                        /// Break all breakable platform
                        if (tile.hitbox.isEnabled && new Rectangle(hitbox.rectangle.X, hitbox.rectangle.Y + 1, hitbox.rectangle.Width, hitbox.rectangle.Height).Intersects(tile.hitbox.rectangle) && tile.blockType == TileV2.BlockType.platform && !tile.isSlope)
                        {

                            if (OldPosition.Y + hitbox.rectangle.Height <= tile.hitbox.rectangle.Y && Velocity.Y >= 0 && !pressDown && canBreakBlock)
                            {
                                if (tile.isBreakable)
                                    tile.Break();
                            }
                        }

                    }
                }
            }

        }
        public virtual void UpStaticCollision()
        {

            //isOnSlope = TileV2.SlopeType.None;

            /// Static Block
            Vector2 Point1 = new Vector2(hitbox.rectangle.X, hitbox.rectangle.Y);
            Vector2 Point2 = new Vector2(hitbox.rectangle.X + hitbox.rectangle.Width - 0, hitbox.rectangle.Y);
            Vector2 Point3 = new Vector2(hitbox.rectangle.X, hitbox.rectangle.Y + hitbox.rectangle.Height - 1);

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
                        if (tile.hitbox.isEnabled && hitbox.rectangle.Intersects(tile.hitbox.rectangle) && tile.blockType == TileV2.BlockType.block && !tile.isSlope)
                        {

                            //Console.WriteLine("Up");


                            Position.Y = tile.Position.Y + tile.hitbox.rectangle.Height;

                            UpCollision = true;
                            Velocity.Y = 0;
                            Acceleration.Y = 0;

                            UpStaticCollisionAction();

                            //SetRidingTile(null);

                        }

                        /// Slope
                        /*if (tile.isSlope && Collision.TriangleSolidVsActor(this, tile) && Main.SolidTile[tile.getType()])
                        {
                            /// Down Slope
                            if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightDown || (Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftDown)
                            {
                                //Console.WriteLine("Collision Slope");

                                if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightDown)
                                    PosOfDebug = Collision.TriangleSolidVsActorResolution(this, tile) + new Vector2(0, hitbox.rectangle.Height);

                                if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftDown)
                                    PosOfDebug = Collision.TriangleSolidVsActorResolution(this, tile) + new Vector2(hitbox.rectangle.Width - 1, hitbox.rectangle.Height);

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

                                if (Handler.Level[i, j + 1].hitbox.rectangle.Y - Collision.TriangleSolidVsActorResolution(this, tile).Y - hitbox.rectangle.Height <= 1)
                                    Acceleration.Y = 0;

                                //Console.WriteLine(Handler.Level[i, j + 1].hitbox.rectangle.Y - Collision.TriangleSolidVsActorResolution(this, tile).Y - hitbox.rectangle.Height);

                                Position.Y = Collision.TriangleSolidVsActorResolution(this, tile).Y;
                            }

                            /// Up Slope
                            if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightUp || (Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftUp)
                            {


                                if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftUp)
                                    PosOfDebug = Collision.TriangleSolidVsActorResolution(this, tile) + new Vector2(hitbox.rectangle.Width - 1, 0);
                                else if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightUp)
                                    PosOfDebug = Collision.TriangleSolidVsActorResolution(this, tile) + new Vector2(0, 0);

                                Position.Y = Collision.TriangleSolidVsActorResolution(this, tile).Y;

                                Up = true;
                                Velocity.Y = 0;
                                Acceleration.Y = 1;

                            }

                        }*/
                    }
                }

                if (result != 0)
                    Position.Y = result;

                //if (IsSquish())
                    //KillPlayer();

            }

        }

        public virtual void LeftStaticCollisionAction() { }
        public virtual void RightStaticCollisionAction() { }
        public virtual void DownStaticCollisionAction() { }
        public virtual void UpStaticCollisionAction() { }



        public virtual void LeftDynamicCollision() 
        {

            /// Dynamic Block
            for (int b = 0; b < Handler.blocks.Count; b++)
            {

                MovingBlock block = Handler.blocks[b];

                Vector2 Point1 = new Vector2(hitbox.rectangle.X, hitbox.rectangle.Y);
                Vector2 Point2 = new Vector2(hitbox.rectangle.X + hitbox.rectangle.Width - 0, hitbox.rectangle.Y);
                Vector2 Point3 = new Vector2(hitbox.rectangle.X, hitbox.rectangle.Y + hitbox.rectangle.Height - 1);

                int xMin = (int)(Point1.X - block.Position.X) / 16;
                int xMax = (int)(Point2.X - block.Position.X) / 16;

                int yMin = (int)(Point1.Y - block.Position.Y) / 16;
                int yMax = (int)(Point3.Y - block.Position.Y) / 16;

                if (xMin < 0)
                    xMin = 0;
                if (xMax > block.tiles.GetLength(0) - 1)
                    xMax = block.tiles.GetLength(0) - 1;
                if (yMin < 0)
                    yMin = 0;
                if (yMax >= block.tiles.GetLength(1))
                    yMax = block.tiles.GetLength(1) - 1;

                for (int j = yMin; j <= yMax; j++)
                {
                    for (int i = xMin; i <= xMax; i++)
                    {

                        TileV2 tile = block.tiles[i, j];

                        int index = i - 1;  ///
                        if (index < 0)      /// For Slope BUG           (Pas encore fait dans TileV2 et EnemyV2)
                            index = 0;      ///

                        if (tile == null)                        /// provisoir
                            break;

                        /// Collision dans le même sens     <-  ||  <-
                        int v = 0;
                        if (block.Velocity.X < 0 && (i == block.tiles.GetLength(0) - 1 || block.tiles[i + 1, j].blockType != TileV2.BlockType.block)) v = (int)block.Velocity.X;

                        if (tile.hitbox.isEnabled && hitbox.rectangle.Intersects(new Rectangle(tile.hitbox.rectangle.X + v, tile.hitbox.rectangle.Y, tile.hitbox.rectangle.Width, tile.hitbox.rectangle.Height)) && Main.SolidTile[(int)tile.ID] && !tile.isSlope)
                        {

                            Position.X = tile.hitbox.rectangle.X + v + tile.hitbox.rectangle.Width;
                            LeftCollision = true;

                            LeftDynamicCollisionAction(block);

                        }
                    }
                }
            }

            UpdateHitbox();

        }
        public virtual void RightDynamicCollision() 
        {

            /// Dynamic Block
            for (int b = 0; b < Handler.blocks.Count; b++)
            {

                MovingBlock block = Handler.blocks[b];

                Vector2 Point1 = new Vector2(hitbox.rectangle.X, hitbox.rectangle.Y);
                Vector2 Point2 = new Vector2(hitbox.rectangle.X + hitbox.rectangle.Width - 0, hitbox.rectangle.Y);
                Vector2 Point3 = new Vector2(hitbox.rectangle.X, hitbox.rectangle.Y + hitbox.rectangle.Height - 1);

                int xMin = (int)(Point1.X - block.Position.X) / 16;
                int xMax = (int)(Point2.X - block.Position.X) / 16;

                int yMin = (int)(Point1.Y - block.Position.Y) / 16;
                int yMax = (int)(Point3.Y - block.Position.Y) / 16;

                if (xMin < 0)
                    xMin = 0;
                if (xMax > block.tiles.GetLength(0) - 1)
                    xMax = block.tiles.GetLength(0) - 1;
                if (yMin < 0)
                    yMin = 0;
                if (yMax >= block.tiles.GetLength(1))
                    yMax = block.tiles.GetLength(1) - 1;

                for (int j = yMin; j <= yMax; j++)
                {
                    for (int i = xMin; i <= xMax; i++)
                    {

                        TileV2 tile = block.tiles[i, j];

                        int index = i - 1;  ///
                        if (index < 0)      /// For Slope BUG           (Pas encore fait dans TileV2 et EnemyV2)
                            index = 0;      ///

                        if (tile == null)                        /// provisoir
                            break;


                        if (tile.hitbox.isEnabled && hitbox.rectangle.Intersects(tile.hitbox.rectangle) && Main.SolidTile[(int)tile.ID] && !tile.isSlope)
                        {

                            Position.X = tile.Position.X - hitbox.rectangle.Width;
                            RightCollision = true;

                            RightDynamicCollisionAction(block);

                        }
                    }
                }
            }

            UpdateHitbox();

        }
        public virtual void DownDynamicCollision() 
        {

            tileOnUp = false;
            for (int b = 0; b < Handler.blocks.Count; b++)
            {

                MovingBlock block = Handler.blocks[b];
                blockUnder = null;

                Vector2 Point1 = new Vector2(hitbox.rectangle.X, hitbox.rectangle.Y);
                Vector2 Point2 = new Vector2(hitbox.rectangle.X + hitbox.rectangle.Width - 0, hitbox.rectangle.Y);
                Vector2 Point3 = new Vector2(hitbox.rectangle.X, hitbox.rectangle.Y + hitbox.rectangle.Height - 1);

                int xMin = (int)(Point1.X - block.Position.X) / 16;
                int xMax = (int)(Point2.X - block.Position.X) / 16;

                int yMin = (int)(Point1.Y - block.Position.Y) / 16 - 1;
                int yMax = (int)(Point3.Y - block.Position.Y) / 16 + 1;



                if (xMin < 0)
                    xMin = 0;
                if (xMax > block.tiles.GetLength(0) - 1)
                    xMax = block.tiles.GetLength(0) - 1;
                if (yMin < 0)
                    yMin = 0;
                if (yMax >= block.tiles.GetLength(1))
                    yMax = block.tiles.GetLength(1) - 1;

                float result = 0;

                for (int j = yMin; j <= yMax; j++)
                {
                    for (int i = xMin; i <= xMax; i++)
                    {

                        TileV2 tile = block.tiles[i, j];

                        /// NoSlope
                        if (tile.hitbox.isEnabled && hitbox.rectangle.Intersects(tile.hitbox.rectangle) && tile.blockType == TileV2.BlockType.block && !tile.isSlope)
                        {

                            Position.Y = tile.Position.Y - hitbox.rectangle.Height;

                            DownCollision = true;
                            Acceleration.Y = 0;
                            Velocity.Y = 0;
                            isOnGround = true;

                            DownDynamicCollisionAction(block);

                            /// Break Block if Player is on and is on solid block in the left
                            //if (Position.X > tile.Position.X && Handler.Level[xMax, yMax].isBreakable && !tile.isBreakable)
                                //Handler.Level[xMax, yMax].Break();

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
                                    PosOfDebug = Collision.TriangleSolidVsActorResolution(this, tile) + new Vector2(0, hitbox.rectangle.Height);

                                if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftDown)
                                    PosOfDebug = Collision.TriangleSolidVsActorResolution(this, tile) + new Vector2(hitbox.rectangle.Width - 1, hitbox.rectangle.Height);

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

                                if (Handler.Level[i, j + 1].hitbox.rectangle.Y - Collision.TriangleSolidVsActorResolution(this, tile).Y - hitbox.rectangle.Height <= 1)
                                    Acceleration.Y = 0;

                                //Console.WriteLine(Handler.Level[i, j + 1].hitbox.rectangle.Y - Collision.TriangleSolidVsActorResolution(this, tile).Y - hitbox.rectangle.Height);

                                Position.Y = Collision.TriangleSolidVsActorResolution(this, tile).Y;
                            }

                        }*/

                        /// Platform
                        if (tile.hitbox.isEnabled && hitbox.rectangle.Intersects(tile.hitbox.rectangle) && tile.blockType == TileV2.BlockType.platform && !tile.isSlope)
                        {
                            ///          ///Provisoir///           ///
                            if (OldPosition.Y + hitbox.rectangle.Height <= tile.hitbox.rectangle.Y && Velocity.Y >= 0 && !pressDown)// (!KeyInput.getKeyState().IsKeyDown(Main.Down) && !GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.DownPad)))
                            {

                                Position.Y = tile.Position.Y - hitbox.rectangle.Height;

                                DownCollision = true;
                                Acceleration.Y = 0;
                                Velocity.Y = 0;
                                isOnGround = true;

                                DownDynamicCollisionAction();

                                //if (tile.isBreakable)
                                    //tile.Break();

                            }
                        }

                        UpdateHitbox();


                        if (tile.hitbox.isEnabled && new Rectangle(hitbox.rectangle.X, hitbox.rectangle.Y + 1, hitbox.rectangle.Width, hitbox.rectangle.Height).Intersects(tile.hitbox.rectangle))
                        {
                            if (tile.blockType == TileV2.BlockType.block)
                                blockUnder = block;
                            if (tile.blockType == TileV2.BlockType.platform && OldPosition.Y + hitbox.rectangle.Height <= tile.hitbox.rectangle.Y)
                                blockUnder = block;
                        }

                        if (tile.hitbox.isEnabled && new Rectangle(hitbox.rectangle.X, hitbox.rectangle.Y - 1, hitbox.rectangle.Width, 1).Intersects(tile.hitbox.rectangle))
                        {
                            if (tile.blockType == TileV2.BlockType.block)
                                tileOnUp = true;
                        }

                        /// Break all breakable platform
                        if (tile.hitbox.isEnabled && new Rectangle(hitbox.rectangle.X, hitbox.rectangle.Y + 1, hitbox.rectangle.Width, hitbox.rectangle.Height).Intersects(tile.hitbox.rectangle) && tile.blockType == TileV2.BlockType.platform && !tile.isSlope)
                        {

                            if (OldPosition.Y + hitbox.rectangle.Height <= tile.hitbox.rectangle.Y && Velocity.Y >= 0 && !pressDown && canBreakBlock)
                            {
                                if (tile.isBreakable)
                                    tile.Break();
                            }
                        }


                    }
                }

            }

        }
        public virtual void UpDynamicCollision() 
        {

            for (int b = 0; b < Handler.blocks.Count; b++)
            {

                MovingBlock block = Handler.blocks[b];

                Vector2 Point1 = new Vector2(hitbox.rectangle.X, hitbox.rectangle.Y);
                Vector2 Point2 = new Vector2(hitbox.rectangle.X + hitbox.rectangle.Width - 0, hitbox.rectangle.Y);
                Vector2 Point3 = new Vector2(hitbox.rectangle.X, hitbox.rectangle.Y + hitbox.rectangle.Height - 1);

                int xMin = (int)(Point1.X - block.Position.X) / 16;
                int xMax = (int)(Point2.X - block.Position.X) / 16;

                int yMin = (int)(Point1.Y - block.Position.Y) / 16;
                int yMax = (int)(Point3.Y - block.Position.Y) / 16;



                if (xMin < 0)
                    xMin = 0;
                if (xMax > block.tiles.GetLength(0) - 1)
                    xMax = block.tiles.GetLength(0) - 1;
                if (yMin < 0)
                    yMin = 0;
                if (yMax >= block.tiles.GetLength(1))
                    yMax = block.tiles.GetLength(1) - 1;

                float result = 0;

                for (int j = yMin; j <= yMax; j++)
                {
                    for (int i = xMin; i <= xMax; i++)
                    {

                        TileV2 tile = block.tiles[i, j];

                        /// NoSlope
                        if (tile.hitbox.isEnabled && hitbox.rectangle.Intersects(tile.hitbox.rectangle) && tile.blockType == TileV2.BlockType.block && !tile.isSlope)
                        {

                            //Console.WriteLine("Up");

                            Position.Y = tile.Position.Y + tile.hitbox.rectangle.Height;

                            UpCollision = true;
                            Velocity.Y = 0;
                            Acceleration.Y = 0;

                            UpDynamicCollisionAction(block);

                        }

                        /// Slope
                        /*if (tile.isSlope && Collision.TriangleSolidVsActor(this, tile) && Main.SolidTile[(int)tile.ID])
                        {
                            /// Down Slope
                            if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightDown || (Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftDown)
                            {
                                //Console.WriteLine("Collision Slope");

                                if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.RightDown)
                                    PosOfDebug = Collision.TriangleSolidVsActorResolution(this, tile) + new Vector2(0, hitbox.rectangle.Height);

                                if ((Collision.BasicTriangleType)tile.SlopeType == Collision.BasicTriangleType.LeftDown)
                                    PosOfDebug = Collision.TriangleSolidVsActorResolution(this, tile) + new Vector2(hitbox.rectangle.Width - 1, hitbox.rectangle.Height);

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

                                if (Handler.Level[i, j + 1].hitbox.rectangle.Y - Collision.TriangleSolidVsActorResolution(this, tile).Y - hitbox.rectangle.Height <= 1)
                                    Acceleration.Y = 0;

                                //Console.WriteLine(Handler.Level[i, j + 1].hitbox.rectangle.Y - Collision.TriangleSolidVsActorResolution(this, tile).Y - hitbox.rectangle.Height);

                                Position.Y = Collision.TriangleSolidVsActorResolution(this, tile).Y;
                            }

                        }*/

                    }

                }

            }

        }

        public virtual void LeftDynamicCollisionAction(MovingBlock block = null) { }
        public virtual void RightDynamicCollisionAction(MovingBlock block = null) { }
        public virtual void DownDynamicCollisionAction(MovingBlock block = null) { }
        public virtual void UpDynamicCollisionAction(MovingBlock block = null) { }


        public abstract void UpdateHitbox();


    }
}
