using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Animations
{
    public abstract class Animation
    {
        public TimeSpan Duration { get; }
        public float Percentage { get { return progress < Duration ? ((float)progress.TotalMilliseconds / (float)Duration.TotalMilliseconds) : 1.0f; } }

        protected TimeSpan progress;

        public event EventHandler OnFinished;

        public Animation(TimeSpan duration)
        {
            Duration = duration;
            progress = new TimeSpan(0);
        }

        public void Finish()
        {
            progress = Duration;
        }

        public void Reset()
        {
            progress = new TimeSpan(0);
        }

        protected void AnimationFinished()
        {
            OnFinished?.Invoke(this, null);
        }

        public abstract void Update(GameTime gameTime);
    }
}
