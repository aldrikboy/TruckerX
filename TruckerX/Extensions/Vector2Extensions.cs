using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2 Center(this Vector2 vec)
        {
            return new Vector2(vec.X/2, vec.Y/2);
        }

        public static Vector2 RandomNormalized(this Vector2 vec)
        {
            var rand = new Random();
            float x = (float)rand.NextDouble() * 2 - 1;
            float y = (float)rand.NextDouble() * 2 - 1;
            vec = new Vector2(x, y);
            vec.Normalize();
            return vec;
        }

        public static Vector2 FromPercentageWithOffset(this Vector2 vec, float x, float y)
        {
            return new Vector2(TruckerX.TargetRetangle.X + x * TruckerX.TargetRetangle.Width, TruckerX.TargetRetangle.Y + y * TruckerX.TargetRetangle.Height);
        }

        public static Vector2 FromPercentage(this Vector2 vec, float x, float y)
        {
            return new Vector2(x * TruckerX.TargetRetangle.Width, y * TruckerX.TargetRetangle.Height);
        }
    }
}
