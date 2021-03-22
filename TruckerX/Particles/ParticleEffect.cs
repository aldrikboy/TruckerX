using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TruckerX.Animations;
using TruckerX.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Particles
{
    public class Particle
    {
        public Texture2D Texture { get; }
        public Vector2 Position { get; set; }
        public Vector2? Direction { get; set; }
        public float Speed { get; set; }
        public TimeSpan AliveTime { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Color Color { get; set; } = Color.White;

        public Particle(Texture2D texture, Vector2 position, float speed, Vector2 origin)
        {
            Texture = texture;
            Position = position;
            Direction = null;
            Speed = speed;
            AliveTime = TimeSpan.Zero;
            Origin = origin;
        }
    }

    public abstract class ParticleEffect
    {
        private List<Texture2D> possibleTextures = new List<Texture2D>();

        protected List<Particle> Particles = new List<Particle>();

        protected TimeSpan timer = TimeSpan.Zero;
        private int particlesPerSpawn = 1;
        protected TimeSpan Interval = TimeSpan.Zero;
        private float particleSpeed;
        private TimeSpan particleLifespan;
        private Vector2 particleSize;

        private bool StopAfterNextEmit = false;
        private bool emitting = true;

        // Area where particles can spawn
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }

        protected ParticleEffect(List<Texture2D> textures, TimeSpan interval, int particlesPerSpawn, float particleSpeed, TimeSpan particleLifespan, Vector2 particleSize)
        {
            this.possibleTextures = textures;
            this.Interval = interval;
            this.particlesPerSpawn = particlesPerSpawn;
            this.particleLifespan = particleLifespan;
            this.particleSpeed = particleSpeed;
            this.particleSize = particleSize;
        }

        public void EmitOnce()
        {
            this.emitting = true;
            this.StopAfterNextEmit = true;
        }

        public void Stop()
        {
            this.emitting = false;
            this.StopAfterNextEmit = false;
        }

        public void Start()
        {
            this.emitting = true;
            this.StopAfterNextEmit = false;
        }

        private void SpawnParticle()
        {
            if (!emitting) return;
            var rand = new Random();
            int random = rand.Next(0, possibleTextures.Count);
            var particle = new Particle(possibleTextures[random], new Vector2(Position.X + rand.Next(0, (int)Size.X), Position.Y + rand.Next(0, (int)Size.Y)), particleSpeed, this.particleSize.Center());
            this.Particles.Add(particle);
        }

        public void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime;

            if (timer >= Interval)
            {
                for (int i = 0; i < particlesPerSpawn; i++) SpawnParticle();
                if (StopAfterNextEmit) this.Stop();
                timer = TimeSpan.Zero;
            }

            UpdateParticles(gameTime);

            float delta = gameTime.Delta();
            for (int i = 0; i < Particles.Count; i++)
            {
                var particle = Particles[i];

                if (particle.AliveTime >= this.particleLifespan)
                {
                    Particles.RemoveAt(i);
                    i--;
                }

                if (particle.Direction.HasValue)
                {
                    particle.AliveTime += gameTime.ElapsedGameTime;
                    particle.Position += new Vector2(particle.Direction.Value.X * delta * particle.Speed,
                        particle.Direction.Value.Y * delta * particle.Speed);
                }
            }
        }

        public void Draw(SpriteBatch batch, GameTime gameTime)
        {
            foreach (var particle in Particles)
            {
                if (particle.Direction.HasValue)
                {
                    double ms = (this.particleLifespan - particle.AliveTime).TotalMilliseconds;
                    double alpha = ms < 1000 ? ms / 1000.0 : 1.0;

                    batch.Draw(particle.Texture, new Rectangle(particle.Position.ToPoint(), this.particleSize.ToPoint()), 
                        null, particle.Color * (float)alpha, particle.Rotation, particle.Origin, SpriteEffects.None, 1);
                    // new Vector2(particle.Texture.Width/2, particle.Texture.Height/2)
                }
            }
        }

        protected abstract void UpdateParticles(GameTime gameTime);
    }
}
