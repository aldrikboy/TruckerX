using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TruckerX.Animations;
using TruckerX.Extensions;
using TruckerX.Particles;
using TruckerX.Widgets;
using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Scenes
{
    public class MenuScene : BaseScene
    {
        private LeafParticleEffect leafParticleEffectTree1;
        private LeafParticleEffect leafParticleEffectTree2;
        private ConfettiParticleEffect clickEffect;

        private MenuButtonWidget buttonCampaign;
        private MenuButtonWidget buttonFreePlay;
        private MenuButtonWidget buttonSettings;

        private SoundEffect ropeFallSoundEffect;

        private LinearAnimation buttonsEnterScreenAnimation;

        private WorldMapScene nextCampaignSelectorScene;

        public MenuScene()
        {
            leafParticleEffectTree1 = new LeafParticleEffect(this);
            leafParticleEffectTree2 = new LeafParticleEffect(this);

            buttonCampaign = new MenuButtonWidget(this, "Campaign", new Vector2(150, 100), new Vector2(300, 80));
            buttonCampaign.OnClick += ButtonStart_OnClick;

            buttonFreePlay = new MenuButtonWidget(this, "Free Play", new Vector2(150, 200), new Vector2(300, 80));
            buttonFreePlay.OnClick += ButtonStart_OnClick;

            buttonSettings = new MenuButtonWidget(this, "Settings", new Vector2(150, 300), new Vector2(300, 80));
            buttonSettings.OnClick += ButtonStart_OnClick;

            clickEffect = new ConfettiParticleEffect(this);
            clickEffect.Stop();
            this.OnDisplay += MenuScene_OnDisplay;

            buttonsEnterScreenAnimation = new LinearAnimation(TimeSpan.FromMilliseconds(500));

            nextCampaignSelectorScene = new WorldMapScene();
        }

        private void MenuScene_OnDisplay(object sender, EventArgs e)
        {
            ropeFallSoundEffect = ContentLoader.GetSample("rope-pull");
            ropeFallSoundEffect.Play(0.2f, 0.0f, 0.0f);
        }

        private void ButtonStart_OnClick(object sender, EventArgs e)
        {
            var sample1 = ContentLoader.GetSample("pop2");
            sample1.Play(0.15f, 0.0f, 0.0f);

            clickEffect.Position = Mouse.GetState().Position.ToVector2();
            clickEffect.EmitOnce();

            SwitchSceneTo(nextCampaignSelectorScene);
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
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

                if (leafParticleEffectTree1 != null)
                {
                    leafParticleEffectTree1.Size = new Vector2(200, 200);
                    leafParticleEffectTree1.Position = new Vector2(20, 20);
                    leafParticleEffectTree1.Draw(batch, gameTime);
                }
            }

            int offsetY = (int)(-300 + (300*buttonsEnterScreenAnimation.Percentage));
            var rope = ContentLoader.GetTexture("rope");
            batch.Draw(rope, new Rectangle(200, offsetY, 43, 500), Color.White);
            batch.Draw(rope, new Rectangle(400-43, offsetY, 43, 500), Color.White);

            buttonCampaign.Position = new Vector2(buttonCampaign.Position.X, 100 + offsetY);
            buttonFreePlay.Position = new Vector2(buttonFreePlay.Position.X, 200 + offsetY);
            buttonSettings.Position = new Vector2(buttonSettings.Position.X, 300 + offsetY);
            buttonCampaign.Draw(batch, gameTime);
            buttonFreePlay.Draw(batch, gameTime);
            buttonSettings.Draw(batch, gameTime);

            clickEffect.Draw(batch, gameTime);

            base.Draw(batch, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            buttonsEnterScreenAnimation.Update(gameTime);
            clickEffect.Update(gameTime);

            leafParticleEffectTree1?.Update(gameTime);
            leafParticleEffectTree2?.Update(gameTime);

            if (!this.ChangingScene)
            {
                buttonCampaign.Update(this, gameTime);
                buttonFreePlay.Update(this, gameTime);
                buttonSettings.Update(this, gameTime);
            }

            base.Update(gameTime);
        }
    }
}
