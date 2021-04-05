using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Extensions;

namespace TruckerX.Scenes
{
    public abstract class BaseScene : IScene, IDisposable
    {
        public float SceneHeightHalfRD { get { return 360.0f * GetRDMultiplier(); } }
        public float SceneWidthHalfRD { get { return 640.0f * GetRDMultiplier(); } }

        public float SceneHeightHalf { get { return 360.0f; } }
        public float SceneWidthHalf { get { return 640.0f; } }
        public float SceneHeight { get { return 720.0f; } }
        public float SceneWidth { get { return 1280.0f; } }

        private TimeSpan fadeInDuration = TimeSpan.FromMilliseconds(200);
        protected TimeSpan elapsedTime = TimeSpan.Zero;
        private BaseScene loadingScene = null;
        private TimeSpan loadingSceneSwapDelay = TimeSpan.Zero;
        public bool ChangingScene { get { return this.loadingScene != null; } }

        protected event EventHandler OnDisplay;

        public BaseScene()
        {
            this.elapsedTime = new TimeSpan(0);
        }

        public virtual void Draw(SpriteBatch batch, GameTime gameTime)
        {
            if (!ContentLoader.Done) return;
            var black = ContentLoader.GetTexture("black");
            float opacity = 1 - (float)elapsedTime.TotalMilliseconds / (float)fadeInDuration.TotalMilliseconds;

            if (this.loadingScene != null && elapsedTime >= fadeInDuration)
                opacity = ((float)loadingSceneSwapDelay.TotalMilliseconds / (float)fadeInDuration.TotalMilliseconds);

            batch.Draw(black, new Rectangle(0, 0, TruckerX.WindowWidth,
                    TruckerX.WindowHeight), Color.White*opacity);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (this.elapsedTime == TimeSpan.Zero) OnDisplay?.Invoke(this, null);
            this.elapsedTime += gameTime.ElapsedGameTime;

            if (this.loadingScene != null && elapsedTime >= fadeInDuration)
                this.loadingSceneSwapDelay += gameTime.ElapsedGameTime;

            if (this.loadingSceneSwapDelay.TotalMilliseconds >= fadeInDuration.TotalMilliseconds)
            {
                TruckerX.Game.SetScene(loadingScene);
                loadingScene.Update(gameTime);
            }
        }

        public float GetRDMultiplier()
        {
            return (TruckerX.TargetRetangle.Width / 1280.0f);
        }

        public SpriteFont GetRDFont(string identifier)
        {
            float ratio = TruckerX.TargetRetangle.Width / 1280.0f;
            var current = ContentLoader.GetFont(identifier);
            var lookingFor = current.LineSpacing * ratio;
            foreach(var font in ContentDefinition.Fonts)
            {
                if (font.Value.Asset.LineSpacing >= lookingFor) return font.Value.Asset;
            }    
            return null;
        }

        protected void SwitchSceneTo(BaseScene scene)
        {
            loadingScene = scene;
        }

        public virtual void Dispose()
        {
            // Nothing to do here.
        }
    }
}
