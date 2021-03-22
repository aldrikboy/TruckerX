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

namespace TruckerX.Widgets
{
    class BannerListWidget : BaseWidget
    {
        private Texture2D bg;
        private SpriteFont font;
        private DetailScene scene;

        public List<BannerWidget> Widgets { get; set; } = new List<BannerWidget>();
        private ScrollWidget scroll;
        int totalScrollArea = 0;

        public BannerListWidget(DetailScene scene, List<BannerWidget> widgets) : base(Vector2.Zero, Vector2.Zero, false)
        {
            bg = scene.GetTexture("detail-view");
            font = scene.GetRDFont("main_font_15");
            this.scene = scene;
            Widgets = widgets;

            scroll = new ScrollWidget(scene);
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            var rec = new Rectangle(this.Position.ToPoint(), this.Size.ToPoint());
#if false
            MonoGame.Primitives2D.DrawRectangle(batch, rec, Color.Red);
#endif

            var currentScissor = TruckerX.Game.GraphicsDevice.ScissorRectangle;
            TruckerX.Game.GraphicsDevice.ScissorRectangle = rec;

            foreach (var item in Widgets)
            {
                item.Draw(batch, gameTime);
            }

            scroll.Draw(batch, gameTime);

            TruckerX.Game.GraphicsDevice.ScissorRectangle = currentScissor;
        }

        public override void Update(GameTime gameTime)
        {
            var rec = new Rectangle(this.Position.ToPoint(), this.Size.ToPoint());
            var currentScissor = TruckerX.Game.GraphicsDevice.ScissorRectangle;
            TruckerX.Game.GraphicsDevice.ScissorRectangle = rec;

            base.Update(gameTime);

            int spacing = 2;
            float scrollY = (this.scroll.Percentage * (totalScrollArea - (float)this.Size.Y));
            for (int i = 0; i < Widgets.Count; i++)
            {
                var item = Widgets[i];
                item.Size = new Vector2(367, 88) * scene.GetRDMultiplier();
                item.Position = new Vector2(this.Position.X, this.Position.Y + (i * ((int)item.Size.Y + spacing) - scrollY));
                item.Update(gameTime);
            }
            totalScrollArea = 0;
            if (Widgets.Count != 0) totalScrollArea = Widgets.Count * ((int)Widgets[0].Size.Y + spacing);
            if (totalScrollArea < this.Size.Y) totalScrollArea = (int)this.Size.Y;

            scroll.Size = new Vector2(30 * scene.GetRDMultiplier(), this.Size.Y);
            scroll.Position = new Vector2(this.Position.X + this.Size.X - scroll.Size.X, this.Position.Y);
            if (totalScrollArea > scroll.Size.Y) scroll.ScrollbarTrackHeight = (int)(scroll.Size.Y * (scroll.Size.Y / totalScrollArea));
            else scroll.ScrollbarTrackHeight = (int)scroll.Size.Y;
            scroll.Update(gameTime);

            TruckerX.Game.GraphicsDevice.ScissorRectangle = currentScissor;
        }
    }
}
