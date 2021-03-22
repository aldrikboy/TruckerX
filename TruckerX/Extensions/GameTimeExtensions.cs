using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Extensions
{
    public static class GameTimeExtensions
    {
        public static float Delta(this GameTime gameTime)
        {
            TimeSpan deltaTime = gameTime.ElapsedGameTime;
            float delta = deltaTime.Milliseconds;
            delta = delta / 1000;
            return delta;
        }
    }
}
