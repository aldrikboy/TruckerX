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
using TruckerX.Input;

namespace TruckerX.Widgets
{
    public class ScrollWidget : BaseWidget
    {
        private Texture2D bg;

        public BasePlace Place { get; set; }
        public bool owned = false;

        public int ScrollbarTrackHeight { get; set; } = 0;

        public float Percentage { get { if (ScrollbarTrackHeight == this.Size.Y) return 0; else return scrollY / (this.Size.Y - (float)ScrollbarTrackHeight); } }
        private int scrollY = 0;

        public ScrollWidget(BaseScene scene) : base()
        {
            bg = scene.GetTexture("white");
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            batch.Draw(bg, new Rectangle((this.Position).ToPoint(), (this.Size).ToPoint()), Color.FromNonPremultiplied(160,160,160,255));
            batch.Draw(bg, new Rectangle(new Point((int)this.Position.X, (int)this.Position.Y + scrollY), new Point((int)this.Size.X, ScrollbarTrackHeight)), Color.FromNonPremultiplied(255, 173, 123, 255));
        }

        public override void Update(BaseScene scene, GameTime gameTime)
        {
            base.Update(scene, gameTime);

            var scroll = InputHelper.ScrollValue();
            var maxScroll = this.Size.Y - ScrollbarTrackHeight;
            if (scroll != 0)
            {
                scrollY -= scroll > 0 ? 10 : -10;
                if (scrollY < 0) scrollY = 0;
                if (scrollY > maxScroll) scrollY = (int)maxScroll;
            }
        }
    }
}
