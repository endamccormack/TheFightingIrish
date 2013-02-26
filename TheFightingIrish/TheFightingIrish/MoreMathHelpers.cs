using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheFightingIrish
{
    public class MoreMathHelpers
    {
        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        public static Vector2 GetDirectionVector(Vector2 start, Vector2 end)
        {
            Vector2 direction = end - start;
            return Vector2.Normalize(direction);
        }

        public static float GetRotation(Vector2 direction)
        {
            return (float)Math.Atan2((double)direction.Y, (double)direction.X);
        }
    }
}
