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

        public virtual void LeftCollision()
        {

        }

        public virtual void RightCollision()
        {

        }

        public virtual void DownCollision()
        {

            UpdateHitbox();

            isOnSlope = false;


            /// Moving Block
            /*for (int i = 0; i < Handler.solids.Count; i++)
            {

                Solid tile = Handler.solids[i];

                if (Main.SolidTile[Handler.solids[i].getType()]) //&& !Main.tiles[i].IsSlope())
                {

                    if (hitbox.rectangle.Intersects(tile.GetRectangle()))
                    {

                        Position.Y = Collision.SolidVsActorResolution(Collision.Direction.Down, this, tile).Y;

                        Down = true;
                        Acceleration.Y = 0;
                        Velocity.Y = 0;
                        isJump = false;
                        isOnGround = true;
                        OnLadder = false;

                        //Console.WriteLine("Down");
                        
                    }
                }
            }*/


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
                            //if (OldPosition.Y + GetRectangle().Height <= tile.GetRectangle().Y && (!KeyInput.getKeyState().IsKeyDown(Main.Down) && !GamePadInput.GetPadState((PlayerIndex)ID - 1).IsButtonDown(Main.DownPad)))
                            //{

                                Position.Y = tile.Position.Y - hitbox.rectangle.Height;

                                Acceleration.Y = 0;
                                Velocity.Y = 0;
                                isOnGround = true;

                                if (tile.isBreakable)
                                    tile.Break();

                            //}
                        }
                    }
                }
            }

        }

        public virtual void UpCollision()
        {

        }


        public abstract void UpdateHitbox();


    }
}
