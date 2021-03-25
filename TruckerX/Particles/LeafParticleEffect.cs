using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TruckerX.Extensions;
using TruckerX.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Particles
{
    public class LeafParticleEffect : ParticleEffect
    {
        public LeafParticleEffect(BaseScene scene)
            : base(new List<Texture2D> { ContentLoader.GetTexture("leaf") }, TimeSpan.FromMilliseconds(2000), 1, 60.0f, TimeSpan.FromSeconds(6), new Vector2(30, 30))
        {

        }

        protected override void UpdateParticles(GameTime gameTime)
        {
            foreach (var particle in Particles)
            {
                if (!particle.Direction.HasValue)
                {
                    particle.Rotation = (float)Math.PI * new Random().Next(0, 360) / 180.0f;
                    particle.Direction = new Vector2(0, 1.0f);
                }
                else
                {
                    particle.Rotation += 1.0f * gameTime.Delta();
                }
            }
        }
    }
}
