using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TruckerX.Particles;
using TruckerX.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Locations;
using TruckerX.State;
using Microsoft.Xna.Framework.Input;

namespace TruckerX.Widgets
{
    public class SplitterBannerWidget : BannerWidget
    {
        private SpriteFont font;
        public EmployeeState Employee { get; set; }
        private string text;

        public SplitterBannerWidget(string text) : base()
        {
            this.Interactive = false;
            this.text = text;
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            base.Draw(batch, gameTime);

            int portraitSize = (int)(this.Size.Y * 0.8f);
            int padding = (int)((this.Size.Y - portraitSize) / 2);

            font = ContentLoader.GetRDFont("main_font_18");
            {
                // Name
                var str = text;
                var strSize = font.MeasureString(str);
                int offsetx = (int)this.Position.X + padding*2;
                int offsety = (int)(this.Position.Y + (this.Size.Y/2)-(strSize.Y/2));
                batch.DrawString(font, str, new Vector2(offsetx, offsety), Color.FromNonPremultiplied(60, 60, 60, 255));
            }
        }

        public override void Update(BaseScene scene, GameTime gameTime)
        {
            this.Size = new Vector2(367, 38) * scene.GetRDMultiplier();
            // We intentionally dont call base update here.
        }
    }
}
