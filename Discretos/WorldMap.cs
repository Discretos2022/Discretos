using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetworkEngine_5._0.Server;
using Plateform_2D_v9.NetCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Plateform_2D_v9
{
    class WorldMap
    {

        private static Vector2 LevelSelectorPos;

        private static float Velocity = 1f;


        static MapNode level_1 = new MapNode(new Vector2(368, 848), MapNodeType.Level, 3, "level 1");
        static MapNode path_1 = new MapNode(new Vector2(368, 864), MapNodeType.Path, name: "path 1");
        static MapNode path_2 = new MapNode(new Vector2(256, 896), MapNodeType.Path, name: "path 2");
        static MapNode level_2 = new MapNode(new Vector2(256, 928), MapNodeType.Level, 4, "level 2");
        static MapNode level_3 = new MapNode(new Vector2(368, 928), MapNodeType.Level, 5, "level 3");
        static MapNode level_4 = new MapNode(new Vector2(496, 928), MapNodeType.Level, 7, "level 4");
        static MapNode level_5 = new MapNode(new Vector2(608, 928), MapNodeType.Level, 6, "level 5");
        static MapNode path_3 = new MapNode(new Vector2(672, 928), MapNodeType.Path, name: "path 3");
        static MapNode path_4 = new MapNode(new Vector2(672, 912), MapNodeType.Path, name: "path 4");
        static MapNode path_5 = new MapNode(new Vector2(688, 912), MapNodeType.Path, name: "path 5");
        static MapNode path_6 = new MapNode(new Vector2(688, 848), MapNodeType.Path, name: "path 6");
        static MapNode path_7 = new MapNode(new Vector2(736, 848), MapNodeType.Path, name: "path 7");
        static MapNode path_8 = new MapNode(new Vector2(736, 864), MapNodeType.Path, name: "path 8");
        static MapNode level_6 = new MapNode(new Vector2(800, 864), MapNodeType.Level, 10, "level 6");

        private static MapNode node;

        private static List<MapNode> nodes = new List<MapNode>();

        private static Animation WaterFall_1 = new Animation(Main.Cascade, 5, 1, 0.1f, 1);

        private static float num = 0.0f; 


        public WorldMap()
        {

        }


        public static void Update(GameTime gameTime)
        {

            if (KeyInput.getKeyState().IsKeyDown(Main.Right) && LevelSelectorPos == node.Position)
            {
                if (node.rightNode != null)
                    node = node.rightNode;
            }

            else if (KeyInput.getKeyState().IsKeyDown(Main.Left) && LevelSelectorPos == node.Position)
            {
                if (node.leftNode != null)
                    node = node.leftNode;
            }

            else if (KeyInput.getKeyState().IsKeyDown(Main.Up) && LevelSelectorPos == node.Position)
            {
                if (node.upNode != null)
                    node = node.upNode;
            }

            else if (KeyInput.getKeyState().IsKeyDown(Main.Down) && LevelSelectorPos == node.Position)
            {
                if (node.downNode != null)
                    node = node.downNode;
            }

            else if (KeyInput.getKeyState().IsKeyDown(Keys.Enter) && LevelSelectorPos == node.Position)
            {
                if(node.type == MapNodeType.Level)
                {
                    if (NetPlay.IsMultiplaying)
                        ServerSender.SendLevelStarted(node.level);

                    Main.StartLevel(node.level);
                }
            }


            if(NetPlay.MyPlayerID() == 1)
            {
                if (LevelSelectorPos != node.Position)
                {

                    Vector2 dist = new Vector2(node.Position.X - LevelSelectorPos.X, node.Position.Y - LevelSelectorPos.Y);
                    Vector2 norm = Vector2.Normalize(dist) * Velocity;


                    if (norm.X > 0)
                        if (LevelSelectorPos.X + norm.X > node.Position.X)
                            LevelSelectorPos.X = node.Position.X;

                    if (norm.X < 0)
                        if (LevelSelectorPos.X + norm.X < node.Position.X)
                            LevelSelectorPos.X = node.Position.X;

                    if (norm.Y > 0)
                        if (LevelSelectorPos.Y + norm.Y > node.Position.Y)
                            LevelSelectorPos.Y = node.Position.Y;

                    if (norm.Y < 0)
                        if (LevelSelectorPos.Y + norm.Y < node.Position.Y)
                            LevelSelectorPos.Y = node.Position.Y;


                    if (LevelSelectorPos.X != node.Position.X)
                        LevelSelectorPos.X += norm.X;
                    if (LevelSelectorPos.Y != node.Position.Y)
                        LevelSelectorPos.Y += norm.Y;

                }
            }
            

            WaterFall_1.Update(gameTime);
            num += 0.025f;


        }


        public static void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(Main.WorldMapImg, Vector2.Zero, Color.White); //new Vector2(-200, -700)

            spriteBatch.Draw(Main.Bounds, new Vector2((int)LevelSelectorPos.X, (int)LevelSelectorPos.Y), Color.DarkBlue);

            if (Main.Debug)
            {

                for (int i = 0; i < nodes.Count; i++)
                {
                    spriteBatch.Draw(Main.Bounds, nodes[i].Position, Color.Red);
                    Writer.DrawText(Main.UltimateFont, nodes[i].name, nodes[i].Position + new Vector2(2, -5), Color.Black, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f, 0.5f, spriteBatch);
                }

            }

            WaterFall_1.Draw(spriteBatch, new Vector2(432, 960), SpriteEffects.None);
            WaterFall_1.Draw(spriteBatch, new Vector2(448, 960), SpriteEffects.None);

            WaterFall_1.Draw(spriteBatch, new Vector2(784, 928), SpriteEffects.None);
            WaterFall_1.Draw(spriteBatch, new Vector2(784, 944), SpriteEffects.None);

            spriteBatch.Draw(Main.Propeller, new Vector2(640, 842), null, Color.White, num, new Vector2(12, 12), 1f, SpriteEffects.None, 0f);


        }


        public static void CreateWorldMapData()
        {

            LevelSelectorPos = new Vector2(23 * 16, 58 * 16);


            level_1.SetAdjNode(down:path_1);

            path_1.SetAdjNode(up: level_1, left: path_2);

            path_2.SetAdjNode(right: path_1, down: level_2);

            level_2.SetAdjNode(up: path_2, right:level_3);

            level_3.SetAdjNode(left: level_2, right: level_4);

            level_4.SetAdjNode(left: level_3, right: level_5);

            level_5.SetAdjNode(left: level_4, right: path_3);

            path_3.SetAdjNode(left: level_5, up:path_4);

            path_4.SetAdjNode(down: path_3, right: path_5);

            path_5.SetAdjNode(left: path_4, up: path_6);

            path_6.SetAdjNode(down: path_5, right: path_7);

            path_7.SetAdjNode(left: path_6, down: path_8);

            path_8.SetAdjNode(up: path_7, right: level_6);

            level_6.SetAdjNode(left: path_8);


            nodes.Add(level_1);
            nodes.Add(path_1);
            nodes.Add(path_2);
            nodes.Add(level_2);
            nodes.Add(level_3);
            nodes.Add(level_4);
            nodes.Add(level_5);
            nodes.Add(path_3);
            nodes.Add(path_4);
            nodes.Add(path_5);
            nodes.Add(path_6);
            nodes.Add(path_7);
            nodes.Add(path_8);
            nodes.Add(level_6);

            node = level_1;
            LevelSelectorPos = node.Position;

            WaterFall_1.Start();

        }

        public enum MapNodeType
        {

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


        class MapNode
        {

            public Vector2 Position;

            public MapNode rightNode;
            public MapNode upNode;
            public MapNode downNode;
            public MapNode leftNode;

            public string name;
            public int level;
            public MapNodeType type;

            public MapNode(Vector2 position, MapNodeType type, int level = 0, string name = "")
            {
                Position = position;
                this.name = name;
                this.level = level;
                this.type = type;
            }

            public void SetAdjNode(MapNode up = null, MapNode down = null, MapNode right = null, MapNode left = null)
            {
                rightNode = right;
                upNode = up;
                downNode = down;
                leftNode = left;
                
            }


        }


    }

}