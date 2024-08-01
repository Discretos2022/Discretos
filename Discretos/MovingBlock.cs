using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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


        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            Position += Velocity;


            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (tiles[i, j].ID > 0)
                    {
                        tiles[i, j].Update(gameTime);
                        tiles[i, j].Draw(spriteBatch, gameTime);
                    }

                    tiles[i, j].Position = Position + new Vector2(i * 16, j * 16);

                }
            }

            if (Position.X > 400)
            {
                Velocity.X *= -1.0f;
                Position.X = 400;
            }
                

            else if (Position.X < 100)
            {
                Velocity.X *= -1.0f;
                Position.X = 100;
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
                                tiles[i, j] = new TileV2(new Vector2(i * 16, j * 16), (TileV2.BlockID)table[j, i], false, true, table, Position);
                            if ((int)table[j, i] == 8)
                            {
                                tiles[i, j] = new TileV2(new Vector2(i * 16, j * 16), (TileV2.BlockID)table[j, i], false, true, table, Position);

                                //Handler.Level[i, j] = new TileV2(new Vector2(i * 16, j * 16), 0, false, true);
                                //Handler.solids.Add(new TileV2(new Vector2(i * 16, j * 16), (TileV2.BlockID)LevelData.getLevel(Main.LevelPlaying)[j, i], true));
                            }
                            if (table[j, i] - (int)table[j, i] > 0)
                                tiles[i, j] = new TileV2(new Vector2(i * 16, j * 16), (TileV2.BlockID)table[j, i], true, true, table, Position);

                        }
                    }
                    else
                    {
                        tiles[i, j] = new TileV2(new Vector2(i * 16, j * 16), TileV2.BlockID.none, false, true, table, Position);
                    }



                }
            }
        }

    }
}
