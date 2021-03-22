using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Animations
{
    public class LinearAnimation : Animation
    {
        public LinearAnimation(TimeSpan duration) : base(duration)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (progress < Duration) progress += gameTime.ElapsedGameTime;
            else return;

            if (progress >= Duration) AnimationFinished();
        }
    }
}
