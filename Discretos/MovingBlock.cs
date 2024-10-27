using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateform_2D_v9.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateform_2D_v9
{
    public class MovingBlock
    {

        public TileV2[,] tiles;
        public List<Actor> actors;
        public Vector2 Position;
        public Vector2 Velocity;

        public MovingBlock(Vector2 position, float[,] table, List<Actor> actors = default)
        {

            Position = position;

            InitTile(table);

            this.actors = actors;

            Velocity = new Vector2(1, 0);
            

        }

        public void Update(GameTime gameTime)
        {

            //Velocity.X = 10;

            if (Position.X > 94 * 16 - 1)
            {
                Velocity.X *= -1.0f;
                //Position.X = 600;
            }


            else if (Position.X < 75 * 16 + 1)
            {
                Velocity.X *= -1.0f;
                //Position.X = 200;
            }

            if (Position.Y > 10)
            {
                Velocity.Y *= -1.0f;
                //Position.Y = 300;
            }


            else if (Position.Y < 13 * 16)
            {
                Velocity.Y *= -1.0f;
                //Position.Y = 50;
            }

        }


        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (tiles[i, j].ID > 0)
                    {
                        tiles[i, j].Update(gameTime);
                        tiles[i, j].Draw(spriteBatch, gameTime);
                    }

                    //tiles[i, j].Position = Position + new Vector2(i * 16, j * 16);

                }
            }

        }




        public void LeftDisplacement(GameTime gameTime)
        {

            if (Velocity.X < 0)
            {
                Position.X += Velocity.X;

                for (int i = 0; i < tiles.GetLength(0); i++)
                    for (int j = 0; j < tiles.GetLength(1); j++)
                    {
                        tiles[i, j].Position = Position + new Vector2(i * 16, j * 16);
                        tiles[i, j].hitbox.rectangle = new Rectangle((int)Position.X + i * 16, (int)Position.Y + j * 16, 16, 16);
                    }

            }

        }

        public void RightDisplacement(GameTime gameTime)
        {

            if (Velocity.X > 0)
            {
                Position.X += Velocity.X;

                for (int i = 0; i < tiles.GetLength(0); i++)
                    for (int j = 0; j < tiles.GetLength(1); j++)
                    {
                        tiles[i, j].Position = Position + new Vector2(i * 16, j * 16);
                        tiles[i, j].hitbox.rectangle = new Rectangle((int)tiles[i, j].Position.X, (int)tiles[i, j].Position.Y, 16, 16);
                    }

            }

        }

        public void DownDisplacement(GameTime gameTime)
        {

            if (Velocity.Y > 0)
            {
                Position.Y += Velocity.Y;

                for (int i = 0; i < tiles.GetLength(0); i++)
                    for (int j = 0; j < tiles.GetLength(1); j++)
                    {
                        tiles[i, j].Position = Position + new Vector2(i * 16, j * 16);
                        tiles[i, j].hitbox.rectangle = new Rectangle((int)Position.X + i * 16, (int)Position.Y + j * 16, 16, 16);
                    }

            }

        }

        public void UpDisplacement(GameTime gameTime)
        {

            if (Velocity.Y < 0)
            {
                Position.Y += Velocity.Y;

                for (int i = 0; i < tiles.GetLength(0); i++)
                    for (int j = 0; j < tiles.GetLength(1); j++)
                    {
                        tiles[i, j].Position = Position + new Vector2(i * 16, j * 16);
                        tiles[i, j].hitbox.rectangle = new Rectangle((int)Position.X + i * 16, (int)Position.Y + j * 16, 16, 16);
                    }

            }

        }




        public void InitTile(float[,] table)
        {

            tiles = new TileV2[table.GetLength(1), table.GetLength(0)];


            for (int j = 0; j < table.GetLength(0); j++)
            {
                for (int i = 0; i < table.GetLength(1); i++)
                {

                    if (table[j, i] > 0)
                    {
                        if (Main.SolidTile[(int)table[j, i]] || Main.SolidTileTop[(int)table[j, i]])
                        {
                            if ((int)table[j, i] != 8) // 8
                                tiles[i, j] = new TileV2(new Vector2(i * 16, j * 16), (TileV2.BlockID)table[j, i], false, Position);
                            if ((int)table[j, i] == 8)
                            {
                                tiles[i, j] = new TileV2(new Vector2(i * 16, j * 16), (TileV2.BlockID)table[j, i], false, Position);

                                //Handler.Level[i, j] = new TileV2(new Vector2(i * 16, j * 16), 0, false, true);
                                //Handler.solids.Add(new TileV2(new Vector2(i * 16, j * 16), (TileV2.BlockID)LevelData.getLevel(Main.LevelPlaying)[j, i], true));
                            }
                            if (table[j, i] - (int)table[j, i] > 0)
                                tiles[i, j] = new TileV2(new Vector2(i * 16, j * 16), (TileV2.BlockID)table[j, i], true, Position);

                        }
                    }
                    else
                    {
                        tiles[i, j] = new TileV2(new Vector2(i * 16, j * 16), TileV2.BlockID.none, false, Position);
                    }



                }
            }


            for (int i = 0; i < tiles.GetLength(0); i++)
            {

                for (int j = 0; j < tiles.GetLength(1); j++)
                {

                    tiles[i, j].InitImg(tiles);

                }

            }


        }

    }
}
