using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Animations
{
    public abstract class Animation
    {
        public TimeSpan Duration { get; }
        public double Percentage { get { return progress < Duration ? (progress.TotalMilliseconds / Duration.TotalMilliseconds) : 1.0; } }

        protected TimeSpan progress;

        public event EventHandler OnFinished;

        public Animation(TimeSpan duration)
        {
            Duration = duration;
            progress = new TimeSpan(0);
        }

        protected void AnimationFinished()
        {
            OnFinished?.Invoke(this, null);
        }

        public abstract void Update(GameTime gameTime);
    }
}
