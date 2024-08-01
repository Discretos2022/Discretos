using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateform_2D_v9.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    public class TileV2 //: Solid
    {

        public Vector2 Position;
        private Vector2 PosInLevel;

        private Rectangle TileFrame;

        private int w;
        private int h;
        private int Size;

        private bool isUnknowedTile;

        private bool Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft;

        private Animation mechanicBlock;
        private Animation platformBrickBreak;

        public int timer = 0;



        public bool isStatic;
        public bool isSlope;
        public bool isBreakable;
        public bool isBroken;
        public bool isTouched;    /// For Breackable Block
        public bool isInvisible;
        public int SlopeType;

        public Vector2 OldPosition;
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public Vector2 Wind;
        public Vector2 Gravity;



        public BlockID ID;
        public BlockType blockType;

        public Hitbox hitbox;

        public float[,] grid;


        public TileV2(Vector2 Position, BlockID id, bool isSlope = false, bool isStatic = true, float[,] grid = null, Vector2 add = default)
        {
            this.Position = Position;
            this.ID = id;
            this.isSlope = isSlope;
            this.isInvisible = false;

            w = 16;
            h = 16;

            PosInLevel = Position / 16;

            this.Position += add;

            if (grid == null)
                this.grid = LevelData.getLevel(Main.LevelPlaying);
            else
                this.grid = grid;

            if (ID > 0)
            {
                setImg();
                //Console.WriteLine("Loading");
            }

            hitbox = new Hitbox((int)Position.X, (int)Position.Y, 16, 16);

            Init();

            isStatic = false;

            /*if(type == 8)
            {
                Velocity = new Vector2(1, 0);
                mechanicBlock.Start();
            }*/
            //if(type == 9)
            //{
            //    platformBrickBreak.Start();
            //}

            


        }


        public void Init()
        {
            switch (ID)
            {
                case BlockID.none:
                    break;

                case BlockID.grass:
                    setImg();
                    blockType = BlockType.block;
                    //hitbox.isEnabled = false;
                    break;

                case BlockID.platform_wood:
                    setImg();
                    blockType = BlockType.platform;
                    break;

                case BlockID.sand:
                    setImg();
                    blockType = BlockType.block;
                    //hitbox.isEnabled = false;
                    break;

                case BlockID.snow:
                    setImg();
                    blockType = BlockType.block;
                    break;

                case BlockID.cloud:
                    setImg();
                    blockType = BlockType.block;
                    break;

                case BlockID.brick_gray:
                    setImg();
                    blockType = BlockType.block;
                    break;

                case BlockID.volcano:
                    setImg();
                    blockType = BlockType.block;
                    break;

                case BlockID.mechanical:
                    blockType = BlockType.block;
                    mechanicBlock = new Animation(Main.Tileset[(int)ID], 2, 1, 0.05f);
                    //Velocity = new Vector2(1, 0);
                    mechanicBlock.Start();
                    break;

                case BlockID.platform_brick_break:
                    blockType = BlockType.platform;
                    platformBrickBreak = new Animation(Main.Tileset[(int)ID], 7, 2, 0.55f / 7.5f, 1);  // 0.55f
                    platformBrickBreak.Start();
                    isBreakable = true;
                    break;


            }
        }


        public void Update(GameTime gameTime)
        {

            if (Position.X < 200 || Position.X > 600)
                Velocity.X *= -1;

            if (Position.Y < 100 || Position.Y > 200)
                Velocity.Y *= -1;


            /*for(int i = 0; i < Handler.solids.Count - 1; i++)
            {
                Solid solid = Handler.solids[i];

                if(solid != this)
                    if (Collision.RectVsRect(GetRectangle(), solid.GetRectangle()))
                    {
                        Velocity.X *= -1;
                    }


            }*/


            Position.X += Velocity.X;
            Position.Y += Velocity.Y;
            //if(type == 8)
                //Velocity = new Vector2(1, 1);

            //Console.WriteLine(Position.Y + " ; " + GetRectangle().Y);

            if ((int)ID == 8)
                mechanicBlock.Update(gameTime);
            if ((int)ID == 9)
            {

                if (isTouched)
                {
                    timer++;
                    platformBrickBreak.Start();
                    platformBrickBreak.Update(gameTime);
                }

                if (timer >= 30) // 120
                {
                    isBroken = true;
                    hitbox.isEnabled = false;
                }

                if (timer >= 240)
                {
                    timer = 0;
                    isTouched = false;
                    isBroken = false;
                    hitbox.isEnabled = true;
                    platformBrickBreak.Reset();
                }



            }
                

        }


        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            //if (Position.X >= Handler.playersV2[PlayerID].Position.X - 500 && Position.X <= Handler.playersV2[PlayerID].Position.X + 500 && Position.Y >= Handler.playersV2[PlayerID].Position.Y - 400 && Position.Y <= Handler.playersV2[PlayerID].Position.Y + 400)
            //{

                if (ID != BlockID.mechanical && ID != BlockID.platform_brick_break)
                {
                    if (isUnknowedTile)
                        spriteBatch.Draw(Main.Tileset[0], new Vector2((int)Position.X, (int)Position.Y), getImg(), Color.White);
                    else
                        spriteBatch.Draw(Main.Tileset[(int)ID], new Vector2(Position.X, Position.Y), getImg(), Color.White);
                }

                if ((int)ID == 8)
                   mechanicBlock.Draw(spriteBatch, GetPosition());

                if ((int)ID == 9 && !isBroken)
                    platformBrickBreak.Draw(spriteBatch, GetPosition() - new Vector2(3, 6));


                DEBUG.DebugCollision(GetRectangle(), Color.Black, spriteBatch);
                DEBUG.DebugCollision(new Rectangle(GetRectangle().X + 1, GetRectangle().Y + 1, w - 2, h - 2), isSlope ? Color.Aqua : Color.Blue, spriteBatch);

                if (Main.Debug)
                {
                    spriteBatch.DrawString(Main.UltimateFont, "x:" + (int)(Position.X / 16), Position + new Vector2(+2, +1), Color.Black, 0f, Vector2.Zero, 0.4f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(Main.UltimateFont, "y:" + (int)(Position.Y / 16), Position + new Vector2(+2, +6), Color.Black, 0f, Vector2.Zero, 0.4f, SpriteEffects.None, 0f);
                }
                    
            //}

        }


        public Rectangle GetRectangle()
        {
            //if(isBroken)
                //return new Rectangle(0,0,0,0);

            return new Rectangle((int)Position.X, (int)Position.Y, w, h);
        }


        public Vector2 GetPosition() {return Position;}

        public bool IsSlope() {return isSlope;}

        public void SetSlopeType(int type) {SlopeType = type;}

        public int GetSlopeType() {return SlopeType;}

        public void SetSlope(bool IsSlope = false) {isSlope = IsSlope;}


        public void Break()
        {
            if(!isTouched)
                isTouched = true;
        }


        #region TileImg 

        public Rectangle getImg() {return TileFrame;}

        public void setImg()
        {
            Size = Main.TILESIZE;

            if (ID > 0)
            {

                CalulateTileAdjacent();

                if (!isSlope || isStatic)
                    if (Main.SolidTile[(int)ID])
                    {
                        TileFrame = new Rectangle((int)getImgTile().X, (int)getImgTile().Y, Size, Size);
                    }

                if (isSlope)
                {
                    UpRight = false;
                    DownRight = false;
                    DownLeft = false;
                    UpLeft = false;
                    TileFrame = new Rectangle((int)GetImgSlope().X, (int)GetImgSlope().Y, Size, Size);
                }

                if (Main.SolidTileTop[(int)ID] && (int)ID != 9)
                {
                    Up = false;
                    UpRight = false;
                    DownRight = false;
                    Down = false;
                    DownLeft = false;
                    UpLeft = false;
                    TileFrame = new Rectangle((int)getImgPlateform().X, (int)getImgPlateform().Y, Size, Size);

                }

                
                //if(type == 8)
                  //  mechanicBlock = new Animation(Main.Tileset[type], 2, 1, 0.05f);

                //if(type == 9)
                    //platformBrickBreak = new Animation(Main.Tileset[type], 7, 2, 0.55f / 7.5f, 1);  // 0.55f


            }

            else
            {
                TileFrame = new Rectangle(0, 0, 16, 16);
            }



        }

        private Vector2 getImgTile()
        {

            /// Center
            if (Up && UpRight && Right && DownRight && Down && DownLeft && Left && UpLeft)
            {
                return new Vector2(17, 17);
            }

            /// Up
            if (!Up && /*UpRight &&*/ Right && DownRight && Down && DownLeft && Left /*&& UpLeft*/)
            {
                return new Vector2(17, 0);
            }

            /// Up Right
            if (!Up && !UpRight && !Right && /*DownRight &&*/ Down && DownLeft && Left /*&& UpLeft*/)
            {
                return new Vector2(34, 0);
            }

            /// Right
            if (Up && /*UpRight &&*/ !Right && /*DownRight &&*/ Down && DownLeft && Left && UpLeft)
            {
                return new Vector2(34, 17);
            }

            /// Down Right
            if (Up && /*UpRight &&*/ !Right && !DownRight && !Down && /*DownLeft &&*/ Left && UpLeft)
            {
                return new Vector2(34, 34);
            }

            /// Down
            if (Up && UpRight && Right && /*DownRight &&*/ !Down && /*DownLeft &&*/ Left && UpLeft)
            {
                return new Vector2(17, 34);
            }

            /// Down Left
            if (Up && UpRight && Right && /*DownRight &&*/ !Down && !DownLeft && !Left /*&& UpLeft*/)
            {
                return new Vector2(0, 34);
            }

            /// Left
            if (Up && UpRight && Right && DownRight && Down && /*DownLeft &&*/ !Left /*&& UpLeft*/)
            {
                return new Vector2(0, 17);
            }

            /// Up Left
            if (!Up && /*UpRight &&*/ Right && DownRight && Down && /*DownLeft &&*/ !Left && !UpLeft)
            {
                return new Vector2(0, 0);
            }

            /// Coin Up Right
            if (Up && UpRight && Right && DownRight && Down && !DownLeft && Left && UpLeft)
            {
                return new Vector2(69, 0);
            }

            /// Coin Down Right
            if (Up && UpRight && Right && DownRight && Down && DownLeft && Left && !UpLeft)
            {
                return new Vector2(69, 17);
            }

            /// Coin Up Left
            if (Up && UpRight && Right && !DownRight && Down && DownLeft && Left && UpLeft)
            {
                return new Vector2(52, 0);
            }

            /// Coin Down Left
            if (Up && !UpRight && Right && DownRight && Down && DownLeft && Left && UpLeft)
            {
                return new Vector2(52, 17);
            }

            /// Center
            if (!Up && !UpRight && !Right && !DownRight && !Down && !DownLeft && !Left && !UpLeft)
            {
                return new Vector2(0, 51);
            }

            isUnknowedTile = true;
            return new Vector2(0, 0);

        }

        private Vector2 getImgPlateform()
        {
            if (!Right && !Left)
            {
                return new Vector2(51, 0);
            }

            if (Right && !Left)
            {
                return new Vector2(34, 0);
            }

            if (!Right && Left)
            {
                return new Vector2(17, 0);
            }

            if (Right && Left)
            {
                return new Vector2(0, 0);
            }

            return new Vector2(8, 0);

        }

        private Vector2 GetImgSlope()
        {
            if (!Up && Right && Down && !Left)
            {
                SetSlopeType(1);
                return new Vector2(52, 34);
            }

            if (!Up && !Right && Down && Left)
            {
                SetSlopeType(2);
                return new Vector2(69, 34);
            }

            if (Up && Right && !Down && !Left)
            {
                SetSlopeType(3);
                return new Vector2(52, 51);
            }

            if (Up && !Right && !Down && Left)
            {
                SetSlopeType(4);
                return new Vector2(69, 51);
            }


            //return new Vector2(10, 10);


            SetSlopeType(1);
            return new Vector2(52, 34);

        }

        private void CalulateTileAdjacent()
        {

            Up = false;
            UpRight = false;
            Right = false;
            DownRight = false;
            Down = false;
            DownLeft = false;
            Left = false;
            UpLeft = false;

            int num = grid.GetLength(1);
            int num2 = grid.GetLength(0);
            //Console.WriteLine(num + " : " + num2);

            /// Up
            if (PosInLevel.X == 0 || PosInLevel.Y == 0 || GetTileAdjacent(-1, 0))
                Up = true;

            /// UpRight
            if (PosInLevel.X == num - 1 || PosInLevel.Y == 0 || GetTileAdjacent(-1, +1))
                UpRight = true;

            /// Right
            if (PosInLevel.X == num - 1 || GetTileAdjacent(0, +1))
                Right = true;

            /// DownRight                                               
            if (PosInLevel.X == num - 1 || PosInLevel.Y == num2 - 1 || GetTileAdjacent(+1, +1))
                DownRight = true;

            /// Down                                                      
            if (PosInLevel.Y == num2 - 1 || GetTileAdjacent(+1, 0))
                Down = true;

            /// DownLeft                                                 
            if (PosInLevel.X == 0 || PosInLevel.Y == num2 - 1 || GetTileAdjacent(+1, -1))
                DownLeft = true;

            /// Left                                                   
            if (PosInLevel.X == 0 || GetTileAdjacent(0, -1))
                Left = true;

            /// UpLeft                                                
            if (PosInLevel.X == 0 || PosInLevel.Y == 0 || GetTileAdjacent(-1, -1))
                UpLeft = true;



        }


        private bool GetTileAdjacent(int posY, int posX)
        {

            int num1 = (int)PosInLevel.X + posX;
            int num2 = (int)PosInLevel.Y + posY;

            int num3 = (int)grid[num2, num1];

            //Console.WriteLine(LevelData.getLevel(Main.LevelPlaying).GetLength(0));

            if (num3 == -1)
                return false;

            if ((int)ID == 3 && num3 == 5)
                return true;

            if ((int)ID == 5 && num3 == 3)
                return true;

            if ((int)ID == 5 && num3 == 6)
                return true;

            if ((int)ID == 3 && num3 == 6)
                return true;

            if ((int)ID == 2 && Main.SolidTile[num3])
                return true;

            if ((int)ID == 2 && Main.SolidTileTop[num3])
                return false;

            if ((int)ID == num3)
                return true;


            //if (type == 1 && num3 == 1)
            //  return true;

            //if (type == 6 && num3 == 6)
            //  return true;


            

            //if (Main.SolidTile[num3])
            //  return true;

            return false;
        }


        #endregion




        public enum BlockID
        {
            none = 0,
            grass = 1,
            platform_wood = 2,
            sand = 3,
            snow = 4,
            cloud = 5,
            brick_gray = 6,
            volcano = 7,
            mechanical = 8,
            platform_brick_break = 9,
        }

        public enum BlockType
        {
            none = 0,
            block = 1,
            platform = 2,
            liquid = 3,
        }

        public Vector2 GetVelocity()
        {
            return Velocity;
        }

    }
}
