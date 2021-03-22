using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TruckerX.Extensions;
using TruckerX.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Particles
{
    public class ConfettiParticleEffect : ParticleEffect
    {
        public ConfettiParticleEffect(BaseScene scene)
           : base(new List<Texture2D> { scene.GetTexture("white") }, TimeSpan.FromMilliseconds(150), 50, 150.0f, TimeSpan.FromSeconds(1), new Vector2(10, 10))
        {

        }

        protected override void UpdateParticles(GameTime gameTime)
        {
            foreach (var particle in Particles)
            {
                if (!particle.Direction.HasValue)
                {
                    var rand = new Random();
                    if (rand.Next(0, 4) != 0) continue;  // Make sure the confetti doesn't come out all at once.

                    particle.Rotation = (float)Math.PI * new Random().Next(0, 360) / 180.0f;
                    particle.Color = Color.FromNonPremultiplied(rand.Next(100,255), rand.Next(100, 255), rand.Next(100, 255), 255);
                    particle.Direction = new Vector2().RandomNormalized() * 2;
                }
                else
                {
                    particle.Rotation += 4.0f * gameTime.Delta();
                    particle.Direction = new Vector2(particle.Direction.Value.X, particle.Direction.Value.Y + (2.5f * gameTime.Delta()));
                }
            }
        }
    }
}
