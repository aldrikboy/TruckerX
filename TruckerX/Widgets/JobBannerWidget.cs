using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TruckerX.Particles;
using TruckerX.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Locations;
using TruckerX.State;

namespace TruckerX.Widgets
{
    public class JobBannerWidget : BannerWidget
    {
        private Texture2D portrait;
        private SpriteFont font;
        private BaseScene scene;

        public JobOffer Job { get; set; }

        public JobBannerWidget(BaseScene scene, JobOffer job) : base()
        {
            font = scene.GetRDFont("main_font_15");
            this.scene = scene;
            Job = job;

            portrait = ContentLoader.GetTexture("logo-placeholder");
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            base.Draw(batch, gameTime);

            int portraitSize = (int)(this.Size.Y * 0.8f);
            int padding = (int)((this.Size.Y - portraitSize) / 2);
            {
                // Portrait
                MonoGame.Primitives2D.FillRectangle(batch, 
                    new Rectangle((int)this.Position.X + padding, (int)this.Position.Y + padding, portraitSize, portraitSize),
                    Color.FromNonPremultiplied(60, 60, 60, 255));

                batch.Draw(portrait, new Rectangle((int)this.Position.X + padding+2, (int)this.Position.Y + padding+2, portraitSize-4, portraitSize-4), Color.White);
            }

            font = scene.GetRDFont("main_font_18");
            int nameHeight;
            {
                // Name
                var str = Job.Company;
                var strSize = font.MeasureString(str);
                int offsetx = (int)this.Position.X + padding + portraitSize + padding;
                int offsety = (int)this.Position.Y + padding;
                nameHeight = (int)strSize.Y;
                batch.DrawString(font, str, new Vector2(offsetx, offsety), Color.FromNonPremultiplied(60, 60, 60, 255));
            }

            font = scene.GetRDFont("main_font_12");
            {
                // Transport item name
                var str = Job.Item.Name;
                var strSize = font.MeasureString(str);
                int offsetx = (int)this.Position.X + padding + portraitSize + padding;
                int offsety = (int)this.Position.Y + padding + nameHeight;
                nameHeight += (int)strSize.Y;
                batch.DrawString(font, str, new Vector2(offsetx, offsety), Color.FromNonPremultiplied(80, 80, 80, 255));
            }

            {
                // Reward
                var str = "Reward: " + Job.From.Country.Currency.Sign + Job.OfferedReward.ToString() + "/Trip";
                var strSize = font.MeasureString(str);
                int offsetx = (int)this.Position.X + padding + portraitSize + padding;
                int offsety = (int)this.Position.Y + padding + nameHeight;
                nameHeight += (int)strSize.Y;
                batch.DrawString(font, str, new Vector2(offsetx, offsety), Color.FromNonPremultiplied(80, 80, 80, 255));
            }
        }
    }
}
