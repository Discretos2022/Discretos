using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateform_2D_v9.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Plateform_2D_v9
{
    class Level
    {

        static int num;
        static int tiles;
        static Vector2 Spawn;

        public static int lastCheckPointNumber = 0;

        public static void LoadLevel(object threadContext)
        {

            lastCheckPointNumber = 0;

            while (!Main.ContentLoaded)
            {
                Console.Clear();
                Console.WriteLine("Chargement ...");
            }

            /// TileMap
            for (int j = 0; j < Handler.Level.GetLength(1); j++)
            {
                for (int i = 0; i < Handler.Level.GetLength(0); i++)
                {

                    if (LevelData.getLevel(Main.LevelPlaying)[j, i] > 0)
                    {
                        if (Main.SolidTile[(int)LevelData.getLevel(Main.LevelPlaying)[j, i]] || Main.SolidTileTop[(int)LevelData.getLevel(Main.LevelPlaying)[j, i]])
                        { 
                            if((int)LevelData.getLevel(Main.LevelPlaying)[j, i] != 8) // 8
                                Handler.Level[i, j] = new TileV2(new Vector2(i*16, j*16), (TileV2.BlockID)LevelData.getLevel(Main.LevelPlaying)[j, i], false);
                            if((int)LevelData.getLevel(Main.LevelPlaying)[j, i] == 8)
                            {
                                Handler.Level[i, j] = new TileV2(new Vector2(i * 16, j * 16), (TileV2.BlockID)LevelData.getLevel(Main.LevelPlaying)[j, i], false);
                            }
                            if(LevelData.getLevel(Main.LevelPlaying)[j, i] - (int)LevelData.getLevel(Main.LevelPlaying)[j, i] > 0)
                                Handler.Level[i, j] = new TileV2(new Vector2(i * 16, j * 16), (TileV2.BlockID)LevelData.getLevel(Main.LevelPlaying)[j, i], true);

                        }

                    }
                    else
                    {
                        Handler.Level[i, j] = new TileV2(new Vector2(i * 16, j * 16), TileV2.BlockID.none, false);
                        if (LevelData.getLevel(Main.LevelPlaying)[j, i] == -1) { setSpawn(i, j); }
                    }

                    num++;


                }
                Main.text = num + "";  // * tiles / 100 + "    /   100";
            }





            for (int j = 0; j < Handler.Level.GetLength(1); j++)
            {
                for (int i = 0; i < Handler.Level.GetLength(0); i++)
                {

                    Handler.Level[i, j].InitImg(Handler.Level);

                }
                Main.text = num + "";  // * tiles / 100 + "    /   100";
            }


            if(Main.LevelPlaying == 7)
            {

                float[,] table = new float[,]
                {
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 8, 6, 6, 2, 2, 6, 6, 8, 0 },
                    { 0, 6, 6, 6, 0, 0, 6, 6, 6, 0 },
                    { 0, 6, 6, 6, 0, 0, 6, 6, 6, 0 },
                    { 0, 8, 6, 6, 6, 6, 6, 6, 8, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                };

                Handler.blocks.Add(new MovingBlock(new Vector2(75 * 16 + 1, 16 * 16), table)); // new Vector2(300, 100)
            }


            LevelData.getEnnemyData(Main.LevelPlaying);
            //LevelData.GetWallData(Main.LevelPlaying);
            LoadWall(Main.LevelPlaying);
            
            Console.Clear();
            Console.WriteLine("Level " + Main.LevelPlaying + " Loaded");

            for (int i = 1; i <= Handler.playersV2.Count; i++)
            {
                Handler.playersV2[i].Position = getSpawn() - new Vector2(i * 10, 0);
            }
            

            Main.MapLoaded = true;

        }


        public static void setSpawn(int x, int y){ Spawn = new Vector2(x*16, y*16);}

        public static void setCheckPoint(CheckPoint checkPoint) { Spawn = checkPoint.Position; lastCheckPointNumber = checkPoint.number; } 

        public static Vector2 getSpawn()
        {
            return Spawn;
        }


        public static void LoadWall(int Level)
        {

            for (int j = 0; j < Handler.Walls.GetLength(1); j++)
            {
                for (int i = 0; i < Handler.Walls.GetLength(0); i++)
                {

                    int[,] grid = LevelData.GetWallVariante(Level);

                    int type = LevelData.GetWallType(Level)[j, i];
                    int variante = LevelData.GetWallVariante(Level)[j, i];

                    Handler.Walls[i, j] = new Wall(new Vector2(i * 16, j * 16), (Wall.WallID)type, variante);

                }

            }



        }

    }
}
