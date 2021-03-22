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
    public class DetailButtonWidget : BaseWidget
    {
        private Texture2D bg;
        private SpriteFont font;
        private BaseScene scene;

        public string Text { get; set; }

        public DetailButtonWidget(BaseScene scene, Vector2 position, Vector2 size) : base(position, size)
        {
            bg = scene.GetTexture("detail-button");
            font = scene.GetRDFont("main_font_15");
            this.scene = scene;
        }

        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            font = scene.GetRDFont("main_font_18");
            batch.Draw(bg, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), Color.White);

            if (this.State == WidgetState.MouseHover)
            {
                batch.Draw(bg, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), Color.FromNonPremultiplied(0, 0, 0, 50));
            }
            else if (this.State == WidgetState.MouseDown)
            {
                batch.Draw(bg, new Rectangle(this.Position.ToPoint(), this.Size.ToPoint()), Color.FromNonPremultiplied(0, 0, 0, 150));
            }

            var str = Text;
            var strSize = font.MeasureString(str);
            float angle = (float)Math.PI * 0.0f / 180.0f;
            int offsetx = (int)this.Position.X - (int)strSize.X / 2 + (int)this.Size.X / 2;
            int offsety = (int)this.Position.Y - (int)strSize.Y / 2 + (int)this.Size.Y / 2;
            batch.DrawString(font, str, new Vector2(offsetx, offsety), Color.FromNonPremultiplied(60,60,60,255), angle, new Vector2(0, 0), 1.0f, SpriteEffects.None, 1.0f);
        }
    }
}
