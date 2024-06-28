using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    public static class LightManager
    {

        public static List<Light> lights;

        public static Color AmbianteLightR;
        public static Color AmbianteLightG;
        public static Color AmbianteLightB;


        public static void Init()
        {
            lights = new List<Light>();
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


        public static void DrawHull(SpriteBatch spriteBatch, Game game, Main main, Screen screen)
        {

            //spriteBatch.Draw(Screen.LevelTarget, new Rectangle(0, 0, 1920, 1080), Color.Black);

            for (int i = 0; i < lights.Count; i++)
            {

                if (lights[i].Radius > 30)
                {

                    int factor = 8;

                    Vector2 Point1 = new Vector2(lights[i].Position.X - 16 * factor, lights[i].Position.Y - 16 * factor);
                    Vector2 Point2 = new Vector2(lights[i].Position.X + 16 * factor, lights[i].Position.Y - 16 * factor);
                    Vector2 Point3 = new Vector2(lights[i].Position.X - 16 * factor, lights[i].Position.Y + 16 * factor);

                    //Render.DrawLineV1_1(Main.Bounds, Point1, Point2, spriteBatch, Color.Black, 1);
                    //Render.DrawLineV1_1(Main.Bounds, Point1, Point3, spriteBatch, Color.Black, 1);

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

                            /*if (b != null && b.type != 0 && b.type != 2 && GetIfBlockIsSolid(x, y - 1) && lights[0].Position.X < b.Position.X * 2 + 32 - 4 && lights[0].Position.Y > b.Position.Y + 32 - 4 && KeyInput.getKeyState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F8))
                            {
                                Vector2 p11 = new Vector2(b.Position.X * 2 + 32 - 4, b.Position.Y * 2 - 28 - 0);
                                Vector2 p33 = new Vector2(b.Position.X * 2 + 32 - 4, b.Position.Y * 2 + 32 - 4);
                                Vector2 p22 = p33 + new Vector2(p33.X / 2 - lights[0].Position.X, (p33.Y / 2 - lights[0].Position.Y)) * 200; // 4   8


                                //Render.DrawLineV1_1(Main.Bounds, p33/2, p22/2, spriteBatch, Color.Red, 1);

                                //Render.DrawLineV1_1(Main.Bounds, p1, p1 + new Vector2(10,10), spriteBatch, Color.Red, 4);

                                p11.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2);
                                p22.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2);
                                p33.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2);

                                p11.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2);
                                p22.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2);
                                p33.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2);


                                Main.CreateVertexTriangle(p11, p22, p33, game, main, Color.Yellow);
                            }*/

                            /*if (b != null && b.type != 0 && b.type != 2 && GetIfBlockIsSolid(x + 1, y) && lights[0].Position.X < b.Position.X + 4 && lights[0].Position.Y > b.Position.Y + 32 - 4 && KeyInput.getKeyState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F8))
                            {
                                Vector2 p11 = new Vector2(b.Position.X * 2 + 4 + 28 + 32, b.Position.Y * 2);
                                Vector2 p33 = new Vector2(b.Position.X * 2 + 4, b.Position.Y * 2 + 4);
                                Vector2 p22 = p33 + new Vector2(p33.X / 2 - lights[0].Position.X, (p33.Y / 2 - lights[0].Position.Y)) * 8; // 4


                                //Render.DrawLineV1_1(Main.Bounds, p33/2, p22/2, spriteBatch, Color.Red, 1);

                                //Render.DrawLineV1_1(Main.Bounds, p1, p1 + new Vector2(10,10), spriteBatch, Color.Red, 4);

                                p11.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2);
                                p22.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2);
                                p33.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2);

                                p11.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2);
                                p22.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2);
                                p33.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2);


                                Main.CreateVertexTriangle(p22, p11, p33, game, main, Color.Red);

                            }*/

                            /// Diagonale  \
                            if (b != null && b.type != 0 && b.type != 2 && !GetIfBlockIsSolid(x, y + 1) && lights[0].Position.X < b.Position.X * 2 + 32 - 4 && lights[0].Position.Y > b.Position.Y + 32 - 4 && KeyInput.getKeyState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F8))
                            {
                                Vector2 p1 = new Vector2(b.Position.X * 2, b.Position.Y * 2);
                                Vector2 p3 = new Vector2(b.Position.X * 2 + 32, b.Position.Y * 2 + 32); // -4
                                Vector2 p2 = p3 + new Vector2(p3.X / 2 - lights[0].Position.X, (p3.Y / 2 - lights[0].Position.Y)) * 8; // 4   8
                                Vector2 p4 = p1 + new Vector2(p1.X / 2 - lights[0].Position.X, (p1.Y / 2 - lights[0].Position.Y)) * 6; // 4   8

                                //Render.DrawLineV1_1(Main.Bounds, p33/2, p22/2, spriteBatch, Color.Red, 1);

                                //Render.DrawLineV1_1(Main.Bounds, p1, p1 + new Vector2(10,10), spriteBatch, Color.Red, 4);


                                /*if (GetIfBlockIsSolid(x + 1, y))
                                    p1.X += 4;*/


                                p1.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                p2.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                p3.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                p4.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);

                                p1.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                p2.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                p3.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                p4.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);


                                Main.CreateVertexTriangle(p1, p2, p3, game, main, Color.Yellow);
                                Main.CreateVertexTriangle(p1, p4, p2, game, main, Color.Red);


                            }


                            /// Down __
                            if (b != null && b.type != 0 && b.type != 2 && !GetIfBlockIsSolid(x, y + 1) && lights[0].Position.Y > b.Position.Y && KeyInput.getKeyState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F8))
                            {
                                Vector2 p1 = new Vector2(b.Position.X * 2, b.Position.Y * 2 + 32);
                                Vector2 p3 = new Vector2(b.Position.X * 2 + 32, b.Position.Y * 2 + 32); // -4
                                Vector2 p2 = p3 + new Vector2(p3.X / 2 - lights[0].Position.X, (p3.Y / 2 - lights[0].Position.Y)) * 8; // 4   8
                                Vector2 p4 = p1 + new Vector2(p1.X / 2 - lights[0].Position.X, (p1.Y / 2 - lights[0].Position.Y)) * 6; // 4   8

                                //Render.DrawLineV1_1(Main.Bounds, p33/2, p22/2, spriteBatch, Color.Red, 1);

                                //Render.DrawLineV1_1(Main.Bounds, p1, p1 + new Vector2(10,10), spriteBatch, Color.Red, 4);



                                p1.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                p2.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                p3.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                p4.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);

                                p1.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                p2.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                p3.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                p4.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);

                                Console.WriteLine(game.GraphicsDevice.PresentationParameters.Bounds);


                                Main.CreateVertexTriangle(p1, p2, p3, game, main, Color.Yellow);
                                Main.CreateVertexTriangle(p1, p4, p2, game, main, Color.Red);


                            }

                            /// Left |_
                            if (b != null && b.type != 0 && b.type != 2 /*&& !GetIfBlockIsSolid(x - 1, y)*/ /*&& lights[0].Position.X < b.Position.X * 2*/ && KeyInput.getKeyState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F8))
                            {
                                Vector2 p1 = new Vector2(b.Position.X * 2, b.Position.Y * 2);
                                Vector2 p3 = new Vector2(b.Position.X * 2, b.Position.Y * 2 + 32); // -4
                                Vector2 p2 = p3 + new Vector2(p3.X / 2 - lights[0].Position.X, (p3.Y / 2 - lights[0].Position.Y)) * 8; // 4   8
                                Vector2 p4 = p1 + new Vector2(p1.X / 2 - lights[0].Position.X, (p1.Y / 2 - lights[0].Position.Y)) * 6; // 4   8

                                //Render.DrawLineV1_1(Main.Bounds, p33/2, p22/2, spriteBatch, Color.Red, 1);

                                //Render.DrawLineV1_1(Main.Bounds, p1, p1 + new Vector2(10,10), spriteBatch, Color.Red, 4);




                                p1.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                p2.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                p3.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                p4.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);

                                p1.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                p2.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                p3.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                p4.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);


                                Main.CreateVertexTriangle(p1, p2, p3, game, main, Color.Yellow);
                                Main.CreateVertexTriangle(p1, p4, p2, game, main, Color.Red);


                            }

                            /// Diagonale  /
                            if (b != null && b.type != 0 && b.type != 2 /*&& !GetIfBlockIsSolid(x, y + 1)*/ && lights[0].Position.X < b.Position.X * 2 + 32 - 4 && lights[0].Position.Y > b.Position.Y + 32 - 4 && KeyInput.getKeyState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F8))
                            {
                                Vector2 p1 = new Vector2(b.Position.X * 2, b.Position.Y * 2 + 32);
                                Vector2 p3 = new Vector2(b.Position.X * 2 + 32, b.Position.Y * 2); // -4
                                Vector2 p2 = p3 + new Vector2(p3.X / 2 - lights[0].Position.X, (p3.Y / 2 - lights[0].Position.Y)) * 8; // 4   8
                                Vector2 p4 = p1 + new Vector2(p1.X / 2 - lights[0].Position.X, (p1.Y / 2 - lights[0].Position.Y)) * 6; // 4   8

                                //Render.DrawLineV1_1(Main.Bounds, p33/2, p22/2, spriteBatch, Color.Red, 1);

                                //Render.DrawLineV1_1(Main.Bounds, p1, p1 + new Vector2(10,10), spriteBatch, Color.Red, 4);



                                p1.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                p2.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                p3.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                p4.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);

                                p1.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                p2.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                p3.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                p4.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);


                                Main.CreateVertexTriangle(p1, p2, p3, game, main, Color.Yellow);
                                Main.CreateVertexTriangle(p1, p4, p2, game, main, Color.Red);


                            }

                            /// Up --
                            if (b != null && b.type != 0 && b.type != 2 /*&& !GetIfBlockIsSolid(x, y + 1)*/ && lights[0].Position.Y < b.Position.Y + 32 && KeyInput.getKeyState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F8))
                            {
                                Vector2 p1 = new Vector2(b.Position.X * 2 + 32, b.Position.Y * 2);
                                Vector2 p3 = new Vector2(b.Position.X * 2, b.Position.Y * 2); // -4
                                Vector2 p2 = p3 + new Vector2(p3.X / 2 - lights[0].Position.X, (p3.Y / 2 - lights[0].Position.Y)) * 8; // 4   8
                                Vector2 p4 = p1 + new Vector2(p1.X / 2 - lights[0].Position.X, (p1.Y / 2 - lights[0].Position.Y)) * 6; // 4   8

                                //Render.DrawLineV1_1(Main.Bounds, p33/2, p22/2, spriteBatch, Color.Red, 1);

                                //Render.DrawLineV1_1(Main.Bounds, p1, p1 + new Vector2(10,10), spriteBatch, Color.Red, 4);




                                p1.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                p2.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                p3.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                p4.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);

                                p1.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                p2.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                p3.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                p4.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);


                                Main.CreateVertexTriangle(p1, p2, p3, game, main, Color.Yellow);
                                Main.CreateVertexTriangle(p1, p4, p2, game, main, Color.Red);


                            }

                            /// Right _|
                            if (b != null && b.type != 0 && b.type != 2 /*&& !GetIfBlockIsSolid(x - 1, y)*/ /*&& lights[0].Position.X > b.Position.X * 2*/ && KeyInput.getKeyState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F8))
                            {
                                Vector2 p1 = new Vector2(b.Position.X * 2 + 32, b.Position.Y * 2 + 32);
                                Vector2 p3 = new Vector2(b.Position.X * 2 + 32, b.Position.Y * 2); // -4
                                Vector2 p2 = p3 + new Vector2(p3.X / 2 - lights[0].Position.X, (p3.Y / 2 - lights[0].Position.Y)) * 8; // 4   8
                                Vector2 p4 = p1 + new Vector2(p1.X / 2 - lights[0].Position.X, (p1.Y / 2 - lights[0].Position.Y)) * 6; // 4   8

                                //Render.DrawLineV1_1(Main.Bounds, p33/2, p22/2, spriteBatch, Color.Red, 1);

                                //Render.DrawLineV1_1(Main.Bounds, p1, p1 + new Vector2(10,10), spriteBatch, Color.Red, 4);


                                p1.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                p2.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                p3.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);
                                p4.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2.0f);

                                p1.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                p2.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                p3.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);
                                p4.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2.0f);


                                Main.CreateVertexTriangle(p1, p2, p3, game, main, Color.Yellow);
                                Main.CreateVertexTriangle(p1, p4, p2, game, main, Color.Red);


                            }



                        }
                    }

                }

                

              

                /**Vector2 p11 = new Vector2(128*2 + 32, 96);
                Vector2 p33 = new Vector2(128*2 + 32 - 4, 96 + 128 - 4);
                Vector2 p22 = p33 + new Vector2(p33.X/2 - lights[0].Position.X, (p33.Y/2 - lights[0].Position.Y)) * 4;


                //Render.DrawLineV1_1(Main.Bounds, p33/2, p22/2, spriteBatch, Color.Red, 1);

                //Render.DrawLineV1_1(Main.Bounds, p1, p1 + new Vector2(10,10), spriteBatch, Color.Red, 4);

                p11.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2);
                p22.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2);
                p33.X *= game.GraphicsDevice.PresentationParameters.BackBufferWidth / (screen.Width / 2);

                p11.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2);
                p22.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2);
                p33.Y *= game.GraphicsDevice.PresentationParameters.BackBufferHeight / (screen.Height / 2);


                Main.DrawVertexTriangle(p11, p22, p33, game, main);**/



            }

            Main.DrawVertex(main, game);

            Main.vertex = new VertexPositionColor[65535];
            Main.vertexCount = 0;
            Main.triangle_count = 0;

        }


        public static bool GetIfBlockIsSolid(int x, int y)
        {

            if (x <= 0 || y <= 0 || x == Handler.Level.GetLength(0) || y == Handler.Level.GetLength(1))
                return true;

            if (Handler.Level[x, y] != null && Handler.Level[x, y].type != 0)
                return true;

            return false;


        }

        public static Rectangle CalculateDestinationRectangle(Game game)
        {
            Rectangle backbufferBounds = game.GraphicsDevice.PresentationParameters.Bounds;
            float backbufferAspectRatio = (float)backbufferBounds.Width / backbufferBounds.Height;
            float screenAspectRatio = (float)(1920/2) / (1080/2);

            float rx = 0f;
            float ry = 0f;
            float rw = backbufferBounds.Width;
            float rh = backbufferBounds.Height;


            if (backbufferAspectRatio > screenAspectRatio)
            {

                rw = rh * screenAspectRatio;
                rx = ((float)backbufferBounds.Width - rw) / 2f;

            }
            else if (backbufferAspectRatio < screenAspectRatio)
            {

                rh = rw / screenAspectRatio;
                ry = ((float)backbufferBounds.Height - rh) / 2f;

            }


            Rectangle result = new Rectangle((int)rx, (int)ry, (int)rw / 1, (int)rh / 1);
            return result;


        }


    }
}
