using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Plateform_2D_v9.NetWorkEngine_3._0.Server;
using Plateform_2D_v9.NetWorkEngine_3._0;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Plateform_2D_v9
{
    class WorldMap
    {

        private static TileMap[,] MapLogic;
        private static Vector2 LevelSelectorPos;
        private static bool InDisplacement = false;
        private static Vector2 TilePosition = new Vector2(23 * 16, 58 * 16);
        private static Vector2 OldTilePosition;

        private static bool HorizontaleDisplacement;
        private static bool VerticaleDisplacement;

        private static float Velocity = 1f;


        public WorldMap()
        {

        }


        public static void Update(GameTime gameTime)
        {

            

            int X = (int)LevelSelectorPos.X/16;
            int Y = (int)LevelSelectorPos.Y/16;

            if (InDisplacement)
                Displacment(X, Y);

            MapLogic[X, Y].color = Color.Red;

            if (KeyInput.getKeyState().IsKeyDown(Keys.Enter) && MapLogic[X, Y].tileMapType == TileMapType.Level && !InDisplacement)
            {
                int Level = MapLogic[X, Y].LevelNum;

                Main.MapLoaded = false;
                Main.LevelSelector(Level);
                Main.inWorldMap = false;
                Main.inLevel = true;
                Camera.Zoom = 4f;
                Main.gameState = GameState.Playing;

                if (NetPlay.IsMultiplaying)
                    Server.SendLevelStated(Level);


            }

            if (KeyInput.getKeyState().IsKeyDown(Main.Right) && (MapLogic[X, Y].tileMapType == TileMapType.Level || MapLogic[X, Y].tileMapType == TileMapType.Crossroads) && !InDisplacement)
            {
                if(MapLogic[X + 1, Y].tileMapType == TileMapType.Path)
                {
                    InDisplacement = true;
                    
                    GotoTile(MapLogic[X + 1, Y].Position);
                    //OldTilePosition = MapLogic[X, Y].Position;

                } 
            }

            else if (KeyInput.getKeyState().IsKeyDown(Main.Left) && (MapLogic[X, Y].tileMapType == TileMapType.Level || MapLogic[X, Y].tileMapType == TileMapType.Crossroads) && !InDisplacement)
            {
                if (MapLogic[X - 1, Y].tileMapType == TileMapType.Path)
                {
                    InDisplacement = true;
                    GotoTile(MapLogic[X - 1, Y].Position);

                }
            }

            else if (KeyInput.getKeyState().IsKeyDown(Main.Up) && (MapLogic[X, Y].tileMapType == TileMapType.Level || MapLogic[X, Y].tileMapType == TileMapType.Crossroads) && !InDisplacement)
            {
                if (MapLogic[X, Y - 1].tileMapType == TileMapType.Path)
                {
                    InDisplacement = true;
                    GotoTile(MapLogic[X, Y - 1].Position);
                }
            }

            else if (KeyInput.getKeyState().IsKeyDown(Main.Down) && (MapLogic[X, Y].tileMapType == TileMapType.Level || MapLogic[X, Y].tileMapType == TileMapType.Crossroads) && !InDisplacement)
            {
                if (MapLogic[X, Y + 1].tileMapType == TileMapType.Path)
                {
                    InDisplacement = true;
                    GotoTile(MapLogic[X, Y + 1].Position);
                }
            }


            //Console.WriteLine(MapLogic[X, Y].tileMapType);
            //Console.WriteLine(InDisplacement);

            //Console.WriteLine(OldTilePosition);

        }

        public static void GotoTile(Vector2 Position)
        {
            OldTilePosition = TilePosition;
            TilePosition = Position;
        }

        public static void Displacment(int X, int Y)
        {

            #region Displacment X

            if ((TilePosition.X == (int)LevelSelectorPos.X || TilePosition.X == Util.UpperInteger(LevelSelectorPos.X)) && HorizontaleDisplacement && MapLogic[X, Y].Position != OldTilePosition)
            {

                if (MapLogic[(int)LevelSelectorPos.X / 16, (int)LevelSelectorPos.Y / 16].tileMapType == TileMapType.Level ||
                    MapLogic[(int)LevelSelectorPos.X / 16, (int)LevelSelectorPos.Y / 16].tileMapType == TileMapType.Crossroads)
                    InDisplacement = false;



                else if (MapLogic[X - 1, Y].tileMapType != TileMapType.Ground && MapLogic[X - 1, Y].Position != OldTilePosition)
                    GotoTile(MapLogic[X - 1, Y].Position);
                else if (MapLogic[X + 1, Y].tileMapType != TileMapType.Ground && MapLogic[X + 1, Y].Position != OldTilePosition)
                    GotoTile(MapLogic[X + 1, Y].Position);
                else if (MapLogic[X, Y - 1].tileMapType != TileMapType.Ground && MapLogic[X, Y - 1].Position != OldTilePosition)
                    GotoTile(MapLogic[X, Y - 1].Position);
                else if (MapLogic[X, Y + 1].tileMapType != TileMapType.Ground && MapLogic[X, Y + 1].Position != OldTilePosition)
                    GotoTile(MapLogic[X, Y + 1].Position);



                /*else if (MapLogic[X - 1, Y].tileMapType == TileMapType.Level && MapLogic[X - 1, Y].Position != OldTilePosition)
                    GotoTile(MapLogic[X - 1, Y].Position);
                else if (MapLogic[X + 1, Y].tileMapType == TileMapType.Level && MapLogic[X + 1, Y].Position != OldTilePosition)
                    GotoTile(MapLogic[X + 1, Y].Position);
                else if (MapLogic[X, Y - 1].tileMapType == TileMapType.Level && MapLogic[X, Y - 1].Position != OldTilePosition)
                    GotoTile(MapLogic[X, Y - 1].Position);
                else if (MapLogic[X, Y + 1].tileMapType == TileMapType.Level && MapLogic[X, Y + 1].Position != OldTilePosition)
                    GotoTile(MapLogic[X, Y + 1].Position);*/

                HorizontaleDisplacement = false;

            }

            if (TilePosition.X > Util.UpperInteger(LevelSelectorPos.X))
            {
                LevelSelectorPos.X += Velocity;
                HorizontaleDisplacement = true;
                //Console.WriteLine("Forward");
            }

            if (TilePosition.X < (int)LevelSelectorPos.X)
            {
                LevelSelectorPos.X -= Velocity;
                HorizontaleDisplacement = true;
                //Console.WriteLine("Backward");
            }

            #endregion

            #region Displacement Y

            if ((TilePosition.Y == (int)LevelSelectorPos.Y || TilePosition.Y == Util.UpperInteger(LevelSelectorPos.Y)) && VerticaleDisplacement && MapLogic[X, Y].Position != OldTilePosition)
            {


                if (MapLogic[(int)LevelSelectorPos.X / 16, (int)LevelSelectorPos.Y / 16].tileMapType == TileMapType.Level ||
                    MapLogic[(int)LevelSelectorPos.X / 16, (int)LevelSelectorPos.Y / 16].tileMapType == TileMapType.Crossroads)
                    InDisplacement = false;

                else if (MapLogic[X - 1, Y].tileMapType != TileMapType.Ground && MapLogic[X - 1, Y].Position != OldTilePosition)
                    GotoTile(MapLogic[X - 1, Y].Position);
                else if (MapLogic[X + 1, Y].tileMapType != TileMapType.Ground && MapLogic[X + 1, Y].Position != OldTilePosition)
                    GotoTile(MapLogic[X + 1, Y].Position);
                else if (MapLogic[X, Y - 1].tileMapType != TileMapType.Ground && MapLogic[X, Y - 1].Position != OldTilePosition)
                    GotoTile(MapLogic[X, Y - 1].Position);
                else if (MapLogic[X, Y + 1].tileMapType != TileMapType.Ground && MapLogic[X, Y + 1].Position != OldTilePosition)
                    GotoTile(MapLogic[X, Y + 1].Position);

                /*else if (MapLogic[X - 1, Y].tileMapType == TileMapType.Level && MapLogic[X - 1, Y].Position != OldTilePosition)
                    GotoTile(MapLogic[X - 1, Y].Position);
                else if (MapLogic[X + 1, Y].tileMapType == TileMapType.Level && MapLogic[X + 1, Y].Position != OldTilePosition)
                    GotoTile(MapLogic[X + 1, Y].Position);
                else if (MapLogic[X, Y - 1].tileMapType == TileMapType.Level && MapLogic[X, Y - 1].Position != OldTilePosition)
                    GotoTile(MapLogic[X, Y - 1].Position);
                else if (MapLogic[X, Y + 1].tileMapType == TileMapType.Level && MapLogic[X, Y + 1].Position != OldTilePosition)
                    GotoTile(MapLogic[X, Y + 1].Position);*/

                VerticaleDisplacement = false;

            }

            if (TilePosition.Y < (int)LevelSelectorPos.Y)
            {
                LevelSelectorPos.Y -= Velocity;
                VerticaleDisplacement = true;
            }
                
            if (TilePosition.Y > Util.UpperInteger(LevelSelectorPos.Y))
            {
                LevelSelectorPos.Y += Velocity;
                VerticaleDisplacement = true;
            }

            #endregion


        }


        public static void Draw(SpriteBatch spriteBatch)
        {
            /*for (int j = 0; j < Map.GetLength(1); j++)
                for (int i = 0; i < Map.GetLength(0); i++)
                {

                    tileMap[i, j].Draw(spriteBatch);
                }*/


            spriteBatch.Draw(Main.WorldMapImg, Vector2.Zero, Color.White); //new Vector2(-200, -700)

            for (int j = 0; j < MapLogic.GetLength(1); j++)
                for (int i = 0; i < MapLogic.GetLength(1); i++)
                {
                    MapLogic[j, i].Draw(spriteBatch);
                }

            spriteBatch.Draw(Main.Bounds, new Vector2((int)LevelSelectorPos.X, (int)LevelSelectorPos.Y), Color.DarkBlue);


        }


        public static void LoadWorldMap()
        {
            
            

        }


        public static void CreateWorldMapData()
        {
            MapLogic = new TileMap[62, 62]; // The World Dimension

            MapLogic[23, 53] = new TileMap(TileMapType.Level, 6); // Level Pos
            MapLogic[23, 54] = new TileMap(TileMapType.Path);
            MapLogic[23, 55] = new TileMap(TileMapType.Path);

            MapLogic[20, 56] = new TileMap(TileMapType.Level, 3); // Level Pos
            MapLogic[21, 56] = new TileMap(TileMapType.Path);
            MapLogic[22, 56] = new TileMap(TileMapType.Path);
            MapLogic[23, 56] = new TileMap(TileMapType.Crossroads);
            MapLogic[23, 57] = new TileMap(TileMapType.Path);
            MapLogic[23, 58] = new TileMap(TileMapType.Level, 4); // Level Pos
            MapLogic[24, 58] = new TileMap(TileMapType.Path);
            MapLogic[25, 58] = new TileMap(TileMapType.Path);
            MapLogic[26, 58] = new TileMap(TileMapType.Path);
            MapLogic[27, 58] = new TileMap(TileMapType.Path);
            MapLogic[28, 58] = new TileMap(TileMapType.Path);
            MapLogic[29, 58] = new TileMap(TileMapType.Path);
            MapLogic[30, 58] = new TileMap(TileMapType.Path);
            MapLogic[31, 58] = new TileMap(TileMapType.Level, 5);  // Level Pos
            MapLogic[32, 58] = new TileMap(TileMapType.Path);
            MapLogic[33, 58] = new TileMap(TileMapType.Path);
            MapLogic[34, 58] = new TileMap(TileMapType.Path);
            MapLogic[35, 58] = new TileMap(TileMapType.Path);
            MapLogic[36, 58] = new TileMap(TileMapType.Path);
            MapLogic[37, 58] = new TileMap(TileMapType.Path);
            MapLogic[38, 58] = new TileMap(TileMapType.Level, 7);  // Level Pos

            for (int j = 0; j < MapLogic.GetLength(1); j++)
                for (int i = 0; i < MapLogic.GetLength(1); i++)
                {
                    if (MapLogic[i, j] == null)
                        MapLogic[i, j] = new TileMap(TileMapType.Ground);

                    MapLogic[i, j].Position = new Vector2(i * 16, j * 16);
                }

            LevelSelectorPos = new Vector2(23 * 16, 58 * 16);

        }

        public enum TileMapType
        {
            Ground = 0,         // Terrain, obstacle, arbre, ...
            Path = 1,           // Chemin
            Level = 2,          // Niveau
            PathInWater = 3,    // Chemin dans l'eau
            Crossroads = 4,     // Croisment de chemins
            Ladder = 5,         // Échelle

        }

        public static Vector2 GetLevelSelectorPos()
        {
            return LevelSelectorPos;
        }

        public static void SetLevelSelectorPos(Vector2 _newPosition)
        {
            LevelSelectorPos = _newPosition;
        }

    }


    class TileMap
    {

        public Vector2 Position;
        public WorldMap.TileMapType tileMapType;
        public Color color = Color.CornflowerBlue;
        public int LevelNum;

        public TileMap(WorldMap.TileMapType tileMapType, int LevelNum = -1)
        {
            this.tileMapType = tileMapType;
            this.LevelNum = LevelNum;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DEBUG.DebugCollision(new Rectangle((int)Position.X, (int)Position.Y, 16, 16), color, spriteBatch);

            if (tileMapType == WorldMap.TileMapType.Ground)
                DEBUG.DebugCollision(new Rectangle((int)Position.X, (int)Position.Y, 16, 16), Color.CornflowerBlue, spriteBatch);

            color = new Color(Color.White, 0);

            //spriteBatch.Draw(Main.Bounds, new Rectangle((int)Position.X, (int)Position.Y, 16, 16), Color.White);
        }

    }

}