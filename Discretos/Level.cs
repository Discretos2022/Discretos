using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public static void LoadLevel(object threadContext)
        {

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
                            if((int)LevelData.getLevel(Main.LevelPlaying)[j, i] != 8)
                                Handler.Level[i, j] = new TileV2(new Vector2(i*16, j*16), (int)LevelData.getLevel(Main.LevelPlaying)[j, i], false, true);
                            if((int)LevelData.getLevel(Main.LevelPlaying)[j, i] == 8)
                            {
                                Handler.Level[i, j] = new TileV2(new Vector2(i * 16, j * 16), 0, false, true);
                                Handler.solids.Add(new TileV2(new Vector2(i * 16, j * 16), (int)LevelData.getLevel(Main.LevelPlaying)[j, i], true));
                            }
                            if(LevelData.getLevel(Main.LevelPlaying)[j, i] - (int)LevelData.getLevel(Main.LevelPlaying)[j, i] > 0)
                                Handler.Level[i, j] = new TileV2(new Vector2(i * 16, j * 16), (int)LevelData.getLevel(Main.LevelPlaying)[j, i], true, true);

                        }

                    }
                    else
                    {
                        Handler.Level[i, j] = new TileV2(new Vector2(i * 16, j * 16), 0, false, true);
                        if (LevelData.getLevel(Main.LevelPlaying)[j, i] == -1) { setSpawn(i, j); }
                    }

                    num++;


                }
                Main.text = num + "";  // * tiles / 100 + "    /   100";
            }

            Console.WriteLine(LevelData.getObjectData(Main.LevelPlaying).GetLength(1));

            /// LevelObject
            for (int j = 0; j < LevelData.getObjectData(Main.LevelPlaying).GetLength(1); j++)
            {
                for (int i = 0; i < LevelData.getObjectData(Main.LevelPlaying).GetLength(0); i++)
                {
                    Vector2 pos = new Vector2(j * 16, i * 16);
                    int[,] tableau = LevelData.getObjectData(Main.LevelPlaying);
                    int type = tableau[i, j];
                    if (type != 0)
                        Handler.actors.Add(new Object(pos, type));    //Handler.levelitem.Add(new LevelItem(pos, type));

                }

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

        public static void setCheckPoint(Vector2 CheckPointPos) { Spawn = CheckPointPos; } 

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

                    Handler.Walls[i, j] = new Wall(new Vector2(i * 16, j * 16), type, variante);

                }

            }



        }

    }
}
