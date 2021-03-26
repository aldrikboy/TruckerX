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

        public BannerWidget(BaseScene scene) : base()
        {
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
        }

        public override void Update(BaseScene scene, GameTime gameTime)
        {
            if (this.State == WidgetState.MouseHover) Helper.CursorToSet = MouseCursor.Hand;
            this.Size = new Vector2(367, 88) * scene.GetRDMultiplier();
            base.Update(scene, gameTime);
        }
    }
}
