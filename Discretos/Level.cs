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

        public static void LoadLevel()
        {

            lastCheckPointNumber = 0;

            while (!Main.ContentLoaded)
            {
                Console.Clear();
                Console.WriteLine("Chargement ...");
            }

            Handler.Initialize();

            Handler.Level = null;
            Handler.Level = new TileV2[LevelData.getLevel(Main.LevelPlaying).GetLength(1), LevelData.getLevel(Main.LevelPlaying).GetLength(0)];

            Handler.Walls = null;
            Handler.Walls = new Wall[LevelData.GetWallType(Main.LevelPlaying).GetLength(1), LevelData.GetWallType(Main.LevelPlaying).GetLength(0)];


            /// TileMap
            for (int j = 0; j < Handler.Level.GetLength(1); j++)
            {
                for (int i = 0; i < Handler.Level.GetLength(0); i++)
                {

                    if (LevelData.getLevel(Main.LevelPlaying)[j, i] > 0)
                    {
                        //if (Main.SolidTile[(int)LevelData.getLevel(Main.LevelPlaying)[j, i]] || Main.SolidTileTop[(int)LevelData.getLevel(Main.LevelPlaying)[j, i]])
                        //{ 
                            if((int)LevelData.getLevel(Main.LevelPlaying)[j, i] != 8) // 8
                                Handler.Level[i, j] = new TileV2(new Vector2(i*16, j*16), (TileV2.BlockID)LevelData.getLevel(Main.LevelPlaying)[j, i], false);
                            if((int)LevelData.getLevel(Main.LevelPlaying)[j, i] == 8)
                            {
                                Handler.Level[i, j] = new TileV2(new Vector2(i * 16, j * 16), (TileV2.BlockID)LevelData.getLevel(Main.LevelPlaying)[j, i], false);
                            }
                            if(LevelData.getLevel(Main.LevelPlaying)[j, i] - (int)LevelData.getLevel(Main.LevelPlaying)[j, i] > 0)
                                Handler.Level[i, j] = new TileV2(new Vector2(i * 16, j * 16), (TileV2.BlockID)LevelData.getLevel(Main.LevelPlaying)[j, i], true);

                        //}

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


            LevelData.InitMovingBlock(Main.LevelPlaying);


            LevelData.getEnnemyData(Main.LevelPlaying);
            
            LoadWall(Main.LevelPlaying);
            
            Console.Clear();
            Console.WriteLine("Level " + Main.LevelPlaying + " Loaded");

            for (int i = 1; i <= Handler.playersV2.Count; i++)
            {
                Handler.playersV2[i].Position = getSpawn() - new Vector2(i * 10, 0);
            }

            LevelData.InitLevelSystem(Main.LevelPlaying);
            LevelData.InitBackground(Main.LevelPlaying);

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
