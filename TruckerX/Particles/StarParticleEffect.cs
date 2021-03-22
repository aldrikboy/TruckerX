using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TruckerX.Extensions;
using TruckerX.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Particles
{
    public class StarParticleEffect : ParticleEffect
    {
        public StarParticleEffect(BaseScene scene) 
            : base(new List<Texture2D> { scene.GetTexture("star") }, TimeSpan.FromMilliseconds(150), 1, 100.0f, TimeSpan.FromSeconds(1), new Vector2(50, 50))
        {

        }

        protected override void UpdateParticles(GameTime gameTime)
        {
            foreach(var particle in Particles)
            {
                if (!particle.Direction.HasValue)
                {
                    particle.Direction = new Vector2().RandomNormalized();
                }
            }
        }
    }
}
