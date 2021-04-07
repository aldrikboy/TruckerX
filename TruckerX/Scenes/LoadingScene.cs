using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TruckerX.Animations;
using TruckerX.Extensions;
using TruckerX.Particles;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.State;
using TruckerX.Locations;
using Newtonsoft.Json;
using System.IO;

namespace TruckerX.Scenes
{
    public class LoadingScene : BaseScene
    {
        private Animation loadingBgMoveIn = new LinearAnimation(TimeSpan.FromSeconds(1));
        private StarParticleEffect starParticleEffect;
        private LeafParticleEffect leafParticleEffectTree1;
        private LeafParticleEffect leafParticleEffectTree2;

        private TimeSpan minimumDisplayDuration = TimeSpan.FromSeconds(0);
        private BaseScene nextScene;

        public LoadingScene()
        {
            loadingBgMoveIn.OnFinished += LoadingBgMoveIn_OnFinished;
        }

        private void LoadingBgMoveIn_OnFinished(object sender, EventArgs e)
        {
            var sample1 = ContentLoader.GetSample("pop1");
            sample1.Play(0.15f, 0.0f, 0.0f);
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            if (!ContentLoader.Done) return;

#if false
            if (MediaPlayer.PlayPosition.TotalMilliseconds == 0)
            {
                var bgSong = this.GetSong("background-song");
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = 0.01f;
                MediaPlayer.Play(bgSong);
            }
#endif

            {
                var bg = ContentLoader.GetTexture("loading-background");
                Vector2 size = bg.ScaleToWindow(1.0f);
                int offsetx = (TruckerX.WindowWidth - (int)size.X) / 2;
                int offsety = (TruckerX.WindowHeight - (int)size.Y) / 2;
                batch.Draw(bg, new Rectangle(offsetx, offsety, (int)size.X, (int)size.Y), Color.White);
            }

            {
                var trees = ContentLoader.GetTexture("trees");
                Vector2 size = trees.ScaleToWindow(1.0f);
                int offsetx = (TruckerX.WindowWidth - (int)size.X) / 2;
                int offsety = (TruckerX.WindowHeight - (int)size.Y) / 2;
                batch.Draw(trees, new Rectangle(offsetx, offsety, (int)size.X, (int)size.Y), Color.White);

                if (leafParticleEffectTree2 != null)
                {
                    leafParticleEffectTree2.Size = new Vector2(200, 200);
                    leafParticleEffectTree2.Position = new Vector2(TruckerX.WindowWidth - 200, 20);
                    leafParticleEffectTree2.Draw(batch, gameTime);
                }
            }

            {
                var logo = ContentLoader.GetTexture("sloth");
                Vector2 size = logo.ScaleToWindow(0.6f);
                int offsetx = (TruckerX.WindowWidth - (int)size.X) / 2;
                int offsety = (TruckerX.WindowHeight - (int)size.Y) / 2;
                batch.Draw(logo, new Rectangle(offsetx, offsety, (int)size.X, (int)size.Y), Color.White);
            }

            {
                if (leafParticleEffectTree1 != null)
                {
                    leafParticleEffectTree1.Size = new Vector2(200, 200);
                    leafParticleEffectTree1.Position = new Vector2(20, 20);
                    leafParticleEffectTree1.Draw(batch, gameTime);
                }
            }

            {
                var sign = ContentLoader.GetTexture("loading-bg");
                Vector2 size = sign.ScaleToWindow(0.4f);
                int offsetx = (TruckerX.WindowWidth - (int)(size.X * loadingBgMoveIn.Percentage));
                int offsety = (TruckerX.WindowHeight - (int)size.Y);
                batch.Draw(sign, new Rectangle(offsetx-4, offsety, (int)size.X, (int)size.Y), Color.FromNonPremultiplied(0,0,0,100));
                batch.Draw(sign, new Rectangle(offsetx, offsety, (int)size.X, (int)size.Y), Color.White);

                var font = ContentLoader.GetFont("main_font_24");
                string text = "LOADING";
                var strSize = font.MeasureString(text);
                float angle = (float)Math.PI * -45.0f / 180.0f;
                int margin = 45;
                offsetx = offsetx + margin * 2 + 2;
                offsety = offsetx + margin * 2;

                if (starParticleEffect != null)
                {
                    starParticleEffect.Position = new Vector2(offsetx + 60, offsety - 30);
                    starParticleEffect.Draw(batch, gameTime);
                }

                batch.DrawString(font, text, new Vector2(offsetx, TruckerX.WindowHeight - margin+2), Color.Black, angle, new Vector2(0, 0), 1.0f, SpriteEffects.None, 1.0f);
                batch.DrawString(font, text, new Vector2(offsetx, TruckerX.WindowHeight - margin), Color.White, angle, new Vector2(0, 0), 1.0f, SpriteEffects.None, 1.0f);
            }

            base.Draw(batch, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (!ContentLoader.Done) return;

            if (starParticleEffect == null) starParticleEffect = new StarParticleEffect(this);
            if (leafParticleEffectTree1 == null) leafParticleEffectTree1 = new LeafParticleEffect(this);
            if (leafParticleEffectTree2 == null) leafParticleEffectTree2 = new LeafParticleEffect(this);

            starParticleEffect?.Update(gameTime);
            leafParticleEffectTree1?.Update(gameTime);
            leafParticleEffectTree2?.Update(gameTime);
            loadingBgMoveIn.Update(gameTime);

            if (nextScene == null && ContentLoader.Done)
            {
                nextScene = new WorldMapScene();
            }
            if (nextScene != null && elapsedTime >= minimumDisplayDuration)
            {
                this.SwitchSceneTo(nextScene);
            }

            base.Update(gameTime);
        }
    }
}
