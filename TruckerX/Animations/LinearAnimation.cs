using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Animations
{
    public class LinearAnimation : Animation
    {
        private bool reversed = false;

        public LinearAnimation(TimeSpan duration) : base(duration)
        {

        }

        public void Reverse()
        {
            reversed = !reversed;
        }

        public override void Update(GameTime gameTime)
        {
            if (!reversed)
            {
                if (progress < Duration)
                {
                    progress += gameTime.ElapsedGameTime;
                    if (progress > Duration) progress = Duration;
                }
                else return;
            }
            else
            {
                if (progress > TimeSpan.Zero)
                {
                    progress -= gameTime.ElapsedGameTime;
                    if (progress < TimeSpan.Zero) progress = TimeSpan.Zero;
                }
                else return;
            }

            if (progress >= Duration) AnimationFinished();
        }
    }
}
