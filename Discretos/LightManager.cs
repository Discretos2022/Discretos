using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Plateform_2D_v9
{
    public static class LightManager
    {

        public static List<Light> lights;

        public static Color AmbianteLightR;
        public static Color AmbianteLightG;
        public static Color AmbianteLightB;

        //BasicEffect for rendering
        public static BasicEffect basicEffect;

        public static VertexBuffer vertexBuffer; // 65535
        public static IndexBuffer indexBuffer;

        public static VertexPositionColor[] vertex = new VertexPositionColor[65535];
        public static ushort[] indices = new ushort[65535];

        public static byte VertexIndex = 0;

        public static int vertexCount = 0;
        public static int indexCount = 0;
        public static int triangle_count = 0;


        public static void Init()
        {
            lights = new List<Light>();
        } 

        public static void InitHullSystem(Game game)
        {
            //BasicEffect
            basicEffect = new BasicEffect(game.GraphicsDevice);
            basicEffect.Alpha = 1f;

            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;

            vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionColor), 65535, BufferUsage.WriteOnly);
            indexBuffer = new IndexBuffer(game.GraphicsDevice, typeof(ushort), 65535, BufferUsage.WriteOnly);

        }

        public static void Update()
        {

        }

        public static void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(Main.Bounds, new Rectangle(0, 0, 2000, 500), AmbianteLightR);
            spriteBatch.Draw(Main.Bounds, new Rectangle(0, 0, 2000, 500), AmbianteLightG);
            spriteBatch.Draw(Main.Bounds, new Rectangle(0, 0, 2000, 500), AmbianteLightB);

            for (int i = 0; i < lights.Count; i++)
            {
                lights[i].Draw(spriteBatch);
            }
        }


        public static void DrawHull(Game game, Screen screen)
        {

            vertex = new VertexPositionColor[65535];
            vertexCount = 0;
            triangle_count = 0;

            for (int i = 0; i < lights.Count; i++)
            {

                if (lights[i].Radius > 40)
                {

                    int factor = 100;
                    //Vector2 Point1 = new Vector2(lights[i].Position.X - 16 * factor, lights[i].Position.Y - 16 * factor);
                    //Vector2 Point2 = new Vector2(lights[i].Position.X + 16 * factor, lights[i].Position.Y - 16 * factor);
                    //Vector2 Point3 = new Vector2(lights[i].Position.X - 16 * factor, lights[i].Position.Y + 16 * factor);

                    Vector2 Point1 = new Vector2(lights[i].Position.X - lights[i].Radius * factor, lights[i].Position.Y - lights[i].Radius * factor);
                    Vector2 Point2 = new Vector2(lights[i].Position.X + lights[i].Radius * factor, lights[i].Position.Y - lights[i].Radius * factor);
                    Vector2 Point3 = new Vector2(lights[i].Position.X - lights[i].Radius * factor, lights[i].Position.Y + lights[i].Radius * factor);

                    int xMin = (int)Point1.X / 16;
                    int xMax = (int)Point2.X / 16;

                    int yMin = (int)Point1.Y / 16;
                    int yMax = (int)Point3.Y / 16;

                    if (xMin < 0)
                        xMin = 0;
                    if (xMax > Handler.Level.GetLength(0) - 1)
                        xMax = Handler.Level.GetLength(0) - 1;
                    if (yMin < 0)
                        yMin = 0;
                    if (yMax >= Handler.Level.GetLength(1))
                        yMax = Handler.Level.GetLength(1) - 1;

                    for (int x = xMin; x <= xMax; x++)
                    {
                        for (int y = yMin; y <= yMax; y++)
                        {

                            TileV2 b = Handler.Level[x, y];  // 8, 6

                            if (b != null)
                            {

                                /// Diagonale  \
                                if (b.ID != TileV2.BlockID.none && b.ID != TileV2.BlockID.platform_wood && !GetIfBlockIsSolid(x, y + 1)) //  && ((lights[0].Position.X < b.Position.X + 32 && lights[0].Position.Y > b.Position.Y + 32) || b.Position.Y < 100)
                                {
                                    Vector2 p1 = new Vector2(b.Position.X * 2, b.Position.Y * 2);
                                    Vector2 p3 = new Vector2(b.Position.X * 2 + 32, b.Position.Y * 2 + 32); // -4
                                    Vector2 p2 = p3 + new Vector2(p3.X / 2 - lights[0].Position.X, (p3.Y / 2 - lights[0].Position.Y)) * 60; // 4   8
                                    Vector2 p4 = p1 + new Vector2(p1.X / 2 - lights[0].Position.X, (p1.Y / 2 - lights[0].Position.Y)) * 60; // 4   8

                                    p1.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                    p2.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                    p3.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                    p4.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);

                                    p1.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                    p2.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                    p3.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                    p4.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);

                                    CreateVertexTriangle(p1, p2, p3, Color.Yellow);
                                    CreateVertexTriangle(p1, p4, p2, Color.Red);

                                }


                                /// Down __
                                if (b.ID != TileV2.BlockID.none && b.ID != TileV2.BlockID.platform_wood && !GetIfBlockIsSolid(x, y + 1)) // (lights[i].Position.Y > b.Position.Y || b.Position.Y < 100)
                                {
                                    Vector2 p1 = new Vector2(b.Position.X * 2, b.Position.Y * 2 + 32);
                                    Vector2 p3 = new Vector2(b.Position.X * 2 + 32, b.Position.Y * 2 + 32); // -4
                                    Vector2 p2 = p3 + new Vector2(p3.X / 2 - lights[0].Position.X, (p3.Y / 2 - lights[0].Position.Y)) * 60; // 4   8
                                    Vector2 p4 = p1 + new Vector2(p1.X / 2 - lights[0].Position.X, (p1.Y / 2 - lights[0].Position.Y)) * 60; // 4   8

                                    p1.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                    p2.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                    p3.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                    p4.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);

                                    p1.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                    p2.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                    p3.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                    p4.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);

                                    CreateVertexTriangle(p1, p2, p3, Color.Yellow);
                                    CreateVertexTriangle(p1, p4, p2, Color.Red);

                                }


                                /// Left |_
                                if (b.ID != TileV2.BlockID.none && b.ID != TileV2.BlockID.platform_wood && !GetIfBlockIsSolid(x - 1, y)) // /*&& lights[0].Position.X < b.Position.X * 2*/
                                {
                                    Vector2 p1 = new Vector2(b.Position.X * 2, b.Position.Y * 2);
                                    Vector2 p3 = new Vector2(b.Position.X * 2, b.Position.Y * 2 + 32); // -4
                                    Vector2 p2 = p3 + new Vector2(p3.X / 2 - lights[0].Position.X, (p3.Y / 2 - lights[0].Position.Y)) * 60; // 4   8
                                    Vector2 p4 = p1 + new Vector2(p1.X / 2 - lights[0].Position.X, (p1.Y / 2 - lights[0].Position.Y)) * 60; // 4   8

                                    p1.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                    p2.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                    p3.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                    p4.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);

                                    p1.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                    p2.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                    p3.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                    p4.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);

                                    CreateVertexTriangle(p1, p2, p3, Color.Yellow);
                                    CreateVertexTriangle(p1, p4, p2, Color.Red);

                                }


                                /// Diagonale  /
                                if (b.ID != TileV2.BlockID.none && b.ID != TileV2.BlockID.platform_wood && !GetIfBlockIsSolid(x, y + 1)) //  && lights[0].Position.X < b.Position.X * 2 + 32 - 4 && lights[0].Position.Y > b.Position.Y + 32 - 4
                                {
                                    Vector2 p1 = new Vector2(b.Position.X * 2, b.Position.Y * 2 + 32);
                                    Vector2 p3 = new Vector2(b.Position.X * 2 + 32, b.Position.Y * 2); // -4
                                    Vector2 p2 = p3 + new Vector2(p3.X / 2 - lights[0].Position.X, (p3.Y / 2 - lights[0].Position.Y)) * 60; // 4   8
                                    Vector2 p4 = p1 + new Vector2(p1.X / 2 - lights[0].Position.X, (p1.Y / 2 - lights[0].Position.Y)) * 60; // 4   8

                                    p1.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                    p2.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                    p3.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                    p4.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);

                                    p1.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                    p2.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                    p3.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                    p4.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);

                                    CreateVertexTriangle(p1, p2, p3, Color.Yellow);
                                    CreateVertexTriangle(p1, p4, p2, Color.Red);

                                }


                                /// Up --
                                if (b != null && b.ID != TileV2.BlockID.none && b.ID != TileV2.BlockID.platform_wood && !GetIfBlockIsSolid(x, y - 1)) // lights[0].Position.Y < b.Position.Y + 32
                                {
                                    Vector2 p1 = new Vector2(b.Position.X * 2 + 32, b.Position.Y * 2);
                                    Vector2 p3 = new Vector2(b.Position.X * 2, b.Position.Y * 2); // -4
                                    Vector2 p2 = p3 + new Vector2(p3.X / 2 - lights[0].Position.X, (p3.Y / 2 - lights[0].Position.Y)) * 60; // 4   8
                                    Vector2 p4 = p1 + new Vector2(p1.X / 2 - lights[0].Position.X, (p1.Y / 2 - lights[0].Position.Y)) * 60; // 4   8

                                    p1.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                    p2.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                    p3.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                    p4.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);

                                    p1.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                    p2.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                    p3.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                    p4.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);

                                    CreateVertexTriangle(p1, p2, p3, Color.Yellow);
                                    CreateVertexTriangle(p1, p4, p2, Color.Red);

                                }


                                /// Right _|
                                if (b.ID != TileV2.BlockID.none && b.ID != TileV2.BlockID.platform_wood && !GetIfBlockIsSolid(x + 1, y)) //  /*&& lights[0].Position.X > b.Position.X * 2*/
                                {
                                    Vector2 p1 = new Vector2(b.Position.X * 2 + 32, b.Position.Y * 2 + 32);
                                    Vector2 p3 = new Vector2(b.Position.X * 2 + 32, b.Position.Y * 2); // -4
                                    Vector2 p2 = p3 + new Vector2(p3.X / 2 - lights[0].Position.X, (p3.Y / 2 - lights[0].Position.Y)) * 60; // 4   8
                                    Vector2 p4 = p1 + new Vector2(p1.X / 2 - lights[0].Position.X, (p1.Y / 2 - lights[0].Position.Y)) * 60; // 4   8

                                    p1.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                    p2.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                    p3.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                    p4.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);

                                    p1.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                    p2.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                    p3.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                    p4.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);

                                    CreateVertexTriangle(p1, p2, p3, Color.Yellow);
                                    CreateVertexTriangle(p1, p4, p2, Color.Red);

                                }

                            }

                        }
                    }

                }

            }

            game.GraphicsDevice.SetVertexBuffer(vertexBuffer);
            game.GraphicsDevice.Indices = indexBuffer;

            DrawVertex(game);

        }


        public static bool GetIfBlockIsSolid(int x, int y)
        {

            if (x <= 0 || y <= 0 || x == Handler.Level.GetLength(0) || y == Handler.Level.GetLength(1))
                return true;

            if (Handler.Level[x, y] != null && Handler.Level[x, y].ID != 0 && (int)Handler.Level[x, y].ID != 2)
                return true;

            return false;


        }



        public static void CreateVertexTriangle(Vector2 p1, Vector2 p2, Vector2 p3, Color color)
        {

            vertex[vertexCount] = new VertexPositionColor(new Vector3(p1, 0), color);
            vertexCount += 1;
            vertex[vertexCount] = new VertexPositionColor(new Vector3(p2, 0), color);
            vertexCount += 1;
            vertex[vertexCount] = new VertexPositionColor(new Vector3(p3, 0), color);
            vertexCount += 1;

            triangle_count += 1;

        }

        public static void DrawVertex(Game game)
        {

            vertexBuffer = new VertexBuffer(game.GraphicsDevice, VertexPositionColor.VertexDeclaration, 65535, BufferUsage.None);
            vertexBuffer.SetData(vertex);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                if (triangle_count > 0)
                    game.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, triangle_count);
            }
        }

    }


}
