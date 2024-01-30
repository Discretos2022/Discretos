using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{

    /// <summary>
    /// Déconseillé
    /// </summary>
    public static class Collision
    {
        /// <summary>
        /// La Velocity n'est pas utilisé pour la collision ?
        /// </summary>
        static int correctionX = 0; // a.Velocity.X
        static int correctionY = 0; // a.Velocity.Y


        /// <summary>
        /// Détecte la collision entre 2 Rectangle.
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <returns></returns>
        public static bool RectVsRect(Rectangle r1, Rectangle r2)
        {
            return (r1.X < r2.X + r2.Width && r1.X + r1.Width > r2.X &&
                    r1.Y < r2.Y + r2.Height && r1.Y + r1.Height > r2.Y);
        }


        public static bool SolidVsActor(Actor a,  Solid t)
        {
            return (a.GetRectangle().X + a.Velocity.X < t.GetRectangle().X + t.GetRectangle().Width && a.GetRectangle().X + a.GetRectangle().Width + Util.UpperInteger(a.Velocity.X) + 1 > t.GetRectangle().X &&
                    a.GetRectangle().Y + a.Velocity.Y < t.GetRectangle().Y + t.GetRectangle().Height && a.GetRectangle().Y + a.GetRectangle().Height + a.Velocity.Y > t.GetRectangle().Y);
        }


        public static bool TriangleSolidVsActor(Actor a, Solid t)
        {

            Vector2 Point1 = new Vector2(a.GetRectangle().X, a.GetRectangle().Y);
            Vector2 Point2 = new Vector2(a.GetRectangle().X + a.GetRectangle().Width, a.GetRectangle().Y);
            Vector2 Point3 = new Vector2(a.GetRectangle().X, a.GetRectangle().Y + a.GetRectangle().Height);

            if ((BasicTriangleType)t.SlopeType == BasicTriangleType.LeftDown)
            {
                if (Point2.X > t.GetRectangle().X && Point1.X <= t.GetRectangle().X + t.GetRectangle().Width &&
                    Point3.Y >= t.GetRectangle().Y && Point3.Y <= t.GetRectangle().Y + t.GetRectangle().Height)
                {
                    int posX = a.GetRectangle().X + a.GetRectangle().Width - t.GetRectangle().X - 1;
                    int posY =   (a.GetRectangle().Y + a.GetRectangle().Height) - t.GetRectangle().Y - 1;

                    if (posY < 0)
                        posY = 0;

                    if (posX < 0) ///////  revoir !!!!!!!!!!!!!!!!!
                        posX = 0;

                    if (posX > 15)
                        posX = 15;

                    //Console.WriteLine("Test" + " ; " + posX + " ; " + posY + " ; " + (a.GetRectangle().Y + a.GetRectangle().Height) + " ; " + t.GetRectangle().Y + " ; " + (a.GetRectangle().Y - t.GetRectangle().Y));

                    return BasicTriangle((BasicTriangleType)t.SlopeType)[posY, posX] == 1;
                }
            }

            else if ((BasicTriangleType)t.SlopeType == BasicTriangleType.RightDown)
            {
                if (Point2.X >= t.GetRectangle().X && Point1.X < t.GetRectangle().X + t.GetRectangle().Width &&
                    Point3.Y >= t.GetRectangle().Y && Point3.Y <= t.GetRectangle().Y + t.GetRectangle().Height)
                {
                    int posX = a.GetRectangle().X - t.GetRectangle().X - 1;
                    int posY = (a.GetRectangle().Y + a.GetRectangle().Height) - t.GetRectangle().Y - 1;

                    if (posY < 0)
                        posY = 0;

                    if (posX < 0) ///////  revoir !!!!!!!!!!!!!!!!!
                        posX = 0;

                    if (posX > 15)
                        posX = 15;

                    //Console.WriteLine("Test" + " ; " + posX + " ; " + posY + " ; " + (a.GetRectangle().Y + a.GetRectangle().Height) + " ; " + t.GetRectangle().Y + " ; " + (a.GetRectangle().Y - t.GetRectangle().Y));

                    return BasicTriangle((BasicTriangleType)t.SlopeType)[posY, posX] == 1;
                }
            }

            else if((BasicTriangleType)t.SlopeType == BasicTriangleType.LeftUp)
            {
                if (Point2.X > t.GetRectangle().X && Point1.X <= t.GetRectangle().X + t.GetRectangle().Width &&
                    Point1.Y >= t.GetRectangle().Y && Point1.Y <= t.GetRectangle().Y + t.GetRectangle().Height)
                {
                    int posX = a.GetRectangle().X + a.GetRectangle().Width - t.GetRectangle().X - 1;
                    int posY = a.GetRectangle().Y - (t.GetRectangle().Y + t.GetRectangle().Height) - 1;

                    if (posY < 0)
                        posY = -posY;

                    if (posY < 0)
                        posY = 0;

                    if (posX < 0) ///////  revoir !!!!!!!!!!!!!!!!!
                        posX = 0;

                    if (posX > 15)
                        posX = 15;

                    if (posY > 15)
                        posY = 15;

                    //Console.WriteLine("Test" + " ; " + posX + " ; " + posY + " ; " + (a.GetRectangle().X - t.GetRectangle().X - 1) + " ; " + t.GetRectangle().Y + " ; " + (a.GetRectangle().Y - t.GetRectangle().Y));

                    return BasicTriangle(BasicTriangleType.LeftDown)[posY, posX] == 1;
                }
            }

            else if ((BasicTriangleType)t.SlopeType == BasicTriangleType.RightUp)
            {
                if (Point2.X >= t.GetRectangle().X && Point1.X < t.GetRectangle().X + t.GetRectangle().Width &&
                    Point1.Y >= t.GetRectangle().Y && Point1.Y <= t.GetRectangle().Y + t.GetRectangle().Height)
                {
                    int posX = a.GetRectangle().X - t.GetRectangle().X - 1;
                    int posY = a.GetRectangle().Y - (t.GetRectangle().Y + t.GetRectangle().Height) - 1;

                    if (posY < 0)
                        posY = -posY;

                    if (posY < 0)
                        posY = 0;

                    if (posX < 0) ///////  revoir !!!!!!!!!!!!!!!!!
                        posX = 0;

                    if (posX > 15)
                        posX = 15;

                    if (posY > 15)
                        posY = 15;

                    //Console.WriteLine("Test" + " ; " + posX + " ; " + posY + " ; " + (a.GetRectangle().X - t.GetRectangle().X - 1) + " ; " + t.GetRectangle().Y + " ; " + (a.GetRectangle().Y - t.GetRectangle().Y));

                    return BasicTriangle(BasicTriangleType.RightDown)[posY, posX] == 1;
                }
            }

            return false; 

        }


        public static Direction SolidVsActorDirection(Actor a, Solid t, Direction XelseY = Direction.NoCollision)
        {
            if (XelseY == Direction.X)                                                                                                                                                                                                                       // Recent Add //
                if (a.GetRectangle().X + a.Velocity.X < t.GetRectangle().X + t.GetRectangle().Width && a.GetRectangle().Y < t.GetRectangle().Y + t.GetRectangle().Height - a.Velocity.Y && a.GetRectangle().Y + a.GetRectangle().Height > t.GetRectangle().Y - t.Velocity.Y                             && a.OldPosition.X > t.GetRectangle().X + t.GetRectangle().Width - 5)   // 2 ou 10
                    return Direction.Left;

            if (XelseY == Direction.X)
                if (a.GetRectangle().X + a.GetRectangle().Width + Util.UpperInteger(a.Velocity.X) + 1 > t.GetRectangle().X && a.GetRectangle().Y < t.GetRectangle().Y + t.GetRectangle().Height - a.Velocity.Y && a.GetRectangle().Y + a.GetRectangle().Height > t.GetRectangle().Y - t.Velocity.Y                             && a.OldPosition.X + a.GetRectangle().Width - 5 < t.GetRectangle().X)   // 2 ou 10
                    return Direction.Right;


            if (XelseY == Direction.Y)
                if (a.GetRectangle().Y + a.Velocity.Y < t.GetRectangle().Y + t.GetRectangle().Height && a.GetRectangle().X < t.GetRectangle().X + t.GetRectangle().Width && a.GetRectangle().X + a.GetRectangle().Width > t.GetRectangle().X && a.GetRectangle().Y > t.GetRectangle().Y                 && a.OldPosition.Y > t.GetRectangle().Y + t.GetRectangle().Height/2)
                    return Direction.Up;

            if (XelseY == Direction.Y)
                if (a.GetRectangle().Y + a.GetRectangle().Height + a.Velocity.Y > t.GetRectangle().Y && a.GetRectangle().X < t.GetRectangle().X + t.GetRectangle().Width && a.GetRectangle().X + a.GetRectangle().Width > t.GetRectangle().X)
                    return Direction.Down;

            return Direction.NoCollision;

        }


        public static Vector2 SolidVsActorResolution(Direction direction, Actor a, Solid t)
        {
            if (direction == Direction.Left)
                return new Vector2(t.GetRectangle().X + t.GetRectangle().Width, 0);

            else if (direction == Direction.Right)
                return new Vector2(t.GetRectangle().X - a.GetRectangle().Width, 0);

            else if (direction == Direction.Up)
                return new Vector2(0, t.GetPosition().Y + t.GetRectangle().Height + t.Velocity.Y); //////////////////

            else if (direction == Direction.Down)
                return new Vector2(0, t.GetPosition().Y - a.GetRectangle().Height);


            return Vector2.Zero;

        }


        public static Vector2 TriangleSolidVsActorResolution(Actor a, Solid t)
        {
            if ((BasicTriangleType)t.SlopeType == BasicTriangleType.LeftDown)
            {
                int posX = a.GetRectangle().X + a.GetRectangle().Width - t.GetRectangle().X - 1;
                int posY = (a.GetRectangle().Y + a.GetRectangle().Height) - t.GetRectangle().Y;

                if (posY < 0)
                    posY = 0;

                if (posX < 0)
                    posX = 0;

                if (posX > 15)
                    posX = 15;

                //Console.WriteLine("HIC");

                for (int i = 0; i < posY; i++)
                {
                    //Console.WriteLine(i);
                    //Console.WriteLine(BasicTriangle((BasicTriangleType)t.SlopeType)[i, posX]);

                    if (BasicTriangle((BasicTriangleType)t.SlopeType)[i, posX] == 1)
                    {
                        //Console.WriteLine(i);
                        return new Vector2(a.GetRectangle().X, t.GetRectangle().Y + i - a.GetRectangle().Height);
                    }

                }

                



            }

            else if((BasicTriangleType)t.SlopeType == BasicTriangleType.RightDown)
            {

                int posX = a.GetRectangle().X - t.GetRectangle().X;
                int posY = (a.GetRectangle().Y + a.GetRectangle().Height) - t.GetRectangle().Y;

                //Console.WriteLine(posX);

                if (posY < 0)
                    posY = 0;

                if (posX < 0)
                    posX = 0;

                if (posX > 15)
                    posX = 15;

                //Console.WriteLine("HIC");

                for (int i = 0; i <= posY; i++)
                {
                    //Console.WriteLine(i);
                    //Console.WriteLine(BasicTriangle((BasicTriangleType)t.SlopeType)[i, posX]);

                    if (BasicTriangle(BasicTriangleType.RightDown)[i, posX] == 1)
                    {
                        //Console.WriteLine(i);
                        return new Vector2(a.GetRectangle().X, t.GetRectangle().Y + i - a.GetRectangle().Height);
                    }

                }

            }

            else if((BasicTriangleType)t.SlopeType == BasicTriangleType.LeftUp)
            {
                int posX = a.GetRectangle().X + a.GetRectangle().Width - t.GetRectangle().X;
                int posY = a.GetRectangle().Y - (t.GetRectangle().Y + t.GetRectangle().Height);

                if (posY < 0)
                    posY = -posY;

                if (posY < 0)
                    posY = 0;

                if (posX < 0) ///////  revoir !!!!!!!!!!!!!!!!!
                    posX = 0;

                if (posX > 15)
                    posX = 15;

                if (posY > 15)
                    posY = 15;

                //Console.WriteLine("HIC");

                for (int i = 0; i < posY; i++)
                {
                    //Console.WriteLine(i);
                    //Console.WriteLine(BasicTriangle((BasicTriangleType)t.SlopeType)[i, posX]);

                    if (BasicTriangle(BasicTriangleType.LeftDown)[i, posX] == 1)  /// ça marche, mais... a l'envers ///
                    {
                        //Console.WriteLine(i);
                        return new Vector2(a.GetRectangle().X, t.GetRectangle().Y + t.GetRectangle().Height - i);
                    }

                }
            }

            else if ((BasicTriangleType)t.SlopeType == BasicTriangleType.RightUp)
            {
                int posX = a.GetRectangle().X - t.GetRectangle().X;
                int posY = a.GetRectangle().Y - (t.GetRectangle().Y + t.GetRectangle().Height);

                if (posY < 0)
                    posY = -posY;

                if (posY < 0)
                    posY = 0;

                if (posX < 0) ///////  revoir !!!!!!!!!!!!!!!!!
                    posX = 0;

                if (posX > 15)
                    posX = 15;

                if (posY > 15)
                    posY = 15;

                //Console.WriteLine("HIC");

                for (int i = 0; i < posY; i++)
                {
                    //Console.WriteLine(i);
                    //Console.WriteLine(BasicTriangle((BasicTriangleType)t.SlopeType)[i, posX]);

                    if (BasicTriangle(BasicTriangleType.RightDown)[i, posX] == 1)  /// ça marche, mais... a l'envers ///
                    {
                        //Console.WriteLine(i);
                        return new Vector2(a.GetRectangle().X, t.GetRectangle().Y + t.GetRectangle().Height - i);
                    }

                }
            }

            return new Vector2(a.GetRectangle().X, a.GetRectangle().Y);
                
        }


        public enum Direction
        {
            NoCollision = 0,

            Right = 1,
            Left = 2,
            Up = 3,
            Down = 4,

            RightAndLeft = 5,
            UpAndDown = 6,

            X = 7,
            Y = 8,

        };


        public static Direction SolidVsActorDirectionFonctionnel(Actor a, Solid t, Direction XelseY = Direction.NoCollision)
        {
            if (XelseY == Direction.X)
                if (a.GetRectangle().X + a.Velocity.X < t.GetRectangle().X + t.GetRectangle().Width && a.GetRectangle().Y < t.GetRectangle().Y + t.GetRectangle().Height - a.Velocity.Y && a.GetRectangle().Y + a.GetRectangle().Height > t.GetRectangle().Y && a.OldPosition.X > t.GetRectangle().X)
                    return Direction.Left;

            if (XelseY == Direction.X)
                if (a.GetRectangle().X + a.GetRectangle().Width + a.Velocity.X > t.GetRectangle().X && a.GetRectangle().Y < t.GetRectangle().Y + t.GetRectangle().Height - a.Velocity.Y && a.GetRectangle().Y + a.GetRectangle().Height > t.GetRectangle().Y && a.OldPosition.X < t.GetRectangle().X)
                    return Direction.Right;


            if (XelseY == Direction.Y)
                if (a.GetRectangle().Y + a.Velocity.Y < t.GetRectangle().Y + t.GetRectangle().Height && a.GetRectangle().X < t.GetRectangle().X + t.GetRectangle().Width && a.GetRectangle().X + a.GetRectangle().Width > t.GetRectangle().X && a.GetRectangle().Y > t.GetRectangle().Y && a.OldPosition.Y > t.GetRectangle().Y + t.GetRectangle().Height / 2)
                    return Direction.Up;

            if (XelseY == Direction.Y)
                if (a.GetRectangle().Y + a.GetRectangle().Height + a.Velocity.Y > t.GetRectangle().Y && a.GetRectangle().X < t.GetRectangle().X + t.GetRectangle().Width && a.GetRectangle().X + a.GetRectangle().Width > t.GetRectangle().X)
                    return Direction.Down;

            return Direction.NoCollision;

        }


        public static int[,] BasicTriangle(BasicTriangleType type)
        {
            if(type == BasicTriangleType.LeftDown)
                return new int[,]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 },    
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1 },    
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1 },    //         0
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1 },    //       0 0
                { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 },    //     0 0 0
                { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },

                /*
                 * { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1 },
                 * { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1 },
                 * { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1 },
                 * { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1 },
                 * { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1 },
                 * { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1 },
                 * { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1 },
                 * { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1 },
                 * { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                 * { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                 * { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                 * { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                 * { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                 * { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                 * { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                 * { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                 */

            };

            if (type == BasicTriangleType.RightDown)   ////
                return new int[,]
            {
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },    //     0  
                { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },    //     0 0 
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },    //     0 0 0
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            };

            if (type == BasicTriangleType.RightUp)
                return new int[,]
            {
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },    //     0 0 0  
                { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },    //     0 0 
                { 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },    //     0  
                { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };

            if (type == BasicTriangleType.LeftUp)
                return new int[,]
            {
                { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 },    //     0 0 0  
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1 },    //     0 0 
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1 },    //     0  
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };

            return null;

        }


        public enum BasicTriangleType
        {
            LeftDown = 1,
            RightDown = 2,
            LeftUp = 3,
            RightUp = 4,
        };



    }

}
