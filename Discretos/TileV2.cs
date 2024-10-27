using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetworkEngine_5._0.Server;
using Plateform_2D_v9.Core;
using Plateform_2D_v9.NetCore;
using Plateform_2D_v9.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    public class TileV2
    {

        public Vector2 Position;
        private Vector2 PosInLevel;

        private Rectangle TileFrame;

        private int w;
        private int h;
        private int Size;

        private bool isUnknowedTile;

        private Animation mechanicBlock;
        private Animation platformBrickBreak;

        public int timer = 0;


        public bool isSlope;
        public bool isBreakable;
        public bool isBroken;
        public bool isTouched;    /// For Breackable Block
        public bool isInvisible;

        public Vector2 OldPosition;
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public Vector2 Wind;
        public Vector2 Gravity;



        public BlockID ID;
        public BlockType blockType;
        public SlopeType slopeType = SlopeType.None;

        public Hitbox hitbox;

        public TileV2[,] grid;

        public bool isInMovingBlock;


        public TileV2(Vector2 Position, BlockID id, bool isSlope = false, Vector2 add = default, bool isInMovingBlock = false)
        {
            this.Position = Position;
            this.ID = id;
            this.isSlope = isSlope;
            this.isInvisible = false;
            this.isInMovingBlock = isInMovingBlock;

            w = 16;
            h = 16;

            PosInLevel = Position / 16;

            this.Position += add;

            hitbox = new Hitbox((int)Position.X, (int)Position.Y, 16, 16);

            InitBlock();

        }


        public void InitBlock()
        {
            switch (ID)
            {
                case BlockID.none:
                    break;

                case BlockID.grass:
                    blockType = BlockType.block;
                    break;

                case BlockID.platform_wood:
                    blockType = BlockType.platform;
                    break;

                case BlockID.sand:
                    blockType = BlockType.block;
                    break;

                case BlockID.snow:
                    blockType = BlockType.block;
                    break;

                case BlockID.cloud:
                    blockType = BlockType.block;
                    break;

                case BlockID.brick_gray:
                    blockType = BlockType.block;
                    break;

                case BlockID.volcano:
                    blockType = BlockType.block;
                    break;

                case BlockID.mechanical:
                    blockType = BlockType.block;
                    mechanicBlock = new Animation(Main.Tileset[(int)ID], 2, 1, 0.05f);
                    mechanicBlock.Start();
                    break;

                case BlockID.platform_brick_break:
                    blockType = BlockType.platform;
                    isBreakable = true;
                    break;

                case BlockID.ice:
                    blockType = BlockType.block;
                    break;

                case BlockID.low_snow: 
                    blockType = BlockType.block;
                    break;

            }
        }


        public void Update(GameTime gameTime)
        {

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

            if (ID != BlockID.mechanical && ID != BlockID.platform_brick_break)
            {
                if (TileFrame.IsEmpty)
                    spriteBatch.Draw(Main.Tileset[0], new Vector2((int)Position.X, (int)Position.Y), new Rectangle(0, 0, 16, 16), Color.White);
                else
                    spriteBatch.Draw(Main.Tileset[(int)ID], new Vector2(Position.X, Position.Y), TileFrame, Color.White);
            }

            if ((int)ID == 8)
                mechanicBlock.Draw(spriteBatch, GetPosition());

            if ((int)ID == 9 && !isBroken)
                platformBrickBreak.Draw(spriteBatch, GetPosition());  // - new Vector2(3, 6));


            DEBUG.DebugCollision(GetRectangle(), Color.Black, spriteBatch);
            DEBUG.DebugCollision(new Rectangle(GetRectangle().X + 1, GetRectangle().Y + 1, w - 2, h - 2), isSlope ? Color.Aqua : (hitbox.isEnabled ? Color.Blue : Color.Blue * 0.5f), spriteBatch);

            if (Main.Debug)
            {
                spriteBatch.DrawString(Main.UltimateFont, "x:" + (int)(Position.X / 16), Position + new Vector2(+2, +1), Color.Black, 0f, Vector2.Zero, 0.4f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(Main.UltimateFont, "y:" + (int)(Position.Y / 16), Position + new Vector2(+2, +6), Color.Black, 0f, Vector2.Zero, 0.4f, SpriteEffects.None, 0f);
            }
                    
        }


        public Rectangle GetRectangle()
        {
            return hitbox.rectangle;
        }


        public Vector2 GetPosition() {return Position;}

        public bool IsSlope() {return isSlope;}

        public void SetSlopeType(int type) {slopeType = (SlopeType)type;}

        public SlopeType GetSlopeType() {return slopeType;}

        public void SetSlope(bool IsSlope = false) {isSlope = IsSlope;}


        public void Break()
        {
            if(!isTouched)
                isTouched = true;
        }


        public void InitImg(TileV2[,] grid)
        {
            this.grid = grid;

            if (blockType == BlockType.block)
            {

                if (!isSlope)
                {

                    switch (ID)
                    {

                        case BlockID.grass:

                            if (RightID() == BlockID.low_snow && LeftID() == ID && UpType() == BlockType.none)
                            {
                                TileFrame = GetRectImg(19);
                                break;
                            }

                            goto default;

                        case BlockID.low_snow:

                            if (RightID() == ID && LeftID() == BlockID.grass && UpType() == BlockType.none)
                            {
                                TileFrame = GetRectImg(19);
                                break;
                            }

                            goto default;


                        default:

                            // RightID() == ID && LeftID() != ID && UpID() == ID && DownID() == ID && UpRightID() != ID && DownRightID() != ID && UpLeftID() != ID && DownLeftID() == ID

                            if (RightID() == ID && LeftID() == ID && UpID() != ID && DownID() == ID && DownRightID() == ID && DownLeftID() == ID)
                                TileFrame = GetRectImg(2);

                            else if (RightID() == ID && LeftID() != ID && UpID() != ID && DownID() == ID && DownRightID() == ID && UpLeftID() != ID)
                                TileFrame = GetRectImg(1);

                            else if (RightID() != ID && LeftID() == ID && UpID() != ID && DownID() == ID && UpRightID() != ID && DownLeftID() == ID)
                                TileFrame = GetRectImg(3);

                            else if (RightID() == ID && LeftID() != ID && UpID() == ID && DownID() == ID && UpRightID() == ID && DownRightID() == ID)
                                TileFrame = GetRectImg(4);

                            else if (RightID() == ID && LeftID() == ID && UpID() == ID && DownID() == ID && UpRightID() == ID && DownRightID() == ID && UpLeftID() == ID && DownLeftID() == ID)
                                TileFrame = GetRectImg(5);

                            else if (RightID() != ID && LeftID() == ID && UpID() == ID && DownID() == ID && UpLeftID() == ID && DownLeftID() == ID)
                                TileFrame = GetRectImg(6);

                            else if (RightID() == ID && LeftID() != ID && UpID() == ID && DownID() != ID && UpRightID() == ID && DownLeftID() != ID)
                                TileFrame = GetRectImg(7);

                            else if (RightID() == ID && LeftID() == ID && UpID() == ID && DownID() != ID && UpRightID() == ID && UpLeftID() == ID)
                                TileFrame = GetRectImg(8);

                            else if (RightID() != ID && LeftID() == ID && UpID() == ID && DownID() != ID && DownRightID() != ID && UpLeftID() == ID)
                                TileFrame = GetRectImg(9);

                            else if (RightID() != ID && LeftID() != ID && UpID() != ID && DownID() != ID && UpRightID() != ID && DownRightID() != ID && UpLeftID() != ID && DownLeftID() != ID)
                                TileFrame = GetRectImg(10);

                            else if (RightID() == ID && LeftID() == ID && UpID() == ID && DownID() == ID && UpRightID() == ID && DownRightID() != ID && UpLeftID() == ID && DownLeftID() == ID)
                                TileFrame = GetRectImg(11);

                            else if (RightID() == ID && LeftID() == ID && UpID() == ID && DownID() == ID && UpRightID() == ID && DownRightID() == ID && UpLeftID() == ID && DownLeftID() != ID)
                                TileFrame = GetRectImg(12);

                            else if (RightID() == ID && LeftID() == ID && UpID() == ID && DownID() == ID && UpRightID() != ID && DownRightID() == ID && UpLeftID() == ID && DownLeftID() == ID)
                                TileFrame = GetRectImg(13);

                            else if (RightID() == ID && LeftID() == ID && UpID() == ID && DownID() == ID && UpRightID() == ID && DownRightID() == ID && UpLeftID() != ID && DownLeftID() == ID)
                                TileFrame = GetRectImg(14);

                            break;

                    }

                }
                else
                {

                    switch (ID)
                    {

                        default:

                            // RightID() == ID && LeftID() != ID && UpID() == ID && DownID() == ID && UpRightID() != ID && DownRightID() != ID && UpLeftID() != ID && DownLeftID() == ID

                            if (RightID() == ID && LeftID() != ID && UpID() != ID && DownID() == ID && DownRightID() == ID && UpLeftID() != ID)
                            {
                                TileFrame = GetRectImg(15);
                                slopeType = SlopeType.LeftDown; 
                                break;
                            }

                            else if (RightID() != ID && LeftID() == ID && UpID() != ID && DownID() == ID && UpRightID() != ID && DownLeftID() == ID)
                            {
                                TileFrame = GetRectImg(16);
                                slopeType = SlopeType.RightDown;
                                break;
                            }

                            break;

                    }

                }

            }

            else if (blockType == BlockType.platform)
            {

                switch (ID)
                {

                    case BlockID.platform_wood:

                        if (RightType() == BlockType.block && LeftType() == BlockType.block)
                        { TileFrame = GetRectPlatformImg(1); break; }

                        if (RightType() != BlockType.block && LeftType() == BlockType.block)
                        { TileFrame = GetRectPlatformImg(2); break; }

                        if (RightType() == BlockType.block && LeftType() != BlockType.block)
                        { TileFrame = GetRectPlatformImg(3); break; }

                        if (RightType() != BlockType.block && LeftType() != BlockType.block)
                        { TileFrame = GetRectPlatformImg(4); break; }

                        goto default;

                    case BlockID.platform_brick_break:

                        if (RightType() != BlockType.block && LeftType() == BlockType.block)
                        {
                            platformBrickBreak = new Animation(Main.Tileset[(int)ID], 7, 3, 0.55f / 7.5f, 1);  // 0.55f
                            platformBrickBreak.Start();
                            break; 
                        }

                        else if (RightType() == BlockType.block && LeftType() != BlockType.block)
                        {
                            platformBrickBreak = new Animation(Main.Tileset[(int)ID], 7, 3, 0.55f / 7.5f, 2);  // 0.55f
                            platformBrickBreak.Start();
                            break;
                        }

                        else if (RightType() == BlockType.block && LeftType() != BlockType.block)
                        {
                            platformBrickBreak = new Animation(Main.Tileset[(int)ID], 7, 3, 0.55f / 7.5f, 3);  // 0.55f
                            platformBrickBreak.Start();
                            break;
                        }


                        platformBrickBreak = new Animation(Main.Tileset[(int)ID], 7, 3, 0.55f / 7.5f, 3);  // 0.55f

                        break;

                    default:
                        break;

                }

            }

        }


        public BlockID RightID()
        {
            int x = (int)PosInLevel.X + 1;
            int y = (int)PosInLevel.Y;

            if (x >= grid.GetLength(0))
            {
                if (isInMovingBlock) return BlockID.none;
                else return ID;
            }

            return grid[x, y].ID;

        }

        public BlockID LeftID()
        {
            int x = (int)PosInLevel.X - 1;
            int y = (int)PosInLevel.Y;

            if (x < 0)
            {
                if (isInMovingBlock) return BlockID.none;
                else return ID;
            }

            return grid[x, y].ID;

        }

        public BlockID UpID()
        {
            int x = (int)PosInLevel.X;
            int y = (int)PosInLevel.Y - 1;

            if (y < 0)
            {
                if (isInMovingBlock) return BlockID.none;
                else return ID;
            }

            return grid[x, y].ID;

        }

        public BlockID DownID()
        {
            int x = (int)PosInLevel.X;
            int y = (int)PosInLevel.Y + 1;

            if (y >= grid.GetLength(1))
            {
                if (isInMovingBlock) return BlockID.none;
                else return ID;
            }

            return grid[x, y].ID;

        }

        public BlockID UpRightID()
        {
            int x = (int)PosInLevel.X + 1;
            int y = (int)PosInLevel.Y - 1;

            if (x >= grid.GetLength(0) || y < 0)
            {
                if (isInMovingBlock) return BlockID.none;
                else return ID;
            }

            return grid[x, y].ID;

        }

        public BlockID DownRightID()
        {
            int x = (int)PosInLevel.X + 1;
            int y = (int)PosInLevel.Y + 1;

            if (x >= grid.GetLength(0) || y >= grid.GetLength(1))
            {
                if (isInMovingBlock) return BlockID.none;
                else return ID;
            }

            return grid[x, y].ID;

        }

        public BlockID UpLeftID()
        {
            int x = (int)PosInLevel.X - 1;
            int y = (int)PosInLevel.Y - 1;

            if (x < 0 || y < 0)
            {
                if (isInMovingBlock) return BlockID.none;
                else return ID;
            }

            return grid[x, y].ID;

        }

        public BlockID DownLeftID()
        {
            int x = (int)PosInLevel.X - 1;
            int y = (int)PosInLevel.Y + 1;

            if (x < 0 || y >= grid.GetLength(1))
            {
                if (isInMovingBlock) return BlockID.none;
                else return ID;
            }

            return grid[x, y].ID;

        }



        public BlockType RightType()
        {
            int x = (int)PosInLevel.X + 1;
            int y = (int)PosInLevel.Y;

            if (x >= grid.GetLength(0))
            {
                if (isInMovingBlock) return BlockType.none;
                else return blockType;
            }

            return grid[x, y].blockType;

        }

        public BlockType LeftType()
        {
            int x = (int)PosInLevel.X - 1;
            int y = (int)PosInLevel.Y;

            if (x < 0)
            {
                if (isInMovingBlock) return BlockType.none;
                else return blockType;
            }

            return grid[x, y].blockType;

        }

        public BlockType UpType()
        {
            int x = (int)PosInLevel.X;
            int y = (int)PosInLevel.Y - 1;

            if (y < 0)
            {
                if (isInMovingBlock) return BlockType.none;
                else return blockType;
            }

            return grid[x, y].blockType;

        }

        public BlockType DownType()
        {
            int x = (int)PosInLevel.X;
            int y = (int)PosInLevel.Y + 1;

            if (y >= grid.GetLength(1))
            {
                if (isInMovingBlock) return BlockType.none;
                else return blockType;
            }

            return grid[x, y].blockType;

        }

        public BlockType UpRightType()
        {
            int x = (int)PosInLevel.X + 1;
            int y = (int)PosInLevel.Y - 1;

            if (x >= grid.GetLength(0) || y < 0)
            {
                if (isInMovingBlock) return BlockType.none;
                else return blockType;
            }

            return grid[x, y].blockType;

        }

        public BlockType DownRightType()
        {
            int x = (int)PosInLevel.X + 1;
            int y = (int)PosInLevel.Y + 1;

            if (x >= grid.GetLength(0) || y >= grid.GetLength(1))
            {
                if (isInMovingBlock) return BlockType.none;
                else return blockType;
            }

            return grid[x, y].blockType;

        }

        public BlockType UpLeftType()
        {
            int x = (int)PosInLevel.X - 1;
            int y = (int)PosInLevel.Y - 1;

            if (x < 0 || y < 0)
            {
                if (isInMovingBlock) return BlockType.none;
                else return blockType;
            }

            return grid[x, y].blockType;

        }

        public BlockType DownLeftType()
        {
            int x = (int)PosInLevel.X - 1;
            int y = (int)PosInLevel.Y + 1;

            if (x < 0 || y >= grid.GetLength(1))
            {
                if (isInMovingBlock) return BlockType.none;
                else return blockType;
            }

            return grid[x, y].blockType;

        }



        public Rectangle GetRectImg(int id)
        {

            switch (id)
            {

                case 1:
                    return new Rectangle(0, 0, 16, 16);

                case 2:
                    return new Rectangle(17, 0, 16, 16);

                case 3:
                    return new Rectangle(34, 0, 16, 16);

                case 4:
                    return new Rectangle(0, 17, 16, 16);

                case 5:
                    return new Rectangle(17, 17, 16, 16);

                case 6:
                    return new Rectangle(34, 17, 16, 16);

                case 7:
                    return new Rectangle(0, 34, 16, 16);

                case 8:
                    return new Rectangle(17, 34, 16, 16);

                case 9:
                    return new Rectangle(34, 34, 16, 16);

                case 10:
                    return new Rectangle(0, 51, 16, 16);

                case 11:
                    return new Rectangle(52, 0, 16, 16);

                case 12:
                    return new Rectangle(69, 0, 16, 16);

                case 13:
                    return new Rectangle(52, 17, 16, 16);

                case 14:
                    return new Rectangle(69, 17, 16, 16);


                case 15:
                    return new Rectangle(52, 34, 16, 16);

                case 16:
                    return new Rectangle(69, 34, 16, 16);

                case 17:
                    return new Rectangle(52, 51, 16, 16);

                case 18:
                    return new Rectangle(69, 51, 16, 16); // J0279644, PH01046J


                case 19:
                    return new Rectangle(87, 0, 16, 16);


                default:
                    return new Rectangle(0, 0, 0, 0);

            }

        }

        public Rectangle GetRectPlatformImg(int id)
        {

            switch (id)
            {

                case 1:
                    return new Rectangle(0, 0, 16, 16);

                case 2:
                    return new Rectangle(17, 0, 16, 16);

                case 3:
                    return new Rectangle(34, 0, 16, 16);

                case 4:
                    return new Rectangle(51, 0, 16, 16);

                default:
                    return new Rectangle(0, 0, 0, 0);

            }

        }


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
            ice = 10,
            low_snow = 11,
        }

        public enum BlockType
        {
            none = 0,
            block = 1,
            platform = 2,
            liquid = 3,
        }

        public enum SlopeType
        {
            None = 0,
            LeftDown = 1,   //   /|
            RightDown = 2,  //   |\
            LeftUp = 3,     //   \|
            RightUp = 4,    //   |/
        }

        public Vector2 GetVelocity()
        {
            return Velocity;
        }

    }
}
