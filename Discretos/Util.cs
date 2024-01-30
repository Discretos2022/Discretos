using System;
using System.Collections.Generic;
using System.Text;

namespace Plateform_2D_v9
{
    class Util
    {

        public static Random random = new Random();


        public static int UpperInteger(float value)
        {
            if (value == (int)value)
                return (int)value;
            if (value < 0)
                return (int)value - 1;
            if (value > 0)
                return (int)value + 1;
            if (value == 0)
                return 0;

            return 0;

        }


        public static bool IsMultiple(int num, int multiple)
        {
            if ((num / multiple) == (int)(num / multiple))
                return true;

            return false;
        }


        public static float PositiveOf(float num)
        {
            if (num < 0)
                return -num;

            return num;
        }


        public static int Clamp(int value, int min, int max)
        {

            if(min > max)
            {
                throw new ArgumentOutOfRangeException("La valeur de \"min\" est superieur  à la valeur de \"max\".");
            }



            if(value < min)
            {
                return min;
            }
            else if(value > max)
            {
                return max;
            }else
            {
                return value;
            }
        }


        public static float Clamp(float value, float min, float max)
        {

            if (min > max)
            {
                throw new ArgumentOutOfRangeException("La valeur de \"min\" est superieur  à la valeur de \"max\".");
            }



            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }
            else
            {
                return value;
            }
        }


        public static decimal Clamp(decimal value, decimal min, decimal max)
        {

            if (min > max)
            {
                throw new ArgumentOutOfRangeException("La valeur de \"min\" est superieur  à la valeur de \"max\".");
            }



            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }
            else
            {
                return value;
            }
        }


        public static float NextFloat(float min, float max)
        {
            System.Random random = new System.Random();
            double val = (random.NextDouble() * (max - min) + min);
            return (float)val;
        }


        public static int GetNumUnderPoint(float num, int numUnderPoint)
        {
            //double val = num - (Math.Round(num, numUnderPoint, MidpointRounding.ToZero) - (int)num) - (Math.Round(num, numUnderPoint + 2, MidpointRounding.ToZero) - (int)num);

            //double val = num - (int)num;

            //val = val * Math.Pow(10, numUnderPoint);

            if (numUnderPoint > 6)
                throw new ArgumentOutOfRangeException("NumUnderPoint is superior of 6");


            double val = num;

            for (int i = 0; i < numUnderPoint; i++)
            {
                val = val - (int)val;

                val *= 10;

            }


            return (int)val;
        }

    }
}
