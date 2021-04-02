using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TruckerX.Particles;
using TruckerX.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using TruckerX.Locations;
using TruckerX.State;
using TruckerX.Extensions;
using Microsoft.Xna.Framework.Input;

namespace TruckerX.Widgets
{
    public class BannerWidget : BaseWidget
    {
        private Texture2D bg;
        private string placeholderText;

        public BannerWidget(string placeholderText = null) : base()
        {
            this.placeholderText = placeholderText;
            bg = ContentLoader.GetTexture("detail-view");
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            batch.Draw(bg, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), Color.White);

            if (this.State == WidgetState.MouseHover)
            {
                batch.Draw(bg, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), Color.FromNonPremultiplied(0, 0, 0, 50));
            }
            else if (this.State == WidgetState.MouseDown)
            {
                batch.Draw(bg, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), Color.FromNonPremultiplied(0, 0, 0, 150));
            }

            if (placeholderText != null)
            {
                var font = ContentLoader.GetRDFont("main_font_18");
                var str = placeholderText;
                var strSize = font.MeasureString(str);
                int offsetx = (int)this.Position.X - (int)strSize.X / 2 + (int)this.Size.X / 2;
                int offsety = (int)this.Position.Y - (int)strSize.Y / 2 + (int)this.Size.Y / 2;
                batch.DrawString(font, str, new Vector2(offsetx, offsety), Color.FromNonPremultiplied(60, 60, 60, 100));
            }
        }

        public override void Update(BaseScene scene, GameTime gameTime)
        {
            if (this.State == WidgetState.MouseHover) Helper.CursorToSet = MouseCursor.Hand;
            this.Size = new Vector2(367, 88) * scene.GetRDMultiplier();
            base.Update(scene, gameTime);
        }
    }
}
